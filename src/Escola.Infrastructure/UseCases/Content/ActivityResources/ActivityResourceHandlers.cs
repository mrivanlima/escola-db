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

        // Resolve ResourceType code to FK
        short? resourceTypeId = null;
        if (!string.IsNullOrWhiteSpace(request.Resource.ResourceType))
        {
            var typeCodeNormalized = request.Resource.ResourceType.ToLowerInvariant();
            var resourceType = await _context.ResourceTypes
                .Where(rt => rt.ResourceTypeCodeNormalized == typeCodeNormalized && rt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            resourceTypeId = resourceType?.ResourceTypeId;
        }

        // Resolve UsageContext code to FK
        short? usageContextId = null;
        if (!string.IsNullOrWhiteSpace(request.Resource.UsageContext))
        {
            var contextCodeNormalized = request.Resource.UsageContext.ToLowerInvariant();
            var usageContext = await _context.UsageContexts
                .Where(uc => uc.ContextCodeNormalized == contextCodeNormalized && uc.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            usageContextId = usageContext?.UsageContextId;
        }

        var resource = new ActivityResource
        {
            ResourceUuid = Guid.NewGuid(),
            ActivityId = activity.ActivityId,
            ResourceName = request.Resource.ResourceName,
            ResourceTypeId = resourceTypeId,
            MediaFileId = mediaFile.FileId,
            DisplayOrder = request.Resource.DisplayOrder,
            IsRequired = request.Resource.IsRequired,
            UsageContextId = usageContextId,
            ResourceConfig = request.Resource.ResourceConfig,
            IsPublished = request.Resource.IsPublished,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userId
        };

        _context.ActivityResources.Add(resource);
        await _context.SaveChangesAsync(cancellationToken);

        // Re-query with ResourceType and UsageContext joins
        var resourceWithLookups = await _context.ActivityResources
            .Include(ar => ar.ResourceType)
            .Include(ar => ar.UsageContext)
            .Where(ar => ar.ResourceId == resource.ResourceId)
            .FirstOrDefaultAsync(cancellationToken);

        var dto = new ActivityResourceDto
        {
            ResourceUuid = resourceWithLookups!.ResourceUuid,
            ActivityUuid = activity.ActivityUuid,
            ActivityName = activity.ActivityName,
            ResourceName = resourceWithLookups.ResourceName,
            ResourceType = resourceWithLookups.ResourceType != null ? resourceWithLookups.ResourceType.ResourceTypeCode : null,
            MediaFileUuid = mediaFile.FileUuid,
            MediaFileName = mediaFile.OriginalName,
            DisplayOrder = resourceWithLookups.DisplayOrder,
            IsRequired = resourceWithLookups.IsRequired,
            UsageContext = resourceWithLookups.UsageContext != null ? resourceWithLookups.UsageContext.ContextCode : null,
            ResourceConfig = resourceWithLookups.ResourceConfig,
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
            .Include(ar => ar.ResourceType)
            .Include(ar => ar.UsageContext)
            .Where(ar => ar.ResourceUuid == request.ResourceUuid)
            .Select(ar => new ActivityResourceDto
            {
                ResourceUuid = ar.ResourceUuid,
                ActivityUuid = ar.Activity.ActivityUuid,
                ActivityName = ar.Activity.ActivityName,
                ResourceName = ar.ResourceName,
                ResourceType = ar.ResourceType != null ? ar.ResourceType.ResourceTypeCode : null,
                MediaFileUuid = ar.MediaFile.FileUuid,
                MediaFileName = ar.MediaFile.OriginalName,
                DisplayOrder = ar.DisplayOrder,
                IsRequired = ar.IsRequired,
                UsageContext = ar.UsageContext != null ? ar.UsageContext.ContextCode : null,
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
            .Include(ar => ar.ResourceType)
            .Include(ar => ar.UsageContext)
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
                ResourceType = ar.ResourceType != null ? ar.ResourceType.ResourceTypeCode : null,
                MediaFileUuid = ar.MediaFile.FileUuid,
                MediaFileName = ar.MediaFile.OriginalName,
                DisplayOrder = ar.DisplayOrder,
                IsRequired = ar.IsRequired,
                UsageContext = ar.UsageContext != null ? ar.UsageContext.ContextCode : null,
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
        {
            var typeCodeNormalized = request.Resource.ResourceType.ToLowerInvariant();
            var resourceType = await _context.ResourceTypes
                .Where(rt => rt.ResourceTypeCodeNormalized == typeCodeNormalized && rt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            resource.ResourceTypeId = resourceType?.ResourceTypeId;
        }

        if (request.Resource.DisplayOrder.HasValue)
            resource.DisplayOrder = request.Resource.DisplayOrder;

        if (request.Resource.IsRequired.HasValue)
            resource.IsRequired = request.Resource.IsRequired.Value;

        if (request.Resource.UsageContext != null)
        {
            var contextCodeNormalized = request.Resource.UsageContext.ToLowerInvariant();
            var usageContext = await _context.UsageContexts
                .Where(uc => uc.ContextCodeNormalized == contextCodeNormalized && uc.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            resource.UsageContextId = usageContext?.UsageContextId;
        }

        if (request.Resource.ResourceConfig != null)
            resource.ResourceConfig = request.Resource.ResourceConfig;

        if (request.Resource.IsPublished.HasValue)
            resource.IsPublished = request.Resource.IsPublished.Value;

        resource.UpdatedAt = DateTimeOffset.UtcNow;
        resource.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        // Re-query with ResourceType and UsageContext joins
        var resourceWithLookups = await _context.ActivityResources
            .Include(ar => ar.Activity)
            .Include(ar => ar.MediaFile)
            .Include(ar => ar.ResourceType)
            .Include(ar => ar.UsageContext)
            .Where(ar => ar.ResourceId == resource.ResourceId)
            .FirstOrDefaultAsync(cancellationToken);

        var dto = new ActivityResourceDto
        {
            ResourceUuid = resourceWithLookups!.ResourceUuid,
            ActivityUuid = resourceWithLookups.Activity.ActivityUuid,
            ActivityName = resourceWithLookups.Activity.ActivityName,
            ResourceName = resourceWithLookups.ResourceName,
            ResourceType = resourceWithLookups.ResourceType != null ? resourceWithLookups.ResourceType.ResourceTypeCode : null,
            MediaFileUuid = resourceWithLookups.MediaFile.FileUuid,
            MediaFileName = resourceWithLookups.MediaFile.OriginalName,
            DisplayOrder = resourceWithLookups.DisplayOrder,
            IsRequired = resourceWithLookups.IsRequired,
            UsageContext = resourceWithLookups.UsageContext != null ? resourceWithLookups.UsageContext.ContextCode : null,
            ResourceConfig = resourceWithLookups.ResourceConfig,
            IsPublished = resource.IsPublished,
            CreatedAt = resource.CreatedAt,
            UpdatedAt = resource.UpdatedAt
        };

        return new UpdateActivityResourceResponse(dto);
    }
}
