using FluentValidation;
using PhoneStoreAPI.Features.SystemSettings.DTOs;

namespace PhoneStoreAPI.Features.SystemSettings.Validators;

public class UpdateThamSoRequestValidator : AbstractValidator<UpdateThamSoRequest>
{
    public UpdateThamSoRequestValidator()
    {
        RuleFor(x => x.GiaTri)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá trị tham số phải lớn hơn hoặc bằng 0.");
    }
}

