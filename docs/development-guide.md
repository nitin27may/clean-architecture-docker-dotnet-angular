---
layout: default
title: Development Guide
nav_order: 4
permalink: /development-guide
---
# Development Guide

This guide provides detailed instructions for setting up your development environment and working with the Clean Architecture Docker .NET Angular starter project.

## Prerequisites

Before you begin, ensure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop) (latest version)
- [Git](https://git-scm.com/downloads)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended) or another IDE
- [.NET 9 SDK](https://dotnet.microsoft.com/download) (for local development without Docker)
- [Node.js](https://nodejs.org/) (LTS version, for local development without Docker)

## Getting the Code

Clone the repository:

```bash
git clone https://github.com/nitin27may/clean-architecture-docker-dotnet-angular.git
cd clean-architecture-docker-dotnet-angular
```

## Development Options

You have two primary options for development:

1. **Docker-based development** (recommended)
2. Local development (separate setup for frontend and backend)

## Docker-based Development

### Environment Setup

1. Create your environment file:

   ```bash
   cp .env.example .env
   
   # (PowerShell)
   Copy-Item .env.example .env
   ```

2. Edit the `.env` file to configure environment variables as needed. Key settings include:
   - PostgreSQL connection details
   - JWT secret and configuration
   - SMTP settings for email functionality

### Starting the Development Environment

Run the development containers:

```bash
docker-compose up
```

This will start the following services:
- PostgreSQL database
- .NET API with hot reload
- Angular frontend with hot reload
- NGINX as a reverse proxy

### Development URLs

- **Frontend**: http://localhost:4200
- **API**: http://localhost:5000/swagger
- **API through NGINX**: http://localhost/api

### Rebuilding Containers

If you make changes to Dockerfiles or docker-compose.yml:

```bash
docker-compose up --build
```

### Stopping the Environment

```bash
docker-compose down
```

To remove volumes (database data will be lost):

```bash
docker-compose down -v
```

## Local Development (Alternative)

### Backend (.NET API)

1. Navigate to the backend directory:

   ```bash
   cd backend/src
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Update the PostgreSQL connection in `appsettings.Development.json`

4. Run the API:

   ```bash
   dotnet run --project Contact.Api
   ```

   The API will be available at `https://localhost:7224` and `http://localhost:5217`

### Frontend (Angular)

1. Navigate to the frontend directory:

   ```bash
   cd frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Update the proxy configuration in `proxy.conf.json` to point to your local API

4. Start the development server:

   ```bash
   npm run serve
   ```

   The frontend will be available at `http://localhost:4200`

## Database Access

### Using Docker Tools

Access the PostgreSQL database using:

```bash
docker exec -it postgres_db psql -U postgres -d contacts
```

### Using External Tools

You can also connect using external tools like pgAdmin, DBeaver, or JetBrains DataGrip with these connection details:

- **Host**: localhost
- **Port**: 5432
- **Username**: contacts_admin (or as configured in your .env file)
- **Password**: (from your .env file)
- **Database**: contacts

## Development Workflow

### Making Backend Changes

1. Modify code in the appropriate layer:
   - **Domain**: For entity definitions and core business logic
   - **Application**: For use cases, services, and orchestration
   - **Infrastructure**: For external integrations and data access
   - **API**: For endpoints and controllers

2. If using Docker, changes will be automatically applied with hot reload.

3. Follow the Clean Architecture principles:
   - Domain layer has no external dependencies
   - Application layer depends only on Domain
   - Infrastructure implements interfaces defined in Domain/Application
   - API layer is the entry point but delegates to Application services

### Making Frontend Changes

1. Follow these best practices:
   - Use standalone components
   - Use signals for state management
   - Use the inject() function for dependency injection
   - Integrate Material components with TailwindCSS utilities
   - Implement permission checks for UI elements

2. Structure new features following the established pattern:
   ```
   /feature
     /your-feature
       /components
       your-feature.routes.ts
       your-feature.service.ts
       your-feature.interface.ts
   ```

3. For state management, follow the signal pattern:
   ```typescript
   // Component state
   loading = signal<boolean>(false);
   items = signal<YourType[]>([]);
   
   // Computed values
   totalItems = computed(() => this.items().length);
   
   // Updating state
   fetchItems() {
     this.loading.set(true);
     this.service.getItems().subscribe({
       next: (data) => {
         this.items.set(data);
         this.loading.set(false);
       },
       error: (err) => {
         this.loading.set(false);
         this.notificationService.error('Failed to load items');
       }
     });
   }
   ```

## Testing

### Backend Tests

Run .NET tests:

```bash
cd backend/src
dotnet test
```

### Frontend Tests

Run Angular tests:

```bash
cd frontend
npm test
```

## Debugging

### Debugging Backend

1. **With Docker**:
   
   Configure VS Code for Docker debugging by adding this to `.vscode/launch.json`:
   ```json
   {
     "name": ".NET Core Docker Attach",
     "type": "coreclr",
     "request": "attach",
     "processId": "${command:pickRemoteProcess}",
     "pipeTransport": {
       "pipeProgram": "docker",
       "pipeArgs": ["exec", "-i", "api"],
       "debuggerPath": "/usr/local/bin/vsdbg",
       "pipeCwd": "${workspaceFolder}",
       "quoteArgs": false
     },
     "sourceFileMap": {
       "/app": "${workspaceFolder}/backend/src"
     }
   }
   ```

2. **Without Docker**:
   - Use standard .NET debugging in your IDE
   - Set breakpoints and run with F5 in VS Code or Visual Studio

### Debugging Frontend

1. **With Docker**:
   - Access the application at http://localhost:4200
   - Use browser developer tools to debug
   - Source maps are enabled for easier debugging

2. **Without Docker**:
   - Use standard Angular debugging with browser dev tools
   - For VS Code, use the "Launch Chrome" launch configuration

## Working with Permissions

The system uses a role-based permission model:

1. **Backend Permission Definition**: 
   - Permissions are stored in the database as Page-Operation combinations
   - Controllers use the `[AuthorizePermission("PageName.Operation")]` attribute

2. **Frontend Permission Checks**:
   - Routes are protected with the `PermissionGuard` 
   - UI elements use the `*hasPermission` directive
   - Example: `*hasPermission="{ page: 'Contacts', operation: 'Create' }"`

3. **Adding New Permissions**:
   - Add permission in the database seed script
   - Assign permission to roles in `RolePermissions` table
   - Use consistent naming: `{EntityName}.{Operation}`

## Common Tasks

### Adding a New Entity

1. **Create Entity in Domain Layer**:
   ```csharp
   public class YourEntity : BaseEntity
   {
       public required string Name { get; set; }
       // Other properties...
   }
   ```

2. **Create Repository Interface**:
   ```csharp
   public interface IYourEntityRepository : IGenericRepository<YourEntity>
   {
       // Additional methods if needed
   }
   ```

3. **Implement Repository**:
   ```csharp
   public class YourEntityRepository : GenericRepository<YourEntity>, IYourEntityRepository
   {
       public YourEntityRepository(IDapperHelper dapperHelper)
           : base(dapperHelper, "YourEntities")
       {
       }
   }
   ```

4. **Create DTOs**:
   ```csharp
   public class CreateYourEntityDto { /* properties */ }
   public class UpdateYourEntityDto { /* properties */ }
   public class YourEntityResponseDto { /* properties */ }
   ```

5. **Create Service Interface and Implementation**:
   ```csharp
   public interface IYourEntityService : IGenericService<YourEntity, YourEntityResponseDto, CreateYourEntityDto, UpdateYourEntityDto> { }
   
   public class YourEntityService : GenericService<YourEntity, YourEntityResponseDto, CreateYourEntityDto, UpdateYourEntityDto>, IYourEntityService
   {
       public YourEntityService(IGenericRepository<YourEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
           : base(repository, mapper, unitOfWork)
       {
       }
   }
   ```

6. **Register in Dependency Injection**:
   ```csharp
   // In ApplicationServiceCollectionExtensions.cs
   services.AddScoped<IYourEntityService, YourEntityService>();
   
   // In InfrastructureServiceCollectionExtensions.cs
   services.AddScoped<IYourEntityRepository, YourEntityRepository>();
   ```

7. **Create API Controller**:
   ```csharp
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class YourEntityController : ControllerBase
   {
       private readonly IYourEntityService _service;
       
       public YourEntityController(IYourEntityService service)
       {
           _service = service;
       }
       
       [HttpGet]
       [AuthorizePermission("YourEntity.Read")]
       public async Task<IActionResult> GetAll()
       {
           var entities = await _service.FindAll();
           return Ok(entities);
       }
       
       // Other actions...
   }
   ```

8. **Create Angular Service and Components**:
   ```typescript
   // service
   @Injectable({ providedIn: 'root' })
   export class YourEntityService {
     private http = inject(HttpClient);
     
     getAll() {
       return this.http.get<YourEntity[]>('/api/YourEntity');
     }
     
     // Other methods...
   }
   
   // component
   @Component({
     standalone: true,
     // other metadata
   })
   export class YourEntityListComponent {
     private service = inject(YourEntityService);
     entities = signal<YourEntity[]>([]);
     
     // Component logic...
   }
   ```

## Building for Production

### Backend

```bash
cd backend/src
dotnet publish -c Release
```

### Frontend

```bash
cd frontend
npm run build
```

### Using Docker

```bash
docker-compose -f docker-compose.yml up --build
```

## Troubleshooting

### Common Issues

1. **PostgreSQL Connection Issues**:
   - Check connection string in `.env` file
   - Ensure PostgreSQL container is running

2. **Frontend API Connection Issues**:
   - Verify API URL in environment settings
   - Check proxy configuration

3. **Permission Errors**:
   - Verify user has the required role and permissions
   - Check permission attribute on controller action

4. **Hot Reload Not Working**:
   - Ensure Docker volume mappings are correct
   - Check for file watching issues

### Getting Help

If you encounter issues:
1. Check the documentation
2. Review existing GitHub issues
3. Open a new issue with detailed information about your problem

## Additional Resources

- See [Backend Documentation](./backend.md) for detailed backend information
- See [Frontend Documentation](./frontend.md) for detailed frontend information
- See [Clean Architecture Series](./architecture-series.md) for architectural deep dives