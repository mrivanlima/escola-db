namespace Escola.Domain.Game;

/// <summary>
/// Tracks badges earned by students.
/// </summary>
public class StudentBadge
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int StudentBadgeId { get; set; }

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
    public DateOnly EarnedAt { get; set; }

    /// <summary>
    /// JSON metadata about how the badge was earned.
    /// </summary>
    public string? EarnMetadata { get; set; }

    /// <summary>
    /// Timestamp when the record was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual School.Student Student { get; set; } = null!;
    public virtual Badge Badge { get; set; } = null!;
    public virtual Identity.Tenant Tenant { get; set; } = null!;
}
