using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

/// <summary>
/// Handler for retrieving a single teacher by UUID.
/// </summary>
public class GetTeacherHandler : IGetTeacherHandler
{
    private readonly EscolaDbContext _context;

    public GetTeacherHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherResponse?> Handle(Guid teacherUuid, CancellationToken cancellationToken)
    {
        var teacher = await _context.Teachers
            .AsNoTracking()
            .Where(t => t.TeacherUuid == teacherUuid)
            .Select(t => new TeacherResponse
            {
                TeacherUuid = t.TeacherUuid,
                Specialization = t.Specialization,
                HireDate = t.HireDate,
                TeacherConfig = t.TeacherConfig,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return teacher;
    }
}
