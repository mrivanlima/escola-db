using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateSubjectValidator : AbstractValidator<CreateSubjectDto>
{
    public CreateSubjectValidator()
    {
        RuleFor(x => x.SubjectName)
            .NotEmpty().WithMessage("Subject name is required")
            .MinimumLength(2).WithMessage("Subject name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Subject name must not exceed 200 characters");
    }
}

public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectDto>
{
    public UpdateSubjectValidator()
    {
        RuleFor(x => x.SubjectName)
            .NotEmpty().WithMessage("Subject name is required")
            .MinimumLength(2).WithMessage("Subject name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Subject name must not exceed 200 characters");
    }
}
