using Escola.Application.Content.Activities;
using Escola.Application.Services;
using Escola.Domain.Content;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Content.Activities;

public class CreateActivityHandler : ICreateActivityHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateActivityHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateActivityResponse> Handle(CreateActivityRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var module = await _context.Modules
            .Where(m => m.ModuleUuid == request.Activity.ModuleUuid)
            .Select(m => new { m.ModuleId, m.ModuleUuid, m.ModuleName })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Module with UUID {request.Activity.ModuleUuid} not found");

        // Resolve ActivityType code to FK
        short? activityTypeId = null;
        if (!string.IsNullOrWhiteSpace(request.Activity.ActivityType))
        {
            var typeCodeNormalized = request.Activity.ActivityType.ToLowerInvariant();
            var activityType = await _context.ActivityTypes
                .Where(at => at.ActivityTypeCodeNormalized == typeCodeNormalized && at.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            activityTypeId = activityType?.ActivityTypeId;
        }

        var activity = new Activity
        {
            ActivityUuid = Guid.NewGuid(),
            ModuleId = module.ModuleId,
            ActivityName = request.Activity.ActivityName,
            Description = request.Activity.Description,
            ActivityTypeId = activityTypeId ?? (short)0,
            DisplayOrder = request.Activity.DisplayOrder ?? 0,
            EstimatedDuration = request.Activity.EstimatedDuration,
            PointsReward = request.Activity.PointsReward ?? 0,
            ActivityData = request.Activity.ActivityData ?? "{}",
            ThumbnailUrl = request.Activity.ThumbnailUrl,
            IsPublished = request.Activity.IsPublished,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userId
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync(cancellationToken);

        // Re-query with ActivityType join
        var activityWithType = await _context.Activities
            .Include(a => a.ActivityType)
            .Where(a => a.ActivityId == activity.ActivityId)
            .FirstOrDefaultAsync(cancellationToken);

        var dto = new ActivityDto
        {
            ActivityUuid = activityWithType!.ActivityUuid,
            ModuleUuid = module.ModuleUuid,
            ModuleName = module.ModuleName,
            ActivityName = activityWithType.ActivityName,
            Description = activityWithType.Description,
            ActivityType = activityWithType.ActivityType != null ? activityWithType.ActivityType.ActivityTypeCode : null,
            DisplayOrder = activityWithType.DisplayOrder,
            EstimatedDuration = activity.EstimatedDuration,
            PointsReward = activity.PointsReward,
            ActivityData = activity.ActivityData,
            ThumbnailUrl = activity.ThumbnailUrl,
            IsPublished = activity.IsPublished,
            CreatedAt = activity.CreatedAt,
            UpdatedAt = activity.UpdatedAt
        };

        return new CreateActivityResponse(dto);
    }
}

public class GetActivityHandler : IGetActivityHandler
{
    private readonly EscolaDbContext _context;

    public GetActivityHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _context.Activities
            .Include(a => a.Module)
            .Include(a => a.ActivityType)
            .Where(a => a.ActivityUuid == request.ActivityUuid)
            .Select(a => new ActivityDto
            {
                ActivityUuid = a.ActivityUuid,
                ModuleUuid = a.Module.ModuleUuid,
                ModuleName = a.Module.ModuleName,
                ActivityName = a.ActivityName,
                Description = a.Description,
                ActivityType = a.ActivityType != null ? a.ActivityType.ActivityTypeCode : null,
                DisplayOrder = a.DisplayOrder,
                EstimatedDuration = a.EstimatedDuration,
                PointsReward = a.PointsReward,
                ActivityData = a.ActivityData,
                ThumbnailUrl = a.ThumbnailUrl,
                IsPublished = a.IsPublished,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Activity with UUID {request.ActivityUuid} not found");

        return new GetActivityResponse(activity);
    }
}

public class GetActivitiesHandler : IGetActivitiesHandler
{
    private readonly EscolaDbContext _context;

    public GetActivitiesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Activities.Include(a => a.Module).Include(a => a.ActivityType).AsQueryable();

        if (request.ModuleUuid.HasValue)
        {
            query = query.Where(a => a.Module.ModuleUuid == request.ModuleUuid.Value);
        }

        var activities = await query
            .OrderBy(a => a.DisplayOrder).ThenBy(a => a.ActivityName)
            .Select(a => new ActivityDto
            {
                ActivityUuid = a.ActivityUuid,
                ModuleUuid = a.Module.ModuleUuid,
                ModuleName = a.Module.ModuleName,
                ActivityName = a.ActivityName,
                Description = a.Description,
                ActivityType = a.ActivityType != null ? a.ActivityType.ActivityTypeCode : null,
                DisplayOrder = a.DisplayOrder,
                EstimatedDuration = a.EstimatedDuration,
                PointsReward = a.PointsReward,
                ActivityData = a.ActivityData,
                ThumbnailUrl = a.ThumbnailUrl,
                IsPublished = a.IsPublished,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetActivitiesResponse(activities);
    }
}

public class UpdateActivityHandler : IUpdateActivityHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateActivityHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateActivityResponse> Handle(UpdateActivityRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var activity = await _context.Activities
            .Include(a => a.Module)
            .Where(a => a.ActivityUuid == request.ActivityUuid)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Activity with UUID {request.ActivityUuid} not found");

        if (!string.IsNullOrWhiteSpace(request.Activity.ActivityName))
            activity.ActivityName = request.Activity.ActivityName;

        if (request.Activity.Description != null)
            activity.Description = request.Activity.Description;

        if (request.Activity.ActivityType != null)
        {
            var typeCodeNormalized = request.Activity.ActivityType.ToLowerInvariant();
            var activityType = await _context.ActivityTypes
                .Where(at => at.ActivityTypeCodeNormalized == typeCodeNormalized && at.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            activity.ActivityTypeId = activityType?.ActivityTypeId ?? activity.ActivityTypeId;
        }

        if (request.Activity.DisplayOrder.HasValue)
            activity.DisplayOrder = request.Activity.DisplayOrder.Value;

        if (request.Activity.EstimatedDuration.HasValue)
            activity.EstimatedDuration = request.Activity.EstimatedDuration;

        if (request.Activity.PointsReward.HasValue)
            activity.PointsReward = request.Activity.PointsReward.Value;

        if (request.Activity.ActivityData != null)
            activity.ActivityData = request.Activity.ActivityData;

        if (request.Activity.ThumbnailUrl != null)
            activity.ThumbnailUrl = request.Activity.ThumbnailUrl;

        if (request.Activity.IsPublished.HasValue)
            activity.IsPublished = request.Activity.IsPublished.Value;

        activity.UpdatedAt = DateTimeOffset.UtcNow;
        activity.UpdatedBy = userId;

        await _context.SaveChangesAsync(cancellationToken);

        // Re-query with ActivityType join
        var activityWithType = await _context.Activities
            .Include(a => a.Module)
            .Include(a => a.ActivityType)
            .Where(a => a.ActivityId == activity.ActivityId)
            .FirstOrDefaultAsync(cancellationToken);

        var dto = new ActivityDto
        {
            ActivityUuid = activityWithType!.ActivityUuid,
            ModuleUuid = activityWithType.Module.ModuleUuid,
            ModuleName = activityWithType.Module.ModuleName,
            ActivityName = activityWithType.ActivityName,
            Description = activityWithType.Description,
            ActivityType = activityWithType.ActivityType != null ? activityWithType.ActivityType.ActivityTypeCode : null,
            DisplayOrder = activityWithType.DisplayOrder,
            EstimatedDuration = activity.EstimatedDuration,
            PointsReward = activity.PointsReward,
            ActivityData = activity.ActivityData,
            ThumbnailUrl = activity.ThumbnailUrl,
            IsPublished = activity.IsPublished,
            CreatedAt = activity.CreatedAt,
            UpdatedAt = activity.UpdatedAt
        };

        return new UpdateActivityResponse(dto);
    }
}
