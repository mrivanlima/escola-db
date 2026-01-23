using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherCertifications.CreateTeacherCertification;

public record CreateTeacherCertificationRequest
{
    public CreateTeacherCertificationDto TeacherCertification { get; init; } = null!;
}

public record CreateTeacherCertificationResponse
{
    public TeacherCertificationDto TeacherCertification { get; init; } = null!;
}

public interface ICreateTeacherCertificationHandler
{
    Task<CreateTeacherCertificationResponse> HandleAsync(CreateTeacherCertificationRequest request, CancellationToken cancellationToken);
}
