-- =====================================================
-- TABLE: game.badge_types
-- Description: Lookup table for badge types
-- Scope: Reference data for badge classification
-- =====================================================

CREATE TABLE IF NOT EXISTS game.badge_types (
    -- 1. IDs Híbridos
    badge_type_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    badge_type_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    badge_type_code  TEXT NOT NULL, -- 'achievement', 'milestone', 'special', 'seasonal'
    badge_type_name  TEXT NOT NULL, -- Display name: "Achievement", "Milestone", "Special Event"
    description      TEXT,
    icon_name        TEXT, -- Icon identifier for frontend
    display_order    INTEGER NOT NULL DEFAULT 0,
    is_active        BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    badge_type_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(badge_type_code))) STORED,
    badge_type_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(badge_type_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_badge_types PRIMARY KEY (badge_type_id),
    CONSTRAINT uq_badge_types_uuid UNIQUE (badge_type_uuid),
    CONSTRAINT uq_badge_types_code UNIQUE (badge_type_code),
    
    CONSTRAINT fk_badge_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_badge_types_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_badge_types_code CHECK (LENGTH(badge_type_code) >= 2),
    CONSTRAINT ck_badge_types_name CHECK (LENGTH(badge_type_name) >= 2)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_badge_types_code_normalized ON game.badge_types(badge_type_code_normalized);
CREATE INDEX IF NOT EXISTS idx_badge_types_name_normalized ON game.badge_types(badge_type_name_normalized);
CREATE INDEX IF NOT EXISTS idx_badge_types_display_order ON game.badge_types(display_order);
CREATE INDEX IF NOT EXISTS idx_badge_types_deleted_at ON game.badge_types(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_badge_types_updated_at
BEFORE UPDATE ON game.badge_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE game.badge_types ENABLE ROW LEVEL SECURITY;

-- Badge types are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON game.badge_types
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON game.badge_types
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE game.badge_types IS 'Reference table for badge types (achievement, milestone, special, seasonal)';
COMMENT ON COLUMN game.badge_types.badge_type_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN game.badge_types.badge_type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN game.badge_types.badge_type_code IS 'Code identifier: achievement, milestone, special, seasonal';
COMMENT ON COLUMN game.badge_types.badge_type_name IS 'Human-readable display name';
COMMENT ON COLUMN game.badge_types.icon_name IS 'Icon identifier for frontend rendering';

-- 9. Seed Data (Badge Types)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO game.badge_types (badge_type_code, badge_type_name, description, icon_name, display_order, created_by)
-- VALUES 
--     ('achievement', 'Achievement', 'Badges earned for completing specific tasks or reaching goals', 'achievement_icon', 1, 1),
--     ('milestone', 'Milestone', 'Badges awarded for reaching significant progress milestones', 'milestone_icon', 2, 1),
--     ('special', 'Special Event', 'Limited-time or special occasion badges', 'special_icon', 3, 1),
--     ('seasonal', 'Seasonal', 'Badges available during specific seasons or holidays', 'seasonal_icon', 4, 1)
-- ON CONFLICT (badge_type_code) DO NOTHING;
