using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevels;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ProficiencyLevels;

public class GetProficiencyLevelsHandler : IGetProficiencyLevelsHandler
{
    private readonly EscolaDbContext _context;

    public GetProficiencyLevelsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetProficiencyLevelsResponse> HandleAsync(GetProficiencyLevelsRequest request, CancellationToken cancellationToken)
    {
        var query = _context.ProficiencyLevels
            .Include(p => p.Tenant)
            .AsQueryable();

        if (request.TenantUuid.HasValue)
            query = query.Where(p => p.Tenant.TenantUuid == request.TenantUuid.Value);

        var levels = await query
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(cancellationToken);

        return new GetProficiencyLevelsResponse
        {
            ProficiencyLevels = levels.Select(p => new ProficiencyLevelDto
            {
                ProficiencyUuid = p.ProficiencyUuid,
                TenantUuid = p.Tenant.TenantUuid,
                TenantName = p.Tenant.TenantName,
                ProficiencyCode = p.ProficiencyCode,
                ProficiencyName = p.ProficiencyName,
                Description = p.Description,
                MinimumYears = p.MinimumYears,
                IconName = p.IconName,
                ColorCode = p.ColorCode,
                DisplayOrder = p.DisplayOrder
            }).ToList()
        };
    }
}
