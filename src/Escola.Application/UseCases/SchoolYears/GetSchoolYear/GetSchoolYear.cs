using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.SchoolYears.GetSchoolYear;

public class GetSchoolYearRequest
{
    public Guid SchoolYearUuid { get; set; }
}

public class GetSchoolYearResponse
{
    public SchoolYearDto SchoolYear { get; set; } = null!;
}

public interface IGetSchoolYearHandler
{
    Task<GetSchoolYearResponse> HandleAsync(GetSchoolYearRequest request, CancellationToken cancellationToken = default);
}
