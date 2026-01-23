using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.UserRoles.GetUserRole;

/// <summary>
/// Response containing a single user role's details.
/// </summary>
public class GetUserRoleResponse
{
    /// <summary>
    /// The user role details.
    /// </summary>
    public UserRoleDto? UserRole { get; set; }
}
