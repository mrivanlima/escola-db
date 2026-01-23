namespace Escola.Domain.School;

/// <summary>
/// Represents a teacher profile.
/// </summary>
public class Teacher
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int TeacherId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid TeacherUuid { get; set; }

    /// <summary>
    /// Tenant ID this teacher belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Associated user ID (links to AppUser).
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Teacher specialization (e.g., "Math", "Science").
    /// </summary>
    public string? Specialization { get; set; }

    /// <summary>
    /// Normalized specialization for search (lowercase, no accents).
    /// </summary>
    public string? SpecializationNormalized { get; set; }

    /// <summary>
    /// Date when the teacher was hired.
    /// </summary>
    public DateOnly? HireDate { get; set; }

    /// <summary>
    /// JSON configuration for teacher-specific settings.
    /// </summary>
    public string? TeacherConfig { get; set; }

    /// <summary>
    /// Indicates if the teacher profile is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when the teacher was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this teacher.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the teacher was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this teacher.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual Identity.AppUser User { get; set; } = null!;
}
