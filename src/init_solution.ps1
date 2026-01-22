# Escola Platform - Solution Initialization Script
# .NET Clean Architecture Setup
# Run from project root: .\src\init_solution.ps1

param(
    [string]$DotNetVersion = "net9.0"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Escola Platform - .NET Solution Setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get the absolute path to the src directory
$SrcDir = $PSScriptRoot
$RootDir = Split-Path $SrcDir -Parent

Write-Host "Root Directory: $RootDir" -ForegroundColor Yellow
Write-Host "Src Directory: $SrcDir" -ForegroundColor Yellow
Write-Host ""

# Verify .NET SDK is installed
Write-Host "Checking .NET SDK..." -ForegroundColor Cyan
$sdkVersion = (dotnet --version 2>&1) | Out-String
$sdkVersion = $sdkVersion.Trim()

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: .NET SDK not found. Please install .NET 8 or 9." -ForegroundColor Red
    exit 1
}
Write-Host "[OK] Found .NET SDK version: $sdkVersion" -ForegroundColor Green

# Determine framework target based on SDK version (only if using default)
if ($DotNetVersion -eq "net9.0") {
    if ($sdkVersion -like "10.*") {
        $DotNetVersion = "net10.0"
        Write-Host "[INFO] Auto-detected target: net10.0 (SDK 10.x)" -ForegroundColor Cyan
    } elseif ($sdkVersion -like "9.*") {
        $DotNetVersion = "net9.0"
        Write-Host "[INFO] Auto-detected target: net9.0 (SDK 9.x)" -ForegroundColor Cyan
    } else {
        $DotNetVersion = "net8.0"
        Write-Host "[INFO] Auto-detected target: net8.0 (SDK 8.x)" -ForegroundColor Cyan
    }
}

Write-Host "Target Framework: $DotNetVersion" -ForegroundColor Yellow

# Navigate to src directory
Set-Location $SrcDir

# STEP 1: Create Solution
Write-Host "STEP 1: Creating Solution..." -ForegroundColor Magenta
$solutionFile = if (Test-Path "Escola.sln") { "Escola.sln" } elseif (Test-Path "Escola.slnx") { "Escola.slnx" } else { "" }
if ($solutionFile) {
    Write-Host "[SKIP] Solution already exists ($solutionFile)." -ForegroundColor Yellow
} else {
    dotnet new sln -n Escola
    $solutionFile = if (Test-Path "Escola.sln") { "Escola.sln" } else { "Escola.slnx" }
    Write-Host "[OK] Created $solutionFile" -ForegroundColor Green
}
Write-Host ""

# STEP 2: Create Projects
Write-Host "STEP 2: Creating Projects..." -ForegroundColor Magenta

# Domain (Class Library - Core)
if (Test-Path "Escola.Domain") {
    Write-Host "[SKIP] Escola.Domain already exists." -ForegroundColor Yellow
} else {
    dotnet new classlib -n Escola.Domain -f $DotNetVersion
    Write-Host "[OK] Created Escola.Domain (Class Library)" -ForegroundColor Green
}

# Application (Class Library - Business Logic)
if (Test-Path "Escola.Application") {
    Write-Host "[SKIP] Escola.Application already exists." -ForegroundColor Yellow
} else {
    dotnet new classlib -n Escola.Application -f $DotNetVersion
    Write-Host "[OK] Created Escola.Application (Class Library)" -ForegroundColor Green
}

# Infrastructure (Class Library - Data Access)
if (Test-Path "Escola.Infrastructure") {
    Write-Host "[SKIP] Escola.Infrastructure already exists." -ForegroundColor Yellow
} else {
    dotnet new classlib -n Escola.Infrastructure -f $DotNetVersion
    Write-Host "[OK] Created Escola.Infrastructure (Class Library)" -ForegroundColor Green
}

# Api (Web API - Entry Point)
if (Test-Path "Escola.Api") {
    Write-Host "[SKIP] Escola.Api already exists." -ForegroundColor Yellow
} else {
    dotnet new webapi -n Escola.Api -f $DotNetVersion --no-https --use-controllers
    Write-Host "[OK] Created Escola.Api (Web API)" -ForegroundColor Green
}

Write-Host ""

# STEP 3: Establish Project References
Write-Host "STEP 3: Establishing Project References..." -ForegroundColor Magenta

# Application -> Domain
dotnet add Escola.Application/Escola.Application.csproj reference Escola.Domain/Escola.Domain.csproj
Write-Host "[OK] Application -> Domain" -ForegroundColor Green

# Infrastructure -> Application, Domain
dotnet add Escola.Infrastructure/Escola.Infrastructure.csproj reference Escola.Application/Escola.Application.csproj
dotnet add Escola.Infrastructure/Escola.Infrastructure.csproj reference Escola.Domain/Escola.Domain.csproj
Write-Host "[OK] Infrastructure -> Application, Domain" -ForegroundColor Green

