namespace Escola.Domain.Identity;

/// <summary>
/// Represents a user role in the system (e.g., Admin, Teacher, Guardian, Student).
/// Lookup table for user role classification.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Internal primary key. Never exposed to API.
    /// </summary>
    public short UserRoleId { get; set; }

    /// <summary>
    /// External UUID for API exposure. This is the only ID exposed to clients.
    /// </summary>
    public Guid UserRoleUuid { get; set; }

    /// <summary>
    /// Display name of the user role (e.g., "Admin", "Teacher", "Guardian", "Student").
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of RoleName (lowercase, unaccented) for searching.
    /// </summary>
    public string RoleNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of this user role.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this user role is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual AppUser? Creator { get; set; }
    public virtual AppUser? Updater { get; set; }
}
