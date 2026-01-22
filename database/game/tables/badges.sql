-- =====================================================
-- TABLE: game.badges
-- Description: Achievement badges/rewards configuration
-- Purpose: Gamification elements (catalog)
-- =====================================================

CREATE TABLE IF NOT EXISTS game.badges (
    -- 1. IDs Híbridos
    badge_id        INTEGER GENERATED ALWAYS AS IDENTITY,
    badge_uuid      UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    badge_name      TEXT NOT NULL,
    description     TEXT,
    badge_type      TEXT NOT NULL, -- 'achievement', 'milestone', 'special', 'seasonal'
    
    -- 2.1. Normalized columns for search
    badge_name_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(badge_name))) STORED,
    description_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED,
    icon_url        TEXT,
    rarity          TEXT NOT NULL DEFAULT 'common', -- 'common', 'rare', 'epic', 'legendary'
    points_value    INTEGER NOT NULL DEFAULT 0,
    unlock_criteria JSONB NOT NULL, -- {"type": "activity_count", "value": 10, "activity_type": "quiz"}
    display_order   INTEGER NOT NULL DEFAULT 0,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_badges PRIMARY KEY (badge_id),
    CONSTRAINT uq_badges_uuid UNIQUE (badge_uuid),
    
    CONSTRAINT fk_badges_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_badges_name CHECK (LENGTH(badge_name) >= 2),
    CONSTRAINT ck_badges_type CHECK (badge_type IN ('achievement', 'milestone', 'special', 'seasonal')),
    CONSTRAINT ck_badges_rarity CHECK (rarity IN ('common', 'rare', 'epic', 'legendary')),
    CONSTRAINT ck_badges_points CHECK (points_value >= 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_badges_type ON game.badges(badge_type);
CREATE INDEX IF NOT EXISTS idx_badges_rarity ON game.badges(rarity);
CREATE INDEX IF NOT EXISTS idx_badges_name_normalized ON game.badges(badge_name_normalized);
CREATE INDEX IF NOT EXISTS idx_badges_description_normalized ON game.badges(description_normalized);
CREATE INDEX IF NOT EXISTS idx_badges_active ON game.badges(is_active) WHERE is_active = TRUE;
CREATE INDEX IF NOT EXISTS idx_badges_deleted_at ON game.badges(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_badges_updated_at
BEFORE UPDATE ON game.badges
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. No RLS (Badges are shared across tenants)

-- 8. Metadata (Documentation)
COMMENT ON TABLE game.badges IS 'Achievement badges/rewards catalog for gamification';
COMMENT ON COLUMN game.badges.badge_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN game.badges.badge_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN game.badges.unlock_criteria IS 'JSON: {"type": "activity_count", "value": 10, "module_uuid": "..."}';
COMMENT ON COLUMN game.badges.rarity IS 'Badge rarity level: common, rare, epic, legendary';
COMMENT ON COLUMN game.badges.badge_type IS 'Type: achievement (progress), milestone (goals), special, seasonal (events)';
