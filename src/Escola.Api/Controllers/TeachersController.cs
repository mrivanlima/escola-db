using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Teachers.CreateTeacher;
using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Application.UseCases.Teachers.UpdateTeacher;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ICreateTeacherHandler _createHandler;
    private readonly IGetTeacherHandler _getHandler;
    private readonly IGetTeachersHandler _getAllHandler;
    private readonly IUpdateTeacherHandler _updateHandler;
    private readonly CreateTeacherValidator _createValidator;
    private readonly UpdateTeacherValidator _updateValidator;

    public TeachersController(ICreateTeacherHandler createHandler, IGetTeacherHandler getHandler,
        IGetTeachersHandler getAllHandler, IUpdateTeacherHandler updateHandler,
        CreateTeacherValidator createValidator, UpdateTeacherValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, [FromQuery] Guid? specializationUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetTeachersRequest { TenantUuid = tenantUuid, SpecializationUuid = specializationUuid }, cancellationToken);
            return Ok(ApiResponse<List<TeacherDto>>.SuccessResult(response.Teachers));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<TeacherDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetTeacherRequest { TeacherUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<TeacherDto>.SuccessResult(response.Teacher));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateTeacherRequest { Teacher = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { uuid = response.Teacher.TeacherUuid }, ApiResponse<TeacherDto>.SuccessResult(response.Teacher));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateTeacherDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateTeacherRequest { TeacherUuid = uuid, Teacher = dto }, cancellationToken);
            return Ok(ApiResponse<TeacherDto>.SuccessResult(response.Teacher));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherDto>.FailureResult(ex.Message));
        }
    }
}
