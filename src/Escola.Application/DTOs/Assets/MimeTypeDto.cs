namespace Escola.Application.DTOs.Assets;

public class MimeTypeDto
{
    public Guid MimeTypeUuid { get; set; }
    public string MimeTypeCode { get; set; } = string.Empty;
    public string MimeTypeName { get; set; } = string.Empty;
    public Guid CategoryUuid { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? FileExtension { get; set; }
    public string? IconName { get; set; }
    public int? MaxFileSizeMb { get; set; }
    public bool IsActive { get; set; }
}
