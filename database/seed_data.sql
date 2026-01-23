-- =====================================================
-- SEED DATA - Project Escola
-- Sample data for testing
-- Execution order respects FK dependencies
-- Updated: 2026-01-23 to match current schema
-- =====================================================

-- =====================================================
-- PHASE 1: LOOKUP TABLES (Reference Data)
-- =====================================================

-- 1.1 Identity: Tenant Types
INSERT INTO identity.tenant_types (tenant_id, type_code, type_name, description, max_students, max_teachers, max_storage_gb, has_analytics, has_api_access, has_white_label, pricing_tier, icon_name, color_code, display_order, created_by)
VALUES
    (1, 'individual', 'Individual', 'Personal account for individual educators or parents', 5, 1, 1, FALSE, FALSE, FALSE, 'free', 'person', '#2196F3', 1, NULL),
    (1, 'school', 'School', 'Educational institution with multiple teachers and students', 500, 50, 50, TRUE, FALSE, FALSE, 'premium', 'school', '#4CAF50', 2, NULL),
    (1, 'enterprise', 'Enterprise', 'Large organization with custom requirements and unlimited resources', NULL, NULL, NULL, TRUE, TRUE, TRUE, 'enterprise', 'business', '#FF9800', 3, NULL)
ON CONFLICT (tenant_id, type_code) DO NOTHING;

-- 1.2 Identity: User Roles
INSERT INTO identity.user_roles (tenant_id, role_code, role_name, description, permission_level, can_manage_students, can_manage_content, can_manage_users, can_manage_tenant, icon_name, color_code, display_order, created_by)
VALUES
    (1, 'parent', 'Parent', 'Guardian with access to their children''s progress', 1, FALSE, FALSE, FALSE, FALSE, 'people', '#2196F3', 1, NULL),
    (1, 'teacher', 'Teacher', 'Educator who manages content and student progress', 10, TRUE, TRUE, FALSE, FALSE, 'school', '#4CAF50', 2, NULL),
    (1, 'admin', 'Administrator', 'School administrator with full tenant management', 50, TRUE, TRUE, TRUE, TRUE, 'admin_panel_settings', '#FF9800', 3, NULL),
    (1, 'super_admin', 'Super Administrator', 'Platform administrator with cross-tenant access', 100, TRUE, TRUE, TRUE, TRUE, 'shield', '#F44336', 4, NULL)
ON CONFLICT (tenant_id, role_code) DO NOTHING;

-- =====================================================
-- PHASE 2: MASTER DATA (Tenants and Users)
-- =====================================================

-- 2.1 Identity: Tenants (must be inserted before app_users)
-- Note: created_by is NULL for initial system-created tenants
INSERT INTO identity.tenants (tenant_name, tenant_type_id, tenant_config, is_active, created_by)
VALUES
    ('Escola Pequenos Gênios', 2, '{"branding": {"logo": "https://example.com/logo1.png"}, "limits": {"max_students": 500}}', TRUE, NULL),
    ('Centro Educacional Futuro Brilhante', 2, '{"branding": {"logo": "https://example.com/logo2.png"}, "limits": {"max_students": 300}}', TRUE, NULL),
    ('Colégio Aprender e Crescer', 2, '{"branding": {"logo": "https://example.com/logo3.png"}, "limits": {"max_students": 200}}', TRUE, NULL),
    ('Maria Silva - Conta Individual', 1, '{"branding": null, "limits": {"max_students": 5}}', TRUE, NULL),
    ('João Santos - Conta Individual', 1, '{"branding": null, "limits": {"max_students": 3}}', TRUE, NULL);

