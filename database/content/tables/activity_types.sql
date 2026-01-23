-- =====================================================
-- TABLE: content.activity_types
-- Description: Lookup table for activity types
-- Scope: Reference data for activity classification
-- =====================================================

CREATE TABLE IF NOT EXISTS content.activity_types (
    -- 1. IDs Híbridos
    activity_type_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    activity_type_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    activity_type_code  TEXT NOT NULL, -- 'quiz', 'game', 'video', 'interactive', 'puzzle'
    activity_type_name  TEXT NOT NULL, -- Display name: "Quiz", "Interactive Game", "Video Lesson"
    description         TEXT,
    icon_name           TEXT, -- Icon identifier for frontend
    default_points      INTEGER DEFAULT 10,
    requires_interaction BOOLEAN DEFAULT TRUE,
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    activity_type_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(activity_type_code))) STORED,
    activity_type_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(activity_type_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_activity_types PRIMARY KEY (activity_type_id),
    CONSTRAINT uq_activity_types_uuid UNIQUE (activity_type_uuid),
    CONSTRAINT uq_activity_types_code UNIQUE (activity_type_code),
    
    CONSTRAINT fk_activity_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_activity_types_code CHECK (LENGTH(activity_type_code) >= 2),
    CONSTRAINT ck_activity_types_name CHECK (LENGTH(activity_type_name) >= 2),
    CONSTRAINT ck_activity_types_points CHECK (default_points >= 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_activity_types_code_normalized ON content.activity_types(activity_type_code_normalized);
CREATE INDEX IF NOT EXISTS idx_activity_types_name_normalized ON content.activity_types(activity_type_name_normalized);
CREATE INDEX IF NOT EXISTS idx_activity_types_deleted_at ON content.activity_types(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_activity_types_updated_at
BEFORE UPDATE ON content.activity_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE content.activity_types ENABLE ROW LEVEL SECURITY;

-- Activity types are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON content.activity_types
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON content.activity_types
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE content.activity_types IS 'Reference table for activity types (quiz, game, video, interactive, puzzle)';
COMMENT ON COLUMN content.activity_types.activity_type_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN content.activity_types.activity_type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.activity_types.activity_type_code IS 'Code identifier: quiz, game, video, interactive, puzzle';
COMMENT ON COLUMN content.activity_types.activity_type_name IS 'Human-readable display name';
COMMENT ON COLUMN content.activity_types.icon_name IS 'Icon identifier for frontend rendering';
COMMENT ON COLUMN content.activity_types.default_points IS 'Default gamification points for this activity type';
COMMENT ON COLUMN content.activity_types.requires_interaction IS 'TRUE if activity requires user interaction (not passive like video)';

-- 9. Seed Data (Common Activity Types)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO content.activity_types (activity_type_code, activity_type_name, description, icon_name, default_points, requires_interaction, created_by)
-- VALUES 
--     ('quiz', 'Quiz', 'Multiple choice or true/false questions', 'quiz_icon', 10, TRUE, 1),
--     ('game', 'Interactive Game', 'Gamified learning experience', 'game_icon', 15, TRUE, 1),
--     ('video', 'Video Lesson', 'Educational video content', 'video_icon', 5, FALSE, 1),
--     ('interactive', 'Interactive Activity', 'Hands-on interactive learning', 'interactive_icon', 12, TRUE, 1),
--     ('puzzle', 'Puzzle', 'Problem-solving puzzles', 'puzzle_icon', 10, TRUE, 1),
--     ('reading', 'Reading', 'Text-based reading activity', 'book_icon', 8, FALSE, 1),
--     ('drawing', 'Drawing Activity', 'Creative drawing exercise', 'draw_icon', 10, TRUE, 1),
--     ('matching', 'Matching Game', 'Match items or concepts', 'matching_icon', 10, TRUE, 1)
-- ON CONFLICT (activity_type_code) DO NOTHING;
