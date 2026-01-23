using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.AppUsers.CreateAppUser;
using Escola.Domain.Identity;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.AppUsers;

/// <summary>
/// Handler for creating app users.
/// </summary>
public class CreateAppUserHandler : ICreateAppUserHandler
{
    private readonly EscolaDbContext _context;

    public CreateAppUserHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateAppUserResponse> HandleAsync(
        CreateAppUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.AppUser;

        // Validate tenant exists
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.DeletedAt == null, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException("Tenant not found.");
        }

        // Validate user role exists
        var userRole = await _context.Set<UserRole>()
            .FirstOrDefaultAsync(ur => ur.UserRoleUuid == dto.UserRoleUuid && ur.DeletedAt == null, cancellationToken);

        if (userRole == null)
        {
            throw new InvalidOperationException("User role not found.");
        }

        // Check if email is already in use in this tenant
        var emailNormalized = dto.Email.ToLowerInvariant();
        var emailExists = await _context.AppUsers
            .AnyAsync(u => u.TenantId == tenant.TenantId
                        && u.EmailNormalized == emailNormalized
                        && u.DeletedAt == null, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use in this tenant.");
        }

        // Check if auth user ID is already in use
        var authUserExists = await _context.AppUsers
            .AnyAsync(u => u.AuthUserId == dto.AuthUserId && u.DeletedAt == null, cancellationToken);

        if (authUserExists)
        {
            throw new InvalidOperationException("Auth User ID is already in use.");
        }

        // Create new app user
        var appUser = new AppUser
        {
            UserUuid = Guid.NewGuid(),
            TenantId = tenant.TenantId,
            AuthUserId = dto.AuthUserId,
            FullName = dto.FullName,
            FullNameNormalized = dto.FullName.ToLowerInvariant(),
            Email = dto.Email,
            EmailNormalized = emailNormalized,
            UserRoleId = userRole.UserRoleId,
            IsActive = dto.IsActive,
            UserConfig = dto.UserConfig,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.AppUsers.Add(appUser);
        await _context.SaveChangesAsync(cancellationToken);

        // Load relationships for response
        await _context.Entry(appUser)
            .Reference(u => u.Tenant)
            .LoadAsync(cancellationToken);

        await _context.Entry(appUser)
            .Reference(u => u.UserRole)
            .LoadAsync(cancellationToken);

        return new CreateAppUserResponse
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
