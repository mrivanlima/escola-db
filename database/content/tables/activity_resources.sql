-- =====================================================
-- TABLE: content.activity_resources
-- Description: Media resources (images, videos, audio, documents) used in activities
-- Purpose: Links activities to centralized media library (assets.media_files)
-- Previous Name: content.assets (renamed to avoid schema naming conflict)
-- =====================================================

CREATE TABLE IF NOT EXISTS content.activity_resources (
    -- 1. IDs Híbridos
    resource_id     INTEGER GENERATED ALWAYS AS IDENTITY,
    resource_uuid   UUID NOT NULL DEFAULT gen_random_uuid(),
    activity_id     INTEGER NOT NULL,
    
    -- 2. Business Data
    resource_name   TEXT NOT NULL,
    resource_type   TEXT NOT NULL, -- 'image', 'video', 'audio', 'document'
    media_file_id   UUID NOT NULL, -- FK to assets.media_files(file_id)
    display_order   INTEGER NOT NULL DEFAULT 0,
    is_required     BOOLEAN NOT NULL DEFAULT FALSE, -- Required for activity completion
    usage_context   TEXT, -- 'instruction', 'question', 'answer', 'feedback'
    
    -- 2.1. Normalized columns for search
    resource_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(resource_name))) STORED,
    
    -- 3. Resource metadata
    resource_config JSONB, -- {"autoplay": true, "loop": false, "caption": "..."}
    is_published    BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_activity_resources PRIMARY KEY (resource_id),
    
    CONSTRAINT fk_activity_resources_activity FOREIGN KEY (activity_id)
        REFERENCES content.activities (activity_id),
    
    CONSTRAINT fk_activity_resources_media FOREIGN KEY (media_file_id)
        REFERENCES assets.media_files (file_id),
    
    CONSTRAINT fk_activity_resources_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_activity_resources_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_activity_resources_name CHECK (LENGTH(resource_name) >= 2),
    CONSTRAINT ck_activity_resources_type CHECK (resource_type IN ('image', 'video', 'audio', 'document'))
);

-- 6. Indexes
CREATE INDEX IF NOT EXISTS idx_activity_resources_activity ON content.activity_resources(activity_id);
CREATE INDEX IF NOT EXISTS idx_activity_resources_media ON content.activity_resources(media_file_id);
CREATE INDEX IF NOT EXISTS idx_activity_resources_type ON content.activity_resources(resource_type);
CREATE INDEX IF NOT EXISTS idx_activity_resources_name_normalized ON content.activity_resources(resource_name_normalized);
CREATE INDEX IF NOT EXISTS idx_activity_resources_published ON content.activity_resources(is_published) WHERE is_published = TRUE;
CREATE INDEX IF NOT EXISTS idx_activity_resources_deleted_at ON content.activity_resources(deleted_at) WHERE deleted_at IS NULL;

-- 7. Soft Delete Unique Index (UUID must be unique only for non-deleted records)
CREATE UNIQUE INDEX IF NOT EXISTS idx_activity_resources_uuid_active ON content.activity_resources(resource_uuid) WHERE deleted_at IS NULL;

-- 8. Evolutionary Changes (Migration from old content.assets)
DO $$ 
BEGIN
    -- If old table exists, we could migrate data here
    -- This is a placeholder for future migration logic
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema='content' AND table_name='assets') THEN
        RAISE NOTICE 'Old content.assets table exists. Consider data migration before dropping.';
    END IF;
END $$;

-- 9. Trigger for Updated At
DROP TRIGGER IF EXISTS trg_activity_resources_updated_at ON content.activity_resources;
CREATE TRIGGER trg_activity_resources_updated_at
BEFORE UPDATE ON content.activity_resources
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 10. Metadata (Documentation)
COMMENT ON TABLE content.activity_resources IS 'Activity resources: Links activities to media files (images, videos, audio, documents)';
COMMENT ON COLUMN content.activity_resources.resource_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN content.activity_resources.resource_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.activity_resources.media_file_id IS 'FK to assets.media_files: Ensures referential integrity';
COMMENT ON COLUMN content.activity_resources.usage_context IS 'How the resource is used: instruction, question, answer, feedback';
COMMENT ON COLUMN content.activity_resources.resource_config IS 'JSON: {"autoplay": true, "loop": false, "caption": "...", "thumbnail_time": 5}';
COMMENT ON COLUMN content.activity_resources.is_required IS 'Whether this resource must be viewed/interacted with for activity completion';
