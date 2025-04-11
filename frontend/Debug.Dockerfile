FROM node:22-alpine

# Install essential tools (using alpine versions to keep image smaller)
RUN apk add --no-cache git procps curl vim bash

WORKDIR /app

# Don't copy package.json here - we'll mount it from the volume

# Install Angular CLI globally
RUN npm install -g @angular/cli@19.1.8

# Set up for development
ENV NODE_ENV=development

# Expose port 4200
EXPOSE 4200

# Use a startup script instead of tail
CMD ["/bin/bash", "-c", "echo 'Frontend container is ready. Run npm commands manually.' && tail -f /dev/null"]
