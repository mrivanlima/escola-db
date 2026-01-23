using Escola.Application.Game.StudentBadges;
using Escola.Application.Services;
using Escola.Domain.Game;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.Handlers.Game.StudentBadges;

public class AwardStudentBadgeHandler : IAwardStudentBadgeHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AwardStudentBadgeHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<AwardStudentBadgeResponse> Handle(AwardStudentBadgeRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");

        var student = await _context.Students
            .Where(s => s.StudentUuid == request.Badge.StudentUuid && s.TenantId == currentTenantId && s.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Student not found with UUID {request.Badge.StudentUuid}");

        var badge = await _context.Badges
            .Where(b => b.BadgeUuid == request.Badge.BadgeUuid && b.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Badge not found with UUID {request.Badge.BadgeUuid}");

        // Check if student already has this badge
        var existing = await _context.StudentBadges
            .Where(sb => sb.StudentId == student.StudentId && sb.BadgeId == badge.BadgeId && sb.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing != null)
        {
            throw new InvalidOperationException($"Student already has this badge");
        }

        var studentBadge = new StudentBadge
        {
            StudentId = student.StudentId,
            BadgeId = badge.BadgeId,
            TenantId = currentTenantId,
            EarnedAt = request.Badge.EarnedAt ?? DateOnly.FromDateTime(DateTime.UtcNow),
            EarnMetadata = request.Badge.EarnMetadata,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.StudentBadges.Add(studentBadge);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context.StudentBadges
            .Where(sb => sb.StudentBadgeId == studentBadge.StudentBadgeId)
            .Include(sb => sb.Student)
            .Include(sb => sb.Badge)
            .Include(sb => sb.Tenant)
            .Select(sb => new StudentBadgeDto
            {
                StudentUuid = sb.Student.StudentUuid,
                StudentName = sb.Student.FirstName + " " + sb.Student.LastName,
                BadgeUuid = sb.Badge.BadgeUuid,
                BadgeName = sb.Badge.BadgeName,
                BadgeIconUrl = sb.Badge.IconUrl ?? string.Empty,
                TenantUuid = sb.Tenant.TenantUuid,
                EarnedAt = sb.EarnedAt,
                EarnMetadata = sb.EarnMetadata,
                CreatedAt = sb.CreatedAt
            })
            .FirstAsync(cancellationToken);

        return new AwardStudentBadgeResponse(dto);
    }
}

public class GetStudentBadgesHandler : IGetStudentBadgesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentBadgesHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetStudentBadgesResponse> Handle(GetStudentBadgesRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");

        var query = _context.StudentBadges
            .Where(sb => sb.TenantId == currentTenantId && sb.DeletedAt == null)
            .Include(sb => sb.Student)
            .Include(sb => sb.Badge)
            .Include(sb => sb.Tenant)
            .AsQueryable();

        if (request.StudentUuid.HasValue)
        {
            query = query.Where(sb => sb.Student.StudentUuid == request.StudentUuid.Value);
        }

        var dtos = await query
            .OrderByDescending(sb => sb.EarnedAt)
            .Select(sb => new StudentBadgeDto
            {
                StudentUuid = sb.Student.StudentUuid,
                StudentName = sb.Student.FirstName + " " + sb.Student.LastName,
                BadgeUuid = sb.Badge.BadgeUuid,
                BadgeName = sb.Badge.BadgeName,
                BadgeIconUrl = sb.Badge.IconUrl ?? string.Empty,
                TenantUuid = sb.Tenant.TenantUuid,
                EarnedAt = sb.EarnedAt,
                EarnMetadata = sb.EarnMetadata,
                CreatedAt = sb.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetStudentBadgesResponse(dtos);
    }
}
