#!/bin/bash
BACKUP_DIR="/opt/accesscontrol/backups"
DATE=$(date +%Y%m%d_%H%M%S)
RETENTION_DAYS=30
SQL_SA_PASSWORD=${SQL_SA_PASSWORD:-"YourStrong@Passw0rd"}

mkdir -p $BACKUP_DIR

# Backup SQL Server
echo "Backing up SQL Server..."
docker exec sqlserver /opt/mssql-tools/bin/sqlcmd \
    -S localhost -U sa -P "$SQL_SA_PASSWORD" \
    -Q "BACKUP DATABASE [AccessControl] TO DISK = N'/var/opt/mssql/backup/AccessControl_$DATE.bak'"

docker cp sqlserver:/var/opt/mssql/backup/AccessControl_$DATE.bak $BACKUP_DIR/

# Backup Redis
echo "Backing up Redis..."
docker exec redis redis-cli BGSAVE
sleep 5
docker cp redis:/data/dump.rdb $BACKUP_DIR/redis_$DATE.rdb

# Backup configurations
echo "Backing up configurations..."
tar -czf $BACKUP_DIR/config_$DATE.tar.gz -C /opt/accesscontrol docker-compose.yml .env nginx/

# Cleanup old backups
echo "Cleaning up old backups..."
find $BACKUP_DIR -type f -mtime +$RETENTION_DAYS -delete

echo "Backup completed: $DATE"
