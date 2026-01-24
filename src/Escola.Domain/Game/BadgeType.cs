namespace Escola.Domain.Game;

/// <summary>
/// Lookup table for badge types.
/// </summary>
public class BadgeType
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short BadgeTypeId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid BadgeTypeUuid { get; set; }

    /// <summary>
    /// Badge type code (e.g., 'achievement', 'milestone', 'mastery').
    /// </summary>
    public string TypeCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized type code for search.
    /// </summary>
    public string TypeCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Achievement", "Milestone", "Mastery").
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized type name for search.
    /// </summary>
    public string TypeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the badge type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Icon identifier for frontend.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this badge type is active.
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
