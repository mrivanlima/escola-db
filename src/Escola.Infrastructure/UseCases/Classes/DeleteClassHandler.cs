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

    public DeleteClassHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Guid classUuid, CancellationToken cancellationToken)
    {
        var classEntity = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassUuid == classUuid, cancellationToken);

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
