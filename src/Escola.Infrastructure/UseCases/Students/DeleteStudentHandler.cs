using Escola.Application.UseCases.Students.DeleteStudent;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Delete Student handler in Infrastructure layer.
/// Uses soft delete pattern - sets DeletedAt and IsActive = false.
/// </summary>
public class DeleteStudentHandler : IDeleteStudentHandler
{
    private readonly EscolaDbContext _context;

    public DeleteStudentHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Guid studentUuid, CancellationToken cancellationToken = default)
    {
        // Find the student by UUID
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentUuid == studentUuid, cancellationToken);

        if (student == null)
        {
            return false;
        }

        // Soft delete: Set DeletedAt and IsActive = false
        // Do NOT use _context.Students.Remove() - that would be a hard delete
        student.DeletedAt = DateTimeOffset.UtcNow;
        student.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
