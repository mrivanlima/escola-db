namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for updating an existing Guardian
/// All fields are optional - only provided fields will be updated
/// </summary>
public class UpdateGuardianDto
{
    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Relationship type UUID
    /// </summary>
    public Guid? RelationshipTypeUuid { get; set; }

    /// <summary>
    /// Indicates if this is the primary guardian
    /// </summary>
    public bool? IsPrimary { get; set; }
}
