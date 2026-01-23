using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.AppUsers.UpdateAppUser;
using Escola.Domain.Identity;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.AppUsers;

/// <summary>
/// Handler for updating app users.
/// </summary>
public class UpdateAppUserHandler : IUpdateAppUserHandler
{
    private readonly EscolaDbContext _context;

    public UpdateAppUserHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateAppUserResponse> HandleAsync(
        UpdateAppUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.AppUser;

        var appUser = await _context.AppUsers
            .Include(u => u.Tenant)
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.UserUuid == request.UserUuid && u.DeletedAt == null, cancellationToken);

        if (appUser == null)
        {
            throw new InvalidOperationException("App user not found.");
        }

        // Update full name if provided
        if (!string.IsNullOrWhiteSpace(dto.FullName))
        {
            appUser.FullName = dto.FullName;
            appUser.FullNameNormalized = dto.FullName.ToLowerInvariant();
        }

        // Update email if provided
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var emailNormalized = dto.Email.ToLowerInvariant();

            // Check if new email is already in use by another user in this tenant
            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.TenantId == appUser.TenantId
                            && u.UserId != appUser.UserId
                            && u.EmailNormalized == emailNormalized
                            && u.DeletedAt == null, cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException("Email is already in use by another user in this tenant.");
            }

            appUser.Email = dto.Email;
            appUser.EmailNormalized = emailNormalized;
        }

        // Update user role if provided
        if (dto.UserRoleUuid.HasValue)
        {
            var userRole = await _context.Set<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserRoleUuid == dto.UserRoleUuid.Value && ur.DeletedAt == null, cancellationToken);

            if (userRole == null)
            {
                throw new InvalidOperationException("User role not found.");
            }

            appUser.UserRoleId = userRole.UserRoleId;
        }

        // Update IsActive if provided
        if (dto.IsActive.HasValue)
        {
            appUser.IsActive = dto.IsActive.Value;
        }

        // Update UserConfig if provided
        if (dto.UserConfig != null)
        {
            appUser.UserConfig = dto.UserConfig;
        }

        appUser.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Reload relationships in case UserRole was updated
        await _context.Entry(appUser)
            .Reference(u => u.UserRole)
            .LoadAsync(cancellationToken);

        return new UpdateAppUserResponse
        {
            AppUser = new AppUserDto
            {
                UserUuid = appUser.UserUuid,
                TenantUuid = appUser.Tenant.TenantUuid,
                TenantName = appUser.Tenant.TenantName,
                AuthUserId = appUser.AuthUserId,
                FullName = appUser.FullName,
                Email = appUser.Email,
                UserRoleUuid = appUser.UserRole.UserRoleUuid,
                UserRoleName = appUser.UserRole.RoleName,
                IsActive = appUser.IsActive,
                UserConfig = appUser.UserConfig,
                CreatedAt = appUser.CreatedAt.DateTime,
                UpdatedAt = appUser.UpdatedAt?.DateTime
            }
        };
    }
}
