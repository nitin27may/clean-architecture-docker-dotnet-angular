# Security Policy

## Supported Versions

This is a reference architecture, not a versioned library — only the code on `main` is supported. Security fixes land there; there are no maintained release branches.

## Reporting a Vulnerability

Please do not open a public GitHub issue for a security vulnerability.

Instead, email **nitin27may@gmail.com** with:

- A description of the vulnerability and its potential impact
- Steps to reproduce it
- Affected file(s)/endpoint(s), if known

You should get a response within a few days. Once a fix is confirmed, it will be merged and you'll be credited in the commit/PR unless you prefer otherwise.

## Scope

This project is a starter/reference architecture intended to be forked and adapted. Known scope for security review:

- Backend (`backend/`) — ASP.NET Core API, JWT authentication, PostgreSQL access via Dapper
- Frontend (`frontend/`) — Angular SPA
- Container/orchestration config (`aspire/`, `docker-compose*.yml`, `loadbalancer/`)

Default credentials and secrets in `.env.example` and the seed data (`scripts/02-seed-data.sql`) are for **local development only** — never deploy this project with the default JWT secret, database password, or seeded user passwords unchanged.
