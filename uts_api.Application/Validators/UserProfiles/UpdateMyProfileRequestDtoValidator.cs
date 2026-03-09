using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.UserProfiles;

namespace uts_api.Application.Validators.UserProfiles;

public sealed class UpdateMyProfileRequestDtoValidator : AbstractValidator<UpdateMyProfileRequestDto>
{
    public UpdateMyProfileRequestDtoValidator()
    {
        RuleFor(x => x.ProfilePictureUrl)
            .MaximumLength(500)
            .WithMessage(LocalizationKeys.MaximumLengthExceeded);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage(LocalizationKeys.MaximumLengthExceeded);

        RuleFor(x => x.JobTitle)
            .MaximumLength(150)
            .WithMessage(LocalizationKeys.MaximumLengthExceeded);

        RuleFor(x => x.Department)
            .MaximumLength(150)
            .WithMessage(LocalizationKeys.MaximumLengthExceeded);

        RuleFor(x => x.Bio)
            .MaximumLength(2000)
            .WithMessage(LocalizationKeys.MaximumLengthExceeded);
    }
}
