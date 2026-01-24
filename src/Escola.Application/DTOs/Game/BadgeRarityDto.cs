namespace Escola.Application.DTOs.Game;

/// <summary>
/// DTO for badge rarity lookup data.
/// </summary>
public record BadgeRarityDto
{
    public Guid RarityUuid { get; init; }
    public string RarityCode { get; init; } = string.Empty;
    public string RarityName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ColorCode { get; init; }
    public decimal? PointMultiplier { get; init; }
    public int DisplayOrder { get; init; }
}
