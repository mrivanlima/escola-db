using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.TeacherCertifications.CreateTeacherCertification;
using Escola.Application.UseCases.TeacherCertifications.GetTeacherCertification;
using Escola.Application.UseCases.TeacherCertifications.GetTeacherCertifications;
using Escola.Application.UseCases.TeacherCertifications.UpdateTeacherCertification;
using Escola.Application.Validators.School;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherCertificationsController : ControllerBase
{
    private readonly ICreateTeacherCertificationHandler _createHandler;
    private readonly IGetTeacherCertificationHandler _getHandler;
    private readonly IGetTeacherCertificationsHandler _getAllHandler;
    private readonly IUpdateTeacherCertificationHandler _updateHandler;
    private readonly CreateTeacherCertificationValidator _createValidator;
    private readonly UpdateTeacherCertificationValidator _updateValidator;

    public TeacherCertificationsController(ICreateTeacherCertificationHandler createHandler, IGetTeacherCertificationHandler getHandler,
        IGetTeacherCertificationsHandler getAllHandler, IUpdateTeacherCertificationHandler updateHandler,
        CreateTeacherCertificationValidator createValidator, UpdateTeacherCertificationValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? teacherUuid, [FromQuery] Guid? certificationUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetTeacherCertificationsRequest { TeacherUuid = teacherUuid, CertificationUuid = certificationUuid }, cancellationToken);
            return Ok(ApiResponse<List<TeacherCertificationDto>>.SuccessResult(response.TeacherCertifications));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<TeacherCertificationDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{teacherUuid:guid}/{certificationUuid:guid}")]
    public async Task<IActionResult> GetById(Guid teacherUuid, Guid certificationUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetTeacherCertificationRequest { TeacherUuid = teacherUuid, CertificationUuid = certificationUuid }, cancellationToken);
            return Ok(ApiResponse<TeacherCertificationDto>.SuccessResult(response.TeacherCertification));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherCertificationDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherCertificationDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _createHandler.HandleAsync(new CreateTeacherCertificationRequest { TeacherCertification = dto }, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { teacherUuid = dto.TeacherUuid, certificationUuid = dto.CertificationUuid }, ApiResponse<TeacherCertificationDto>.SuccessResult(response.TeacherCertification));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{teacherUuid:guid}/{certificationUuid:guid}")]
    public async Task<IActionResult> Update(Guid teacherUuid, Guid certificationUuid, [FromBody] UpdateTeacherCertificationDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(ApiResponse<TeacherCertificationDto>.FailureResult(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));

        try
        {
            var response = await _updateHandler.HandleAsync(new UpdateTeacherCertificationRequest { TeacherUuid = teacherUuid, CertificationUuid = certificationUuid, TeacherCertification = dto }, cancellationToken);
            return Ok(ApiResponse<TeacherCertificationDto>.SuccessResult(response.TeacherCertification));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TeacherCertificationDto>.FailureResult(ex.Message));
        }
    }
}
