# Check for duplicate folders backend/ vs src/
Get-ChildItem -Directory -Path . | Where-Object { $_.Name -in @('backend','src') } | ForEach-Object {
    Write-Host "Found: $($_.FullName)"
}

Write-Host "Search for duplicate csproj files:"
Get-ChildItem -Recurse -Filter *.csproj | Select-Object FullName | Sort-Object FullName | Format-List
