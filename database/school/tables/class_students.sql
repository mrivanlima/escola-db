-- =====================================================
-- TABLE: school.class_students
-- Description: Many-to-Many relationship between classes and students
-- Business Rule: A student can be in multiple classes, a class has multiple students
-- =====================================================

CREATE TABLE IF NOT EXISTS school.class_students (
    -- 1. IDs
    class_student_id    INTEGER GENERATED ALWAYS AS IDENTITY,
    class_id            INTEGER NOT NULL,
    student_id          INTEGER NOT NULL,
    tenant_id           INTEGER NOT NULL,
    
    -- 2. Business Data
    enrollment_date     DATE NOT NULL DEFAULT CURRENT_DATE,
    status_id           SMALLINT NOT NULL, -- FK to school.enrollment_statuses (default 'active')
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_class_students PRIMARY KEY (class_student_id),
    CONSTRAINT uq_class_students_pair UNIQUE (class_id, student_id),
    
    CONSTRAINT fk_class_students_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_class_students_class FOREIGN KEY (class_id)
        REFERENCES school.classes (class_id),
    
    CONSTRAINT fk_class_students_student FOREIGN KEY (student_id)
        REFERENCES school.students (student_id),
    
    CONSTRAINT fk_class_students_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_class_students_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_class_students_status FOREIGN KEY (status_id)
        REFERENCES school.enrollment_statuses (status_id)
);

-- 5. Indexes
CREATE INDEX idx_class_students_tenant ON school.class_students(tenant_id);
CREATE INDEX idx_class_students_class ON school.class_students(class_id);
CREATE INDEX idx_class_students_student ON school.class_students(student_id);
CREATE INDEX idx_class_students_status ON school.class_students(status_id);
CREATE INDEX idx_class_students_deleted_at ON school.class_students(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_class_students_updated_at
BEFORE UPDATE ON school.class_students
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.class_students ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON school.class_students
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.class_students IS 'Many-to-Many: Links students to their classes';
COMMENT ON COLUMN school.class_students.status_id IS 'FK to school.enrollment_statuses: current enrollment status';
COMMENT ON COLUMN school.class_students.enrollment_date IS 'Date student was enrolled in this class';
