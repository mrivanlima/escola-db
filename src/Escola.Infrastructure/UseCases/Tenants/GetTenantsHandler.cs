using Escola.Application.UseCases.Tenants.GetTenants;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Tenants;

/// <summary>
/// Implementation of Get Tenants handler in Infrastructure layer.
/// </summary>
public class GetTenantsHandler : IGetTenantsHandler
{
    private readonly EscolaDbContext _context;

    public GetTenantsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetTenantsResponse> Handle(GetTenantsRequest request, CancellationToken cancellationToken = default)
    {
        var tenants = await _context.Tenants
            .OrderBy(t => t.TenantName)
            .ToListAsync(cancellationToken);

        var response = new GetTenantsResponse
        {
            Tenants = tenants.Select(t => new TenantDto
            {
                TenantId = t.TenantId,
                TenantUuid = t.TenantUuid,
                TenantName = t.TenantName,
                TenantType = t.TenantType,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList()
        };

        return response;
    }
}
