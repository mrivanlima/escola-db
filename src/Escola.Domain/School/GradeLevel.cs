namespace Escola.Domain.School;

/// <summary>
/// Represents a grade level in the educational system.
/// Lookup table for grade/year classification (e.g., "1st Grade", "Year 7").
/// </summary>
public class GradeLevel
{
    /// <summary>
    /// Internal primary key. Never exposed to API.
    /// </summary>
    public short GradeLevelId { get; set; }

    /// <summary>
    /// External UUID for API exposure. This is the only ID exposed to clients.
    /// </summary>
    public Guid GradeLevelUuid { get; set; }

    /// <summary>
    /// Tenant (school/organization) this grade level belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Display name of the grade level (e.g., "1st Grade", "Year 7", "Kindergarten").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of Name (lowercase, unaccented) for searching.
    /// </summary>
    public string NameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display order for sorting grade levels sequentially (1, 2, 3...).
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

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Escola.Domain.Identity.Tenant Tenant { get; set; } = null!;
}
