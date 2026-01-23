using Escola.Application.UseCases.Teachers.CreateTeacher;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

/// <summary>
/// Handler for creating a new teacher.
/// </summary>
public class CreateTeacherHandler : ICreateTeacherHandler
{
    private readonly EscolaDbContext _context;

    public CreateTeacherHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateTeacherResponse> Handle(CreateTeacherRequest request, CancellationToken cancellationToken)
    {
        // Verify tenant exists
        var tenantExists = await _context.Tenants
            .AnyAsync(t => t.TenantId == request.TenantId, cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} does not exist");
        }

        // Verify user exists
        var userExists = await _context.AppUsers
            .AnyAsync(u => u.UserId == request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException($"User with ID {request.UserId} does not exist");
        }

        var teacher = new Teacher
        {
            TeacherUuid = Guid.NewGuid(),
            TenantId = request.TenantId,
            UserId = request.UserId,
            Specialization = request.Specialization,
            HireDate = request.HireDate,
            TeacherConfig = request.TeacherConfig,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = 1 // TODO: Replace with actual authenticated user
        };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateTeacherResponse
        {
            TeacherUuid = teacher.TeacherUuid,
            Specialization = teacher.Specialization
        };
    }
}
