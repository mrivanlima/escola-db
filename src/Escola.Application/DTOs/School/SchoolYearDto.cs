namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for SchoolYear responses.
/// </summary>
public class SchoolYearDto
{
    /// <summary>
    /// External school year identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid SchoolYearUuid { get; set; }

    /// <summary>
    /// External tenant UUID.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name.
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// Display name of the school year (e.g., "2024-2025").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the school year.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the school year.
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Indicates if this is the current active school year.
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Indicates if this school year is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the school year was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the school year was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
