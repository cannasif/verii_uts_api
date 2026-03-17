using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.UtsLogs;

namespace uts_api.Application.Validators.UtsLogs;

public sealed class CreateUtsLogRequestDtoValidator : AbstractValidator<CreateUtsLogRequestDto>
{
    public CreateUtsLogRequestDtoValidator()
    {
        RuleFor(x => x.Bno)
            .NotEmpty().WithMessage(AppLocalizer.Get(LocalizationKeys.RequiredField))
            .MaximumLength(255).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 255));

        RuleFor(x => x.StokKodu)
            .MaximumLength(35).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 35));

        RuleFor(x => x.SeriNo)
            .MaximumLength(50).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 50));

        RuleFor(x => x.GonderenKisi)
            .MaximumLength(128).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 128));

        RuleFor(x => x.GonderimTipi)
            .MaximumLength(5).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 5));

        RuleFor(x => x.Durum)
            .MaximumLength(1).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 1));
    }
}
