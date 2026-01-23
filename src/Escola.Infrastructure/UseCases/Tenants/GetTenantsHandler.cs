using Escola.Application.UseCases.Tenants.GetTenants;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.Tenants;

/// <summary>
/// Implementation of Get Tenants handler in Infrastructure layer.
/// </summary>
public class GetTenantsHandler : IGetTenantsHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<GetTenantsHandler> _logger;

    public GetTenantsHandler(EscolaDbContext context, ILogger<GetTenantsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetTenantsResponse> Handle(GetTenantsRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all tenants");

        var tenants = await _context.Tenants
            .AsNoTracking()
            .OrderBy(t => t.TenantName)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} tenants", tenants.Count);

        var response = new GetTenantsResponse
        {
            Tenants = tenants.Select(t => new TenantDto
            {
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
