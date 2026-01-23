namespace Escola.Application.UseCases.Classes.DeleteClass;

/// <summary>
/// Handler interface for soft deleting a class.
/// </summary>
public interface IDeleteClassHandler
{
    /// <summary>
    /// Soft deletes a class by UUID.
    /// </summary>
    /// <param name="classUuid">The class UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if deleted, false if not found.</returns>
    Task<bool> Handle(Guid classUuid, CancellationToken cancellationToken);
}
