using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.SchoolYears.CreateSchoolYear;

public class CreateSchoolYearRequest
{
    public CreateSchoolYearDto SchoolYear { get; set; } = null!;
}

public class CreateSchoolYearResponse
{
    public SchoolYearDto SchoolYear { get; set; } = null!;
}

public interface ICreateSchoolYearHandler
{
    Task<CreateSchoolYearResponse> HandleAsync(CreateSchoolYearRequest request, CancellationToken cancellationToken = default);
}
