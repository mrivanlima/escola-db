using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class UpdateGuardianValidator : AbstractValidator<UpdateGuardianDto>
{
    public UpdateGuardianValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
