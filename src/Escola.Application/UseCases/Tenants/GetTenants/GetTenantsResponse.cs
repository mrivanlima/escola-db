using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.Tenants.GetTenants;

/// <summary>
/// Response containing tenant data.
/// </summary>
public class GetTenantsResponse
{
    public List<TenantDto> Tenants { get; set; } = new();
}
