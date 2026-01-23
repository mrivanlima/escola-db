namespace Escola.Domain.Identity;

/// <summary>
/// Represents an application user (parent, teacher, admin).
/// </summary>
public class AppUser
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid UserUuid { get; set; }

    /// <summary>
    /// Tenant ID this user belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Authentication user ID (external auth system).
    /// </summary>
    public Guid AuthUserId { get; set; }

    /// <summary>
    /// Full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized full name for search (lowercase, no accents).
    /// </summary>
    public string FullNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Normalized email for search (lowercase).
    /// </summary>
    public string EmailNormalized { get; set; } = string.Empty;

    /// <summary>
    /// User role (e.g., "parent", "teacher", "admin").
    /// </summary>
    public string UserRole { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the user account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// JSON configuration for user-specific settings.
    /// </summary>
    public string? UserConfig { get; set; }

    /// <summary>
    /// Timestamp when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this user.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the user was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this user.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ICollection<School.Guardian> Guardians { get; set; } = new List<School.Guardian>();
    public virtual ICollection<School.Teacher> Teachers { get; set; } = new List<School.Teacher>();
}
