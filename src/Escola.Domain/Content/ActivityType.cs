namespace Escola.Domain.Content;

/// <summary>
/// Lookup table for activity types.
/// </summary>
public class ActivityType
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short ActivityTypeId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ActivityTypeUuid { get; set; }

    /// <summary>
    /// Activity type code (e.g., 'quiz', 'video', 'game').
    /// </summary>
    public string ActivityTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized activity type code for search.
    /// </summary>
    public string ActivityTypeCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Interactive Quiz", "Video Lesson").
    /// </summary>
    public string ActivityTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized activity type name for search.
    /// </summary>
    public string ActivityTypeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the activity type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Icon identifier for frontend.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Default points for this activity type.
    /// </summary>
    public int? DefaultPoints { get; set; }

    /// <summary>
    /// Indicates if interaction is required.
    /// </summary>
    public bool RequiresInteraction { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

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
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
