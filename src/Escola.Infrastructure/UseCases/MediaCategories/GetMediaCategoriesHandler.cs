using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MediaCategories.GetMediaCategories;
using Escola.Domain.Assets;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.MediaCategories;

public class GetMediaCategoriesHandler : IGetMediaCategoriesHandler
{
    private readonly EscolaDbContext _context;

    public GetMediaCategoriesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetMediaCategoriesResponse> HandleAsync(GetMediaCategoriesRequest request, CancellationToken cancellationToken = default)
    {
        var categories = await _context.Set<MediaCategory>()
            .OrderBy(mc => mc.DisplayOrder)
            .Select(mc => new MediaCategoryDto
            {
                CategoryUuid = mc.CategoryUuid,
                CategoryCode = mc.CategoryCode,
                CategoryName = mc.CategoryName,
                DefaultIconName = mc.DefaultIconName,
                DefaultMaxSizeMb = mc.DefaultMaxSizeMb,
                DisplayOrder = mc.DisplayOrder,
                IsActive = mc.IsActive
            })
            .ToListAsync(cancellationToken);

        return new GetMediaCategoriesResponse { Categories = categories };
    }
}
