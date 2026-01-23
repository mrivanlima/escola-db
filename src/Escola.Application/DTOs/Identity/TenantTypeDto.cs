namespace Escola.Application.DTOs.Identity;

/// <summary>
/// Data Transfer Object for TenantType responses.
/// Only exposes the UUID, never the internal TenantTypeId.
/// </summary>
public class TenantTypeDto
{
    /// <summary>
    /// External tenant type identifier (UUID). This is the only ID exposed to clients.
    /// </summary>
    public Guid TenantTypeUuid { get; set; }

    /// <summary>
    /// Display name of the tenant type (e.g., "School", "Organization", "Individual").
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of this tenant type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this tenant type is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the tenant type was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the tenant type was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
