using DirectoryService.Core.Dto;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using FluentValidation;

namespace DirectoryService.Core.Validators;

[ScopedDependency]
public class RegisterDomainValidator : AbstractValidator<RegisterDomainDto>
{
    public RegisterDomainValidator()
    {
        var config = ServiceConfigurationContainer.Config.DirectoryService;
        var profanityFilter = new ProfanityFilter.ProfanityFilter();
        RuleFor(m => m.Name).NotEmpty().WithMessage("{PropertyName} not provided.")
            .MinimumLength(config!.MinDomainNameLength).WithMessage("{PropertyName} must be a minimum of " + config.MinDomainNameLength + " characters")
            .MaximumLength(config.MaxDomainNameLength).WithMessage("{PropertyName} must be a maximum of " + config.MaxDomainNameLength + "characters")
            .Matches(@"^[A-Za-z0-9._-]+$").WithMessage("{PropertyName} can only contain letters, numbers, hyphens, dashes, periods.")
            .Custom((s, context) =>
            {
                if(profanityFilter.IsProfanity(s.ToLower()))
                {
                    context.AddFailure("{PropertyName} contains a blacklisted word.");
                }
            });
    }
}