-- =====================================================
-- TABLE: game.progress_statuses
-- Description: Lookup table for student progress statuses
-- Purpose: Normalized reference data for progress tracking states
-- =====================================================

CREATE TABLE IF NOT EXISTS game.progress_statuses (
    -- 1. IDs Híbridos
    status_id       SMALLINT GENERATED ALWAYS AS IDENTITY,
    status_uuid     UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    
    -- 2. Business Data
    status_code     TEXT NOT NULL, -- 'started', 'in_progress', 'completed', 'abandoned'
    status_name     TEXT NOT NULL, -- 'Started', 'In Progress', 'Completed', 'Abandoned'
    description     TEXT,
    icon_name       TEXT, -- UI icon identifier
    color_code      TEXT, -- Hex color for UI (e.g., '#4CAF50' for completed)
    is_final_state  BOOLEAN NOT NULL DEFAULT FALSE, -- TRUE for 'completed' and 'abandoned'
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
    CONSTRAINT pk_progress_statuses PRIMARY KEY (status_id),
    CONSTRAINT uq_progress_statuses_uuid UNIQUE (status_uuid),
    CONSTRAINT uq_progress_statuses_code UNIQUE (tenant_id, status_code),
    
    CONSTRAINT fk_progress_statuses_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_progress_statuses_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_progress_statuses_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT ck_progress_statuses_color CHECK (color_code IS NULL OR color_code ~* '^#[0-9A-F]{6}$')
);

-- 6. Indexes
CREATE INDEX idx_progress_statuses_tenant ON game.progress_statuses(tenant_id);
CREATE INDEX idx_progress_statuses_code_normalized ON game.progress_statuses(status_code_normalized);
CREATE INDEX idx_progress_statuses_deleted_at ON game.progress_statuses(deleted_at) WHERE deleted_at IS NULL;
CREATE INDEX idx_progress_statuses_display_order ON game.progress_statuses(display_order);

-- 7. Trigger for Updated At
CREATE TRIGGER trg_progress_statuses_updated_at
BEFORE UPDATE ON game.progress_statuses
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE game.progress_statuses ENABLE ROW LEVEL SECURITY;

-- Policy: Public Read (reference data accessible to all authenticated users)
CREATE POLICY "Public Read" ON game.progress_statuses
    FOR SELECT
    USING (deleted_at IS NULL);

-- Policy: Admin Write (only admins can modify)
-- DEFERRED: Commented out to avoid circular dependency with app_users
-- This policy will be added in Phase 3 after app_users table exists
-- CREATE POLICY "Admin Write" ON game.progress_statuses
--     FOR ALL
--     USING (
--         EXISTS (
--             SELECT 1 FROM identity.app_users
--             WHERE user_id = current_setting('app.current_user_id', TRUE)::INTEGER
--             AND role = 'admin'
--         )
--     );

-- 9. Metadata (Documentation)
COMMENT ON TABLE game.progress_statuses IS 'Lookup: defines possible states for student activity progress';
COMMENT ON COLUMN game.progress_statuses.status_id IS 'INTERNAL PK: SMALLINT for lookup table. Never expose to API';
COMMENT ON COLUMN game.progress_statuses.status_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN game.progress_statuses.status_code IS 'Unique code: started, in_progress, completed, abandoned';
COMMENT ON COLUMN game.progress_statuses.is_final_state IS 'TRUE if this status represents a final state (completed/abandoned)';
COMMENT ON COLUMN game.progress_statuses.color_code IS 'UI color in hex format (e.g., #4CAF50)';

-- 10. Seed Data
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO game.progress_statuses (tenant_id, status_code, status_name, description, icon_name, color_code, is_final_state, display_order, created_by)
-- VALUES
--     (1, 'started', 'Started', 'Student has initiated the activity', 'play_circle', '#2196F3', FALSE, 1, 1),
--     (1, 'in_progress', 'In Progress', 'Student is actively working on the activity', 'hourglass_empty', '#FF9800', FALSE, 2, 1),
--     (1, 'completed', 'Completed', 'Student has successfully completed the activity', 'check_circle', '#4CAF50', TRUE, 3, 1),
--     (1, 'abandoned', 'Abandoned', 'Student has abandoned or quit the activity', 'cancel', '#F44336', TRUE, 4, 1)
-- ON CONFLICT (tenant_id, status_code) DO NOTHING;
