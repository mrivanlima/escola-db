namespace Escola.Domain.Common;

/// <summary>
/// Base entity class with common audit fields and soft delete support.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Internal database ID (not exposed to API).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Timestamp when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this entity.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the entity was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this entity.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp. If set, the entity is considered deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Indicates whether this entity is soft deleted.
    /// </summary>
    public bool IsDeleted => DeletedAt.HasValue;
}
