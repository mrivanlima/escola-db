using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class CreateClassValidator : AbstractValidator<CreateClassDto>
{
    public CreateClassValidator()
    {
        RuleFor(x => x.TenantUuid).NotEmpty();
        RuleFor(x => x.ClassName).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.SchoolYearUuid).NotEmpty();
        RuleFor(x => x.MaxStudents).GreaterThan(0).When(x => x.MaxStudents.HasValue);
    }
}
