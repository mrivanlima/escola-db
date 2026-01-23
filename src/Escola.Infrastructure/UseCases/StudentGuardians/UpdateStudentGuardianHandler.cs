using Escola.Application.DTOs.School;
using Escola.Application.UseCases.StudentGuardians.UpdateStudentGuardian;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.StudentGuardians;

public class UpdateStudentGuardianHandler : IUpdateStudentGuardianHandler
{
    private readonly EscolaDbContext _context;

    public UpdateStudentGuardianHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateStudentGuardianResponse> HandleAsync(
        UpdateStudentGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.StudentGuardian;

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

        // Update relationship notes if provided
        if (dto.RelationshipNotes != null)
        {
            studentGuardian.RelationshipNotes = dto.RelationshipNotes;
            studentGuardian.RelationshipNotesNormalized = dto.RelationshipNotes.ToLowerInvariant();
        }

        // Update is authorized pickup if provided
        if (dto.IsAuthorizedPickup.HasValue)
        {
            studentGuardian.IsAuthorizedPickup = dto.IsAuthorizedPickup.Value;
        }

        studentGuardian.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateStudentGuardianResponse
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
