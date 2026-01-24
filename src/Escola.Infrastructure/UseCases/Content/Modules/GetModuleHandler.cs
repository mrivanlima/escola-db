using Escola.Application.Content.Modules;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.Modules;

public class GetModuleHandler : IGetModuleHandler
{
    private readonly EscolaDbContext _context;

    public GetModuleHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetModuleResponse> Handle(GetModuleRequest request, CancellationToken cancellationToken)
    {
        var module = await _context.Modules
            .Include(m => m.ModuleType)
            .Where(m => m.ModuleUuid == request.ModuleUuid)
            .Select(m => new ModuleDto
            {
                ModuleUuid = m.ModuleUuid,
                ModuleName = m.ModuleName,
                Description = m.Description,
                ModuleType = m.ModuleType != null ? m.ModuleType.ModuleTypeCode : null,
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
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Module with UUID {request.ModuleUuid} not found");

        return new GetModuleResponse(module);
    }
}
