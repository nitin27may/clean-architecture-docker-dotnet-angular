FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install required dependencies
RUN apt-get update && apt-get install -y \
    curl \
    git \
    procps \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy a minimal set of files required for restoring dependencies
COPY ["Contact.Api/Contact.Api.csproj", "Contact.Api/"]
COPY ["Contact.Application/Contact.Application.csproj", "Contact.Application/"]
COPY ["Contact.Domain/Contact.Domain.csproj", "Contact.Domain/"]
COPY ["Contact.Infrastructure/Contact.Infrastructure.csproj", "Contact.Infrastructure/"]
COPY ["Contact.Common/Contact.Common.csproj", "Contact.Common/"]

# Restore dependencies
RUN dotnet restore "Contact.Api/Contact.Api.csproj"

# Keep the container running
CMD ["tail", "-f", "/dev/null"]
