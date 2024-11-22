# Create image based off of the official 12.8-alpine
FROM node:22-alpine

#RUN echo "nameserver 8.8.8.8" |  tee /etc/resolv.conf > /dev/null
WORKDIR /app

# RUN npm install -g @angular/cli@latest
# Copy dependency definitions
COPY package.json .

## installing and Storing node modules on a separate layer will prevent unnecessary npm installs at each build
RUN npm install --legacy-peer-deps

COPY . .

EXPOSE 4200 49153

# Start the application in debug mode
# CMD ["npm", "run", "start:debug"]

