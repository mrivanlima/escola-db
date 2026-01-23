-- =====================================================
-- TABLE: school.teacher_certifications
-- Description: Junction table for teacher certifications (Many-to-Many)
-- Links: school.teachers ↔ school.certifications
-- =====================================================

CREATE TABLE IF NOT EXISTS school.teacher_certifications (
    -- 1. Composite PK (No surrogate ID needed for pure junction tables)
    teacher_id          INTEGER NOT NULL,
    certification_id    INTEGER NOT NULL,
    
    -- 2. Business Data (Relationship-specific attributes)
    obtained_date       DATE,
    expiry_date         DATE,
    credential_number   TEXT,
    notes               TEXT,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_teacher_certifications PRIMARY KEY (teacher_id, certification_id),
    
    CONSTRAINT fk_teacher_certifications_teacher FOREIGN KEY (teacher_id)
        REFERENCES school.teachers (teacher_id) ON DELETE CASCADE,
    
    CONSTRAINT fk_teacher_certifications_certification FOREIGN KEY (certification_id)
        REFERENCES school.certifications (certification_id),
    
    CONSTRAINT fk_teacher_certifications_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_teacher_certifications_dates CHECK (expiry_date IS NULL OR expiry_date >= obtained_date)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_teacher_certifications_teacher ON school.teacher_certifications(teacher_id);
CREATE INDEX IF NOT EXISTS idx_teacher_certifications_certification ON school.teacher_certifications(certification_id);
CREATE INDEX IF NOT EXISTS idx_teacher_certifications_expiry ON school.teacher_certifications(expiry_date) WHERE expiry_date IS NOT NULL;
CREATE INDEX IF NOT EXISTS idx_teacher_certifications_deleted_at ON school.teacher_certifications(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_teacher_certifications_updated_at
BEFORE UPDATE ON school.teacher_certifications
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.teacher_certifications ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Teacher Access" ON school.teacher_certifications
    USING (
        teacher_id IN (
            SELECT teacher_id FROM school.teachers 
            WHERE tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER
        )
    );

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.teacher_certifications IS 'Junction table linking teachers to their professional certifications (Many-to-Many)';
COMMENT ON COLUMN school.teacher_certifications.teacher_id IS 'FK to school.teachers';
COMMENT ON COLUMN school.teacher_certifications.certification_id IS 'FK to school.certifications';
COMMENT ON COLUMN school.teacher_certifications.obtained_date IS 'Date when certification was obtained';
COMMENT ON COLUMN school.teacher_certifications.expiry_date IS 'Date when certification expires (NULL if permanent)';
COMMENT ON COLUMN school.teacher_certifications.credential_number IS 'Official credential/license number';
