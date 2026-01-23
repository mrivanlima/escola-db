namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for creating a new school year.
/// </summary>
public class CreateSchoolYearDto
{
    /// <summary>
    /// External tenant UUID.
    /// Required.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Display name of the school year (e.g., "2024-2025").
    /// Required.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the school year.
    /// Required.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the school year.
    /// Required. Must be after StartDate.
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Indicates if this should be the current active school year.
    /// Defaults to false.
    /// </summary>
    public bool IsCurrent { get; set; } = false;

    /// <summary>
    /// Indicates if this school year should be active.
    /// Defaults to true.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
