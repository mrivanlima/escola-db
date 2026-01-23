namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for updating an existing school year.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public class UpdateSchoolYearDto
{
    /// <summary>
    /// Display name of the school year.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Start date of the school year.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// End date of the school year.
    /// Must be after StartDate if both are provided.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Indicates if this should be the current active school year.
    /// </summary>
    public bool? IsCurrent { get; set; }

    /// <summary>
    /// Indicates if this school year is active.
    /// </summary>
    public bool? IsActive { get; set; }
}