-- 2.2 Identity: App Users
-- Note: created_by references user_id, so first users have NULL created_by
INSERT INTO identity.app_users (tenant_id, auth_user_id, full_name, email, user_role_id, is_active, user_config, created_by)
VALUES
    -- Tenant 1 users (tenant_id=1, Escola Pequenos Gênios)
    (1, gen_random_uuid(), 'Admin Escola 1', 'admin@escolagenios.com.br', 3, TRUE, '{"language": "pt-BR", "notifications": true}', NULL),
    (1, gen_random_uuid(), 'Prof. Ana Costa', 'ana.costa@escolagenios.com.br', 2, TRUE, '{"language": "pt-BR", "notifications": true}', 1),
    (1, gen_random_uuid(), 'Prof. Carlos Souza', 'carlos.souza@escolagenios.com.br', 2, TRUE, '{"language": "pt-BR", "notifications": true}', 1),
    (1, gen_random_uuid(), 'Mariana Oliveira', 'mariana@gmail.com', 1, TRUE, '{"language": "pt-BR", "notifications": true}', 1),
    (1, gen_random_uuid(), 'Roberto Silva', 'roberto@gmail.com', 1, TRUE, '{"language": "pt-BR", "notifications": false}', 1),
    
    -- Tenant 2 users (tenant_id=2, Centro Educacional Futuro Brilhante)
    (2, gen_random_uuid(), 'Admin Escola 2', 'admin@futurobrilhante.com.br', 3, TRUE, '{"language": "pt-BR", "notifications": true}', NULL),
    (2, gen_random_uuid(), 'Prof. Beatriz Lima', 'beatriz.lima@futurobrilhante.com.br', 2, TRUE, '{"language": "pt-BR", "notifications": true}', 6),
    (2, gen_random_uuid(), 'Paula Santos', 'paula@gmail.com', 1, TRUE, '{"language": "pt-BR", "notifications": true}', 6),
    
    -- Tenant 3 users (tenant_id=3, Colégio Aprender e Crescer)
    (3, gen_random_uuid(), 'Admin Escola 3', 'admin@aprenderecrescer.com.br', 3, TRUE, '{"language": "pt-BR", "notifications": true}', NULL),
    (3, gen_random_uuid(), 'Prof. Fernando Dias', 'fernando.dias@aprenderecrescer.com.br', 2, TRUE, '{"language": "pt-BR", "notifications": true}', 9),
    
    -- Individual tenants
    (4, gen_random_uuid(), 'Maria Silva', 'maria.silva@gmail.com', 1, TRUE, '{"language": "pt-BR", "notifications": true}', NULL),
    (5, gen_random_uuid(), 'João Santos', 'joao.santos@gmail.com', 1, TRUE, '{"language": "pt-BR", "notifications": true}', NULL);

-- =====================================================
-- PHASE 3: SCHOOL DATA (Students, Guardians, Classes, Teachers)
-- =====================================================

