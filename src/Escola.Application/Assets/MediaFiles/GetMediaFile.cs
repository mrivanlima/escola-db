namespace Escola.Application.Assets.MediaFiles;

public record GetMediaFileRequest(Guid FileUuid);

public record GetMediaFileResponse(MediaFileDto MediaFile);

public interface IGetMediaFileHandler
{
    Task<GetMediaFileResponse> Handle(GetMediaFileRequest request, CancellationToken cancellationToken);
}
