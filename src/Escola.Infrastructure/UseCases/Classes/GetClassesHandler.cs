using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.Classes;

/// <summary>
/// Handler for retrieving all classes.
/// </summary>
public class GetClassesHandler : IGetClassesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<GetClassesHandler> _logger;

    public GetClassesHandler(EscolaDbContext context, ILogger<GetClassesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ClassResponse>> Handle(GetClassesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all classes");

        var classes = await _context.Classes
            .AsNoTracking()
            .OrderBy(c => c.SchoolYear)
            .ThenBy(c => c.ClassName)
            .Select(c => new ClassResponse
            {
                ClassUuid = c.ClassUuid,
                ClassName = c.ClassName,
                GradeLevel = c.GradeLevel,
                SchoolYear = c.SchoolYear,
                MaxStudents = c.MaxStudents,
                ClassConfig = c.ClassConfig,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} classes", classes.Count);

        return classes;
    }
}
