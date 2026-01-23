-- =====================================================
-- TABLE: assets.media_categories
-- Description: Lookup table for media file categories
-- Scope: Reference data for high-level media classification
-- =====================================================

CREATE TABLE IF NOT EXISTS assets.media_categories (
    -- 1. IDs Híbridos
    category_id     SMALLINT GENERATED ALWAYS AS IDENTITY,
    category_uuid   UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    category_code   TEXT NOT NULL, -- 'image', 'video', 'audio', 'document'
    category_name   TEXT NOT NULL, -- Display name: "Image", "Video", "Audio", "Document"
    description     TEXT,
    default_icon_name TEXT, -- Default icon for this category
    default_max_size_mb INTEGER, -- Default maximum file size in megabytes
    display_order   INTEGER NOT NULL DEFAULT 0,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    category_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(category_code))) STORED,
    category_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(category_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_media_categories PRIMARY KEY (category_id),
    CONSTRAINT uq_media_categories_uuid UNIQUE (category_uuid),
    CONSTRAINT uq_media_categories_code UNIQUE (category_code),
    
    CONSTRAINT fk_media_categories_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_media_categories_code CHECK (LENGTH(category_code) >= 2),
    CONSTRAINT ck_media_categories_name CHECK (LENGTH(category_name) >= 2),
    CONSTRAINT ck_media_categories_max_size CHECK (default_max_size_mb IS NULL OR default_max_size_mb > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_media_categories_code_normalized ON assets.media_categories(category_code_normalized);
CREATE INDEX IF NOT EXISTS idx_media_categories_name_normalized ON assets.media_categories(category_name_normalized);
CREATE INDEX IF NOT EXISTS idx_media_categories_display_order ON assets.media_categories(display_order);
CREATE INDEX IF NOT EXISTS idx_media_categories_deleted_at ON assets.media_categories(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_media_categories_updated_at
BEFORE UPDATE ON assets.media_categories
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE assets.media_categories ENABLE ROW LEVEL SECURITY;

-- Media categories are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON assets.media_categories
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON assets.media_categories
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE assets.media_categories IS 'Reference table for media file categories (image, video, audio, document)';
COMMENT ON COLUMN assets.media_categories.category_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN assets.media_categories.category_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN assets.media_categories.category_code IS 'Code identifier: image, video, audio, document';
COMMENT ON COLUMN assets.media_categories.category_name IS 'Human-readable display name';
COMMENT ON COLUMN assets.media_categories.default_icon_name IS 'Default icon for this media category';
COMMENT ON COLUMN assets.media_categories.default_max_size_mb IS 'Default maximum file size in megabytes for this category';

-- 9. Seed Data (Media Categories)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO assets.media_categories (category_code, category_name, description, default_icon_name, default_max_size_mb, display_order, created_by)
-- VALUES 
--     ('image', 'Image', 'Image files (photos, illustrations, diagrams)', 'image_icon', 10, 1, 1),
--     ('video', 'Video', 'Video files (lessons, demonstrations, animations)', 'video_icon', 100, 2, 1),
--     ('audio', 'Audio', 'Audio files (pronunciations, music, sound effects)', 'audio_icon', 20, 3, 1),
--     ('document', 'Document', 'Document files (PDFs, worksheets, presentations)', 'document_icon', 25, 4, 1)
-- ON CONFLICT (category_code) DO NOTHING;
