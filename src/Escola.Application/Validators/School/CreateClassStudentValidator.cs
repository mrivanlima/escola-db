using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateClassStudentValidator : AbstractValidator<CreateClassStudentDto>
{
    public CreateClassStudentValidator()
    {
        RuleFor(x => x.ClassUuid).NotEmpty();
        RuleFor(x => x.StudentUuid).NotEmpty();
        RuleFor(x => x.StatusUuid).NotEmpty();
    }
}
