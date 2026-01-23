using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Classes.GetClasses;

public class GetClassesRequest
{
    public Guid? TenantUuid { get; set; }
    public Guid? SchoolYearUuid { get; set; }
}

public class GetClassesResponse
{
    public List<ClassDto> Classes { get; set; } = new();
}

public interface IGetClassesHandler
{
    Task<GetClassesResponse> HandleAsync(GetClassesRequest request, CancellationToken cancellationToken = default);
}
