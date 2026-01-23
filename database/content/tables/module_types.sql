-- =====================================================
-- TABLE: content.module_types
-- Description: Lookup table for module/course types
-- Scope: Reference data for learning module classification
-- =====================================================

CREATE TABLE IF NOT EXISTS content.module_types (
    -- 1. IDs Híbridos
    module_type_id    SMALLINT GENERATED ALWAYS AS IDENTITY,
    module_type_uuid  UUID NOT NULL DEFAULT gen_random_uuid(),
    
    -- 2. Business Data
    module_type_code  TEXT NOT NULL, -- 'math', 'reading', 'science', 'art', 'language', 'social', 'physical'
    module_type_name  TEXT NOT NULL, -- Display name: "Mathematics", "Reading & Literacy"
    description       TEXT,
    icon_name         TEXT, -- Icon identifier for frontend
    color_code        TEXT, -- Hex color for UI theming (#FF5722)
    display_order     INTEGER NOT NULL DEFAULT 0,
    is_active         BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 2.1. Normalized columns for search
    module_type_code_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(module_type_code))) STORED,
    module_type_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(module_type_name))) STORED,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER, -- Nullable for system-generated records
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_module_types PRIMARY KEY (module_type_id),
    CONSTRAINT uq_module_types_uuid UNIQUE (module_type_uuid),
    CONSTRAINT uq_module_types_code UNIQUE (module_type_code),
    
    CONSTRAINT fk_module_types_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_module_types_code CHECK (LENGTH(module_type_code) >= 2),
    CONSTRAINT ck_module_types_name CHECK (LENGTH(module_type_name) >= 2),
    CONSTRAINT ck_module_types_color CHECK (color_code IS NULL OR color_code ~ '^#[0-9A-Fa-f]{6}$')
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_module_types_code_normalized ON content.module_types(module_type_code_normalized);
CREATE INDEX IF NOT EXISTS idx_module_types_name_normalized ON content.module_types(module_type_name_normalized);
CREATE INDEX IF NOT EXISTS idx_module_types_display_order ON content.module_types(display_order);
CREATE INDEX IF NOT EXISTS idx_module_types_deleted_at ON content.module_types(deleted_at) WHERE deleted_at IS NULL;

-- 6. Trigger for Updated At
CREATE TRIGGER trg_module_types_updated_at
BEFORE UPDATE ON content.module_types
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 7. RLS (Row Level Security)
ALTER TABLE content.module_types ENABLE ROW LEVEL SECURITY;

-- Module types are shared across tenants (global reference data)
CREATE POLICY "Public Read" ON content.module_types
    FOR SELECT USING (TRUE);

CREATE POLICY "Admin Only Write" ON content.module_types
    FOR ALL USING (current_setting('app.current_user_role', TRUE) = 'admin');

-- 8. Metadata (Documentation)
COMMENT ON TABLE content.module_types IS 'Reference table for module/course types (math, reading, science, art, etc.)';
COMMENT ON COLUMN content.module_types.module_type_id IS 'INTERNAL PK: SmallInt. Never expose to API';
COMMENT ON COLUMN content.module_types.module_type_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN content.module_types.module_type_code IS 'Code identifier: math, reading, science, art, language, social, physical';
COMMENT ON COLUMN content.module_types.module_type_name IS 'Human-readable display name';
COMMENT ON COLUMN content.module_types.icon_name IS 'Icon identifier for frontend rendering';
COMMENT ON COLUMN content.module_types.color_code IS 'Hex color code for UI theming (e.g., #FF5722)';

-- 9. Seed Data (Common Module Types for Educational Platform)
-- MOVED: Seed data moved to database/seed_data.sql to run after all tables are created
-- INSERT INTO content.module_types (module_type_code, module_type_name, description, icon_name, color_code, display_order, created_by)
-- VALUES 
--     ('math', 'Mathematics', 'Numbers, counting, shapes, patterns, basic operations', 'math_icon', '#2196F3', 1, 1),
--     ('reading', 'Reading & Literacy', 'Letters, phonics, reading comprehension, vocabulary', 'book_icon', '#4CAF50', 2, 1),
--     ('science', 'Science', 'Nature, experiments, animals, plants, weather', 'science_icon', '#9C27B0', 3, 1),
--     ('art', 'Art & Creativity', 'Drawing, coloring, crafts, music, creative expression', 'art_icon', '#FF5722', 4, 1),
--     ('language', 'Language', 'Speaking, listening, communication skills, second language', 'language_icon', '#FF9800', 5, 1),
--     ('social', 'Social & Emotional', 'Feelings, relationships, cooperation, problem-solving', 'social_icon', '#E91E63', 6, 1),
--     ('physical', 'Physical Activity', 'Movement, coordination, motor skills, sports', 'physical_icon', '#00BCD4', 7, 1)
-- ON CONFLICT (module_type_code) DO NOTHING;
