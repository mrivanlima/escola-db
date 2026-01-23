namespace Escola.Application.UseCases.UserRoles.GetUserRole;

/// <summary>
/// Handler interface for retrieving a single user role.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetUserRoleHandler
{
    Task<GetUserRoleResponse> Handle(GetUserRoleRequest request, CancellationToken cancellationToken = default);
}
