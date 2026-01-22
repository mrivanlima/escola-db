-- Test accent normalization
UPDATE school.students 
SET first_name = 'José', middle_name = 'María', last_name = 'Gonçalves' 
WHERE student_id = 1;

UPDATE school.students 
SET first_name = 'João', middle_name = 'Paulo', last_name = 'Silva' 
WHERE student_id = 2;

UPDATE school.students 
SET first_name = 'Ana', middle_name = 'Luíza', last_name = 'Araújo' 
WHERE student_id = 3;

-- Show results
SELECT 
    first_name, 
    first_name_normalized, 
    middle_name, 
    middle_name_normalized, 
    last_name, 
    last_name_normalized 
FROM school.students 
WHERE student_id IN (1, 2, 3);

-- Test search without accents
SELECT 
    student_id,
    first_name || ' ' || COALESCE(middle_name, '') || ' ' || last_name AS full_name
FROM school.students 
WHERE first_name_normalized LIKE '%jose%' 
   OR last_name_normalized LIKE '%goncalves%';
