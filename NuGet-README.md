# Clean Architecture Full-Stack Template

A comprehensive template for building full-stack applications using Clean Architecture principles with a .NET 9 API backend and Angular 19 frontend, containerized with Docker.

## Features

- **Clean Architecture**: Well-organized solution following Clean Architecture principles
- **.NET 9 Backend**: Modern API built with the latest .NET 9 features
- **Angular 19 Frontend**: Responsive UI built with Angular 19
- **Docker Support**: Ready-to-use Docker configuration for development and deployment
- **PostgreSQL**: Configured to use PostgreSQL database
- **Modern Development Patterns**: 
  - Generic Repositories
  - Dependency Injection
  - Entity Framework Core
  - Angular Material UI components
  - TailwindCSS for styling

## Installation

```bash
dotnet new install CleanArchitecture.FullStack.Template
```

## Creating a New Project

```bash
dotnet new cleanarch-fullstack --ProjectName YourProjectName
```

### Template Parameters

| Parameter | Description | Default Value | Required |
|-----------|-------------|---------------|----------|
| --ProjectName | Name of your project and solution | MyCleanApp | Yes |
| --IncludeAngular | Include the Angular frontend | true | No |
| --SkipRestore | Skip automatic package restore | false | No |
| --Framework | Target framework | net9.0 | No |

## Project Structure

```
YourProjectName/
├── backend/
│   ├── src/
│   │   ├── YourProjectName.API           # API project
│   │   ├── YourProjectName.Application   # Application layer
│   │   ├── YourProjectName.Domain        # Domain layer
│   │   └── YourProjectName.Infrastructure # Infrastructure layer
│   └── tests/                            # Test projects
├── frontend/
│   ├── src/
│   │   ├── app/                          # Angular application code
│   │   ├── assets/                       # Static assets
│   │   └── environments/                 # Environment configurations
│   └── ...
├── docker-compose.yml                    # Docker composition
└── README.md                             # Project README
```

## Getting Started

### Running the Backend

```bash
cd YourProjectName/backend
dotnet restore
dotnet build
dotnet run --project src/YourProjectName.API
```

### Running the Frontend

```bash
cd YourProjectName/frontend
npm install
npm start
```

### Running with Docker

```bash
cd YourProjectName
docker-compose up
```

## Additional Resources

- [GitHub Repository](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular)
- [Clean Architecture Principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## License

This template is licensed under the MIT License - see the LICENSE file for details.
