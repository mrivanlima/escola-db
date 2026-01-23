namespace Escola.Application.Content.Modules;

public record UpdateModuleDto
{
    public string? ModuleName { get; init; }
    public string? Description { get; init; }
    public string? ModuleType { get; init; }
    public int? DifficultyLevel { get; init; }
    public int? RecommendedAgeMin { get; init; }
    public int? RecommendedAgeMax { get; init; }
    public int? DisplayOrder { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? ModuleConfig { get; init; }
    public bool? IsPublished { get; init; }
}
