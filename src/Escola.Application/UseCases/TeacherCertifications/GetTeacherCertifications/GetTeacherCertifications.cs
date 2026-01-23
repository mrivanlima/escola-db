using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherCertifications.GetTeacherCertifications;

public record GetTeacherCertificationsRequest
{
    public Guid? TeacherUuid { get; init; }
    public Guid? CertificationUuid { get; init; }
}

public record GetTeacherCertificationsResponse
{
    public List<TeacherCertificationDto> TeacherCertifications { get; init; } = new();
}

public interface IGetTeacherCertificationsHandler
{
    Task<GetTeacherCertificationsResponse> HandleAsync(GetTeacherCertificationsRequest request, CancellationToken cancellationToken);
}
