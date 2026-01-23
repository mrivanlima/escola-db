using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.UserRoles.GetUserRoles;

/// <summary>
/// Response containing all user roles.
/// </summary>
public class GetUserRolesResponse
{
    public List<UserRoleDto> UserRoles { get; set; } = new();
}
