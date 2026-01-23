namespace Escola.Application.UseCases.Classes.GetClasses;

/// <summary>
/// Handler interface for retrieving all classes.
/// </summary>
public interface IGetClassesHandler
{
    /// <summary>
    /// Retrieves all classes from the system.
    /// </summary>
    /// <param name="request">The request containing query parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of classes.</returns>
    Task<List<ClassResponse>> Handle(GetClassesRequest request, CancellationToken cancellationToken);
}
