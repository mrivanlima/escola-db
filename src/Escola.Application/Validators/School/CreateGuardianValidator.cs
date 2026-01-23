using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateGuardianValidator : AbstractValidator<CreateGuardianDto>
{
    public CreateGuardianValidator()
    {
        RuleFor(x => x.TenantUuid)
            .NotEmpty()
            .WithMessage("Tenant UUID is required.");

        RuleFor(x => x.UserUuid)
            .NotEmpty()
            .WithMessage("User UUID is required.");

        RuleFor(x => x.RelationshipTypeUuid)
            .NotEmpty()
            .WithMessage("Relationship type UUID is required.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
