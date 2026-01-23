using Escola.Domain.Identity;

namespace Escola.Domain.Assets;

/// <summary>
/// High-level media classification category (Image, Video, Audio, Document)
/// </summary>
public class MediaCategory
{
    public short CategoryId { get; set; }
    public Guid CategoryUuid { get; set; }
    
    // Business data
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultIconName { get; set; }
    public int? DefaultMaxSizeMb { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    
    // Normalized search columns
    public string CategoryCodeNormalized { get; set; } = string.Empty;
    public string CategoryNameNormalized { get; set; } = string.Empty;
    
    // Audit trail
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
    public ICollection<MimeType> MimeTypes { get; set; } = new List<MimeType>();
}
