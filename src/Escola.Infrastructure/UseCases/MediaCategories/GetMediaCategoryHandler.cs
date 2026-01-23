using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MediaCategories.GetMediaCategory;
using Escola.Domain.Assets;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.MediaCategories;

public class GetMediaCategoryHandler : IGetMediaCategoryHandler
{
    private readonly EscolaDbContext _context;

    public GetMediaCategoryHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetMediaCategoryResponse> HandleAsync(GetMediaCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _context.Set<MediaCategory>()
            .Where(mc => mc.CategoryUuid == request.CategoryUuid)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
            throw new InvalidOperationException($"Media category with UUID {request.CategoryUuid} not found");

        return new GetMediaCategoryResponse { Category = category };
    }
}
