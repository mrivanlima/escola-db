using FluentValidation;

namespace Escola.Application.UseCases.Teachers.CreateTeacher;

/// <summary>
/// Validator for CreateTeacherRequest.
/// </summary>
public class CreateTeacherValidator : AbstractValidator<CreateTeacherRequest>
{
    public CreateTeacherValidator()
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage("TenantId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");
    }
}
