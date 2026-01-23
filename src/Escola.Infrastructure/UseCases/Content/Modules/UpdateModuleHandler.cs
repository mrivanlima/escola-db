using Escola.Application.Content.Modules;
using Escola.Application.Services;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.Modules;

public class UpdateModuleHandler : IUpdateModuleHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateModuleHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateModuleResponse> Handle(UpdateModuleRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var module = await _context.Modules
            .Where(m => m.ModuleUuid == request.ModuleUuid)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Module with UUID {request.ModuleUuid} not found");

        if (!string.IsNullOrWhiteSpace(request.Module.ModuleName))
            module.ModuleName = request.Module.ModuleName;

        if (request.Module.Description != null)
            module.Description = request.Module.Description;

        if (request.Module.ModuleType != null)
            module.ModuleType = request.Module.ModuleType;

        if (request.Module.DifficultyLevel.HasValue)
            module.DifficultyLevel = request.Module.DifficultyLevel;

        if (request.Module.RecommendedAgeMin.HasValue)
            module.RecommendedAgeMin = request.Module.RecommendedAgeMin;

        if (request.Module.RecommendedAgeMax.HasValue)
            module.RecommendedAgeMax = request.Module.RecommendedAgeMax;

        if (request.Module.DisplayOrder.HasValue)
            module.DisplayOrder = request.Module.DisplayOrder;

        if (request.Module.ThumbnailUrl != null)
            module.ThumbnailUrl = request.Module.ThumbnailUrl;

        if (request.Module.ModuleConfig != null)
            module.ModuleConfig = request.Module.ModuleConfig;

        if (request.Module.IsPublished.HasValue)
            module.IsPublished = request.Module.IsPublished.Value;

        module.UpdatedAt = DateTimeOffset.UtcNow;
        module.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        var dto = new ModuleDto
        {
            ModuleUuid = module.ModuleUuid,
            ModuleName = module.ModuleName,
            Description = module.Description,
            ModuleType = module.ModuleType,
            DifficultyLevel = module.DifficultyLevel,
            RecommendedAgeMin = module.RecommendedAgeMin,
            RecommendedAgeMax = module.RecommendedAgeMax,
            DisplayOrder = module.DisplayOrder,
            ThumbnailUrl = module.ThumbnailUrl,
            ModuleConfig = module.ModuleConfig,
            IsPublished = module.IsPublished,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return new UpdateModuleResponse(dto);
    }
}
