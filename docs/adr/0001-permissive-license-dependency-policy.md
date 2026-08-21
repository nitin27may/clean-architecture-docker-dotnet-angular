---
layout: default
title: "ADR 0001: Permissive-licence dependency policy"
parent: Architecture Decisions
nav_order: 1
permalink: /adr/permissive-license-dependency-policy
redirect_from:
  - /adr/0001-permissive-license-dependency-policy.html
description: "Architecture decision record: why this repository only takes dependencies published under permissive licences."
last_modified_at: 2026-08-21
priority: "0.4"
changefreq: yearly
---

# ADR 0001: Permissive-license-only dependency policy

- **Status:** Accepted
- **Date:** 2026-08-19

## Context

This repository is MIT-licensed and exists to be copied wholesale into other people's
projects — that is its entire purpose as a reference architecture. Anyone who forks it
inherits every dependency choice made here, without necessarily reading the fine print
on each one.

Through 2025 and into 2026, several widely-used .NET libraries relicensed from a fully
permissive open-source license to a dual license: a copyleft license (typically the
Reciprocal Public License 1.5) for community use, plus a paid commercial tier.
[AutoMapper](https://www.jimmybogard.com/automapper-and-mediatr-commercial-editions-launch-today/)
and [MediatR](https://github.com/LuckyPennySoftware/MediatR/discussions/1105) — both
maintained by Jimmy Bogard's Lucky Penny Software — made this change together on
2025-07-02. AutoMapper 15.0.0 was the first commercial release (14.0.0 was the last MIT
release); MediatR 13 was the equivalent cutover (12.x remains Apache 2.0).

This repository was, until this ADR, shipping **AutoMapper 15.1.0** — a commercial
release — under an MIT license, with no notice to consumers. RPL-1.5 carries reciprocal
obligations; a free Community tier exists for organizations under $5M revenue, but it
requires registering for a license key. Neither the obligation nor the registration
requirement was visible to anyone who forked this repo.

Separately, [issue #32](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/32)
proposed adopting MediatR to decouple controllers from application services — a good
architectural idea that would have reintroduced the same problem with a second library.

## Decision

This repository will only take a dependency on libraries under a permissive license
(MIT, Apache 2.0, BSD) with no commercial tier, no reciprocal/copyleft obligation, and no
registration requirement. Where a library the project previously relied on moves to a
dual-license model, it will be replaced rather than pinned to an old version or paid for.

Concretely:

- **AutoMapper → [Mapperly](https://github.com/riok/mapperly) (MIT).** Mapperly is a
  Roslyn source generator: mappings are generated at compile time into partial classes
  instead of resolved by reflection at runtime. This has a real side effect beyond the
  license fix — a mapping that references a property that doesn't exist, or a `required`
  member nothing supplies a value for, is now a **build error** instead of a value that
  silently comes back `null`. See `Mapping/ObjectMapper.cs` and the `[Mapper]` classes
  next to it for how the previous `IMapper.Map<TDestination>(object)` call shape was
  preserved so no call site outside the mapping layer had to change.

- **MediatR → an in-repo abstraction, not a NuGet package.** The pattern requested in
  #32 (decoupling controllers from services via commands/queries) is worth adopting; the
  library is not, for this repo specifically. See ADR 0002 for the shape agreed with the
  issue's author.

This does **not** mean MediatR or a pre-15 AutoMapper are bad choices in general — both
remain reasonable, well-maintained libraries in a private codebase where the license
terms are a deliberate, informed choice made once by the team that owns the code. The
constraint here is specific to this repo's role: it is copied, not just referenced, so a
license decision made here becomes every fork's decision without their consent.

## Consequences

- Contributors proposing a new dependency should check its license before opening a PR.
  Dual-licensed (RPL, BUSL, etc.) and source-available-but-not-OSI-approved licenses are
  out, regardless of how good the free tier looks today — free tiers are a business
  decision the vendor can revise.
- `Contact.Application/Mapping/` is now the mapping layer. Adding a new entity/DTO pair
  means adding a partial method to the relevant `[Mapper]` class and a registration line
  in `ObjectMapper`'s dispatch dictionary — see the existing entries for the pattern.
- One known gap surfaced by this migration, not fixed here: `PermissionMapper.ToPermission(UpdatePermission, ...)`
  takes a `description` placeholder because `UpdatePermission` never carried a
  `Description`, and unlike the audit fields, `Description` is **not** excluded from the
  generated UPDATE statement — a placeholder would blank it in the database if that code
  path (the generic `IGenericService<...>.Update`, not `PermissionService`'s own
  hand-written `UpdatePermission` method, which never used the mapper) is ever actually
  invoked. This gap existed under AutoMapper too — it just failed silently instead of at
  compile time. Tracked for a follow-up rather than fixed here to keep this change scoped
  to the license/library swap.
