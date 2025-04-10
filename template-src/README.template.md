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
   # Create with organization - output folder will be "YourCompany.MyApp"
   dotnet new cleanarch-fullstack --Organization YourCompany --ProjectName MyApp

   # Create without organization - output folder will be just "MyApp"
   dotnet new cleanarch-fullstack --ProjectName MyApp

   # Create in a specific folder (overrides default folder behavior)
   dotnet new cleanarch-fullstack --Organization YourCompany --ProjectName MyApp -o CustomFolder
   ```

3. **Navigate to the project directory:**

   ```bash
   # If created with organization
   cd YourCompany.MyApp
   
   # If created without organization
   cd MyApp
   
   # If created with custom output folder
   cd CustomFolder
   ```

4. **Build and run the project:**

   ```bash
   docker-compose up
   ```

## Command Examples for Different Scenarios

### Basic Usage

```bash
# Full-stack application with organization name
dotnet new cleanarch-fullstack --Organization Acme --ProjectName ECommerce

# Full-stack application without organization name
dotnet new cleanarch-fullstack --ProjectName ECommerce
```

### Backend-only Application

```bash
# Create backend-only application (exclude Angular)
dotnet new cleanarch-fullstack --Organization Acme --ProjectName ApiService --includeAngular false

# Backend-only application without organization name
dotnet new cleanarch-fullstack --ProjectName ApiService --includeAngular false
```

### Custom Output Location

```bash
# Specify a custom output folder
dotnet new cleanarch --Organization Acme --ProjectName ECommerce -o ./projects/ecommerce-app

# Specify output folder without organization name
dotnet new cleanarch --ProjectName ECommerce -o ./projects/ecommerce-app
```

### Combining Options

```bash
# Backend-only application with custom output folder
dotnet new cleanarch --Organization Acme --ProjectName ApiService --includeAngular false -o ./apis/service

# Full options example
dotnet new cleanarch --Organization Acme --ProjectName ECommerce --includeAngular true -o ./projects/ecommerce-app
```

## Template Parameters

- `--Organization`: Optional organization or company name to be used in namespaces. If specified, namespaces will be formatted as `YourCompany.MyApp.Feature`. If not specified, namespaces will be formatted as `MyApp.Feature`.
- `--ProjectName`: The name of the project. Default is `MyApp`.
- `--includeAngular`: Boolean flag to indicate whether to include the Angular frontend project. Default is `true`.
- `-o|--output`: Optional parameter to specify a custom output folder. If not specified, the output folder will be `<Organization>.<ProjectName>` if organization is provided, or just `<ProjectName>` if it isn't.

## Output Folder Naming

The template uses the following rules to determine the output folder name:

| Command Parameters | Output Folder |
| ------------------ | ------------- |
| `--Organization YourCompany --ProjectName MyApp` | `YourCompany.MyApp` |
| `--ProjectName MyApp` (no organization) | `MyApp` |
| `--Organization YourCompany --ProjectName MyApp -o CustomFolder` | `CustomFolder` |

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
