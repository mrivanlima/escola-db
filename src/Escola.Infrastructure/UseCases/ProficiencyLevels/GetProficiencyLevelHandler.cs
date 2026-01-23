using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevel;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ProficiencyLevels;

public class GetProficiencyLevelHandler : IGetProficiencyLevelHandler
{
    private readonly EscolaDbContext _context;

    public GetProficiencyLevelHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetProficiencyLevelResponse> HandleAsync(GetProficiencyLevelRequest request, CancellationToken cancellationToken)
    {
        var proficiency = await _context.ProficiencyLevels
            .Include(p => p.Tenant)
            .FirstOrDefaultAsync(p => p.ProficiencyUuid == request.ProficiencyUuid, cancellationToken)
            ?? throw new InvalidOperationException($"Proficiency level with UUID {request.ProficiencyUuid} not found");

        return new GetProficiencyLevelResponse
        {
            ProficiencyLevel = new ProficiencyLevelDto
            {
                ProficiencyUuid = proficiency.ProficiencyUuid,
                TenantUuid = proficiency.Tenant.TenantUuid,
                TenantName = proficiency.Tenant.TenantName,
                ProficiencyCode = proficiency.ProficiencyCode,
                ProficiencyName = proficiency.ProficiencyName,
                Description = proficiency.Description,
                MinimumYears = proficiency.MinimumYears,
                IconName = proficiency.IconName,
                ColorCode = proficiency.ColorCode,
                DisplayOrder = proficiency.DisplayOrder
            }
        };
    }
}
