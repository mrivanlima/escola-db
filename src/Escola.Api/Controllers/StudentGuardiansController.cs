using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.StudentGuardians.CreateStudentGuardian;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardian;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardians;
using Escola.Application.UseCases.StudentGuardians.UpdateStudentGuardian;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGuardiansController : ControllerBase
{
    private readonly ICreateStudentGuardianHandler _createHandler;
    private readonly IGetStudentGuardianHandler _getHandler;
    private readonly IGetStudentGuardiansHandler _getAllHandler;
    private readonly IUpdateStudentGuardianHandler _updateHandler;
    private readonly CreateStudentGuardianValidator _createValidator;
    private readonly UpdateStudentGuardianValidator _updateValidator;

    public StudentGuardiansController(
        ICreateStudentGuardianHandler createHandler,
        IGetStudentGuardianHandler getHandler,
        IGetStudentGuardiansHandler getAllHandler,
        IUpdateStudentGuardianHandler updateHandler,
        CreateStudentGuardianValidator createValidator,
        UpdateStudentGuardianValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? studentUuid,
        [FromQuery] Guid? guardianUuid,
        [FromQuery] Guid? tenantUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetStudentGuardiansRequest 
            { 
                StudentUuid = studentUuid,
                GuardianUuid = guardianUuid,
                TenantUuid = tenantUuid 
            };
            var response = await _getAllHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<List<StudentGuardianDto>>.SuccessResult(response.StudentGuardians));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<StudentGuardianDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{studentUuid:guid}/{guardianUuid:guid}")]
    public async Task<IActionResult> GetById(Guid studentUuid, Guid guardianUuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetStudentGuardianRequest 
            { 
                StudentUuid = studentUuid,
                GuardianUuid = guardianUuid 
            };
            var response = await _getHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<StudentGuardianDto>.SuccessResult(response.StudentGuardian));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentGuardianDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<StudentGuardianDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new CreateStudentGuardianRequest { StudentGuardian = dto };
            var response = await _createHandler.HandleAsync(request, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { studentUuid = response.StudentGuardian.StudentUuid, guardianUuid = response.StudentGuardian.GuardianUuid },
                ApiResponse<StudentGuardianDto>.SuccessResult(response.StudentGuardian));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{studentUuid:guid}/{guardianUuid:guid}")]
    public async Task<IActionResult> Update(
        Guid studentUuid, 
        Guid guardianUuid, 
        [FromBody] UpdateStudentGuardianDto dto, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<StudentGuardianDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        try
        {
            var request = new UpdateStudentGuardianRequest 
            { 
                StudentUuid = studentUuid,
                GuardianUuid = guardianUuid,
                StudentGuardian = dto 
            };
            var response = await _updateHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<StudentGuardianDto>.SuccessResult(response.StudentGuardian));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGuardianDto>.FailureResult(ex.Message));
        }
    }
}
