using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Classes.CreateClass;
using Escola.Application.UseCases.Classes.GetClass;
using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Application.UseCases.Classes.UpdateClass;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly ICreateClassHandler _createHandler;
    private readonly IGetClassHandler _getHandler;
    private readonly IGetClassesHandler _getAllHandler;
    private readonly IUpdateClassHandler _updateHandler;
    private readonly CreateClassValidator _createValidator;
    private readonly UpdateClassValidator _updateValidator;

    public ClassesController(ICreateClassHandler createHandler, IGetClassHandler getHandler, IGetClassesHandler getAllHandler,
        IUpdateClassHandler updateHandler, CreateClassValidator createValidator, UpdateClassValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, [FromQuery] Guid? schoolYearUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetClassesRequest { TenantUuid = tenantUuid, SchoolYearUuid = schoolYearUuid }, cancellationToken);
            return Ok(ApiResponse<List<ClassDto>>.SuccessResult(response.Classes));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<ClassDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetClassRequest { ClassUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<ClassDto>.SuccessResult(response.Class));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<ClassDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateClassRequest { Class = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { uuid = response.Class.ClassUuid }, ApiResponse<ClassDto>.SuccessResult(response.Class));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateClassDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<ClassDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateClassRequest { ClassUuid = uuid, Class = dto }, cancellationToken);
            return Ok(ApiResponse<ClassDto>.SuccessResult(response.Class));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassDto>.FailureResult(ex.Message));
        }
    }
}
