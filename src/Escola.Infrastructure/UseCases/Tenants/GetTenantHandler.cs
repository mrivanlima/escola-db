using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.Tenants.GetTenant;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Tenants;

/// <summary>
/// Implementation of Get Tenant handler using EF Core.
/// </summary>
public class GetTenantHandler : IGetTenantHandler
{
    private readonly EscolaDbContext _context;

    public GetTenantHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetTenantResponse> Handle(GetTenantRequest request, CancellationToken cancellationToken = default)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .Include(t => t.TenantType)
            .Where(t => t.TenantUuid == request.TenantUuid)
            .Select(t => new TenantDto
            {
                TenantUuid = t.TenantUuid,
                TenantName = t.TenantName,
                TenantTypeUuid = t.TenantType != null ? t.TenantType.TenantTypeUuid : null,
                TenantTypeName = t.TenantType != null ? t.TenantType.TypeName : null,
                TenantConfig = t.TenantConfig,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt.DateTime,
                UpdatedAt = t.UpdatedAt.HasValue ? t.UpdatedAt.Value.DateTime : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetTenantResponse
        {
            Tenant = tenant
        };
    }
}
