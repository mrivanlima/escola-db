using Escola.Application.UseCases.Classes.GetClass;
using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

/// <summary>
/// Handler for retrieving a single class by UUID.
/// </summary>
public class GetClassHandler : IGetClassHandler
{
    private readonly EscolaDbContext _context;

    public GetClassHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<ClassResponse?> Handle(Guid classUuid, CancellationToken cancellationToken)
    {
        var classEntity = await _context.Classes
            .AsNoTracking()
            .Where(c => c.ClassUuid == classUuid)
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
            .FirstOrDefaultAsync(cancellationToken);

        return classEntity;
    }
}
