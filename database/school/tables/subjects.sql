-- =====================================================
-- TABLE: school.subjects
-- Description: Lookup table for teaching subjects
-- Scope: Reference data for subjects taught
-- =====================================================

CREATE TABLE IF NOT EXISTS school.subjects (
    -- 1. IDs Híbridos
    subject_id      INTEGER GENERATED ALWAYS AS IDENTITY,
    subject_uuid    UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    subject_name    TEXT NOT NULL,
    description     TEXT,
    grade_level     TEXT, -- "pre-k", "kindergarten", "1st-grade", etc.
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    subject_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(subject_name))) STORED,
    grade_level_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(grade_level, '')))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_subjects PRIMARY KEY (subject_id),
    CONSTRAINT uq_subjects_uuid UNIQUE (subject_uuid),
    CONSTRAINT uq_subjects_name UNIQUE (subject_name),
    
    CONSTRAINT fk_subjects_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_subjects_name CHECK (LENGTH(subject_name) >= 2)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_subjects_name_normalized ON school.subjects(subject_name_normalized);
CREATE INDEX IF NOT EXISTS idx_subjects_grade_level_normalized ON school.subjects(grade_level_normalized);
CREATE INDEX IF NOT EXISTS idx_subjects_deleted_at ON school.subjects(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_subjects_updated_at
BEFORE UPDATE ON school.subjects
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.subjects ENABLE ROW LEVEL SECURITY;

-- Subjects are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON school.subjects
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON school.subjects
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.subjects IS 'Reference table for teaching subjects (e.g., "Mathematics", "Reading", "Science", "Art")';
COMMENT ON COLUMN school.subjects.subject_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.subjects.subject_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.subjects.subject_name IS 'Display name of subject';
COMMENT ON COLUMN school.subjects.grade_level IS 'Target grade level for this subject (optional)';
