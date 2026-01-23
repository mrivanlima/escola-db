using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.TeacherSubjects.CreateTeacherSubject;
using Escola.Application.UseCases.TeacherSubjects.GetTeacherSubject;
using Escola.Application.UseCases.TeacherSubjects.GetTeacherSubjects;
using Escola.Application.UseCases.TeacherSubjects.UpdateTeacherSubject;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherSubjectsController : ControllerBase
{
    private readonly ICreateTeacherSubjectHandler _createHandler;
    private readonly IGetTeacherSubjectHandler _getHandler;
    private readonly IGetTeacherSubjectsHandler _getAllHandler;
    private readonly IUpdateTeacherSubjectHandler _updateHandler;
    private readonly CreateTeacherSubjectValidator _createValidator;
    private readonly UpdateTeacherSubjectValidator _updateValidator;

    public TeacherSubjectsController(ICreateTeacherSubjectHandler createHandler, IGetTeacherSubjectHandler getHandler,
        IGetTeacherSubjectsHandler getAllHandler, IUpdateTeacherSubjectHandler updateHandler,
        CreateTeacherSubjectValidator createValidator, UpdateTeacherSubjectValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? teacherUuid, [FromQuery] Guid? subjectUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetTeacherSubjectsRequest { TeacherUuid = teacherUuid, SubjectUuid = subjectUuid }, cancellationToken);
            return Ok(ApiResponse<List<TeacherSubjectDto>>.SuccessResult(response.TeacherSubjects));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<TeacherSubjectDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{teacherUuid:guid}/{subjectUuid:guid}")]
    public async Task<IActionResult> GetById(Guid teacherUuid, Guid subjectUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetTeacherSubjectRequest { TeacherUuid = teacherUuid, SubjectUuid = subjectUuid }, cancellationToken);
            return Ok(ApiResponse<TeacherSubjectDto>.SuccessResult(response.TeacherSubject));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherSubjectDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherSubjectDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateTeacherSubjectRequest { TeacherSubject = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { teacherUuid = dto.TeacherUuid, subjectUuid = dto.SubjectUuid }, ApiResponse<TeacherSubjectDto>.SuccessResult(response.TeacherSubject));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{teacherUuid:guid}/{subjectUuid:guid}")]
    public async Task<IActionResult> Update(Guid teacherUuid, Guid subjectUuid, [FromBody] UpdateTeacherSubjectDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherSubjectDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateTeacherSubjectRequest { TeacherUuid = teacherUuid, SubjectUuid = subjectUuid, TeacherSubject = dto }, cancellationToken);
            return Ok(ApiResponse<TeacherSubjectDto>.SuccessResult(response.TeacherSubject));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherSubjectDto>.FailureResult(ex.Message));
        }
    }
}
