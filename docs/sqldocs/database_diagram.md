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
        SMALLINT tenant_type_id FK
        TEXT tenant_name
        TEXT tenant_name_normalized
        JSONB tenant_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    tenant_types {
        SMALLINT type_id PK
        UUID type_uuid UK
        INTEGER tenant_id FK
        TEXT type_code UK
        TEXT type_name
        TEXT description
        INTEGER max_students
        INTEGER max_teachers
        INTEGER max_storage_gb
        BOOLEAN has_analytics
        BOOLEAN has_api_access
        BOOLEAN has_white_label
        TEXT pricing_tier
        TEXT icon_name
        TEXT color_code
        SMALLINT display_order
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
        SMALLINT user_role_id FK
        BOOLEAN is_active
        JSONB user_config
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    user_roles {
        SMALLINT role_id PK
        UUID role_uuid UK
        INTEGER tenant_id FK
        TEXT role_code UK
        TEXT role_name
        TEXT description
        INTEGER permission_level
        BOOLEAN can_manage_students
        BOOLEAN can_manage_content
        BOOLEAN can_manage_users
        BOOLEAN can_manage_tenant
        TEXT icon_name
        TEXT color_code
        SMALLINT display_order
    }
    
    %% ========================================
    %% ASSETS SCHEMA
    %% ========================================
    
    media_files {
        INTEGER file_id PK
        UUID file_uuid UK
        INTEGER tenant_id FK
        SMALLINT mime_type_id FK
        TEXT storage_path
        TEXT original_name
        TEXT original_name_normalized
        BIGINT size_bytes
        TEXT alt_text
        TEXT alt_text_normalized
        JSONB metadata
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    mime_types {
        SMALLINT mime_type_id PK
        UUID mime_type_uuid UK
        INTEGER tenant_id FK
        SMALLINT category_id FK
        TEXT mime_type_code UK
        TEXT mime_type_name
        TEXT file_extension
        INTEGER max_file_size_mb
        TEXT icon_name
        SMALLINT display_order
    }
    
    media_categories {
        SMALLINT category_id PK
        UUID category_uuid UK
        INTEGER tenant_id FK
        TEXT category_code UK
        TEXT category_name
        TEXT description
        TEXT default_icon_name
        INTEGER default_max_size_mb
        SMALLINT display_order
    }
    
    %% ========================================
    %% SCHOOL SCHEMA
    %% ========================================
    
    students {
        INTEGER student_id PK
        UUID student_uuid UK
        INTEGER tenant_id FK
        INTEGER user_id FK
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
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    guardians {
        INTEGER guardian_id PK
        UUID guardian_uuid UK
        INTEGER tenant_id FK
        INTEGER user_id FK
        TEXT phone_number
        SMALLINT relationship_type_id FK
        BOOLEAN is_primary
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    relationship_types {
        SMALLINT relationship_type_id PK
        UUID relationship_uuid UK
        INTEGER tenant_id FK
        TEXT relationship_code UK
        TEXT relationship_name
        TEXT description
        BOOLEAN can_authorize
        BOOLEAN requires_legal_proof
        TEXT icon_name
        SMALLINT display_order
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
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    classes {
        INTEGER class_id PK
        UUID class_uuid UK
        INTEGER tenant_id FK
        TEXT class_name
        TEXT class_name_normalized
        SMALLINT grade_level_id FK
        SMALLINT school_year_id FK
        INTEGER max_students
        JSONB class_config
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    grade_levels {
        SMALLINT grade_level_id PK
        UUID grade_uuid UK
        INTEGER tenant_id FK
        TEXT grade_code UK
        TEXT grade_name
        TEXT description
        INTEGER age_range_min
        INTEGER age_range_max
        SMALLINT display_order
        TEXT icon_name
        TEXT color_code
    }
    
    school_years {
        SMALLINT school_year_id PK
        UUID year_uuid UK
        INTEGER tenant_id FK
        TEXT year_code UK
        TEXT year_name
        TEXT description
        DATE start_date
        DATE end_date
        BOOLEAN is_current
        SMALLINT display_order
    }
    
    teachers {
        INTEGER teacher_id PK
        UUID teacher_uuid UK
        INTEGER tenant_id FK
        INTEGER user_id FK
        INTEGER specialization_id FK
        DATE hire_date
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    specializations {
        INTEGER specialization_id PK
        UUID specialization_uuid UK
        TEXT specialization_name UK
        TEXT description
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    certifications {
        INTEGER certification_id PK
        UUID certification_uuid UK
        TEXT certification_name UK
        TEXT description
        TEXT issuing_organization
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    teacher_certifications {
        INTEGER teacher_id FK
        INTEGER certification_id FK
        DATE obtained_date
        DATE expiry_date
        TEXT credential_number
        TEXT notes
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    subjects {
        INTEGER subject_id PK
        UUID subject_uuid UK
        TEXT subject_name UK
        TEXT description
        SMALLINT grade_level_id FK
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    teacher_subjects {
        INTEGER teacher_id FK
        INTEGER subject_id FK
        SMALLINT proficiency_level_id FK
        INTEGER years_experience
        BOOLEAN is_primary_subject
        TEXT notes
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    proficiency_levels {
        SMALLINT proficiency_level_id PK
        UUID proficiency_uuid UK
        INTEGER tenant_id FK
        TEXT proficiency_code UK
        TEXT proficiency_name
        TEXT description
        INTEGER minimum_years
        TEXT icon_name
        TEXT color_code
        SMALLINT display_order
    }
    
    class_students {
        INTEGER class_student_id PK
        INTEGER class_id FK
        INTEGER student_id FK
        INTEGER tenant_id FK
        DATE enrollment_date
        SMALLINT status_id FK
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    enrollment_statuses {
        SMALLINT status_id PK
        UUID status_uuid UK
        INTEGER tenant_id FK
        TEXT status_code UK
        TEXT status_name
        TEXT description
        BOOLEAN allows_attendance
        BOOLEAN allows_grading
        BOOLEAN is_final_state
        TEXT icon_name
        TEXT color_code
        SMALLINT display_order
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
        SMALLINT module_type_id FK
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
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    module_types {
        SMALLINT module_type_id PK
        UUID module_type_uuid UK
        INTEGER tenant_id FK
        TEXT module_type_code UK
        TEXT module_type_name
        TEXT description
        TEXT icon_name
        TEXT color_code
        SMALLINT display_order
    }
    
    activities {
        INTEGER activity_id PK
        UUID activity_uuid UK
        INTEGER module_id FK
        TEXT activity_name
        TEXT activity_name_normalized
        TEXT description
        TEXT description_normalized
        SMALLINT activity_type_id FK
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
    
    activity_types {
        SMALLINT activity_type_id PK
        UUID activity_type_uuid UK
        INTEGER tenant_id FK
        TEXT activity_type_code UK
        TEXT activity_type_name
        TEXT description
        TEXT icon_name
        INTEGER default_points
        BOOLEAN requires_interaction
        SMALLINT display_order
    }
    
    activity_resources {
        INTEGER resource_id PK
        UUID resource_uuid UK
        INTEGER activity_id FK
        TEXT resource_name
        TEXT resource_name_normalized
        SMALLINT resource_type_id FK
        INTEGER media_file_id FK
        INTEGER display_order
        BOOLEAN is_required
        SMALLINT usage_context_id FK
        JSONB resource_config
        BOOLEAN is_published
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    resource_types {
        SMALLINT resource_type_id PK
        UUID resource_type_uuid UK
        INTEGER tenant_id FK
        TEXT resource_type_code UK
        TEXT resource_type_name
        TEXT description
        JSONB mime_types
        INTEGER max_file_size_mb
        TEXT icon_name
        SMALLINT display_order
    }
    
    usage_contexts {
        SMALLINT usage_context_id PK
        UUID usage_context_uuid UK
        INTEGER tenant_id FK
        TEXT context_code UK
        TEXT context_name
        TEXT description
        TEXT icon_name
        SMALLINT display_order
    }
    
    %% ========================================
    %% GAME SCHEMA
    %% ========================================
    
    student_progress {
        BIGINT progress_id PK
        UUID progress_uuid UK
        INTEGER student_id FK
        INTEGER activity_id FK
        INTEGER tenant_id FK
        SMALLINT status_id FK
        INTEGER score
        INTEGER attempts_count
        INTEGER time_spent_seconds
        JSONB progress_data
        TIMESTAMPTZ completion_date
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    progress_statuses {
        SMALLINT status_id PK
        UUID status_uuid UK
        INTEGER tenant_id FK
        TEXT status_code UK
        TEXT status_name
        TEXT description
        TEXT icon_name
        TEXT color_code
        BOOLEAN is_final_state
        SMALLINT display_order
    }
    
    badges {
        INTEGER badge_id PK
        UUID badge_uuid UK
        TEXT badge_name
        TEXT badge_name_normalized
        TEXT description
        TEXT description_normalized
        SMALLINT badge_type_id FK
        TEXT icon_url
        SMALLINT rarity_id FK
        INTEGER points_value
        JSONB unlock_criteria
        INTEGER display_order
        BOOLEAN is_active
        TIMESTAMPTZ created_at
        INTEGER created_by FK
        TIMESTAMPTZ updated_at
        INTEGER updated_by FK
        TIMESTAMPTZ deleted_at
    }
    
    badge_types {
        SMALLINT badge_type_id PK
        UUID badge_type_uuid UK
        TEXT badge_type_code UK
        TEXT badge_type_name
        TEXT description
        TEXT icon_name
        INTEGER display_order
        BOOLEAN is_active
    }
    
    badge_rarities {
        SMALLINT rarity_id PK
        UUID rarity_uuid UK
        TEXT rarity_code UK
        TEXT rarity_name
        TEXT description
        TEXT color_code
        NUMERIC points_multiplier
        INTEGER display_order
        BOOLEAN is_active
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
    tenants ||--o{ tenant_types : "has_type"
    tenants ||--o{ app_users : "has"
    tenants ||--o{ user_roles : "defines"
    app_users }o--|| user_roles : "has_role"
    
    %% Assets relationships
    tenants ||--o{ media_files : "owns"
    tenants ||--o{ mime_types : "defines"
    tenants ||--o{ media_categories : "defines"
    media_files }o--|| mime_types : "has_type"
    mime_types }o--|| media_categories : "belongs_to"
    
    %% School relationships
    tenants ||--o{ students : "has"
    tenants ||--o{ guardians : "has"
    tenants ||--o{ classes : "has"
    tenants ||--o{ teachers : "has"
    tenants ||--o{ grade_levels : "defines"
    tenants ||--o{ school_years : "defines"
    tenants ||--o{ relationship_types : "defines"
    tenants ||--o{ enrollment_statuses : "defines"
    tenants ||--o{ proficiency_levels : "defines"
    
    app_users ||--o{ students : "created_by"
    app_users ||--o| students : "is_account_for"
    app_users ||--o{ guardians : "is"
    app_users ||--o{ teachers : "is"
    
    students ||--o{ student_guardians : "has"
    guardians ||--o{ student_guardians : "has"
    guardians }o--|| relationship_types : "has_type"
    
    students ||--o{ class_students : "enrolled_in"
    classes ||--o{ class_students : "has"
    classes }o--|| grade_levels : "belongs_to"
    classes }o--|| school_years : "belongs_to"
    class_students }o--|| enrollment_statuses : "has_status"
    
    students ||--o{ student_progress : "tracks"
    students ||--o{ student_badges : "earned"
    
    teachers }o--|| specializations : "has"
    teachers ||--o{ teacher_certifications : "has"
    certifications ||--o{ teacher_certifications : "awarded_to"
    teachers ||--o{ teacher_subjects : "teaches"
    subjects ||--o{ teacher_subjects : "taught_by"
    subjects }o--|| grade_levels : "for_grade"
    teacher_subjects }o--|| proficiency_levels : "with_proficiency"
    
    %% Content relationships
    tenants ||--o{ module_types : "defines"
    tenants ||--o{ activity_types : "defines"
    tenants ||--o{ resource_types : "defines"
    tenants ||--o{ usage_contexts : "defines"
    
    modules ||--o{ activities : "contains"
    modules }o--|| module_types : "has_type"
    activities ||--o{ activity_resources : "uses"
    activities }o--|| activity_types : "has_type"
    activities ||--o{ student_progress : "tracked_by"
    activity_resources }o--|| media_files : "references"
    activity_resources }o--|| resource_types : "has_type"
    activity_resources }o--|| usage_contexts : "used_in"
    
    %% Game relationships
    tenants ||--o{ student_progress : "has"
    tenants ||--o{ progress_statuses : "defines"
    student_progress }o--|| progress_statuses : "has_status"
    badges ||--o{ student_badges : "awarded_as"
    badges }o--|| badge_types : "classified_as"
    badges }o--|| badge_rarities : "has_rarity"
```

## Schema Summary

### Identity Schema (2 tables + 2 lookup tables)
- **tenants**: Multi-tenant root (schools/organizations)
- **tenant_types**: Lookup for tenant classifications (individual, school, enterprise)
- **app_users**: Application users (parents, teachers, admins)
- **user_roles**: Lookup for user roles with hierarchical permissions

### Assets Schema (1 table + 2 lookup tables)
- **media_files**: Central media library (images, audio, videos, documents)
- **mime_types**: Lookup for MIME types with metadata
- **media_categories**: Lookup for media categories (image, video, audio, document)

### School Schema (9 tables + 6 lookup tables)
- **students**: Child profiles (includes optional user_id FK for future hybrid auth model)
- **guardians**: Parents/legal guardians
- **student_guardians**: M:N relationship between students and guardians
- **classes**: School classes/groups
- **teachers**: Teacher profiles
- **class_students**: Class enrollment tracking
- **specializations**: Lookup for teacher specializations
- **certifications**: Lookup for professional certifications
- **teacher_certifications**: Junction table for teacher certifications
- **subjects**: Lookup for teaching subjects
- **teacher_subjects**: Junction table for teacher subjects
- **grade_levels**: Lookup for educational grade levels
- **school_years**: Lookup for academic years
- **relationship_types**: Lookup for guardian-student relationships
- **enrollment_statuses**: Lookup for class enrollment statuses
- **proficiency_levels**: Lookup for teaching proficiency levels

### Content Schema (3 tables + 4 lookup tables)
- **modules**: Learning modules/courses
- **activities**: Individual learning activities
- **activity_resources**: Links activities to media files
- **module_types**: Lookup for module/course types
- **activity_types**: Lookup for activity types (quiz, game, video, etc.)
- **resource_types**: Lookup for resource/media types
- **usage_contexts**: Lookup for resource usage contexts

### Game Schema (3 tables + 3 lookup tables)
- **student_progress**: Activity completion tracking
- **badges**: Achievement badges configuration
- **student_badges**: Earned badges by students
- **progress_statuses**: Lookup for student progress states
- **badge_types**: Lookup for badge classifications (achievement, milestone, special)
- **badge_rarities**: Lookup for badge rarity levels (common, rare, epic, legendary)

## Key Features

### Hybrid ID Strategy
- **Internal ID**: Integer/Smallint (student_id, teacher_id, etc.) - Database use only
- **External UUID**: UUID (student_uuid, teacher_uuid, etc.) - API/Frontend exposure

### Hybrid Authentication Model
- **students.user_id (Nullable FK)**: Optional link to app_users for future scalability
- Allows students to have their own login credentials when needed
- Currently nullable - students are managed through guardian accounts
- Supports transition to direct student authentication without schema changes

### Full Normalization (3NF)
All free-text enums normalized to lookup tables:
- No CHECK constraints with IN clauses
- All lookup tables have metadata (icons, colors, descriptions)
- Consistent pattern: code, name, display_order

### Text Normalization
All TEXT columns have corresponding `*_normalized` columns:
- Lowercase conversion
- Accent removal (unaccent)
- Optimized for Portuguese search (José → jose)
- Indexed for performance

### JSONB Validation
All JSONB columns validated with CHECK constraints:
- `CHECK (column IS NULL OR jsonb_typeof(column) = 'object')`
- `CHECK (column IS NULL OR jsonb_typeof(column) = 'array')`
- Ensures data integrity at database level

### Audit Trail
All tables include:
- created_at, created_by (FK to app_users)
- updated_at, updated_by (FK to app_users)
- deleted_at (soft delete)

### Multi-Tenancy
- RLS (Row Level Security) enabled on all tables
- Tenant isolation via tenant_id
- Lookup tables: some shared globally, some tenant-specific

## Database Statistics
- **Schemas**: 6 (identity, assets, school, content, game, audit)
- **Main Tables**: 18
- **Lookup Tables**: 19
- **Junction Tables**: 4
- **Total Tables**: 41
- **Normalized Columns**: 30+
- **Extensions**: uuid-ossp, pgcrypto, unaccent
