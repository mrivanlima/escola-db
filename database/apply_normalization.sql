-- =====================================================
-- Apply Normalization to All TEXT Columns
-- Purpose: Add normalized columns and indexes for accent-insensitive search
-- Run Order: After immutable_unaccent function is created
-- =====================================================

-- 1. identity.app_users
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='identity' AND table_name='app_users' AND column_name='full_name_normalized') THEN
        ALTER TABLE identity.app_users ADD COLUMN full_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(full_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='identity' AND table_name='app_users' AND column_name='email_normalized') THEN
        ALTER TABLE identity.app_users ADD COLUMN email_normalized TEXT GENERATED ALWAYS AS (LOWER(email)) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_app_users_full_name_normalized ON identity.app_users(full_name_normalized);
CREATE INDEX IF NOT EXISTS idx_app_users_email_normalized ON identity.app_users(email_normalized);

-- 2. identity.tenants
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='identity' AND table_name='tenants' AND column_name='tenant_name_normalized') THEN
        ALTER TABLE identity.tenants ADD COLUMN tenant_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(tenant_name))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_tenants_name_normalized ON identity.tenants(tenant_name_normalized);

-- 3. school.students
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='nickname_normalized') THEN
        ALTER TABLE school.students ADD COLUMN nickname_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(nickname))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_students_nickname_normalized ON school.students(nickname_normalized);

-- 4. school.classes
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='classes' AND column_name='class_name_normalized') THEN
        ALTER TABLE school.classes ADD COLUMN class_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(class_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='classes' AND column_name='grade_level_normalized') THEN
        ALTER TABLE school.classes ADD COLUMN grade_level_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(grade_level, '')))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='classes' AND column_name='school_year_normalized') THEN
        ALTER TABLE school.classes ADD COLUMN school_year_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(school_year))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_classes_name_normalized ON school.classes(class_name_normalized);
CREATE INDEX IF NOT EXISTS idx_classes_grade_level_normalized ON school.classes(grade_level_normalized);

-- 5. school.teachers
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='teachers' AND column_name='specialization_normalized') THEN
        ALTER TABLE school.teachers ADD COLUMN specialization_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(specialization, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_teachers_specialization_normalized ON school.teachers(specialization_normalized);

-- 6. school.student_guardians
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='student_guardians' AND column_name='relationship_notes_normalized') THEN
        ALTER TABLE school.student_guardians ADD COLUMN relationship_notes_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(relationship_notes, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_student_guardians_notes_normalized ON school.student_guardians(relationship_notes_normalized);

-- 7. content.modules
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='modules' AND column_name='module_name_normalized') THEN
        ALTER TABLE content.modules ADD COLUMN module_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(module_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='modules' AND column_name='description_normalized') THEN
        ALTER TABLE content.modules ADD COLUMN description_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_modules_name_normalized ON content.modules(module_name_normalized);
CREATE INDEX IF NOT EXISTS idx_modules_description_normalized ON content.modules(description_normalized);

-- 8. content.activities
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='activities' AND column_name='activity_name_normalized') THEN
        ALTER TABLE content.activities ADD COLUMN activity_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(activity_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='activities' AND column_name='description_normalized') THEN
        ALTER TABLE content.activities ADD COLUMN description_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_activities_name_normalized ON content.activities(activity_name_normalized);
CREATE INDEX IF NOT EXISTS idx_activities_description_normalized ON content.activities(description_normalized);

-- 9. content.assets
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='assets' AND column_name='asset_name_normalized') THEN
        ALTER TABLE content.assets ADD COLUMN asset_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(asset_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='content' AND table_name='assets' AND column_name='alt_text_normalized') THEN
        ALTER TABLE content.assets ADD COLUMN alt_text_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(alt_text, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_assets_name_normalized ON content.assets(asset_name_normalized);
CREATE INDEX IF NOT EXISTS idx_assets_alt_text_normalized ON content.assets(alt_text_normalized);

-- 10. game.badges
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='game' AND table_name='badges' AND column_name='badge_name_normalized') THEN
        ALTER TABLE game.badges ADD COLUMN badge_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(badge_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='game' AND table_name='badges' AND column_name='description_normalized') THEN
        ALTER TABLE game.badges ADD COLUMN description_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_badges_name_normalized ON game.badges(badge_name_normalized);
CREATE INDEX IF NOT EXISTS idx_badges_description_normalized ON game.badges(description_normalized);

-- 11. assets.media_files
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='assets' AND table_name='media_files' AND column_name='original_name_normalized') THEN
        ALTER TABLE assets.media_files ADD COLUMN original_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(original_name))) STORED;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='assets' AND table_name='media_files' AND column_name='alt_text_normalized') THEN
        ALTER TABLE assets.media_files ADD COLUMN alt_text_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(alt_text, '')))) STORED;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS idx_media_files_original_name_normalized ON assets.media_files(original_name_normalized);
CREATE INDEX IF NOT EXISTS idx_media_files_alt_text_normalized ON assets.media_files(alt_text_normalized);

-- Summary
SELECT 'Normalization complete: All TEXT columns now have *_normalized generated columns with indexes' AS status;
