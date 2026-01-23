using FluentValidation;

namespace Escola.Application.UseCases.Classes.UpdateClass;

/// <summary>
/// Validator for UpdateClassRequest.
/// </summary>
public class UpdateClassValidator : AbstractValidator<UpdateClassRequest>
{
    public UpdateClassValidator()
    {
        RuleFor(x => x.ClassName)
            .MinimumLength(2)
            .When(x => !string.IsNullOrEmpty(x.ClassName))
            .WithMessage("ClassName must be at least 2 characters");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0)
            .When(x => x.MaxStudents.HasValue)
            .WithMessage("MaxStudents must be greater than 0");
    }
}
