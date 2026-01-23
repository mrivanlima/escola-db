using Escola.Domain.Identity;

namespace Escola.Domain.Assets;

/// <summary>
/// MIME type classification for media files (image/jpeg, video/mp4, etc.)
/// </summary>
public class MimeType
{
    public short MimeTypeId { get; set; }
    public Guid MimeTypeUuid { get; set; }
    
    // Business data
    public string MimeTypeCode { get; set; } = string.Empty;
    public string MimeTypeName { get; set; } = string.Empty;
    public short CategoryId { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public int? MaxFileSizeMb { get; set; }
    public bool IsActive { get; set; }
    
    // Normalized search column
    public string MimeTypeCodeNormalized { get; set; } = string.Empty;
    
    // Audit trail
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public MediaCategory Category { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
    public ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
}
