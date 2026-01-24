namespace Escola.Domain.Content;

/// <summary>
/// Lookup table for resource usage contexts.
/// </summary>
public class UsageContext
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short UsageContextId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid UsageContextUuid { get; set; }

    /// <summary>
    /// Usage context code (e.g., 'instruction', 'example', 'practice').
    /// </summary>
    public string ContextCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized context code for search.
    /// </summary>
    public string ContextCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Instruction", "Example", "Practice Exercise").
    /// </summary>
    public string ContextName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized context name for search.
    /// </summary>
    public string ContextNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the usage context.
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
    /// Indicates if this usage context is active.
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
