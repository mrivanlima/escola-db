-- =====================================================
-- TABLE: content.resource_types
-- Description: Lookup table for resource/media types
-- Scope: Reference data for activity resource classification
-- =====================================================

CREATE TABLE IF NOT EXISTS content.resource_types (
    -- 1. IDs Híbridos
    resource_type_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    resource_type_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    resource_type_code  TEXT NOT NULL, -- 'image', 'video', 'audio', 'document'
    resource_type_name  TEXT NOT NULL, -- Display name: "Image", "Video", "Audio", "Document"
    description         TEXT,
    mime_types          JSONB, -- ["image/jpeg", "image/png", "image/gif"]
    max_file_size_mb    INTEGER, -- Maximum file size in megabytes
    icon_name           TEXT, -- Icon identifier for frontend
    is_active           BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    resource_type_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(resource_type_code))) STORED,
    resource_type_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(resource_type_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_resource_types PRIMARY KEY (resource_type_id),
    CONSTRAINT uq_resource_types_uuid UNIQUE (resource_type_uuid),
    CONSTRAINT uq_resource_types_code UNIQUE (resource_type_code),
    
    CONSTRAINT fk_resource_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_resource_types_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_resource_types_code CHECK (LENGTH(resource_type_code) >= 2),
    CONSTRAINT ck_resource_types_name CHECK (LENGTH(resource_type_name) >= 2),
    CONSTRAINT ck_resource_types_file_size CHECK (max_file_size_mb IS NULL OR max_file_size_mb > 0),
    CONSTRAINT ck_resource_types_mime_is_array CHECK (mime_types IS NULL OR jsonb_typeof(mime_types) = 'array')
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_resource_types_code_normalized ON content.resource_types(resource_type_code_normalized);
CREATE INDEX IF NOT EXISTS idx_resource_types_name_normalized ON content.resource_types(resource_type_name_normalized);
CREATE INDEX IF NOT EXISTS idx_resource_types_deleted_at ON content.resource_types(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_resource_types_updated_at
BEFORE UPDATE ON content.resource_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE content.resource_types ENABLE ROW LEVEL SECURITY;

-- Resource types are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON content.resource_types
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON content.resource_types
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE content.resource_types IS 'Reference table for resource/media types (image, video, audio, document)';
COMMENT ON COLUMN content.resource_types.resource_type_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN content.resource_types.resource_type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.resource_types.resource_type_code IS 'Code identifier: image, video, audio, document';
COMMENT ON COLUMN content.resource_types.resource_type_name IS 'Human-readable display name';
COMMENT ON COLUMN content.resource_types.mime_types IS 'JSON array of accepted MIME types for this resource type';
COMMENT ON COLUMN content.resource_types.max_file_size_mb IS 'Maximum file size in megabytes for this resource type';
COMMENT ON COLUMN content.resource_types.icon_name IS 'Icon identifier for frontend rendering';

-- 9. Seed Data (Common Resource Types)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO content.resource_types (resource_type_code, resource_type_name, description, mime_types, max_file_size_mb, icon_name, created_by)
-- VALUES 
--     ('image', 'Image', 'Image files (photos, illustrations, diagrams)', '["image/jpeg", "image/png", "image/gif", "image/webp"]'::jsonb, 10, 'image_icon', 1),
--     ('video', 'Video', 'Video files (lessons, demonstrations, animations)', '["video/mp4", "video/webm", "video/ogg"]'::jsonb, 100, 'video_icon', 1),
--     ('audio', 'Audio', 'Audio files (pronunciations, music, sound effects)', '["audio/mpeg", "audio/wav", "audio/ogg", "audio/mp3"]'::jsonb, 20, 'audio_icon', 1),
--     ('document', 'Document', 'Document files (PDFs, worksheets)', '["application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"]'::jsonb, 25, 'document_icon', 1)
-- ON CONFLICT (resource_type_code) DO NOTHING;
