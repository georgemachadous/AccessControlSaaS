param(
    [string]$MigrationsDir = "flyway",
    [string]$Project = "src/Infrastructure/AccessControl.Infrastructure.csproj",
    [string]$Startup = "src/Infrastructure/AccessControl.Infrastructure.csproj"
)

# Ensure dotnet-ef is available
try {
    dotnet tool restore
} catch {
    Write-Host "dotnet tool restore failed or dotnet-ef already installed globally; continuing..."
}

# Create migrations directory
New-Item -ItemType Directory -Force -Path $MigrationsDir | Out-Null

# Add migration
$timestamp = (Get-Date).ToString('yyyyMMddHHmmss')
$mName = "Initial_$timestamp"
Write-Host "Creating migration $mName"
dotnet ef migrations add $mName -p $Project -s $Startup -o $MigrationsDir\$mName

# Script SQL for flyway
# Use Flyway naming convention: V{timestamp}__ef.sql
$sqlPath = Join-Path $MigrationsDir "V$timestamp`__ef.sql"
Write-Host "Scripting SQL to $sqlPath"
# SQLite does not support idempotent scripts; generate a non-idempotent script
dotnet ef migrations script -p $Project -s $Startup -o $sqlPath

Write-Host "Migration and SQL script generated."
