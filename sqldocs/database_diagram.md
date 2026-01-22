# Database Schema Diagram - Escola Platform

## Entity Relationship Diagram

```mermaid
erDiagram
    %% ========================================
    %% IDENTITY SCHEMA
    %% ========================================
    
    tenants {
        INTEGER tenant_id PK
        UUID tenant_uuid UK
        TEXT tenant_name
        TEXT tenant_name_normalized
        TEXT tenant_type
        JSONB tenant_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    app_users {
        INTEGER user_id PK
        UUID user_uuid UK
        INTEGER tenant_id FK
        UUID auth_user_id UK
        TEXT full_name
        TEXT full_name_normalized
        TEXT email
        TEXT email_normalized
        TEXT user_role
        BOOLEAN is_active
        JSONB user_config
        TIMESTAMPTZ created_at
        INTEGER created_by
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    %% ========================================
    %% ASSETS SCHEMA
    %% ========================================
    
    media_files {
        UUID file_id PK
        INTEGER tenant_id FK
        TEXT storage_path
        TEXT original_name
        TEXT original_name_normalized
        TEXT mime_type
        BIGINT size_bytes
        TEXT alt_text
        TEXT alt_text_normalized
        JSONB metadata
        TIMESTAMPTZ created_at
        INTEGER created_by
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    %% ========================================
    %% SCHOOL SCHEMA
    %% ========================================
    
    students {
        INTEGER student_id PK
        UUID student_uuid UK
        INTEGER tenant_id FK
        TEXT nickname
        TEXT nickname_normalized
        TEXT first_name
        TEXT first_name_normalized
        TEXT middle_name
        TEXT middle_name_normalized
        TEXT last_name
        TEXT last_name_normalized
        DATE birth_date
        JSONB avatar_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    guardians {
        INTEGER guardian_id PK
        UUID guardian_uuid UK
        INTEGER tenant_id FK
        INTEGER user_id FK
        TEXT phone_number
        TEXT relationship
        BOOLEAN is_primary
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    student_guardians {
        INTEGER student_guardian_id PK
        INTEGER student_id FK
        INTEGER guardian_id FK
        INTEGER tenant_id FK
        TEXT relationship_notes
        TEXT relationship_notes_normalized
        BOOLEAN is_authorized_pickup
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    classes {
        INTEGER class_id PK
        UUID class_uuid UK
        INTEGER tenant_id FK
        TEXT class_name
        TEXT class_name_normalized
        TEXT grade_level
        TEXT grade_level_normalized
        TEXT school_year
        TEXT school_year_normalized
        INTEGER max_students
        JSONB class_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    teachers {
        INTEGER teacher_id PK
        UUID teacher_uuid UK
        INTEGER tenant_id FK
        INTEGER user_id FK
        TEXT specialization
        TEXT specialization_normalized
        DATE hire_date
        JSONB teacher_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    class_students {
        INTEGER class_student_id PK
        INTEGER class_id FK
        INTEGER student_id FK
        INTEGER tenant_id FK
        DATE enrollment_date
        TEXT status
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    %% ========================================
    %% CONTENT SCHEMA
    %% ========================================
    
    modules {
        INTEGER module_id PK
        UUID module_uuid UK
        TEXT module_name
        TEXT module_name_normalized
        TEXT description
        TEXT description_normalized
        TEXT module_type
        INTEGER difficulty_level
        INTEGER recommended_age_min
        INTEGER recommended_age_max
        INTEGER display_order
        TEXT thumbnail_url
        JSONB module_config
        BOOLEAN is_published
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    activities {
        INTEGER activity_id PK
        UUID activity_uuid UK
        INTEGER module_id FK
        TEXT activity_name
        TEXT activity_name_normalized
        TEXT description
        TEXT description_normalized
        TEXT activity_type
        INTEGER display_order
        INTEGER estimated_duration
        INTEGER points_reward
        JSONB activity_data
        TEXT thumbnail_url
        BOOLEAN is_published
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    activity_resources {
        INTEGER resource_id PK
        UUID resource_uuid UK
        INTEGER activity_id FK
        TEXT resource_name
        TEXT resource_name_normalized
        TEXT resource_type
        UUID media_file_id FK
        INTEGER display_order
        BOOLEAN is_required
        TEXT usage_context
        JSONB resource_config
        BOOLEAN is_published
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    %% ========================================
    %% GAME SCHEMA
    %% ========================================
    
    student_progress {
        INTEGER progress_id PK
        UUID progress_uuid UK
        INTEGER student_id FK
        INTEGER activity_id FK
        INTEGER tenant_id FK
        TEXT status
        INTEGER score
        INTEGER attempts
        INTEGER time_spent_seconds
        JSONB progress_data
        DATE completed_at
        TIMESTAMPTZ created_at
        TIMESTAMPTZ updated_at
        TIMESTAMPTZ deleted_at
    }
    
    badges {
        INTEGER badge_id PK
        UUID badge_uuid UK
        TEXT badge_name
        TEXT badge_name_normalized
        TEXT description
        TEXT description_normalized
        TEXT badge_type
        TEXT icon_url
        TEXT rarity
        INTEGER points_value
        JSONB unlock_criteria
        INTEGER display_order
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by
        TIMESTAMPTZ deleted_at
    }
    
    student_badges {
        INTEGER student_badge_id PK
        INTEGER student_id FK
        INTEGER badge_id FK
        INTEGER tenant_id FK
        DATE earned_at
        JSONB earn_metadata
        TIMESTAMPTZ created_at
        TIMESTAMPTZ deleted_at
    }
    
    %% ========================================
    %% RELATIONSHIPS
    %% ========================================
    
    %% Identity relationships
    tenants ||--o{ app_users : "has"
    tenants ||--o{ media_files : "owns"
    tenants ||--o{ students : "has"
    tenants ||--o{ guardians : "has"
    tenants ||--o{ classes : "has"
    tenants ||--o{ teachers : "has"
    tenants ||--o{ student_progress : "has"
    
    %% School relationships
    app_users ||--o{ students : "created_by"
    app_users ||--o{ guardians : "is"
    app_users ||--o{ teachers : "is"
    
    students ||--o{ student_guardians : "has"
    guardians ||--o{ student_guardians : "has"
    
    students ||--o{ class_students : "enrolled_in"
    classes ||--o{ class_students : "has"
    
    students ||--o{ student_progress : "tracks"
    students ||--o{ student_badges : "earned"
    
    %% Content relationships
    modules ||--o{ activities : "contains"
    activities ||--o{ activity_resources : "uses"
    activities ||--o{ student_progress : "tracked_by"
    activity_resources }o--|| media_files : "references"
    
    %% Game relationships
    badges ||--o{ student_badges : "awarded_as"
```

