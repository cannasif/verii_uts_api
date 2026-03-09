using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.Auth;

namespace uts_api.Application.Validators.Auth;

public sealed class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
{
    public ForgotPasswordRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .EmailAddress().WithMessage(AppLocalizer.Get(LocalizationKeys.InvalidEmailAddress));
    }
}
