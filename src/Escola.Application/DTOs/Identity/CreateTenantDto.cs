namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for creating a new tenant.
/// </summary>
public class CreateTenantDto
{
    /// <summary>
    /// Display name of the tenant (e.g., school name, organization name).
    /// Required. Must be unique within the system.
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// External UUID of the tenant type (e.g., School, Organization, Individual).
    /// Optional. If not provided, will use the default tenant type.
    /// </summary>
    public Guid? TenantTypeUuid { get; set; }

    /// <summary>
    /// JSONB configuration object containing tenant-specific settings.
    /// Optional. Must be valid JSON if provided.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant should be active upon creation.
    /// Defaults to true.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
