using Escola.Application.Assets.MediaFiles;
using Escola.Application.Services;
using Escola.Domain.Assets;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Assets.MediaFiles;

public class CreateMediaFileHandler : ICreateMediaFileHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateMediaFileHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateMediaFileResponse> Handle(CreateMediaFileRequest request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.GetCurrentTenantId();
        if (!tenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var userId = _currentUserService.GetCurrentUserId();

        // Resolve MimeTypeId from MimeTypeUuid
        var mimeType = await _context.MimeTypes
            .Where(mt => mt.MimeTypeUuid == request.MediaFile.MimeTypeUuid)
            .Select(mt => new { mt.MimeTypeId, mt.MimeTypeUuid })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MimeType with UUID {request.MediaFile.MimeTypeUuid} not found");

        var mediaFile = new MediaFile
        {
            FileUuid = Guid.NewGuid(),
            TenantId = tenantId.Value,
            MimeTypeId = (short)mimeType.MimeTypeId,
            OriginalName = request.MediaFile.OriginalName,
            StoragePath = request.MediaFile.StoragePath,
            SizeBytes = request.MediaFile.SizeBytes,
            AltText = request.MediaFile.AltText,
            Metadata = request.MediaFile.Metadata,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _context.MediaFiles.Add(mediaFile);
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

        return new CreateMediaFileResponse(dto);
    }
}
