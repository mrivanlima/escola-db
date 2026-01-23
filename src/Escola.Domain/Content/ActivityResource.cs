namespace Escola.Domain.Content;

/// <summary>
/// Links activities to media files (resources).
/// </summary>
public class ActivityResource
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ResourceUuid { get; set; }

    /// <summary>
    /// Activity ID this resource belongs to.
    /// </summary>
    public int ActivityId { get; set; }

    /// <summary>
    /// Resource name.
    /// </summary>
    public string ResourceName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized resource name for search (lowercase, no accents).
    /// </summary>
    public string ResourceNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Resource type (e.g., "image", "audio", "video").
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Media file ID (references the MediaFile entity).
    /// </summary>
    public Guid MediaFileId { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this resource is required for the activity.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Usage context description.
    /// </summary>
    public string? UsageContext { get; set; }

    /// <summary>
    /// JSON configuration for resource-specific settings.
    /// </summary>
    public string? ResourceConfig { get; set; }

    /// <summary>
    /// Indicates if the resource is published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Timestamp when the resource was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this resource.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the resource was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this resource.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Activity Activity { get; set; } = null!;
    public virtual Assets.MediaFile MediaFile { get; set; } = null!;
}
