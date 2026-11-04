using FluentValidation;
using PhoneStoreAPI.Features.Purchasing.DTOs;

namespace PhoneStoreAPI.Features.Purchasing.Validators;

public class CreatePhieuNhapRequestValidator : AbstractValidator<CreatePhieuNhapRequest>
{
    public CreatePhieuNhapRequestValidator()
    {
        RuleFor(x => x.MaNCC)
            .NotEmpty().WithMessage("Mã nhà cung cấp không được để trống.");

        RuleFor(x => x.ChiTiet)
            .NotEmpty().WithMessage("Phiếu nhập phải có ít nhất một mặt hàng.");

        RuleForEach(x => x.ChiTiet).ChildRules(item =>
        {
            item.RuleFor(i => i.MaHang)
                .NotEmpty().WithMessage("Mã mặt hàng không được để trống.");

            item.RuleFor(i => i.SlNhap)
                .GreaterThan(0).WithMessage("Số lượng nhập phải lớn hơn 0.");

            item.RuleFor(i => i.DonGiaNhap)
                .GreaterThan(0).WithMessage("Đơn giá nhập phải lớn hơn 0.");
        });
    }
}

