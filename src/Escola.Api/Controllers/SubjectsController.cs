using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Subjects.CreateSubject;
using Escola.Application.UseCases.Subjects.GetSubject;
using Escola.Application.UseCases.Subjects.GetSubjects;
using Escola.Application.UseCases.Subjects.UpdateSubject;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ICreateSubjectHandler _createHandler;
    private readonly IGetSubjectHandler _getHandler;
    private readonly IGetSubjectsHandler _getAllHandler;
    private readonly IUpdateSubjectHandler _updateHandler;
    private readonly CreateSubjectValidator _createValidator;
    private readonly UpdateSubjectValidator _updateValidator;

    public SubjectsController(ICreateSubjectHandler createHandler, IGetSubjectHandler getHandler,
        IGetSubjectsHandler getAllHandler, IUpdateSubjectHandler updateHandler,
        CreateSubjectValidator createValidator, UpdateSubjectValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? gradeLevelUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetSubjectsRequest { GradeLevelUuid = gradeLevelUuid }, cancellationToken);
            return Ok(ApiResponse<List<SubjectDto>>.SuccessResult(response.Subjects));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<SubjectDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetSubjectRequest { SubjectUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<SubjectDto>.SuccessResult(response.Subject));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubjectDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<SubjectDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateSubjectRequest { Subject = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { uuid = response.Subject.SubjectUuid }, ApiResponse<SubjectDto>.SuccessResult(response.Subject));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<IActionResult> Update(Guid uuid, [FromBody] UpdateSubjectDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<SubjectDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateSubjectRequest { SubjectUuid = uuid, Subject = dto }, cancellationToken);
            return Ok(ApiResponse<SubjectDto>.SuccessResult(response.Subject));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SubjectDto>.FailureResult(ex.Message));
        }
    }
}
