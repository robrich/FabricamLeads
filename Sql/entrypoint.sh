# simultaniously:
# - start SQL Server
# - start the script to create the DB and import the data
# - keep the process running
/opt/mssql/bin/sqlservr & /app/import-data.sh & tail -f /dev/null
