# Create image based off of the official 12.8-alpine
FROM node:22

WORKDIR /app

# Copy dependency definitions
COPY package.json package-lock.json ./

RUN npm install -g @angular/cli@latest

RUN npm install

COPY . .
