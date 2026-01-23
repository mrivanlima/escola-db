using Escola.Api.Common;
using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MimeTypes.GetMimeType;
using Escola.Application.UseCases.MimeTypes.GetMimeTypes;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MimeTypesController : ControllerBase
{
    private readonly IGetMimeTypeHandler _getHandler;
    private readonly IGetMimeTypesHandler _getAllHandler;

    public MimeTypesController(
        IGetMimeTypeHandler getHandler,
        IGetMimeTypesHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? categoryUuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetMimeTypesRequest { CategoryUuid = categoryUuid }, cancellationToken);
            return Ok(ApiResponse<List<MimeTypeDto>>.SuccessResult(response.MimeTypes));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<MimeTypeDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetMimeTypeRequest { MimeTypeUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<MimeTypeDto>.SuccessResult(response.MimeType));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<MimeTypeDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MimeTypeDto>.FailureResult(ex.Message));
        }
    }
}
