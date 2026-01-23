using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Classes.CreateClass;

public class CreateClassRequest
{
    public CreateClassDto Class { get; set; } = null!;
}

public class CreateClassResponse
{
    public ClassDto Class { get; set; } = null!;
}

public interface ICreateClassHandler
{
    Task<CreateClassResponse> HandleAsync(CreateClassRequest request, CancellationToken cancellationToken = default);
}
