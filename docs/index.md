---
layout: default
title: Home
nav_order: 1
description: "Clean Architecture Full-Stack documentation"
permalink: /
---

# Clean Architecture Full-Stack Starter
{: .fs-9 }

A production-ready full-stack application with Angular 19, .NET 9, and PostgreSQL using Clean Architecture principles
{: .fs-6 .fw-300 }

[Get Started](#-quick-start-in-60-seconds){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 }
[View on GitHub](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular){: .btn .fs-5 .mb-4 .mb-md-0 }
[Feature List](visual-feature-guide.md){: .btn .btn-purple .fs-5 .mb-4 .mb-md-0 .ml-2 }

---



<p align="center">
  <a href="screenshots/clean-architecture-demo.gif" target="_blank">
    <img src="screenshots/clean-architecture-demo.gif" alt="Application Demo" width="700">
  </a>
</p>

A modern, full-stack contact management system built with Angular 19, .NET 9, and PostgreSQL following Clean Architecture principles. This project demonstrates how to structure enterprise applications for maintainability, testability, and scalability while providing a complete development workflow with Docker containerization.

## 🌟 What You'll Learn

- **Clean Architecture** principles and implementation
- **Angular 19** with signals, standalone components, and Material Design
- **.NET 9** with dependency injection and repository pattern
- **PostgreSQL** with Dapper for efficient data access
- **JWT Authentication** with role-based permissions
- **Docker** containerization for development and production
- **NGINX** as a reverse proxy and API gateway
- **CI/CD** with GitHub Actions

## 🚀 Quick Start in 60 Seconds

### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop){:target="_blank"} and Docker Compose
- [Git](https://git-scm.com/downloads){:target="_blank"}

### Launch Commands

Clone the repository:
```bash
git clone https://github.com/nitin27may/clean-architecture-docker-dotnet-angular.git clean-app
cd clean-app
```

Create environment file:
```bash
cp .env.example .env
```

Start the application:
```bash
docker-compose up
```

That's it! Visit [http://localhost](http://localhost) in your browser.

## 👤 Default Users

| Username | Password | Role |
|----------|----------|------|
| nitin27may@gmail.com | P@ssword#321 | Admin |
| editor@gmail.com | P@ssword#321 | Editor |
| reader@gmail.com | P@ssword#321 | Reader |

## 🏗️ System Architecture

<p align="center">
  <a href="screenshots/architecture.png" target="_blank">
    <img src="screenshots/architecture.png" alt="Architecture Diagram" width="700">
  </a>
</p>

### Container Architecture

The application is structured into multiple containers that work together:

- **Frontend Container**: Angular 19 with Material Design and TailwindCSS
- **API Container**: .NET 9 RESTful API built with Clean Architecture
- **Database Container**: PostgreSQL for data persistence
- **NGINX Container**: Reverse proxy that routes requests to the appropriate service

## 📐 Clean Architecture Explained

<p align="center">
  <a href="screenshots/clean-architecture.png" target="_blank">
    <img src="screenshots/clean-architecture.png" alt="Clean Architecture Diagram" width="500">
  </a>
</p>

### Why Choose Clean Architecture?

Clean Architecture provides **significant benefits** for your application:

- ✅ **Maintainability**: Separate concerns to make your code easier to understand and modify
- ✅ **Testability**: Independent components that can be tested in isolation  
- ✅ **Flexibility**: Swap frameworks or technologies without rewriting your core business logic
- ✅ **Scalability**: Grow your application with a clear structure that new team members can quickly understand

### Core Principles

- **Separation of concerns**: Each layer has a specific responsibility
- **Dependency rule**: Dependencies point inward, with inner circles having no knowledge of outer circles
- **Abstraction**: Business rules are independent of UI, database, and external services
- **Testability**: Core business logic can be tested without dependencies on external systems

## 💻 Key Features

### Modern Angular Frontend

- **Signals-based state management**
- **Material Design with TailwindCSS** for responsive UI
- **Role-based routing and permissions**
- **Dark/light theme support**

### Secure .NET Backend

- **Clean Architecture implementation**
- **Generic Repository pattern**
- **JWT authentication**
- **Global exception handling**

### Contact Management

- **CRUD operations** for contacts
- **Role-based access control**
- **Search, sort, and filter functionality**
- **Form validation**

## 📚 Documentation

For more detailed information, explore these documentation pages:

- [Development Guide](development-guide.md)
- [Clean Architecture Series](architecture-series.md)
- [Frontend Documentation](frontend.md)
- [Backend Documentation](backend.md)
- [Feature List](visual-feature-guide.md)
- [Roadmap](roadmap.md)
- [Visual Feature Guide](visual-feature-guide.md)

## 🤝 Contributing

We welcome contributions! Please check our [contributing guide](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/blob/main/CONTRIBUTING.md){:target="_blank"} for details on how to get involved.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/blob/main/LICENSE){:target="_blank"} file for details.
