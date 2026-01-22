-- =====================================================
-- TABLE: school.student_guardians
-- Description: Many-to-Many relationship between students and guardians
-- Business Rule: A student can have multiple guardians, a guardian can have multiple students
-- =====================================================

CREATE TABLE IF NOT EXISTS school.student_guardians (
    -- 1. IDs
    student_guardian_id     INTEGER GENERATED ALWAYS AS IDENTITY,
    student_id              INTEGER NOT NULL,
    guardian_id             INTEGER NOT NULL,
    tenant_id               INTEGER NOT NULL,
    
    -- 2. Business Data
    relationship_notes      TEXT, -- Additional context: "Lives with mother"
    is_authorized_pickup    BOOLEAN NOT NULL DEFAULT TRUE, -- Can pick up child from school
    
    -- 2.1. Normalized columns for search
    relationship_notes_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(relationship_notes, '')))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_student_guardians PRIMARY KEY (student_guardian_id),
    CONSTRAINT uq_student_guardians_pair UNIQUE (student_id, guardian_id),
    
    CONSTRAINT fk_student_guardians_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_student_guardians_student FOREIGN KEY (student_id)
        REFERENCES school.students (student_id),
    
    CONSTRAINT fk_student_guardians_guardian FOREIGN KEY (guardian_id)
        REFERENCES school.guardians (guardian_id),
    
    CONSTRAINT fk_student_guardians_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_student_guardians_tenant ON school.student_guardians(tenant_id);
CREATE INDEX IF NOT EXISTS idx_student_guardians_student ON school.student_guardians(student_id);
CREATE INDEX IF NOT EXISTS idx_student_guardians_guardian ON school.student_guardians(guardian_id);
CREATE INDEX IF NOT EXISTS idx_student_guardians_notes_normalized ON school.student_guardians(relationship_notes_normalized);
CREATE INDEX IF NOT EXISTS idx_student_guardians_deleted_at ON school.student_guardians(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_student_guardians_updated_at
BEFORE UPDATE ON school.student_guardians
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.student_guardians ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON school.student_guardians
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.student_guardians IS 'Many-to-Many: Links students with their guardians/parents';
COMMENT ON COLUMN school.student_guardians.is_authorized_pickup IS 'Authorization to pick up child from school premises';
COMMENT ON COLUMN school.student_guardians.relationship_notes IS 'Additional context: custody arrangements, living situation, etc.';
