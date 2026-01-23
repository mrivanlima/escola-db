using Escola.Domain.Identity;

namespace Escola.Domain.School;

/// <summary>
/// Represents a student enrollment status (Active, Inactive, Transferred, etc.)
/// This is a lookup table.
/// </summary>
public class EnrollmentStatus
{
    /// <summary>
    /// Primary key - internal identifier (SMALLINT)
    /// Maps to: enrollment_status_id
    /// </summary>
    public short EnrollmentStatusId { get; set; }

    /// <summary>
    /// External UUID identifier for API exposure
    /// Maps to: enrollment_status_uuid
    /// </summary>
    public Guid EnrollmentStatusUuid { get; set; }

    /// <summary>
    /// Foreign key to tenant
    /// Maps to: tenant_id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Enrollment status name (e.g., "Active", "Inactive", "Transferred")
    /// Maps to: name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized name for case-insensitive searches
    /// Maps to: name_normalized
    /// </summary>
    public string NameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display order for UI sorting
    /// Maps to: display_order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this enrollment status is currently active
    /// Maps to: is_active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Navigation property to tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;
}
