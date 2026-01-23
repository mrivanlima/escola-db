-- =====================================================
-- TABLE: school.guardians
-- Description: Parents/Legal guardians of students
-- Links to: identity.app_users (user_role = 'parent')
-- =====================================================

CREATE TABLE IF NOT EXISTS school.guardians (
    -- 1. IDs Híbridos
    guardian_id     INTEGER GENERATED ALWAYS AS IDENTITY,
    guardian_uuid   UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    user_id         INTEGER NOT NULL, -- Links to identity.app_users
    
    -- 2. Business Data
    phone_number    TEXT,
    relationship_type_id SMALLINT NOT NULL, -- FK to school.relationship_types
    is_primary      BOOLEAN NOT NULL DEFAULT FALSE, -- Primary contact
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_guardians PRIMARY KEY (guardian_id),
    CONSTRAINT uq_guardians_uuid UNIQUE (guardian_uuid),
    CONSTRAINT uq_guardians_user UNIQUE (user_id), -- 1:1 relationship with app_users
    
    CONSTRAINT fk_guardians_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_guardians_user FOREIGN KEY (user_id)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_guardians_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_guardians_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_guardians_relationship FOREIGN KEY (relationship_type_id)
        REFERENCES school.relationship_types (relationship_type_id)
);

-- 5. Indexes
CREATE INDEX idx_guardians_tenant ON school.guardians(tenant_id);
CREATE INDEX idx_guardians_user ON school.guardians(user_id);
CREATE INDEX idx_guardians_deleted_at ON school.guardians(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_guardians_updated_at
BEFORE UPDATE ON school.guardians
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.guardians ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON school.guardians
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.guardians IS 'Parents/Legal guardians linked to app_users';
COMMENT ON COLUMN school.guardians.guardian_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.guardians.guardian_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.guardians.user_id IS 'FK to identity.app_users (1:1 relationship)';
COMMENT ON COLUMN school.guardians.relationship_type_id IS 'FK to school.relationship_types: defines guardian-student relationship';
COMMENT ON COLUMN school.guardians.is_primary IS 'Indicates primary contact for notifications/communications';
