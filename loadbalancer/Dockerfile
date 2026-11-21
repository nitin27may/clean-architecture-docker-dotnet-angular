# Nginx stable, pinned to a minor line so Dependabot can propose upgrades
FROM nginx:1.30-alpine
# Copy the main nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf

# Copy the load balancer-specific configuration
# COPY default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

# Start Nginx when the container has provisioned.
CMD ["nginx", "-g", "daemon off;"]
