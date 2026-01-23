namespace Escola.Application.UseCases.Tenants.GetTenants;

/// <summary>
/// Handler interface for getting all tenants from the database.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetTenantsHandler
{
    Task<GetTenantsResponse> Handle(GetTenantsRequest request, CancellationToken cancellationToken = default);
}
