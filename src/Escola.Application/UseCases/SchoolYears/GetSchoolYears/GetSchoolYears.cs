using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.SchoolYears.GetSchoolYears;

public class GetSchoolYearsRequest
{
    public Guid? TenantUuid { get; set; }
}

public class GetSchoolYearsResponse
{
    public List<SchoolYearDto> SchoolYears { get; set; } = new();
}

public interface IGetSchoolYearsHandler
{
    Task<GetSchoolYearsResponse> HandleAsync(GetSchoolYearsRequest request, CancellationToken cancellationToken = default);
}
