namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for AppUser responses.
/// Only exposes UUIDs, never internal IDs.
/// </summary>
public class AppUserDto
{
    /// <summary>
    /// External user identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid UserUuid { get; set; }

    /// <summary>
    /// External tenant UUID.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name.
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// Supabase Auth user ID.
    /// </summary>
    public Guid AuthUserId { get; set; }

    /// <summary>
    /// User's full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// External user role UUID.
    /// </summary>
    public Guid UserRoleUuid { get; set; }

    /// <summary>
    /// User role name (Admin, Teacher, Guardian, Student).
    /// </summary>
    public string? UserRoleName { get; set; }

    /// <summary>
    /// Indicates if the user account is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// JSONB configuration object containing user preferences.
    /// </summary>
    public string? UserConfig { get; set; }

    /// <summary>
    /// Timestamp when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the user was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
