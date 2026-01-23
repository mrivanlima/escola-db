using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Classes.UpdateClass;

public class UpdateClassRequest
{
    public Guid ClassUuid { get; set; }
    public UpdateClassDto Class { get; set; } = null!;
}

public class UpdateClassResponse
{
    public ClassDto Class { get; set; } = null!;
}

public interface IUpdateClassHandler
{
    Task<UpdateClassResponse> HandleAsync(UpdateClassRequest request, CancellationToken cancellationToken = default);
}
