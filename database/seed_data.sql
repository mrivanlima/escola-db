-- =====================================================
-- SEED DATA - Project Escola
-- Sample data for testing (10 rows per table)
-- Execution order respects FK dependencies
-- =====================================================

-- =====================================================
-- 1. IDENTITY.TENANTS (3 tenants)
-- =====================================================
INSERT INTO identity.tenants (tenant_name, tenant_type, tenant_config, is_active, created_at) VALUES
('Escola Pequenos Gênios', 'school', '{"branding": {"logo": "https://example.com/logo1.png"}, "limits": {"max_students": 500}}', true, NOW()),
('Centro Educacional Futuro Brilhante', 'school', '{"branding": {"logo": "https://example.com/logo2.png"}, "limits": {"max_students": 300}}', true, NOW()),
('Colégio Aprender e Crescer', 'school', '{"branding": {"logo": "https://example.com/logo3.png"}, "limits": {"max_students": 200}}', true, NOW()),
('Maria Silva - Conta Individual', 'individual', '{"branding": null, "limits": {"max_students": 5}}', true, NOW()),
('João Santos - Conta Individual', 'individual', '{"branding": null, "limits": {"max_students": 3}}', true, NOW());

-- =====================================================
-- 2. IDENTITY.APP_USERS (12 users)
-- =====================================================
INSERT INTO identity.app_users (tenant_id, auth_user_id, full_name, email, user_role, is_active, user_config, created_at) VALUES
-- Tenant 1 users
(1, gen_random_uuid(), 'Admin Escola 1', 'admin@escolagenios.com.br', 'admin', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(1, gen_random_uuid(), 'Prof. Ana Costa', 'ana.costa@escolagenios.com.br', 'teacher', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(1, gen_random_uuid(), 'Prof. Carlos Souza', 'carlos.souza@escolagenios.com.br', 'teacher', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(1, gen_random_uuid(), 'Mariana Oliveira', 'mariana@gmail.com', 'parent', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(1, gen_random_uuid(), 'Roberto Silva', 'roberto@gmail.com', 'parent', true, '{"language": "pt-BR", "notifications": false}', NOW()),
-- Tenant 2 users
(2, gen_random_uuid(), 'Admin Escola 2', 'admin@futurobrilhante.com.br', 'admin', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(2, gen_random_uuid(), 'Prof. Beatriz Lima', 'beatriz.lima@futurobrilhante.com.br', 'teacher', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(2, gen_random_uuid(), 'Paula Santos', 'paula@gmail.com', 'parent', true, '{"language": "pt-BR", "notifications": true}', NOW()),
-- Tenant 3 users
(3, gen_random_uuid(), 'Admin Escola 3', 'admin@aprenderecrescer.com.br', 'admin', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(3, gen_random_uuid(), 'Prof. Fernando Dias', 'fernando.dias@aprenderecrescer.com.br', 'teacher', true, '{"language": "pt-BR", "notifications": true}', NOW()),
-- Individual tenants
(4, gen_random_uuid(), 'Maria Silva', 'maria.silva@gmail.com', 'parent', true, '{"language": "pt-BR", "notifications": true}', NOW()),
(5, gen_random_uuid(), 'João Santos', 'joao.santos@gmail.com', 'parent', true, '{"language": "pt-BR", "notifications": true}', NOW());

-- =====================================================
-- 3. SCHOOL.STUDENTS (13 students)
-- =====================================================
INSERT INTO school.students (tenant_id, nickname, first_name, middle_name, last_name, birth_date, avatar_config, is_active, created_by, created_at) VALUES
-- Tenant 1 students
(1, 'Pedrinho', 'Pedro', 'Oliveira', 'Silva', '2020-05-15', '{"hair": 1, "color": "#FF5733", "accessories": ["cap"]}', true, 4, NOW()),
(1, 'Aninha', 'Ana', 'Clara Oliveira', 'Silva', '2021-08-22', '{"hair": 2, "color": "#FFC300", "accessories": ["bow"]}', true, 4, NOW()),
(1, 'Luquinhas', 'Lucas', NULL, 'Mendes', '2020-11-10', '{"hair": 3, "color": "#3498DB", "accessories": []}', true, 5, NOW()),
(1, 'Julinha', 'Julia', NULL, 'Costa', '2019-03-18', '{"hair": 4, "color": "#E74C3C", "accessories": ["glasses"]}', true, 5, NOW()),
(1, 'Dudu', 'Eduardo', NULL, 'Santos', '2021-01-25', '{"hair": 1, "color": "#2ECC71", "accessories": ["hat"]}', true, 4, NOW()),
-- Tenant 2 students
(2, 'Sofi', 'Sofia', NULL, 'Lima', '2020-07-08', '{"hair": 5, "color": "#9B59B6", "accessories": ["crown"]}', true, 8, NOW()),
(2, 'Gui', 'Guilherme', NULL, 'Pereira', '2019-12-30', '{"hair": 2, "color": "#1ABC9C", "accessories": []}', true, 8, NOW()),
(2, 'Laurinha', 'Laura', NULL, 'Martins', '2021-04-12', '{"hair": 3, "color": "#F39C12", "accessories": ["ribbon"]}', true, 8, NOW()),
-- Tenant 3 students
(3, 'Mateus', 'Mateus', NULL, 'Rodrigues', '2020-09-05', '{"hair": 1, "color": "#34495E", "accessories": ["cap"]}', true, 9, NOW()),
(3, 'Isa', 'Isabella', NULL, 'Fernandes', '2019-06-20', '{"hair": 4, "color": "#E67E22", "accessories": []}', true, 9, NOW()),
-- Individual tenants
(4, 'Clarinha', 'Clara', NULL, 'Silva', '2020-02-14', '{"hair": 5, "color": "#8E44AD", "accessories": ["bow"]}', true, 11, NOW()),
(4, 'Pedroca', 'Pedro', 'Silva', 'Jr', '2021-10-03', '{"hair": 2, "color": "#16A085", "accessories": []}', true, 11, NOW()),
(5, 'Theo', 'Theo', NULL, 'Santos', '2019-08-17', '{"hair": 3, "color": "#C0392B", "accessories": ["glasses"]}', true, 12, NOW());

-- =====================================================
-- 4. SCHOOL.GUARDIANS (8 guardians)
-- =====================================================
INSERT INTO school.guardians (tenant_id, user_id, phone_number, relationship, is_primary, created_by, created_at) VALUES
(1, 4, '+55 11 98765-4321', 'mother', true, 1, NOW()),
(1, 5, '+55 11 98765-4322', 'father', true, 1, NOW()),
(2, 8, '+55 21 98765-4323', 'mother', true, 6, NOW()),
(3, 9, '+55 11 98765-4324', 'mother', true, 9, NOW()),
(4, 11, '+55 11 98765-4325', 'mother', true, 11, NOW()),
(5, 12, '+55 11 98765-4326', 'father', true, 12, NOW());

-- =====================================================
-- 5. SCHOOL.STUDENT_GUARDIANS (13 relationships)
-- =====================================================
INSERT INTO school.student_guardians (tenant_id, student_id, guardian_id, relationship_notes, is_authorized_pickup, created_by, created_at) VALUES
-- Tenant 1 relationships
(1, 1, 1, 'Mora com a mãe', true, 1, NOW()),
(1, 2, 1, 'Mora com a mãe', true, 1, NOW()),
(1, 3, 2, 'Mora com o pai', true, 1, NOW()),
(1, 4, 2, 'Visita nos fins de semana', true, 1, NOW()),
(1, 5, 1, 'Guarda compartilhada', true, 1, NOW()),
-- Tenant 2 relationships
(2, 6, 3, 'Mora com a mãe', true, 6, NOW()),
(2, 7, 3, 'Mora com a mãe', true, 6, NOW()),
(2, 8, 3, 'Mora com a mãe', true, 6, NOW()),
-- Tenant 3 relationships
(3, 9, 4, 'Mora com a mãe', true, 9, NOW()),
(3, 10, 4, 'Mora com a mãe', true, 9, NOW()),
-- Individual tenants
(4, 11, 5, 'Educação em casa', true, 11, NOW()),
(4, 12, 5, 'Educação em casa', true, 11, NOW()),
(5, 13, 6, 'Educação em casa', true, 12, NOW());

-- =====================================================
-- 6. SCHOOL.CLASSES (8 classes)
-- =====================================================
INSERT INTO school.classes (tenant_id, class_name, grade_level, school_year, max_students, class_config, is_active, created_by, created_at) VALUES
(1, 'Maternal A', 'maternal', '2024/2025', 15, '{"room": "101", "schedule": "08:00-12:00", "teacher": "Ana Costa"}', true, 1, NOW()),
(1, 'Maternal B', 'maternal', '2024/2025', 15, '{"room": "102", "schedule": "13:00-17:00", "teacher": "Carlos Souza"}', true, 1, NOW()),
(1, 'Jardim I', 'jardim-1', '2024/2025', 20, '{"room": "201", "schedule": "08:00-12:00"}', true, 1, NOW()),
(2, 'Turma do Sol', 'maternal', '2024/2025', 12, '{"room": "A1", "schedule": "07:30-11:30"}', true, 6, NOW()),
(2, 'Turma da Lua', 'jardim-1', '2024/2025', 18, '{"room": "A2", "schedule": "13:00-17:00"}', true, 6, NOW()),
(3, 'Pré-escola A', 'pre-escola', '2024/2025', 25, '{"room": "201", "schedule": "08:00-12:00"}', true, 9, NOW()),
(3, 'Pré-escola B', 'pre-escola', '2024/2025', 25, '{"room": "202", "schedule": "13:00-17:00"}', true, 9, NOW());

-- =====================================================
-- 7. SCHOOL.TEACHERS (5 teachers)
-- =====================================================
INSERT INTO school.teachers (tenant_id, user_id, specialization, hire_date, teacher_config, is_active, created_by, created_at) VALUES
(1, 2, 'Educação Infantil', '2023-01-15', '{"certifications": ["Pedagogia"], "subjects": ["Matemática", "Leitura"]}', true, 1, NOW()),
(1, 3, 'Educação Infantil', '2023-03-01', '{"certifications": ["Pedagogia"], "subjects": ["Artes", "Música"]}', true, 1, NOW()),
(2, 7, 'Educação Infantil', '2023-02-10', '{"certifications": ["Pedagogia", "Alfabetização"], "subjects": ["Leitura", "Escrita"]}', true, 6, NOW()),
(3, 10, 'Educação Infantil', '2022-08-20', '{"certifications": ["Pedagogia"], "subjects": ["Ciências", "Matemática"]}', true, 9, NOW());

-- =====================================================
-- 8. SCHOOL.CLASS_STUDENTS (10 enrollments)
-- =====================================================
INSERT INTO school.class_students (tenant_id, class_id, student_id, enrollment_date, status, created_by, created_at) VALUES
(1, 1, 1, '2024-02-01', 'active', 1, NOW()),
(1, 1, 2, '2024-02-01', 'active', 1, NOW()),
(1, 2, 3, '2024-02-01', 'active', 1, NOW()),
(1, 3, 4, '2024-02-01', 'active', 1, NOW()),
(1, 3, 5, '2024-02-05', 'active', 1, NOW()),
(2, 4, 6, '2024-02-01', 'active', 6, NOW()),
(2, 4, 7, '2024-02-01', 'active', 6, NOW()),
(2, 5, 8, '2024-02-01', 'active', 6, NOW()),
(3, 6, 9, '2024-02-01', 'active', 9, NOW()),
(3, 7, 10, '2024-02-01', 'active', 9, NOW());

-- =====================================================
-- 9. CONTENT.MODULES (8 modules)
-- =====================================================
INSERT INTO content.modules (module_name, description, module_type, difficulty_level, recommended_age_min, recommended_age_max, display_order, thumbnail_url, module_config, is_published, created_by, created_at) VALUES
('Números Mágicos', 'Aprenda a contar de 1 a 10 com diversão!', 'math', 1, 36, 60, 1, 'https://example.com/modules/numbers.jpg', '{"tags": ["números", "contar"], "duration_minutes": 30}', true, 1, NOW()),
('Cores e Formas', 'Descubra o mundo das cores e formas geométricas', 'art', 1, 24, 48, 2, 'https://example.com/modules/colors.jpg', '{"tags": ["cores", "formas"], "duration_minutes": 25}', true, 1, NOW()),
('Alfabeto Divertido', 'Conhecendo as letras de A a Z', 'reading', 2, 48, 72, 3, 'https://example.com/modules/alphabet.jpg', '{"tags": ["alfabeto", "letras"], "duration_minutes": 40}', true, 1, NOW()),
('Animais da Fazenda', 'Conheça os animais e seus sons', 'science', 1, 24, 48, 4, 'https://example.com/modules/farm.jpg', '{"tags": ["animais", "sons"], "duration_minutes": 20}', true, 1, NOW()),
('Soma Simples', 'Primeiras operações de adição', 'math', 3, 60, 84, 5, 'https://example.com/modules/addition.jpg', '{"tags": ["soma", "matemática"], "duration_minutes": 35}', true, 1, NOW()),
('Palavras Mágicas', 'Aprenda palavrinhas novas todos os dias', 'reading', 2, 36, 60, 6, 'https://example.com/modules/words.jpg', '{"tags": ["vocabulário", "palavras"], "duration_minutes": 30}', true, 1, NOW()),
('Música e Ritmo', 'Descubra instrumentos e ritmos', 'art', 2, 36, 72, 7, 'https://example.com/modules/music.jpg', '{"tags": ["música", "ritmo"], "duration_minutes": 25}', true, 1, NOW()),
('O Corpo Humano', 'Partes do corpo e cuidados com a saúde', 'science', 3, 48, 84, 8, 'https://example.com/modules/body.jpg', '{"tags": ["corpo", "saúde"], "duration_minutes": 30}', true, 1, NOW());

-- =====================================================
-- 10. CONTENT.ACTIVITIES (12 activities)
-- =====================================================
INSERT INTO content.activities (module_id, activity_name, description, activity_type, display_order, estimated_duration, points_reward, activity_data, thumbnail_url, is_published, created_by, created_at) VALUES
-- Module 1: Números Mágicos
(1, 'Conte os Patinhos', 'Conte quantos patinhos estão na lagoa', 'game', 1, 5, 10, '{"max_number": 5, "theme": "ducks"}', 'https://example.com/activities/ducks.jpg', true, 1, NOW()),
(1, 'Quiz dos Números', 'Perguntas sobre números de 1 a 10', 'quiz', 2, 8, 15, '{"questions": [{"q": "Quantos dedos você tem?", "a": "10"}]}', 'https://example.com/activities/quiz1.jpg', true, 1, NOW()),
-- Module 2: Cores e Formas
(2, 'Pinte o Arco-íris', 'Descubra as cores do arco-íris', 'interactive', 1, 10, 20, '{"colors": ["red", "orange", "yellow", "green", "blue", "purple"]}', 'https://example.com/activities/rainbow.jpg', true, 1, NOW()),
(2, 'Encontre as Formas', 'Identifique círculos, quadrados e triângulos', 'game', 2, 7, 15, '{"shapes": ["circle", "square", "triangle"]}', 'https://example.com/activities/shapes.jpg', true, 1, NOW()),
-- Module 3: Alfabeto Divertido
(3, 'Letra A de Avião', 'Aprenda a letra A', 'interactive', 1, 5, 10, '{"letter": "A", "examples": ["avião", "abelha", "amor"]}', 'https://example.com/activities/letter-a.jpg', true, 1, NOW()),
(3, 'Caça às Letras', 'Encontre as letras escondidas', 'game', 2, 10, 20, '{"letters": ["A", "B", "C", "D", "E"]}', 'https://example.com/activities/hunt.jpg', true, 1, NOW()),
-- Module 4: Animais da Fazenda
(4, 'Sons da Fazenda', 'Ouça e identifique os sons dos animais', 'interactive', 1, 8, 15, '{"animals": ["cow", "pig", "chicken", "horse"]}', 'https://example.com/activities/farm-sounds.jpg', true, 1, NOW()),
(4, 'Quiz dos Animais', 'Perguntas sobre animais da fazenda', 'quiz', 2, 6, 10, '{"questions": [{"q": "O que a vaca faz?", "a": "Muuu"}]}', 'https://example.com/activities/animal-quiz.jpg', true, 1, NOW()),
-- Module 5: Soma Simples
(5, 'Somando Frutas', 'Some maçãs e laranjas', 'game', 1, 10, 25, '{"max_sum": 10, "theme": "fruits"}', 'https://example.com/activities/fruit-math.jpg', true, 1, NOW()),
-- Module 6: Palavras Mágicas
(6, 'Forma a Palavra', 'Monte palavras com as letras', 'puzzle', 1, 12, 30, '{"words": ["gato", "casa", "bola"]}', 'https://example.com/activities/word-puzzle.jpg', true, 1, NOW()),
-- Module 7: Música e Ritmo
(7, 'Toque o Tambor', 'Acompanhe o ritmo batendo no tambor', 'game', 1, 8, 15, '{"rhythm_patterns": ["slow", "medium", "fast"]}', 'https://example.com/activities/drum.jpg', true, 1, NOW()),
-- Module 8: O Corpo Humano
(8, 'Monte o Boneco', 'Coloque as partes do corpo no lugar certo', 'puzzle', 1, 10, 20, '{"parts": ["head", "arms", "legs", "torso"]}', 'https://example.com/activities/body-puzzle.jpg', true, 1, NOW());

-- =====================================================
-- 11. ASSETS.MEDIA_FILES (10 files)
-- =====================================================
INSERT INTO assets.media_files (tenant_id, storage_path, original_name, mime_type, size_bytes, alt_text, metadata, created_by, created_at) VALUES
(1, 'tenants/1/media/duck-animated.png', 'Patinho Animado', 'image/png', 245760, 'Ilustração de um patinho amarelo', '{"tags": ["animal", "pato"], "language": "pt-BR", "url": "https://cdn.example.com/assets/duck-animated.png", "dimensions": {"width": 512, "height": 512}}', 1, NOW()),
(1, 'tenants/1/media/rainbow.jpg', 'Arco-íris Colorido', 'image/jpeg', 389120, 'Imagem de um arco-íris', '{"tags": ["cores", "natureza"], "language": "pt-BR", "url": "https://cdn.example.com/assets/rainbow.jpg", "dimensions": {"width": 1920, "height": 1080}}', 1, NOW()),
(1, 'tenants/1/media/cow-sound.mp3', 'Som da Vaca', 'audio/mpeg', 81920, 'Som de vaca mugindo', '{"tags": ["animal", "som"], "language": "pt-BR", "url": "https://cdn.example.com/assets/cow-sound.mp3", "duration": 3}', 1, NOW()),
(1, 'tenants/1/media/alphabet-song.mp4', 'Vídeo do Alfabeto', 'video/mp4', 15728640, 'Música do alfabeto', '{"tags": ["alfabeto", "música"], "language": "pt-BR", "url": "https://cdn.example.com/assets/alphabet-song.mp4", "dimensions": {"width": 1280, "height": 720}, "duration": 120}', 1, NOW()),
(1, 'tenants/1/media/shapes.svg', 'Formas Geométricas', 'image/svg+xml', 16384, 'Círculo, quadrado e triângulo', '{"tags": ["formas", "geometria"], "language": "pt-BR", "url": "https://cdn.example.com/assets/shapes.svg", "dimensions": {"width": 800, "height": 600}}', 1, NOW()),
(1, 'tenants/1/media/numbers-poster.png', 'Números 1 a 10', 'image/png', 524288, 'Pôster com números de 1 a 10', '{"tags": ["números", "matemática"], "language": "pt-BR", "url": "https://cdn.example.com/assets/numbers-poster.png", "dimensions": {"width": 2048, "height": 1536}}', 1, NOW()),
(1, 'tenants/1/media/drum-beat.mp3', 'Música do Tambor', 'audio/mpeg', 163840, 'Ritmo de tambor', '{"tags": ["música", "ritmo"], "language": "pt-BR", "url": "https://cdn.example.com/assets/drum-beat.mp3", "duration": 30}', 1, NOW()),
(1, 'tenants/1/media/body-parts.jpg', 'Corpo Humano Infantil', 'image/jpeg', 327680, 'Desenho do corpo humano para crianças', '{"tags": ["corpo", "anatomia"], "language": "pt-BR", "url": "https://cdn.example.com/assets/body-parts.jpg", "dimensions": {"width": 1024, "height": 1024}}', 1, NOW()),
(1, 'tenants/1/media/fruits.png', 'Frutas Sortidas', 'image/png', 409600, 'Maçãs, laranjas e bananas', '{"tags": ["frutas", "comida"], "language": "pt-BR", "url": "https://cdn.example.com/assets/fruits.png", "dimensions": {"width": 1200, "height": 800}}', 1, NOW()),
(1, 'tenants/1/media/piano-notes.mp3', 'Som de Piano', 'audio/mpeg', 122880, 'Notas musicais no piano', '{"tags": ["música", "piano"], "language": "pt-BR", "url": "https://cdn.example.com/assets/piano-notes.mp3", "duration": 15}', 1, NOW());

-- =====================================================
-- 12. CONTENT.ACTIVITY_RESOURCES (10 resources)
-- =====================================================
INSERT INTO content.activity_resources (activity_id, resource_name, resource_type, media_file_id, display_order, is_required, usage_context, is_published, created_by, created_at) VALUES
-- Activity 1: Conte os Patinhos
(1, 'Imagem do Patinho', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Patinho Animado'), 1, true, 'instruction', true, 1, NOW()),
-- Activity 3: Pinte o Arco-íris
(3, 'Imagem do Arco-íris', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Arco-íris Colorido'), 1, true, 'instruction', true, 1, NOW()),
-- Activity 4: Encontre as Formas
(4, 'Formas Geométricas', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Formas Geométricas'), 1, true, 'instruction', true, 1, NOW()),
-- Activity 7: Sons da Fazenda
(7, 'Som da Vaca', 'audio', (SELECT file_id FROM assets.media_files WHERE original_name = 'Som da Vaca'), 1, true, 'question', true, 1, NOW()),
-- Activity 9: Somando Frutas
(9, 'Imagem de Frutas', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Frutas Sortidas'), 1, true, 'instruction', true, 1, NOW()),
-- Activity 11: Toque o Tambor
(11, 'Som do Tambor', 'audio', (SELECT file_id FROM assets.media_files WHERE original_name = 'Música do Tambor'), 1, true, 'instruction', true, 1, NOW()),
(11, 'Som do Piano', 'audio', (SELECT file_id FROM assets.media_files WHERE original_name = 'Som de Piano'), 2, false, 'feedback', true, 1, NOW()),
-- Activity 12: Monte o Boneco
(12, 'Corpo Humano', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Corpo Humano Infantil'), 1, true, 'instruction', true, 1, NOW()),
-- Activity 2: Quiz dos Números (additional)
(2, 'Pôster dos Números', 'image', (SELECT file_id FROM assets.media_files WHERE original_name = 'Números 1 a 10'), 1, false, 'instruction', true, 1, NOW()),
-- Activity 5: Letra A de Avião (additional)
(5, 'Vídeo do Alfabeto', 'video', (SELECT file_id FROM assets.media_files WHERE original_name = 'Vídeo do Alfabeto'), 1, false, 'instruction', true, 1, NOW());

-- =====================================================
-- 12. GAME.STUDENT_PROGRESS (15 progress records)
-- =====================================================
INSERT INTO game.student_progress (tenant_id, student_id, activity_id, status, score, time_spent_seconds, attempts_count, completion_date, progress_data, created_by, created_at) VALUES
(1, 1, 1, 'completed', 100, 180, 1, NOW() - INTERVAL '2 days', '{"correct_answers": 5, "total_questions": 5}', 4, NOW() - INTERVAL '2 days'),
(1, 1, 2, 'completed', 80, 240, 2, NOW() - INTERVAL '1 day', '{"correct_answers": 8, "total_questions": 10}', 4, NOW() - INTERVAL '1 day'),
(1, 1, 3, 'in_progress', NULL, 120, 1, NULL, '{"current_color": 3, "total_colors": 7}', 4, NOW()),
(1, 2, 1, 'completed', 100, 150, 1, NOW() - INTERVAL '3 days', '{"correct_answers": 5, "total_questions": 5}', 4, NOW() - INTERVAL '3 days'),
(1, 2, 3, 'completed', 90, 300, 1, NOW() - INTERVAL '1 day', '{"colors_painted": 7, "perfection": 90}', 4, NOW() - INTERVAL '1 day'),
(1, 3, 1, 'completed', 80, 200, 2, NOW() - INTERVAL '2 days', '{"correct_answers": 4, "total_questions": 5, "mistakes": 1}', 5, NOW() - INTERVAL '2 days'),
(1, 3, 4, 'completed', 100, 180, 1, NOW() - INTERVAL '1 day', '{"shapes_found": 10, "time_bonus": true}', 5, NOW() - INTERVAL '1 day'),
(1, 4, 5, 'completed', 100, 240, 1, NOW() - INTERVAL '5 days', '{"letter_traced": "A", "attempts": 1}', 5, NOW() - INTERVAL '5 days'),
(1, 4, 6, 'in_progress', NULL, 360, 1, NULL, '{"letters_found": 3, "total_letters": 5}', 5, NOW()),
(2, 6, 1, 'completed', 100, 170, 1, NOW() - INTERVAL '1 day', '{"correct_answers": 5, "total_questions": 5}', 8, NOW() - INTERVAL '1 day'),
(2, 6, 7, 'completed', 90, 280, 2, NOW(), '{"animals_identified": 4, "total_animals": 4, "mistakes": 0}', 8, NOW()),
(2, 7, 3, 'completed', 85, 330, 1, NOW() - INTERVAL '2 days', '{"colors_painted": 6, "perfection": 85}', 8, NOW() - INTERVAL '2 days'),
(3, 9, 9, 'completed', 100, 420, 1, NOW() - INTERVAL '1 day', '{"correct_sums": 10, "total_problems": 10}', 9, NOW() - INTERVAL '1 day'),
(4, 11, 1, 'completed', 100, 165, 1, NOW() - INTERVAL '3 days', '{"correct_answers": 5, "total_questions": 5}', 11, NOW() - INTERVAL '3 days'),
(5, 13, 5, 'completed', 100, 210, 1, NOW() - INTERVAL '4 days', '{"letter_traced": "A", "perfect": true}', 12, NOW() - INTERVAL '4 days');

-- =====================================================
-- 13. GAME.BADGES (10 badges)
-- =====================================================
INSERT INTO game.badges (badge_name, description, badge_type, icon_url, rarity, points_value, unlock_criteria, display_order, is_active, created_by, created_at) VALUES
('Primeira Conquista', 'Complete sua primeira atividade!', 'milestone', 'https://cdn.example.com/badges/first.png', 'common', 10, '{"type": "activity_count", "value": 1}', 1, true, 1, NOW()),
('Contador Iniciante', 'Complete 5 atividades de matemática', 'achievement', 'https://cdn.example.com/badges/math-beginner.png', 'common', 25, '{"type": "activity_count", "value": 5, "activity_type": "math"}', 2, true, 1, NOW()),
('Mestre das Cores', 'Complete todas as atividades de cores', 'achievement', 'https://cdn.example.com/badges/color-master.png', 'rare', 50, '{"type": "module_complete", "module_id": 2}', 3, true, 1, NOW()),
('Leitor Estrela', 'Complete 10 atividades de leitura', 'achievement', 'https://cdn.example.com/badges/reader-star.png', 'rare', 50, '{"type": "activity_count", "value": 10, "activity_type": "reading"}', 4, true, 1, NOW()),
('Perfeição Total', 'Obtenha 100% em 5 atividades', 'achievement', 'https://cdn.example.com/badges/perfect.png', 'epic', 100, '{"type": "perfect_score_count", "value": 5}', 5, true, 1, NOW()),
('Explorador Curioso', 'Explore 3 módulos diferentes', 'milestone', 'https://cdn.example.com/badges/explorer.png', 'common', 30, '{"type": "unique_modules", "value": 3}', 6, true, 1, NOW()),
('Maratonista', 'Complete 20 atividades em um mês', 'achievement', 'https://cdn.example.com/badges/marathon.png', 'epic', 150, '{"type": "monthly_activity_count", "value": 20}', 7, true, 1, NOW()),
('Artista Nato', 'Complete todas as atividades de arte', 'achievement', 'https://cdn.example.com/badges/artist.png', 'rare', 75, '{"type": "category_complete", "category": "art"}', 8, true, 1, NOW()),
('Cientista Junior', 'Complete 5 atividades de ciências', 'achievement', 'https://cdn.example.com/badges/scientist.png', 'rare', 60, '{"type": "activity_count", "value": 5, "activity_type": "science"}', 9, true, 1, NOW()),
('Lenda da Escola', 'Complete todos os módulos disponíveis', 'special', 'https://cdn.example.com/badges/legend.png', 'legendary', 500, '{"type": "all_modules_complete"}', 10, true, 1, NOW());

-- =====================================================
-- 14. GAME.STUDENT_BADGES (12 badges earned)
-- =====================================================
INSERT INTO game.student_badges (tenant_id, student_id, badge_id, earned_at, progress_id, created_by, created_at) VALUES
(1, 1, 1, NOW() - INTERVAL '2 days', 1, 4, NOW() - INTERVAL '2 days'),
(1, 1, 6, NOW() - INTERVAL '1 day', 3, 4, NOW() - INTERVAL '1 day'),
(1, 2, 1, NOW() - INTERVAL '3 days', 4, 4, NOW() - INTERVAL '3 days'),
(1, 2, 3, NOW() - INTERVAL '1 day', 5, 4, NOW() - INTERVAL '1 day'),
(1, 3, 1, NOW() - INTERVAL '2 days', 6, 5, NOW() - INTERVAL '2 days'),
(1, 4, 1, NOW() - INTERVAL '5 days', 8, 5, NOW() - INTERVAL '5 days'),
(1, 4, 4, NOW() - INTERVAL '5 days', 8, 5, NOW() - INTERVAL '5 days'),
(2, 6, 1, NOW() - INTERVAL '1 day', 10, 8, NOW() - INTERVAL '1 day'),
(2, 6, 9, NOW(), 11, 8, NOW()),
(2, 7, 1, NOW() - INTERVAL '2 days', 12, 8, NOW() - INTERVAL '2 days'),
(3, 9, 1, NOW() - INTERVAL '1 day', 13, 9, NOW() - INTERVAL '1 day'),
(3, 9, 2, NOW() - INTERVAL '1 day', 13, 9, NOW() - INTERVAL '1 day'),
(4, 11, 1, NOW() - INTERVAL '3 days', 14, 11, NOW() - INTERVAL '3 days'),
(5, 13, 1, NOW() - INTERVAL '4 days', 15, 12, NOW() - INTERVAL '4 days');

-- =====================================================
-- SEED DATA COMPLETED
-- =====================================================
-- Summary:
-- - 5 Tenants (3 schools + 2 individuals)
-- - 12 App Users (admins, teachers, parents)
-- - 13 Students
-- - 6 Guardians
-- - 13 Student-Guardian relationships
-- - 7 Classes
-- - 4 Teachers
-- - 10 Class enrollments
-- - 8 Modules
-- - 12 Activities
-- - 10 Assets
-- - 15 Progress records
-- - 10 Badges
-- - 14 Badges earned
-- =====================================================
