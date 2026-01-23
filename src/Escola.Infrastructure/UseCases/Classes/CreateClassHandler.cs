using Escola.Application.UseCases.Classes.CreateClass;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

/// <summary>
/// Handler for creating a new class.
/// </summary>
public class CreateClassHandler : ICreateClassHandler
{
    private readonly EscolaDbContext _context;

    public CreateClassHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateClassResponse> Handle(CreateClassRequest request, CancellationToken cancellationToken)
    {
        // Verify tenant exists
        var tenantExists = await _context.Tenants
            .AnyAsync(t => t.TenantId == request.TenantId, cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} does not exist");
        }

        var classEntity = new Domain.School.Class
        {
            ClassUuid = Guid.NewGuid(),
            TenantId = request.TenantId,
            ClassName = request.ClassName,
            GradeLevel = request.GradeLevel,
            SchoolYear = request.SchoolYear,
            MaxStudents = request.MaxStudents,
            ClassConfig = request.ClassConfig,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = 1 // TODO: Replace with actual authenticated user
        };

        _context.Classes.Add(classEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateClassResponse
        {
            ClassUuid = classEntity.ClassUuid,
            ClassName = classEntity.ClassName,
            SchoolYear = classEntity.SchoolYear
        };
    }
}
