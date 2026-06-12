<#
Setup user secrets for the API project (src/Api) using dotnet user-secrets.
Run from repository root in PowerShell:
  powershell -ExecutionPolicy Bypass -File scripts/setup-user-secrets.ps1

This script generates a strong random JWT symmetric key and random placeholder SSO client ids/secrets.
It uses `dotnet user-secrets set` with the --project pointing to src/Api.
#>

param(
    [string]$ProjectPath = "src/Api"
)

function Ensure-DotNet {
    try {
        $info = dotnet --info 2>&1
        if ($LASTEXITCODE -ne 0) { throw "dotnet not found" }
    } catch {
        Write-Error "dotnet SDK not found. Install .NET SDK and retry."
        exit 1
    }
}

function New-RandomBase64([int]$bytes = 64) {
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $buffer = New-Object 'Byte[]' $bytes
    $rng.GetBytes($buffer)
    return [Convert]::ToBase64String($buffer)
}

Ensure-DotNet

# Generate secrets
$jwtKey = New-RandomBase64 48
$googleId = "google-" + (New-RandomBase64 8)
$googleSecret = New-RandomBase64 32
$msId = "ms-" + (New-RandomBase64 8)
$msSecret = New-RandomBase64 32
$ghId = "gh-" + (New-RandomBase64 8)
$ghSecret = New-RandomBase64 32

Write-Host "Setting user-secrets for project: $ProjectPath"

# Initialize user-secrets if needed (safe to run twice)
Push-Location $ProjectPath
try {
    dotnet user-secrets init 2>$null | Out-Null
} catch {
    # ignore
}

# Set secrets
dotnet user-secrets set "Jwt:PrivateKey" "$jwtKey" --project .
dotnet user-secrets set "Jwt:Issuer" "AccessControlSaaS" --project .
dotnet user-secrets set "Jwt:Audience" "AccessControlSaaS-Client" --project .

dotnet user-secrets set "Sso:GoogleClientId" "$googleId" --project .
dotnet user-secrets set "Sso:GoogleClientSecret" "$googleSecret" --project .

dotnet user-secrets set "Sso:MicrosoftClientId" "$msId" --project .
dotnet user-secrets set "Sso:MicrosoftClientSecret" "$msSecret" --project .

dotnet user-secrets set "Sso:GithubClientId" "$ghId" --project .
dotnet user-secrets set "Sso:GithubClientSecret" "$ghSecret" --project .

# Also set connection string for local sqlite dev (optional)
dotnet user-secrets set "ConnectionStrings:Default" "Data Source=accesscontrol.db" --project .

Write-Host "User-secrets set. Review or replace SSO client IDs/secrets with real provider values when integrating with Google/Microsoft/GitHub."
Write-Host "JWT key stored in user-secrets."
Pop-Location

Write-Host "Done. To view secrets run: dotnet user-secrets list --project src/Api"
