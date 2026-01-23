namespace Escola.Domain.Identity;

/// <summary>
/// Represents an application user in the system.
/// Core user entity with Supabase Auth integration.
/// </summary>
public class AppUser
{
    /// <summary>
    /// Internal primary key. Never exposed to API.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// External UUID for API exposure. This is the only ID exposed to clients.
    /// </summary>
    public Guid UserUuid { get; set; }

    /// <summary>
    /// Tenant (school/organization) this user belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Supabase Auth user ID. References auth.users.id in Supabase.
    /// </summary>
    public Guid AuthUserId { get; set; }

    /// <summary>
    /// User's full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of full name (lowercase, unaccented) for searching.
    /// </summary>
    public string FullNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of email (lowercase) for uniqueness checks.
    /// </summary>
    public string EmailNormalized { get; set; } = string.Empty;

    /// <summary>
    /// User role (Admin, Teacher, Guardian, Student).
    /// </summary>
    public short UserRoleId { get; set; }

    /// <summary>
    /// Indicates if the user account is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// JSONB configuration object containing user-specific preferences.
    /// </summary>
    public string? UserConfig { get; set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual UserRole UserRole { get; set; } = null!;
    public virtual AppUser? Creator { get; set; }
    public virtual AppUser? Updater { get; set; }
}
