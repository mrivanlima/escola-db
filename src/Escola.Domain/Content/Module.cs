namespace Escola.Domain.Content;

/// <summary>
/// Represents a learning module/course.
/// </summary>
public class Module
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid ModuleUuid { get; set; }

    /// <summary>
    /// Module name.
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized module name for search (lowercase, no accents).
    /// </summary>
    public string ModuleNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Module description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Normalized description for search (lowercase, no accents).
    /// </summary>
    public string? DescriptionNormalized { get; set; }

    /// <summary>
    /// Module type ID (FK to module_types).
    /// </summary>
    public short ModuleTypeId { get; set; }

    /// <summary>
    /// Difficulty level (1-10).
    /// </summary>
    public int DifficultyLevel { get; set; }

    /// <summary>
    /// Recommended minimum age.
    /// </summary>
    public int? RecommendedAgeMin { get; set; }

    /// <summary>
    /// Recommended maximum age.
    /// </summary>
    public int? RecommendedAgeMax { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Thumbnail URL.
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// JSON configuration for module-specific settings.
    /// </summary>
    public string? ModuleConfig { get; set; }

    /// <summary>
    /// Indicates if the module is published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Timestamp when the module was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this module.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the module was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this module.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual ModuleType? ModuleType { get; set; }
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
