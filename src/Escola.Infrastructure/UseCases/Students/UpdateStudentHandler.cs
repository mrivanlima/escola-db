using Escola.Application.UseCases.Students.UpdateStudent;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Update Student handler in Infrastructure layer.
/// </summary>
public class UpdateStudentHandler : IUpdateStudentHandler
{
    private readonly EscolaDbContext _context;

    public UpdateStudentHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateStudentResponse?> Handle(Guid studentUuid, UpdateStudentRequest request, CancellationToken cancellationToken = default)
    {
        // Find the student by UUID
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentUuid == studentUuid, cancellationToken);

        if (student == null)
        {
            return null;
        }

        // Update the student properties
        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.MiddleName = request.MiddleName;
        student.Nickname = request.Nickname;
        student.BirthDate = request.BirthDate;
        student.UpdatedAt = DateTimeOffset.UtcNow;
        student.UpdatedBy = 1; // TODO: Replace with actual authenticated user ID

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateStudentResponse
        {
            StudentUuid = student.StudentUuid,
            FirstName = student.FirstName,
            LastName = student.LastName,
            MiddleName = student.MiddleName,
            Nickname = student.Nickname,
            BirthDate = student.BirthDate,
            UpdatedAt = student.UpdatedAt
        };
    }
}