## Schema Summary

### Identity Schema
- **tenants**: Multi-tenant root (schools/organizations)
- **app_users**: Application users (parents, teachers, admins)

### Assets Schema
- **media_files**: Central media library (images, audio, videos, documents)

### School Schema
- **students**: Child profiles
- **guardians**: Parents/legal guardians
- **student_guardians**: M:N relationship between students and guardians
- **classes**: School classes/groups
- **teachers**: Teacher profiles
- **class_students**: Class enrollment tracking

### Content Schema
- **modules**: Learning modules/courses
- **activities**: Individual learning activities
- **activity_resources**: Links activities to media files (replaces old assets table)

### Game Schema
- **student_progress**: Activity completion tracking
- **badges**: Achievement badges configuration
- **student_badges**: Earned badges by students

## Key Features

### Hybrid ID Strategy
- **Internal ID**: Integer (student_id, teacher_id, etc.) - Database use only
- **External UUID**: UUID (student_uuid, teacher_uuid, etc.) - API/Frontend exposure

### Text Normalization
All TEXT columns have corresponding `*_normalized` columns:
- Lowercase conversion
- Accent removal (unaccent)
- Optimized for Portuguese search (José → jose)
- Indexed for performance

### Audit Trail
All tables include:
- created_at, created_by
- updated_at, updated_by
- deleted_at (soft delete)

### Multi-Tenancy
- RLS (Row Level Security) enabled
- Tenant isolation via tenant_id
- Content tables shared across tenants

## Database Statistics
- **Schemas**: 6 (identity, assets, school, content, game, audit)
- **Tables**: 17
- **Normalized Columns**: 22
- **Extensions**: uuid-ossp, pgcrypto, unaccent
