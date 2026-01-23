namespace Escola.Application.Assets.MediaFiles;

public record GetMediaFilesRequest(Guid? MimeTypeUuid = null);

public record GetMediaFilesResponse(List<MediaFileDto> MediaFiles);

public interface IGetMediaFilesHandler
{
    Task<GetMediaFilesResponse> Handle(GetMediaFilesRequest request, CancellationToken cancellationToken);
}
