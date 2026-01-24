namespace Escola.Application.DTOs.Game;

/// <summary>
/// DTO for badge type lookup data.
/// </summary>
public record BadgeTypeDto
{
    public Guid BadgeTypeUuid { get; init; }
    public string TypeCode { get; init; } = string.Empty;
    public string TypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? IconName { get; init; }
    public string? ColorCode { get; init; }
    public int DisplayOrder { get; init; }
}
