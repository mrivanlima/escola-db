using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatuses;

public class GetEnrollmentStatusesRequest
{
    public Guid? TenantUuid { get; set; }
}

public class GetEnrollmentStatusesResponse
{
    public List<EnrollmentStatusDto> EnrollmentStatuses { get; set; } = new();
}

public interface IGetEnrollmentStatusesHandler
{
    Task<GetEnrollmentStatusesResponse> HandleAsync(
        GetEnrollmentStatusesRequest request,
        CancellationToken cancellationToken = default);
}
