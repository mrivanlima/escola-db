namespace Escola.Application.Services;

/// <summary>
/// Service for accessing current user and tenant context.
/// Must be set by the API layer for each request.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current tenant's internal ID.
    /// Used for filtering queries and setting foreign keys.
    /// </summary>
    int? GetCurrentTenantId();

    /// <summary>
    /// Gets the current user's internal ID.
    /// Used for audit fields (created_by, updated_by).
    /// </summary>
    int? GetCurrentUserId();

    /// <summary>
    /// Gets the current tenant's UUID.
    /// </summary>
    Guid? GetCurrentTenantUuid();

    /// <summary>
    /// Gets the current user's UUID.
    /// </summary>
    Guid? GetCurrentUserUuid();

    /// <summary>
    /// Gets whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated();
}
