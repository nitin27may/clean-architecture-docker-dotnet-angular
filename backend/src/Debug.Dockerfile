FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install required dependencies
RUN apt-get update && apt-get install -y \
    curl \
    git \
    procps \
    wget \
    vim \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Install dotnet dev certificates
RUN dotnet dev-certs https

# Copy a minimal set of files required for restoring dependencies

FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install required dependencies
RUN apt-get update && apt-get install -y \
    curl \
    git \
    procps \
    wget \
    vim \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Install dotnet dev certificates
RUN dotnet dev-certs https

# Copy a minimal set of files required for restoring dependencies
COPY ["Contact.Api/Contact.Api.csproj", "src/Contact.Api/"]
COPY ["Contact.Application/Contact.Application.csproj", "src/Contact.Application/"]
COPY ["Contact.Domain/Contact.Domain.csproj", "src/Contact.Domain/"]
COPY ["Contact.Infrastructure/Contact.Infrastructure.csproj", "src/Contact.Infrastructure/"]
COPY ["Contact.Common/Contact.Common.csproj", "src/Contact.Common/"]

# Restore dependencies
WORKDIR /app/src
RUN dotnet restore "Contact.Api/Contact.Api.csproj"

# Keep the container running
CMD ["tail", "-f", "/dev/null"]
