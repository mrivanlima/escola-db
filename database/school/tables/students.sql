-- =====================================================
-- TABLE: school.students
-- Description: Core entity representing child profiles
-- Managed by: Parents/Guardians and School admins
-- =====================================================

CREATE TABLE IF NOT EXISTS school.students (
    -- 1. IDs Híbridos
    student_id      INTEGER GENERATED ALWAYS AS IDENTITY,
    student_uuid    UUID NOT NULL DEFAULT gen_random_uuid(),
    tenant_id       INTEGER NOT NULL,
    user_id         INTEGER, -- Nullable: Optional link for Hybrid Auth Model (future student logins)
    
    -- 2. Business Data
    nickname        TEXT NOT NULL,
    first_name      TEXT NOT NULL,
    middle_name     TEXT,
    last_name       TEXT NOT NULL,
    birth_date      DATE NOT NULL,
    avatar_config   JSONB, -- Visual customization: {"hair": 1, "color": "#FF5733", "accessories": []}
    
    -- 2.1. Normalized columns for accent-insensitive search (Generated Columns)
    nickname_normalized     TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(nickname))) STORED,
    first_name_normalized   TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(first_name))) STORED,
    middle_name_normalized  TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(middle_name, '')))) STORED,
    last_name_normalized    TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(last_name))) STORED,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- 3. Full Audit Trail
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_by      INTEGER NOT NULL, 
    updated_at      TIMESTAMPTZ,
    updated_by      INTEGER,
    deleted_at      TIMESTAMPTZ, -- Soft Delete

    -- 4. Named Constraints (Bottom)
    CONSTRAINT pk_students PRIMARY KEY (student_id),
    CONSTRAINT uq_students_uuid UNIQUE (student_uuid),
    
    CONSTRAINT fk_students_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id),
    
    CONSTRAINT fk_students_user FOREIGN KEY (user_id)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_students_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id),
    
    CONSTRAINT fk_students_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id),

    CONSTRAINT ck_students_nickname CHECK (LENGTH(nickname) >= 2),
    CONSTRAINT ck_students_first_name CHECK (LENGTH(first_name) >= 2),
    CONSTRAINT ck_students_last_name CHECK (LENGTH(last_name) >= 2),
    CONSTRAINT ck_students_birth_date CHECK (birth_date <= CURRENT_DATE),
    CONSTRAINT ck_students_avatar_config CHECK (avatar_config IS NULL OR jsonb_typeof(avatar_config) = 'object')
);

-- 5. Indexes
CREATE INDEX IF NOT EXISTS idx_students_tenant ON school.students(tenant_id);
CREATE INDEX IF NOT EXISTS idx_students_birth_date ON school.students(birth_date);
CREATE INDEX IF NOT EXISTS idx_students_first_name ON school.students(first_name);
CREATE INDEX IF NOT EXISTS idx_students_last_name ON school.students(last_name);
CREATE INDEX IF NOT EXISTS idx_students_nickname_normalized ON school.students(nickname_normalized);
CREATE INDEX IF NOT EXISTS idx_students_first_name_normalized ON school.students(first_name_normalized);
CREATE INDEX IF NOT EXISTS idx_students_last_name_normalized ON school.students(last_name_normalized);
CREATE INDEX IF NOT EXISTS idx_students_deleted_at ON school.students(deleted_at) WHERE deleted_at IS NULL;

-- 6. Evolutionary Changes (Idempotency)
DO $$ 
BEGIN
    -- Remove full_name if it exists (migration from old schema)
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema='school' AND table_name='students' AND column_name='full_name') THEN
        ALTER TABLE school.students DROP COLUMN full_name;
    END IF;
    
    -- Add first_name if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='first_name') THEN
        ALTER TABLE school.students ADD COLUMN first_name TEXT NOT NULL DEFAULT 'Unknown';
        ALTER TABLE school.students ADD CONSTRAINT ck_students_first_name CHECK (LENGTH(first_name) >= 2);
    END IF;
    
    -- Add middle_name if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='middle_name') THEN
        ALTER TABLE school.students ADD COLUMN middle_name TEXT;
    END IF;
    
    -- Add last_name if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='last_name') THEN
        ALTER TABLE school.students ADD COLUMN last_name TEXT NOT NULL DEFAULT 'Unknown';
        ALTER TABLE school.students ADD CONSTRAINT ck_students_last_name CHECK (LENGTH(last_name) >= 2);
    END IF;
    
    -- Add first_name_normalized if it doesn't exist (Generated Column)
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='first_name_normalized') THEN
        ALTER TABLE school.students ADD COLUMN first_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(first_name))) STORED;
    END IF;
    
    -- Add middle_name_normalized if it doesn't exist (Generated Column)
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='middle_name_normalized') THEN
        ALTER TABLE school.students ADD COLUMN middle_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(COALESCE(middle_name, '')))) STORED;
    END IF;
    
    -- Add last_name_normalized if it doesn't exist (Generated Column)
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_schema='school' AND table_name='students' AND column_name='last_name_normalized') THEN
        ALTER TABLE school.students ADD COLUMN last_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(last_name))) STORED;
    END IF;
END $$;

-- 7. Trigger for Updated At
DROP TRIGGER IF EXISTS trg_students_updated_at ON school.students;
CREATE TRIGGER trg_students_updated_at
BEFORE UPDATE ON school.students
FOR EACH ROW EXECUTE PROCEDURE public.handle_updated_at();

-- 8. RLS (Row Level Security)
ALTER TABLE school.students ENABLE ROW LEVEL SECURITY;

DROP POLICY IF EXISTS "Tenant Isolation" ON school.students;
CREATE POLICY "Tenant Isolation" ON school.students
    USING (tenant_id = current_setting('app.current_tenant', TRUE)::INTEGER);

-- 9. Metadata (Documentation)
COMMENT ON TABLE school.students IS 'Core entity: Child profile managed by parents/schools';
COMMENT ON COLUMN school.students.student_id IS 'INTERNAL PK: Int. Never expose to API';
COMMENT ON COLUMN school.students.student_uuid IS 'EXTERNAL ID: UUID exposed to Frontend/API';
COMMENT ON COLUMN school.students.nickname IS 'Display name used in app (gamification friendly)';
COMMENT ON COLUMN school.students.first_name IS 'Student first name (required)';
COMMENT ON COLUMN school.students.middle_name IS 'Student middle name (optional)';
COMMENT ON COLUMN school.students.last_name IS 'Student last name (required)';
COMMENT ON COLUMN school.students.first_name_normalized IS 'GENERATED: Normalized first name (lowercase, no accents) for search';
COMMENT ON COLUMN school.students.middle_name_normalized IS 'GENERATED: Normalized middle name (lowercase, no accents) for search';
COMMENT ON COLUMN school.students.last_name_normalized IS 'GENERATED: Normalized last name (lowercase, no accents) for search';
COMMENT ON COLUMN school.students.avatar_config IS 'JSON: {"hair": int, "color": hex, "accessories": []}. Visual customization. Must be object type';
