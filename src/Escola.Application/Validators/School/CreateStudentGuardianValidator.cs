using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateStudentGuardianValidator : AbstractValidator<CreateStudentGuardianDto>
{
    public CreateStudentGuardianValidator()
    {
        RuleFor(x => x.StudentUuid)
            .NotEmpty()
            .WithMessage("Student UUID is required.");

        RuleFor(x => x.GuardianUuid)
            .NotEmpty()
            .WithMessage("Guardian UUID is required.");

        RuleFor(x => x.TenantUuid)
            .NotEmpty()
            .WithMessage("Tenant UUID is required.");

        RuleFor(x => x.RelationshipNotes)
            .MaximumLength(500)
            .WithMessage("Relationship notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RelationshipNotes));
    }
}
