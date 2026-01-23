namespace Escola.Application.UseCases.UserRoles.GetUserRoles;

/// <summary>
/// Handler interface for retrieving all user roles.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetUserRolesHandler
{
    Task<GetUserRolesResponse> Handle(GetUserRolesRequest request, CancellationToken cancellationToken = default);
}
