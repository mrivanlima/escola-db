using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.TenantTypes.GetTenantTypes;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.TenantTypes;

/// <summary>
/// Implementation of Get TenantTypes handler in Infrastructure layer.
/// </summary>
public class GetTenantTypesHandler : IGetTenantTypesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<GetTenantTypesHandler> _logger;

    public GetTenantTypesHandler(EscolaDbContext context, ILogger<GetTenantTypesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetTenantTypesResponse> Handle(GetTenantTypesRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all tenant types");

        var tenantTypes = await _context.Set<Domain.Identity.TenantType>()
            .AsNoTracking()
            .OrderBy(tt => tt.TypeName)
            .Select(tt => new TenantTypeDto
            {
                TenantTypeUuid = tt.TenantTypeUuid,
                TypeName = tt.TypeName,
                Description = tt.Description,
                IsActive = tt.IsActive,
                CreatedAt = tt.CreatedAt.DateTime,
                UpdatedAt = tt.UpdatedAt.HasValue ? tt.UpdatedAt.Value.DateTime : null
            })
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} tenant types", tenantTypes.Count);

        return new GetTenantTypesResponse
        {
            TenantTypes = tenantTypes
        };
    }
}
