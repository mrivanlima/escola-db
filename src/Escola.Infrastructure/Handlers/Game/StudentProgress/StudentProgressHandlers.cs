using Escola.Application.Game.StudentProgress;
using Escola.Application.Services;
using Escola.Domain.Game;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.Handlers.Game.StudentProgress;

public class CreateStudentProgressHandler : ICreateStudentProgressHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateStudentProgressHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateStudentProgressResponse> Handle(CreateStudentProgressRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");
        var currentUserId = _currentUserService.GetCurrentUserId();

        var student = await _context.Students
            .Where(s => s.StudentUuid == request.Progress.StudentUuid && s.TenantId == currentTenantId && s.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Student not found with UUID {request.Progress.StudentUuid}");

        var activity = await _context.Activities
            .Where(a => a.ActivityUuid == request.Progress.ActivityUuid && a.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Activity not found with UUID {request.Progress.ActivityUuid}");

        var statusCodeNormalized = request.Progress.Status.ToLowerInvariant();
        var status = await _context.ProgressStatuses
            .Where(ps => ps.StatusCodeNormalized == statusCodeNormalized && ps.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Progress status not found with code {request.Progress.Status}");

        var progress = new Domain.Game.StudentProgress
        {
            ProgressUuid = Guid.NewGuid(),
            StudentId = student.StudentId,
            ActivityId = activity.ActivityId,
            TenantId = currentTenantId,
            StatusId = status.StatusId,
            Score = request.Progress.Score,
            AttemptsCount = request.Progress.Attempts,
            TimeSpentSeconds = request.Progress.TimeSpentSeconds,
            ProgressData = request.Progress.ProgressData,
            CompletionDate = request.Progress.CompletedAt.HasValue ? request.Progress.CompletedAt.Value.ToDateTime(TimeOnly.MinValue) : null,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.StudentProgress.Add(progress);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context.StudentProgress
            .Where(p => p.ProgressId == progress.ProgressId)
            .Include(p => p.Student)
            .Include(p => p.Activity)
            .Include(p => p.Tenant)
            .Include(p => p.ProgressStatus)
            .Select(p => new StudentProgressDto
            {
                ProgressUuid = p.ProgressUuid,
                StudentUuid = p.Student.StudentUuid,
                StudentName = p.Student.FirstName + " " + p.Student.LastName,
                ActivityUuid = p.Activity.ActivityUuid,
                ActivityName = p.Activity.ActivityName,
                TenantUuid = p.Tenant.TenantUuid,
                Status = p.ProgressStatus.StatusCode,
                Score = p.Score,
                Attempts = p.AttemptsCount,
                TimeSpentSeconds = p.TimeSpentSeconds,
                ProgressData = p.ProgressData,
                CompletedAt = p.CompletionDate.HasValue ? DateOnly.FromDateTime(p.CompletionDate.Value.DateTime) : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstAsync(cancellationToken);

        return new CreateStudentProgressResponse(dto);
    }
}

public class GetStudentProgressHandler : IGetStudentProgressHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentProgressHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetStudentProgressResponse> Handle(GetStudentProgressRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");

        var dto = await _context.StudentProgress
            .Where(p => p.ProgressUuid == request.ProgressUuid && p.TenantId == currentTenantId && p.DeletedAt == null)
            .Include(p => p.Student)
            .Include(p => p.Activity)
            .Include(p => p.Tenant)
            .Include(p => p.ProgressStatus)
            .Select(p => new StudentProgressDto
            {
                ProgressUuid = p.ProgressUuid,
                StudentUuid = p.Student.StudentUuid,
                StudentName = p.Student.FirstName + " " + p.Student.LastName,
                ActivityUuid = p.Activity.ActivityUuid,
                ActivityName = p.Activity.ActivityName,
                TenantUuid = p.Tenant.TenantUuid,
                Status = p.ProgressStatus.StatusCode,
                Score = p.Score,
                Attempts = p.AttemptsCount,
                TimeSpentSeconds = p.TimeSpentSeconds,
                ProgressData = p.ProgressData,
                CompletedAt = p.CompletionDate.HasValue ? DateOnly.FromDateTime(p.CompletionDate.Value.DateTime) : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"StudentProgress not found with UUID {request.ProgressUuid}");

        return new GetStudentProgressResponse(dto);
    }
}

public class GetStudentProgressListHandler : IGetStudentProgressListHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentProgressListHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetStudentProgressListResponse> Handle(GetStudentProgressListRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");

        var query = _context.StudentProgress
            .Where(p => p.TenantId == currentTenantId && p.DeletedAt == null)
            .Include(p => p.Student)
            .Include(p => p.Activity)
            .Include(p => p.Tenant)
            .AsQueryable();

        if (request.StudentUuid.HasValue)
        {
            query = query.Where(p => p.Student.StudentUuid == request.StudentUuid.Value);
        }

        if (request.ActivityUuid.HasValue)
        {
            query = query.Where(p => p.Activity.ActivityUuid == request.ActivityUuid.Value);
        }

        query = query.Include(p => p.ProgressStatus);

        var dtos = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new StudentProgressDto
            {
                ProgressUuid = p.ProgressUuid,
                StudentUuid = p.Student.StudentUuid,
                StudentName = p.Student.FirstName + " " + p.Student.LastName,
                ActivityUuid = p.Activity.ActivityUuid,
                ActivityName = p.Activity.ActivityName,
                TenantUuid = p.Tenant.TenantUuid,
                Status = p.ProgressStatus.StatusCode,
                Score = p.Score,
                Attempts = p.AttemptsCount,
                TimeSpentSeconds = p.TimeSpentSeconds,
                ProgressData = p.ProgressData,
                CompletedAt = p.CompletionDate.HasValue ? DateOnly.FromDateTime(p.CompletionDate.Value.DateTime) : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new GetStudentProgressListResponse(dtos);
    }
}

public class UpdateStudentProgressHandler : IUpdateStudentProgressHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateStudentProgressHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateStudentProgressResponse> Handle(UpdateStudentProgressRequest request, CancellationToken cancellationToken)
    {
        var currentTenantId = _currentUserService.GetCurrentTenantId()
            ?? throw new InvalidOperationException("Tenant ID is required");
        var currentUserId = _currentUserService.GetCurrentUserId();

        var progress = await _context.StudentProgress
            .Where(p => p.ProgressUuid == request.ProgressUuid && p.TenantId == currentTenantId && p.DeletedAt == null)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"StudentProgress not found with UUID {request.ProgressUuid}");

        if (!string.IsNullOrWhiteSpace(request.Progress.Status))
        {
            var statusCodeNormalized = request.Progress.Status.ToLowerInvariant();
            var status = await _context.ProgressStatuses
                .Where(ps => ps.StatusCodeNormalized == statusCodeNormalized && ps.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new InvalidOperationException($"Progress status not found with code {request.Progress.Status}");
            progress.StatusId = status.StatusId;
        }

        if (request.Progress.Score.HasValue)
        {
            progress.Score = request.Progress.Score.Value;
        }

        if (request.Progress.Attempts.HasValue)
        {
            progress.AttemptsCount = request.Progress.Attempts.Value;
        }

        if (request.Progress.TimeSpentSeconds.HasValue)
        {
            progress.TimeSpentSeconds = request.Progress.TimeSpentSeconds.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Progress.ProgressData))
        {
            progress.ProgressData = request.Progress.ProgressData;
        }

        if (request.Progress.CompletedAt.HasValue)
        {
            progress.CompletionDate = request.Progress.CompletedAt.Value.ToDateTime(TimeOnly.MinValue);
        }

        progress.UpdatedAt = DateTimeOffset.UtcNow;
        progress.UpdatedBy = currentUserId;

        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context.StudentProgress
            .Where(p => p.ProgressId == progress.ProgressId)
            .Include(p => p.Student)
            .Include(p => p.Activity)
            .Include(p => p.Tenant)
            .Include(p => p.ProgressStatus)
            .Select(p => new StudentProgressDto
            {
                ProgressUuid = p.ProgressUuid,
                StudentUuid = p.Student.StudentUuid,
                StudentName = p.Student.FirstName + " " + p.Student.LastName,
                ActivityUuid = p.Activity.ActivityUuid,
                ActivityName = p.Activity.ActivityName,
                TenantUuid = p.Tenant.TenantUuid,
                Status = p.ProgressStatus.StatusCode,
                Score = p.Score,
                Attempts = p.AttemptsCount,
                TimeSpentSeconds = p.TimeSpentSeconds,
                ProgressData = p.ProgressData,
                CompletedAt = p.CompletionDate.HasValue ? DateOnly.FromDateTime(p.CompletionDate.Value.DateTime) : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstAsync(cancellationToken);

        return new UpdateStudentProgressResponse(dto);
    }
}
