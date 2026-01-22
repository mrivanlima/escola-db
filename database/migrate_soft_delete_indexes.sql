-- =====================================================
-- MIGRATION: Soft Delete Aware Unique Indexes
-- Description: Replace UNIQUE constraints with partial indexes for soft delete support
-- =====================================================

-- Start transaction
BEGIN;

-- =====================================================
-- STEP 1: RENAME content.assets to content.activity_resources
-- =====================================================

-- Check if old table exists
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables 
               WHERE table_schema='content' AND table_name='assets') THEN
        
        -- Rename table
        ALTER TABLE content.assets RENAME TO activity_resources;
        
        -- Rename sequence
        ALTER SEQUENCE content.assets_asset_id_seq RENAME TO activity_resources_resource_id_seq;
        
        -- Rename constraints
        ALTER TABLE content.activity_resources RENAME CONSTRAINT pk_assets TO pk_activity_resources;
        ALTER TABLE content.activity_resources RENAME CONSTRAINT uq_assets_uuid TO uq_activity_resources_uuid;
        ALTER TABLE content.activity_resources RENAME CONSTRAINT fk_assets_created TO fk_activity_resources_created;
        ALTER TABLE content.activity_resources RENAME CONSTRAINT ck_assets_name TO ck_activity_resources_name;
        ALTER TABLE content.activity_resources RENAME CONSTRAINT ck_assets_type TO ck_activity_resources_type;
        ALTER TABLE content.activity_resources RENAME CONSTRAINT ck_assets_file_size TO ck_activity_resources_file_size;
        
        -- Rename indexes
        ALTER INDEX content.idx_assets_type RENAME TO idx_activity_resources_type;
        ALTER INDEX content.idx_assets_name_normalized RENAME TO idx_activity_resources_name_normalized;
        ALTER INDEX content.idx_assets_alt_text_normalized RENAME TO idx_activity_resources_alt_text_normalized;
        ALTER INDEX content.idx_assets_published RENAME TO idx_activity_resources_published;
        ALTER INDEX content.idx_assets_deleted_at RENAME TO idx_activity_resources_deleted_at;
        
        -- Rename columns
        ALTER TABLE content.activity_resources RENAME COLUMN asset_id TO resource_id;
        ALTER TABLE content.activity_resources RENAME COLUMN asset_uuid TO resource_uuid;
        ALTER TABLE content.activity_resources RENAME COLUMN asset_name TO resource_name;
        ALTER TABLE content.activity_resources RENAME COLUMN asset_name_normalized TO resource_name_normalized;
        ALTER TABLE content.activity_resources RENAME COLUMN asset_type TO resource_type;
        ALTER TABLE content.activity_resources RENAME COLUMN asset_metadata TO resource_metadata;
        
        RAISE NOTICE 'Renamed content.assets to content.activity_resources';
    END IF;
END $$;

-- =====================================================
-- STEP 2: Add media_file_id FK (replace file_url)
-- =====================================================

DO $$ 
BEGIN
    -- Add media_file_id if not exists
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='activity_resources' AND column_name='media_file_id') THEN
        ALTER TABLE content.activity_resources ADD COLUMN media_file_id UUID;
        
        -- Add FK constraint
        ALTER TABLE content.activity_resources 
            ADD CONSTRAINT fk_activity_resources_media_file 
            FOREIGN KEY (media_file_id) REFERENCES assets.media_files(file_id);
        
        -- Create index
        CREATE INDEX idx_activity_resources_media_file ON content.activity_resources(media_file_id);
        
        RAISE NOTICE 'Added media_file_id FK to activity_resources';
    END IF;
    
    -- Drop file_url if exists
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema='content' AND table_name='activity_resources' AND column_name='file_url') THEN
        ALTER TABLE content.activity_resources DROP COLUMN file_url;
        RAISE NOTICE 'Removed file_url from activity_resources';
    END IF;
END $$;

-- =====================================================
-- STEP 3: Replace UNIQUE constraints with Partial Indexes
-- =====================================================

