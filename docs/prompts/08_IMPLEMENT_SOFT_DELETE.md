# Prompt: Implement Soft Delete (Student)

**Context:**
We have CREATE and READ operations. Now we need to implement the DELETE operation.
**CRITICAL RULE:** According to `ARCHITECTURE.md`, we MUST use **Soft Delete**. We never physically remove rows from the database.

**Goal:**
Implement the `DELETE /api/students/{id}` endpoint that marks a student as deleted.

**The Prompt:**

@src/Escola.Api @src/Escola.Application @src/Escola.Domain

Act as a Senior .NET Developer.
Implement the "Soft Delete Student" feature.

**Task 1: Application Layer**
1.  Create `Escola.Application/UseCases/Students/DeleteStudent/DeleteStudentHandler.cs`.
2.  Logic:
    - Retrieve the student by ID (Guid).
    - If not found -> Return NotFound.
    - **Soft Delete Logic:**
      - Set `student.DeletedAt = DateTimeOffset.UtcNow;`
      - Set `student.IsActive = false;`
      - (Do NOT call `_context.Students.Remove()`).
    - Save changes.

**Task 2: API Layer**
1.  Update `StudentsController.cs`.
2.  Add a `[HttpDelete("{id}")]` endpoint.
3.  Call the Handler.
4.  Return `NoContent()` (204) on success.

**Task 3: Verification Instruction**
- Remind me to call the `GET /api/tenants` (or create a `GET /api/students` list if we haven't yet) to verify the student "disappears" from the list after deletion, proving the Global Query Filter is working.

**Output:**
Generate the code for the Handler and the Controller update.