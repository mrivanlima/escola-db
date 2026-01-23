using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateTeacherSubjectValidator : AbstractValidator<CreateTeacherSubjectDto>
{
    public CreateTeacherSubjectValidator()
    {
        RuleFor(x => x.TeacherUuid)
            .NotEmpty().WithMessage("Teacher UUID is required");

        RuleFor(x => x.SubjectUuid)
            .NotEmpty().WithMessage("Subject UUID is required");

        RuleFor(x => x.YearsExperience)
            .GreaterThanOrEqualTo(0).When(x => x.YearsExperience.HasValue)
            .WithMessage("Years of experience must be 0 or greater");
    }
}

public class UpdateTeacherSubjectValidator : AbstractValidator<UpdateTeacherSubjectDto>
{
    public UpdateTeacherSubjectValidator()
    {
        RuleFor(x => x.YearsExperience)
            .GreaterThanOrEqualTo(0).When(x => x.YearsExperience.HasValue)
            .WithMessage("Years of experience must be 0 or greater");
    }
}
