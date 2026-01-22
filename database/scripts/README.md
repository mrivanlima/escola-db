# Database Scripts

This folder contains all database management scripts for the Escola Platform.

## Scripts Overview

### PowerShell Scripts

#### `build_db.ps1`
Builds the database schema by executing SQL files in the correct order.

**Usage:**
```powershell
# Run from any location
.\database\scripts\build_db.ps1

# Or with explicit connection parameters
.\database\scripts\build_db.ps1 -Server "localhost" -Port "5432" -Database "escola" -Username "postgres" -Password "yourpass"
```

#### `quick_build.ps1`
Fast database build using the consolidated `build_all.sql` file.

**Usage:**
```powershell
.\database\scripts\quick_build.ps1
```

#### `rebuild_db.ps1`
Complete database rebuild - drops all schemas and recreates everything with seed data.

**⚠️ WARNING:** This script will destroy all existing data!

**Usage:**
```powershell
.\database\scripts\rebuild_db.ps1
```

#### `run_query.ps1`
Execute ad-hoc SQL queries with proper UTF-8 encoding.

**Usage:**
```powershell
.\database\scripts\run_query.ps1 -Query "SELECT * FROM identity.tenants"
```

#### `db_config.ps1`
Database connection configuration file.

**⚠️ SECURITY:** Contains sensitive credentials. Never commit to Git!

### Python Scripts

#### `generate_db_diagram.py`
Generates a PDF visualization of the database schema.

**Usage:**
```powershell
# From project root
python .\database\scripts\generate_db_diagram.py
```

**Output:** Creates `docs/sqldocs/database_schema_diagram.pdf`

### SQL Scripts

#### `test_normalization.sql`
Test queries for verifying data normalization implementation.

## Notes

- All scripts have been updated to work from the `database/scripts` location
- Scripts use relative paths to find SQL files in the parent database folder
- The `db_config.ps1` file is sourced automatically by PowerShell scripts
