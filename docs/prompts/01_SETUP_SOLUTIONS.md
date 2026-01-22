# Prompt: Inicialização da Solução .NET (Clean Architecture)

**Contexto:**
Este prompt deve ser enviado ao Agente de IA (VS Code/Cursor) após a definição do `ARCHITECTURE.md` e a limpeza da pasta raiz.

**Objetivo:**
Gerar o script `init_solution.ps1` que cria a estrutura física do projeto (Solution, Projects, Folders) seguindo os padrões definidos.

---
**O Prompt:**

@ARCHITECTURE.md

Act as a Senior .NET Architect.
We are ready to initialize the .NET 9 (or 8) Solution structure.

Please generate a PowerShell script named `src/init_solution.ps1` (create the file inside src) that performs the following steps:

1. SOLUTION SETUP:
   - Ensure the `/src` directory exists.
   - Create a solution file `Escola.sln` INSIDE the `/src` folder (keeps the root clean).

2. CREATE PROJECTS (All inside `/src`):
   - `Escola.Domain` (Class Library) -> The Core. Zero dependencies.
   - `Escola.Application` (Class Library) -> Business logic. References Domain.
   - `Escola.Infrastructure` (Class Library) -> Database/EF Core. References Application & Domain.
   - `Escola.Api` (Web API) -> The Entry Point. References Application & Infrastructure.

3. ESTABLISH REFERENCES (dotnet add reference):
   - Application adds reference to Domain.
   - Infrastructure adds reference to Application and Domain.
   - Api adds reference to Application and Infrastructure.

4. ADD TO SLN:
   - Add all 4 projects to `Escola.sln`.

5. FOLDER SCAFFOLDING (Crucial - Match the Schemas):
   - In `Escola.Domain`, create folders: /Common, /Identity, /Assets, /School, /Content, /Game.
   - In `Escola.Application`, create folders: /Common, /Interfaces, /DTOs, /UseCases.
   - In `Escola.Infrastructure`, create folders: /Persistence, /Services.
   - In `Escola.Api`, create folders: /Controllers, /Middlewares.

6. CLEANUP:
   - Delete the default `Class1.cs`, `WeatherForecast.cs`, and `Program.cs` (only if it's the default template one, we will rewrite it later).

Generate the script so I can run it from the terminal.