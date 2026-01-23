using Escola.Application.UseCases.Teachers.DeleteTeacher;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

/// <summary>
/// Handler for soft deleting a teacher.
/// </summary>
public class DeleteTeacherHandler : IDeleteTeacherHandler
{
    private readonly EscolaDbContext _context;

    public DeleteTeacherHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Guid teacherUuid, CancellationToken cancellationToken)
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(t => t.TeacherUuid == teacherUuid, cancellationToken);

        if (teacher == null)
        {
            return false;
        }

        // Soft delete
        teacher.DeletedAt = DateTimeOffset.UtcNow;
        teacher.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
