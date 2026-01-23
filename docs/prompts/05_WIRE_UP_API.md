# Prompt: Wire Up API & Connection String

**Context:**
The Infrastructure layer is ready. Now we need to configure the `Escola.Api` project to connect to the database on startup.

**Goal:**
Update `appsettings.json` and `Program.cs` to register the `EscolaDbContext`.

**The Prompt:**

@src/Escola.Api/Program.cs @src/Escola.Api/appsettings.json

Act as a Senior .NET Developer.
We need to configure the API startup logic.

**Task 1: Update appsettings.json**
Add a `ConnectionStrings` section to `src/Escola.Api/appsettings.json`.
- Key: "DefaultConnection"
- Value: "Host=db.YOUR_PROJECT_ID.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
- (Note: Just put the placeholder structure for now).

**Task 2: Update Program.cs**
Rewrite `src/Escola.Api/Program.cs` to include:
1.  **Imports:** `using Escola.Infrastructure.Persistence;` and `using Microsoft.EntityFrameworkCore;`
2.  **Registration:** Add the DbContext to the DI container:
    ```csharp
    builder.Services.AddDbContext<EscolaDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    ```
3.  **Cleanup:** Keep the Swagger/OpenAPI setup if it exists, but ensure the DB connection is registered *before* `builder.Build()`.

**Output:**
Modify the two files directly.