namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for UserRole responses.
/// Only exposes the UUID, never the internal UserRoleId.
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// External user role identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid UserRoleUuid { get; set; }

    /// <summary>
    /// Display name of the user role (e.g., "Admin", "Teacher", "Guardian", "Student").
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of this user role.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this user role is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the user role was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the user role was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
