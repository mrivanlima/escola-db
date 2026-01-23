namespace Escola.Application.UseCases.MimeTypes.GetMimeType;

public record GetMimeTypeRequest
{
    public Guid MimeTypeUuid { get; init; }
}

public record GetMimeTypeResponse
{
    public DTOs.Assets.MimeTypeDto MimeType { get; init; } = null!;
}

public interface IGetMimeTypeHandler
{
    Task<GetMimeTypeResponse> HandleAsync(GetMimeTypeRequest request, CancellationToken cancellationToken = default);
}
