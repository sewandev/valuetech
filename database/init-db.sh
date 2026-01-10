#!/bin/bash
# Wait for SQL Server to be up
echo "Waiting for SQL Server to be ready..."
for i in {1..50};
do
    /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P ValueTech123! -C -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]
    then
        echo "SQL Server is ready."
        break
    else
        echo "Not ready yet..."
        sleep 1
    fi
done

# Run Init Scripts
echo "Running initialization scripts..."
for file in /init-scripts/*.sql
do
    echo "Executing $file..."
    /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P ValueTech123! -C -d master -i "$file"
done

echo "Database initialization completed."
