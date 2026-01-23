using Escola.Application.UseCases.Tenants.UpdateTenant;
using Escola.Domain.Identity;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Tenants;

/// <summary>
/// Implementation of Update Tenant handler in Infrastructure layer.
/// </summary>
public class UpdateTenantHandler : IUpdateTenantHandler
{
    private readonly EscolaDbContext _context;

    public UpdateTenantHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateTenantResponse> Handle(UpdateTenantRequest request, CancellationToken cancellationToken = default)
    {
        // Find the tenant by UUID
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.TenantUuid == request.TenantUuid, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException($"Tenant with UUID {request.TenantUuid} not found");
        }

        // Update TenantName if provided
        if (!string.IsNullOrWhiteSpace(request.TenantName))
        {
            var normalizedName = request.TenantName.ToLowerInvariant();

            // Check if another tenant with same normalized name exists
            var existingTenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.TenantNameNormalized == normalizedName && t.TenantId != tenant.TenantId, cancellationToken);

            if (existingTenant != null)
            {
                throw new InvalidOperationException($"A tenant with name '{request.TenantName}' already exists");
            }

            tenant.TenantName = request.TenantName;
            tenant.TenantNameNormalized = normalizedName;
        }

        // Update TenantTypeId if TenantTypeUuid is provided
        if (request.TenantTypeUuid.HasValue)
        {
            var tenantType = await _context.Set<TenantType>()
                .FirstOrDefaultAsync(tt => tt.TenantTypeUuid == request.TenantTypeUuid.Value, cancellationToken);

            if (tenantType == null)
            {
                throw new InvalidOperationException($"Tenant type with UUID {request.TenantTypeUuid} does not exist");
            }

            tenant.TenantTypeId = tenantType.TenantTypeId;
        }

        // Update TenantConfig if provided
        if (request.TenantConfig != null)
        {
            tenant.TenantConfig = request.TenantConfig;
        }

        // Update IsActive if provided
        if (request.IsActive.HasValue)
        {
            tenant.IsActive = request.IsActive.Value;
        }

        // Update audit fields
        tenant.UpdatedAt = DateTimeOffset.UtcNow;
        // UpdatedBy is nullable - will be set when authentication is implemented

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTenantResponse
        {
            TenantUuid = tenant.TenantUuid,
            TenantName = tenant.TenantName,
            IsActive = tenant.IsActive
        };
    }
}
