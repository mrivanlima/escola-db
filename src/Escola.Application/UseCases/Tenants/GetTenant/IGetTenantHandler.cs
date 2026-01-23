namespace Escola.Application.UseCases.Tenants.GetTenant;

/// <summary>
/// Handler interface for retrieving a single tenant.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetTenantHandler
{
    Task<GetTenantResponse> Handle(GetTenantRequest request, CancellationToken cancellationToken = default);
}
