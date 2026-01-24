namespace Escola.Domain.Content;

/// <summary>
/// Represents an individual learning activity within a module.
/// </summary>
public class Activity
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ActivityId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ActivityUuid { get; set; }

    /// <summary>
    /// Module ID this activity belongs to.
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// Activity name.
    /// </summary>
    public string ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized activity name for search (lowercase, no accents).
    /// </summary>
    public string ActivityNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Activity description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Normalized description for search (lowercase, no accents).
    /// </summary>
    public string? DescriptionNormalized { get; set; }

    /// <summary>
    /// Activity type ID (FK to activity_types).
    /// </summary>
    public short ActivityTypeId { get; set; }

    /// <summary>
    /// Display order for sorting within the module.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Estimated duration in minutes.
    /// </summary>
    public int? EstimatedDuration { get; set; }

    /// <summary>
    /// Points reward for completing the activity.
    /// </summary>
    public int PointsReward { get; set; }

    /// <summary>
    /// JSON data specific to the activity (questions, answers, etc.).
    /// </summary>
    public string ActivityData { get; set; } = "{}";

    /// <summary>
    /// Thumbnail URL.
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Indicates if the activity is published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Timestamp when the activity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this activity.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the activity was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this activity.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Module Module { get; set; } = null!;
    public virtual ActivityType? ActivityType { get; set; }
    public virtual ICollection<ActivityResource> ActivityResources { get; set; } = new List<ActivityResource>();
    public virtual ICollection<Game.StudentProgress> StudentProgress { get; set; } = new List<Game.StudentProgress>();
}
