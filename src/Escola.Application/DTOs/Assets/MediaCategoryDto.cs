namespace Escola.Application.DTOs.Assets;

public class MediaCategoryDto
{
    public Guid CategoryUuid { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? DefaultIconName { get; set; }
    public int? DefaultMaxSizeMb { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
