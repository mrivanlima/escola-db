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

        var module = new Module
        {
            ModuleUuid = Guid.NewGuid(),
            ModuleName = request.Module.ModuleName,
            Description = request.Module.Description,
            ModuleType = request.Module.ModuleType,
            DifficultyLevel = request.Module.DifficultyLevel,
            RecommendedAgeMin = request.Module.RecommendedAgeMin,
            RecommendedAgeMax = request.Module.RecommendedAgeMax,
            DisplayOrder = request.Module.DisplayOrder,
            ThumbnailUrl = request.Module.ThumbnailUrl,
            ModuleConfig = request.Module.ModuleConfig,
            IsPublished = request.Module.IsPublished,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userId
        };

        _context.Modules.Add(module);
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

        return new CreateModuleResponse(dto);
    }
}
