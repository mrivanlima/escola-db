-- =====================================================
-- COMPLETE DATABASE BUILD - Project Escola
-- Single file execution to avoid multiple connections
-- =====================================================

-- Load configuration file
\i shared/init_schemas.sql
\i shared/functions/immutable_unaccent.sql
\i shared/functions/handle_updated_at.sql

-- Identity Schema - Careful ordering to handle circular dependencies
-- Step 1: Create tenants first (without FK to tenant_types, without FK from app_users)
\i identity/tables/tenants.sql
-- Step 2: Create user_roles (needs tenants)
\i identity/tables/user_roles.sql
-- Step 3: Create app_users (needs tenants, user_roles)
\i identity/tables/app_users.sql
-- Step 4: Create tenant_types (needs tenants, app_users)
\i identity/tables/tenant_types.sql

-- Assets Schema - Lookup Tables First
\i assets/tables/media_categories.sql
\i assets/tables/mime_types.sql
\i assets/tables/media_files.sql

-- School Schema - Lookup Tables First
\i school/tables/grade_levels.sql
\i school/tables/school_years.sql
\i school/tables/specializations.sql
\i school/tables/certifications.sql
\i school/tables/relationship_types.sql
\i school/tables/proficiency_levels.sql
\i school/tables/enrollment_statuses.sql
\i school/tables/subjects.sql

-- School Schema - Main Tables
\i school/tables/students.sql
\i school/tables/guardians.sql
\i school/tables/student_guardians.sql
\i school/tables/classes.sql
\i school/tables/teachers.sql
\i school/tables/teacher_certifications.sql
\i school/tables/teacher_subjects.sql
\i school/tables/class_students.sql

-- Content Schema - Lookup Tables First
\i content/tables/module_types.sql
\i content/tables/activity_types.sql
\i content/tables/resource_types.sql
\i content/tables/usage_contexts.sql

-- Content Schema - Main Tables
\i content/tables/modules.sql
\i content/tables/activities.sql
\i content/tables/activity_resources.sql

-- Game Schema - Lookup Tables First
\i game/tables/progress_statuses.sql
\i game/tables/badge_types.sql
\i game/tables/badge_rarities.sql

-- Game Schema - Main Tables
\i game/tables/student_progress.sql
\i game/tables/badges.sql
\i game/tables/student_badges.sql

-- Post-Creation: Add FK constraints that were deferred due to circular dependencies
ALTER TABLE identity.tenant_types
    ADD CONSTRAINT fk_tenant_types_tenant FOREIGN KEY (tenant_id) 
        REFERENCES identity.tenants (tenant_id);

ALTER TABLE identity.tenants
    ADD CONSTRAINT fk_tenants_type FOREIGN KEY (tenant_type_id)
        REFERENCES identity.tenant_types (type_id);

ALTER TABLE identity.tenants
    ADD CONSTRAINT fk_tenants_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id);

ALTER TABLE identity.tenants
    ADD CONSTRAINT fk_tenants_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id);

ALTER TABLE identity.user_roles
    ADD CONSTRAINT fk_user_roles_created FOREIGN KEY (created_by)
        REFERENCES identity.app_users (user_id);

ALTER TABLE identity.user_roles
    ADD CONSTRAINT fk_user_roles_updated FOREIGN KEY (updated_by)
        REFERENCES identity.app_users (user_id);

-- Post-Creation: Add RLS policies that were deferred due to circular dependencies
-- Policy: Admin Write (only admins can modify user_roles)
CREATE POLICY "Admin Write" ON identity.user_roles
    FOR ALL
    USING (
        EXISTS (
            SELECT 1 FROM identity.app_users u
            JOIN identity.user_roles r ON u.user_role_id = r.role_id
            WHERE u.user_id = current_setting('app.current_user_id', TRUE)::INTEGER
            AND r.role_code IN ('admin', 'super_admin')
        )
    );

-- Policy: Super Admin Write (only super admins can modify tenant_types)
CREATE POLICY "Super Admin Write" ON identity.tenant_types
    FOR ALL
    USING (
        EXISTS (
            SELECT 1 FROM identity.app_users u
            JOIN identity.user_roles r ON u.user_role_id = r.role_id
            WHERE u.user_id = current_setting('app.current_user_id', TRUE)::INTEGER
            AND r.role_code = 'super_admin'
        )
    );

-- Policy: Admin Write (only admins can modify progress_statuses)
CREATE POLICY "Admin Write" ON game.progress_statuses
    FOR ALL
    USING (
        EXISTS (
            SELECT 1 FROM identity.app_users u
            JOIN identity.user_roles r ON u.user_role_id = r.role_id
            WHERE u.user_id = current_setting('app.current_user_id', TRUE)::INTEGER
            AND r.role_code IN ('admin', 'super_admin')
        )
    );
