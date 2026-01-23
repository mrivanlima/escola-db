namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for updating an existing tenant.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public class UpdateTenantDto
{
    /// <summary>
    /// Display name of the tenant (e.g., school name, organization name).
    /// If provided, must be unique within the system.
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// External UUID of the tenant type (e.g., School, Organization, Individual).
    /// Set to null to remove the tenant type association.
    /// </summary>
    public Guid? TenantTypeUuid { get; set; }

    /// <summary>
    /// JSONB configuration object containing tenant-specific settings.
    /// Must be valid JSON if provided.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant is currently active.
    /// </summary>
    public bool? IsActive { get; set; }
}
