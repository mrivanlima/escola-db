using Escola.Domain.Identity;

namespace Escola.Domain.School;

/// <summary>
/// Represents a class/group in the school.
/// </summary>
public class Class
{
    /// <summary>
    /// Internal database ID. Maps to: class_id
    /// </summary>
    public int ClassId { get; set; }

    /// <summary>
    /// External UUID for API exposure. Maps to: class_uuid
    /// </summary>
    public Guid ClassUuid { get; set; }

    /// <summary>
    /// Tenant ID. Maps to: tenant_id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Class name. Maps to: class_name
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized class name. Maps to: class_name_normalized
    /// </summary>
    public string ClassNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Grade level ID. Maps to: grade_level_id
    /// </summary>
    public short? GradeLevelId { get; set; }

    /// <summary>
    /// School year ID. Maps to: school_year_id
    /// </summary>
    public short SchoolYearId { get; set; }

    /// <summary>
    /// Maximum students. Maps to: max_students
    /// </summary>
    public int? MaxStudents { get; set; }

    /// <summary>
    /// Class config. Maps to: class_config
    /// </summary>
    public string? ClassConfig { get; set; }

    /// <summary>
    /// Is active. Maps to: is_active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Created at. Maps to: created_at
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Created by. Maps to: created_by
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Updated at. Maps to: updated_at
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Updated by. Maps to: updated_by
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete. Maps to: deleted_at
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public GradeLevel? GradeLevel { get; set; }
    public SchoolYear SchoolYear { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
    public ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
}
