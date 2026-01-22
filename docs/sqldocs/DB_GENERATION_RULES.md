# Database Generation Rules for AI Agent

Role: Senior DBA & Architect.
Context: K-12 Educational Platform (Project School).
Files to Reference: @ARCHITECTURE.md, @schema_template.sql

## Goal
Generate or update SQL files in the `/database` folder following "Smart Idempotency".

## File Structure Rules
- Do NOT generate a single huge file.
- Generate separate files per object in: `/database/[schema]/[type]/[name].sql`
- Order of schemas: 1.identity -> 2.content -> 3.school -> 4.game

## SQL Writing Standards (Idempotency)

1. TABLES (Evolutionary approach):
   - Start with `CREATE TABLE IF NOT EXISTS`.
   - Use `DO $$... END$$` blocks to check and add columns/constraints via `ALTER TABLE` if they don't exist.
   - Example pattern:
     ```sql
     CREATE TABLE IF NOT EXISTS schema.table (...);
     DO $$ BEGIN
       IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE column_name='new_col') THEN
         ALTER TABLE schema.table ADD COLUMN new_col text;
       END IF;
     END $$;
     ```

2. VIEWS / FUNCTIONS:
   - Always use `CREATE OR REPLACE`.

3. TRIGGERS:
   - Always `DROP TRIGGER IF EXISTS` before creating.

## Soft Delete & Unique Constraints

**Problem**: Standard UNIQUE constraints prevent re-using values (email, UUID) after soft delete.

**Solution**: Use **Partial Unique Indexes** instead of UNIQUE constraints:

```sql
-- DON'T DO THIS (blocks re-use after soft delete):
CONSTRAINT uq_users_email UNIQUE (email)

-- DO THIS (allows re-use after soft delete):
CREATE UNIQUE INDEX idx_users_email_active ON users(email) WHERE deleted_at IS NULL;
```

**Apply to all tables with**:
- UUID columns (user_uuid, student_uuid, etc.)
- Email addresses
- Any business unique identifier (nickname per tenant, etc.)

**Pattern**:
```sql
-- Instead of constraint in CREATE TABLE
CREATE TABLE users (...);

-- Create partial index after table
CREATE UNIQUE INDEX idx_users_uuid_active ON users(user_uuid) WHERE deleted_at IS NULL;
```

## Text Normalization for Search (Brazilian Portuguese Support)

**Context**: Brazilian names commonly use accents (José, João, María, etc.). Users expect to search without accents.

**Solution**: Use **Generated Columns** with the `unaccent` extension:

1. **Enable Extension** (in `init_schemas.sql`):
   ```sql
   CREATE EXTENSION IF NOT EXISTS "unaccent";
   ```

2. **Create Immutable Wrapper** (in `shared/functions/immutable_unaccent.sql`):
   ```sql
   CREATE OR REPLACE FUNCTION public.immutable_unaccent(text) 
   RETURNS text AS $$
     SELECT unaccent($1);
   $$ LANGUAGE SQL IMMUTABLE PARALLEL SAFE STRICT;
   ```

3. **Add Normalized Columns** (for any TEXT column that requires search):
   ```sql
   -- Original column
   first_name TEXT NOT NULL,
   
   -- Generated normalized column (lowercase + no accents)
   first_name_normalized TEXT GENERATED ALWAYS AS (LOWER(immutable_unaccent(first_name))) STORED,
   ```

3. **Create Indexes** on normalized columns:
   ```sql
   CREATE INDEX IF NOT EXISTS idx_table_column_normalized ON schema.table(column_normalized);
   ```

4. **Query Pattern**:
   ```sql
   -- Search without accents: "jose" matches "José"
   SELECT * FROM students 
   WHERE first_name_normalized LIKE LOWER(immutable_unaccent('%jose%'));
   ```

**Benefits**:
- Automatic maintenance (generated columns update automatically)
- No application logic needed
- Indexed for performance
- Works for all TEXT columns

**Apply this pattern to**: Names, addresses, descriptions, or any TEXT field requiring accent-insensitive search.

## Documentation Synchronization Rule

**CRITICAL**: Whenever you make **ANY DDL change** to the database (CREATE, ALTER, DROP, RENAME), you **MUST** update all three diagram files:

### Required Updates After DDL Changes:

1. **Update Mermaid Diagram**:
   - File: `sqldocs/database_diagram.md`
   - Action: Manually update the ERD table definitions and relationships
   - Includes: Column changes, new tables, renamed tables, relationship changes

2. **Update DOT Graph**:
   - File: `sqldocs/database_schema.dot`
   - Action: Update GraphViz node definitions and edges
   - Includes: Schema changes, table names, key relationships

3. **Regenerate PDF**:
   - Script: `generate_db_diagram.py`
   - Action: Update table definitions in Python script if needed, then run:
     ```powershell
     C:/Development/Escola/.venv/Scripts/python.exe generate_db_diagram.py
     ```
   - Output: `sqldocs/database_schema_diagram.pdf`

### Workflow Checklist:
- [ ] Execute DDL changes on database
- [ ] Update corresponding `.sql` file in `database/` folder
- [ ] Update `sqldocs/database_diagram.md` (Mermaid ERD)
- [ ] Update `sqldocs/database_schema.dot` (GraphViz)
- [ ] Update `generate_db_diagram.py` if table structure changed
- [ ] Run `python generate_db_diagram.py` to regenerate PDF
- [ ] Verify all 3 files are synchronized
- [ ] Commit all changes together (SQL + documentation)

**Why**: Keeping documentation in sync prevents confusion, ensures accurate onboarding, and maintains a single source of truth for the database schema.

## Output
- Always provide the necessary `mkdir` commands to create the folder structure if missing.
- After generating files, remind the user to run the `./build_db.ps1` script.