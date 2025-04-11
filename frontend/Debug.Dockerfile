FROM node:22

# Install essential tools
RUN apt-get update && apt-get install -y \
    git \
    procps \
    curl \
    vim \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy package files
COPY package.json package-lock.json ./

# Install Angular CLI globally with specific version
RUN npm install -g @angular/cli@19.1.8

# Install dependencies
RUN npm install

# Set up for development
ENV NODE_ENV=development

# Expose port 4200
EXPOSE 4200

# Keep the container running with a default command
ENTRYPOINT ["tail", "-f", "/dev/null"]
