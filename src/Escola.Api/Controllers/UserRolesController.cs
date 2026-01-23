using Escola.Api.Common;
using Escola.Application.UseCases.UserRoles.GetUserRole;
using Escola.Application.UseCases.UserRoles.GetUserRoles;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// User Roles management endpoints (Read-only lookup table).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly IGetUserRolesHandler _getUserRolesHandler;
    private readonly IGetUserRoleHandler _getUserRoleHandler;
    private readonly ILogger<UserRolesController> _logger;

    public UserRolesController(
        IGetUserRolesHandler getUserRolesHandler,
        IGetUserRoleHandler getUserRoleHandler,
        ILogger<UserRolesController> logger)
    {
        _getUserRolesHandler = getUserRolesHandler;
        _getUserRoleHandler = getUserRoleHandler;
        _logger = logger;
    }

    /// <summary>
    /// Get all user roles.
    /// </summary>
    /// <returns>List of all user roles in the system.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetUserRolesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserRolesRequest();
            var response = await _getUserRolesHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<GetUserRolesResponse>.SuccessResult(
                response,
                $"Retrieved {response.UserRoles.Count} user roles"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user roles");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching user roles",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Get a single user role by UUID.
    /// </summary>
    /// <param name="uuid">External UUID of the user role.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User role details.</returns>
    [HttpGet("{uuid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetUserRoleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserRoleRequest { UserRoleUuid = uuid };
            var response = await _getUserRoleHandler.Handle(request, cancellationToken);

            if (response.UserRole == null)
            {
                return NotFound(ApiResponse<object>.FailureResult($"User role with UUID {uuid} not found"));
            }

            return Ok(ApiResponse<GetUserRoleResponse>.SuccessResult(response, "User role retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user role {UserRoleUuid}", uuid);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching the user role",
                new List<string> { ex.Message }));
        }
    }
}
