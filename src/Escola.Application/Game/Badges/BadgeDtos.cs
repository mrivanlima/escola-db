namespace Escola.Application.Game.Badges;

public record BadgeDto
{
    public required Guid BadgeUuid { get; init; }
    public required string BadgeName { get; init; }
    public string? Description { get; init; }
    public string? BadgeType { get; init; }
    public string? IconUrl { get; init; }
    public string? Rarity { get; init; }
    public int? PointsRequired { get; init; }
    public string? Criteria { get; init; }
    public int? DisplayOrder { get; init; }
    public bool IsActive { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public record CreateBadgeDto
{
    public required string BadgeName { get; init; }
    public string? Description { get; init; }
    public string? BadgeType { get; init; }
    public string? IconUrl { get; init; }
    public string? Rarity { get; init; }
    public int? PointsRequired { get; init; }
    public string? Criteria { get; init; }
    public int? DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}

public record UpdateBadgeDto
{
    public string? BadgeName { get; init; }
    public string? Description { get; init; }
    public string? BadgeType { get; init; }
    public string? IconUrl { get; init; }
    public string? Rarity { get; init; }
    public int? PointsRequired { get; init; }
    public string? Criteria { get; init; }
    public int? DisplayOrder { get; init; }
    public bool? IsActive { get; init; }
}
