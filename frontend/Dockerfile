# Build stage: Node 26 LTS
FROM node:26-alpine AS builder

# Copy dependency definitions
COPY package*.json ./

## installing and Storing node modules on a separate layer will prevent unnecessary npm installs at each build
RUN npm ci && mkdir /app && mv ./node_modules /app/node_modules

# Change directory so that our commands run inside this new directory
WORKDIR /app

# Get all the code needed to run the app
COPY . .

# Build server side bundles
RUN npm run build

# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:1.30-alpine

# Remove default Nginx website and configuration
RUN rm -rf /usr/share/nginx/html/* /etc/nginx/conf.d/default.conf

# Copy the build output to replace the default nginx contents.
COPY --from=builder /app/dist/contact-portal/browser /usr/share/nginx/html

# Copy the nginx file to fix fallback issue on refreshing.
COPY /nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 8080

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
