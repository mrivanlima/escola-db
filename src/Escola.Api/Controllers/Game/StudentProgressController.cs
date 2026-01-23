using Escola.Api.Common;
using Escola.Application.Game.StudentProgress;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers.Game;

[ApiController]
[Route("api/game/student-progress")]
public class StudentProgressController : ControllerBase
{
    private readonly ICreateStudentProgressHandler _createHandler;
    private readonly IGetStudentProgressHandler _getHandler;
    private readonly IGetStudentProgressListHandler _getListHandler;
    private readonly IUpdateStudentProgressHandler _updateHandler;
    private readonly IValidator<CreateStudentProgressDto> _createValidator;
    private readonly IValidator<UpdateStudentProgressDto> _updateValidator;

    public StudentProgressController(
        ICreateStudentProgressHandler createHandler,
        IGetStudentProgressHandler getHandler,
        IGetStudentProgressListHandler getListHandler,
        IUpdateStudentProgressHandler updateHandler,
        IValidator<CreateStudentProgressDto> createValidator,
        IValidator<UpdateStudentProgressDto> updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getListHandler = getListHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StudentProgressDto>>> CreateStudentProgress(
        [FromBody] CreateStudentProgressDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<StudentProgressDto>.FailureResult(
                "Validation failed",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var response = await _createHandler.Handle(new CreateStudentProgressRequest(dto), cancellationToken);
            return Ok(ApiResponse<StudentProgressDto>.SuccessResult(response.Progress));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<StudentProgressDto>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{progressUuid}")]
    public async Task<ActionResult<ApiResponse<StudentProgressDto>>> GetStudentProgress(
        Guid progressUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.Handle(new GetStudentProgressRequest(progressUuid), cancellationToken);
            return Ok(ApiResponse<StudentProgressDto>.SuccessResult(response.Progress));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<StudentProgressDto>.FailureResult(ex.Message));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<StudentProgressDto>>>> GetStudentProgressList(
        [FromQuery] Guid? studentUuid,
        [FromQuery] Guid? activityUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getListHandler.Handle(
                new GetStudentProgressListRequest(studentUuid, activityUuid),
                cancellationToken);
            return Ok(ApiResponse<List<StudentProgressDto>>.SuccessResult(response.ProgressList));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<StudentProgressDto>>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{progressUuid}")]
    public async Task<ActionResult<ApiResponse<StudentProgressDto>>> UpdateStudentProgress(
        Guid progressUuid,
        [FromBody] UpdateStudentProgressDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<StudentProgressDto>.FailureResult(
                "Validation failed",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var response = await _updateHandler.Handle(new UpdateStudentProgressRequest(progressUuid, dto), cancellationToken);
            return Ok(ApiResponse<StudentProgressDto>.SuccessResult(response.Progress));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<StudentProgressDto>.FailureResult(ex.Message));
        }
    }
}
