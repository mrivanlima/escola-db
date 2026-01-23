namespace Escola.Application.UseCases.Tenants.GetTenants;

/// <summary>
/// Handler interface for retrieving all tenants.
/// </summary>
public interface IGetTenantsHandler
{
    /// <summary>
    /// Retrieves all tenants from the system.
    /// </summary>
    /// <param name="request">The request containing query parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response containing list of tenants.</returns>
    Task<GetTenantsResponse> Handle(GetTenantsRequest request, CancellationToken cancellationToken);
}
