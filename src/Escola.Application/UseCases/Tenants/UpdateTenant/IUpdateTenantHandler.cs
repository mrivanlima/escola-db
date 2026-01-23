namespace Escola.Application.UseCases.Tenants.UpdateTenant;

/// <summary>
/// Handler interface for updating an existing tenant.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IUpdateTenantHandler
{
    Task<UpdateTenantResponse> Handle(UpdateTenantRequest request, CancellationToken cancellationToken = default);
}
