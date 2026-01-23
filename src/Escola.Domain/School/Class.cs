namespace Escola.Domain.School;

/// <summary>
/// Represents a class/group in the school.
/// </summary>
public class Class
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ClassId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ClassUuid { get; set; }

    /// <summary>
    /// Tenant ID this class belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Class name (e.g., "Morning Kindergarten").
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized class name for search (lowercase, no accents).
    /// </summary>
    public string ClassNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Grade level (e.g., "K", "1st", "2nd").
    /// </summary>
    public string? GradeLevel { get; set; }

    /// <summary>
    /// Normalized grade level for search (lowercase, no accents).
    /// </summary>
    public string? GradeLevelNormalized { get; set; }

    /// <summary>
    /// School year (e.g., "2025-2026").
    /// </summary>
    public string? SchoolYear { get; set; }

    /// <summary>
    /// Normalized school year for search (lowercase).
    /// </summary>
    public string? SchoolYearNormalized { get; set; }

    /// <summary>
    /// Maximum number of students allowed.
    /// </summary>
    public int? MaxStudents { get; set; }

    /// <summary>
    /// JSON configuration for class-specific settings.
    /// </summary>
    public string? ClassConfig { get; set; }

    /// <summary>
    /// Indicates if the class is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when the class was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this class.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the class was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this class.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
}
