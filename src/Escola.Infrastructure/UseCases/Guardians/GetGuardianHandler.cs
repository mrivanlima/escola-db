using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Guardians.GetGuardian;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Guardians;

public class GetGuardianHandler : IGetGuardianHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetGuardianHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetGuardianResponse> HandleAsync(
        GetGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var guardian = await _context.Set<Guardian>()
            .Include(g => g.Tenant)
            .Include(g => g.User)
            .Include(g => g.RelationshipType)
            .Include(g => g.CreatedByUser)
            .Include(g => g.UpdatedByUser)
            .FirstOrDefaultAsync(g => g.GuardianUuid == request.GuardianUuid && g.TenantId == currentTenantId.Value && g.DeletedAt == null, cancellationToken);

        if (guardian == null)
        {
            throw new InvalidOperationException("Guardian not found or access denied.");
        }

        return new GetGuardianResponse
        {
            Guardian = new GuardianDto
            {
                GuardianUuid = guardian.GuardianUuid,
                TenantUuid = guardian.Tenant.TenantUuid,
                TenantName = guardian.Tenant.TenantName,
                UserUuid = guardian.User.UserUuid,
                UserFullName = guardian.User.FullName,
                UserEmail = guardian.User.Email,
                PhoneNumber = guardian.PhoneNumber,
                RelationshipTypeUuid = guardian.RelationshipType.RelationshipTypeUuid,
                RelationshipTypeName = guardian.RelationshipType.Name,
                IsPrimary = guardian.IsPrimary,
                CreatedAt = guardian.CreatedAt.DateTime,
                CreatedByUuid = guardian.CreatedByUser?.UserUuid,
                UpdatedAt = guardian.UpdatedAt?.DateTime,
                UpdatedByUuid = guardian.UpdatedByUser?.UserUuid
            }
        };
    }
}
