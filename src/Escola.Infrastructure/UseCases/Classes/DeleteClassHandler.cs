using Escola.Application.Services;
using Escola.Application.UseCases.Classes.DeleteClass;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

/// <summary>
/// Handler for soft deleting a class.
/// </summary>
public class DeleteClassHandler : IDeleteClassHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteClassHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(Guid classUuid, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var classEntity = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassUuid == classUuid && c.TenantId == currentTenantId.Value && c.DeletedAt == null, cancellationToken);

        if (classEntity == null)
        {
            return false;
        }

        // Soft delete
        classEntity.DeletedAt = DateTimeOffset.UtcNow;
        classEntity.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
