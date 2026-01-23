-- =====================================================
-- TABLE: school.enrollment_statuses
-- Description: Lookup table for class enrollment statuses
-- Purpose: Normalized reference data for student enrollment states
-- =====================================================

CREATE TABLE IF NOT EXISTS school.enrollment_statuses (
    -- 1. IDs Híbridos
    status_id       SMALLINT GENERATED ALWAYS AS IDENTITY,
    status_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    status_code     TEXT NOT NULL, -- 'active', 'inactive', 'transferred'
    status_name     TEXT NOT NULL, -- 'Active', 'Inactive', 'Transferred'
    description     TEXT,
    allows_attendance BOOLEAN NOT NULL DEFAULT TRUE, -- Can student attend classes?
    allows_grading  BOOLEAN NOT NULL DEFAULT TRUE, -- Can student receive grades?
    is_final_state  BOOLEAN NOT NULL DEFAULT FALSE, -- TRUE for 'transferred'
    icon_name       TEXT, -- UI icon identifier
    color_code      TEXT, -- Hex color for UI badges
    display_order   SMALLINT NOT NULL,
    
    -- 3. Normalized Search Columns
    status_code_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(status_code)))
    ) STORED,
    status_name_normalized TEXT GENERATED ALWAYS AS (
        public.immutable_unaccent(LOWER(TRIM(status_name)))
    ) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_enrollment_statuses PRIMARY KEY (status_id),
    CONSTRAINT uq_enrollment_statuses_uuid UNIQUE (status_uuid),
    CONSTRAINT uq_enrollment_statuses_code UNIQUE (tenant_id, status_code),
    
    CONSTRAINT fk_enrollment_statuses_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_enrollment_statuses_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_enrollment_statuses_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_enrollment_statuses_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_enrollment_statuses_tenant ON school.enrollment_statuses(tenant_id);
CREATE INDEX idx_enrollment_statuses_code_normalized ON school.enrollment_statuses(status_code_normalized);
CREATE INDEX idx_enrollment_statuses_deleted_at ON school.enrollment_statuses(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_enrollment_statuses_display_order ON school.enrollment_statuses(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_enrollment_statuses_updated_at
BEFORE UPDATE ON school.enrollment_statuses
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.enrollment_statuses ENABLE ROW LEVEL SECURITY;

-- Policy: Tenant Isolation (enrollment statuses are tenant-specific)
CREATE POLICY "Tenant Isolation" ON school.enrollment_statuses
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.enrollment_statuses IS 'Lookup: defines enrollment status states for class-student relationships';
COMMENT ON COLUMN school.enrollment_statuses.status_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN school.enrollment_statuses.status_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.enrollment_statuses.status_code IS 'Unique code: active, inactive, transferred';
COMMENT ON COLUMN school.enrollment_statuses.allows_attendance IS 'Whether student can attend classes with this status';
COMMENT ON COLUMN school.enrollment_statuses.allows_grading IS 'Whether student can receive grades with this status';
COMMENT ON COLUMN school.enrollment_statuses.is_final_state IS 'TRUE if this is a terminal state (e.g., transferred)';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO school.enrollment_statuses (tenant_id, status_code, status_name, description, allows_attendance, allows_grading, is_final_state, icon_name, color_code, display_order, created_by)
-- VALUES
--     (1, 'active', 'Active', 'Student is actively enrolled and attending classes', TRUE, TRUE, FALSE, 'check_circle', '#4CAF50', 1, 1),
--     (1, 'inactive', 'Inactive', 'Student enrollment is temporarily inactive', FALSE, FALSE, FALSE, 'pause_circle', '#FF9800', 2, 1),
--     (1, 'transferred', 'Transferred', 'Student has been transferred to another class or school', FALSE, FALSE, TRUE, 'swap_horiz', '#2196F3', 3, 1)
-- ON CONFLICT (tenant_id, status_code) DO NOTHING;
