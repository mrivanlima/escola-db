# Database Structure - Project Escola

## Overview
This directory contains the complete SQL schema for the Escola educational platform.

## Structure
```
database/
├── shared/
│   ├── init_schemas.sql          # Schema creation and extensions
│   └── functions/
│       └── handle_updated_at.sql # Trigger function for audit
├── identity/
│   └── tables/
│       ├── tenants.sql           # Multi-tenant root
│       └── app_users.sql         # Application users
├── assets/
│   └── tables/
│       └── media_files.sql       # Media library (uploaded files metadata)
├── school/
│   └── tables/
│       ├── students.sql          # Student profiles
│       ├── guardians.sql         # Parents/Guardians
│       ├── student_guardians.sql # Student-Guardian relationships
│       ├── classes.sql           # School classes
│       ├── teachers.sql          # Teachers
│       └── class_students.sql    # Class-Student relationships
├── content/
│   └── tables/
│       ├── modules.sql           # Learning modules
│       ├── activities.sql        # Learning activities
│       └── assets.sql            # Media assets
└── game/
    └── tables/
        ├── student_progress.sql  # Progress tracking
        ├── badges.sql            # Achievement badges
        └── student_badges.sql    # Student badge collection
```

## Execution

### Quick Start
```powershell
# Run the build script with default settings
.\build_db.ps1

# Or with custom connection details
.\build_db.ps1 -Server "localhost" -Port "5432" -Database "escola_db" -Username "postgres"
```

### Manual Execution
If you prefer to run files individually:
```powershell
psql -h localhost -U postgres -d escola_db -f database/shared/init_schemas.sql
psql -h localhost -U postgres -d escola_db -f database/shared/functions/handle_updated_at.sql
# ... continue with other files in order
```

## Schema Descriptions

### Identity Schema
- **tenants**: Multi-tenant root (Schools/Organizations)
- **app_users**: Application users (Parents, Teachers, Admins) linked to Supabase Auth

### Assets Schema
- **media_files**: Media library for uploaded files (images, audio, videos, documents) - stores metadata only

### School Schema
- **students**: Child profiles managed by parents/schools
- **guardians**: Parents/Legal guardians
- **student_guardians**: M:N relationship between students and guardians
- **classes**: School classes/groups (B2B feature)
- **teachers**: Teachers linked to app_users
- **class_students**: M:N relationship between classes and students

### Content Schema
- **modules**: Learning modules/courses (content hierarchy root)
- **activities**: Individual learning activities within modules
- **assets**: Media assets (images, videos, audio)

### Game Schema
- **student_progress**: High-volume progress tracking through activities
- **badges**: Achievement badges/rewards catalog
- **student_badges**: M:N linking students to earned badges

## Key Features

### Hybrid ID Strategy
- **Internal ID** (`*_id`): Integer for JOINs and FKs - NEVER exposed to API
- **External ID** (`*_uuid`): UUID exposed to Frontend/API

### Row Level Security (RLS)
All tenant-aware tables have RLS policies for multi-tenant isolation.

### Audit Trail
All tables include:
- `created_at` / `created_by`
- `updated_at` / `updated_by` (auto-updated via trigger)
- `deleted_at` (soft delete)

### Idempotency
All scripts use:
- `CREATE TABLE IF NOT EXISTS`
- `CREATE OR REPLACE` for functions/views
- `DROP IF EXISTS` before triggers

## Notes
- Execute scripts in order (dependency chain)
- All constraints are named following convention: `pk_`, `fk_`, `uq_`, `ck_`, `idx_`
- All tables and columns have SQL COMMENTS for documentation
- Content and badge tables do NOT have RLS (shared across tenants)
