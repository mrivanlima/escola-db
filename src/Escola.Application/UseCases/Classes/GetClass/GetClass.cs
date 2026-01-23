using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Classes.GetClass;

public class GetClassRequest
{
    public Guid ClassUuid { get; set; }
}

public class GetClassResponse
{
    public ClassDto Class { get; set; } = null!;
}

public interface IGetClassHandler
{
    Task<GetClassResponse> HandleAsync(GetClassRequest request, CancellationToken cancellationToken = default);
}
