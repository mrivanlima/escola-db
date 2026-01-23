namespace Escola.Application.Content.ActivityResources;

public record ActivityResourceDto
{
    public required Guid ResourceUuid { get; init; }
    public required Guid ActivityUuid { get; init; }
    public required string ActivityName { get; init; }
    public required string ResourceName { get; init; }
    public required string ResourceType { get; init; }
    public required Guid MediaFileUuid { get; init; }
    public required string MediaFileName { get; init; }
    public int? DisplayOrder { get; init; }
    public bool IsRequired { get; init; }
    public string? UsageContext { get; init; }
    public string? ResourceConfig { get; init; }
    public bool IsPublished { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public record CreateActivityResourceDto
{
    public required Guid ActivityUuid { get; init; }
    public required string ResourceName { get; init; }
    public required string ResourceType { get; init; }
    public required Guid MediaFileUuid { get; init; }
    public int? DisplayOrder { get; init; }
    public bool IsRequired { get; init; }
    public string? UsageContext { get; init; }
    public string? ResourceConfig { get; init; }
    public bool IsPublished { get; init; }
}

public record UpdateActivityResourceDto
{
    public string? ResourceName { get; init; }
    public string? ResourceType { get; init; }
    public int? DisplayOrder { get; init; }
    public bool? IsRequired { get; init; }
    public string? UsageContext { get; init; }
    public string? ResourceConfig { get; init; }
    public bool? IsPublished { get; init; }
}
