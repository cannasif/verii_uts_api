using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.Auth;

namespace uts_api.Application.Validators.Auth;

public sealed class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    public ChangePasswordRequestDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MinimumLength(6).WithMessage(AppLocalizer.Get(LocalizationKeys.MinimumLengthRequired, 6));
    }
}
