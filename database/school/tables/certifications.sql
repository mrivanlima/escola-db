-- =====================================================
-- TABLE: school.certifications
-- Description: Lookup table for teacher certifications
-- Scope: Reference data for professional certifications
-- =====================================================

CREATE TABLE IF NOT EXISTS school.certifications (
    -- 1. IDs Híbridos
    certification_id    INTEGER GENERATED ALWAYS AS IDENTITY,
    certification_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    certification_name  TEXT NOT NULL,
    description         TEXT,
    issuing_organization TEXT,
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    certification_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(certification_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_certifications PRIMARY KEY (certification_id),
    CONSTRAINT uq_certifications_uuid UNIQUE (certification_uuid),
    CONSTRAINT uq_certifications_name UNIQUE (certification_name),
    
    CONSTRAINT fk_certifications_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_certifications_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_certifications_name CHECK (LENGTH(certification_name) >= 2)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_certifications_name_normalized ON school.certifications(certification_name_normalized);
CREATE INDEX IF NOT EXISTS idx_certifications_deleted_at ON school.certifications(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_certifications_updated_at
BEFORE UPDATE ON school.certifications
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.certifications ENABLE ROW LEVEL SECURITY;

-- Certifications are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON school.certifications
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON school.certifications
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.certifications IS 'Reference table for professional certifications (e.g., "Early Childhood Education Certificate", "Special Needs Training")';
COMMENT ON COLUMN school.certifications.certification_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.certifications.certification_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.certifications.certification_name IS 'Display name of certification';
COMMENT ON COLUMN school.certifications.issuing_organization IS 'Organization that issues this certification';
