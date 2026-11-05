using FluentValidation;
using PhoneStoreAPI.Features.Sales.DTOs;

namespace PhoneStoreAPI.Features.Sales.Validators;

public class CreateHoaDonRequestValidator : AbstractValidator<CreateHoaDonRequest>
{
    private static readonly string[] ValidPaymentMethods = { "tien_mat", "chuyen_khoan", "the" };

    public CreateHoaDonRequestValidator()
    {
        RuleFor(x => x.PhuongThucThanhToan)
            .NotEmpty().WithMessage("Phương thức thanh toán không được để trống")
            .Must(m => ValidPaymentMethods.Contains(m))
            .WithMessage("Phương thức thanh toán phải là tien_mat, chuyen_khoan hoặc the");

        RuleFor(x => x.ChiTiet)
            .NotEmpty().WithMessage("Hóa đơn phải có ít nhất một dòng hàng hóa");

        RuleForEach(x => x.ChiTiet).SetValidator(new ChiTietHoaDonRequestValidator());
    }
}

public class ChiTietHoaDonRequestValidator : AbstractValidator<ChiTietHoaDonRequest>
{
    public ChiTietHoaDonRequestValidator()
    {
        RuleFor(x => x.MaHang)
            .NotEmpty().WithMessage("Mã mặt hàng không được để trống");

        RuleFor(x => x.SoLuong)
            .GreaterThan(0).WithMessage("Số lượng bán phải lớn hơn 0");
    }
}
