#!/bin/bash
set -e

# Create all required directories with proper ownership
mkdir -p /.system/system
mkdir -p /var/opt/mssql/log
mkdir -p /var/opt/mssql/data
mkdir -p /var/opt/mssql/backup
chown -R mssql:mssql /.system
chown -R mssql:mssql /var/opt/mssql

# Switch to mssql user to start the server
su mssql -c "/opt/mssql/bin/sqlservr &"

# Wait for SQL Server to be ready
for i in {1..60}; do
  if /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" &>/dev/null; then
    echo "SQL Server is ready"
    break
  else
    echo "SQL Server is not ready yet... waiting 2 seconds (attempt $i/60)"
    sleep 2
  fi
  
  # If we've waited too long, check server status
  if [ $i -eq 60 ]; then
    echo "SQL Server startup timed out. Checking server status..."
    ps aux | grep sqlservr
    cat /var/opt/mssql/log/errorlog* 2>/dev/null || echo "No error logs found"
  fi
done

# Run seed script if it exists
if [ -f /scripts/seed-data.sql ]; then
  echo "Running seed script..."
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -d master -i /scripts/seed-data.sql || echo "Warning: Seed script execution failed"
fi

# Keep container running by tailing logs or sleeping forever
tail -f /var/opt/mssql/log/errorlog* 2>/dev/null || sleep infinity