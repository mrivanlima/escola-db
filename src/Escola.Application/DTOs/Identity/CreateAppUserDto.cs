namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for creating a new app user.
/// </summary>
public class CreateAppUserDto
{
    /// <summary>
    /// External tenant UUID.
    /// Required.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Supabase Auth user ID.
    /// Required. Must reference a valid auth.users.id in Supabase.
    /// </summary>
    public Guid AuthUserId { get; set; }

    /// <summary>
    /// User's full name.
    /// Required.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// Required. Must be unique.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// External user role UUID.
    /// Required.
    /// </summary>
    public Guid UserRoleUuid { get; set; }

    /// <summary>
    /// Indicates if the user should be active upon creation.
    /// Defaults to true.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Optional JSONB configuration object containing user preferences.
    /// Must be valid JSON if provided.
    /// </summary>
    public string? UserConfig { get; set; }
}
