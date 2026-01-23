namespace Escola.Application.UseCases.Tenants.CreateTenant;

/// <summary>
/// Request for creating a new tenant.
/// </summary>
public class CreateTenantRequest
{
    /// <summary>
    /// Display name of the tenant (e.g., school name, organization name).
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// External UUID of the tenant type (e.g., School, Organization, Individual).
    /// </summary>
    public Guid? TenantTypeUuid { get; set; }

    /// <summary>
    /// JSONB configuration object containing tenant-specific settings.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant should be active upon creation.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
