---
layout: default
title: Backend Guide
nav_order: 7
permalink: /backend
---
# Backend Documentation

## Overview

The backend of this project is built with .NET 9, following Clean Architecture principles. It provides a robust API layer for the Angular frontend, using Dapper for data access and PostgreSQL as the database.

## Technology Stack

- **.NET 9**
  - ASP.NET Core Web API
  - Minimal API approach where applicable
  - Dependency Injection system
  - Middleware pipeline
  
- **Data Access**
  - Dapper for efficient data access
  - PostgreSQL database
  - Generic Repository pattern
  - Unit of Work for transaction management
  
- **Authentication & Authorization**
  - JWT token authentication
  - Role-based authorization
  - Policy-based access control
  - Password hashing and security

- **Validation & Mapping**
  - FluentValidation for request validation
  - AutoMapper for object mapping
  - DTO pattern for data transfer

## Clean Architecture Layers

The backend follows Clean Architecture with four distinct layers:

### 1. Domain Layer

The core of the application, containing:
- Business entities
- Value objects
- Domain events
- Domain exceptions
- Domain interfaces

This layer has no dependencies on other layers or external frameworks.

### 2. Application Layer

Contains the business logic and orchestrates the domain objects:
- Application services
- Command and query handlers
- DTO (Data Transfer Objects)
- Interfaces for infrastructure services
- Validation logic

### 3. Infrastructure Layer

Implements interfaces defined in the domain and application layers:
- Database access with Dapper and PostgreSQL
- External service integrations
- File storage implementations
- Email service implementation
- Logging implementations

### 4. API Layer

The entry point to the application:
- API controllers and endpoints
- Request/response models
- API-specific mappings
- Middleware configuration
- Authentication setup
- Swagger/OpenAPI documentation

## Key Design Patterns

### Generic Repository Pattern

The solution implements a generic repository pattern for data access:

```csharp
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> Add(T item, IDbTransaction? transaction = null);
    Task<bool> Delete(Guid id);
    Task<T> Update(T item, IDbTransaction? transaction = null);
    Task<T> FindByID(Guid id);
    Task<IEnumerable<T>> Find(string query, object? parameters = null);
    Task<IEnumerable<T>> FindAll();
}

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly IDapperHelper _dapperHelper;
    protected readonly string _tableName;

    public GenericRepository(IDapperHelper dapperHelper, string tableName)
    {
        _dapperHelper = dapperHelper;
        _tableName = tableName;
    }

    // Implementation methods...
}
```

### PostgreSQL Integration

The application uses Npgsql for PostgreSQL database access:

```csharp
public class DapperHelper : IDapperHelper
{
    private readonly AppSettings myConfig;
    private readonly ILogger _logger;

    public DapperHelper(IOptions<AppSettings> myConfigValue, ILogger<DapperHelper> logger)
    {
        myConfig = myConfigValue.Value;
        _logger = logger;
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(myConfig.ConnectionStrings.DefaultConnection);
    }

    // Implementation methods...
}
```

### Generic Service Pattern

Services follow a generic pattern for common operations:

```csharp
public class GenericService<TEntity, TResponse, TCreate, TUpdate>
        : IGenericService<TEntity, TResponse, TCreate, TUpdate>
        where TEntity : BaseEntity
        where TResponse : class
        where TCreate : class
        where TUpdate : class
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    // Implementation methods...
}
```

### Unit of Work Pattern

