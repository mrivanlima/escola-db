using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.RelationshipTypes.GetRelationshipType;

public class GetRelationshipTypeRequest
{
    public Guid RelationshipTypeUuid { get; set; }
}

public class GetRelationshipTypeResponse
{
    public RelationshipTypeDto RelationshipType { get; set; } = null!;
}

public interface IGetRelationshipTypeHandler
{
    Task<GetRelationshipTypeResponse> HandleAsync(
        GetRelationshipTypeRequest request,
        CancellationToken cancellationToken = default);
}
