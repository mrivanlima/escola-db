-- =====================================================
-- TABLE: identity.user_roles
-- Description: Lookup table for user role types
-- Purpose: Normalized reference data for application user roles
-- =====================================================

CREATE TABLE IF NOT EXISTS identity.user_roles (
    -- 1. IDs Híbridos
    role_id         SMALLINT GENERATED ALWAYS AS IDENTITY,
    role_uuid       UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    role_code       TEXT NOT NULL, -- 'parent', 'teacher', 'admin', 'super_admin'
    role_name       TEXT NOT NULL, -- 'Parent', 'Teacher', 'Administrator', 'Super Administrator'
    description     TEXT,
    permission_level INTEGER NOT NULL, -- Hierarchical level: 1=parent, 10=teacher, 50=admin, 100=super_admin
    can_manage_students BOOLEAN NOT NULL DEFAULT FALSE,
    can_manage_content BOOLEAN NOT NULL DEFAULT FALSE,
    can_manage_users BOOLEAN NOT NULL DEFAULT FALSE,
    can_manage_tenant BOOLEAN NOT NULL DEFAULT FALSE,
    icon_name       TEXT, -- UI icon identifier
    color_code      TEXT, -- Hex color for UI badges
    display_order   SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    role_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(role_code)))
    ) STORED,
    role_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(role_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_user_roles PRIMARY KEY (role_id),
    CONSTRAINT uq_user_roles_uuid UNIQUE (role_uuid),
    CONSTRAINT uq_user_roles_code UNIQUE (tenant_id, role_code),
    
    CONSTRAINT fk_user_roles_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    -- NOTE: FK to app_users deferred to avoid circular dependency
    -- CONSTRAINT fk_user_roles_created FOREIGN KEY (created_by)
    --     REFERENCES identity.app_users (user_id),
    -- 
    -- CONSTRAINT fk_user_roles_updated FOREIGN KEY (updated_by)
    --     REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_user_roles_permission_level CHECK (permission_level > 0 AND permission_level <= 100),
    CONSTRAINT ck_user_roles_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_user_roles_tenant ON identity.user_roles(tenant_id);
CREATE INDEX idx_user_roles_code_normalized ON identity.user_roles(role_code_normalized);
CREATE INDEX idx_user_roles_permission_level ON identity.user_roles(permission_level);
CREATE INDEX idx_user_roles_deleted_at ON identity.user_roles(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_user_roles_display_order ON identity.user_roles(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_user_roles_updated_at
BEFORE UPDATE ON identity.user_roles
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE identity.user_roles ENABLE ROW LEVEL SECURITY;

-- Policy: Public Read (reference data accessible to all authenticated users)
CREATE POLICY "Public Read" ON identity.user_roles
    FOR SELECT
    USING (deleted_at IS NULL);

-- Policy: Admin Write (only admins can modify)
-- DEFERRED: Commented out to avoid circular dependency with app_users
-- This policy will be added in Phase 3 after app_users table exists
-- CREATE POLICY "Admin Write" ON identity.user_roles
--     FOR ALL
--     USING (
--         EXISTS (
--             SELECT 1 FROM identity.app_users
--             WHERE user_id = current_setting('app.current_user_id', TRUE)::INTEGER
--             AND user_role IN ('admin', 'super_admin')
--         )
--     );

-- 9. Metadata (Documentation)
COMMENT ON TABLE identity.user_roles IS 'Lookup: defines user roles with hierarchical permissions';
COMMENT ON COLUMN identity.user_roles.role_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN identity.user_roles.role_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN identity.user_roles.role_code IS 'Unique code: parent, teacher, admin, super_admin';
COMMENT ON COLUMN identity.user_roles.permission_level IS 'Hierarchical permission level (1-100): higher = more privileges';
COMMENT ON COLUMN identity.user_roles.can_manage_students IS 'Permission to create/edit/delete student records';
COMMENT ON COLUMN identity.user_roles.can_manage_content IS 'Permission to create/edit/delete educational content';
COMMENT ON COLUMN identity.user_roles.can_manage_users IS 'Permission to create/edit/delete user accounts';
COMMENT ON COLUMN identity.user_roles.can_manage_tenant IS 'Permission to manage tenant settings and configuration';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO identity.user_roles (tenant_id, role_code, role_name, description, permission_level, can_manage_students, can_manage_content, can_manage_users, can_manage_tenant, icon_name, color_code, display_order, created_by)
-- VALUES
--     (1, 'parent', 'Parent', 'Guardian with access to their children''s progress', 1, FALSE, FALSE, FALSE, FALSE, 'people', '#2196F3', 1, 1),
--     (1, 'teacher', 'Teacher', 'Educator who manages content and student progress', 10, TRUE, TRUE, FALSE, FALSE, 'school', '#4CAF50', 2, 1),
--     (1, 'admin', 'Administrator', 'School administrator with full tenant management', 50, TRUE, TRUE, TRUE, TRUE, 'admin_panel_settings', '#FF9800', 3, 1),
--     (1, 'super_admin', 'Super Administrator', 'Platform administrator with cross-tenant access', 100, TRUE, TRUE, TRUE, TRUE, 'shield', '#F44336', 4, 1)
-- ON CONFLICT (tenant_id, role_code) DO NOTHING;
