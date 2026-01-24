namespace Escola.Application.Game.StudentBadges;

public record StudentBadgeDto
{
    public required Guid StudentUuid { get; init; }
    public required string StudentName { get; init; }
    public required Guid BadgeUuid { get; init; }
    public required string BadgeName { get; init; }
    public required string BadgeIconUrl { get; init; }
    public required Guid TenantUuid { get; init; }
    public required DateTimeOffset EarnedAt { get; init; }
    public string? EarnMetadata { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}

public record AwardStudentBadgeDto
{
    public required Guid StudentUuid { get; init; }
    public required Guid BadgeUuid { get; init; }
    public DateTimeOffset? EarnedAt { get; init; }
    public string? EarnMetadata { get; init; }
}
