using Escola.Application.Game.Badges;
using Escola.Application.Services;
using Escola.Domain.Game;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.Handlers.Game.Badges;

public class CreateBadgeHandler : ICreateBadgeHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateBadgeHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateBadgeResponse> Handle(CreateBadgeRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetCurrentUserId();

        // Resolve BadgeType FK if provided
        short? badgeTypeId = null;
        if (!string.IsNullOrWhiteSpace(request.Badge.BadgeType))
        {
            var typeCodeNormalized = request.Badge.BadgeType.ToLowerInvariant();
            var badgeType = await _context.BadgeTypes
                .Where(bt => bt.TypeCodeNormalized == typeCodeNormalized && bt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            badgeTypeId = badgeType?.BadgeTypeId;
        }

        // Resolve Rarity FK if provided
        short? rarityId = null;
        if (!string.IsNullOrWhiteSpace(request.Badge.Rarity))
        {
            var rarityCodeNormalized = request.Badge.Rarity.ToLowerInvariant();
            var rarity = await _context.BadgeRarities
                .Where(br => br.RarityCodeNormalized == rarityCodeNormalized && br.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            rarityId = rarity?.RarityId;
        }

        var badge = new Badge
        {
            BadgeUuid = Guid.NewGuid(),
            BadgeName = request.Badge.BadgeName,
            BadgeNameNormalized = request.Badge.BadgeName.ToUpperInvariant(),
            Description = request.Badge.Description,
            DescriptionNormalized = request.Badge.Description?.ToUpperInvariant(),
            BadgeTypeId = badgeTypeId ?? (short)0,
            IconUrl = request.Badge.IconUrl,
            RarityId = rarityId ?? (short)0,
            PointsValue = request.Badge.PointsRequired ?? 0,
            UnlockCriteria = request.Badge.Criteria ?? "{}",
            DisplayOrder = request.Badge.DisplayOrder ?? 0,
            IsActive = request.Badge.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = currentUserId
        };

        _context.Badges.Add(badge);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context.Badges
            .Where(b => b.BadgeId == badge.BadgeId)
            .Include(b => b.BadgeType)
            .Include(b => b.BadgeRarity)
            .Select(b => new BadgeDto
            {
                BadgeUuid = b.BadgeUuid,
                BadgeName = b.BadgeName,
                Description = b.Description,
                BadgeType = b.BadgeType != null ? b.BadgeType.TypeCode : null,
                IconUrl = b.IconUrl,
                Rarity = b.BadgeRarity != null ? b.BadgeRarity.RarityCode : null,
                PointsRequired = b.PointsValue,
                Criteria = b.UnlockCriteria,
                DisplayOrder = b.DisplayOrder,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstAsync(cancellationToken);

        return new CreateBadgeResponse(dto);
    }
}

public class GetBadgeHandler : IGetBadgeHandler
{
    private readonly EscolaDbContext _context;

    public GetBadgeHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetBadgeResponse> Handle(GetBadgeRequest request, CancellationToken cancellationToken)
    {
        var dto = await _context.Badges
            .Where(b => b.BadgeUuid == request.BadgeUuid && b.DeletedAt == null)
            .Include(b => b.BadgeType)
            .Include(b => b.BadgeRarity)
            .Select(b => new BadgeDto
            {
                BadgeUuid = b.BadgeUuid,
                BadgeName = b.BadgeName,
                Description = b.Description,
                BadgeType = b.BadgeType != null ? b.BadgeType.TypeCode : null,
                IconUrl = b.IconUrl,
                Rarity = b.BadgeRarity != null ? b.BadgeRarity.RarityCode : null,
                PointsRequired = b.PointsValue,
                Criteria = b.UnlockCriteria,
                DisplayOrder = b.DisplayOrder,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Badge not found with UUID {request.BadgeUuid}");

        return new GetBadgeResponse(dto);
    }
}

public class GetBadgesHandler : IGetBadgesHandler
{
    private readonly EscolaDbContext _context;

    public GetBadgesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetBadgesResponse> Handle(GetBadgesRequest request, CancellationToken cancellationToken)
    {
        var dtos = await _context.Badges
            .Where(b => b.DeletedAt == null)
            .Include(b => b.BadgeType)
            .Include(b => b.BadgeRarity)
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.BadgeName)
            .Select(b => new BadgeDto
            {
                BadgeUuid = b.BadgeUuid,
                BadgeName = b.BadgeName,
                Description = b.Description,
                BadgeType = b.BadgeType != null ? b.BadgeType.TypeCode : null,
                IconUrl = b.IconUrl,
                Rarity = b.BadgeRarity != null ? b.BadgeRarity.RarityCode : null,
                PointsRequired = b.PointsValue,
                Criteria = b.UnlockCriteria,
                DisplayOrder = b.DisplayOrder,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetBadgesResponse(dtos);
    }
}

public class UpdateBadgeHandler : IUpdateBadgeHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBadgeHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateBadgeResponse> Handle(UpdateBadgeRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetCurrentUserId();

        var badge = await _context.Badges
            .Where(b => b.BadgeUuid == request.BadgeUuid && b.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Badge not found with UUID {request.BadgeUuid}");

        if (!string.IsNullOrWhiteSpace(request.Badge.BadgeName))
        {
            badge.BadgeName = request.Badge.BadgeName;
            badge.BadgeNameNormalized = request.Badge.BadgeName.ToUpperInvariant();
        }

        if (!string.IsNullOrWhiteSpace(request.Badge.Description))
        {
            badge.Description = request.Badge.Description;
            badge.DescriptionNormalized = request.Badge.Description.ToUpperInvariant();
        }

        if (!string.IsNullOrWhiteSpace(request.Badge.BadgeType))
        {
            var typeCodeNormalized = request.Badge.BadgeType.ToLowerInvariant();
            var badgeType = await _context.BadgeTypes
                .Where(bt => bt.TypeCodeNormalized == typeCodeNormalized && bt.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            if (badgeType != null)
                badge.BadgeTypeId = badgeType.BadgeTypeId;
        }

        if (!string.IsNullOrWhiteSpace(request.Badge.IconUrl))
        {
            badge.IconUrl = request.Badge.IconUrl;
        }

        if (!string.IsNullOrWhiteSpace(request.Badge.Rarity))
        {
            var rarityCodeNormalized = request.Badge.Rarity.ToLowerInvariant();
            var rarity = await _context.BadgeRarities
                .Where(br => br.RarityCodeNormalized == rarityCodeNormalized && br.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
            if (rarity != null)
                badge.RarityId = rarity.RarityId;
        }

        if (request.Badge.PointsRequired.HasValue)
        {
            badge.PointsValue = request.Badge.PointsRequired.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Badge.Criteria))
        {
            badge.UnlockCriteria = request.Badge.Criteria;
        }

        if (request.Badge.DisplayOrder.HasValue)
        {
            badge.DisplayOrder = request.Badge.DisplayOrder.Value;
        }

        if (request.Badge.IsActive.HasValue)
        {
            badge.IsActive = request.Badge.IsActive.Value;
        }

        badge.UpdatedAt = DateTimeOffset.UtcNow;
        badge.UpdatedBy = currentUserId;

        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context.Badges
            .Where(b => b.BadgeId == badge.BadgeId)
            .Include(b => b.BadgeType)
            .Include(b => b.BadgeRarity)
            .Select(b => new BadgeDto
            {
                BadgeUuid = b.BadgeUuid,
                BadgeName = b.BadgeName,
                Description = b.Description,
                BadgeType = b.BadgeType != null ? b.BadgeType.TypeCode : null,
                IconUrl = b.IconUrl,
                Rarity = b.BadgeRarity != null ? b.BadgeRarity.RarityCode : null,
                PointsRequired = b.PointsValue,
                Criteria = b.UnlockCriteria,
                DisplayOrder = b.DisplayOrder,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstAsync(cancellationToken);

        return new UpdateBadgeResponse(dto);
    }
}
