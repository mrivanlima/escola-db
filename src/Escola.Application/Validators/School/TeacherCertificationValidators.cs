using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateTeacherCertificationValidator : AbstractValidator<CreateTeacherCertificationDto>
{
    public CreateTeacherCertificationValidator()
    {
        RuleFor(x => x.TeacherUuid)
            .NotEmpty().WithMessage("Teacher UUID is required");

        RuleFor(x => x.CertificationUuid)
            .NotEmpty().WithMessage("Certification UUID is required");

        RuleFor(x => x.ExpiryDate)
            .GreaterThanOrEqualTo(x => x.ObtainedDate)
            .When(x => x.ExpiryDate.HasValue && x.ObtainedDate.HasValue)
            .WithMessage("Expiry date must be after obtained date");
    }
}

public class UpdateTeacherCertificationValidator : AbstractValidator<UpdateTeacherCertificationDto>
{
    public UpdateTeacherCertificationValidator()
    {
        RuleFor(x => x.ExpiryDate)
            .GreaterThanOrEqualTo(x => x.ObtainedDate)
            .When(x => x.ExpiryDate.HasValue && x.ObtainedDate.HasValue)
            .WithMessage("Expiry date must be after obtained date");
    }
}
