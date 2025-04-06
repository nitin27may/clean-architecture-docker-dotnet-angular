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
   # Unix/Mac
   cp .env.example .env
   
   # Windows (PowerShell)
   Copy-Item .env.example .env
   ```

2. Edit the `.env` file to configure environment variables as needed.

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

3. Run the API:

   ```bash
   dotnet run --project API
   ```

   The API will be available at `https://localhost:5001` and `http://localhost:5000`

### Frontend (Angular)

1. Navigate to the frontend directory:

   ```bash
   cd frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm start
   ```

   The frontend will be available at `http://localhost:4200`

## Database Access

### Using Docker Tools

Access the PostgreSQL database using:

```bash
docker exec -it [postgres-container-name] psql -U postgres
```

Replace `[postgres-container-name]` with the actual container name from `docker ps`.

### Using External Tools

You can also connect using external tools like pgAdmin, DBeaver, or JetBrains DataGrip with these connection details:

- **Host**: localhost
- **Port**: 5432
- **Username**: postgres
- **Password**: (from your .env file)
- **Database**: ContactsDB

## Development Workflow

### Making Backend Changes

1. Modify code in the appropriate layer:
   - Domain: For entity changes
   - Application: For business logic
   - Infrastructure: For external service implementations
   - API: For endpoint changes

2. If using Docker, changes will be automatically applied with hot reload.

3. If running locally, some changes may require application restart.

### Making Frontend Changes

1. Modify Angular code in the frontend directory.

2. Follow these best practices:
   - Use signals for state management
   - Use inject() for dependency injection
   - Use computed signals for derived state
   - Use Angular Material components
   - Use TailwindCSS for styling
   - Create reusable components in the shared module

3. Changes will be automatically applied with hot reload.

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
   - The debug.dockerfile is configured for remote debugging
   - In Visual Studio Code, use the "Attach to .NET Core in Docker" launch configuration

2. **Without Docker**:
   - Use standard .NET debugging in your IDE

### Debugging Frontend

1. **With Docker**:
   - Access the application and use browser dev tools

2. **Without Docker**:
   - Use standard Angular debugging with browser dev tools
   - For VS Code, use the "Launch Chrome" launch configuration

## Common Tasks

### Adding a New Entity

1. Create the entity in the Domain layer
2. Create repository interface in Domain layer
3. Implement repository in Infrastructure layer
4. Create DTOs in Application layer
5. Create mapping profile in Application layer
6. Create service in Application layer
7. Create API controller in API layer
8. Add Angular service and components in Frontend

### Adding a New API Endpoint

1. Choose the appropriate controller or create a new one
2. Add the endpoint method with proper HTTP verb
3. Implement validation if needed
4. Add appropriate authorization attributes
5. Update Swagger documentation

### Adding a New Angular Component

1. Generate component:
   ```
   ng generate component features/your-feature/your-component
   ```

2. Add routing if needed
3. Implement the component with signals for state
4. Use inject() for dependency injection
5. Add necessary services

## Building for Production

### Backend

```bash
cd backend/src
dotnet publish -c Release
```

### Frontend

```bash
cd frontend
npm run build:prod
```

### Using Docker

```bash
docker-compose -f docker-compose.prod.yml up --build
```

## Additional Resources

- See [Backend Documentation](./backend.md) for detailed backend information
- See [Frontend Documentation](./frontend.md) for detailed frontend information
- See [Clean Architecture Series](./architecture-series.md) for architectural deep dives
