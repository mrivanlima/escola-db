namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for GradeLevel responses.
/// Lookup table - read-only.
/// </summary>
public class GradeLevelDto
{
    /// <summary>
    /// External grade level identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid GradeLevelUuid { get; set; }

    /// <summary>
    /// External tenant UUID.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name.
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// Display name of the grade level (e.g., "1st Grade", "Year 7").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display order for sorting grade levels sequentially.
    /// </summary>
    public short DisplayOrder { get; set; }

    /// <summary>
    /// Optional description of this grade level.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this grade level is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the grade level was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the grade level was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
