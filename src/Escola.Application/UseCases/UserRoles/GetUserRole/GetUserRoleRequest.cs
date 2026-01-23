namespace Escola.Application.UseCases.UserRoles.GetUserRole;

/// <summary>
/// Request for retrieving a single user role by UUID.
/// </summary>
public class GetUserRoleRequest
{
    /// <summary>
    /// External UUID of the user role to retrieve.
    /// </summary>
    public Guid UserRoleUuid { get; set; }
}
