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
- Database access with Dapper
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
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    // Implementation using Dapper
}
```

### Generic Service Pattern

Services follow a generic pattern for common operations:

```csharp
public interface IGenericService<TDto, TEntity> 
    where TDto : class
    where TEntity : class
{
    Task<TDto> GetByIdAsync(int id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TDto dto);
    Task UpdateAsync(TDto dto);
    Task DeleteAsync(int id);
}

public class GenericService<TDto, TEntity> : IGenericService<TDto, TEntity>
    where TDto : class
    where TEntity : class
{
    // Implementation using repository and automapper
}
```

### Unit of Work Pattern

For transaction management:

```csharp
public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```

## Authentication System

JWT-based authentication with:
- Token generation and validation
- Refresh token mechanism
- Role-based authorization
- Claims-based authorization policies

## API Features

### Contact Management API

Complete CRUD operations:
- `GET /api/contacts` - List all contacts
- `GET /api/contacts/{id}` - Get contact by ID
- `POST /api/contacts` - Create new contact
- `PUT /api/contacts/{id}` - Update contact
- `DELETE /api/contacts/{id}` - Delete contact

### User Management API

User operations:
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/forgot-password` - Initiate password reset
- `POST /api/auth/reset-password` - Complete password reset
- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update user profile
- `PUT /api/users/change-password` - Change password

## Error Handling

Standardized error handling approach:
- Global exception middleware
- Structured error responses
- Detailed logging
- Custom domain exceptions

## Validation

Request validation using FluentValidation:
- Input sanitization
- Business rule validation
- Custom validation messages
- Validation failure responses

## Logging & Auditing

Comprehensive logging strategy:
- Activity logging for user actions
- Audit trails for data changes
- Exception logging
- Performance metrics

## Docker Integration

The backend is containerized with two Docker configurations:

1. **Production Build**
   - Multi-stage build for minimal image size
   - Optimized for performance
   - Environment-specific configurations

2. **Development Build**
   - Configured for active development
   - Hot reload support
   - Debugging capabilities

## Database Migration & Seeding

Database initialization strategy:
- SQL scripts for schema creation
- Data seeding for initial setup
- Default users and roles creation
- Docker volume persistence

## Project Structure

```
/backend/src
├── API/                       # API Layer
│   ├── Controllers/           # API endpoints
│   ├── Middleware/            # Custom middleware
│   ├── Extensions/            # Service configuration extensions
│   └── Program.cs             # Application entry point
├── Application/               # Application Layer
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Interfaces/            # Application interfaces
│   ├── Mapping/               # AutoMapper profiles
│   ├── Services/              # Application services
│   └── Validation/            # Request validators
├── Domain/                    # Domain Layer
│   ├── Entities/              # Domain entities
│   ├── Events/                # Domain events
│   ├── Exceptions/            # Domain-specific exceptions
│   └── Interfaces/            # Domain interfaces
└── Infrastructure/            # Infrastructure Layer
    ├── Data/                  # Data access
    ├── Identity/              # Authentication implementation
    ├── Logging/               # Logging implementation
    └── Services/              # External service implementations
```

## Best Practices

When working with the backend codebase, follow these guidelines:

1. Keep domain entities clean and framework-independent
2. Place business logic in the appropriate layer
3. Use proper exception handling
4. Validate all incoming requests
5. Follow SOLID principles
6. Create comprehensive unit tests
7. Document API endpoints properly
8. Implement proper logging
9. Use async/await consistently
10. Maintain high code coverage in tests
