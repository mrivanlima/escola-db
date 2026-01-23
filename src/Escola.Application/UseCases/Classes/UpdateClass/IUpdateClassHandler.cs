namespace Escola.Application.UseCases.Classes.UpdateClass;

/// <summary>
/// Handler interface for updating a class.
/// </summary>
public interface IUpdateClassHandler
{
    /// <summary>
    /// Updates an existing class.
    /// </summary>
    /// <param name="classUuid">The class UUID.</param>
    /// <param name="request">The class update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated class information or null if not found.</returns>
    Task<UpdateClassResponse?> Handle(Guid classUuid, UpdateClassRequest request, CancellationToken cancellationToken);
}
