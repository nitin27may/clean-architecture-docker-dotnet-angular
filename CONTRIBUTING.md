# Contributing

Thanks for considering a contribution to this project. This is a personal reference architecture, so keep contributions focused and in line with the goals below — the README lays out what the project is trying to demonstrate.

## Before you start

- Check [open issues](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues) and [pull requests](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/pulls) to avoid duplicate work.
- For anything beyond a small fix, open an issue first describing what you want to change and why. This saves you from doing work that doesn't fit the project's direction.
- Read the [ADRs](docs/adr/) before proposing a new dependency or an architectural change — several decisions (e.g. no commercially-licensed dependencies, see [ADR 0001](docs/adr/0001-permissive-license-dependency-policy.md)) are deliberate and documented.

## Development setup

See the [Development Guide](docs/development-guide.md) for full environment setup (Aspire, Docker, or local). Quick start:

```bash
git clone https://github.com/nitin27may/clean-architecture-docker-dotnet-angular.git
cd clean-architecture-docker-dotnet-angular
cd frontend && npm install && cd ..
dotnet run --project aspire/AppHost
```

## Branch naming

This repo uses prefix-based branch names describing the change type:

- `feature/<short-description>` — new functionality or a version upgrade
- `fix/<short-description>` — bug fixes
- `docs/<short-description>` — documentation-only changes

## Making a change

1. Fork the repo and create a branch off `main` using the naming convention above.
2. Make your change. Keep it scoped — a bug fix shouldn't also refactor unrelated code.
3. Run the checks below before opening a PR.
4. Open a pull request against `main` with a clear description of what changed and why. Link the issue it addresses if there is one.

## Running the checks

**Backend:**

```bash
dotnet build Contact.Api.sln -c Release
dotnet test
```

**Frontend:**

```bash
cd frontend
npm run build
npm test
```

CI runs `angular-build.yml` and `api-build.yml` on every push to `main`; a PR is expected to build cleanly before it's merged. There is currently no CI job for `dotnet test`/`npm test` — please run them locally.

## Code style

- **Backend**: follow the existing Clean Architecture layering (`Contact.Api` → `Contact.Application` → `Contact.Domain` ← `Contact.Infrastructure`). Don't introduce a dependency from `Contact.Domain` on anything outside it.
- **Frontend**: standalone components, signals for state, `inject()` over constructor injection — match what's already in `frontend/src/app`.
- **Dependencies**: MIT/Apache 2.0/BSD only — no commercially-licensed or reciprocal-licensed (RPL, BUSL, etc.) packages. See [ADR 0001](docs/adr/0001-permissive-license-dependency-policy.md) for why.

## Reporting bugs

Open an issue with:
- What you did
- What you expected to happen
- What actually happened
- .NET/Node versions and OS, if relevant

## Security issues

Please don't open a public issue for a security vulnerability — see [SECURITY.md](SECURITY.md) for how to report one.
