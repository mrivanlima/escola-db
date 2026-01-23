using Escola.Application.UseCases.Teachers.UpdateTeacher;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

/// <summary>
/// Handler for updating a teacher.
/// </summary>
public class UpdateTeacherHandler : IUpdateTeacherHandler
{
    private readonly EscolaDbContext _context;

    public UpdateTeacherHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateTeacherResponse?> Handle(Guid teacherUuid, UpdateTeacherRequest request, CancellationToken cancellationToken)
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(t => t.TeacherUuid == teacherUuid, cancellationToken);

        if (teacher == null)
        {
            return null;
        }

        teacher.Specialization = request.Specialization;
        teacher.HireDate = request.HireDate;
        teacher.TeacherConfig = request.TeacherConfig;
        teacher.UpdatedAt = DateTimeOffset.UtcNow;
        teacher.UpdatedBy = 1; // TODO: Replace with actual authenticated user

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTeacherResponse
        {
            TeacherUuid = teacher.TeacherUuid,
            Specialization = teacher.Specialization,
            HireDate = teacher.HireDate,
            UpdatedAt = teacher.UpdatedAt
        };
    }
}
