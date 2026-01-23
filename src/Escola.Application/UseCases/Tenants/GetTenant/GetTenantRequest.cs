namespace Escola.Application.UseCases.Tenants.GetTenant;

/// <summary>
/// Request for retrieving a single tenant by UUID.
/// </summary>
public class GetTenantRequest
{
    /// <summary>
    /// External UUID of the tenant to retrieve.
    /// </summary>
    public Guid TenantUuid { get; set; }
}
