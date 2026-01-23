using Escola.Application.UseCases.Students.GetStudent;
using Escola.Application.UseCases.Students.GetStudents;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Get Student handler in Infrastructure layer.
/// </summary>
public class GetStudentHandler : IGetStudentHandler
{
    private readonly EscolaDbContext _context;

    public GetStudentHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponse?> Handle(Guid studentUuid, CancellationToken cancellationToken = default)
    {
        // AsNoTracking for read-only queries
        // Global query filter automatically excludes soft-deleted records
        var student = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentUuid == studentUuid)
            .Select(s => new StudentResponse
            {
                StudentUuid = s.StudentUuid,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MiddleName = s.MiddleName,
                Nickname = s.Nickname,
                BirthDate = s.BirthDate,
                AvatarConfig = s.AvatarConfig,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return student;
    }
}
