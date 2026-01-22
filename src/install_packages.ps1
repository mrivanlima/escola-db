# ==============================================
# Escola Platform - NuGet Package Installation
# ==============================================
# Installs Entity Framework Core and PostgreSQL packages

param(
    [string]$RootDir = $PSScriptRoot
)

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Installing EF Core & PostgreSQL Packages" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Navigate to root directory
if ($RootDir -eq $PSScriptRoot) {
    $RootDir = Split-Path -Parent $PSScriptRoot
}

$SrcDir = Join-Path $RootDir "src"

if (-not (Test-Path $SrcDir)) {
    Write-Host "ERROR: src directory not found at $SrcDir" -ForegroundColor Red
    exit 1
}

Set-Location $SrcDir
Write-Host "Working Directory: $SrcDir" -ForegroundColor Yellow
Write-Host ""

# STEP 1: Install packages in Infrastructure project
Write-Host "STEP 1: Installing packages in Escola.Infrastructure..." -ForegroundColor Magenta

Write-Host "  [+] Microsoft.EntityFrameworkCore" -ForegroundColor Cyan
dotnet add Escola.Infrastructure/Escola.Infrastructure.csproj package Microsoft.EntityFrameworkCore --source https://api.nuget.org/v3/index.json

Write-Host "  [+] Npgsql.EntityFrameworkCore.PostgreSQL" -ForegroundColor Cyan
dotnet add Escola.Infrastructure/Escola.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --source https://api.nuget.org/v3/index.json

Write-Host "[OK] Infrastructure packages installed" -ForegroundColor Green
Write-Host ""

# STEP 2: Install packages in Api project
Write-Host "STEP 2: Installing packages in Escola.Api..." -ForegroundColor Magenta

Write-Host "  [+] Microsoft.EntityFrameworkCore.Design" -ForegroundColor Cyan
dotnet add Escola.Api/Escola.Api.csproj package Microsoft.EntityFrameworkCore.Design --source https://api.nuget.org/v3/index.json

Write-Host "[OK] Api packages installed" -ForegroundColor Green
Write-Host ""

# STEP 3: Restore all projects
Write-Host "STEP 3: Restoring all projects..." -ForegroundColor Magenta
dotnet restore --source https://api.nuget.org/v3/index.json

if ($LASTEXITCODE -eq 0) {
    Write-Host "[OK] All packages restored successfully" -ForegroundColor Green
} else {
    Write-Host "[WARN] Restore completed with warnings" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Package Installation Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Installed Packages:" -ForegroundColor Yellow
Write-Host "  Infrastructure:" -ForegroundColor Cyan
Write-Host "    - Microsoft.EntityFrameworkCore" -ForegroundColor White
Write-Host "    - Npgsql.EntityFrameworkCore.PostgreSQL" -ForegroundColor White
Write-Host "  Api:" -ForegroundColor Cyan
Write-Host "    - Microsoft.EntityFrameworkCore.Design" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Create DbContext in Escola.Infrastructure/Persistence" -ForegroundColor White
Write-Host "  2. Create domain entities in Escola.Domain" -ForegroundColor White
Write-Host "  3. Configure connection string in appsettings.json" -ForegroundColor White
Write-Host "  4. Set up dependency injection in Program.cs" -ForegroundColor White
Write-Host ""

# Return to root directory
Set-Location $RootDir
