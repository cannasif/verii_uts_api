using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.UserPermissionGroups;

namespace uts_api.Application.Validators.UserPermissionGroups;

public sealed class UpdateUserPermissionGroupsRequestDtoValidator : AbstractValidator<UpdateUserPermissionGroupsRequestDto>
{
    public UpdateUserPermissionGroupsRequestDtoValidator()
    {
        RuleFor(x => x.PermissionGroupIds)
            .NotNull().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField));
    }
}
