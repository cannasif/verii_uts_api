using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.PermissionGroups;

namespace uts_api.Application.Validators.PermissionGroups;

public sealed class UpdatePermissionGroupPermissionsRequestDtoValidator : AbstractValidator<UpdatePermissionGroupPermissionsRequestDto>
{
    public UpdatePermissionGroupPermissionsRequestDtoValidator()
    {
        RuleFor(x => x.PermissionDefinitionIds)
            .NotNull().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField));
    }
}
