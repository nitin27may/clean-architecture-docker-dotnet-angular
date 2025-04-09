---
layout: default
title: Docker Guide
nav_order: 3
permalink: /docker-guide
---

# Docker Guide
{: .no_toc }

This guide explains the Docker setup for the Clean Architecture Full-Stack application. It covers all Dockerfiles, docker-compose configurations, and environment variables.

## Table of Contents
{: .no_toc .text-delta }

1. TOC
{:toc}

## Overview

The application uses Docker to containerize all services for consistent development and deployment. The Docker setup includes:

- Frontend container (Angular 19)
- Backend API container (.NET 9)
- Database container (PostgreSQL)
- Nginx container (Reverse proxy)

## Dockerfiles

### Frontend Dockerfile

Located at `frontend/Dockerfile`, this file builds the Angular application:

```dockerfile
# Build stage
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine
COPY --from=build /app/dist/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

This is a multi-stage build that:
1. Uses Node.js to build the Angular application
2. Copies the built files to an Nginx container
3. Uses a custom Nginx configuration
4. Exposes port 80

### Backend Dockerfile

Located at `backend/src/Dockerfile`, this file builds the .NET API:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Contact.Api/Contact.Api.csproj", "Contact.Api/"]
COPY ["Contact.Application/Contact.Application.csproj", "Contact.Application/"]
COPY ["Contact.Domain/Contact.Domain.csproj", "Contact.Domain/"]
COPY ["Contact.Infrastructure/Contact.Infrastructure.csproj", "Contact.Infrastructure/"]
RUN dotnet restore "Contact.Api/Contact.Api.csproj"
COPY . .
WORKDIR "/src/Contact.Api"
RUN dotnet build "Contact.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Contact.Api.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contact.Api.dll"]
```

This uses a multi-stage build to:
1. Restore NuGet packages
2. Build the .NET application
3. Publish the application
4. Create a lightweight runtime image

### Nginx Dockerfile

Located at `loadbalancer/Dockerfile`, this serves as the entry point for all requests:

```dockerfile
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
```

This simple Dockerfile:
1. Uses the official Nginx Alpine image
2. Replaces the default configuration with our custom one

## Docker Compose Configuration

The primary docker-compose file is `docker-compose.yml`:

```yaml
version: '3.8'

services:
  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    container_name: contact-frontend
    restart: always
    depends_on:
      - api
    networks:
      - contact-network

  api:
    build:
      context: ./backend/src
      dockerfile: Dockerfile
    container_name: contact-api
    restart: always
    depends_on:
      - postgres
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=${POSTGRES_CONNECTION_STRING}
      - JwtSettings__SecretKey=${JWT_SECRET_KEY}
      - JwtSettings__Issuer=${JWT_ISSUER}
      - JwtSettings__Audience=${JWT_AUDIENCE}
      - JwtSettings__ExpiryMinutes=${JWT_EXPIRY_MINUTES}
    networks:
      - contact-network

  postgres:
    image: postgres:16-alpine
    container_name: contact-postgres
    restart: always
    environment:
      - POSTGRES_USER=${POSTGRES_USER}
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
      - POSTGRES_DB=${POSTGRES_DB}
    volumes:
      - postgres-data:/var/lib/postgresql/data
      - ./backend/db/init.sql:/docker-entrypoint-initdb.d/init.sql
    networks:
      - contact-network

  nginx:
    build:
      context: ./loadbalancer
      dockerfile: Dockerfile
    container_name: contact-nginx
    restart: always
    ports:
      - "80:80"
    depends_on:
      - frontend
      - api
    networks:
      - contact-network

networks:
  contact-network:
    driver: bridge

volumes:
  postgres-data:
```

This configuration:
1. Defines all four services (frontend, api, postgres, nginx)
2. Establishes service dependencies
3. Sets environment variables from the `.env` file
4. Configures networking
5. Creates a persistent volume for PostgreSQL data

## Environment Variables (.env file)

The application uses a `.env` file (copied from `.env.example`) to configure various settings. Here's an explanation of each variable:

