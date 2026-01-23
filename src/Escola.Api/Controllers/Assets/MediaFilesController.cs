using Escola.Api.Common;
using Escola.Application.Assets.MediaFiles;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers.Assets;

[ApiController]
[Route("api/mediafiles")]
public class MediaFilesController : ControllerBase
{
    private readonly ICreateMediaFileHandler _createHandler;
    private readonly IGetMediaFileHandler _getHandler;
    private readonly IGetMediaFilesHandler _getAllHandler;
    private readonly IUpdateMediaFileHandler _updateHandler;
    private readonly CreateMediaFileValidator _createValidator;
    private readonly UpdateMediaFileValidator _updateValidator;

    public MediaFilesController(
        ICreateMediaFileHandler createHandler,
        IGetMediaFileHandler getHandler,
        IGetMediaFilesHandler getAllHandler,
        IUpdateMediaFileHandler updateHandler,
        CreateMediaFileValidator createValidator,
        UpdateMediaFileValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MediaFileDto>>> CreateMediaFile(
        [FromBody] CreateMediaFileDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<MediaFileDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new CreateMediaFileRequest(dto);
        var response = await _createHandler.Handle(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetMediaFile),
            new { uuid = response.MediaFile.FileUuid },
            ApiResponse<MediaFileDto>.SuccessResult(response.MediaFile));
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<MediaFileDto>>> GetMediaFile(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var request = new GetMediaFileRequest(uuid);
        var response = await _getHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<MediaFileDto>.SuccessResult(response.MediaFile));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<MediaFileDto>>>> GetMediaFiles(
        [FromQuery] Guid? mimeTypeUuid,
        CancellationToken cancellationToken)
    {
        var request = new GetMediaFilesRequest(mimeTypeUuid);
        var response = await _getAllHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<List<MediaFileDto>>.SuccessResult(response.MediaFiles));
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<MediaFileDto>>> UpdateMediaFile(
        Guid uuid,
        [FromBody] UpdateMediaFileDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<MediaFileDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new UpdateMediaFileRequest(uuid, dto);
        var response = await _updateHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<MediaFileDto>.SuccessResult(response.MediaFile));
    }
}
