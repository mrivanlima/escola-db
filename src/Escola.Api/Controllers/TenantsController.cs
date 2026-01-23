using Escola.Api.Common;
using Escola.Application.UseCases.Tenants.CreateTenant;
using Escola.Application.UseCases.Tenants.GetTenant;
using Escola.Application.UseCases.Tenants.GetTenants;
using Escola.Application.UseCases.Tenants.UpdateTenant;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Tenants management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IGetTenantsHandler _getTenantsHandler;
    private readonly IGetTenantHandler _getTenantHandler;
    private readonly ICreateTenantHandler _createTenantHandler;
    private readonly IUpdateTenantHandler _updateTenantHandler;
    private readonly IValidator<CreateTenantRequest> _createValidator;
    private readonly IValidator<UpdateTenantRequest> _updateValidator;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(
        IGetTenantsHandler getTenantsHandler,
        IGetTenantHandler getTenantHandler,
        ICreateTenantHandler createTenantHandler,
        IUpdateTenantHandler updateTenantHandler,
        IValidator<CreateTenantRequest> createValidator,
        IValidator<UpdateTenantRequest> updateValidator,
        ILogger<TenantsController> logger)
    {
        _getTenantsHandler = getTenantsHandler;
        _getTenantHandler = getTenantHandler;
        _createTenantHandler = createTenantHandler;
        _updateTenantHandler = updateTenantHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all tenants.
    /// </summary>
    /// <returns>List of all tenants in the system.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetTenantsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTenantsRequest();
            var response = await _getTenantsHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<GetTenantsResponse>.SuccessResult(
                response,
                $"Retrieved {response.Tenants.Count} tenants"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenants");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching tenants",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Get a single tenant by UUID.
    /// </summary>
    /// <param name="uuid">External UUID of the tenant.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tenant details.</returns>
    [HttpGet("{uuid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetTenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTenantRequest { TenantUuid = uuid };
            var response = await _getTenantHandler.Handle(request, cancellationToken);

            if (response.Tenant == null)
            {
                return NotFound(ApiResponse<object>.FailureResult($"Tenant with UUID {uuid} not found"));
            }

            return Ok(ApiResponse<GetTenantResponse>.SuccessResult(response, "Tenant retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenant {TenantUuid}", uuid);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching the tenant",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Create a new tenant.
    /// </summary>
    /// <param name="request">Tenant creation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Newly created tenant.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateTenantResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var response = await _createTenantHandler.Handle(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { uuid = response.TenantUuid },
                ApiResponse<CreateTenantResponse>.SuccessResult(response, "Tenant created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating tenant");
            return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while creating the tenant",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Update an existing tenant.
    /// </summary>
    /// <param name="uuid">External UUID of the tenant to update.</param>
    /// <param name="request">Tenant update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated tenant.</returns>
    [HttpPut("{uuid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UpdateTenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Set the UUID from route parameter
            request.TenantUuid = uuid;

            // Validate request
            var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var response = await _updateTenantHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<UpdateTenantResponse>.SuccessResult(response, "Tenant updated successfully"));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            _logger.LogWarning(ex, "Tenant {TenantUuid} not found", uuid);
            return NotFound(ApiResponse<object>.FailureResult(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating tenant {TenantUuid}", uuid);
            return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant {TenantUuid}", uuid);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while updating the tenant",
                new List<string> { ex.Message }));
        }
    }
}
