using FluentValidation;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    private static readonly string[] ValidPaymentMethods = { "cod", "chuyen_khoan", "the", "banking", "momo", "vnpay", "zalopay", "card", "tien_mat" };

    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.TenNguoiNhan)
            .NotEmpty().WithMessage("Tên người nhận không được để trống")
            .MinimumLength(2).WithMessage("Tên người nhận phải có ít nhất 2 ký tự");

        RuleFor(x => x.SdtNguoiNhan)
            .NotEmpty().WithMessage("Số điện thoại người nhận không được để trống")
            .Matches(@"^(0)(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$")
            .WithMessage("Số điện thoại không đúng định dạng Việt Nam (10 chữ số)");

        RuleFor(x => x.DiaChiGiao)
            .NotEmpty().WithMessage("Địa chỉ giao không được để trống")
            .MinimumLength(5).WithMessage("Địa chỉ giao phải có ít nhất 5 ký tự");

        RuleFor(x => x.PhuongThucThanhToan)
            .NotEmpty().WithMessage("Phương thức thanh toán không được để trống")
            .Must(m => !string.IsNullOrWhiteSpace(m) && ValidPaymentMethods.Contains(m.Trim().ToLowerInvariant()))
            .WithMessage("Phương thức thanh toán phải là cod, chuyen_khoan hoặc the");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Đơn hàng phải có ít nhất 1 sản phẩm");

        RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
    }
}

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.MaHang)
            .NotEmpty().WithMessage("Mã mặt hàng không được để trống");

        RuleFor(x => x.SoLuong)
            .InclusiveBetween(1, 99).WithMessage("Số lượng mua mỗi sản phẩm phải từ 1 đến 10");
    }
}
