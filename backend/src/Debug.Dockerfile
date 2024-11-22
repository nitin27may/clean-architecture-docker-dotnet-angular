# Use the .NET SDK image for development
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

EXPOSE 5000

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . .

# Restore the project dependencies
RUN dotnet restore

# Run the app in watch mode so that it rebuilds automatically when code changes

