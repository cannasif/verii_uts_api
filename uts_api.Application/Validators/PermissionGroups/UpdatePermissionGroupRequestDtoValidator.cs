using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.PermissionGroups;

namespace uts_api.Application.Validators.PermissionGroups;

public sealed class UpdatePermissionGroupRequestDtoValidator : AbstractValidator<UpdatePermissionGroupRequestDto>
{
    public UpdatePermissionGroupRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(100).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 100));
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 500));
    }
}
