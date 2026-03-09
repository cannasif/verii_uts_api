using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.Users;

namespace uts_api.Application.Validators.Users;

public sealed class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(100).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 100));
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(100).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 100));
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .EmailAddress().WithMessage(AppLocalizer.Get(LocalizationKeys.InvalidEmailAddress))
            .MaximumLength(255).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 255));
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField));
    }
}
