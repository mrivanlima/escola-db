# Prompt: Create First Feature (Get Tenants)

**Context:**
The API is running and connected to the Database. We need to create our first "Vertical Slice" to verify the connection works and we can read data.

**Goal:**
Implement a Use Case to "Get All Tenants" following Clean Architecture.

**The Prompt:**

@src/Escola.Api @src/Escola.Application @src/Escola.Domain

Act as a Senior .NET Developer.
Create the "Get All Tenants" feature to test the database connection.

**Task 1: Application Layer**
1. Create `Escola.Application/UseCases/Tenants/GetTenants/GetTenantsRequest.cs` (DTO).
2. Create `Escola.Application/UseCases/Tenants/GetTenants/GetTenantsResponse.cs` (DTO).
3. Create `Escola.Application/UseCases/Tenants/GetTenants/GetTenantsHandler.cs`.
   - Implement the logic to fetch ALL tenants from `EscolaDbContext`.
   - Map Entity -> Response DTO manually (keep it simple for now).

**Task 2: API Layer**
1. Create `Escola.Api/Controllers/TenantsController.cs`.
2. Inherit from `ControllerBase`.
3. Create an HTTP GET endpoint: `[HttpGet]` that calls the Handler (or calls the Context directly if we haven't set up MediatR yet - PLEASE CHECK: If MediatR is not installed, verify if we should install it or just inject the Context into the Controller for this first test. *Recommendation: For this Smoke Test, inject the DbContext directly into the Controller to keep it simple and isolate DB issues from MediatR issues.*).

**Task 3: Run Instructions**
Tell me the URL to call to see the data.

**Output:**
Generate the code for the Controller and the DTOs.