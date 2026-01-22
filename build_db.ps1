param(
    [string]$Server,
    [string]$Port,
    [string]$Database,
    [string]$Username,
    [string]$Password
)

# Configure UTF-8 encoding for proper character display
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
chcp 65001 | Out-Null

$ErrorActionPreference = "Continue"

# Load credentials from config file if not provided
if (-not $Server -and (Test-Path "$PSScriptRoot\db_config.ps1")) {
    . "$PSScriptRoot\db_config.ps1"
    $Server = $DbConfig.Server
    $Port = $DbConfig.Port
    $Database = $DbConfig.Database
    $Username = $DbConfig.Username
    $Password = $DbConfig.Password
}

# Set environment variable for password (so psql doesn't prompt)
if ($Password) {
    $env:PGPASSWORD = $Password
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Database Build Script - Project Escola" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if psql is available
if (-not (Get-Command psql -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: psql command not found. Please install PostgreSQL client tools." -ForegroundColor Red
    exit 1
}

Write-Host "Connection Details:" -ForegroundColor Yellow
Write-Host "  Server: $Server"
Write-Host "  Port: $Port"
Write-Host "  Database: $Database"
Write-Host "  Username: $Username"
Write-Host ""

# Execution order
$ExecutionOrder = @(
    "database/shared/init_schemas.sql",
    "database/shared/functions/handle_updated_at.sql",
    "database/identity/tables/tenants.sql",
    "database/identity/tables/app_users.sql",
    "database/assets/tables/media_files.sql",
    "database/school/tables/students.sql",
    "database/school/tables/guardians.sql",
    "database/school/tables/student_guardians.sql",
    "database/school/tables/classes.sql",
    "database/school/tables/teachers.sql",
    "database/school/tables/class_students.sql",
    "database/content/tables/modules.sql",
    "database/content/tables/activities.sql",
    "database/content/tables/assets.sql",
    "database/game/tables/student_progress.sql",
    "database/game/tables/badges.sql",
    "database/game/tables/student_badges.sql"
)

Write-Host "Starting database build process..." -ForegroundColor Green
Write-Host ""

$SuccessCount = 0
$ErrorCount = 0

foreach ($file in $ExecutionOrder) {
    $fullPath = Join-Path $PSScriptRoot $file
    
    if (Test-Path $fullPath) {
        Write-Host "Executing: $file" -ForegroundColor Cyan
        
        try {
            $result = psql -h $Server -p $Port -U $Username -d $Database -f $fullPath 2>&1
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "  Success" -ForegroundColor Green
                $SuccessCount++
            } else {
                Write-Host "  Failed" -ForegroundColor Red
                Write-Host "  Error: $result" -ForegroundColor Red
                $ErrorCount++
            }
        } catch {
            Write-Host "  Exception: $($_.Exception.Message)" -ForegroundColor Red
            $ErrorCount++
        }
    } else {
        Write-Host "  File not found: $fullPath" -ForegroundColor Yellow
        $ErrorCount++
    }
    
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Build Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total files: $($ExecutionOrder.Count)"
Write-Host "Successful: $SuccessCount" -ForegroundColor Green
Write-Host "Failed: $ErrorCount" -ForegroundColor $(if ($ErrorCount -gt 0) { "Red" } else { "Green" })
Write-Host ""

if ($ErrorCount -eq 0) {
    Write-Host "Database build completed successfully!" -ForegroundColor Green
} else {
    Write-Host "Database build completed with errors." -ForegroundColor Red
    exit 1
}
