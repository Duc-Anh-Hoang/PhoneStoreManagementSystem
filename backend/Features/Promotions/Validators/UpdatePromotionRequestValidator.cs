using FluentValidation;
using PhoneStoreAPI.Features.Promotions.DTOs;

namespace PhoneStoreAPI.Features.Promotions.Validators;

public class UpdatePromotionRequestValidator : AbstractValidator<UpdatePromotionRequest>
{
    public UpdatePromotionRequestValidator()
    {
        RuleFor(x => x.TenKM)
            .NotEmpty().WithMessage("Tên khuyến mãi không được để trống.")
            .Length(2, 100).WithMessage("Tên khuyến mãi phải từ 2 đến 100 ký tự.");

        RuleFor(x => x.PhanTramGiam)
            .GreaterThan(0).WithMessage("Phần trăm giảm phải lớn hơn 0.")
            .LessThanOrEqualTo(100).WithMessage("Phần trăm giảm không được vượt quá 100%.");

        RuleFor(x => x.NgayKT)
            .GreaterThan(x => x.NgayBD).WithMessage("Ngày kết thúc phải sau ngày bắt đầu.");
    }
}

