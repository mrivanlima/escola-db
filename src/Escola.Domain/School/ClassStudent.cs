namespace Escola.Domain.School;

/// <summary>
/// Many-to-many relationship tracking student enrollment in classes.
/// </summary>
public class ClassStudent
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ClassStudentId { get; set; }

    /// <summary>
    /// Class ID.
    /// </summary>
    public int ClassId { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Date when the student was enrolled.
    /// </summary>
    public DateOnly? EnrollmentDate { get; set; }

    /// <summary>
    /// Enrollment status (e.g., "active", "withdrawn", "completed").
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Timestamp when the enrollment was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this enrollment.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the enrollment was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this enrollment.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Class Class { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
    public virtual Identity.Tenant Tenant { get; set; } = null!;
}
