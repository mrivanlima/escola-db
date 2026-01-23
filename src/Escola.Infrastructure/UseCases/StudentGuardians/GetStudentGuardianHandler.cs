using Escola.Application.DTOs.School;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardian;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.StudentGuardians;

public class GetStudentGuardianHandler : IGetStudentGuardianHandler
{
    private readonly EscolaDbContext _context;

    public GetStudentGuardianHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetStudentGuardianResponse> HandleAsync(
        GetStudentGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        var studentGuardian = await _context.Set<StudentGuardian>()
            .Include(sg => sg.Student)
            .Include(sg => sg.Guardian)
                .ThenInclude(g => g.User)
            .Include(sg => sg.Guardian)
                .ThenInclude(g => g.RelationshipType)
            .Include(sg => sg.Tenant)
            .Where(sg => sg.DeletedAt == null)
            .FirstOrDefaultAsync(sg => 
                sg.Student.StudentUuid == request.StudentUuid && 
                sg.Guardian.GuardianUuid == request.GuardianUuid,
                cancellationToken);

        if (studentGuardian == null)
        {
            throw new InvalidOperationException("Student-guardian relationship not found.");
        }

        return new GetStudentGuardianResponse
        {
            StudentGuardian = new StudentGuardianDto
            {
                StudentUuid = studentGuardian.Student.StudentUuid,
                StudentName = $"{studentGuardian.Student.FirstName} {studentGuardian.Student.LastName}",
                GuardianUuid = studentGuardian.Guardian.GuardianUuid,
                GuardianName = studentGuardian.Guardian.User.FullName,
                RelationshipType = studentGuardian.Guardian.RelationshipType.Name,
                TenantUuid = studentGuardian.Tenant.TenantUuid,
                TenantName = studentGuardian.Tenant.TenantName,
                RelationshipNotes = studentGuardian.RelationshipNotes,
                IsAuthorizedPickup = studentGuardian.IsAuthorizedPickup,
                CreatedAt = studentGuardian.CreatedAt.DateTime,
                UpdatedAt = studentGuardian.UpdatedAt?.DateTime
            }
        };
    }
}
