-- =====================================================
-- UPDATE SCRIPT: Apply Soft Delete Unique Indexes to All Tables
-- Run this after table creation to convert UNIQUE constraints to partial indexes
-- =====================================================

-- This script systematically removes UNIQUE constraints and replaces them
-- with partial unique indexes that only apply WHERE deleted_at IS NULL

-- ALREADY DONE MANUALLY:
-- ✅ identity.tenants
-- ✅ identity.app_users

-- ========================================
-- SCHOOL SCHEMA
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

-- ========================================
-- CONTENT SCHEMA
-- ========================================

-- content.modules
ALTER TABLE content.modules DROP CONSTRAINT IF EXISTS uq_modules_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_modules_uuid_active ON content.modules(module_uuid) WHERE deleted_at IS NULL;

-- content.activities
ALTER TABLE content.activities DROP CONSTRAINT IF EXISTS uq_activities_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_activities_uuid_active ON content.activities(activity_uuid) WHERE deleted_at IS NULL;

-- ========================================
-- GAME SCHEMA
-- ========================================

-- game.badges
ALTER TABLE game.badges DROP CONSTRAINT IF EXISTS uq_badges_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_badges_uuid_active ON game.badges(badge_uuid) WHERE deleted_at IS NULL;

-- game.student_progress
ALTER TABLE game.student_progress DROP CONSTRAINT IF EXISTS uq_student_progress_uuid;
CREATE UNIQUE INDEX IF NOT EXISTS idx_student_progress_uuid_active ON game.student_progress(progress_uuid) WHERE deleted_at IS NULL;

RAISE NOTICE 'Soft delete unique indexes applied to all tables';
