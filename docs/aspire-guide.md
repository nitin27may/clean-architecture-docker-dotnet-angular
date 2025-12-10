# .NET Aspire Guide

This guide explains how to run the Clean Architecture application using .NET Aspire for local development.

## What is .NET Aspire?

.NET Aspire is a cloud-ready stack for building observable, production-ready, distributed applications. It provides:

- **Service Discovery**: Automatic service registration and discovery
- **Orchestration**: Start and manage all application components from a single command
- **Observability**: Built-in dashboard with logs, traces, and metrics
- **Container Management**: Automatic container lifecycle management
- **Health Monitoring**: Real-time health checks for all services

## Prerequisites

Before running the application with Aspire, ensure you have:

| Requirement | Version | Download |
|-------------|---------|----------|
| .NET SDK | 10.0+ | [Download](https://dotnet.microsoft.com/download/dotnet/10.0) |
| Node.js | 22 LTS | [Download](https://nodejs.org/) |
| Docker Desktop | Latest | [Download](https://www.docker.com/products/docker-desktop) |

> ⚠️ **Important**: Use Node.js 22 LTS, not Node.js 23. Odd-numbered Node.js versions are not LTS and may have compatibility issues with Angular.

### Verify Installations

```powershell
# Check .NET version
dotnet --version
# Should output: 10.0.x or higher

# Check Node.js version
node --version
# Should output: v22.x.x

# Check npm version
npm --version
# Should output: 10.x.x or higher

# Check Docker is running
docker --version
```

## Project Structure

```
clean-architecture-docker-dotnet-angular/
├── aspire/
│   ├── AppHost/              # Aspire orchestration host
│   │   ├── AppHost.cs        # Service definitions
│   │   └── AppHost.csproj    # Project file
│   └── ServiceDefaults/      # Shared Aspire configurations
│       ├── Extensions.cs     # Extension methods
│       └── ServiceDefaults.csproj
```

## Running with Aspire

### Step 1: Install Frontend Dependencies

Before starting Aspire, you must install the Angular dependencies:

```bash
cd frontend
npm install
cd ..
```

### Step 2: Start Aspire

From the repository root:

```bash
dotnet run --project aspire/AppHost
```

### Step 3: Access the Dashboard

Once started, Aspire will display URLs in the console. Open the **Aspire Dashboard** (typically at `https://localhost:17178`) to see all services.

## Services Overview

When running with Aspire, the following services are orchestrated:

| Service | Description | Default Port |
|---------|-------------|--------------|
| **postgres** | PostgreSQL 17 database | 5432 |
| **pgadmin** | PostgreSQL admin interface | Dynamic |
| **contact-api** | .NET 10 backend API | 5217 |
| **frontend** | Angular 21 application | 4200 |

## Aspire Dashboard Features

### Resources View

View all running services, their status, and endpoints:

![Aspire Resources](screenshots/aspire-resources.png)

### Console Logs

Real-time aggregated logs from all services:

- Filter by service
- Search within logs
- Color-coded log levels

### Traces

Distributed tracing across services:

- Request timelines
- Service dependencies
- Performance bottlenecks

### Metrics

Built-in metrics collection:

- Request rates
- Response times
- Error rates

## Configuration

### AppHost.cs Overview

The `aspire/AppHost/AppHost.cs` file defines all services:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL with pgAdmin
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume();

var contactsDb = postgres.AddDatabase("contacts");

// .NET API with reference to database
var api = builder.AddProject<Projects.Contact_Api>("contact-api")
    .WithReference(contactsDb);

// Angular frontend with reference to API
builder.AddNpmApp("frontend", "../../frontend", "serve")
    .WithReference(api)
    .WithEndpoint(port: 4200, scheme: "http");
```

### Database Connection

Aspire automatically:
1. Creates the PostgreSQL container
2. Generates connection strings
3. Injects them into the API service

No manual connection string configuration needed!

### Environment Variables

Aspire injects these automatically:
- `ConnectionStrings__contacts` - Database connection string
- `services__contact-api__http__0` - API URL for frontend

## Troubleshooting

### Error: Cannot find module '@angular/cli'

**Solution**: Install frontend dependencies first:
```bash
cd frontend
npm install
cd ..
```

### Error: Port already in use

**Solution**: Stop any services using the conflicting ports:
```powershell
# Find process using port 5217
netstat -ano | findstr :5217
# Kill the process
taskkill /PID <process_id> /F
```

### Error: Docker not running

**Solution**: Start Docker Desktop and wait for it to fully initialize.

### Error: Database not initialized

**Solution**: The database is automatically created on first run. If issues persist:
1. Stop Aspire
2. Remove the PostgreSQL volume: `docker volume rm <volume_name>`
3. Restart Aspire

### Error: Frontend not building

**Solution**: Check Node.js version:
```bash
node --version  # Should be v22.x.x
```

If using nvm:
```bash
nvm use 22
```

## Development Workflow

### Hot Reload

- **Frontend**: Angular uses `ng serve` with automatic hot reload
- **Backend**: .NET uses hot reload by default in development mode

### Making Changes

1. **Frontend changes**: Save files, Angular CLI automatically rebuilds
2. **Backend changes**: Save files, .NET hot reload applies changes
3. **Database changes**: Update seed scripts in `scripts/` folder

### Viewing Logs

Use the Aspire Dashboard to view logs from all services in one place, or check individual terminal outputs.

## Stopping Aspire

Press `Ctrl+C` in the terminal running Aspire to gracefully stop all services.

## Comparison: Aspire vs Docker Compose

| Feature | Aspire | Docker Compose |
|---------|--------|----------------|
| Best for | Local development | Production deployment |
| Service discovery | Automatic | Manual configuration |
| Hot reload | Built-in | Requires volume mounts |
| Dashboard | Included | Separate tools needed |
| Container management | Automatic | Manual |
| Debug integration | Native VS/VS Code | Requires attach |

## Next Steps

- [Development Guide](./development-guide.md) - Detailed development setup
- [Backend Documentation](./backend.md) - API architecture
- [Frontend Documentation](./frontend.md) - Angular structure
- [Docker Guide](./docker-guide.md) - Production deployment
