namespace Escola.Application.UseCases.TenantTypes.GetTenantTypes;

/// <summary>
/// Handler interface for retrieving all tenant types.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetTenantTypesHandler
{
    Task<GetTenantTypesResponse> Handle(GetTenantTypesRequest request, CancellationToken cancellationToken = default);
}
