-- =====================================================
-- TABLE: school.school_years
-- Description: Lookup table for academic/school years
-- Purpose: Normalized reference data for academic year management
-- =====================================================

CREATE TABLE IF NOT EXISTS school.school_years (
    -- 1. IDs Híbridos
    school_year_id  SMALLINT GENERATED ALWAYS AS IDENTITY,
    year_uuid       UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    year_code       TEXT NOT NULL, -- '2024-2025', '2025-2026'
    year_name       TEXT NOT NULL, -- '2024/2025', '2025/2026'
    description     TEXT,
    start_date      DATE NOT NULL,
    end_date        DATE NOT NULL,
    is_current      BOOLEAN NOT NULL DEFAULT FALSE, -- Only one current year per tenant
    display_order   SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    year_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(year_code)))
    ) STORED,
    year_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(year_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_school_years PRIMARY KEY (school_year_id),
    CONSTRAINT uq_school_years_uuid UNIQUE (year_uuid),
    CONSTRAINT uq_school_years_code UNIQUE (tenant_id, year_code),
    
    CONSTRAINT fk_school_years_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_school_years_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_school_years_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_school_years_dates CHECK (start_date < end_date)
);

-- 6. Indexes
CREATE INDEX idx_school_years_tenant ON school.school_years(tenant_id);
CREATE INDEX idx_school_years_code_normalized ON school.school_years(year_code_normalized);
CREATE INDEX idx_school_years_is_current ON school.school_years(tenant_id, is_current) WHERE is_current = TRUE;
CREATE INDEX idx_school_years_dates ON school.school_years(start_date, end_date);
CREATE INDEX idx_school_years_deleted_at ON school.school_years(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_school_years_display_order ON school.school_years(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_school_years_updated_at
BEFORE UPDATE ON school.school_years
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.school_years ENABLE ROW LEVEL SECURITY;

-- Policy: Tenant Isolation
CREATE POLICY "Tenant Isolation" ON school.school_years
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.school_years IS 'Lookup: defines academic/school years with date ranges';
COMMENT ON COLUMN school.school_years.school_year_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN school.school_years.year_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.school_years.year_code IS 'Unique code: 2024-2025, 2025-2026 (hyphen format)';
COMMENT ON COLUMN school.school_years.year_name IS 'Display name: 2024/2025, 2025/2026 (slash format)';
COMMENT ON COLUMN school.school_years.is_current IS 'TRUE if this is the current active academic year';
COMMENT ON COLUMN school.school_years.start_date IS 'First day of the academic year';
COMMENT ON COLUMN school.school_years.end_date IS 'Last day of the academic year';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO school.school_years (tenant_id, year_code, year_name, description, start_date, end_date, is_current, display_order, created_by)
-- VALUES
--     (1, '2023-2024', '2023/2024', 'Academic Year 2023-2024', '2023-09-01', '2024-06-30', FALSE, 1, 1),
--     (1, '2024-2025', '2024/2025', 'Academic Year 2024-2025', '2024-09-01', '2025-06-30', FALSE, 2, 1),
--     (1, '2025-2026', '2025/2026', 'Academic Year 2025-2026', '2025-09-01', '2026-06-30', TRUE, 3, 1),
--     (1, '2026-2027', '2026/2027', 'Academic Year 2026-2027', '2026-09-01', '2027-06-30', FALSE, 4, 1)
-- ON CONFLICT (tenant_id, year_code) DO NOTHING;
