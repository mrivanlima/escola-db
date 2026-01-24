using Escola.Application.Content.Modules;
using Escola.Application.Services;
using Escola.Domain.Content;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.Modules;

public class CreateModuleHandler : ICreateModuleHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateModuleHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateModuleResponse> Handle(CreateModuleRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        // Resolve ModuleType code to FK
        short? moduleTypeId = null;
        if (!string.IsNullOrWhiteSpace(request.Module.ModuleType))
        {
            var typeCodeNormalized = request.Module.ModuleType.ToLowerInvariant();
            var moduleType = await _context.ModuleTypes
                .Where(mt => mt.ModuleTypeCodeNormalized == typeCodeNormalized && mt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            moduleTypeId = moduleType?.ModuleTypeId;
        }

        var module = new Module
        {
            ModuleUuid = Guid.NewGuid(),
            ModuleName = request.Module.ModuleName,
            Description = request.Module.Description,
            ModuleTypeId = moduleTypeId ?? (short)0,
            DifficultyLevel = request.Module.DifficultyLevel ?? 1,
            RecommendedAgeMin = request.Module.RecommendedAgeMin,
            RecommendedAgeMax = request.Module.RecommendedAgeMax,
            DisplayOrder = request.Module.DisplayOrder ?? 0,
            ThumbnailUrl = request.Module.ThumbnailUrl,
            ModuleConfig = request.Module.ModuleConfig,
            IsPublished = request.Module.IsPublished,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userId
        };

        _context.Modules.Add(module);
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

        return new CreateModuleResponse(dto);
    }
}
