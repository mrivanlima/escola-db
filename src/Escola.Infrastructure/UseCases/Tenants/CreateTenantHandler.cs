using Escola.Application.UseCases.Tenants.CreateTenant;
using Escola.Domain.Identity;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Tenants;

/// <summary>
/// Implementation of Create Tenant handler in Infrastructure layer.
/// </summary>
public class CreateTenantHandler : ICreateTenantHandler
{
    private readonly EscolaDbContext _context;

    public CreateTenantHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateTenantResponse> Handle(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        // Resolve TenantTypeId if TenantTypeUuid is provided
        short? tenantTypeId = null;
        if (request.TenantTypeUuid.HasValue)
        {
            var tenantType = await _context.Set<TenantType>()
                .FirstOrDefaultAsync(tt => tt.TenantTypeUuid == request.TenantTypeUuid.Value, cancellationToken);

            if (tenantType == null)
            {
                throw new InvalidOperationException($"Tenant type with UUID {request.TenantTypeUuid} does not exist");
            }

            tenantTypeId = tenantType.TenantTypeId;
        }

        // Normalize tenant name for uniqueness check
        var normalizedName = request.TenantName.ToLowerInvariant();

        // Check if tenant with same normalized name already exists
        var existingTenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.TenantNameNormalized == normalizedName, cancellationToken);

        if (existingTenant != null)
        {
            throw new InvalidOperationException($"A tenant with name '{request.TenantName}' already exists");
        }

        // Create the tenant entity
        var tenant = new Tenant
        {
            TenantUuid = Guid.NewGuid(),
            TenantName = request.TenantName,
            TenantNameNormalized = normalizedName,
            TenantTypeId = tenantTypeId,
            TenantConfig = request.TenantConfig,
            IsActive = request.IsActive,
            CreatedAt = DateTimeOffset.UtcNow
            // CreatedBy is nullable - will be set when authentication is implemented
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateTenantResponse
        {
            TenantUuid = tenant.TenantUuid,
            TenantName = tenant.TenantName,
            IsActive = tenant.IsActive
        };
    }
}
