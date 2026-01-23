# Prompt: Create "Create Student" Feature (POST)

**Context:**
We have verified the GET endpoint. Now we need to implement the **Create (POST)** functionality for Students. This involves receiving JSON, validating it, mapping to the Domain Entity, and saving to PostgreSQL.

**Goal:**
Implement the `POST /api/students` endpoint with FluentValidation.

**The Prompt:**

@src/Escola.Api @src/Escola.Application @src/Escola.Domain

Act as a Senior .NET Developer.
Implement the "Create Student" feature.

**Task 1: Install Dependencies**
Generate a script (or just tell me to run it) to install `FluentValidation.DependencyInjectionExtensions` in the **Escola.Application** project.

**Task 2: Application Layer (The Logic)**
1.  **DTOs:** Create `Escola.Application/UseCases/Students/CreateStudent/CreateStudentRequest.cs`.
    - Fields: `FirstName`, `LastName`, `BirthDate`, `TenantId` (temporary, passed in body).
2.  **Validator:** Create `CreateStudentValidator.cs` (inheriting from `AbstractValidator<CreateStudentRequest>`).
    - Rules: FirstName required, LastName required, BirthDate not in future.
3.  **Handler:** Create `CreateStudentHandler.cs`.
    - Inject `EscolaDbContext`.
    - Logic:
      - Create a new `Student` entity.
      - Generate `StudentUuid = Guid.NewGuid()`.
      - Map fields from Request.
      - Set `IsActive = true`.
      - `_context.Students.Add(student);`
      - `await _context.SaveChangesAsync();`
      - Return the new `student_uuid` (Guid).

**Task 3: API Layer (The Controller)**
1.  Create `Escola.Api/Controllers/StudentsController.cs`.
2.  Add a `[HttpPost]` endpoint.
3.  Inject the Validator and the Handler (or Context if keeping it simple).
4.  Flow:
    - Validate the request. If invalid, return `BadRequest(errors)`.
    - Call Handler.
    - Return `Created($"/api/students/{uuid}", response)`.

**Task 4: DI Registration**
- Ensure the Validator and Handler are registered in `Program.cs` (or a DependencyInjection container).

**Output:**
Generate the code for DTOs, Validator, Handler, Controller, and the registration logic.