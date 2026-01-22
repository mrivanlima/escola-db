-- =====================================================
-- COMPLETE DATABASE BUILD - Project Escola
-- Single file execution to avoid multiple connections
-- =====================================================

-- Load configuration file
\i database/shared/init_schemas.sql
\i database/shared/functions/handle_updated_at.sql

-- Identity Schema
\i database/identity/tables/tenants.sql
\i database/identity/tables/app_users.sql

-- Assets Schema
\i database/assets/tables/media_files.sql

-- School Schema
\i database/school/tables/students.sql
\i database/school/tables/guardians.sql
\i database/school/tables/student_guardians.sql
\i database/school/tables/classes.sql
\i database/school/tables/teachers.sql
\i database/school/tables/class_students.sql

-- Content Schema
\i database/content/tables/modules.sql
\i database/content/tables/activities.sql
\i database/content/tables/activity_resources.sql

-- Game Schema
\i database/game/tables/student_progress.sql
\i database/game/tables/badges.sql
\i database/game/tables/student_badges.sql
