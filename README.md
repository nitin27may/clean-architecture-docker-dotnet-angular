[![Angular Build](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/actions/workflows/angular-build.yml/badge.svg)](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/actions/workflows/angular-build.yml)
[![Api Build](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/actions/workflows/api-build.yml/badge.svg)](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/actions/workflows/api-build.yml)

# Clean Architecture Full-Stack Starter: .NET, Angular, and PostgreSQL with Docker

A production-ready boilerplate for building modern web applications using .NET 9 Web API, Angular 19, and PostgreSQL with Docker. This template implements clean architecture principles, ensuring maintainable, testable, and scalable code that can evolve with changing business requirements.



<!-- Application demo gif via HTTP -->
<p align="center">
  <img src="docs/clean-architecture-demo.gif" alt="Application Screenshot" width="80%">
  <br>
  <em>Contact Management Application with Dark Mode Support</em>
</p>

## Introduction

Are you tired of spending weeks setting up your project infrastructure before writing a single line of business logic? This starter kit solves that problem by providing:

- **Production-Ready Architecture**: Built on enterprise-grade patterns that scale
- **Modern Tech Stack**: Latest versions of .NET 9, Angular 19, and PostgreSQL
- **Developer Experience**: Hot reload, comprehensive testing setup, and Docker integration
- **Best Practices**: Authentication, authorization, logging, and error handling already implemented
- **Code Quality**: Linting, formatting, and static analysis configurations included

Whether you're building a startup MVP or an enterprise application, this template provides the solid foundation you need to focus on what matters - delivering business value.
<p align="center">
  <img src="docs/CleanArchitecture.png" alt="Clean Architecture Diagram" width="70%">
</p>
## Quick Start

To quickly start the application, clone the repository and run Docker Compose:

```bash
git clone https://github.com/nitin27may/clean-architecture-docker-dotnet-angular.git angular-dotnet
cd angular-dotnet 
```

Rename `.env.example` to `.env`:

Shell:
```bash
cp .env.example .env
```


Run all containers: 
```bash
docker-compose up
```

**Note:** The application uses SMTP for email functions. Please update account details in the `.env` file.

### Default Users

After startup, the following test users are available:
 
| Username             | Password      | Role    |
|----------------------|---------------|---------|
| nitin27may@gmail.com | P@ssword#321  | Admin   |
| editor@gmail.com     | P@ssword#321  | Editor  |
| reader@gmail.com     | P@ssword#321  | Reader  |

## Project Overview

<!-- Add dashboard/admin interface screenshot placeholder -->
<!-- <p align="center">
  <img src="docs/dashboard-screenshot.png" alt="Dashboard Screenshot" width="80%">
  <br>
  <em>Admin Dashboard with Role-Based Access Control</em>
</p> -->

This project provides a comprehensive starter template that goes beyond just connecting technologies - it demonstrates how to build a maintainable, production-quality application:

1. **Complete User Management System** - Registration with email verification, login with JWT, password reset, profile management, and role-based authorization
   
2. **Clean Domain-Driven Design** - Business logic isolated from infrastructure concerns, making it easier to adapt to changing requirements

3. **DevOps Ready** - GitHub Actions workflows, Docker configurations for both development and production, and multi-environment support

4. **Performance Optimized** - Database query optimization, frontend bundle optimization, and caching strategies implemented

5. **Enterprise Patterns** - Repository pattern, unit of work, specification pattern, and more to maintain code quality at scale

6. **Developer Productivity** - Auto-generated API clients, comprehensive test suites, and documentation to help new team members get productive quickly

### Feature Highlights

| Feature | Frontend | Backend |
|---------|----------|---------|
| **Authentication** | JWT with auto-refresh, secure storage | Token generation, validation, refresh mechanism |
| **Authorization** | Role-based UI components, route guards | Policy-based endpoints, role verification |
| **User Management** | Profile editing, password management | Secure storage, email verification |
| **Error Handling** | Global error interceptor, user-friendly messages | Exception middleware, structured error responses |
| **Form Management** | Reactive forms with validation | Model validation, error mapping |
| **API Integration** | Strongly-typed HTTP clients | OpenAPI documentation, versioning |
| **State Management** | Signal-based reactive state | Clean separation of concerns |
| **UI Components** | Material Design + TailwindCSS | N/A |
| **Data Access** | N/A | Repository pattern with Dapper |
| **Logging** | Console and error interceptor | Structured logging with Serilog |

## Architecture

![Clean Architecture Diagram](docs/CleanArchitecture.png)

### Container Architecture

![Container Architecture Diagram](docs/architecture.png)

## Key Features

### Frontend (Angular 19)
- Modern dependency injection using `inject()` function
- Signal-based state management
- Angular Material 19 with theme support
- TailwindCSS v4 for utility-first styling
- JWT authentication with auto refresh
- [More details in frontend documentation](./docs/frontend.md)

### Backend (.NET 9)
- Clean Architecture implementation
- Generic Repository and Service patterns
- Dapper with PostgreSQL for data access
- JWT authentication with role-based permissions
- [More details in backend documentation](./docs/backend.md)

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) (latest version)
- Git

## Learn More

For in-depth understanding, check out our [documentation](./docs/):
- [Clean Architecture Article Series](./docs/architecture-series.md)
- [Detailed Feature List](./docs/features.md)
- [Development Guide](./docs/development-guide.md)
- [Roadmap & Upcoming Features](./docs/roadmap.md)

## Contributing

We welcome contributions! See our [contributing guide](./CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License. See the [LICENSE file](LICENSE) for details.

## Contact

For support or questions, please contact Nitin Singh at nitin27may@gmail.com.

## Feature Requests

Have ideas to improve this project? Submit a [feature request](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/new?assignees=&labels=&projects=&template=feature_request.md&title=).