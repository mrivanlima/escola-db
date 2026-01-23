using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Specializations.GetSpecialization;
using Escola.Application.UseCases.Specializations.GetSpecializations;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationsController : ControllerBase
{
    private readonly IGetSpecializationHandler _getHandler;
    private readonly IGetSpecializationsHandler _getAllHandler;

    public SpecializationsController(IGetSpecializationHandler getHandler, IGetSpecializationsHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetSpecializationsRequest(), cancellationToken);
            return Ok(ApiResponse<List<SpecializationDto>>.SuccessResult(response.Specializations));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<SpecializationDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetSpecializationRequest { SpecializationUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<SpecializationDto>.SuccessResult(response.Specialization));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SpecializationDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<SpecializationDto>.FailureResult(ex.Message));
        }
    }
}
