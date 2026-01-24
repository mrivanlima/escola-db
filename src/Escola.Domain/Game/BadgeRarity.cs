namespace Escola.Domain.Game;

/// <summary>
/// Lookup table for badge rarities.
/// </summary>
public class BadgeRarity
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short RarityId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid RarityUuid { get; set; }

    /// <summary>
    /// Rarity code (e.g., 'common', 'rare', 'epic', 'legendary').
    /// </summary>
    public string RarityCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized rarity code for search.
    /// </summary>
    public string RarityCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Common", "Rare", "Epic", "Legendary").
    /// </summary>
    public string RarityName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized rarity name for search.
    /// </summary>
    public string RarityNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the rarity level.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Hex color for UI theming.
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Point multiplier for this rarity.
    /// </summary>
    public decimal? PointMultiplier { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this badge rarity is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this record.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this record.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();
}
