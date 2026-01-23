-- =====================================================
-- TABLE: content.usage_contexts
-- Description: Lookup table for resource usage contexts within activities
-- Scope: Reference data for how resources are used in activities
-- =====================================================

CREATE TABLE IF NOT EXISTS content.usage_contexts (
    -- 1. IDs Híbridos
    usage_context_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    usage_context_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    usage_context_code  TEXT NOT NULL, -- 'instruction', 'question', 'answer', 'feedback'
    usage_context_name  TEXT NOT NULL, -- Display name: "Instruction", "Question", "Answer", "Feedback"
    description         TEXT,
    display_order       INTEGER NOT NULL DEFAULT 0,
    icon_name           TEXT, -- Icon identifier for frontend
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    usage_context_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(usage_context_code))) STORED,
    usage_context_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(usage_context_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_usage_contexts PRIMARY KEY (usage_context_id),
    CONSTRAINT uq_usage_contexts_uuid UNIQUE (usage_context_uuid),
    CONSTRAINT uq_usage_contexts_code UNIQUE (usage_context_code),
    
    CONSTRAINT fk_usage_contexts_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_usage_contexts_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_usage_contexts_code CHECK (LENGTH(usage_context_code) >= 2),
    CONSTRAINT ck_usage_contexts_name CHECK (LENGTH(usage_context_name) >= 2)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_usage_contexts_code_normalized ON content.usage_contexts(usage_context_code_normalized);
CREATE INDEX IF NOT EXISTS idx_usage_contexts_name_normalized ON content.usage_contexts(usage_context_name_normalized);
CREATE INDEX IF NOT EXISTS idx_usage_contexts_display_order ON content.usage_contexts(display_order);
CREATE INDEX IF NOT EXISTS idx_usage_contexts_deleted_at ON content.usage_contexts(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_usage_contexts_updated_at
BEFORE UPDATE ON content.usage_contexts
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE content.usage_contexts ENABLE ROW LEVEL SECURITY;

-- Usage contexts are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON content.usage_contexts
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON content.usage_contexts
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE content.usage_contexts IS 'Reference table for resource usage contexts (instruction, question, answer, feedback)';
COMMENT ON COLUMN content.usage_contexts.usage_context_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN content.usage_contexts.usage_context_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.usage_contexts.usage_context_code IS 'Code identifier: instruction, question, answer, feedback, hint, example';
COMMENT ON COLUMN content.usage_contexts.usage_context_name IS 'Human-readable display name';
COMMENT ON COLUMN content.usage_contexts.display_order IS 'Order for displaying contexts in UI';
COMMENT ON COLUMN content.usage_contexts.icon_name IS 'Icon identifier for frontend rendering';

-- 9. Seed Data (Common Usage Contexts)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO content.usage_contexts (usage_context_code, usage_context_name, description, display_order, icon_name, created_by)
-- VALUES 
--     ('instruction', 'Instruction', 'Resource used to explain instructions or guide the user', 1, 'info_icon', 1),
--     ('question', 'Question', 'Resource that presents a question or challenge', 2, 'question_icon', 1),
--     ('answer', 'Answer', 'Resource that shows the correct answer or solution', 3, 'check_icon', 1),
--     ('feedback', 'Feedback', 'Resource providing feedback on user performance', 4, 'feedback_icon', 1),
--     ('hint', 'Hint', 'Resource providing a hint to help solve the problem', 5, 'lightbulb_icon', 1),
--     ('example', 'Example', 'Resource showing an example to illustrate the concept', 6, 'example_icon', 1)
-- ON CONFLICT (usage_context_code) DO NOTHING;
