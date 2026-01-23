using Escola.Application.Assets.MediaFiles;
using Escola.Application.Services;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Assets.MediaFiles;

public class GetMediaFileHandler : IGetMediaFileHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMediaFileHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetMediaFileResponse> Handle(GetMediaFileRequest request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.GetCurrentTenantId();
        if (!tenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var mediaFile = await _context.MediaFiles
            .Where(mf => mf.FileUuid == request.FileUuid && mf.TenantId == tenantId.Value)
            .Include(mf => mf.Tenant)
            .Include(mf => mf.MimeType)
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
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MediaFile with UUID {request.FileUuid} not found for current tenant");

        return new GetMediaFileResponse(mediaFile);
    }
}
