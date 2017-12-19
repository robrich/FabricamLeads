# guess how long it'll take SQL Server to start up
sleep 20s

echo "Creating Fabricam database"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i setup.sql
echo "Fabricam database created"
