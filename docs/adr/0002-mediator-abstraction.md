---
layout: default
title: "ADR 0002: Mediator abstraction"
parent: Architecture Decisions
nav_order: 2
permalink: /adr/mediator-abstraction
redirect_from:
  - /adr/0002-mediator-abstraction.html
description: "Architecture decision record: why the backend uses its own mediator abstraction instead of taking a direct MediatR dependency."
last_modified_at: 2026-11-21
priority: "0.4"
changefreq: yearly
---

# ADR 0002: In-repo mediator abstraction (no MediatR dependency)

- **Status:** Accepted. Implemented for `ContactPersonController`; other controllers are being
  converted one at a time ([#32](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/32)).
- **Date:** 2026-08-19, revised 2026-11-21 (implementation)

## Context

[#32](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/32) proposes a
mediator to decouple `Contact.Api` controllers from `Contact.Application` services: controllers
send commands and queries instead of calling service methods directly.

The obvious implementation is [MediatR](https://github.com/LuckyPennySoftware/MediatR), but from
v13 it is dual-licensed (RPL-1.5 / commercial); see ADR 0001. This repo is meant to be copied into
other projects under MIT, so it does not take that dependency. The pattern is still worth having;
it just needs an implementation the repo can ship under MIT without qualification.

Two facts found during implementation shaped the decision:

- **Request validation never ran.** `CreateContactPersonValidator` and `RegisterUserValidator` were
  registered with DI, but nothing invoked them (the old `FluentValidation.AspNetCore` integration
  was never switched on). A mediator that only forwards to services adds a layer and buys nothing;
  running validation for every request is where the pattern pays for itself.
- **This does not change the API's reference to Infrastructure.** An earlier draft of this ADR
  claimed the mediator would resolve
  [#18](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/18). It
  cannot: `Contact.Api` is the composition root and must reference Infrastructure to register it,
  however controllers dispatch. #18 was closed on that basis.

## Decision

A minimal mediator owned by `Contact.Application`, in `Abstractions/Messaging`:

```csharp
public interface IRequest<TResponse>;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

- **Dispatch.** `Sender` bridges the request's runtime type into a closed generic dispatcher once
  per request type and caches it. After the first call, sending is a dictionary lookup plus a
  virtual call, with no reflection per request. A missing handler throws
  `InvalidOperationException` naming the request type.
- **Validation.** Before resolving the handler, `Sender` runs every `IValidator<TRequest>` in DI
  and throws FluentValidation's `ValidationException` with all failures. `ExceptionMiddleware`
  maps it to 400 with per-field errors. This is built in rather than a general pipeline-behaviour
  mechanism: validation is the only cross-cutting concern that needs it today (logging and
  authorization are already attributes). Add behaviours when a second concern needs one.
- **Registration by assembly scan.** `AddRequestHandlers(assembly)` registers `ISender` and every
  concrete handler as scoped. *This reverses the original draft*, which asked for one explicit
  `AddScoped` line per handler so the list stayed auditable. With one handler per controller action
  that list reaches dozens of lines, and forgetting one only fails at runtime. A test in
  `Contact.Application.Tests` asserts that every request type has exactly one handler, which keeps
  the list checked without anyone maintaining it by hand.
- **Request types are the existing use case inputs.** `CreateContactPerson` implements
  `IRequest<ContactPersonResponse>` directly, so request bodies and the OpenAPI document do not
  change. Handlers call the existing services; service logic is not moved in this step.

## Consequences

- Converted actions depend on `ISender`, not the application service. Unconverted actions are
  unchanged, so a controller can be migrated one action at a time.
- Validation applies only to requests that go through `ISender`. An endpoint that still calls its
  service directly skips its validator, as before.
- Invalid input to a converted endpoint now returns 400 instead of reaching the database. Clients
  that relied on the database to reject input see a different error.
- The backend gained its first test project, `Contact.Application.Tests` (xUnit v3), which
  `ci.yml` runs.
- Next steps: `UsersController` (which brings `RegisterUserValidator` into effect), then the
  remaining controllers, one per PR.
