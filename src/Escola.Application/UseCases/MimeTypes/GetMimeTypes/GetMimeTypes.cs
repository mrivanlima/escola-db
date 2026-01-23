namespace Escola.Application.UseCases.MimeTypes.GetMimeTypes;

public record GetMimeTypesRequest
{
    public Guid? CategoryUuid { get; init; }
}

public record GetMimeTypesResponse
{
    public List<DTOs.Assets.MimeTypeDto> MimeTypes { get; init; } = new();
}

public interface IGetMimeTypesHandler
{
    Task<GetMimeTypesResponse> HandleAsync(GetMimeTypesRequest request, CancellationToken cancellationToken = default);
}
