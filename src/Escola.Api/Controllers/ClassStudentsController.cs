using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ClassStudents.CreateClassStudent;
using Escola.Application.UseCases.ClassStudents.GetClassStudent;
using Escola.Application.UseCases.ClassStudents.GetClassStudents;
using Escola.Application.UseCases.ClassStudents.UpdateClassStudent;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassStudentsController : ControllerBase
{
    private readonly ICreateClassStudentHandler _createHandler;
    private readonly IGetClassStudentHandler _getHandler;
    private readonly IGetClassStudentsHandler _getAllHandler;
    private readonly IUpdateClassStudentHandler _updateHandler;
    private readonly CreateClassStudentValidator _createValidator;
    private readonly UpdateClassStudentValidator _updateValidator;

    public ClassStudentsController(ICreateClassStudentHandler createHandler, IGetClassStudentHandler getHandler,
        IGetClassStudentsHandler getAllHandler, IUpdateClassStudentHandler updateHandler,
        CreateClassStudentValidator createValidator, UpdateClassStudentValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? classUuid, [FromQuery] Guid? studentUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetClassStudentsRequest { ClassUuid = classUuid, StudentUuid = studentUuid }, cancellationToken);
            return Ok(ApiResponse<List<ClassStudentDto>>.SuccessResult(response.ClassStudents));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<ClassStudentDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{classUuid:guid}/{studentUuid:guid}")]
    public async Task<IActionResult> GetById(Guid classUuid, Guid studentUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetClassStudentRequest { ClassUuid = classUuid, StudentUuid = studentUuid }, cancellationToken);
            return Ok(ApiResponse<ClassStudentDto>.SuccessResult(response.ClassStudent));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassStudentDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<ClassStudentDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateClassStudentRequest { ClassStudent = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { classUuid = response.ClassStudent.ClassUuid, studentUuid = response.ClassStudent.StudentUuid }, 
                ApiResponse<ClassStudentDto>.SuccessResult(response.ClassStudent));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{classUuid:guid}/{studentUuid:guid}")]
    public async Task<IActionResult> Update(Guid classUuid, Guid studentUuid, [FromBody] UpdateClassStudentDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<ClassStudentDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateClassStudentRequest { ClassUuid = classUuid, StudentUuid = studentUuid, ClassStudent = dto }, cancellationToken);
            return Ok(ApiResponse<ClassStudentDto>.SuccessResult(response.ClassStudent));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ClassStudentDto>.FailureResult(ex.Message));
        }
    }
}
