using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatus;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatuses;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentStatusesController : ControllerBase
{
    private readonly IGetEnrollmentStatusHandler _getHandler;
    private readonly IGetEnrollmentStatusesHandler _getAllHandler;

    public EnrollmentStatusesController(
        IGetEnrollmentStatusHandler getHandler,
        IGetEnrollmentStatusesHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetEnrollmentStatusesRequest { TenantUuid = tenantUuid };
            var response = await _getAllHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<List<EnrollmentStatusDto>>.SuccessResult(response.EnrollmentStatuses));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<EnrollmentStatusDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetEnrollmentStatusRequest { EnrollmentStatusUuid = uuid };
            var response = await _getHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<EnrollmentStatusDto>.SuccessResult(response.EnrollmentStatus));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<EnrollmentStatusDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<EnrollmentStatusDto>.FailureResult(ex.Message));
        }
    }
}
