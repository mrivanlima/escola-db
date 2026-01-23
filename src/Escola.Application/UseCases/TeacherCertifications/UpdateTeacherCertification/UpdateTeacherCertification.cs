using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherCertifications.UpdateTeacherCertification;

public record UpdateTeacherCertificationRequest
{
    public Guid TeacherUuid { get; init; }
    public Guid CertificationUuid { get; init; }
    public UpdateTeacherCertificationDto TeacherCertification { get; init; } = null!;
}

public record UpdateTeacherCertificationResponse
{
    public TeacherCertificationDto TeacherCertification { get; init; } = null!;
}

public interface IUpdateTeacherCertificationHandler
{
    Task<UpdateTeacherCertificationResponse> HandleAsync(UpdateTeacherCertificationRequest request, CancellationToken cancellationToken);
}
