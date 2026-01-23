using FluentValidation;

namespace Escola.Application.UseCases.Tenants.UpdateTenant;

/// <summary>
/// Validator for UpdateTenantRequest.
/// </summary>
public class UpdateTenantValidator : AbstractValidator<UpdateTenantRequest>
{
    public UpdateTenantValidator()
    {
        RuleFor(x => x.TenantUuid)
            .NotEmpty()
            .WithMessage("Tenant UUID is required.");

        RuleFor(x => x.TenantName)
            .NotEmpty()
            .WithMessage("Tenant name cannot be empty when provided.")
            .MaximumLength(255)
            .WithMessage("Tenant name cannot exceed 255 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-_\.]+$")
            .WithMessage("Tenant name can only contain letters, numbers, spaces, hyphens, underscores, and periods.")
            .When(x => x.TenantName != null);

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
