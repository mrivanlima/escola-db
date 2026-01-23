# Prompt: Sync Student Entity with New Schema (Hybrid Auth)

**Context:**
We have officially updated the Database Documentation (`database_diagram.md`) to support a **Hybrid Auth Model**.
The `students` table now has a nullable `user_id` column that links to `app_users`.
The C# code is currently out of sync with this documentation.

**Goal:**
Update the Domain Entity and EF Core Configuration to reflect the nullable `UserId` foreign key in the `Student` entity.

**The Prompt:**

@docs/database_diagram.md @src/Escola.Domain/School/Student.cs @src/Escola.Infrastructure/Persistence/EscolaDbContext.cs

Act as a Senior .NET Developer.
Sync the code with the documentation.

**Task 1: Update Domain Entity**
Update `src/Escola.Domain/School/Student.cs`:
1.  Add the property `public int? UserId { get; set; }`.
2.  Place it logically (e.g., after `TenantId`).
3.  Do NOT add the navigation property `public AppUser User { get; set; }` yet (unless `AppUser` is already in the Domain), to keep the Domain clean. Ideally, we keep it as a loose Foreign Key for now, or add the navigation if strictly needed. *Preference: Keep it simple.*

**Task 2: Update EF Core Configuration**
Update `src/Escola.Infrastructure/Persistence/EscolaDbContext.cs` (or the specific Configuration file for Student):
1.  Map the `user_id` column.
2.  Configure the relationship. If `AppUser` entity is available in the context:
    ```csharp
    builder.Entity<Student>()
           .HasOne<AppUser>() // Or .HasOne(s => s.User) if nav prop exists
           .WithMany()        // Or .WithOne() if 1:1, but documentation says 0..1:1 or 1:N.
           .HasForeignKey(s => s.UserId)
           .IsRequired(false); // <--- CRITICAL: It is nullable
    ```
    *If `AppUser` is not yet a mapped entity in the DbContext, just mapping the column `UserId` is sufficient for now.*

**Output:**
Generate the updated `Student.cs` and the updated Configuration code.

