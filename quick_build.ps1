# Quick Build Script - Single Connection
param(
    [switch]$UseConfig
)

# Configure UTF-8 encoding for proper character display
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
chcp 65001 | Out-Null

$ErrorActionPreference = "Stop"

# Load credentials
. "$PSScriptRoot\db_config.ps1"

Write-Host "Assets schema will be created after Identity and before School" -ForegroundColor Cyan

$Server = $DbConfig.Server
$Port = $DbConfig.Port
$Database = $DbConfig.Database
$Username = $DbConfig.Username
$Password = $DbConfig.Password

Write-Host "Connecting to Supabase..." -ForegroundColor Cyan
Write-Host "Server: $Server" -ForegroundColor Yellow

# Create .pgpass file for password (avoids shell escaping issues)
$pgpassFile = "$env:APPDATA\postgresql\pgpass.conf"
$pgpassDir = Split-Path $pgpassFile -Parent

if (-not (Test-Path $pgpassDir)) {
    New-Item -ItemType Directory -Path $pgpassDir -Force | Out-Null
}

# Format: hostname:port:database:username:password
$pgpassContent = "${Server}:${Port}:${Database}:${Username}:${Password}"
Set-Content -Path $pgpassFile -Value $pgpassContent -Force

Write-Host "Executing database build..." -ForegroundColor Green

try {
    # Single connection, multiple files
    & "C:\Program Files\PostgreSQL\17\bin\psql.exe" `
        -h $Server `
        -p $Port `
        -U $Username `
        -d $Database `
        -f "$PSScriptRoot\database\build_all.sql" `
        -v ON_ERROR_STOP=1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Database build completed successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "Build failed with exit code: $LASTEXITCODE" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    # Clean up password file
    if (Test-Path $pgpassFile) {
        Remove-Item $pgpassFile -Force
    }
}
