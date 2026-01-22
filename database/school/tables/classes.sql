-- =====================================================
-- TABLE: school.classes
-- Description: School classes/groups (e.g., "Pre-K A", "1st Grade Morning")
-- Scope: B2B School feature (not for individual users)
-- =====================================================

CREATE TABLE IF NOT EXISTS school.classes (
    -- 1. IDs Híbridos
    class_id        INTEGER GENERATED ALWAYS AS IDENTITY,
    class_uuid      UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    class_name      TEXT NOT NULL, -- "Pre-K A", "1st Grade Morning"
    grade_level     TEXT, -- "pre-k", "1st-grade", etc.
    school_year     TEXT NOT NULL, -- "2024/2025"
    max_students    INTEGER,
    class_config    JSONB, -- Schedule, room number, etc.: {"room": "101", "schedule": "08:00-12:00"}
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    class_name_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(class_name))) STORED,
    grade_level_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(grade_level, '')))) STORED,
    school_year_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(school_year))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_classes PRIMARY KEY (class_id),
    CONSTRAINT uq_classes_uuid UNIQUE (class_uuid),
    
    CONSTRAINT fk_classes_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_classes_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_classes_name CHECK (LENGTH(class_name) >= 2),
    CONSTRAINT ck_classes_max_students CHECK (max_students IS NULL OR max_students > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_classes_tenant ON school.classes(tenant_id);
CREATE INDEX IF NOT EXISTS idx_classes_school_year ON school.classes(school_year);
CREATE INDEX IF NOT EXISTS idx_classes_name_normalized ON school.classes(class_name_normalized);
CREATE INDEX IF NOT EXISTS idx_classes_grade_level_normalized ON school.classes(grade_level_normalized);
CREATE INDEX IF NOT EXISTS idx_classes_deleted_at ON school.classes(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_classes_updated_at
BEFORE UPDATE ON school.classes
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE school.classes ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON school.classes
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE school.classes IS 'School classes/groups (B2B feature): Pre-K A, 1st Grade, etc.';
COMMENT ON COLUMN school.classes.class_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.classes.class_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.classes.class_config IS 'JSON: {"room": "101", "schedule": "08:00-12:00", "notes": "..."}';
COMMENT ON COLUMN school.classes.school_year IS 'Academic year: "2024/2025", "2025/2026"';
