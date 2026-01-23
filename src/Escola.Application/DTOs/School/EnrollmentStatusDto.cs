namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for EnrollmentStatus (read-only lookup table)
/// </summary>
public class EnrollmentStatusDto
{
    /// <summary>
    /// External UUID identifier
    /// </summary>
    public Guid EnrollmentStatusUuid { get; set; }

    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Enrollment status name (e.g., "Active", "Inactive", "Transferred")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display order for UI sorting
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this enrollment status is currently active
    /// </summary>
    public bool IsActive { get; set; }
}
