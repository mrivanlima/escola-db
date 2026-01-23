using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class UpdateClassStudentValidator : AbstractValidator<UpdateClassStudentDto>
{
    public UpdateClassStudentValidator()
    {
        // All fields are optional for update
    }
}
