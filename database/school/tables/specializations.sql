-- =====================================================
-- TABLE: school.specializations
-- Description: Lookup table for teacher specializations
-- Scope: Reference data for teacher qualifications
-- =====================================================

CREATE TABLE IF NOT EXISTS school.specializations (
    -- 1. IDs Híbridos
    specialization_id   INTEGER GENERATED ALWAYS AS IDENTITY,
    specialization_uuid UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    specialization_name TEXT NOT NULL,
    description         TEXT,
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    specialization_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(specialization_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_specializations PRIMARY KEY (specialization_id),
    CONSTRAINT uq_specializations_uuid UNIQUE (specialization_uuid),
    CONSTRAINT uq_specializations_name UNIQUE (specialization_name),
    
    CONSTRAINT fk_specializations_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_specializations_name CHECK (LENGTH(specialization_name) >= 2)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_specializations_name_normalized ON school.specializations(specialization_name_normalized);
CREATE INDEX IF NOT EXISTS idx_specializations_deleted_at ON school.specializations(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_specializations_updated_at
BEFORE UPDATE ON school.specializations
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.specializations ENABLE ROW LEVEL SECURITY;

-- Specializations are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON school.specializations
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON school.specializations
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.specializations IS 'Reference table for teacher specializations (e.g., "Early Childhood Education", "Mathematics")';
COMMENT ON COLUMN school.specializations.specialization_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.specializations.specialization_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.specializations.specialization_name IS 'Display name of specialization';
COMMENT ON COLUMN school.specializations.description IS 'Detailed description of specialization requirements/scope';
