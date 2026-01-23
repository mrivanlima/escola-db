namespace Escola.Application.UseCases.Tenants.GetTenants;

/// <summary>
/// Response containing tenant data.
/// </summary>
public class GetTenantsResponse
{
    public List<TenantDto> Tenants { get; set; } = new();
}

/// <summary>
/// Tenant data transfer object.
/// </summary>
public class TenantDto
{
    public Guid TenantUuid { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string? TenantType { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
