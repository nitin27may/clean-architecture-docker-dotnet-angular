# Clean Architecture Docker Angular Template

This template provides a starting point for creating a Clean Architecture solution with Docker and Angular. Follow the instructions below to use the template.

## Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for Angular)
- [Docker](https://www.docker.com/get-started)

## Getting Started

1. **Install the template:**

   ```bash
   dotnet new install <path-to-template-package>
   # Or from NuGet
   dotnet new install CleanArchitecture.FullStack.Template
   ```

2. **Create a new project using the template:**

   ```bash
   dotnet new cleanarch --organization YourCompany --projectName MyApp
   ```

3. **Navigate to the project directory:**

   ```bash
   cd MyApp
   ```

4. **Build and run the project:**

   ```bash
   docker-compose up
   ```

## Template Parameters

- `--organization`: Optional organization or company name to be used in namespaces. If specified, namespaces will be formatted as `YourCompany.MyApp.Feature`. If not specified, namespaces will be formatted as `MyApp.Feature`.
- `--projectName`: The name of the project. Default is `MyApp`.

## Directory Structure

The template creates the following directory structure:

```
MyApp/
├── backend/
│   ├── src/
│   │   ├── YourCompany.MyApp.Api/
│   │   ├── YourCompany.MyApp.Core/
│   │   ├── YourCompany.MyApp.Infrastructure/
│   │   └── ...
│   ├── tests/
│   │   ├── YourCompany.MyApp.UnitTests/
│   │   ├── YourCompany.MyApp.IntegrationTests/
│   │   └── ...
│   └── ...
├── frontend/
│   ├── src/
│   ├── e2e/
│   └── ...
├── docker-compose.yml
├── .env-example
└── README.md
```

## Customizing the Template

You can customize the template by modifying the source files to meet your specific requirements. The template follows Clean Architecture principles, with proper separation of concerns between API, Core, and Infrastructure layers.

## License

This project is licensed under the MIT License. See the [LICENSE](../LICENSE) file for details.
