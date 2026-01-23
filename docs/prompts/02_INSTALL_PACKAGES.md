# Prompt: Install EF Core & PostgreSQL Packages

**Context:**
The solution structure is created. Now we need to add the Entity Framework Core dependencies to the `Infrastructure` and `Api` projects.

**Goal:**
Generate a PowerShell script (`src/install_packages.ps1`) to install the required NuGet packages for .NET 8/9.

**The Prompt:**

@src/Escola.sln

Act as a Senior .NET Developer.
We need to install the Entity Framework Core packages for PostgreSQL.

Please generate a PowerShell script named `src/install_packages.ps1` that runs the following `dotnet add package` commands:

1. INFRASTRUCTURE PROJECT (`Escola.Infrastructure`):
   - Needs the core logic and the Postgres provider.
   - Install: `Microsoft.EntityFrameworkCore`
   - Install: `Npgsql.EntityFrameworkCore.PostgreSQL`

2. API PROJECT (`Escola.Api`):
   - Needs the tools to run migration commands.
   - Install: `Microsoft.EntityFrameworkCore.Design`

3. DOMAIN & APPLICATION:
   - Do NOT install any database packages here. They must remain agnostic.

4. CLEANUP:
   - The script should run `dotnet restore` at the end.

Generate the script so I can run it to install everything at once.