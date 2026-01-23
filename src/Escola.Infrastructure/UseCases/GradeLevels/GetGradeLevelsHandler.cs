using Escola.Application.DTOs.School;
using Escola.Application.UseCases.GradeLevels.GetGradeLevels;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.GradeLevels;

/// <summary>
/// Handler for getting all grade levels.
/// </summary>
public class GetGradeLevelsHandler : IGetGradeLevelsHandler
{
    private readonly EscolaDbContext _context;

    public GetGradeLevelsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetGradeLevelsResponse> HandleAsync(
        GetGradeLevelsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Domain.School.GradeLevel>()
            .Include(g => g.Tenant)
            .Where(g => g.DeletedAt == null);

        // Filter by tenant if provided
        if (request.TenantUuid.HasValue)
        {
            query = query.Where(g => g.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var gradeLevels = await query
            .OrderBy(g => g.DisplayOrder)
            .ToListAsync(cancellationToken);

        return new GetGradeLevelsResponse
        {
            GradeLevels = gradeLevels.Select(g => new GradeLevelDto
            {
                GradeLevelUuid = g.GradeLevelUuid,
                TenantUuid = g.Tenant.TenantUuid,
                TenantName = g.Tenant.TenantName,
                Name = g.Name,
                DisplayOrder = g.DisplayOrder,
                Description = g.Description,
                IsActive = g.IsActive,
                CreatedAt = g.CreatedAt.DateTime,
                UpdatedAt = g.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
