using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.AppUsers.GetAppUsers;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.AppUsers;

/// <summary>
/// Handler for getting all app users.
/// </summary>
public class GetAppUsersHandler : IGetAppUsersHandler
{
    private readonly EscolaDbContext _context;

    public GetAppUsersHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetAppUsersResponse> HandleAsync(
        GetAppUsersRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AppUsers
            .Include(u => u.Tenant)
            .Include(u => u.UserRole)
            .Where(u => u.DeletedAt == null);

        // Filter by tenant if provided
        if (request.TenantUuid.HasValue)
        {
            query = query.Where(u => u.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var appUsers = await query
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);

        return new GetAppUsersResponse
        {
            AppUsers = appUsers.Select(u => new AppUserDto
            {
                UserUuid = u.UserUuid,
                TenantUuid = u.Tenant.TenantUuid,
                TenantName = u.Tenant.TenantName,
                AuthUserId = u.AuthUserId,
                FullName = u.FullName,
                Email = u.Email,
                UserRoleUuid = u.UserRole.UserRoleUuid,
                UserRoleName = u.UserRole.RoleName,
                IsActive = u.IsActive,
                UserConfig = u.UserConfig,
                CreatedAt = u.CreatedAt.DateTime,
                UpdatedAt = u.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
