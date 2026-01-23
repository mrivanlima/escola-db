using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.UserRoles.GetUserRoles;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.UserRoles;

/// <summary>
/// Implementation of Get UserRoles handler in Infrastructure layer.
/// </summary>
public class GetUserRolesHandler : IGetUserRolesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<GetUserRolesHandler> _logger;

    public GetUserRolesHandler(EscolaDbContext context, ILogger<GetUserRolesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetUserRolesResponse> Handle(GetUserRolesRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all user roles");

        var userRoles = await _context.Set<Domain.Identity.UserRole>()
            .AsNoTracking()
            .OrderBy(ur => ur.RoleName)
            .Select(ur => new UserRoleDto
            {
                UserRoleUuid = ur.UserRoleUuid,
                RoleName = ur.RoleName,
                Description = ur.Description,
                IsActive = ur.IsActive,
                CreatedAt = ur.CreatedAt.DateTime,
                UpdatedAt = ur.UpdatedAt.HasValue ? ur.UpdatedAt.Value.DateTime : null
            })
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} user roles", userRoles.Count);

        return new GetUserRolesResponse
        {
            UserRoles = userRoles
        };
    }
}
