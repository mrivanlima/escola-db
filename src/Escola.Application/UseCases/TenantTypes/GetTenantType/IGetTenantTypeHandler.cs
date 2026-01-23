namespace Escola.Application.UseCases.TenantTypes.GetTenantType;

/// <summary>
/// Handler interface for retrieving a single tenant type.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface IGetTenantTypeHandler
{
    Task<GetTenantTypeResponse> Handle(GetTenantTypeRequest request, CancellationToken cancellationToken = default);
}
