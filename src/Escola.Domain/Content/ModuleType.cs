namespace Escola.Domain.Content;

/// <summary>
/// Lookup table for module/course types.
/// </summary>
public class ModuleType
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public short ModuleTypeId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ModuleTypeUuid { get; set; }

    /// <summary>
    /// Module type code (e.g., 'math', 'reading', 'science').
    /// </summary>
    public string ModuleTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// Normalized module type code for search.
    /// </summary>
    public string ModuleTypeCodeNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Mathematics", "Reading & Literacy").
    /// </summary>
    public string ModuleTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized module type name for search.
    /// </summary>
    public string ModuleTypeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Description of the module type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Icon identifier for frontend.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Hex color for UI theming.
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the module type is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this record.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this record.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
}
