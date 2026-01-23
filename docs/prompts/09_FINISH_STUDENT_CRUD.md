# Prompt: Finish Student CRUD (Get & Update)

**Context:**
We have Create and Soft Delete. To complete the "Student" resource management, we need the ability to **Read** (List/Single) and **Update** details.

**Goal:**
Implement `GET /api/students`, `GET /api/students/{id}`, and `PUT /api/students/{id}`.

**The Prompt:**

@src/Escola.Api @src/Escola.Application @src/Escola.Domain

Act as a Senior .NET Developer.
Finalize the Student CRUD by implementing Read and Update features.

**Task 1: Read Features (GET)**
1.  **Get All:**
    - Create `Escola.Application/UseCases/Students/GetStudents/GetStudentsHandler.cs`.
    - Return a list of DTOs (Use `StudentResponse` - create if missing).
    - Logic: `_context.Students.AsNoTracking().ToListAsync()`.
2.  **Get By ID:**
    - Create `Escola.Application/UseCases/Students/GetStudent/GetStudentHandler.cs`.
    - Logic: Find by UUID. If missing, throw generic `NotFoundException` or return null (handle in Controller).

**Task 2: Update Feature (PUT)**
1.  **DTO:** Create `Escola.Application/UseCases/Students/UpdateStudent/UpdateStudentRequest.cs`.
    - Fields: `FirstName`, `LastName`, `BirthDate` (Allow updating these).
2.  **Validator:** Create `UpdateStudentValidator.cs` (Ensure names aren't empty).
3.  **Handler:** Create `UpdateStudentHandler.cs`.
    - Logic:
      - Fetch by UUID.
      - Update properties from Request.
      - `await _context.SaveChangesAsync();`
      - Return the updated DTO.

**Task 3: API Layer Updates**
Update `Escola.Api/Controllers/StudentsController.cs`:
1.  Add `[HttpGet]` (List).
2.  Add `[HttpGet("{id}")]` (Get Single).
3.  Add `[HttpPut("{id}")]` (Update).
    - Ensure the ID in the URL matches the ID in the body (or ignore body ID).

**Output:**
Generate the code for Handlers, DTOs, Validators, and the Controller updates.