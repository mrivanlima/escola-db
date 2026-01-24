namespace Escola.Domain.Game;

/// <summary>
/// Tracks student progress on activities.
/// </summary>
public class StudentProgress
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ProgressId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ProgressUuid { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Activity ID.
    /// </summary>
    public int ActivityId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Progress status ID (FK to progress_statuses).
    /// </summary>
    public short StatusId { get; set; }

    /// <summary>
    /// Score achieved.
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// Number of attempts.
    /// </summary>
    public int AttemptsCount { get; set; }

    /// <summary>
    /// Time spent in seconds.
    /// </summary>
    public int? TimeSpentSeconds { get; set; }

    /// <summary>
    /// JSON data for progress details.
    /// </summary>
    public string? ProgressData { get; set; }

    /// <summary>
    /// Date when the activity was completed.
    /// </summary>
    public DateTimeOffset? CompletionDate { get; set; }

    /// <summary>
    /// Timestamp when the progress was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this record.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the progress was last updated.
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
    public virtual Content.Activity Activity { get; set; } = null!;
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual ProgressStatus ProgressStatus { get; set; } = null!;
}
