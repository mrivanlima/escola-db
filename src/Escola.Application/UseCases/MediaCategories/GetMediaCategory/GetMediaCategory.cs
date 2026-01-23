namespace Escola.Application.UseCases.MediaCategories.GetMediaCategory;

public record GetMediaCategoryRequest
{
    public Guid CategoryUuid { get; init; }
}

public record GetMediaCategoryResponse
{
    public DTOs.Assets.MediaCategoryDto Category { get; init; } = null!;
}

public interface IGetMediaCategoryHandler
{
    Task<GetMediaCategoryResponse> HandleAsync(GetMediaCategoryRequest request, CancellationToken cancellationToken = default);
}
