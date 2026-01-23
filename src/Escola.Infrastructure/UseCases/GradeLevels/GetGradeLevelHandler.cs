using Escola.Application.DTOs.School;
using Escola.Application.UseCases.GradeLevels.GetGradeLevel;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.GradeLevels;

/// <summary>
/// Handler for getting a single grade level by UUID.
/// </summary>
public class GetGradeLevelHandler : IGetGradeLevelHandler
{
    private readonly EscolaDbContext _context;

    public GetGradeLevelHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetGradeLevelResponse> HandleAsync(
        GetGradeLevelRequest request,
        CancellationToken cancellationToken = default)
    {
        var gradeLevel = await _context.Set<Domain.School.GradeLevel>()
            .Include(g => g.Tenant)
            .FirstOrDefaultAsync(g => g.GradeLevelUuid == request.GradeLevelUuid && g.DeletedAt == null, cancellationToken);

        if (gradeLevel == null)
        {
            throw new InvalidOperationException("Grade level not found.");
        }

        return new GetGradeLevelResponse
        {
            GradeLevel = new GradeLevelDto
            {
                GradeLevelUuid = gradeLevel.GradeLevelUuid,
                TenantUuid = gradeLevel.Tenant.TenantUuid,
                TenantName = gradeLevel.Tenant.TenantName,
                Name = gradeLevel.Name,
                DisplayOrder = gradeLevel.DisplayOrder,
                Description = gradeLevel.Description,
                IsActive = gradeLevel.IsActive,
                CreatedAt = gradeLevel.CreatedAt.DateTime,
                UpdatedAt = gradeLevel.UpdatedAt?.DateTime
            }
        };
    }
}
