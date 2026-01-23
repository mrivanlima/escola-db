-- =====================================================
-- TABLE: school.proficiency_levels
-- Description: Lookup table for teacher subject proficiency levels
-- Purpose: Normalized reference data for teaching expertise levels
-- =====================================================

CREATE TABLE IF NOT EXISTS school.proficiency_levels (
    -- 1. IDs Híbridos
    proficiency_level_id SMALLINT GENERATED ALWAYS AS IDENTITY,
    proficiency_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id            INTEGER NOT NULL,
    
    -- 2. Business Data
    proficiency_code     TEXT NOT NULL, -- 'beginner', 'intermediate', 'expert'
    proficiency_name     TEXT NOT NULL, -- 'Beginner', 'Intermediate', 'Expert'
    description          TEXT,
    minimum_years        INTEGER, -- Minimum years of experience typically required
    icon_name            TEXT, -- UI icon identifier
    color_code           TEXT, -- Hex color for UI badges
    display_order        SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    proficiency_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(proficiency_code)))
    ) STORED,
    proficiency_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(proficiency_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_proficiency_levels PRIMARY KEY (proficiency_level_id),
    CONSTRAINT uq_proficiency_levels_uuid UNIQUE (proficiency_uuid),
    CONSTRAINT uq_proficiency_levels_code UNIQUE (tenant_id, proficiency_code),
    
    CONSTRAINT fk_proficiency_levels_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_proficiency_levels_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_proficiency_levels_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_proficiency_levels_minimum_years CHECK (minimum_years IS NULL OR minimum_years >= 0),
    CONSTRAINT ck_proficiency_levels_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_proficiency_levels_tenant ON school.proficiency_levels(tenant_id);
CREATE INDEX idx_proficiency_levels_code_normalized ON school.proficiency_levels(proficiency_code_normalized);
CREATE INDEX idx_proficiency_levels_deleted_at ON school.proficiency_levels(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_proficiency_levels_display_order ON school.proficiency_levels(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_proficiency_levels_updated_at
BEFORE UPDATE ON school.proficiency_levels
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.proficiency_levels ENABLE ROW LEVEL SECURITY;

-- Policy: Tenant Isolation
CREATE POLICY "Tenant Isolation" ON school.proficiency_levels
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.proficiency_levels IS 'Lookup: defines teacher proficiency levels for teaching subjects';
COMMENT ON COLUMN school.proficiency_levels.proficiency_level_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN school.proficiency_levels.proficiency_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.proficiency_levels.proficiency_code IS 'Unique code: beginner, intermediate, expert';
COMMENT ON COLUMN school.proficiency_levels.minimum_years IS 'Typical minimum years of experience for this level';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO school.proficiency_levels (tenant_id, proficiency_code, proficiency_name, description, minimum_years, icon_name, color_code, display_order, created_by)
-- VALUES
--     (1, 'beginner', 'Beginner', 'New to teaching this subject, requires support', 0, 'school', '#2196F3', 1, 1),
--     (1, 'intermediate', 'Intermediate', 'Comfortable teaching this subject independently', 2, 'star_half', '#FF9800', 2, 1),
--     (1, 'expert', 'Expert', 'Highly experienced, can mentor others in this subject', 5, 'workspace_premium', '#4CAF50', 3, 1)
-- ON CONFLICT (tenant_id, proficiency_code) DO NOTHING;
