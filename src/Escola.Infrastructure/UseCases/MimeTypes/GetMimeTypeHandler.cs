using Escola.Application.DTOs.Assets;
using Escola.Application.UseCases.MimeTypes.GetMimeType;
using Escola.Domain.Assets;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.MimeTypes;

public class GetMimeTypeHandler : IGetMimeTypeHandler
{
    private readonly EscolaDbContext _context;

    public GetMimeTypeHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetMimeTypeResponse> HandleAsync(GetMimeTypeRequest request, CancellationToken cancellationToken = default)
    {
        var mimeType = await _context.Set<MimeType>()
            .Include(mt => mt.Category)
            .Where(mt => mt.MimeTypeUuid == request.MimeTypeUuid)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (mimeType == null)
            throw new InvalidOperationException($"MIME type with UUID {request.MimeTypeUuid} not found");

        return new GetMimeTypeResponse { MimeType = mimeType };
    }
}
