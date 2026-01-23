using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Guardians.GetGuardians;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Guardians;

public class GetGuardiansHandler : IGetGuardiansHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetGuardiansHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetGuardiansResponse> HandleAsync(
        GetGuardiansRequest request,
        CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.Set<Guardian>()
            .Include(g => g.Tenant)
            .Include(g => g.User)
            .Include(g => g.RelationshipType)
            .Where(g => g.TenantId == currentTenantId.Value && g.DeletedAt == null);

        if (request.TenantUuid.HasValue)
        {
            query = query.Where(g => g.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var guardians = await query
            .OrderBy(g => g.User.FullName)
            .ToListAsync(cancellationToken);

        return new GetGuardiansResponse
        {
            Guardians = guardians.Select(g => new GuardianDto
            {
                GuardianUuid = g.GuardianUuid,
                TenantUuid = g.Tenant.TenantUuid,
                TenantName = g.Tenant.TenantName,
                UserUuid = g.User.UserUuid,
                UserFullName = g.User.FullName,
                UserEmail = g.User.Email,
                PhoneNumber = g.PhoneNumber,
                RelationshipTypeUuid = g.RelationshipType.RelationshipTypeUuid,
                RelationshipTypeName = g.RelationshipType.Name,
                IsPrimary = g.IsPrimary,
                CreatedAt = g.CreatedAt.DateTime,
                UpdatedAt = g.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
