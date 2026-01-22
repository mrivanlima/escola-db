# Escola Platform

K-12 Educational Platform with gamification for Brazilian Portuguese market.

## 🎯 Overview

A complete educational platform designed for K-12 students, featuring:
- Multi-tenant architecture (schools and individual users)
- Gamification with badges and progress tracking
- Content management (modules, activities, assets)
- Student/guardian/teacher management
- Brazilian Portuguese support with accent-insensitive search

## 📊 Database Architecture

### Technology Stack
- **Database**: PostgreSQL 17.6 (Supabase)
- **Extensions**: uuid-ossp, pgcrypto, unaccent
- **Key Features**: 
  - Hybrid ID strategy (Internal INT + External UUID)
  - Row Level Security (RLS) for multi-tenancy
  - Text normalization for accent-insensitive search
  - Full audit trails on all tables
  - Soft delete support

### Schema Organization

**6 Schemas | 17 Tables | 22 Normalized Columns**

```
identity/     - Multi-tenant and user management
  ├── tenants
  └── app_users

assets/       - Media file management
  └── media_files

school/       - Educational entities
  ├── students
  ├── guardians
  ├── student_guardians
  ├── classes
  ├── teachers
  └── class_students

content/      - Learning content
  ├── modules
  ├── activities
  └── assets

game/         - Gamification
  ├── student_progress
  ├── badges
  └── student_badges

audit/        - System logs (future)
```

### Key Design Patterns

#### Hybrid ID Strategy
All entities have dual identifiers:
- **Internal ID**: Integer (e.g., `student_id`) - Never exposed to API
- **External UUID**: UUID (e.g., `student_uuid`) - Used in frontend/API

#### Text Normalization
All TEXT columns have corresponding `*_normalized` columns:
```sql
first_name              TEXT NOT NULL
first_name_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(first_name))) STORED
```

This enables accent-insensitive search:
```sql
-- Search "jose" matches "José"
SELECT * FROM students 
WHERE first_name_normalized LIKE '%jose%';
```

#### Audit Trail
Every table includes:
- `created_at` / `created_by`
- `updated_at` / `updated_by`
- `deleted_at` (soft delete)

## 🚀 Getting Started

### Prerequisites
- PostgreSQL 17+ or Supabase account
- PowerShell (for build scripts)
- Python 3.8+ (optional, for diagram generation)

### Database Setup

1. **Configure connection** (create `db_config.ps1`):
```powershell
$env:PGHOST = "your-host.supabase.com"
$env:PGPORT = "5432"
$env:PGUSER = "postgres.yourproject"
$env:PGDATABASE = "postgres"
$env:PGPASSWORD = "your-password"
```

2. **Run build script**:
```powershell
.\quick_build.ps1
```

This will:
- Create all schemas and extensions
- Create all 17 tables with constraints, indexes, and RLS
- Set up triggers for audit trails
- Add text normalization columns

3. **Optional: Seed test data**:
```powershell
psql -f database/seed_data.sql
```

### Project Structure

```
Escola/
├── database/
│   ├── shared/
│   │   ├── init_schemas.sql
│   │   └── functions/
│   │       ├── handle_updated_at.sql
│   │       └── immutable_unaccent.sql
│   ├── identity/tables/
│   ├── assets/tables/
│   ├── school/tables/
│   ├── content/tables/
│   ├── game/tables/
│   ├── apply_normalization.sql
│   ├── seed_data.sql
│   └── build_all.sql
├── sqldocs/
│   ├── ARCHITECTURE.md
│   ├── DB_GENERATION_RULES.md
│   ├── database_diagram.md
│   ├── database_schema_diagram.pdf
│   └── database_schema.dot
├── build_db.ps1
├── quick_build.ps1
└── generate_db_diagram.py
```

## 📖 Documentation

- **[ARCHITECTURE.md](sqldocs/ARCHITECTURE.md)** - Complete architectural overview
- **[DB_GENERATION_RULES.md](sqldocs/DB_GENERATION_RULES.md)** - Database coding standards
- **[database_schema_diagram.pdf](sqldocs/database_schema_diagram.pdf)** - Visual database diagram
- **[database_diagram.md](sqldocs/database_diagram.md)** - Mermaid ERD diagram

## 🔍 Key Features

### Multi-Tenancy
- Complete tenant isolation via Row Level Security (RLS)
- Shared content tables (modules, activities, badges)
- Per-tenant media storage

### Brazilian Portuguese Support
- Accent-insensitive search (José → jose, João → joao)
- 22 normalized columns across all tables
- Optimized indexes for search performance

### Gamification
- Badge system with rarity levels
- Progress tracking per activity
- Points and rewards
- Achievement unlocking criteria

### Security
- Row Level Security (RLS) on all tenant-scoped tables
- Soft delete for data recovery
- Audit trail on all operations
- Integration with Supabase Auth

## 🛠️ Build Scripts

### `quick_build.ps1`
Optimized single-connection build:
- Executes all DDL in order
- Creates schemas, functions, tables, triggers, indexes, RLS
- Idempotent (safe to run multiple times)

### `build_db.ps1`
Legacy multi-file build (use `quick_build.ps1` instead)

### `apply_normalization.sql`
Adds normalized columns to all TEXT fields:
- Evolutionary approach (safe to run on existing database)
- Creates generated columns with indexes
- Idempotent

## 📊 Database Statistics

- **Tables**: 17
- **Schemas**: 6 (identity, assets, school, content, game, audit)
- **Normalized Columns**: 22
- **Extensions**: 3 (uuid-ossp, pgcrypto, unaccent)
- **Indexes**: 60+
- **Foreign Keys**: 25+

## 🔄 Development Workflow

### Adding New Tables
Follow patterns in `DB_GENERATION_RULES.md`:
1. Use `CREATE TABLE IF NOT EXISTS`
2. Add evolutionary changes via `DO $$ ... END $$` blocks
3. Include normalized columns for TEXT fields
4. Add indexes, triggers, RLS policies
5. Document with COMMENT statements

### Schema Changes
Always use evolutionary approach:
```sql
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name='mytable' AND column_name='new_column') THEN
        ALTER TABLE mytable ADD COLUMN new_column TEXT;
    END IF;
END $$;
```

## 📝 License

[Specify your license here]

## 👥 Contributing

[Add contribution guidelines if applicable]

## 📧 Contact

[Add contact information]

---

**Note**: The `db_config.ps1` file containing database credentials is excluded from version control. Create your own based on your Supabase or PostgreSQL setup.
