-- =====================================================
-- TABLE: identity.tenant_types
-- Description: Lookup table for tenant types
-- Purpose: Normalized reference data for tenant classification and limits
-- =====================================================

CREATE TABLE IF NOT EXISTS identity.tenant_types (
    -- 1. IDs Híbridos
    type_id         SMALLINT GENERATED ALWAYS AS IDENTITY,
    type_uuid       UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    type_code       TEXT NOT NULL, -- 'school', 'individual', 'enterprise'
    type_name       TEXT NOT NULL, -- 'School', 'Individual', 'Enterprise'
    description     TEXT,
    max_students    INTEGER, -- Maximum students allowed (NULL = unlimited)
    max_teachers    INTEGER, -- Maximum teachers allowed (NULL = unlimited)
    max_storage_gb  INTEGER, -- Maximum storage in GB (NULL = unlimited)
    has_analytics   BOOLEAN NOT NULL DEFAULT FALSE,
    has_api_access  BOOLEAN NOT NULL DEFAULT FALSE,
    has_white_label BOOLEAN NOT NULL DEFAULT FALSE,
    pricing_tier    TEXT, -- 'free', 'basic', 'premium', 'enterprise'
    icon_name       TEXT, -- UI icon identifier
    color_code      TEXT, -- Hex color for UI badges
    display_order   SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    type_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(type_code)))
    ) STORED,
    type_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(type_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_tenant_types PRIMARY KEY (type_id),
    CONSTRAINT uq_tenant_types_uuid UNIQUE (type_uuid),
    CONSTRAINT uq_tenant_types_code UNIQUE (tenant_id, type_code),
    
    -- NOTE: FK constraint to tenants is added after tenants table is created
    -- to avoid circular dependency during initial build
    -- CONSTRAINT fk_tenant_types_tenant FOREIGN KEY (tenant_id) 
    --     REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_tenant_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_tenant_types_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_tenant_types_max_students CHECK (max_students IS NULL OR max_students > 0),
    CONSTRAINT ck_tenant_types_max_teachers CHECK (max_teachers IS NULL OR max_teachers > 0),
    CONSTRAINT ck_tenant_types_max_storage CHECK (max_storage_gb IS NULL OR max_storage_gb > 0),
    CONSTRAINT ck_tenant_types_pricing CHECK (pricing_tier IS NULL OR pricing_tier IN ('free', 'basic', 'premium', 'enterprise')),
    CONSTRAINT ck_tenant_types_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_tenant_types_tenant ON identity.tenant_types(tenant_id);
CREATE INDEX idx_tenant_types_code_normalized ON identity.tenant_types(type_code_normalized);
CREATE INDEX idx_tenant_types_pricing_tier ON identity.tenant_types(pricing_tier);
CREATE INDEX idx_tenant_types_deleted_at ON identity.tenant_types(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_tenant_types_display_order ON identity.tenant_types(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_tenant_types_updated_at
BEFORE UPDATE ON identity.tenant_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE identity.tenant_types ENABLE ROW LEVEL SECURITY;

-- Policy: Public Read (reference data accessible to all authenticated users)
CREATE POLICY "Public Read" ON identity.tenant_types
    FOR SELECT
    USING (deleted_at IS NULL);

-- Policy: Super Admin Write (only super admins can modify)
-- DEFERRED: Commented out to avoid circular dependency with app_users
-- This policy will be added in Phase 3 after app_users table exists
-- CREATE POLICY "Super Admin Write" ON identity.tenant_types
--     FOR ALL
--     USING (
--         EXISTS (
--             SELECT 1 FROM identity.app_users
--             WHERE user_id = current_setting('app.current_user_id', TRUE)::INTEGER
--             AND user_role IN ('super_admin')
--         )
--     );

-- 9. Metadata (Documentation)
COMMENT ON TABLE identity.tenant_types IS 'Lookup: defines tenant types with feature limits and pricing tiers';
COMMENT ON COLUMN identity.tenant_types.type_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN identity.tenant_types.type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN identity.tenant_types.type_code IS 'Unique code: school, individual, enterprise';
COMMENT ON COLUMN identity.tenant_types.max_students IS 'Maximum students allowed (NULL = unlimited)';
COMMENT ON COLUMN identity.tenant_types.max_teachers IS 'Maximum teachers allowed (NULL = unlimited)';
COMMENT ON COLUMN identity.tenant_types.max_storage_gb IS 'Maximum storage in GB (NULL = unlimited)';
COMMENT ON COLUMN identity.tenant_types.has_analytics IS 'Access to advanced analytics and reporting features';
COMMENT ON COLUMN identity.tenant_types.has_api_access IS 'Access to API for integrations';
COMMENT ON COLUMN identity.tenant_types.has_white_label IS 'Ability to white-label the platform';
COMMENT ON COLUMN identity.tenant_types.pricing_tier IS 'Pricing tier: free, basic, premium, enterprise';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO identity.tenant_types (tenant_id, type_code, type_name, description, max_students, max_teachers, max_storage_gb, has_analytics, has_api_access, has_white_label, pricing_tier, icon_name, color_code, display_order, created_by)
-- VALUES
--     (1, 'individual', 'Individual', 'Personal account for individual educators or parents', 5, 1, 1, FALSE, FALSE, FALSE, 'free', 'person', '#2196F3', 1, 1),
--     (1, 'school', 'School', 'Educational institution with multiple teachers and students', 500, 50, 50, TRUE, FALSE, FALSE, 'premium', 'school', '#4CAF50', 2, 1),
--     (1, 'enterprise', 'Enterprise', 'Large organization with custom requirements and unlimited resources', NULL, NULL, NULL, TRUE, TRUE, TRUE, 'enterprise', 'business', '#FF9800', 3, 1)
-- ON CONFLICT (tenant_id, type_code) DO NOTHING;
