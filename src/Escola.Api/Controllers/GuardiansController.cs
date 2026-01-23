using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Guardians.CreateGuardian;
using Escola.Application.UseCases.Guardians.GetGuardian;
using Escola.Application.UseCases.Guardians.GetGuardians;
using Escola.Application.UseCases.Guardians.UpdateGuardian;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuardiansController : ControllerBase
{
    private readonly ICreateGuardianHandler _createHandler;
    private readonly IGetGuardianHandler _getHandler;
    private readonly IGetGuardiansHandler _getAllHandler;
    private readonly IUpdateGuardianHandler _updateHandler;
    private readonly CreateGuardianValidator _createValidator;
    private readonly UpdateGuardianValidator _updateValidator;

    public GuardiansController(
        ICreateGuardianHandler createHandler,
        IGetGuardianHandler getHandler,
        IGetGuardiansHandler getAllHandler,
        IUpdateGuardianHandler updateHandler,
        CreateGuardianValidator createValidator,
        UpdateGuardianValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetGuardiansRequest { TenantUuid = tenantUuid };
            var response = await _getAllHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<List<GuardianDto>>.SuccessResult(response.Guardians));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<GuardianDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetGuardianRequest { GuardianUuid = uuid };
            var response = await _getHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GuardianDto>.SuccessResult(response.Guardian));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuardianDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<GuardianDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new CreateGuardianRequest { Guardian = dto };
            var response = await _createHandler.HandleAsync(request, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { uuid = response.Guardian.GuardianUuid },
                ApiResponse<GuardianDto>.SuccessResult(response.Guardian));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateGuardianDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<GuardianDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new UpdateGuardianRequest { GuardianUuid = uuid, Guardian = dto };
            var response = await _updateHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GuardianDto>.SuccessResult(response.Guardian));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GuardianDto>.FailureResult(ex.Message));
        }
    }
}
