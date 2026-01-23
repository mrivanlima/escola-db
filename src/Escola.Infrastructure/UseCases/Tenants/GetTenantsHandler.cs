using Escola.Application.DTOs.Identity;
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
            .Include(t => t.TenantType)
            .OrderBy(t => t.TenantName)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} tenants", tenants.Count);

        var response = new GetTenantsResponse
        {
            Tenants = tenants.Select(t => new TenantDto
            {
                TenantUuid = t.TenantUuid,
                TenantName = t.TenantName,
                TenantTypeUuid = t.TenantType != null ? t.TenantType.TenantTypeUuid : null,
                TenantTypeName = t.TenantType != null ? t.TenantType.TypeName : null,
                TenantConfig = t.TenantConfig,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt.DateTime,
                UpdatedAt = t.UpdatedAt.HasValue ? t.UpdatedAt.Value.DateTime : null
            }).ToList()
        };

        return response;
    }
}
