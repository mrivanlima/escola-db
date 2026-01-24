namespace Escola.Domain.Game;

/// <summary>
/// Lookup table for student progress statuses.
/// </summary>
public class ProgressStatus
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short StatusId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid StatusUuid { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Status code (e.g., 'not_started', 'in_progress', 'completed').
    /// </summary>
    public string StatusCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized status code for search.
    /// </summary>
    public string StatusCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Not Started", "In Progress", "Completed").
    /// </summary>
    public string StatusName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized status name for search.
    /// </summary>
    public string StatusNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the status.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Icon identifier for frontend.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Hex color for UI theming.
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Indicates if this is a final state.
    /// </summary>
    public bool IsFinalState { get; set; }

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
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual ICollection<StudentProgress> StudentProgresses { get; set; } = new List<StudentProgress>();
}
