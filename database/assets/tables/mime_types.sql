-- =====================================================
-- TABLE: assets.mime_types
-- Description: Lookup table for MIME types
-- Scope: Reference data for media file type classification
-- =====================================================

CREATE TABLE IF NOT EXISTS assets.mime_types (
    -- 1. IDs Híbridos
    mime_type_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    mime_type_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    mime_type_code  TEXT NOT NULL, -- 'image/jpeg', 'video/mp4', 'audio/mpeg'
    mime_type_name  TEXT NOT NULL, -- Display name: "JPEG Image", "MP4 Video"
    category_id     SMALLINT NOT NULL, -- FK to assets.media_categories
    file_extension  TEXT NOT NULL, -- '.jpg', '.mp4', '.mp3', '.pdf'
    description     TEXT,
    icon_name       TEXT, -- Icon identifier for frontend
    max_file_size_mb INTEGER, -- Maximum file size in megabytes
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    mime_type_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(mime_type_code))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_mime_types PRIMARY KEY (mime_type_id),
    CONSTRAINT uq_mime_types_uuid UNIQUE (mime_type_uuid),
    CONSTRAINT uq_mime_types_code UNIQUE (mime_type_code),
    
    CONSTRAINT fk_mime_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_mime_types_category FOREIGN KEY (category_id)
        REFERENCES assets.media_categories (category_id),

    CONSTRAINT ck_mime_types_code CHECK (LENGTH(mime_type_code) >= 3),
    CONSTRAINT ck_mime_types_name CHECK (LENGTH(mime_type_name) >= 2),
    CONSTRAINT ck_mime_types_extension CHECK (file_extension ~ '^\.[a-z0-9]+$'),
    CONSTRAINT ck_mime_types_file_size CHECK (max_file_size_mb IS NULL OR max_file_size_mb > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_mime_types_code_normalized ON assets.mime_types(mime_type_code_normalized);
CREATE INDEX IF NOT EXISTS idx_mime_types_category ON assets.mime_types(category_id);
CREATE INDEX IF NOT EXISTS idx_mime_types_extension ON assets.mime_types(file_extension);
CREATE INDEX IF NOT EXISTS idx_mime_types_deleted_at ON assets.mime_types(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_mime_types_updated_at
BEFORE UPDATE ON assets.mime_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE assets.mime_types ENABLE ROW LEVEL SECURITY;

-- MIME types are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON assets.mime_types
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON assets.mime_types
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE assets.mime_types IS 'Reference table for MIME types (image/jpeg, video/mp4, audio/mpeg, etc.)';
COMMENT ON COLUMN assets.mime_types.mime_type_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN assets.mime_types.mime_type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN assets.mime_types.mime_type_code IS 'Standard IANA MIME type: image/jpeg, video/mp4, audio/mpeg';
COMMENT ON COLUMN assets.mime_types.category_id IS 'FK to assets.media_categories lookup table';
COMMENT ON COLUMN assets.mime_types.file_extension IS 'File extension including dot: .jpg, .mp4, .mp3';
COMMENT ON COLUMN assets.mime_types.max_file_size_mb IS 'Maximum file size in megabytes for this MIME type';

-- 9. Seed Data (Common MIME Types for Educational Platform)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO assets.mime_types (mime_type_code, mime_type_name, category_id, file_extension, max_file_size_mb, icon_name, created_by)
-- VALUES 
--     -- Images
--     ('image/jpeg', 'JPEG Image', (SELECT category_id FROM assets.media_categories WHERE category_code = 'image'), '.jpg', 10, 'image_icon', 1),
--     ('image/png', 'PNG Image', (SELECT category_id FROM assets.media_categories WHERE category_code = 'image'), '.png', 10, 'image_icon', 1),
--     ('image/gif', 'GIF Image', (SELECT category_id FROM assets.media_categories WHERE category_code = 'image'), '.gif', 5, 'image_icon', 1),
--     ('image/webp', 'WebP Image', (SELECT category_id FROM assets.media_categories WHERE category_code = 'image'), '.webp', 10, 'image_icon', 1),
--     ('image/svg+xml', 'SVG Vector', (SELECT category_id FROM assets.media_categories WHERE category_code = 'image'), '.svg', 2, 'vector_icon', 1),
--     
--     -- Videos
--     ('video/mp4', 'MP4 Video', (SELECT category_id FROM assets.media_categories WHERE category_code = 'video'), '.mp4', 100, 'video_icon', 1),
--     ('video/webm', 'WebM Video', (SELECT category_id FROM assets.media_categories WHERE category_code = 'video'), '.webm', 100, 'video_icon', 1),
--     ('video/ogg', 'Ogg Video', (SELECT category_id FROM assets.media_categories WHERE category_code = 'video'), '.ogv', 100, 'video_icon', 1),
--     ('video/quicktime', 'QuickTime Video', (SELECT category_id FROM assets.media_categories WHERE category_code = 'video'), '.mov', 100, 'video_icon', 1),
--     
--     -- Audio
--     ('audio/mpeg', 'MP3 Audio', (SELECT category_id FROM assets.media_categories WHERE category_code = 'audio'), '.mp3', 20, 'audio_icon', 1),
--     ('audio/wav', 'WAV Audio', (SELECT category_id FROM assets.media_categories WHERE category_code = 'audio'), '.wav', 20, 'audio_icon', 1),
--     ('audio/ogg', 'Ogg Audio', (SELECT category_id FROM assets.media_categories WHERE category_code = 'audio'), '.ogg', 20, 'audio_icon', 1),
--     ('audio/mp4', 'M4A Audio', (SELECT category_id FROM assets.media_categories WHERE category_code = 'audio'), '.m4a', 20, 'audio_icon', 1),
--     ('audio/webm', 'WebM Audio', (SELECT category_id FROM assets.media_categories WHERE category_code = 'audio'), '.weba', 20, 'audio_icon', 1),
--     
--     -- Documents
--     ('application/pdf', 'PDF Document', (SELECT category_id FROM assets.media_categories WHERE category_code = 'document'), '.pdf', 25, 'pdf_icon', 1),
--     ('application/msword', 'Word Document', (SELECT category_id FROM assets.media_categories WHERE category_code = 'document'), '.doc', 25, 'doc_icon', 1),
--     ('application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'Word Document (DOCX)', (SELECT category_id FROM assets.media_categories WHERE category_code = 'document'), '.docx', 25, 'doc_icon', 1),
--     ('application/vnd.ms-powerpoint', 'PowerPoint', (SELECT category_id FROM assets.media_categories WHERE category_code = 'document'), '.ppt', 50, 'ppt_icon', 1),
--     ('application/vnd.openxmlformats-officedocument.presentationml.presentation', 'PowerPoint (PPTX)', (SELECT category_id FROM assets.media_categories WHERE category_code = 'document'), '.pptx', 50, 'ppt_icon', 1)
-- ON CONFLICT (mime_type_code) DO NOTHING;
