namespace Escola.Application.Assets.MediaFiles;

public record CreateMediaFileRequest(CreateMediaFileDto MediaFile);

public record CreateMediaFileResponse(MediaFileDto MediaFile);

public interface ICreateMediaFileHandler
{
    Task<CreateMediaFileResponse> Handle(CreateMediaFileRequest request, CancellationToken cancellationToken);
}
