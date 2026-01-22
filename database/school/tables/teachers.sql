-- =====================================================
-- TABLE: school.teachers
-- Description: Teachers assigned to classes
-- Links to: identity.app_users (user_role = 'teacher')
-- =====================================================

CREATE TABLE IF NOT EXISTS school.teachers (
    -- 1. IDs Híbridos
    teacher_id      INTEGER GENERATED ALWAYS AS IDENTITY,
    teacher_uuid    UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    user_id         INTEGER NOT NULL, -- Links to identity.app_users
    
    -- 2. Business Data
    specialization  TEXT, -- "Early Childhood Education", "Mathematics", etc.
    hire_date       DATE,
    teacher_config  JSONB, -- {"certifications": [...], "subjects": [...]}
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    specialization_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(specialization, '')))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_teachers PRIMARY KEY (teacher_id),
    CONSTRAINT uq_teachers_uuid UNIQUE (teacher_uuid),
    CONSTRAINT uq_teachers_user UNIQUE (user_id), -- 1:1 relationship with app_users
    
    CONSTRAINT fk_teachers_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_teachers_user FOREIGN KEY (user_id)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_teachers_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_teachers_tenant ON school.teachers(tenant_id);
CREATE INDEX IF NOT EXISTS idx_teachers_user ON school.teachers(user_id);
CREATE INDEX IF NOT EXISTS idx_teachers_specialization_normalized ON school.teachers(specialization_normalized);
CREATE INDEX IF NOT EXISTS idx_teachers_deleted_at ON school.teachers(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_teachers_updated_at
BEFORE UPDATE ON school.teachers
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.teachers ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON school.teachers
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.teachers IS 'Teachers linked to app_users (user_role = teacher)';
COMMENT ON COLUMN school.teachers.teacher_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.teachers.teacher_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.teachers.user_id IS 'FK to identity.app_users (1:1 relationship)';
COMMENT ON COLUMN school.teachers.teacher_config IS 'JSON: {"certifications": ["Early Ed"], "subjects": ["Math", "Reading"]}';
