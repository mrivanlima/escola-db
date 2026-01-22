# Complete Database Rebuild Script
# Drops all objects and recreates from scratch with seed data

# Configure UTF-8 encoding for proper character display
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
chcp 65001 | Out-Null

$ErrorActionPreference = "Stop"

# Load credentials
. "$PSScriptRoot\db_config.ps1"

$Server = $DbConfig.Server
$Port = $DbConfig.Port
$Database = $DbConfig.Database
$Username = $DbConfig.Username
$Password = $DbConfig.Password

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "DATABASE COMPLETE REBUILD" -ForegroundColor Yellow
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Server: $Server" -ForegroundColor Yellow
Write-Host "Database: $Database" -ForegroundColor Yellow
Write-Host ""

# Create .pgpass file for password
$pgpassFile = "$env:APPDATA\postgresql\pgpass.conf"
$pgpassDir = Split-Path $pgpassFile -Parent

if (-not (Test-Path $pgpassDir)) {
    New-Item -ItemType Directory -Path $pgpassDir -Force | Out-Null
}

$pgpassContent = "${Server}:${Port}:${Database}:${Username}:${Password}"
Set-Content -Path $pgpassFile -Value $pgpassContent -Force

try {
    # Step 1: Drop all schemas
    Write-Host "Step 1: Dropping all schemas..." -ForegroundColor Magenta
    $dropSQL = "DROP SCHEMA IF EXISTS audit CASCADE;
DROP SCHEMA IF EXISTS game CASCADE;
DROP SCHEMA IF EXISTS content CASCADE;
DROP SCHEMA IF EXISTS school CASCADE;
DROP SCHEMA IF EXISTS assets CASCADE;
DROP SCHEMA IF EXISTS identity CASCADE;"
    
    [System.IO.File]::WriteAllText("$PSScriptRoot\temp_drop.sql", $dropSQL, [System.Text.Encoding]::UTF8)
    
    & "C:\Program Files\PostgreSQL\17\bin\psql.exe" `
        -h $Server `
        -p $Port `
        -U $Username `
        -d $Database `
        -f "$PSScriptRoot\temp_drop.sql" `
        -v ON_ERROR_STOP=1
    
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to drop schemas"
    }
    
    # Clean up temp file
    if (Test-Path "$PSScriptRoot\temp_drop.sql") {
        Remove-Item "$PSScriptRoot\temp_drop.sql" -Force
    }
    
    Write-Host "✅ All schemas dropped successfully" -ForegroundColor Green
    Write-Host ""

    # Step 2: Recreate database structure
    Write-Host "Step 2: Recreating database structure..." -ForegroundColor Magenta
    & "C:\Program Files\PostgreSQL\17\bin\psql.exe" `
        -h $Server `
        -p $Port `
        -U $Username `
        -d $Database `
        -f "$PSScriptRoot\..\build_all.sql" `
        -v ON_ERROR_STOP=1
    
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to recreate database structure"
    }
    Write-Host "✅ Database structure recreated successfully" -ForegroundColor Green
    Write-Host ""

    # Step 3: Seed data
    Write-Host "Step 3: Seeding data..." -ForegroundColor Magenta
    & "C:\Program Files\PostgreSQL\17\bin\psql.exe" `
        -h $Server `
        -p $Port `
        -U $Username `
        -d $Database `
        -f "$PSScriptRoot\..\seed_data.sql" `
        -v ON_ERROR_STOP=1
    
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to seed data"
    }
    Write-Host "✅ Data seeded successfully" -ForegroundColor Green
    Write-Host ""

    Write-Host "=========================================" -ForegroundColor Cyan
    Write-Host "✅ DATABASE REBUILD COMPLETED!" -ForegroundColor Green
    Write-Host "=========================================" -ForegroundColor Cyan

} catch {
    Write-Host ""
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    # Clean up password file
    if (Test-Path $pgpassFile) {
        Remove-Item $pgpassFile -Force
    }
}
