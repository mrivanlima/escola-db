using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.TenantTypes.GetTenantTypes;

/// <summary>
/// Response containing all tenant types.
/// </summary>
public class GetTenantTypesResponse
{
    public List<TenantTypeDto> TenantTypes { get; set; } = new();
}
