using Escola.Api.Common;
using Escola.Application.Content.ActivityResources;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers.Content;

[ApiController]
[Route("api/activityresources")]
public class ActivityResourcesController : ControllerBase
{
    private readonly ICreateActivityResourceHandler _createHandler;
    private readonly IGetActivityResourceHandler _getHandler;
    private readonly IGetActivityResourcesHandler _getAllHandler;
    private readonly IUpdateActivityResourceHandler _updateHandler;
    private readonly CreateActivityResourceValidator _createValidator;
    private readonly UpdateActivityResourceValidator _updateValidator;

    public ActivityResourcesController(
        ICreateActivityResourceHandler createHandler,
        IGetActivityResourceHandler getHandler,
        IGetActivityResourcesHandler getAllHandler,
        IUpdateActivityResourceHandler updateHandler,
        CreateActivityResourceValidator createValidator,
        UpdateActivityResourceValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ActivityResourceDto>>> CreateActivityResource(
        [FromBody] CreateActivityResourceDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ActivityResourceDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new CreateActivityResourceRequest(dto);
        var response = await _createHandler.Handle(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetActivityResource),
            new { uuid = response.Resource.ResourceUuid },
            ApiResponse<ActivityResourceDto>.SuccessResult(response.Resource));
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ActivityResourceDto>>> GetActivityResource(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var request = new GetActivityResourceRequest(uuid);
        var response = await _getHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ActivityResourceDto>.SuccessResult(response.Resource));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ActivityResourceDto>>>> GetActivityResources(
        [FromQuery] Guid? activityUuid,
        CancellationToken cancellationToken)
    {
        var request = new GetActivityResourcesRequest(activityUuid);
        var response = await _getAllHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<List<ActivityResourceDto>>.SuccessResult(response.Resources));
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ActivityResourceDto>>> UpdateActivityResource(
        Guid uuid,
        [FromBody] UpdateActivityResourceDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ActivityResourceDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new UpdateActivityResourceRequest(uuid, dto);
        var response = await _updateHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ActivityResourceDto>.SuccessResult(response.Resource));
    }
}
