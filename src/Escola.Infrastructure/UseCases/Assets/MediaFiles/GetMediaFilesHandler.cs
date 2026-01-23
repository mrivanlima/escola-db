using Escola.Application.Assets.MediaFiles;
using Escola.Application.Services;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Assets.MediaFiles;

public class GetMediaFilesHandler : IGetMediaFilesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMediaFilesHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetMediaFilesResponse> Handle(GetMediaFilesRequest request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.GetCurrentTenantId();
        if (!tenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.MediaFiles
            .Where(mf => mf.TenantId == tenantId.Value)
            .Include(mf => mf.Tenant)
            .Include(mf => mf.MimeType)
            .AsQueryable();

        if (request.MimeTypeUuid.HasValue)
        {
            query = query.Where(mf => mf.MimeType.MimeTypeUuid == request.MimeTypeUuid.Value);
        }

        var mediaFiles = await query
            .OrderByDescending(mf => mf.CreatedAt)
            .Select(mf => new MediaFileDto
            {
                FileUuid = mf.FileUuid,
                TenantUuid = mf.Tenant.TenantUuid,
                TenantName = mf.Tenant.TenantName,
                MimeTypeUuid = mf.MimeType.MimeTypeUuid,
                MimeTypeCode = mf.MimeType.MimeTypeCode,
                OriginalName = mf.OriginalName,
                StoragePath = mf.StoragePath,
                SizeBytes = mf.SizeBytes,
                AltText = mf.AltText,
                Metadata = mf.Metadata,
                CreatedAt = mf.CreatedAt,
                UpdatedAt = mf.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetMediaFilesResponse(mediaFiles);
    }
}
