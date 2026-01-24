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
    /// Resource type ID (FK to resource_types).
    /// </summary>
    public short ResourceTypeId { get; set; }

    /// <summary>
    /// Media file ID (references the MediaFile entity).
    /// </summary>
    public int MediaFileId { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this resource is required for the activity.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Usage context ID (FK to usage_contexts).
    /// </summary>
    public short? UsageContextId { get; set; }

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
    public virtual ResourceType? ResourceType { get; set; }
    public virtual UsageContext? UsageContext { get; set; }
    public virtual Assets.MediaFile MediaFile { get; set; } = null!;
}
