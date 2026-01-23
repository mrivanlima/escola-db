namespace Escola.Application.UseCases.MediaCategories.GetMediaCategories;

public record GetMediaCategoriesRequest
{
}

public record GetMediaCategoriesResponse
{
    public List<DTOs.Assets.MediaCategoryDto> Categories { get; init; } = new();
}

public interface IGetMediaCategoriesHandler
{
    Task<GetMediaCategoriesResponse> HandleAsync(GetMediaCategoriesRequest request, CancellationToken cancellationToken = default);
}
