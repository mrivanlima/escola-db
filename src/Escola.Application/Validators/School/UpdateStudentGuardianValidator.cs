using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class UpdateStudentGuardianValidator : AbstractValidator<UpdateStudentGuardianDto>
{
    public UpdateStudentGuardianValidator()
    {
        RuleFor(x => x.RelationshipNotes)
            .MaximumLength(500)
            .WithMessage("Relationship notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RelationshipNotes));
    }
}
