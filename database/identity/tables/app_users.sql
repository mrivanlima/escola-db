-- =====================================================
-- TABLE: identity.app_users
-- Description: Application users (Parents, Teachers, Admins)
-- Integration: Links to Supabase Auth (auth.users)
-- =====================================================

CREATE TABLE IF NOT EXISTS identity.app_users (
    -- 1. IDs Híbridos
    user_id         INTEGER GENERATED ALWAYS AS IDENTITY,
    user_uuid       UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Supabase Auth Integration
    auth_user_id    UUID NOT NULL, -- References Supabase auth.users.id
    
    -- 3. Business Data
    full_name       TEXT NOT NULL,
    email           TEXT NOT NULL,
    user_role_id    SMALLINT NOT NULL, -- FK to identity.user_roles
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    user_config     JSONB, -- Preferences: {"language": "pt-BR", "notifications": true}
    
    -- 3.1. Normalized columns for search
    full_name_normalized    TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(full_name))) STORED,
    email_normalized        TEXT GENERATED ALWAYS AS (LOWER(email)) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete
    
    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_app_users PRIMARY KEY (user_id),
    
    CONSTRAINT fk_app_users_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_app_users_role FOREIGN KEY (user_role_id)
        REFERENCES identity.user_roles (role_id),
    
    CONSTRAINT fk_app_users_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_app_users_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_app_users_email CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    CONSTRAINT ck_app_users_user_config CHECK (user_config IS NULL OR jsonb_typeof(user_config) = 'object')
);

-- 6. Indexes
CREATE INDEX IF NOT EXISTS idx_app_users_tenant ON identity.app_users(tenant_id);
CREATE INDEX IF NOT EXISTS idx_app_users_auth ON identity.app_users(auth_user_id);
CREATE INDEX IF NOT EXISTS idx_app_users_email ON identity.app_users(email);
CREATE INDEX IF NOT EXISTS idx_app_users_role ON identity.app_users(user_role_id);
CREATE INDEX IF NOT EXISTS idx_app_users_full_name_normalized ON identity.app_users(full_name_normalized);
CREATE INDEX IF NOT EXISTS idx_app_users_email_normalized ON identity.app_users(email_normalized);
CREATE INDEX IF NOT EXISTS idx_app_users_deleted_at ON identity.app_users(deleted_at) WHERE deleted_at IS NULL;

-- 6.1. Soft Delete Unique Indexes (Allow reuse after soft delete)
CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_uuid_active ON identity.app_users(user_uuid) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_auth_active ON identity.app_users(auth_user_id) WHERE deleted_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS idx_app_users_email_tenant_active ON identity.app_users(email, tenant_id) WHERE deleted_at IS NULL;

-- 7. Trigger for Updated At
CREATE TRIGGER trg_app_users_updated_at
BEFORE UPDATE ON identity.app_users
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE identity.app_users ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON identity.app_users
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE identity.app_users IS 'Application users: Parents, Teachers, Admins. Links to Supabase Auth';
COMMENT ON COLUMN identity.app_users.user_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN identity.app_users.user_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN identity.app_users.auth_user_id IS 'Foreign Key to Supabase auth.users.id';
COMMENT ON COLUMN identity.app_users.user_role_id IS 'FK to identity.user_roles: defines user role and permissions';
COMMENT ON COLUMN identity.app_users.user_config IS 'JSON preferences: {"language": "pt-BR", "theme": "dark", "notifications": true}. Must be object type';
