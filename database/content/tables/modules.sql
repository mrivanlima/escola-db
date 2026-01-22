-- =====================================================
-- TABLE: content.modules
-- Description: Learning modules/courses (e.g., "Math Level 1", "Reading Adventures")
-- Scope: Content hierarchy root
-- =====================================================

CREATE TABLE IF NOT EXISTS content.modules (
    -- 1. IDs Híbridos
    module_id       INTEGER GENERATED ALWAYS AS IDENTITY,
    module_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    module_name     TEXT NOT NULL,
    description     TEXT,
    module_type     TEXT NOT NULL, -- 'math', 'reading', 'science', 'art', etc.
    difficulty_level INTEGER NOT NULL, -- 1-10 scale
    
    -- 2.1. Normalized columns for search
    module_name_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(module_name))) STORED,
    description_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(description, '')))) STORED,
    recommended_age_min INTEGER, -- Minimum age in months
    recommended_age_max INTEGER, -- Maximum age in months
    display_order   INTEGER NOT NULL DEFAULT 0,
    thumbnail_url   TEXT,
    module_config   JSONB, -- {"tags": ["shapes", "colors"], "prerequisites": []}
    is_published    BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_modules PRIMARY KEY (module_id),
    CONSTRAINT uq_modules_uuid UNIQUE (module_uuid),
    
    CONSTRAINT fk_modules_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_modules_name CHECK (LENGTH(module_name) >= 2),
    CONSTRAINT ck_modules_difficulty CHECK (difficulty_level BETWEEN 1 AND 10),
    CONSTRAINT ck_modules_age_range CHECK (recommended_age_min IS NULL OR recommended_age_max IS NULL OR recommended_age_min <= recommended_age_max)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_modules_type ON content.modules(module_type);
CREATE INDEX IF NOT EXISTS idx_modules_difficulty ON content.modules(difficulty_level);
CREATE INDEX IF NOT EXISTS idx_modules_name_normalized ON content.modules(module_name_normalized);
CREATE INDEX IF NOT EXISTS idx_modules_description_normalized ON content.modules(description_normalized);
CREATE INDEX IF NOT EXISTS idx_modules_published ON content.modules(is_published) WHERE is_published = TRUE;
CREATE INDEX IF NOT EXISTS idx_modules_deleted_at ON content.modules(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_modules_updated_at
BEFORE UPDATE ON content.modules
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. No RLS (Content is shared across tenants)
-- Content tables typically don't have RLS as they are shared resources

-- 8. Metadata (Documentation)
COMMENT ON TABLE content.modules IS 'Learning modules/courses: root of content hierarchy';
COMMENT ON COLUMN content.modules.module_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN content.modules.module_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.modules.recommended_age_min IS 'Minimum recommended age in months (e.g., 36 = 3 years)';
COMMENT ON COLUMN content.modules.module_config IS 'JSON: {"tags": ["shapes"], "prerequisites": [module_uuid], "duration_minutes": 30}';
