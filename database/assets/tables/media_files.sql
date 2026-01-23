-- =====================================================
-- TABLE: assets.media_files
-- Description: Central registry for all uploaded media (Images, Audio, PDF)
-- Purpose: Stores metadata, not the binary. Files are in Supabase Storage.
-- =====================================================

CREATE TABLE IF NOT EXISTS assets.media_files (
    -- 1. IDs Híbridos
    file_id         INTEGER GENERATED ALWAYS AS IDENTITY,
    file_uuid       UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL, -- Tenant isolation (School A cannot see School B's files)
    
    -- 2. Storage & File Info
    storage_path    TEXT NOT NULL, -- Bucket path (e.g., "tenants/123/math/img_01.png")
    original_name   TEXT NOT NULL, -- Original upload filename
    mime_type_id    SMALLINT NOT NULL, -- FK to assets.mime_types
    size_bytes      BIGINT,        -- For quota control
    
    -- 3. Accessibility & Metadata
    alt_text        TEXT,          -- Accessibility (Screen readers)
    metadata        JSONB DEFAULT '{}', -- Tags, dimensions, audio duration, etc.
    
    -- 3.1. Normalized columns for search
    original_name_normalized    TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(original_name))) STORED,
    alt_text_normalized         TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(alt_text, '')))) STORED,
    
    -- 4. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER,       -- Who uploaded the file
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ,   -- Soft Delete (Storage cleanup can happen later via Job)

    -- 5. Named Constraints (Bottom)
    CONSTRAINT pk_media_files PRIMARY KEY (file_id),
    CONSTRAINT uq_media_files_uuid UNIQUE (file_uuid),
    
    CONSTRAINT fk_media_files_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_media_files_mime_type FOREIGN KEY (mime_type_id)
        REFERENCES assets.mime_types (mime_type_id),
    
    CONSTRAINT fk_media_files_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_media_files_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_media_files_original_name CHECK (LENGTH(original_name) >= 1),
    CONSTRAINT ck_media_files_size CHECK (size_bytes IS NULL OR size_bytes > 0)
);

-- 6. Indexes
CREATE INDEX IF NOT EXISTS idx_media_files_tenant ON assets.media_files(tenant_id);
CREATE INDEX IF NOT EXISTS idx_media_files_mime_type ON assets.media_files(mime_type_id);
CREATE INDEX IF NOT EXISTS idx_media_files_storage_path ON assets.media_files(storage_path);
CREATE INDEX IF NOT EXISTS idx_media_files_original_name_normalized ON assets.media_files(original_name_normalized);
CREATE INDEX IF NOT EXISTS idx_media_files_alt_text_normalized ON assets.media_files(alt_text_normalized);
CREATE INDEX IF NOT EXISTS idx_media_files_deleted_at ON assets.media_files(deleted_at) WHERE deleted_at IS NULL;

-- 7. Trigger for Updated At
DROP TRIGGER IF EXISTS trg_media_files_updated_at ON assets.media_files;
CREATE TRIGGER trg_media_files_updated_at
BEFORE UPDATE ON assets.media_files
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE assets.media_files ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Tenant Isolation" ON assets.media_files
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE assets.media_files IS 'Central registry for all uploaded media (Images, Audio, PDF). Stores metadata, not the binary.';
COMMENT ON COLUMN assets.media_files.file_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN assets.media_files.file_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN assets.media_files.mime_type_id IS 'FK to assets.mime_types lookup table';
COMMENT ON COLUMN assets.media_files.storage_path IS 'Relative path in the Supabase Storage Bucket';
COMMENT ON COLUMN assets.media_files.metadata IS 'JSONB for extra data: {"width": 800, "height": 600, "duration_sec": 15, "tags": ["math", "easy"]}';
COMMENT ON COLUMN assets.media_files.alt_text IS 'Accessibility text for screen readers and SEO';
COMMENT ON COLUMN assets.media_files.deleted_at IS 'Soft Delete: Storage cleanup can happen later via scheduled job';
