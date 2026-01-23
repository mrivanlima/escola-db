namespace Escola.Application.Assets.MediaFiles;

public record UpdateMediaFileRequest(Guid FileUuid, UpdateMediaFileDto MediaFile);

public record UpdateMediaFileResponse(MediaFileDto MediaFile);

public interface IUpdateMediaFileHandler
{
    Task<UpdateMediaFileResponse> Handle(UpdateMediaFileRequest request, CancellationToken cancellationToken);
}
