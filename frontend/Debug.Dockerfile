FROM node:22

# Install essential tools
RUN apt-get update && apt-get install -y \
    git \
    procps \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy package files
COPY package.json package-lock.json ./

# Install Angular CLI globally
RUN npm install -g @angular/cli@latest

# Install dependencies
RUN npm install

# Set up for development
ENV NODE_ENV=development

# Keep the container running
CMD ["tail", "-f", "/dev/null"]
