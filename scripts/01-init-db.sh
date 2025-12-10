#!/bin/bash
set -e

# Create the contacts database if it doesn't exist
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "postgres" <<-EOSQL
    SELECT 'CREATE DATABASE contacts'
    WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'contacts')\gexec
EOSQL

# Run the seed script against the contacts database
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "contacts" -f /docker-entrypoint-initdb.d/02-seed-data.sql
