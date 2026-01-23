using Escola.Api.Common;
using Escola.Application.Content.Activities;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers.Content;

[ApiController]
[Route("api/activities")]
public class ActivitiesController : ControllerBase
{
    private readonly ICreateActivityHandler _createHandler;
    private readonly IGetActivityHandler _getHandler;
    private readonly IGetActivitiesHandler _getAllHandler;
    private readonly IUpdateActivityHandler _updateHandler;
    private readonly CreateActivityValidator _createValidator;
    private readonly UpdateActivityValidator _updateValidator;

    public ActivitiesController(
        ICreateActivityHandler createHandler,
        IGetActivityHandler getHandler,
        IGetActivitiesHandler getAllHandler,
        IUpdateActivityHandler updateHandler,
        CreateActivityValidator createValidator,
        UpdateActivityValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ActivityDto>>> CreateActivity(
        [FromBody] CreateActivityDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ActivityDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new CreateActivityRequest(dto);
        var response = await _createHandler.Handle(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetActivity),
            new { uuid = response.Activity.ActivityUuid },
            ApiResponse<ActivityDto>.SuccessResult(response.Activity));
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ActivityDto>>> GetActivity(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var request = new GetActivityRequest(uuid);
        var response = await _getHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ActivityDto>.SuccessResult(response.Activity));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ActivityDto>>>> GetActivities(
        [FromQuery] Guid? moduleUuid,
        CancellationToken cancellationToken)
    {
        var request = new GetActivitiesRequest(moduleUuid);
        var response = await _getAllHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<List<ActivityDto>>.SuccessResult(response.Activities));
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ActivityDto>>> UpdateActivity(
        Guid uuid,
        [FromBody] UpdateActivityDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ActivityDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new UpdateActivityRequest(uuid, dto);
        var response = await _updateHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ActivityDto>.SuccessResult(response.Activity));
    }
}
