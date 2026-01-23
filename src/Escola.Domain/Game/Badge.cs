namespace Escola.Domain.Game;

/// <summary>
/// Represents an achievement badge configuration.
/// </summary>
public class Badge
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int BadgeId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid BadgeUuid { get; set; }

    /// <summary>
    /// Badge name.
    /// </summary>
    public string BadgeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized badge name for search (lowercase, no accents).
    /// </summary>
    public string BadgeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Badge description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Normalized description for search (lowercase, no accents).
    /// </summary>
    public string? DescriptionNormalized { get; set; }

    /// <summary>
    /// Badge type (e.g., "completion", "streak", "mastery").
    /// </summary>
    public string? BadgeType { get; set; }

    /// <summary>
    /// Icon URL.
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// Rarity level (e.g., "common", "rare", "legendary").
    /// </summary>
    public string? Rarity { get; set; }

    /// <summary>
    /// Points value of the badge.
    /// </summary>
    public int? PointsValue { get; set; }

    /// <summary>
    /// JSON criteria for unlocking the badge.
    /// </summary>
    public string? UnlockCriteria { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the badge is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when the badge was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this badge.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the badge was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this badge.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<StudentBadge> StudentBadges { get; set; } = new List<StudentBadge>();
}
