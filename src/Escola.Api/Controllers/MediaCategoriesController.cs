using Escola.Api.Common;
using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MediaCategories.GetMediaCategories;
using Escola.Application.UseCases.MediaCategories.GetMediaCategory;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaCategoriesController : ControllerBase
{
    private readonly IGetMediaCategoryHandler _getHandler;
    private readonly IGetMediaCategoriesHandler _getAllHandler;

    public MediaCategoriesController(
        IGetMediaCategoryHandler getHandler,
        IGetMediaCategoriesHandler getAllHandler)
    {
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getAllHandler.HandleAsync(new GetMediaCategoriesRequest(), cancellationToken);
            return Ok(ApiResponse<List<MediaCategoryDto>>.SuccessResult(response.Categories));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<MediaCategoryDto>>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.HandleAsync(new GetMediaCategoryRequest { CategoryUuid = uuid }, cancellationToken);
            return Ok(ApiResponse<MediaCategoryDto>.SuccessResult(response.Category));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<MediaCategoryDto>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MediaCategoryDto>.FailureResult(ex.Message));
        }
    }
}
