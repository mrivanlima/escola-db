using FluentValidation;

namespace Escola.Application.UseCases.Teachers.UpdateTeacher;

/// <summary>
/// Validator for UpdateTeacherRequest.
/// </summary>
public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherRequest>
{
    public UpdateTeacherValidator()
    {
        // All fields are optional for update
    }
}
