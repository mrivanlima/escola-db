using FluentValidation;

namespace Escola.Application.UseCases.Classes.CreateClass;

/// <summary>
/// Validator for CreateClassRequest.
/// </summary>
public class CreateClassValidator : AbstractValidator<CreateClassRequest>
{
    public CreateClassValidator()
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage("TenantId must be greater than 0");

        RuleFor(x => x.ClassName)
            .NotEmpty()
            .WithMessage("ClassName is required")
            .MinimumLength(2)
            .WithMessage("ClassName must be at least 2 characters");

        RuleFor(x => x.SchoolYear)
            .NotEmpty()
            .WithMessage("SchoolYear is required");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0)
            .When(x => x.MaxStudents.HasValue)
            .WithMessage("MaxStudents must be greater than 0");
    }
}
