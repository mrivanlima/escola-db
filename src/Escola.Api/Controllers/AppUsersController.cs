using Escola.Api.Common;
using Escola.Application.DTOs.Identity;
using Escola.Application.UseCases.AppUsers.CreateAppUser;
using Escola.Application.UseCases.AppUsers.GetAppUser;
using Escola.Application.UseCases.AppUsers.GetAppUsers;
using Escola.Application.UseCases.AppUsers.UpdateAppUser;
using Escola.Application.Validators.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Controller for managing app users.
/// </summary>
[ApiController]
[Route("api/appusers")]
public class AppUsersController : ControllerBase
{
    private readonly IGetAppUsersHandler _getAppUsersHandler;
    private readonly IGetAppUserHandler _getAppUserHandler;
    private readonly ICreateAppUserHandler _createAppUserHandler;
    private readonly IUpdateAppUserHandler _updateAppUserHandler;
    private readonly IValidator<CreateAppUserDto> _createValidator;
    private readonly IValidator<UpdateAppUserDto> _updateValidator;

    public AppUsersController(
        IGetAppUsersHandler getAppUsersHandler,
        IGetAppUserHandler getAppUserHandler,
        ICreateAppUserHandler createAppUserHandler,
        IUpdateAppUserHandler updateAppUserHandler,
        IValidator<CreateAppUserDto> createValidator,
        IValidator<UpdateAppUserDto> updateValidator)
    {
        _getAppUsersHandler = getAppUsersHandler;
        _getAppUserHandler = getAppUserHandler;
        _createAppUserHandler = createAppUserHandler;
        _updateAppUserHandler = updateAppUserHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all app users, optionally filtered by tenant.
    /// </summary>
    /// <param name="tenantUuid">Optional tenant UUID to filter users.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of app users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAppUsersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppUsers(
        [FromQuery] Guid? tenantUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAppUsersRequest { TenantUuid = tenantUuid };
            var result = await _getAppUsersHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GetAppUsersResponse>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GetAppUsersResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get a single app user by UUID.
    /// </summary>
    /// <param name="uuid">The app user UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested app user.</returns>
    [HttpGet("{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<GetAppUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<GetAppUserResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppUser(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAppUserRequest { UserUuid = uuid };
            var result = await _getAppUserHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GetAppUserResponse>.SuccessResult(result));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GetAppUserResponse>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GetAppUserResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Create a new app user.
    /// </summary>
    /// <param name="dto">The app user data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created app user.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateAppUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CreateAppUserResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAppUser(
        [FromBody] CreateAppUserDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<CreateAppUserResponse>.FailureResult("Validation failed.", errors));
            }

            var request = new CreateAppUserRequest { AppUser = dto };
            var result = await _createAppUserHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<CreateAppUserResponse>.SuccessResult(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<CreateAppUserResponse>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CreateAppUserResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Update an existing app user.
    /// </summary>
    /// <param name="uuid">The app user UUID.</param>
    /// <param name="dto">The updated app user data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated app user.</returns>
    [HttpPut("{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<UpdateAppUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UpdateAppUserResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<UpdateAppUserResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppUser(
        Guid uuid,
        [FromBody] UpdateAppUserDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<UpdateAppUserResponse>.FailureResult("Validation failed.", errors));
            }

            var request = new UpdateAppUserRequest { UserUuid = uuid, AppUser = dto };
            var result = await _updateAppUserHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<UpdateAppUserResponse>.SuccessResult(result));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<UpdateAppUserResponse>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UpdateAppUserResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }
}
