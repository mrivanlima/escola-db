using Escola.Application.Content.ActivityResources;
using Escola.Application.Services;
using Escola.Domain.Content;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.ActivityResources;

public class CreateActivityResourceHandler : ICreateActivityResourceHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateActivityResourceHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateActivityResourceResponse> Handle(CreateActivityResourceRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var activity = await _context.Activities
            .Where(a => a.ActivityUuid == request.Resource.ActivityUuid)
            .Select(a => new { a.ActivityId, a.ActivityUuid, a.ActivityName })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Activity with UUID {request.Resource.ActivityUuid} not found");

        var mediaFile = await _context.MediaFiles
            .Where(mf => mf.FileUuid == request.Resource.MediaFileUuid)
            .Select(mf => new { mf.FileId, mf.FileUuid, mf.OriginalName })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MediaFile with UUID {request.Resource.MediaFileUuid} not found");

        var resource = new ActivityResource
        {
            ResourceUuid = Guid.NewGuid(),
            ActivityId = activity.ActivityId,
            ResourceName = request.Resource.ResourceName,
            ResourceType = request.Resource.ResourceType,
            MediaFileId = mediaFile.FileUuid,
            DisplayOrder = request.Resource.DisplayOrder,
            IsRequired = request.Resource.IsRequired,
            UsageContext = request.Resource.UsageContext,
            ResourceConfig = request.Resource.ResourceConfig,
            IsPublished = request.Resource.IsPublished,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userId
        };

        _context.ActivityResources.Add(resource);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = new ActivityResourceDto
        {
            ResourceUuid = resource.ResourceUuid,
            ActivityUuid = activity.ActivityUuid,
            ActivityName = activity.ActivityName,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            MediaFileUuid = mediaFile.FileUuid,
            MediaFileName = mediaFile.OriginalName,
            DisplayOrder = resource.DisplayOrder,
            IsRequired = resource.IsRequired,
            UsageContext = resource.UsageContext,
            ResourceConfig = resource.ResourceConfig,
            IsPublished = resource.IsPublished,
            CreatedAt = resource.CreatedAt,
            UpdatedAt = resource.UpdatedAt
        };

        return new CreateActivityResourceResponse(dto);
    }
}

public class GetActivityResourceHandler : IGetActivityResourceHandler
{
    private readonly EscolaDbContext _context;

    public GetActivityResourceHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivityResourceResponse> Handle(GetActivityResourceRequest request, CancellationToken cancellationToken)
    {
        var resource = await _context.ActivityResources
            .Include(ar => ar.Activity)
            .Include(ar => ar.MediaFile)
            .Where(ar => ar.ResourceUuid == request.ResourceUuid)
            .Select(ar => new ActivityResourceDto
            {
                ResourceUuid = ar.ResourceUuid,
                ActivityUuid = ar.Activity.ActivityUuid,
                ActivityName = ar.Activity.ActivityName,
                ResourceName = ar.ResourceName,
                ResourceType = ar.ResourceType,
                MediaFileUuid = ar.MediaFile.FileUuid,
                MediaFileName = ar.MediaFile.OriginalName,
                DisplayOrder = ar.DisplayOrder,
                IsRequired = ar.IsRequired,
                UsageContext = ar.UsageContext,
                ResourceConfig = ar.ResourceConfig,
                IsPublished = ar.IsPublished,
                CreatedAt = ar.CreatedAt,
                UpdatedAt = ar.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"ActivityResource with UUID {request.ResourceUuid} not found");

        return new GetActivityResourceResponse(resource);
    }
}

public class GetActivityResourcesHandler : IGetActivityResourcesHandler
{
    private readonly EscolaDbContext _context;

    public GetActivityResourcesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivityResourcesResponse> Handle(GetActivityResourcesRequest request, CancellationToken cancellationToken)
    {
        var query = _context.ActivityResources
            .Include(ar => ar.Activity)
            .Include(ar => ar.MediaFile)
            .AsQueryable();

        if (request.ActivityUuid.HasValue)
        {
            query = query.Where(ar => ar.Activity.ActivityUuid == request.ActivityUuid.Value);
        }

        var resources = await query
            .OrderBy(ar => ar.DisplayOrder).ThenBy(ar => ar.ResourceName)
            .Select(ar => new ActivityResourceDto
            {
                ResourceUuid = ar.ResourceUuid,
                ActivityUuid = ar.Activity.ActivityUuid,
                ActivityName = ar.Activity.ActivityName,
                ResourceName = ar.ResourceName,
                ResourceType = ar.ResourceType,
                MediaFileUuid = ar.MediaFile.FileUuid,
                MediaFileName = ar.MediaFile.OriginalName,
                DisplayOrder = ar.DisplayOrder,
                IsRequired = ar.IsRequired,
                UsageContext = ar.UsageContext,
                ResourceConfig = ar.ResourceConfig,
                IsPublished = ar.IsPublished,
                CreatedAt = ar.CreatedAt,
                UpdatedAt = ar.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetActivityResourcesResponse(resources);
    }
}

public class UpdateActivityResourceHandler : IUpdateActivityResourceHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateActivityResourceHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateActivityResourceResponse> Handle(UpdateActivityResourceRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var resource = await _context.ActivityResources
            .Include(ar => ar.Activity)
            .Include(ar => ar.MediaFile)
            .Where(ar => ar.ResourceUuid == request.ResourceUuid)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"ActivityResource with UUID {request.ResourceUuid} not found");

        if (!string.IsNullOrWhiteSpace(request.Resource.ResourceName))
            resource.ResourceName = request.Resource.ResourceName;

        if (!string.IsNullOrWhiteSpace(request.Resource.ResourceType))
            resource.ResourceType = request.Resource.ResourceType;

        if (request.Resource.DisplayOrder.HasValue)
            resource.DisplayOrder = request.Resource.DisplayOrder;

        if (request.Resource.IsRequired.HasValue)
            resource.IsRequired = request.Resource.IsRequired.Value;

        if (request.Resource.UsageContext != null)
            resource.UsageContext = request.Resource.UsageContext;

        if (request.Resource.ResourceConfig != null)
            resource.ResourceConfig = request.Resource.ResourceConfig;

        if (request.Resource.IsPublished.HasValue)
            resource.IsPublished = request.Resource.IsPublished.Value;

        resource.UpdatedAt = DateTimeOffset.UtcNow;
        resource.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        var dto = new ActivityResourceDto
        {
            ResourceUuid = resource.ResourceUuid,
            ActivityUuid = resource.Activity.ActivityUuid,
            ActivityName = resource.Activity.ActivityName,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            MediaFileUuid = resource.MediaFile.FileUuid,
            MediaFileName = resource.MediaFile.OriginalName,
            DisplayOrder = resource.DisplayOrder,
            IsRequired = resource.IsRequired,
            UsageContext = resource.UsageContext,
            ResourceConfig = resource.ResourceConfig,
            IsPublished = resource.IsPublished,
            CreatedAt = resource.CreatedAt,
            UpdatedAt = resource.UpdatedAt
        };

        return new UpdateActivityResourceResponse(dto);
    }
}
