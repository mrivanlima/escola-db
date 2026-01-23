namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for Tenant responses.
/// Only exposes the UUID, never the internal TenantId.
/// </summary>
public class TenantDto
{
    /// <summary>
    /// External tenant identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Display name of the tenant (e.g., school name, organization name).
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// External UUID of the tenant type (e.g., School, Organization, Individual).
    /// </summary>
    public Guid? TenantTypeUuid { get; set; }

    /// <summary>
    /// Display name of the tenant type.
    /// </summary>
    public string? TenantTypeName { get; set; }

    /// <summary>
    /// JSONB configuration object containing tenant-specific settings.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the tenant was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the tenant was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
