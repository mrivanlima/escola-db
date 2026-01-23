namespace Escola.Application.UseCases.Tenants.CreateTenant;

/// <summary>
/// Response after creating a tenant.
/// </summary>
public class CreateTenantResponse
{
    /// <summary>
    /// The newly created tenant's UUID.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Display name of the created tenant.
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the tenant is active.
    /// </summary>
    public bool IsActive { get; set; }
}
