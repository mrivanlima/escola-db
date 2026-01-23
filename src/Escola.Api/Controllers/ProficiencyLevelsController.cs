using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevel;
using Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevels;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProficiencyLevelsController : ControllerBase
{
    private readonly IGetProficiencyLevelHandler _getHandler;
    private readonly IGetProficiencyLevelsHandler _getAllHandler;

    public ProficiencyLevelsController(IGetProficiencyLevelHandler getHandler, IGetProficiencyLevelsHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetProficiencyLevelsRequest { TenantUuid = tenantUuid }, cancellationToken);
            return Ok(ApiResponse<List<ProficiencyLevelDto>>.SuccessResult(response.ProficiencyLevels));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<ProficiencyLevelDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetProficiencyLevelRequest { ProficiencyUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<ProficiencyLevelDto>.SuccessResult(response.ProficiencyLevel));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ProficiencyLevelDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ProficiencyLevelDto>.FailureResult(ex.Message));
        }
    }
}
