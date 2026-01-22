-- =====================================================
-- MIGRATION: Soft Delete Unique Indexes
-- Description: Replace UNIQUE constraints with partial indexes for soft delete support
-- Reason: Allows re-using emails/UUIDs after soft delete (deleted_at IS NOT NULL)
-- =====================================================

-- ========================================
-- 1. IDENTITY SCHEMA
-- ========================================

-- identity.tenants
ALTER TABLE identity.tenants DROP CONSTRAINT IF EXISTS uq_tenants_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_tenants_uuid_active ON identity.tenants(tenant_uuid) WHERE deleted_at IS NULL;

-- identity.app_users
ALTER TABLE identity.app_users DROP CONSTRAINT IF EXISTS uq_app_users_uuid;
ALTER TABLE identity.app_users DROP CONSTRAINT IF EXISTS uq_app_users_auth;
ALTER TABLE identity.app_users DROP CONSTRAINT IF EXISTS uq_app_users_email_tenant;

CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_uuid_active ON identity.app_users(user_uuid) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_auth_active ON identity.app_users(auth_user_id) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_email_tenant_active ON identity.app_users(email, tenant_id) WHERE deleted_at IS NULL;

-- ========================================
-- 2. SCHOOL SCHEMA
-- ========================================

-- school.students
ALTER TABLE school.students DROP CONSTRAINT IF EXISTS uq_students_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_students_uuid_active ON school.students(student_uuid) WHERE deleted_at IS NULL;

-- school.guardians
ALTER TABLE school.guardians DROP CONSTRAINT IF EXISTS uq_guardians_uuid;
ALTER TABLE school.guardians DROP CONSTRAINT IF EXISTS uq_guardians_user;
CREATE UNIQUE INDEX IF NOT EXISTS idx_guardians_uuid_active ON school.guardians(guardian_uuid) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_guardians_user_active ON school.guardians(user_id) WHERE deleted_at IS NULL;

-- school.teachers
ALTER TABLE school.teachers DROP CONSTRAINT IF EXISTS uq_teachers_uuid;
ALTER TABLE school.teachers DROP CONSTRAINT IF EXISTS uq_teachers_user;
CREATE UNIQUE INDEX IF NOT EXISTS idx_teachers_uuid_active ON school.teachers(teacher_uuid) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_teachers_user_active ON school.teachers(user_id) WHERE deleted_at IS NULL;

-- school.classes
ALTER TABLE school.classes DROP CONSTRAINT IF EXISTS uq_classes_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_classes_uuid_active ON school.classes(class_uuid) WHERE deleted_at IS NULL;

-- school.student_guardians (no UUID, no unique constraints to update)
-- school.class_students (no UUID, no unique constraints to update)

-- ========================================
-- 3. CONTENT SCHEMA
-- ========================================

-- content.modules
ALTER TABLE content.modules DROP CONSTRAINT IF EXISTS uq_modules_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_modules_uuid_active ON content.modules(module_uuid) WHERE deleted_at IS NULL;

-- content.activities
ALTER TABLE content.activities DROP CONSTRAINT IF EXISTS uq_activities_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_activities_uuid_active ON content.activities(activity_uuid) WHERE deleted_at IS NULL;

-- content.assets (old table - will be replaced by activity_resources)
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='content' AND table_name='assets') THEN
        ALTER TABLE content.assets DROP CONSTRAINT IF EXISTS uq_assets_uuid;
        CREATE UNIQUE INDEX IF NOT EXISTS idx_assets_uuid_active ON content.assets(asset_uuid) WHERE deleted_at IS NULL;
    END IF;
END $$;

-- ========================================
-- 4. GAME SCHEMA
-- ========================================

-- game.badges
ALTER TABLE game.badges DROP CONSTRAINT IF EXISTS uq_badges_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_badges_uuid_active ON game.badges(badge_uuid) WHERE deleted_at IS NULL;

-- game.student_progress
ALTER TABLE game.student_progress DROP CONSTRAINT IF EXISTS uq_student_progress_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_student_progress_uuid_active ON game.student_progress(progress_uuid) WHERE deleted_at IS NULL;

-- game.student_badges (no UUID, no unique constraints to update)

-- ========================================
-- 5. ASSETS SCHEMA
-- ========================================

-- assets.media_files (uses file_id as PK, no separate UUID constraint)
-- No changes needed - file_id is the primary key

-- Summary
SELECT 
    'Soft Delete Unique Indexes Applied' AS status,
    '15 tables updated' AS scope,
    'UUIDs and unique business keys now support soft delete reuse' AS result;
