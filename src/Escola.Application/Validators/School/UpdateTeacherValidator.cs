using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherDto>
{
    public UpdateTeacherValidator()
    {
        // All fields optional
    }
}
