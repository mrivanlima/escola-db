# Prompt: Implement CRUD for Teachers and Classes (With Strict Audit)

**Context:**
We have the `Student` module ready. Now we need to implement the academic structure: **Teachers** and **Classes**.
A `Class` (Turma) has a `TeacherId` (Foreign Key).

**Goal:**
Implement full CRUD (Create, Read, Update, Soft Delete) for `Teacher` and `Class` entities following strict architecture rules defined in `ARCHITECTURE.md`.

**The Prompt:**

@docs/ARCHITECTURE.md @src/Escola.Domain/School/Teacher.cs @src/Escola.Domain/School/Class.cs

Act as a Senior .NET Developer.

**Phase 1: Knowledge Loading (CRITICAL)**
Before writing any code, read `@docs/ARCHITECTURE.md`, specifically **Section 4.3 (API & Communication Standards)** and **Section 3.5 (Audit & Soft Delete)**. You MUST understand the `ApiResponse<T>` envelope strict requirement.

**Phase 2: Implementation (The Code)**

**General Constraints for All Files:**
1.  **Response Format:** STRICTLY use `ApiResponse<T>` for ALL Controller returns. Anonymous objects or raw Entities are FORBIDDEN.
2.  **Base Class:** Controllers must inherit from `ControllerBase`.
3.  **Validation:** Use FluentValidation.
    - Teachers: Name required.
    - Classes: Name required, SchoolYear required, TeacherId required.
4.  **Soft Delete:** Implement logic to set `DeletedAt = DateTimeOffset.UtcNow` and `IsActive = false`. Do NOT physically delete.

**Task 2.1: Teachers Module (`Escola.Application/UseCases/Teachers/...`)**
- Create DTOs, Validator, and Handlers (Get, GetById, Create, Update, Delete).
- Create `TeachersController.cs`.

**Task 2.2: Classes Module (`Escola.Application/UseCases/Classes/...`)**
- Create DTOs, Validator, and Handlers.
- **Logic:** When creating a Class, you MUST inject `EscolaDbContext` (or a Repository) to validate if the provided `TeacherId` exists and is active.
- Create `ClassesController.cs`.

**Task 2.3: DI Registration**
- Update `Program.cs` to register all new interfaces and validators.

**Phase 3: Execution**
Generate the code files requested above.

**Phase 4: Mandatory Audit (Self-Correction)**
After generating the code, perform a check against `ARCHITECTURE.md`:
1.  Did I wrap ALL HTTP 200/201 responses in `ApiResponse<T>.SuccessResult(...)`?
2.  Did I wrap ALL HTTP 400/404 responses in `ApiResponse<T>.FailureResult(...)`?
3.  Did I use Soft Delete instead of `.Remove()`?
4.  Did I validate the `TeacherId` foreign key manually in the Handler?

*If any answer is "No", rewrite the code immediately before outputting.*

Generate the complete solution now.