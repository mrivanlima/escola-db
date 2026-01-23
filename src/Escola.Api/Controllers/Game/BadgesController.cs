using Escola.Api.Common;
using Escola.Application.Game.Badges;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers.Game;

[ApiController]
[Route("api/game/badges")]
public class BadgesController : ControllerBase
{
    private readonly ICreateBadgeHandler _createHandler;
    private readonly IGetBadgeHandler _getHandler;
    private readonly IGetBadgesHandler _getBadgesHandler;
    private readonly IUpdateBadgeHandler _updateHandler;
    private readonly IValidator<CreateBadgeDto> _createValidator;
    private readonly IValidator<UpdateBadgeDto> _updateValidator;

    public BadgesController(
        ICreateBadgeHandler createHandler,
        IGetBadgeHandler getHandler,
        IGetBadgesHandler getBadgesHandler,
        IUpdateBadgeHandler updateHandler,
        IValidator<CreateBadgeDto> createValidator,
        IValidator<UpdateBadgeDto> updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getBadgesHandler = getBadgesHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BadgeDto>>> CreateBadge(
        [FromBody] CreateBadgeDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<BadgeDto>.FailureResult(
                "Validation failed",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var response = await _createHandler.Handle(new CreateBadgeRequest(dto), cancellationToken);
            return Ok(ApiResponse<BadgeDto>.SuccessResult(response.Badge));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<BadgeDto>.FailureResult(ex.Message));
        }
    }

    [HttpGet("{badgeUuid}")]
    public async Task<ActionResult<ApiResponse<BadgeDto>>> GetBadge(
        Guid badgeUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getHandler.Handle(new GetBadgeRequest(badgeUuid), cancellationToken);
            return Ok(ApiResponse<BadgeDto>.SuccessResult(response.Badge));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<BadgeDto>.FailureResult(ex.Message));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BadgeDto>>>> GetBadges(
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getBadgesHandler.Handle(new GetBadgesRequest(), cancellationToken);
            return Ok(ApiResponse<List<BadgeDto>>.SuccessResult(response.Badges));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<BadgeDto>>.FailureResult(ex.Message));
        }
    }

    [HttpPut("{badgeUuid}")]
    public async Task<ActionResult<ApiResponse<BadgeDto>>> UpdateBadge(
        Guid badgeUuid,
        [FromBody] UpdateBadgeDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<BadgeDto>.FailureResult(
                "Validation failed",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var response = await _updateHandler.Handle(new UpdateBadgeRequest(badgeUuid, dto), cancellationToken);
            return Ok(ApiResponse<BadgeDto>.SuccessResult(response.Badge));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<BadgeDto>.FailureResult(ex.Message));
        }
    }
}
