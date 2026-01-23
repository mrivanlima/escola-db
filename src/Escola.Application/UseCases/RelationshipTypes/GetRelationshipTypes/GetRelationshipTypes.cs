using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.RelationshipTypes.GetRelationshipTypes;

public class GetRelationshipTypesRequest
{
    public Guid? TenantUuid { get; set; }
}

public class GetRelationshipTypesResponse
{
    public List<RelationshipTypeDto> RelationshipTypes { get; set; } = new();
}

public interface IGetRelationshipTypesHandler
{
    Task<GetRelationshipTypesResponse> HandleAsync(
        GetRelationshipTypesRequest request,
        CancellationToken cancellationToken = default);
}
