-- =====================================================
-- TABLE: game.badge_rarities
-- Description: Lookup table for badge rarity levels
-- Scope: Reference data for badge rarity classification
-- =====================================================

CREATE TABLE IF NOT EXISTS game.badge_rarities (
    -- 1. IDs Híbridos
    rarity_id       SMALLINT GENERATED ALWAYS AS IDENTITY,
    rarity_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    rarity_code     TEXT NOT NULL, -- 'common', 'rare', 'epic', 'legendary'
    rarity_name     TEXT NOT NULL, -- Display name: "Common", "Rare", "Epic", "Legendary"
    description     TEXT,
    color_code      TEXT, -- Hex color for UI display (#808080 for common, #FFD700 for legendary)
    points_multiplier NUMERIC(3,2) NOT NULL DEFAULT 1.00, -- Multiplier for badge points (1.0, 1.5, 2.0, 3.0)
    display_order   INTEGER NOT NULL DEFAULT 0,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    rarity_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(rarity_code))) STORED,
    rarity_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(rarity_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_badge_rarities PRIMARY KEY (rarity_id),
    CONSTRAINT uq_badge_rarities_uuid UNIQUE (rarity_uuid),
    CONSTRAINT uq_badge_rarities_code UNIQUE (rarity_code),
    
    CONSTRAINT fk_badge_rarities_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_badge_rarities_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_badge_rarities_code CHECK (LENGTH(rarity_code) >= 2),
    CONSTRAINT ck_badge_rarities_name CHECK (LENGTH(rarity_name) >= 2),
    CONSTRAINT ck_badge_rarities_color CHECK (color_code IS NULL OR color_code ~ '^#[0-9A-Fa-f]{6}$'),
    CONSTRAINT ck_badge_rarities_multiplier CHECK (points_multiplier > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_badge_rarities_code_normalized ON game.badge_rarities(rarity_code_normalized);
CREATE INDEX IF NOT EXISTS idx_badge_rarities_name_normalized ON game.badge_rarities(rarity_name_normalized);
CREATE INDEX IF NOT EXISTS idx_badge_rarities_display_order ON game.badge_rarities(display_order);
CREATE INDEX IF NOT EXISTS idx_badge_rarities_deleted_at ON game.badge_rarities(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_badge_rarities_updated_at
BEFORE UPDATE ON game.badge_rarities
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE game.badge_rarities ENABLE ROW LEVEL SECURITY;

-- Badge rarities are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON game.badge_rarities
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON game.badge_rarities
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE game.badge_rarities IS 'Reference table for badge rarity levels (common, rare, epic, legendary)';
COMMENT ON COLUMN game.badge_rarities.rarity_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN game.badge_rarities.rarity_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN game.badge_rarities.rarity_code IS 'Code identifier: common, rare, epic, legendary';
COMMENT ON COLUMN game.badge_rarities.rarity_name IS 'Human-readable display name';
COMMENT ON COLUMN game.badge_rarities.color_code IS 'Hex color code for UI display';
COMMENT ON COLUMN game.badge_rarities.points_multiplier IS 'Multiplier applied to badge base points (1.0 for common, 3.0 for legendary)';

-- 9. Seed Data (Badge Rarities)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO game.badge_rarities (rarity_code, rarity_name, description, color_code, points_multiplier, display_order, created_by)
-- VALUES 
--     ('common', 'Common', 'Standard badges easily obtained', '#808080', 1.00, 1, 1),
--     ('rare', 'Rare', 'Badges requiring more effort to obtain', '#4169E1', 1.50, 2, 1),
--     ('epic', 'Epic', 'Exceptional badges for significant achievements', '#9B30FF', 2.00, 3, 1),
--     ('legendary', 'Legendary', 'The rarest and most prestigious badges', '#FFD700', 3.00, 4, 1)
-- ON CONFLICT (rarity_code) DO NOTHING;
