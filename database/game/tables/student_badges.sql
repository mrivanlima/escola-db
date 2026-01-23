-- =====================================================
-- TABLE: game.student_badges
-- Description: Many-to-Many linking students to earned badges
-- Purpose: Tracks badge collection per student
-- =====================================================

CREATE TABLE IF NOT EXISTS game.student_badges (
    -- 1. IDs
    student_badge_id    BIGINT GENERATED ALWAYS AS IDENTITY, -- BIGINT for potential high volume
    student_id          INTEGER NOT NULL,
    badge_id            INTEGER NOT NULL,
    tenant_id           INTEGER NOT NULL,
    
    -- 2. Business Data
    earned_at           TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    progress_id         BIGINT, -- Optional link to the progress entry that triggered the badge
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_student_badges PRIMARY KEY (student_badge_id),
    CONSTRAINT uq_student_badges_pair UNIQUE (student_id, badge_id), -- Each student can earn each badge once
    
    CONSTRAINT fk_student_badges_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_student_badges_student FOREIGN KEY (student_id)
        REFERENCES school.students (student_id),
    
    CONSTRAINT fk_student_badges_badge FOREIGN KEY (badge_id)
        REFERENCES game.badges (badge_id),
    
    CONSTRAINT fk_student_badges_progress FOREIGN KEY (progress_id)
        REFERENCES game.student_progress (progress_id),
    
    CONSTRAINT fk_student_badges_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id)
);

-- 5. Indexes
CREATE INDEX idx_student_badges_tenant ON game.student_badges(tenant_id);
CREATE INDEX idx_student_badges_student ON game.student_badges(student_id);
CREATE INDEX idx_student_badges_badge ON game.student_badges(badge_id);
CREATE INDEX idx_student_badges_earned ON game.student_badges(earned_at);
CREATE INDEX idx_student_badges_deleted_at ON game.student_badges(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_student_badges_updated_at
BEFORE UPDATE ON game.student_badges
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE game.student_badges ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON game.student_badges
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 8. Metadata (Documentation)
COMMENT ON TABLE game.student_badges IS 'Many-to-Many: Links students to their earned badges';
COMMENT ON COLUMN game.student_badges.earned_at IS 'Timestamp when badge was earned';
COMMENT ON COLUMN game.student_badges.progress_id IS 'Optional: The progress entry that triggered this badge unlock';
