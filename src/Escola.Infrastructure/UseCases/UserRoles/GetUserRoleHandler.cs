using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.UserRoles.GetUserRole;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.UserRoles;

/// <summary>
/// Implementation of Get UserRole handler using EF Core.
/// </summary>
public class GetUserRoleHandler : IGetUserRoleHandler
{
    private readonly EscolaDbContext _context;

    public GetUserRoleHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserRoleResponse> Handle(GetUserRoleRequest request, CancellationToken cancellationToken = default)
    {
        var userRole = await _context.Set<Domain.Identity.UserRole>()
            .AsNoTracking()
            .Where(ur => ur.UserRoleUuid == request.UserRoleUuid)
            .Select(ur => new UserRoleDto
            {
                UserRoleUuid = ur.UserRoleUuid,
                RoleName = ur.RoleName,
                Description = ur.Description,
                IsActive = ur.IsActive,
                CreatedAt = ur.CreatedAt.DateTime,
                UpdatedAt = ur.UpdatedAt.HasValue ? ur.UpdatedAt.Value.DateTime : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetUserRoleResponse
        {
            UserRole = userRole
        };
    }
}
