-- =====================================================
-- TABLE: school.teacher_subjects
-- Description: Junction table for teacher subjects (Many-to-Many)
-- Links: school.teachers ↔ school.subjects
-- =====================================================

CREATE TABLE IF NOT EXISTS school.teacher_subjects (
    -- 1. Composite PK (No surrogate ID needed for pure junction tables)
    teacher_id      INTEGER NOT NULL,
    subject_id      INTEGER NOT NULL,
    
    -- 2. Business Data (Relationship-specific attributes)
    proficiency_level_id SMALLINT, -- FK to school.proficiency_levels
    years_experience    INTEGER,
    is_primary_subject  BOOLEAN DEFAULT FALSE, -- Flag for teacher's main subject
    notes               TEXT,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_teacher_subjects PRIMARY KEY (teacher_id, subject_id),
    
    CONSTRAINT fk_teacher_subjects_teacher FOREIGN KEY (teacher_id)
        REFERENCES school.teachers (teacher_id) ON DELETE CASCADE,
    
    CONSTRAINT fk_teacher_subjects_subject FOREIGN KEY (subject_id)
        REFERENCES school.subjects (subject_id),
    
    CONSTRAINT fk_teacher_subjects_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_teacher_subjects_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_teacher_subjects_proficiency FOREIGN KEY (proficiency_level_id)
        REFERENCES school.proficiency_levels (proficiency_level_id),
    
    CONSTRAINT ck_teacher_subjects_years CHECK (years_experience IS NULL OR years_experience >= 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_teacher_subjects_teacher ON school.teacher_subjects(teacher_id);
CREATE INDEX IF NOT EXISTS idx_teacher_subjects_subject ON school.teacher_subjects(subject_id);
CREATE INDEX IF NOT EXISTS idx_teacher_subjects_primary ON school.teacher_subjects(teacher_id, is_primary_subject) WHERE is_primary_subject = TRUE;
CREATE INDEX IF NOT EXISTS idx_teacher_subjects_deleted_at ON school.teacher_subjects(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_teacher_subjects_updated_at
BEFORE UPDATE ON school.teacher_subjects
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.teacher_subjects ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Teacher Access" ON school.teacher_subjects
    USING (
        teacher_id IN (
            SELECT teacher_id FROM school.teachers 
            WHERE tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER
        )
    );

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.teacher_subjects IS 'Junction table linking teachers to subjects they can teach (Many-to-Many)';
COMMENT ON COLUMN school.teacher_subjects.teacher_id IS 'FK to school.teachers';
COMMENT ON COLUMN school.teacher_subjects.subject_id IS 'FK to school.subjects';
COMMENT ON COLUMN school.teacher_subjects.proficiency_level_id IS 'FK to school.proficiency_levels: teacher proficiency in this subject';
COMMENT ON COLUMN school.teacher_subjects.years_experience IS 'Years of experience teaching this subject';
COMMENT ON COLUMN school.teacher_subjects.is_primary_subject IS 'TRUE if this is the teacher main/primary subject';