-- identity.tenants
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_tenants_uuid') THEN
        ALTER TABLE identity.tenants DROP CONSTRAINT uq_tenants_uuid;
        CREATE UNIQUE INDEX idx_tenants_uuid_active ON identity.tenants(tenant_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated tenants uuid constraint';
    END IF;
END $$;

-- identity.app_users
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_app_users_uuid') THEN
        ALTER TABLE identity.app_users DROP CONSTRAINT uq_app_users_uuid;
        CREATE UNIQUE INDEX idx_app_users_uuid_active ON identity.app_users(user_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated app_users uuid constraint';
    END IF;
    
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_app_users_auth') THEN
        ALTER TABLE identity.app_users DROP CONSTRAINT uq_app_users_auth;
        CREATE UNIQUE INDEX idx_app_users_auth_active ON identity.app_users(auth_user_id) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated app_users auth constraint';
    END IF;
    
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_app_users_email_tenant') THEN
        ALTER TABLE identity.app_users DROP CONSTRAINT uq_app_users_email_tenant;
        CREATE UNIQUE INDEX idx_app_users_email_tenant_active ON identity.app_users(email, tenant_id) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated app_users email constraint';
    END IF;
END $$;

-- school.students
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_students_uuid') THEN
        ALTER TABLE school.students DROP CONSTRAINT uq_students_uuid;
        CREATE UNIQUE INDEX idx_students_uuid_active ON school.students(student_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated students uuid constraint';
    END IF;
END $$;

-- school.guardians
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_guardians_uuid') THEN
        ALTER TABLE school.guardians DROP CONSTRAINT uq_guardians_uuid;
        CREATE UNIQUE INDEX idx_guardians_uuid_active ON school.guardians(guardian_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated guardians uuid constraint';
    END IF;
    
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_guardians_user') THEN
        ALTER TABLE school.guardians DROP CONSTRAINT uq_guardians_user;
        CREATE UNIQUE INDEX idx_guardians_user_active ON school.guardians(user_id) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated guardians user constraint';
    END IF;
END $$;

-- school.classes
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_classes_uuid') THEN
        ALTER TABLE school.classes DROP CONSTRAINT uq_classes_uuid;
        CREATE UNIQUE INDEX idx_classes_uuid_active ON school.classes(class_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated classes uuid constraint';
    END IF;
END $$;

-- school.teachers
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_teachers_uuid') THEN
        ALTER TABLE school.teachers DROP CONSTRAINT uq_teachers_uuid;
        CREATE UNIQUE INDEX idx_teachers_uuid_active ON school.teachers(teacher_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated teachers uuid constraint';
    END IF;
    
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_teachers_user') THEN
        ALTER TABLE school.teachers DROP CONSTRAINT uq_teachers_user;
        CREATE UNIQUE INDEX idx_teachers_user_active ON school.teachers(user_id) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated teachers user constraint';
    END IF;
END $$;

-- school.student_guardians
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_student_guardians_pair') THEN
        ALTER TABLE school.student_guardians DROP CONSTRAINT uq_student_guardians_pair;
        CREATE UNIQUE INDEX idx_student_guardians_pair_active ON school.student_guardians(student_id, guardian_id) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated student_guardians pair constraint';
    END IF;
END $$;

-- content.modules
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_modules_uuid') THEN
        ALTER TABLE content.modules DROP CONSTRAINT uq_modules_uuid;
        CREATE UNIQUE INDEX idx_modules_uuid_active ON content.modules(module_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated modules uuid constraint';
    END IF;
END $$;

-- content.activities
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_activities_uuid') THEN
        ALTER TABLE content.activities DROP CONSTRAINT uq_activities_uuid;
        CREATE UNIQUE INDEX idx_activities_uuid_active ON content.activities(activity_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated activities uuid constraint';
    END IF;
END $$;

-- content.activity_resources (previously assets)
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_activity_resources_uuid') THEN
        ALTER TABLE content.activity_resources DROP CONSTRAINT uq_activity_resources_uuid;
        CREATE UNIQUE INDEX idx_activity_resources_uuid_active ON content.activity_resources(resource_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated activity_resources uuid constraint';
    END IF;
END $$;

-- game.student_progress
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_student_progress_uuid') THEN
        ALTER TABLE game.student_progress DROP CONSTRAINT uq_student_progress_uuid;
        CREATE UNIQUE INDEX idx_student_progress_uuid_active ON game.student_progress(progress_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated student_progress uuid constraint';
    END IF;
END $$;

-- game.badges
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'uq_badges_uuid') THEN
        ALTER TABLE game.badges DROP CONSTRAINT uq_badges_uuid;
        CREATE UNIQUE INDEX idx_badges_uuid_active ON game.badges(badge_uuid) WHERE deleted_at IS NULL;
        RAISE NOTICE 'Updated badges uuid constraint';
    END IF;
END $$;

-- Commit transaction
COMMIT;

-- Summary
SELECT 'Migration completed successfully!' AS status,
       'Renamed content.assets to activity_resources' AS change_1,
       'Added media_file_id FK, removed file_url' AS change_2,
       'Converted all UNIQUE constraints to partial indexes' AS change_3;
