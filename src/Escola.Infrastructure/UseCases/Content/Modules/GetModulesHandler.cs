using Escola.Application.Content.Modules;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.Modules;

public class GetModulesHandler : IGetModulesHandler
{
    private readonly EscolaDbContext _context;

    public GetModulesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetModulesResponse> Handle(GetModulesRequest request, CancellationToken cancellationToken)
    {
        var modules = await _context.Modules
            .OrderBy(m => m.DisplayOrder).ThenBy(m => m.ModuleName)
            .Select(m => new ModuleDto
            {
                ModuleUuid = m.ModuleUuid,
                ModuleName = m.ModuleName,
                Description = m.Description,
                ModuleType = m.ModuleType,
                DifficultyLevel = m.DifficultyLevel,
                RecommendedAgeMin = m.RecommendedAgeMin,
                RecommendedAgeMax = m.RecommendedAgeMax,
                DisplayOrder = m.DisplayOrder,
                ThumbnailUrl = m.ThumbnailUrl,
                ModuleConfig = m.ModuleConfig,
                IsPublished = m.IsPublished,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetModulesResponse(modules);
    }
}
