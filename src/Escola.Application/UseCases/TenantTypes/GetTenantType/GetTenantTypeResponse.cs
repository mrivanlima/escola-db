using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.TenantTypes.GetTenantType;

/// <summary>
/// Response containing a single tenant type's details.
/// </summary>
public class GetTenantTypeResponse
{
    /// <summary>
    /// The tenant type details.
    /// </summary>
    public TenantTypeDto? TenantType { get; set; }
}
