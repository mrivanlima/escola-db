using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Certifications.GetCertification;
using Escola.Application.UseCases.Certifications.GetCertifications;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificationsController : ControllerBase
{
    private readonly IGetCertificationHandler _getHandler;
    private readonly IGetCertificationsHandler _getAllHandler;

    public CertificationsController(IGetCertificationHandler getHandler, IGetCertificationsHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetCertificationsRequest(), cancellationToken);
            return Ok(ApiResponse<List<CertificationDto>>.SuccessResult(response.Certifications));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<CertificationDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetCertificationRequest { CertificationUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<CertificationDto>.SuccessResult(response.Certification));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<CertificationDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CertificationDto>.FailureResult(ex.Message));
        }
    }
}
