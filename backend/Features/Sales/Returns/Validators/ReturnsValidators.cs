using FluentValidation;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns.Validators;

public class CreateTraHangRequestValidator : AbstractValidator<CreateTraHangRequest>
{
    private static readonly string[] ValidRefundMethods = { "tien_mat", "chuyen_khoan", "doi_hang" };

    public CreateTraHangRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.MaIMEI) || !string.IsNullOrWhiteSpace(x.SoIMEI))
            .WithMessage("Phải cung cấp MaIMEI hoặc SoIMEI để xác định máy trả");

        RuleFor(x => x.LyDo)
            .NotEmpty().WithMessage("Lý do trả hàng không được để trống")
            .MaximumLength(255).WithMessage("Lý do trả hàng tối đa 255 ký tự");

        RuleFor(x => x.HinhThucHoanTien)
            .NotEmpty().WithMessage("Hình thức hoàn tiền không được để trống")
            .Must(m => ValidRefundMethods.Contains(m))
            .WithMessage("Hình thức hoàn tiền phải là tien_mat, chuyen_khoan hoặc doi_hang");
    }
}
