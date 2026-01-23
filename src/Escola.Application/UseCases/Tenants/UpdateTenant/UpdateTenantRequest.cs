namespace Escola.Application.UseCases.Tenants.UpdateTenant;

/// <summary>
/// Request for updating an existing tenant.
/// </summary>
public class UpdateTenantRequest
{
    /// <summary>
    /// External UUID of the tenant to update.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Display name of the tenant.
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// External UUID of the tenant type.
    /// </summary>
    public Guid? TenantTypeUuid { get; set; }

    /// <summary>
    /// JSONB configuration object.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant is active.
    /// </summary>
    public bool? IsActive { get; set; }
}
