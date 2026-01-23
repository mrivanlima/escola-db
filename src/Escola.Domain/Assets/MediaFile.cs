namespace Escola.Domain.Assets;

/// <summary>
/// Represents a media file in the central media library.
/// </summary>
public class MediaFile
{
    /// <summary>
    /// File ID (UUID, serves as primary key).
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Tenant ID this media file belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Storage path/URL to the file.
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>
    /// Original filename.
    /// </summary>
    public string OriginalName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized original filename for search (lowercase, no accents).
    /// </summary>
    public string OriginalNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// MIME type (e.g., "image/png", "audio/mp3").
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Alt text for accessibility.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Normalized alt text for search (lowercase, no accents).
    /// </summary>
    public string? AltTextNormalized { get; set; }

    /// <summary>
    /// JSON metadata (dimensions, duration, etc.).
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Timestamp when the file was uploaded.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who uploaded this file.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the file metadata was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this file.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual ICollection<Content.ActivityResource> ActivityResources { get; set; } = new List<Content.ActivityResource>();
}
