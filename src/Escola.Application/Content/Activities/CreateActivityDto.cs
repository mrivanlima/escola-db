namespace Escola.Application.Content.Activities;

public record CreateActivityDto
{
    public required Guid ModuleUuid { get; init; }
    public required string ActivityName { get; init; }
    public string? Description { get; init; }
    public string? ActivityType { get; init; }
    public int? DisplayOrder { get; init; }
    public int? EstimatedDuration { get; init; }
    public int? PointsReward { get; init; }
    public string? ActivityData { get; init; }
    public string? ThumbnailUrl { get; init; }
    public bool IsPublished { get; init; }
}
