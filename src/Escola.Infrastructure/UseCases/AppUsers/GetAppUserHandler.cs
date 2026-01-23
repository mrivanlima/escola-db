using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.AppUsers.GetAppUser;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.AppUsers;

/// <summary>
/// Handler for getting a single app user by UUID.
/// </summary>
public class GetAppUserHandler : IGetAppUserHandler
{
    private readonly EscolaDbContext _context;

    public GetAppUserHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetAppUserResponse> HandleAsync(
        GetAppUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var appUser = await _context.AppUsers
            .Include(u => u.Tenant)
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.UserUuid == request.UserUuid && u.DeletedAt == null, cancellationToken);

        if (appUser == null)
        {
            throw new InvalidOperationException("App user not found.");
        }

        return new GetAppUserResponse
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