# Api -> Application, Infrastructure
dotnet add Escola.Api/Escola.Api.csproj reference Escola.Application/Escola.Application.csproj
dotnet add Escola.Api/Escola.Api.csproj reference Escola.Infrastructure/Escola.Infrastructure.csproj
Write-Host "[OK] Api -> Application, Infrastructure" -ForegroundColor Green

Write-Host ""

# STEP 4: Add Projects to Solution
Write-Host "STEP 4: Adding Projects to Solution..." -ForegroundColor Magenta

dotnet sln $solutionFile add Escola.Domain/Escola.Domain.csproj
dotnet sln $solutionFile add Escola.Application/Escola.Application.csproj
dotnet sln $solutionFile add Escola.Infrastructure/Escola.Infrastructure.csproj
dotnet sln $solutionFile add Escola.Api/Escola.Api.csproj

Write-Host "[OK] All projects added to solution" -ForegroundColor Green
Write-Host ""

# STEP 5: Create Folder Structure
Write-Host "STEP 5: Creating Folder Structure..." -ForegroundColor Magenta

# Domain folders (match database schemas)
$domainFolders = @(
    "Escola.Domain/Common",
    "Escola.Domain/Identity",
    "Escola.Domain/Assets",
    "Escola.Domain/School",
    "Escola.Domain/Content",
    "Escola.Domain/Game"
)

foreach ($folder in $domainFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}
Write-Host "[OK] Domain folders created (Common, Identity, Assets, School, Content, Game)" -ForegroundColor Green

# Application folders
$applicationFolders = @(
    "Escola.Application/Common",
    "Escola.Application/Interfaces",
    "Escola.Application/DTOs",
    "Escola.Application/UseCases"
)

foreach ($folder in $applicationFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}
Write-Host "[OK] Application folders created (Common, Interfaces, DTOs, UseCases)" -ForegroundColor Green

# Infrastructure folders
$infrastructureFolders = @(
    "Escola.Infrastructure/Persistence",
    "Escola.Infrastructure/Services"
)

foreach ($folder in $infrastructureFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}
Write-Host "[OK] Infrastructure folders created (Persistence, Services)" -ForegroundColor Green

# Api folders
$apiFolders = @(
    "Escola.Api/Controllers",
    "Escola.Api/Middlewares"
)

foreach ($folder in $apiFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}
Write-Host "[OK] Api folders created (Controllers, Middlewares)" -ForegroundColor Green

Write-Host ""

# STEP 6: Cleanup Default Files
Write-Host "STEP 6: Cleaning Up Default Files..." -ForegroundColor Magenta

# Remove default Class1.cs files
$defaultFiles = @(
    "Escola.Domain/Class1.cs",
    "Escola.Application/Class1.cs",
    "Escola.Infrastructure/Class1.cs"
)

foreach ($file in $defaultFiles) {
    if (Test-Path $file) {
        Remove-Item $file -Force
        Write-Host "[OK] Removed $file" -ForegroundColor Green
    }
}

# Remove default API files (we'll recreate them)
$apiDefaultFiles = @(
    "Escola.Api/WeatherForecast.cs",
    "Escola.Api/Controllers/WeatherForecastController.cs"
)

foreach ($file in $apiDefaultFiles) {
    if (Test-Path $file) {
        Remove-Item $file -Force
        Write-Host "[OK] Removed $file" -ForegroundColor Green
    }
}

Write-Host ""

# STEP 7: Verification
Write-Host "STEP 7: Verifying Solution..." -ForegroundColor Magenta
dotnet build $solutionFile --nologo --verbosity quiet

if ($LASTEXITCODE -eq 0) {
    Write-Host "[OK] Solution builds successfully!" -ForegroundColor Green
} else {
    Write-Host "[WARN] Build had warnings/errors. Check output above." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Solution Initialized Successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Solution Structure:" -ForegroundColor Yellow
Write-Host "  src/" -ForegroundColor Cyan
Write-Host "     $solutionFile" -ForegroundColor White
Write-Host "     Escola.Domain        (Core - Zero dependencies)" -ForegroundColor White
Write-Host "     Escola.Application   (Business Logic)" -ForegroundColor White
Write-Host "     Escola.Infrastructure (Data Access - EF Core)" -ForegroundColor White
Write-Host "     Escola.Api            (Web API - Entry Point)" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Open solution: code src/$solutionFile" -ForegroundColor White
Write-Host "  2. Install NuGet packages (EF Core, Dapper, FluentValidation, etc.)" -ForegroundColor White
Write-Host "  3. Create domain entities in Escola.Domain" -ForegroundColor White
Write-Host "  4. Set up DbContext in Escola.Infrastructure" -ForegroundColor White
Write-Host "  5. Configure dependency injection in Escola.Api/Program.cs" -ForegroundColor White
Write-Host ""

# Return to root directory
Set-Location $RootDir
