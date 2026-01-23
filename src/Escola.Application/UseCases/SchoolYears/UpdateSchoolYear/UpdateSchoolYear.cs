using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.SchoolYears.UpdateSchoolYear;

public class UpdateSchoolYearRequest
{
    public Guid SchoolYearUuid { get; set; }
    public UpdateSchoolYearDto SchoolYear { get; set; } = null!;
}

public class UpdateSchoolYearResponse
{
    public SchoolYearDto SchoolYear { get; set; } = null!;
}

public interface IUpdateSchoolYearHandler
{
    Task<UpdateSchoolYearResponse> HandleAsync(UpdateSchoolYearRequest request, CancellationToken cancellationToken = default);
}
