FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install Node.js
RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    && curl -fsSL https://deb.nodesource.com/setup_22.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g @angular/cli@19.1.8 \
    && apt-get install -y git procps vim \
    && rm -rf /var/lib/apt/lists/*

USER root

# Set up the workspace
WORKDIR /workspaces/app

# Install dotnet dev certificates and tools
RUN dotnet dev-certs https && dotnet tool update -g dotnet-ef
