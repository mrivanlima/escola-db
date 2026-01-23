using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.Teachers;

/// <summary>
/// Handler for retrieving all teachers.
/// </summary>
public class GetTeachersHandler : IGetTeachersHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<GetTeachersHandler> _logger;

    public GetTeachersHandler(EscolaDbContext context, ILogger<GetTeachersHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TeacherResponse>> Handle(GetTeachersRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all teachers");

        var teachers = await _context.Teachers
            .AsNoTracking()
            .OrderBy(t => t.CreatedAt)
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
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} teachers", teachers.Count);

        return teachers;
    }
}
