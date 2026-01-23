using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MimeTypes.GetMimeTypes;
using Escola.Domain.Assets;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.MimeTypes;

public class GetMimeTypesHandler : IGetMimeTypesHandler
{
    private readonly EscolaDbContext _context;

    public GetMimeTypesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetMimeTypesResponse> HandleAsync(GetMimeTypesRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<MimeType>()
            .Include(mt => mt.Category)
            .AsQueryable();

        // Filter by category if provided
        if (request.CategoryUuid.HasValue)
        {
            query = query.Where(mt => mt.Category.CategoryUuid == request.CategoryUuid.Value);
        }

        var mimeTypes = await query
            .OrderBy(mt => mt.Category.DisplayOrder)
            .ThenBy(mt => mt.MimeTypeCode)
            .Select(mt => new MimeTypeDto
            {
                MimeTypeUuid = mt.MimeTypeUuid,
                MimeTypeCode = mt.MimeTypeCode,
                MimeTypeName = mt.MimeTypeName,
                CategoryUuid = mt.Category.CategoryUuid,
                CategoryName = mt.Category.CategoryName,
                FileExtension = mt.FileExtension,
                IconName = mt.IconName,
                MaxFileSizeMb = mt.MaxFileSizeMb,
                IsActive = mt.IsActive
            })
            .ToListAsync(cancellationToken);

        return new GetMimeTypesResponse { MimeTypes = mimeTypes };
    }
}
