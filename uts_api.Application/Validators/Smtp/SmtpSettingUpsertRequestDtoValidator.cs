using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.Smtp;

namespace uts_api.Application.Validators.Smtp;

public sealed class SmtpSettingUpsertRequestDtoValidator : AbstractValidator<SmtpSettingUpsertRequestDto>
{
    public SmtpSettingUpsertRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(100).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 100));
        RuleFor(x => x.Host)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(200).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 200));
        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535).WithMessage(AppLocalizer.Get(LocalizationKeys.InvalidRange, 1, 65535));
        RuleFor(x => x.FromEmail)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .EmailAddress().WithMessage(AppLocalizer.Get(LocalizationKeys.InvalidEmailAddress))
            .MaximumLength(255).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 255));
        RuleFor(x => x.UserName)
            .MaximumLength(255).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 255));
        RuleFor(x => x.FromName)
            .MaximumLength(100).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 100));
    }
}
