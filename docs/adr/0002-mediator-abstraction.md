---
layout: default
title: "ADR 0002: Mediator abstraction"
parent: Architecture Decisions
nav_order: 2
permalink: /adr/mediator-abstraction
redirect_from:
  - /adr/0002-mediator-abstraction.html
description: "Architecture decision record: why the backend uses its own mediator abstraction instead of taking a direct MediatR dependency."
last_modified_at: 2026-08-21
priority: "0.4"
changefreq: yearly
---

# ADR 0002: In-repo mediator abstraction (no MediatR dependency)

- **Status:** Proposed — written to give [issue #32](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/32)
  a concrete spec to build against; not yet implemented.
- **Date:** 2026-08-19

## Context

#32 proposes introducing a mediator to decouple `Contact.Api` controllers from
`Contact.Application` services — controllers would send commands/queries instead of
calling service methods directly. This also addresses #18 (removing the
`Contact.Infrastructure` reference currently injected into `Contact.Api`), since a
mediator boundary is the natural place for that dependency to stop leaking upward.

The obvious implementation is [MediatR](https://github.com/LuckyPennySoftware/MediatR),
but as of v13 it is dual-licensed (RPL-1.5 / commercial) — see ADR 0001. This repo will
not take that dependency. The pattern is still worth having; it just needs an
implementation this repo can ship under MIT without qualification.

## Decision

A minimal mediator abstraction, owned in `Contact.Application`, roughly 50 lines:

```csharp
public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

`Sender : ISender` resolves the matching `IRequestHandler<,>` from the built-in DI
container by request type (reflection over the request's runtime type, one dictionary
lookup — no assembly-scanning magic beyond what `AddApplicationServices` already does for
everything else). Handlers are registered the same way services are today: explicit
`services.AddScoped<IRequestHandler<...>, ...>()` lines in
`ApplicationServiceCollectionExtensions`, not automatic scanning — this keeps the
registration list auditable, matching the rest of the file.

No pipeline behaviours (logging, validation) in the first pass. Once the boundary exists
and one controller action is converted, FluentValidation can be wired in as a `Send`
wrapper or a decorator on `ISender` — that is where most of the value in #32's proposal
actually lands, and it is a natural follow-up once there is a real handler to validate
against.

## Scope of the first PR

Per #32: one controller action, converted end to end, with existing services left
exactly as they are — the handler calls into the current service layer rather than
reimplementing its logic. No breaking changes to any other endpoint.

## Consequences

- Controllers gain a dependency on `ISender` instead of the specific application service
  interface for any action that is converted. Actions not yet converted are unaffected.
- `Contact.Api` no longer needs `Contact.Application`'s concrete service types for
  converted actions, which is the actual fix for #18 — but #18 stays open until the
  conversion covers whichever endpoint currently forces that reference.
- If the pattern proves out on one action, the natural next step is converting the rest
  of a single controller (not the whole API in one PR) to keep the diff reviewable.
