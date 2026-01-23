-- =====================================================
-- TABLE: identity.tenants
-- Description: Multi-tenant root entity (Schools/Organizations)
-- Type: Master table for tenant isolation
-- =====================================================

CREATE TABLE IF NOT EXISTS identity.tenants (
    -- 1. IDs Híbridos
    tenant_id       INTEGER GENERATED ALWAYS AS IDENTITY,
    tenant_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    tenant_name     TEXT NOT NULL,
    tenant_type_id  SMALLINT, -- FK to identity.tenant_types (added later to avoid circular dependency)
    tenant_config   JSONB, -- Flexible configuration: {"branding": {...}, "limits": {...}}
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    tenant_name_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(tenant_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- NULL for system-created tenants
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete
    
    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_tenants PRIMARY KEY (tenant_id),
    
    -- NOTE: FK to tenant_types and app_users are added after all tables are created
    -- to avoid circular dependencies
    -- CONSTRAINT fk_tenants_type FOREIGN KEY (tenant_type_id)
    --     REFERENCES identity.tenant_types (type_id),
    -- 
    -- CONSTRAINT fk_tenants_created FOREIGN KEY (created_by)
    --     REFERENCES identity.app_users (user_id),
    -- 
    -- CONSTRAINT fk_tenants_updated FOREIGN KEY (updated_by)
    --     REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_tenants_name CHECK (LENGTH(tenant_name) >= 2),
    CONSTRAINT ck_tenants_tenant_config CHECK (tenant_config IS NULL OR jsonb_typeof(tenant_config) = 'object')
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_tenants_type ON identity.tenants(tenant_type_id);
CREATE INDEX IF NOT EXISTS idx_tenants_name_normalized ON identity.tenants(tenant_name_normalized);
CREATE INDEX IF NOT EXISTS idx_tenants_active ON identity.tenants(is_active) WHERE is_active = TRUE;
CREATE INDEX IF NOT EXISTS idx_tenants_deleted_at ON identity.tenants(deleted_at) WHERE deleted_at IS NULL;

-- 5.1. Soft Delete Unique Index (UUID can be reused after soft delete)
CREATE UNIQUE INDEX IF NOT EXISTS idx_tenants_uuid_active ON identity.tenants(tenant_uuid) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_tenants_updated_at
BEFORE UPDATE ON identity.tenants
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE identity.tenants ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Self Access" ON identity.tenants
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE identity.tenants IS 'Root multi-tenant entity: Schools, Organizations, or Individual accounts';
COMMENT ON COLUMN identity.tenants.tenant_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN identity.tenants.tenant_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN identity.tenants.tenant_type_id IS 'FK to identity.tenant_types: defines tenant classification and feature limits';
COMMENT ON COLUMN identity.tenants.tenant_config IS 'JSON config: {"branding": {"logo": url}, "limits": {"max_students": 100}}. Must be object type';
