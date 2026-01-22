-- =====================================================
-- TABLE: content.activities
-- Description: Individual learning activities within modules
-- Hierarchy: modules -> activities -> (game interactions)
-- =====================================================

CREATE TABLE IF NOT EXISTS content.activities (
    -- 1. IDs Híbridos
    activity_id     INTEGER GENERATED ALWAYS AS IDENTITY,
    activity_uuid   UUID NOT NULL DEFAULT gen_random_uuid(),
    module_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    activity_name   TEXT NOT NULL,
    description     TEXT,
    activity_type   TEXT NOT NULL, -- 'quiz', 'game', 'video', 'interactive', 'puzzle'
    display_order   INTEGER NOT NULL DEFAULT 0,
    
    -- 2.1. Normalized columns for search
    activity_name_normalized    TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(activity_name))) STORED,
    description_normalized      TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED,
    estimated_duration INTEGER, -- Duration in minutes
    points_reward   INTEGER NOT NULL DEFAULT 0, -- Gamification points
    activity_data   JSONB NOT NULL, -- Activity-specific config/questions
    thumbnail_url   TEXT,
    is_published    BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_activities PRIMARY KEY (activity_id),
    CONSTRAINT uq_activities_uuid UNIQUE (activity_uuid),
    
    CONSTRAINT fk_activities_module FOREIGN KEY (module_id)
        REFERENCES content.modules (module_id),
    
    CONSTRAINT fk_activities_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_activities_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_activities_name CHECK (LENGTH(activity_name) >= 2),
    CONSTRAINT ck_activities_points CHECK (points_reward >= 0),
    CONSTRAINT ck_activities_duration CHECK (estimated_duration IS NULL OR estimated_duration > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_activities_module ON content.activities(module_id);
CREATE INDEX IF NOT EXISTS idx_activities_type ON content.activities(activity_type);
CREATE INDEX IF NOT EXISTS idx_activities_name_normalized ON content.activities(activity_name_normalized);
CREATE INDEX IF NOT EXISTS idx_activities_description_normalized ON content.activities(description_normalized);
CREATE INDEX IF NOT EXISTS idx_activities_published ON content.activities(is_published) WHERE is_published = TRUE;
CREATE INDEX IF NOT EXISTS idx_activities_deleted_at ON content.activities(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_activities_updated_at
BEFORE UPDATE ON content.activities
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. Metadata (Documentation)
COMMENT ON TABLE content.activities IS 'Individual learning activities within modules';
COMMENT ON COLUMN content.activities.activity_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN content.activities.activity_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.activities.activity_data IS 'JSON: Type-specific data (questions, game config, video url, etc.)';
COMMENT ON COLUMN content.activities.points_reward IS 'Gamification points awarded upon completion';
COMMENT ON COLUMN content.activities.activity_type IS 'Type: quiz, game, video, interactive, puzzle, etc.';
