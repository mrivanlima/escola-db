-- =====================================================
-- TABLE: game.student_progress
-- Description: Tracks student progress through activities
-- Purpose: High-volume transactional data for learning analytics
-- =====================================================

CREATE TABLE IF NOT EXISTS game.student_progress (
    -- 1. IDs Híbridos
    progress_id     BIGINT GENERATED ALWAYS AS IDENTITY, -- BIGINT for high volume
    progress_uuid   UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    student_id      INTEGER NOT NULL,
    activity_id     INTEGER NOT NULL,
    
    -- 2. Business Data
    status          TEXT NOT NULL, -- 'started', 'in_progress', 'completed', 'abandoned'
    score           INTEGER, -- Score or percentage (0-100)
    time_spent_seconds INTEGER, -- Time spent on activity
    attempts_count  INTEGER NOT NULL DEFAULT 1,
    completion_date TIMESTAMPTZ,
    progress_data   JSONB, -- Activity-specific data: {"answers": [...], "mistakes": 3}
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_student_progress PRIMARY KEY (progress_id),
    CONSTRAINT uq_student_progress_uuid UNIQUE (progress_uuid),
    
    CONSTRAINT fk_student_progress_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_student_progress_student FOREIGN KEY (student_id)
        REFERENCES school.students (student_id),
    
    CONSTRAINT fk_student_progress_activity FOREIGN KEY (activity_id)
        REFERENCES content.activities (activity_id),
    
    CONSTRAINT fk_student_progress_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_student_progress_status CHECK (status IN ('started', 'in_progress', 'completed', 'abandoned')),
    CONSTRAINT ck_student_progress_score CHECK (score IS NULL OR (score >= 0 AND score <= 100)),
    CONSTRAINT ck_student_progress_time CHECK (time_spent_seconds IS NULL OR time_spent_seconds >= 0),
    CONSTRAINT ck_student_progress_attempts CHECK (attempts_count > 0)
);

-- 5. Indexes
CREATE INDEX idx_student_progress_tenant ON game.student_progress(tenant_id);
CREATE INDEX idx_student_progress_student ON game.student_progress(student_id);
CREATE INDEX idx_student_progress_activity ON game.student_progress(activity_id);
CREATE INDEX idx_student_progress_status ON game.student_progress(status);
CREATE INDEX idx_student_progress_completion ON game.student_progress(completion_date);
CREATE INDEX idx_student_progress_deleted_at ON game.student_progress(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_student_progress_updated_at
BEFORE UPDATE ON game.student_progress
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE game.student_progress ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON game.student_progress
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE game.student_progress IS 'High-volume table: tracks student progress through activities';
COMMENT ON COLUMN game.student_progress.progress_id IS 'INTERNAL PK: BIGINT for high volume. Never expose to API';
COMMENT ON COLUMN game.student_progress.progress_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN game.student_progress.status IS 'Progress status: started, in_progress, completed, abandoned';
COMMENT ON COLUMN game.student_progress.progress_data IS 'JSON: Activity-specific tracking (answers, mistakes, paths taken)';
COMMENT ON COLUMN game.student_progress.score IS 'Score or percentage (0-100). NULL if not applicable';
