using Escola.Application.UseCases.Classes.UpdateClass;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

/// <summary>
/// Handler for updating a class.
/// </summary>
public class UpdateClassHandler : IUpdateClassHandler
{
    private readonly EscolaDbContext _context;

    public UpdateClassHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateClassResponse?> Handle(Guid classUuid, UpdateClassRequest request, CancellationToken cancellationToken)
    {
        var classEntity = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassUuid == classUuid, cancellationToken);

        if (classEntity == null)
        {
            return null;
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.ClassName))
        {
            classEntity.ClassName = request.ClassName;
        }

        if (request.GradeLevel != null)
        {
            classEntity.GradeLevel = request.GradeLevel;
        }

        if (!string.IsNullOrEmpty(request.SchoolYear))
        {
            classEntity.SchoolYear = request.SchoolYear;
        }

        if (request.MaxStudents.HasValue)
        {
            classEntity.MaxStudents = request.MaxStudents;
        }

        if (request.ClassConfig != null)
        {
            classEntity.ClassConfig = request.ClassConfig;
        }

        classEntity.UpdatedAt = DateTimeOffset.UtcNow;
        classEntity.UpdatedBy = 1; // TODO: Replace with actual authenticated user

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateClassResponse
        {
            ClassUuid = classEntity.ClassUuid,
            ClassName = classEntity.ClassName,
            SchoolYear = classEntity.SchoolYear,
            UpdatedAt = classEntity.UpdatedAt
        };
    }
}
