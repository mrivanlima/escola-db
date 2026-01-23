using Escola.Api.Common;
using Escola.Application.Game.StudentBadges;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers.Game;

[ApiController]
[Route("api/game/student-badges")]
public class StudentBadgesController : ControllerBase
{
    private readonly IAwardStudentBadgeHandler _awardHandler;
    private readonly IGetStudentBadgesHandler _getBadgesHandler;
    private readonly IValidator<AwardStudentBadgeDto> _awardValidator;

    public StudentBadgesController(
        IAwardStudentBadgeHandler awardHandler,
        IGetStudentBadgesHandler getBadgesHandler,
        IValidator<AwardStudentBadgeDto> awardValidator)
    {
        _awardHandler = awardHandler;
        _getBadgesHandler = getBadgesHandler;
        _awardValidator = awardValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StudentBadgeDto>>> AwardBadge(
        [FromBody] AwardStudentBadgeDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _awardValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<StudentBadgeDto>.FailureResult(
                "Validation failed",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        try
        {
            var response = await _awardHandler.Handle(new AwardStudentBadgeRequest(dto), cancellationToken);
            return Ok(ApiResponse<StudentBadgeDto>.SuccessResult(response.Badge));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<StudentBadgeDto>.FailureResult(ex.Message));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<StudentBadgeDto>>>> GetStudentBadges(
        [FromQuery] Guid? studentUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _getBadgesHandler.Handle(
                new GetStudentBadgesRequest(studentUuid),
                cancellationToken);
            return Ok(ApiResponse<List<StudentBadgeDto>>.SuccessResult(response.Badges));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<StudentBadgeDto>>.FailureResult(ex.Message));
        }
    }
}
