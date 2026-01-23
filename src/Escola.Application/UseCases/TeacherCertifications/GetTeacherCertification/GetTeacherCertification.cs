using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherCertifications.GetTeacherCertification;

public record GetTeacherCertificationRequest
{
    public Guid TeacherUuid { get; init; }
    public Guid CertificationUuid { get; init; }
}

public record GetTeacherCertificationResponse
{
    public TeacherCertificationDto TeacherCertification { get; init; } = null!;
}

public interface IGetTeacherCertificationHandler
{
    Task<GetTeacherCertificationResponse> HandleAsync(GetTeacherCertificationRequest request, CancellationToken cancellationToken);
}
