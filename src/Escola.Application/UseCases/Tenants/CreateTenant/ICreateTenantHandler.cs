namespace Escola.Application.UseCases.Tenants.CreateTenant;

/// <summary>
/// Handler interface for creating a new tenant.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface ICreateTenantHandler
{
    Task<CreateTenantResponse> Handle(CreateTenantRequest request, CancellationToken cancellationToken = default);
}
