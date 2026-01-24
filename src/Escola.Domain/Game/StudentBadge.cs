namespace Escola.Domain.Game;

/// <summary>
/// Tracks badges earned by students.
/// </summary>
public class StudentBadge
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public long StudentBadgeId { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Badge ID.
    /// </summary>
    public int BadgeId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Date when the badge was earned.
    /// </summary>
    public DateTimeOffset EarnedAt { get; set; }

    /// <summary>
    /// Optional link to the progress entry that triggered the badge.
    /// </summary>
    public long? ProgressId { get; set; }

    /// <summary>
    /// Timestamp when the record was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this record.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the record was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this record.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual School.Student Student { get; set; } = null!;
    public virtual Badge Badge { get; set; } = null!;
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual StudentProgress? Progress { get; set; }
}
