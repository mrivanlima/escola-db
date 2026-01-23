using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Guardians.UpdateGuardian;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Guardians;

public class UpdateGuardianHandler : IUpdateGuardianHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGuardianHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateGuardianResponse> HandleAsync(
        UpdateGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.Guardian;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var guardian = await _context.Set<Guardian>()
            .Include(g => g.Tenant)
            .Include(g => g.User)
            .Include(g => g.RelationshipType)
            .FirstOrDefaultAsync(g => g.GuardianUuid == request.GuardianUuid && g.TenantId == currentTenantId.Value && g.DeletedAt == null, cancellationToken);

        if (guardian == null)
        {
            throw new InvalidOperationException("Guardian not found or access denied.");
        }

        // Update phone number if provided
        if (dto.PhoneNumber != null)
        {
            guardian.PhoneNumber = dto.PhoneNumber;
        }

        // Update relationship type if provided
        if (dto.RelationshipTypeUuid.HasValue)
        {
            var relationshipType = await _context.Set<RelationshipType>()
                .FirstOrDefaultAsync(rt => rt.RelationshipTypeUuid == dto.RelationshipTypeUuid.Value, cancellationToken);

            if (relationshipType == null)
            {
                throw new InvalidOperationException("Relationship type not found.");
            }

            guardian.RelationshipTypeId = relationshipType.RelationshipTypeId;

            // Reload the relationship type navigation property
            await _context.Entry(guardian)
                .Reference(g => g.RelationshipType)
                .LoadAsync(cancellationToken);
        }

        // Update is primary if provided
        if (dto.IsPrimary.HasValue)
        {
            guardian.IsPrimary = dto.IsPrimary.Value;
        }

        guardian.UpdatedAt = DateTimeOffset.UtcNow;
        guardian.UpdatedBy = _currentUserService.GetCurrentUserId();

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateGuardianResponse
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
                UpdatedAt = guardian.UpdatedAt?.DateTime
            }
        };
    }
}
