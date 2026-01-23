using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Guardians.CreateGuardian;
using Escola.Domain.Identity;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Guardians;

public class CreateGuardianHandler : ICreateGuardianHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateGuardianHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateGuardianResponse> HandleAsync(
        CreateGuardianRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.Guardian;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // Validate tenant exists and matches current user's tenant
        var tenant = await _context.Set<Tenant>()
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException("Tenant not found or access denied.");
        }

        // Validate user exists
        var user = await _context.Set<AppUser>()
            .FirstOrDefaultAsync(u => u.UserUuid == dto.UserUuid && u.DeletedAt == null, cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        // Validate relationship type exists
        var relationshipType = await _context.Set<RelationshipType>()
            .FirstOrDefaultAsync(rt => rt.RelationshipTypeUuid == dto.RelationshipTypeUuid, cancellationToken);

        if (relationshipType == null)
        {
            throw new InvalidOperationException("Relationship type not found.");
        }

        // Check if user is already a guardian in this tenant
        var existingGuardian = await _context.Set<Guardian>()
            .FirstOrDefaultAsync(g => g.UserId == user.UserId && g.TenantId == currentTenantId.Value && g.DeletedAt == null, cancellationToken);

        if (existingGuardian != null)
        {
            throw new InvalidOperationException("This user is already registered as a guardian in this tenant.");
        }

        var guardian = new Guardian
        {
            GuardianUuid = Guid.NewGuid(),
            TenantId = currentTenantId.Value,
            UserId = user.UserId,
            PhoneNumber = dto.PhoneNumber,
            RelationshipTypeId = relationshipType.RelationshipTypeId,
            IsPrimary = dto.IsPrimary,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.Set<Guardian>().Add(guardian);
        await _context.SaveChangesAsync(cancellationToken);

        // Load navigation properties
        await _context.Entry(guardian)
            .Reference(g => g.Tenant)
            .LoadAsync(cancellationToken);
        await _context.Entry(guardian)
            .Reference(g => g.User)
            .LoadAsync(cancellationToken);
        await _context.Entry(guardian)
            .Reference(g => g.RelationshipType)
            .LoadAsync(cancellationToken);

        return new CreateGuardianResponse
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
