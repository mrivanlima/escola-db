using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.Tenants.GetTenant;

/// <summary>
/// Response containing a single tenant's details.
/// </summary>
public class GetTenantResponse
{
    /// <summary>
    /// The tenant details.
    /// </summary>
    public TenantDto? Tenant { get; set; }
}
