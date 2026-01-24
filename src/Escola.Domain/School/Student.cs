namespace Escola.Domain.School;

/// <summary>
/// Represents a student (child profile).
/// </summary>
public class Student
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid StudentUuid { get; set; }

    /// <summary>
    /// Tenant ID this student belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Optional user ID for Hybrid Authentication Model.
    /// Links student to app_users for future student login capability.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Student's nickname/preferred name.
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// Normalized nickname for search (lowercase, no accents).
    /// </summary>
    public string NicknameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// First name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized first name for search (lowercase, no accents).
    /// </summary>
    public string FirstNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Middle name.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Normalized middle name for search (lowercase, no accents).
    /// </summary>
    public string? MiddleNameNormalized { get; set; }

    /// <summary>
    /// Last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized last name for search (lowercase, no accents).
    /// </summary>
    public string LastNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// JSON configuration for avatar settings.
    /// </summary>
    public string? AvatarConfig { get; set; }

    /// <summary>
    /// Indicates if the student profile is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when the student was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this student.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the student was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this student.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual Identity.AppUser? Creator { get; set; }
    public virtual ICollection<StudentGuardian> StudentGuardians { get; set; } = new List<StudentGuardian>();
    public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
    public virtual ICollection<Game.StudentProgress> StudentProgress { get; set; } = new List<Game.StudentProgress>();
    public virtual ICollection<Game.StudentBadge> StudentBadges { get; set; } = new List<Game.StudentBadge>();
}
