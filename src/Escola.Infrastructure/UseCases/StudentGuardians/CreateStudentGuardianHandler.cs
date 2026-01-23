using Escola.Application.DTOs.School;
using Escola.Application.UseCases.StudentGuardians.CreateStudentGuardian;
using Escola.Domain.Identity;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.StudentGuardians;

public class CreateStudentGuardianHandler : ICreateStudentGuardianHandler
{
    private readonly EscolaDbContext _context;

    public CreateStudentGuardianHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateStudentGuardianResponse> HandleAsync(
        CreateStudentGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.StudentGuardian;

        // Validate tenant
        var tenant = await _context.Set<Tenant>()
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.DeletedAt == null, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException("Tenant not found.");
        }

        // Validate student
        var student = await _context.Set<Student>()
            .FirstOrDefaultAsync(s => s.StudentUuid == dto.StudentUuid && s.DeletedAt == null, cancellationToken);

        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        // Validate guardian
        var guardian = await _context.Set<Guardian>()
            .Include(g => g.RelationshipType)
            .FirstOrDefaultAsync(g => g.GuardianUuid == dto.GuardianUuid && g.DeletedAt == null, cancellationToken);

        if (guardian == null)
        {
            throw new InvalidOperationException("Guardian not found.");
        }

        // Check if relationship already exists
        var existing = await _context.Set<StudentGuardian>()
            .FirstOrDefaultAsync(sg => sg.StudentId == student.StudentId && sg.GuardianId == guardian.GuardianId && sg.DeletedAt == null, cancellationToken);

        if (existing != null)
        {
            throw new InvalidOperationException("This student-guardian relationship already exists.");
        }

        var studentGuardian = new StudentGuardian
        {
            StudentId = student.StudentId,
            GuardianId = guardian.GuardianId,
            TenantId = tenant.TenantId,
            RelationshipNotes = dto.RelationshipNotes,
            RelationshipNotesNormalized = dto.RelationshipNotes?.ToLowerInvariant(),
            IsAuthorizedPickup = dto.IsAuthorizedPickup,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Set<StudentGuardian>().Add(studentGuardian);
        await _context.SaveChangesAsync(cancellationToken);

        // Load navigation properties
        await _context.Entry(studentGuardian)
            .Reference(sg => sg.Student)
            .LoadAsync(cancellationToken);
        await _context.Entry(studentGuardian)
            .Reference(sg => sg.Guardian)
            .LoadAsync(cancellationToken);
        await _context.Entry(studentGuardian.Guardian)
            .Reference(g => g.User)
            .LoadAsync(cancellationToken);
        await _context.Entry(studentGuardian)
            .Reference(sg => sg.Tenant)
            .LoadAsync(cancellationToken);

        return new CreateStudentGuardianResponse
        {
            StudentGuardian = new StudentGuardianDto
            {
                StudentUuid = studentGuardian.Student.StudentUuid,
                StudentName = $"{studentGuardian.Student.FirstName} {studentGuardian.Student.LastName}",
                GuardianUuid = studentGuardian.Guardian.GuardianUuid,
                GuardianName = studentGuardian.Guardian.User.FullName,
                RelationshipType = guardian.RelationshipType.Name,
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
