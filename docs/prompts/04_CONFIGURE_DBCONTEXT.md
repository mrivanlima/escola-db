# Prompt: Configure EF Core DbContext & Mappings

**Context:**
The Domain Entities are created. We now need to implement the `EscolaDbContext` in the Infrastructure layer to map these entities to our PostgreSQL database.

**Goal:**
Generate the `DbContext` and individual `IEntityTypeConfiguration` files for strict Table/Column mapping.

**The Prompt:**

@src/Escola.Infrastructure

Act as a Senior .NET Developer.
We need to implement the Database Context and Entity Configurations.

**Task 1: Create Entity Configurations**
Inside `Escola.Infrastructure/Persistence/Configurations`, create a folder for each schema (`Identity`, `School`, `Content`, `Game`, `Assets`).
For EACH Domain Entity we created earlier, generate a configuration class implementing `IEntityTypeConfiguration<T>`.

*Rules for Configuration:*
1.  **Table Mapping:** Explicitly map to the correct schema and plural table name.
    - Example: `builder.ToTable("students", "school");`
2.  **Key Mapping:** Define the Primary Key.
    - `builder.HasKey(x => x.Id);`
3.  **Column Mapping:** Map C# PascalCase to DB snake_case.
    - Example: `builder.Property(x => x.FirstName).HasColumnName("first_name").IsRequired();`
4.  **Relationships:** Configure the Foreign Keys.
    - Example: `builder.HasOne(s => s.Tenant)...`
5.  **Global Filters:**
    - Apply a Query Filter for Soft Delete: `builder.HasQueryFilter(x => x.DeletedAt == null);`

**Task 2: Create the DbContext**
Create `Escola.Infrastructure/Persistence/EscolaDbContext.cs`.
- Inherit from `DbContext`.
- Include `DbSet<T>` for all entities.
- In `OnModelCreating`, use `modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaDbContext).Assembly);` to automatically load the config files above.

**Output:**
Generate the `EscolaDbContext.cs` and the configuration files for the core entities (Student, Tenant, Activity, MediaFile, etc.).