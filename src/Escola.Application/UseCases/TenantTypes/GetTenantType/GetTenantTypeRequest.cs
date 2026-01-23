namespace Escola.Application.UseCases.TenantTypes.GetTenantType;

/// <summary>
/// Request for retrieving a single tenant type by UUID.
/// </summary>
public class GetTenantTypeRequest
{
    /// <summary>
    /// External UUID of the tenant type to retrieve.
    /// </summary>
    public Guid TenantTypeUuid { get; set; }
}
