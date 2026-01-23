using Escola.Application.DTOs.School;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipTypes;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.RelationshipTypes;

public class GetRelationshipTypesHandler : IGetRelationshipTypesHandler
{
    private readonly EscolaDbContext _context;

    public GetRelationshipTypesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetRelationshipTypesResponse> HandleAsync(
        GetRelationshipTypesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<RelationshipType>()
            .Include(rt => rt.Tenant)
            .AsQueryable();

        if (request.TenantUuid.HasValue)
        {
            query = query.Where(rt => rt.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var relationshipTypes = await query
            .OrderBy(rt => rt.DisplayOrder)
            .ThenBy(rt => rt.Name)
            .ToListAsync(cancellationToken);

        return new GetRelationshipTypesResponse
        {
            RelationshipTypes = relationshipTypes.Select(rt => new RelationshipTypeDto
            {
                RelationshipTypeUuid = rt.RelationshipTypeUuid,
                TenantUuid = rt.Tenant.TenantUuid,
                TenantName = rt.Tenant.TenantName,
                Name = rt.Name,
                DisplayOrder = rt.DisplayOrder,
                IsActive = rt.IsActive
            }).ToList()
        };
    }
}
