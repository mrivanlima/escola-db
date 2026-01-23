using Escola.Application.DTOs.Identity;
using FluentValidation;

namespace Escola.Application.Validators.Identity;

/// <summary>
/// Validator for CreateTenantDto.
/// Validates tenant creation requests according to business rules.
/// </summary>
public class CreateTenantDtoValidator : AbstractValidator<CreateTenantDto>
{
    public CreateTenantDtoValidator()
    {
        RuleFor(x => x.TenantName)
            .NotEmpty()
            .WithMessage("Tenant name is required.")
            .MaximumLength(255)
            .WithMessage("Tenant name cannot exceed 255 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-_\.]+$")
            .WithMessage("Tenant name can only contain letters, numbers, spaces, hyphens, underscores, and periods.");

        RuleFor(x => x.TenantConfig)
            .Must(BeValidJsonOrNull)
            .WithMessage("Tenant configuration must be valid JSON.")
            .When(x => !string.IsNullOrWhiteSpace(x.TenantConfig));
    }

    private bool BeValidJsonOrNull(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
