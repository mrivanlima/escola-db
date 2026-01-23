namespace Escola.Domain.School;

/// <summary>
/// Represents a school year/academic year.
/// Defines time periods for academic activities.
/// </summary>
public class SchoolYear
{
    /// <summary>
    /// Internal primary key. Never exposed to API.
    /// </summary>
    public short SchoolYearId { get; set; }

    /// <summary>
    /// External UUID for API exposure. This is the only ID exposed to clients.
    /// </summary>
    public Guid SchoolYearUuid { get; set; }

    /// <summary>
    /// Tenant (school/organization) this school year belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Display name of the school year (e.g., "2024-2025", "Fall 2024").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of Name (lowercase, unaccented) for searching.
    /// </summary>
    public string NameNormalized { get; set; } = string.Empty;

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

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Escola.Domain.Identity.Tenant Tenant { get; set; } = null!;
}
