using Escola.Domain.Identity;

namespace Escola.Domain.Assets;

/// <summary>
/// Central registry for all uploaded media files (metadata only, not binary)
/// </summary>
public class MediaFile
{
    public int FileId { get; set; }
    public Guid FileUuid { get; set; }
    public int TenantId { get; set; }
    
    // Storage and file info
    public string StoragePath { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public short MimeTypeId { get; set; }
    public long? SizeBytes { get; set; }
    
    // Accessibility and metadata
    public string? AltText { get; set; }
    public string? Metadata { get; set; } // JSONB in database
    
    // Normalized search columns
    public string OriginalNameNormalized { get; set; } = string.Empty;
    public string AltTextNormalized { get; set; } = string.Empty;
    
    // Audit trail
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public MimeType MimeType { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
    
    // Collections
    public ICollection<Content.ActivityResource> ActivityResources { get; set; } = new List<Content.ActivityResource>();
}
