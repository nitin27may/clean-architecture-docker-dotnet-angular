# Use the standard Nginx image from Docker Hub
FROM nginx
# Copy the main nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf

# Copy the load balancer-specific configuration
# COPY default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

# Start Nginx when the container has provisioned.
CMD ["nginx", "-g", "daemon off;"]
