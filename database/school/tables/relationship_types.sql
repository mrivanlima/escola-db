-- =====================================================
-- TABLE: school.relationship_types
-- Description: Lookup table for guardian-student relationship types
-- Purpose: Normalized reference data for family relationships
-- =====================================================

CREATE TABLE IF NOT EXISTS school.relationship_types (
    -- 1. IDs Híbridos
    relationship_type_id SMALLINT GENERATED ALWAYS AS IDENTITY,
    relationship_uuid    UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id            INTEGER NOT NULL,
    
    -- 2. Business Data
    relationship_code    TEXT NOT NULL, -- 'father', 'mother', 'legal_guardian', 'other'
    relationship_name    TEXT NOT NULL, -- 'Father', 'Mother', 'Legal Guardian', 'Other'
    description          TEXT,
    can_authorize        BOOLEAN NOT NULL DEFAULT TRUE, -- Can authorize school activities
    requires_legal_proof BOOLEAN NOT NULL DEFAULT FALSE, -- Requires legal documentation
    icon_name            TEXT, -- UI icon identifier
    display_order        SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    relationship_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(relationship_code)))
    ) STORED,
    relationship_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(relationship_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_relationship_types PRIMARY KEY (relationship_type_id),
    CONSTRAINT uq_relationship_types_uuid UNIQUE (relationship_uuid),
    CONSTRAINT uq_relationship_types_code UNIQUE (tenant_id, relationship_code),
    
    CONSTRAINT fk_relationship_types_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_relationship_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_relationship_types_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id)
);

-- 6. Indexes
CREATE INDEX idx_relationship_types_tenant ON school.relationship_types(tenant_id);
CREATE INDEX idx_relationship_types_code_normalized ON school.relationship_types(relationship_code_normalized);
CREATE INDEX idx_relationship_types_deleted_at ON school.relationship_types(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_relationship_types_display_order ON school.relationship_types(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_relationship_types_updated_at
BEFORE UPDATE ON school.relationship_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.relationship_types ENABLE ROW LEVEL SECURITY;

-- Policy: Tenant Isolation
CREATE POLICY "Tenant Isolation" ON school.relationship_types
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.relationship_types IS 'Lookup: defines guardian-student relationship types';
COMMENT ON COLUMN school.relationship_types.relationship_type_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN school.relationship_types.relationship_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.relationship_types.relationship_code IS 'Unique code: father, mother, legal_guardian, other';
COMMENT ON COLUMN school.relationship_types.can_authorize IS 'Whether this relationship type can authorize school activities';
COMMENT ON COLUMN school.relationship_types.requires_legal_proof IS 'Whether legal documentation is required for this relationship';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO school.relationship_types (tenant_id, relationship_code, relationship_name, description, can_authorize, requires_legal_proof, icon_name, display_order, created_by)
-- VALUES
--     (1, 'father', 'Father', 'Biological or adoptive father', TRUE, FALSE, 'face', 1, 1),
--     (1, 'mother', 'Mother', 'Biological or adoptive mother', TRUE, FALSE, 'face', 2, 1),
--     (1, 'legal_guardian', 'Legal Guardian', 'Court-appointed legal guardian', TRUE, TRUE, 'gavel', 3, 1),
--     (1, 'other', 'Other', 'Other authorized guardian or family member', FALSE, TRUE, 'people', 4, 1)
-- ON CONFLICT (tenant_id, relationship_code) DO NOTHING;
