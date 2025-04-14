FROM postgres:16-alpine

# Set environment variables
ENV POSTGRES_PASSWORD=postgres
ENV POSTGRES_USER=postgres
ENV POSTGRES_DB=contact_db

# Copy initialization scripts
COPY ./backend/scripts/seed-data.sql /docker-entrypoint-initdb.d/

# Expose PostgreSQL port
EXPOSE 5432

# Set the data directory
VOLUME ["/var/lib/postgresql/data"]