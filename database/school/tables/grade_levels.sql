-- =====================================================
-- TABLE: school.grade_levels
-- Description: Lookup table for educational grade levels
-- Purpose: Normalized reference data for student grade classifications
-- =====================================================

CREATE TABLE IF NOT EXISTS school.grade_levels (
    -- 1. IDs Híbridos
    grade_level_id  SMALLINT GENERATED ALWAYS AS IDENTITY,
    grade_uuid      UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    grade_code      TEXT NOT NULL, -- 'pre-k', 'kindergarten', '1st-grade', '2nd-grade', etc.
    grade_name      TEXT NOT NULL, -- 'Pre-K', 'Kindergarten', '1st Grade', '2nd Grade', etc.
    description     TEXT,
    age_range_min   INTEGER, -- Minimum age (e.g., 3 for pre-k)
    age_range_max   INTEGER, -- Maximum age (e.g., 5 for pre-k)
    display_order   SMALLINT NOT NULL,
    icon_name       TEXT, -- UI icon identifier
    color_code      TEXT, -- Hex color for UI badges
    
    -- 3. Normalized Search Columns
    grade_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(grade_code)))
    ) STORED,
    grade_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(grade_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_grade_levels PRIMARY KEY (grade_level_id),
    CONSTRAINT uq_grade_levels_uuid UNIQUE (grade_uuid),
    CONSTRAINT uq_grade_levels_code UNIQUE (tenant_id, grade_code),
    
    CONSTRAINT fk_grade_levels_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_grade_levels_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_grade_levels_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_grade_levels_age_range CHECK (
        (age_range_min IS NULL AND age_range_max IS NULL) OR
        (age_range_min IS NOT NULL AND age_range_max IS NOT NULL AND age_range_min <= age_range_max)
    ),
    CONSTRAINT ck_grade_levels_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_grade_levels_tenant ON school.grade_levels(tenant_id);
CREATE INDEX idx_grade_levels_code_normalized ON school.grade_levels(grade_code_normalized);
CREATE INDEX idx_grade_levels_deleted_at ON school.grade_levels(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_grade_levels_display_order ON school.grade_levels(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_grade_levels_updated_at
BEFORE UPDATE ON school.grade_levels
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.grade_levels ENABLE ROW LEVEL SECURITY;

-- Policy: Tenant Isolation
CREATE POLICY "Tenant Isolation" ON school.grade_levels
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.grade_levels IS 'Lookup: defines educational grade levels with age ranges';
COMMENT ON COLUMN school.grade_levels.grade_level_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN school.grade_levels.grade_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.grade_levels.grade_code IS 'Unique code: pre-k, kindergarten, 1st-grade, 2nd-grade, etc.';
COMMENT ON COLUMN school.grade_levels.age_range_min IS 'Minimum typical age for this grade level';
COMMENT ON COLUMN school.grade_levels.age_range_max IS 'Maximum typical age for this grade level';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO school.grade_levels (tenant_id, grade_code, grade_name, description, age_range_min, age_range_max, display_order, icon_name, color_code, created_by)
-- VALUES
--     (1, 'pre-k', 'Pre-K', 'Pre-Kindergarten (Early Childhood)', 3, 4, 1, 'child_care', '#E91E63', 1),
--     (1, 'kindergarten', 'Kindergarten', 'Kindergarten', 5, 6, 2, 'school', '#9C27B0', 1),
--     (1, '1st-grade', '1st Grade', 'First Grade', 6, 7, 3, 'looks_one', '#3F51B5', 1),
--     (1, '2nd-grade', '2nd Grade', 'Second Grade', 7, 8, 4, 'looks_two', '#2196F3', 1),
--     (1, '3rd-grade', '3rd Grade', 'Third Grade', 8, 9, 5, 'looks_3', '#00BCD4', 1),
--     (1, '4th-grade', '4th Grade', 'Fourth Grade', 9, 10, 6, 'looks_4', '#009688', 1),
--     (1, '5th-grade', '5th Grade', 'Fifth Grade', 10, 11, 7, 'looks_5', '#4CAF50', 1),
--     (1, '6th-grade', '6th Grade', 'Sixth Grade', 11, 12, 8, 'looks_6', '#8BC34A', 1)
-- ON CONFLICT (tenant_id, grade_code) DO NOTHING;
