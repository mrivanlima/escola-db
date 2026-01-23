using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class Specialization
{
    public int SpecializationId { get; set; }
    public Guid SpecializationUuid { get; set; }
    public string SpecializationName { get; set; } = string.Empty;
    public string SpecializationNameNormalized { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
