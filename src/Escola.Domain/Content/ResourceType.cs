namespace Escola.Domain.Content;

/// <summary>
/// Lookup table for resource types.
/// </summary>
public class ResourceType
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short ResourceTypeId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ResourceTypeUuid { get; set; }

    /// <summary>
    /// Resource type code (e.g., 'image', 'video', 'audio', 'document').
    /// </summary>
    public string ResourceTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized resource type code for search.
    /// </summary>
    public string ResourceTypeCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Image", "Video", "Audio File").
    /// </summary>
    public string ResourceTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized resource type name for search.
    /// </summary>
    public string ResourceTypeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the resource type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Allowed MIME types as JSON array.
    /// </summary>
    public string? MimeTypes { get; set; }

    /// <summary>
    /// Maximum file size in MB.
    /// </summary>
    public int? MaxFileSizeMb { get; set; }

    /// <summary>
    /// Icon identifier for frontend.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this resource type is active.
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
    public virtual ICollection<ActivityResource> ActivityResources { get; set; } = new List<ActivityResource>();
}
