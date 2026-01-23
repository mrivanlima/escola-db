# Prompt: Generate Domain Entities (C#)

**Context:**
The solution structure is ready. We need to create the C# Domain Entities that represent our Database Tables.
Reference: @database_diagram.md and @ARCHITECTURE.md

**Goal:**
Generate C# classes inside `Escola.Domain` matching the logical schema structure.

**The Prompt:**

@database_diagram.md @ARCHITECTURE.md

Act as a Senior .NET Architect.
Generate the C# Entity classes for the `Escola.Domain` project.

**Strict Rules for Entities:**
1. **Placement:**
   - Place `Tenant`, `AppUser` in `Escola.Domain/Identity`
   - Place `MediaFile` in `Escola.Domain/Assets`
   - Place `Student`, `Class`, `Teacher`, `Guardian` in `Escola.Domain/School`
   - Place `Module`, `Activity`, `ActivityResource` in `Escola.Domain/Content`
   - Place `StudentProgress`, `Badge` in `Escola.Domain/Game`

2. **Naming Conventions:**
   - Class names must be **Singular** (e.g., Table `students` -> Class `Student`).
   - Property names must be **PascalCase**.
   - Map DB `snake_case` to C# `PascalCase` implies that we will handle mapping later, but for now, just name the properties correctly (e.g., `first_name` -> `FirstName`).

3. **Types:**
   - `student_id` (int) -> `public int Id { get; set; }`
   - `student_uuid` (uuid) -> `public Guid StudentUuid { get; set; }`
   - `jsonb` columns -> `public string ...Json { get; set; }` (Keep it as string for now, or specific Value Object if obvious).
   - `timestamptz` -> `public DateTimeOffset` (Use DateTimeOffset, NOT DateTime).

4. **Relationships:**
   - Include `virtual` navigation properties for EF Core (e.g., `public virtual ICollection<Student> Students { get; set; }`).

5. **Base Class:**
   - If appropriate, create a `BaseEntity` in `Escola.Domain/Common` containing `Id`, `CreatedAt`, `UpdatedAt`, `DeletedAt`.
   - All entities should inherit from it.

**Output:**
- Generate the code for the classes.
- Do NOT generate the DbContext yet. Just the Entity classes.
- Use `file-scoped namespaces` (e.g., `namespace Escola.Domain.School;`).

Generate the files now.