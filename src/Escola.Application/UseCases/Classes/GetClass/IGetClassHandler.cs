namespace Escola.Application.UseCases.Classes.GetClass;

/// <summary>
/// Handler interface for retrieving a single class by UUID.
/// </summary>
public interface IGetClassHandler
{
    /// <summary>
    /// Retrieves a class by UUID.
    /// </summary>
    /// <param name="classUuid">The class UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Class information or null if not found.</returns>
    Task<GetClasses.ClassResponse?> Handle(Guid classUuid, CancellationToken cancellationToken);
}
