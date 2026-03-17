using FluentValidation;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.UretimTarihi;

namespace uts_api.Application.Validators.UretimTarihi;

public sealed class UpdateUretimTarihiRequestDtoValidator : AbstractValidator<UpdateUretimTarihiRequestDto>
{
    public UpdateUretimTarihiRequestDtoValidator()
    {
        RuleFor(x => x.StokKodu)
            .MaximumLength(50).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 50));
        RuleFor(x => x.SeriLotNo)
            .MaximumLength(50).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 50));
        RuleFor(x => x.LotNo)
            .MaximumLength(50).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 50));
        RuleFor(x => x.SYedek1)
            .MaximumLength(150).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 150));
        RuleFor(x => x.SYedek2)
            .MaximumLength(250).WithMessage(AppLocalizer.Get(LocalizationKeys.MaximumLengthExceeded, 250));
    }
}
