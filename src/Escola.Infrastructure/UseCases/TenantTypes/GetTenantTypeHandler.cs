using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.TenantTypes.GetTenantType;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TenantTypes;

/// <summary>
/// Implementation of Get TenantType handler using EF Core.
/// </summary>
public class GetTenantTypeHandler : IGetTenantTypeHandler
{
    private readonly EscolaDbContext _context;

    public GetTenantTypeHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetTenantTypeResponse> Handle(GetTenantTypeRequest request, CancellationToken cancellationToken = default)
    {
        var tenantType = await _context.Set<Domain.Identity.TenantType>()
            .AsNoTracking()
            .Where(tt => tt.TenantTypeUuid == request.TenantTypeUuid)
            .Select(tt => new TenantTypeDto
            {
                TenantTypeUuid = tt.TenantTypeUuid,
                TypeName = tt.TypeName,
                Description = tt.Description,
                IsActive = tt.IsActive,
                CreatedAt = tt.CreatedAt.DateTime,
                UpdatedAt = tt.UpdatedAt.HasValue ? tt.UpdatedAt.Value.DateTime : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetTenantTypeResponse
        {
            TenantType = tenantType
        };
    }
}
