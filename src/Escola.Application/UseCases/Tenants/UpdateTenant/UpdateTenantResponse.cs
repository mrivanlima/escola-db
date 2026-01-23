namespace Escola.Application.UseCases.Tenants.UpdateTenant;

/// <summary>
/// Response after updating a tenant.
/// </summary>
public class UpdateTenantResponse
{
    /// <summary>
    /// The updated tenant's UUID.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Display name of the updated tenant.
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the tenant is active.
    /// </summary>
    public bool IsActive { get; set; }
}
