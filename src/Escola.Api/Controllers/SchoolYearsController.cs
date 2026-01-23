using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.SchoolYears.CreateSchoolYear;
using Escola.Application.UseCases.SchoolYears.GetSchoolYear;
using Escola.Application.UseCases.SchoolYears.GetSchoolYears;
using Escola.Application.UseCases.SchoolYears.UpdateSchoolYear;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolYearsController : ControllerBase
{
    private readonly ICreateSchoolYearHandler _createHandler;
    private readonly IGetSchoolYearHandler _getHandler;
    private readonly IGetSchoolYearsHandler _getAllHandler;
    private readonly IUpdateSchoolYearHandler _updateHandler;
    private readonly CreateSchoolYearValidator _createValidator;
    private readonly UpdateSchoolYearValidator _updateValidator;

    public SchoolYearsController(
        ICreateSchoolYearHandler createHandler,
        IGetSchoolYearHandler getHandler,
        IGetSchoolYearsHandler getAllHandler,
        IUpdateSchoolYearHandler updateHandler,
        CreateSchoolYearValidator createValidator,
        UpdateSchoolYearValidator updateValidator)
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
            var request = new GetSchoolYearsRequest { TenantUuid = tenantUuid };
            var response = await _getAllHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<List<SchoolYearDto>>.SuccessResult(response.SchoolYears));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<SchoolYearDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetSchoolYearRequest { SchoolYearUuid = uuid };
            var response = await _getHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<SchoolYearDto>.SuccessResult(response.SchoolYear));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSchoolYearDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<SchoolYearDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new CreateSchoolYearRequest { SchoolYear = dto };
            var response = await _createHandler.HandleAsync(request, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { uuid = response.SchoolYear.SchoolYearUuid },
                ApiResponse<SchoolYearDto>.SuccessResult(response.SchoolYear));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateSchoolYearDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<SchoolYearDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new UpdateSchoolYearRequest { SchoolYearUuid = uuid, SchoolYear = dto };
            var response = await _updateHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<SchoolYearDto>.SuccessResult(response.SchoolYear));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SchoolYearDto>.FailureResult(ex.Message));
        }
    }
}
