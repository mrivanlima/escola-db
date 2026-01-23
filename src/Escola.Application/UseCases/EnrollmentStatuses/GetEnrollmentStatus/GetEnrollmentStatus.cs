using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatus;

public class GetEnrollmentStatusRequest
{
    public Guid EnrollmentStatusUuid { get; set; }
}

public class GetEnrollmentStatusResponse
{
    public EnrollmentStatusDto EnrollmentStatus { get; set; } = null!;
}

public interface IGetEnrollmentStatusHandler
{
    Task<GetEnrollmentStatusResponse> HandleAsync(
        GetEnrollmentStatusRequest request,
        CancellationToken cancellationToken = default);
}