-- 3.1 School: Students (with nullable user_id for Hybrid Auth Model)
-- Note: user_id is NULL for now, will be populated when students get login accounts
INSERT INTO school.students (tenant_id, user_id, nickname, first_name, middle_name, last_name, birth_date, avatar_config, is_active, created_by)
VALUES
    -- Tenant 1 students
    (1, NULL, 'Pedrinho', 'Pedro', 'Oliveira', 'Silva', '2020-05-15', '{"hair": 1, "color": "#FF5733", "accessories": ["cap"]}', TRUE, 4),
    (1, NULL, 'Aninha', 'Ana', 'Clara', 'Oliveira Silva', '2021-08-22', '{"hair": 2, "color": "#FFC300", "accessories": ["bow"]}', TRUE, 4),
    (1, NULL, 'Luquinhas', 'Lucas', NULL, 'Mendes', '2020-11-10', '{"hair": 3, "color": "#3498DB", "accessories": []}', TRUE, 5),
    (1, NULL, 'Julinha', 'Julia', NULL, 'Costa', '2019-03-18', '{"hair": 4, "color": "#E74C3C", "accessories": ["glasses"]}', TRUE, 5),
    (1, NULL, 'Dudu', 'Eduardo', NULL, 'Santos', '2021-01-25', '{"hair": 1, "color": "#2ECC71", "accessories": ["hat"]}', TRUE, 4),
    
    -- Tenant 2 students
    (2, NULL, 'Sofi', 'Sofia', NULL, 'Lima', '2020-07-08', '{"hair": 5, "color": "#9B59B6", "accessories": ["crown"]}', TRUE, 8),
    (2, NULL, 'Gui', 'Guilherme', NULL, 'Pereira', '2019-12-30', '{"hair": 2, "color": "#1ABC9C", "accessories": []}', TRUE, 8),
    (2, NULL, 'Laurinha', 'Laura', NULL, 'Martins', '2021-04-12', '{"hair": 3, "color": "#F39C12", "accessories": ["ribbon"]}', TRUE, 8),
    
    -- Tenant 3 students
    (3, NULL, 'Mateus', 'Mateus', NULL, 'Rodrigues', '2020-09-05', '{"hair": 1, "color": "#34495E", "accessories": ["cap"]}', TRUE, 10),
    (3, NULL, 'Isa', 'Isabella', NULL, 'Fernandes', '2019-06-20', '{"hair": 4, "color": "#E67E22", "accessories": []}', TRUE, 10),
    
    -- Individual tenants
    (4, NULL, 'Clarinha', 'Clara', NULL, 'Silva', '2020-02-14', '{"hair": 5, "color": "#8E44AD", "accessories": ["bow"]}', TRUE, 11),
    (4, NULL, 'Pedroca', 'Pedro', 'Silva', 'Junior', '2021-10-03', '{"hair": 2, "color": "#16A085", "accessories": []}', TRUE, 11),
    (5, NULL, 'Theo', 'Theo', NULL, 'Santos', '2019-08-17', '{"hair": 3, "color": "#C0392B", "accessories": ["glasses"]}', TRUE, 12);

-- 3.2 School: Guardians (linked to app_users)
INSERT INTO school.guardians (tenant_id, user_id, phone_number, is_primary, created_by)
VALUES
    (1, 4, '+55 11 98765-4321', TRUE, 1),  -- Mariana Oliveira
    (1, 5, '+55 11 98765-4322', TRUE, 1),  -- Roberto Silva
    (2, 8, '+55 21 98765-4323', TRUE, 6),  -- Paula Santos
    (3, 10, '+55 11 98765-4324', TRUE, 9), -- Prof. Fernando Dias (also acts as guardian)
    (4, 11, '+55 11 98765-4325', TRUE, 11), -- Maria Silva
    (5, 12, '+55 11 98765-4326', TRUE, 12); -- João Santos

-- Note: The 'relationship' column doesn't exist in guardians table, it should be in student_guardians

-- =====================================================
-- PHASE 4: RELATIONSHIPS
-- =====================================================

-- Note: The student_guardians, classes, teachers, and class_students tables
-- need to be reviewed against the actual schema. For now, commenting out
-- sections that may not match the current schema.

-- TODO: Review and add remaining seed data for:
-- - school.student_guardians (needs relationship_type_id FK based on actual schema)
-- - school.classes (needs grade_level_id, school_year_id, etc.)
-- - school.teachers (needs to verify schema)
-- - school.class_students (needs enrollment_status_id based on actual schema)
-- - content.modules, activities, etc.
-- - game.badges, progress, etc.
-- - assets.media_files, etc.

-- =====================================================
-- SEED DATA SUMMARY (Current Version)
-- =====================================================
-- ✅ Phase 1 - Lookup Tables:
--    - identity.tenant_types: 3 records (individual, school, enterprise)
--    - identity.user_roles: 4 records (parent, teacher, admin, super_admin)
--
-- ✅ Phase 2 - Master Data:
--    - identity.tenants: 5 records (3 schools, 2 individuals)
--    - identity.app_users: 12 records (admins, teachers, parents)
--
-- ✅ Phase 3 - School Data:
--    - school.students: 13 records (with nullable user_id for Hybrid Auth)
--    - school.guardians: 6 records (linked to app_users)
--
-- ⏳ Phase 4 - Relationships (TODO):
--    Needs schema verification before adding data
--
-- Note: All seed data uses proper FK references (IDs, not codes)
-- Note: Students have user_id=NULL (ready for future Hybrid Auth implementation)
-- =====================================================
