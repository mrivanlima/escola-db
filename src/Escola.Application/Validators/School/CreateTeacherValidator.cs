using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateTeacherValidator : AbstractValidator<CreateTeacherDto>
{
    public CreateTeacherValidator()
    {
        RuleFor(x => x.TenantUuid).NotEmpty();
        RuleFor(x => x.UserUuid).NotEmpty();
    }
}
