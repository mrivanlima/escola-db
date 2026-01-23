using Escola.Application.DTOs.School;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipType;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.RelationshipTypes;

public class GetRelationshipTypeHandler : IGetRelationshipTypeHandler
{
    private readonly EscolaDbContext _context;

    public GetRelationshipTypeHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetRelationshipTypeResponse> HandleAsync(
        GetRelationshipTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var relationshipType = await _context.Set<RelationshipType>()
            .Include(rt => rt.Tenant)
            .FirstOrDefaultAsync(rt => rt.RelationshipTypeUuid == request.RelationshipTypeUuid, cancellationToken);

        if (relationshipType == null)
        {
            throw new InvalidOperationException("Relationship type not found.");
        }

        return new GetRelationshipTypeResponse
        {
            RelationshipType = new RelationshipTypeDto
            {
                RelationshipTypeUuid = relationshipType.RelationshipTypeUuid,
                TenantUuid = relationshipType.Tenant.TenantUuid,
                TenantName = relationshipType.Tenant.TenantName,
                Name = relationshipType.Name,
                DisplayOrder = relationshipType.DisplayOrder,
                IsActive = relationshipType.IsActive
            }
        };
    }
}
