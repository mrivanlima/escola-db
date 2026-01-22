-- =====================================================
-- TABLE: content.assets
-- Description: Media assets (images, videos, audio) used in activities
-- Purpose: Centralized asset management with CDN support
-- =====================================================

CREATE TABLE IF NOT EXISTS content.assets (
    -- 1. IDs Híbridos
    asset_id        INTEGER GENERATED ALWAYS AS IDENTITY,
    asset_uuid      UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    asset_name      TEXT NOT NULL,
    asset_type      TEXT NOT NULL, -- 'image', 'video', 'audio', 'document'
    file_url        TEXT NOT NULL, -- CDN/Storage URL
    
    -- 2.1. Normalized columns for search
    asset_name_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(asset_name))) STORED,
    file_size_bytes BIGINT,
    mime_type       TEXT,
    duration_seconds INTEGER, -- For video/audio
    dimensions      JSONB, -- {"width": 1920, "height": 1080} for images/videos
    alt_text        TEXT, -- Accessibility
    alt_text_normalized     TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(alt_text, '')))) STORED,
    asset_metadata  JSONB, -- {"tags": ["animal", "cat"], "language": "pt-BR"}
    is_published    BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL,
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_assets PRIMARY KEY (asset_id),
    CONSTRAINT uq_assets_uuid UNIQUE (asset_uuid),
    
    CONSTRAINT fk_assets_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_assets_name CHECK (LENGTH(asset_name) >= 2),
    CONSTRAINT ck_assets_type CHECK (asset_type IN ('image', 'video', 'audio', 'document')),
    CONSTRAINT ck_assets_file_size CHECK (file_size_bytes IS NULL OR file_size_bytes > 0)
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_assets_type ON content.assets(asset_type);
CREATE INDEX IF NOT EXISTS idx_assets_name_normalized ON content.assets(asset_name_normalized);
CREATE INDEX IF NOT EXISTS idx_assets_alt_text_normalized ON content.assets(alt_text_normalized);
CREATE INDEX IF NOT EXISTS idx_assets_published ON content.assets(is_published) WHERE is_published = TRUE;
CREATE INDEX IF NOT EXISTS idx_assets_deleted_at ON content.assets(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_assets_updated_at
BEFORE UPDATE ON content.assets
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. Metadata (Documentation)
COMMENT ON TABLE content.assets IS 'Media assets: images, videos, audio files for activities';
COMMENT ON COLUMN content.assets.asset_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN content.assets.asset_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.assets.file_url IS 'CDN/Cloud Storage URL (Supabase Storage, S3, etc.)';
COMMENT ON COLUMN content.assets.dimensions IS 'JSON: {"width": 1920, "height": 1080} for images/videos';
COMMENT ON COLUMN content.assets.asset_metadata IS 'JSON: {"tags": ["animal"], "language": "pt-BR", "copyright": "..."}';
