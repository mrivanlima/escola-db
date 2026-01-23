namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for updating an existing app user.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public class UpdateAppUserDto
{
    /// <summary>
    /// User's full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// User's email address.
    /// Must be unique if provided.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// External user role UUID.
    /// </summary>
    public Guid? UserRoleUuid { get; set; }

    /// <summary>
    /// Indicates if the user account is currently active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// JSONB configuration object containing user preferences.
    /// Must be valid JSON if provided.
    /// </summary>
    public string? UserConfig { get; set; }
}
