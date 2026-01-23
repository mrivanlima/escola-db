using Escola.Application.UseCases.Students.GetStudents;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Get Students handler in Infrastructure layer.
/// </summary>
public class GetStudentsHandler : IGetStudentsHandler
{
    private readonly EscolaDbContext _context;

    public GetStudentsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentResponse>> Handle(GetStudentsRequest request, CancellationToken cancellationToken = default)
    {
        // AsNoTracking for read-only queries
        // Global query filter automatically excludes soft-deleted records (DeletedAt IS NULL)
        var students = await _context.Students
            .AsNoTracking()
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
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
            .ToListAsync(cancellationToken);

        return students;
    }
}