For transaction management:

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly IDapperHelper _dapperHelper;
    private IDbTransaction _transaction;
    private IDbConnection _connection;

    public UnitOfWork(IDapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
        _connection = _dapperHelper.GetConnection();
    }

    public IDbTransaction BeginTransaction()
    {
        if (_connection.State == ConnectionState.Closed)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
        return _transaction;
    }
    
    // Implementation methods...
}
```

## Authentication System

JWT-based authentication with:

```csharp
public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
{
    var user = await _userRepository.FindByUserName(authenticateRequest.Username);
    if (user == null)
    {
        var failedResponse = new AuthenticateResponse()
        {
            Authenticate = false
        };
        return failedResponse;
    }
    
    var passwordHasher = new PasswordHasher<string>();
    var result = passwordHasher.VerifyHashedPassword(
        user.Email, 
        user.Password, 
        authenticateRequest.Password
    );
    
    var token = await GenerateJwtToken(user);
    
    // Create and return response...
}
```

## Role-Based Authorization

The backend implements a flexible permission system:

```csharp
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var _rolePermissionService = scope.ServiceProvider.GetRequiredService<IRolePermissionService>();

            if (context.User.Identity.IsAuthenticated)
            {
                var userId = _userService.GetUserId(context.User);
                var roles = await _userService.GetUserRolesAsync(context.User);

                var rolePermissionMappings = await _rolePermissionService.GetRolePermissionMappingsAsync();
                var userPermissions = rolePermissionMappings
                    .Where(rpm => roles.Contains(rpm.RoleName))
                    .Select(rpm => $"{rpm.PageName}.{rpm.OperationName}Policy");

                if (userPermissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
```

## API Features

### Global Error Handling

Centralized error handling with middleware:

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Custom exception handling logic...
    }
}
```

### Activity Logging

Comprehensive activity tracking:

```csharp
public class ActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityLoggingMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ActivityLoggingMiddleware(
        RequestDelegate next, 
        ILogger<ActivityLoggingMiddleware> logger, 
        IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var activityAttribute = endpoint?.Metadata.GetMetadata<ActivityLogAttribute>();

        if (activityAttribute != null)
        {
            // Log activity...
        }

        await _next(context);
    }
}
```

### Contact Management API

Complete CRUD operations:
- `GET /api/ContactPerson` - List all contacts
- `GET /api/ContactPerson/{id}` - Get contact by ID
- `POST /api/ContactPerson` - Create new contact
- `PUT /api/ContactPerson/{id}` - Update contact
- `DELETE /api/ContactPerson/{id}` - Delete contact

Each endpoint is protected with appropriate permissions:

```csharp
[HttpPost]
[ActivityLog("Creating new Contact")]
[AuthorizePermission("Contacts.Create")]
public async Task<IActionResult> Add(CreateContactPerson createContactPerson)
{
    var createdContactPerson = await _contactPersonService.Add(createContactPerson);
    return CreatedAtAction(nameof(GetById), new { id = createdContactPerson.Id }, createdContactPerson);
}
```

## Data Seeding

The application includes seed data for essential entities:

- Users with roles (Admin, Editor, Reader)
- Permissions for different operations
- Sample contacts

Seed data is implemented in SQL scripts that run on container initialization:

```sql
-- Insert roles
INSERT INTO "Roles" ("Id", "Name", "Description", "CreatedOn", "CreatedBy") VALUES
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'Admin', 'This is Admin Role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('3a07551f-7473-44a6-a664-e6c7c834902b', 'Reader', 'This is a Reader role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('104102f5-e0ec-4739-8fda-f05552b677c3', 'Editor', 'This is editor role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Insert sample contacts
INSERT INTO "Contacts" ("Id", "FirstName", "LastName", "DateOfBirth", "CountryCode", "Mobile", "Email", "City", "PostalCode", "CreatedOn", "CreatedBy") VALUES
(uuid_generate_v4(), 'John', 'Smith', '1985-03-15', 1, 4155552671, 'john.smith@email.com', 'San Francisco', '94105', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- More contact records...
```

## Docker Integration

The backend is containerized with two Docker configurations:

1. **Production Build**
   - Multi-stage build for minimal image size
   - PostgreSQL database container
   - Environment variable configuration

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_URLS=http://+:8000
RUN groupadd -g 2000 dotnet \
    && useradd -m -u 2000 -g 2000 dotnet
USER dotnet

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG DOTNET_SKIP_POLICY_LOADING=true
WORKDIR /src
# Copy and restore projects...

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Contact.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contact.Api.dll"]
```

2. **Development Build**
   - Configured for active development
   - Hot reload support with `dotnet watch`
   - Debugging capabilities

## Project Structure

```
/backend/src
├── Contact.Api                # API Layer
│   ├── Controllers/           # API endpoints
│   ├── Core/                  # Core API functionality
│   │   ├── Attributes/        # Custom attributes
│   │   ├── Authorization/     # Authorization logic
│   │   └── Middleware/        # API middleware
│   └── Program.cs             # Application entry point
├── Contact.Application        # Application Layer
│   ├── Interfaces/            # Application interfaces
│   ├── Mappings/              # AutoMapper profiles
│   ├── Services/              # Application services
│   └── UseCases/              # Use case DTOs
├── Contact.Domain             # Domain Layer
│   ├── Entities/              # Domain entities
│   ├── Exceptions/            # Domain exceptions
│   └── Interfaces/            # Domain interfaces
└── Contact.Infrastructure     # Infrastructure Layer
    ├── ExternalServices/      # External API integrations
    ├── Persistence/           # Data access
    │   ├── Helper/            # Dapper helpers
    │   └── Repositories/      # Repository implementations
    └── Services/              # Infrastructure services
```

## PostgreSQL Migration Notes

The application has migrated from SQL Server to PostgreSQL with the following changes:

1. Connection string format:
   ```
   Host=localhost;Port=5432;Database=contacts;Username=contacts_admin;Password=password
   ```

2. Dapper helper implementation using Npgsql:
   ```csharp
   public NpgsqlConnection GetConnection()
   {
       return new NpgsqlConnection(myConfig.ConnectionStrings.DefaultConnection);
   }
   ```

3. SQL query adaptations:
   - Use double quotes for identifiers: `"TableName"."ColumnName"`
   - PostgreSQL-specific functions and types
   - UUID type for primary keys

4. Startup configuration:
   - Register the PostgreSQL extension in the database initialization

## Best Practices

When working with the backend codebase, follow these guidelines:

1. Keep domain entities clean and framework-independent
2. Place business logic in the appropriate layer
3. Use proper exception handling
4. Validate all incoming requests
5. Follow SOLID principles
6. Implement proper logging
7. Use async/await consistently
8. Use transactions for operations that modify multiple entities
9. Keep controllers thin by delegating to services
10. Follow the established permission model for security