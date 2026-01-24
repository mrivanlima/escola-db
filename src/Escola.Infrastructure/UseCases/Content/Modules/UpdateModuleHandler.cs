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
        {
            var typeCodeNormalized = request.Module.ModuleType.ToLowerInvariant();
            var moduleType = await _context.ModuleTypes
                .Where(mt => mt.ModuleTypeCodeNormalized == typeCodeNormalized && mt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            module.ModuleTypeId = moduleType?.ModuleTypeId ?? module.ModuleTypeId;
        }

        if (request.Module.DifficultyLevel.HasValue)
            module.DifficultyLevel = request.Module.DifficultyLevel.Value;

        if (request.Module.RecommendedAgeMin.HasValue)
            module.RecommendedAgeMin = request.Module.RecommendedAgeMin;

        if (request.Module.RecommendedAgeMax.HasValue)
            module.RecommendedAgeMax = request.Module.RecommendedAgeMax;

        if (request.Module.DisplayOrder.HasValue)
            module.DisplayOrder = request.Module.DisplayOrder.Value;

        if (request.Module.ThumbnailUrl != null)
            module.ThumbnailUrl = request.Module.ThumbnailUrl;

        if (request.Module.ModuleConfig != null)
            module.ModuleConfig = request.Module.ModuleConfig;

        if (request.Module.IsPublished.HasValue)
            module.IsPublished = request.Module.IsPublished.Value;

        module.UpdatedAt = DateTimeOffset.UtcNow;
        module.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        // Re-query with ModuleType join
        var moduleWithType = await _context.Modules
            .Include(m => m.ModuleType)
            .Where(m => m.ModuleId == module.ModuleId)
            .FirstOrDefaultAsync(cancellationToken);

        var dto = new ModuleDto
        {
            ModuleUuid = moduleWithType!.ModuleUuid,
            ModuleName = moduleWithType.ModuleName,
            Description = moduleWithType.Description,
            ModuleType = moduleWithType.ModuleType != null ? moduleWithType.ModuleType.ModuleTypeCode : null,
            DifficultyLevel = moduleWithType.DifficultyLevel,
            RecommendedAgeMin = moduleWithType.RecommendedAgeMin,
            RecommendedAgeMax = moduleWithType.RecommendedAgeMax,
            DisplayOrder = moduleWithType.DisplayOrder,
            ThumbnailUrl = moduleWithType.ThumbnailUrl,
            ModuleConfig = moduleWithType.ModuleConfig,
            IsPublished = moduleWithType.IsPublished,
            CreatedAt = moduleWithType.CreatedAt,
            UpdatedAt = moduleWithType.UpdatedAt
        };

        return new UpdateModuleResponse(dto);
    }
}
