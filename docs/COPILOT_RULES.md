# GitHub Copilot Instructions - Escola Project

## 🚨 CRITICAL: Read This First

**Before implementing ANY feature, endpoint, or database change:**
1. ✅ Read `@ARCHITECTURE.md` for full context
2. ✅ Verify compliance with sections below
3. ✅ Never deviate from these standards

---

## 📋 Quick Reference Checklist

### Database (Section 3)
- [ ] Use `snake_case` for all database identifiers
- [ ] Table names MUST be **plural** (students, activities, not student/activity)
- [ ] Implement **Hybrid ID Strategy**:
  - Internal: `[table]_id` (int/bigint) - NEVER expose to API
  - External: `[table]_uuid` (UUID) - ONLY this goes to frontend
- [ ] Add audit fields: `created_at`, `created_by`, `updated_at`, `updated_by`, `deleted_at`
- [ ] Use soft delete (set `deleted_at`, never hard delete)
- [ ] Create `[column]_normalized` for searchable text fields
- [ ] Add SQL COMMENT to all tables and columns

### API Standards (Section 4.3) - STRICT
- [ ] **ALL endpoints MUST use `ApiResponse<T>` wrapper**
- [ ] Never return raw objects or anonymous types
- [ ] Use factory methods:
  - Success: `ApiResponse<T>.SuccessResult(data, message)`
  - Failure: `ApiResponse<T>.FailureResult(message, errors)`
- [ ] Status codes:
  - 200 OK → with ApiResponse
  - 201 Created → with ApiResponse
  - 400 Bad Request → with ApiResponse + errors list
  - 404 Not Found → with ApiResponse
  - 500 Internal Server Error → with ApiResponse
- [ ] Controllers inherit from `ControllerBase`
- [ ] Controllers have NO business logic (delegate to handlers)

### Clean Architecture (Section 4.1)
- [ ] **Domain:** Entities, Enums only. Zero dependencies.
- [ ] **Application:** DTOs, Interfaces, Validators (FluentValidation)
- [ ] **Infrastructure:** Implementations, EF Core, Dapper
- [ ] **Api:** Controllers only (presentation layer)
- [ ] Never return entities from controllers (use DTOs)
- [ ] Interfaces in Application, implementations in Infrastructure

### DTOs & Validation
- [ ] Use FluentValidation in Application layer
- [ ] DTOs must NOT expose internal IDs (`[table]_id`)
- [ ] DTOs expose only UUIDs (`[table]_uuid`)
- [ ] Separate Request/Response DTOs for each operation

---

## 🔧 Code Templates

### Controller Template
```csharp
[ApiController]
[Route("api/[controller]")]
public class XxxController : ControllerBase
{
    private readonly IXxxHandler _handler;
    private readonly IValidator<XxxRequest> _validator;
    private readonly ILogger<XxxController> _logger;

    // GET endpoint
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _handler.Handle(id, ct);
        if (result == null)
            return NotFound(ApiResponse<XxxResponse>.FailureResult("Resource not found"));
        
        return Ok(ApiResponse<XxxResponse>.SuccessResult(result));
    }

    // POST endpoint
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] XxxRequest request, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
            return BadRequest(ApiResponse<XxxResponse>.FailureResult("Validation failed", errors));
        }

        var result = await _handler.Handle(request, ct);
        return Created($"/api/xxx/{result.Uuid}", 
            ApiResponse<XxxResponse>.SuccessResult(result, "Created successfully"));
    }
}
```

### Handler Interface (Application)
```csharp
namespace Escola.Application.UseCases.Xxx.GetXxx;

public interface IGetXxxHandler
{
    Task<XxxResponse?> Handle(Guid uuid, CancellationToken ct = default);
}
```

### Handler Implementation (Infrastructure)
```csharp
namespace Escola.Infrastructure.UseCases.Xxx;

public class GetXxxHandler : IGetXxxHandler
{
    private readonly EscolaDbContext _context;

    public async Task<XxxResponse?> Handle(Guid uuid, CancellationToken ct)
    {
        return await _context.Xxxs
            .AsNoTracking()
            .Where(x => x.XxxUuid == uuid)
            .Select(x => new XxxResponse { /* map fields */ })
            .FirstOrDefaultAsync(ct);
    }
}
```

### FluentValidation Template
```csharp
public class XxxValidator : AbstractValidator<XxxRequest>
{
    public XxxValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Date)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date cannot be in the future");
    }
}
```

---

## 🚫 Common Mistakes to Avoid

❌ **DO NOT:**
- Expose `[table]_id` (internal IDs) in DTOs or API responses
- Return entities directly from controllers
- Use anonymous objects in controller responses
- Return `NoContent()` without ApiResponse wrapper
- Create hard deletes (use soft delete with `deleted_at`)
- Use `public` schema for business tables
- Skip FluentValidation for input DTOs
- Put business logic in controllers

✅ **DO:**
- Always use `ApiResponse<T>` wrapper
- Expose only UUIDs to frontend
- Use DTOs for all API communication
- Implement soft delete pattern
- Validate with FluentValidation
- Delegate to handlers/services
- Follow Clean Architecture layers
- Use proper HTTP status codes with ApiResponse

---

## 📝 Feature Implementation Workflow

1. **Read ARCHITECTURE.md** for context
2. **Create Application layer** (DTOs, Interfaces, Validators)
3. **Create Infrastructure layer** (Implementations)
4. **Create API layer** (Controllers)
5. **Register in DI** (Program.cs)
6. **Build & Test**
7. **Audit against this checklist**

---

## 🎯 Priority Sections in ARCHITECTURE.md

- **Section 3.2:** Naming Conventions
- **Section 3.3:** Hybrid ID Strategy
- **Section 3.5:** Audit & Soft Delete
- **Section 4.1:** Clean Architecture
- **Section 4.3:** API Standards (MOST CRITICAL)

---

## 💡 Usage Examples

```
✅ "@ARCHITECTURE.md - Implement Guardian CRUD"
✅ "Following @COPILOT_RULES.md, create Classes endpoints"
✅ "Audit TenantsController against @ARCHITECTURE.md Section 4.3"
✅ "@ARCHITECTURE.md @COPILOT_RULES.md - Add soft delete to Activities"
```

---

**Last Updated:** January 23, 2026  
**Project:** Escola - Gamified Educational Platform  
**Architecture Version:** 1.0
