using Escola.Application.Assets.MediaFiles;
using Escola.Application.Services;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Assets.MediaFiles;

public class UpdateMediaFileHandler : IUpdateMediaFileHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMediaFileHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateMediaFileResponse> Handle(UpdateMediaFileRequest request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.GetCurrentTenantId();
        if (!tenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var userId = _currentUserService.GetCurrentUserId();

        var mediaFile = await _context.MediaFiles
            .Where(mf => mf.FileUuid == request.FileUuid && mf.TenantId == tenantId.Value)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MediaFile with UUID {request.FileUuid} not found for current tenant");

        if (!string.IsNullOrWhiteSpace(request.MediaFile.AltText))
        {
            mediaFile.AltText = request.MediaFile.AltText;
        }

        if (!string.IsNullOrWhiteSpace(request.MediaFile.Metadata))
        {
            mediaFile.Metadata = request.MediaFile.Metadata;
        }

        mediaFile.UpdatedAt = DateTime.UtcNow;
        mediaFile.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with relationships
        var dto = await _context.MediaFiles
            .Where(mf => mf.FileId == mediaFile.FileId)
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
            .FirstAsync(cancellationToken);

        return new UpdateMediaFileResponse(dto);
    }
}
