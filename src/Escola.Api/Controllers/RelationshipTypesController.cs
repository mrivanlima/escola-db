using Escola.Api.Common;
using Escola.Application.DTOs.School;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipType;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipTypes;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelationshipTypesController : ControllerBase
{
    private readonly IGetRelationshipTypeHandler _getHandler;
    private readonly IGetRelationshipTypesHandler _getAllHandler;

    public RelationshipTypesController(
        IGetRelationshipTypeHandler getHandler,
        IGetRelationshipTypesHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? tenantUuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetRelationshipTypesRequest { TenantUuid = tenantUuid };
            var response = await _getAllHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<List<RelationshipTypeDto>>.SuccessResult(response.RelationshipTypes));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<RelationshipTypeDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetRelationshipTypeRequest { RelationshipTypeUuid = uuid };
            var response = await _getHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<RelationshipTypeDto>.SuccessResult(response.RelationshipType));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<RelationshipTypeDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<RelationshipTypeDto>.FailureResult(ex.Message));
        }
    }
}
