using Escola.Application.DTOs.School;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardians;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.StudentGuardians;

public class GetStudentGuardiansHandler : IGetStudentGuardiansHandler
{
    private readonly EscolaDbContext _context;

    public GetStudentGuardiansHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetStudentGuardiansResponse> HandleAsync(
        GetStudentGuardiansRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<StudentGuardian>()
            .Include(sg => sg.Student)
            .Include(sg => sg.Guardian)
                .ThenInclude(g => g.User)
            .Include(sg => sg.Guardian)
                .ThenInclude(g => g.RelationshipType)
            .Include(sg => sg.Tenant)
            .Where(sg => sg.DeletedAt == null);

        if (request.StudentUuid.HasValue)
        {
            query = query.Where(sg => sg.Student.StudentUuid == request.StudentUuid.Value);
        }

        if (request.GuardianUuid.HasValue)
        {
            query = query.Where(sg => sg.Guardian.GuardianUuid == request.GuardianUuid.Value);
        }

        if (request.TenantUuid.HasValue)
        {
            query = query.Where(sg => sg.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var studentGuardians = await query
            .OrderBy(sg => sg.Student.LastName)
            .ThenBy(sg => sg.Student.FirstName)
            .ToListAsync(cancellationToken);

        return new GetStudentGuardiansResponse
        {
            StudentGuardians = studentGuardians.Select(sg => new StudentGuardianDto
            {
                StudentUuid = sg.Student.StudentUuid,
                StudentName = $"{sg.Student.FirstName} {sg.Student.LastName}",
                GuardianUuid = sg.Guardian.GuardianUuid,
                GuardianName = sg.Guardian.User.FullName,
                RelationshipType = sg.Guardian.RelationshipType.Name,
                TenantUuid = sg.Tenant.TenantUuid,
                TenantName = sg.Tenant.TenantName,
                RelationshipNotes = sg.RelationshipNotes,
                IsAuthorizedPickup = sg.IsAuthorizedPickup,
                CreatedAt = sg.CreatedAt.DateTime,
                UpdatedAt = sg.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
