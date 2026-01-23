using FluentValidation;

namespace Escola.Application.UseCases.Students.CreateStudent;

/// <summary>
/// Validator for CreateStudentRequest.
/// </summary>
public class CreateStudentValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Birth date cannot be in the future");

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage("Valid tenant ID is required");

        RuleFor(x => x.Nickname)
            .MaximumLength(100)
            .WithMessage("Nickname cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Nickname));

        RuleFor(x => x.MiddleName)
            .MaximumLength(100)
            .WithMessage("Middle name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));
    }
}
