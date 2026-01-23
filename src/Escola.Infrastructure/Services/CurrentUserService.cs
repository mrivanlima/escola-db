using Escola.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Escola.Infrastructure.Services;

/// <summary>
/// Implementation of ICurrentUserService that extracts user/tenant context from HTTP claims.
/// This should be populated by authentication middleware.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentTenantId()
    {
        var tenantIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value;
        return int.TryParse(tenantIdClaim, out var tenantId) ? tenantId : null;
    }

    public int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value 
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public Guid? GetCurrentTenantUuid()
    {
        var tenantUuidClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_uuid")?.Value;
        return Guid.TryParse(tenantUuidClaim, out var tenantUuid) ? tenantUuid : null;
    }

    public Guid? GetCurrentUserUuid()
    {
        var userUuidClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("user_uuid")?.Value;
        return Guid.TryParse(userUuidClaim, out var userUuid) ? userUuid : null;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
