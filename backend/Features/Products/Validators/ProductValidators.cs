using System.Text.RegularExpressions;
using FluentValidation;
using PhoneStoreAPI.Features.Products.DTOs;

namespace PhoneStoreAPI.Features.Products.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.MaHang)
            .NotEmpty().WithMessage("Mã mặt hàng không được để trống")
            .Length(2, 10).WithMessage("Mã mặt hàng phải từ 2 đến 10 ký tự")
            .Matches(@"^[A-Z0-9_]{2,10}$").WithMessage("Mã mặt hàng chỉ được chứa chữ in hoa, chữ số và dấu gạch dưới");

        RuleFor(x => x.TenHang)
            .NotEmpty().WithMessage("Tên mặt hàng không được để trống")
            .Length(2, 100).WithMessage("Tên mặt hàng phải từ 2 đến 100 ký tự");

        RuleFor(x => x.DonGia)
            .GreaterThan(0).WithMessage("Đơn giá mặt hàng phải lớn hơn 0");

        RuleFor(x => x.Dvt)
            .MaximumLength(20).WithMessage("Đơn vị tính tối đa 20 ký tự");

        RuleFor(x => x.MaNhom)
            .NotEmpty().WithMessage("Mã nhóm hàng không được để trống");

        RuleFor(x => x.MaLoai)
            .NotEmpty().WithMessage("Mã loại hàng không được để trống");

        RuleFor(x => x.ThoiHanBaoHanh)
            .GreaterThanOrEqualTo(0).WithMessage("Thời hạn bảo hành không được âm");
    }
}

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.TenHang)
            .NotEmpty().WithMessage("Tên mặt hàng không được để trống")
            .Length(2, 100).WithMessage("Tên mặt hàng phải từ 2 đến 100 ký tự");

        RuleFor(x => x.DonGia)
            .GreaterThan(0).WithMessage("Đơn giá mặt hàng phải lớn hơn 0");

        RuleFor(x => x.Dvt)
            .MaximumLength(20).WithMessage("Đơn vị tính tối đa 20 ký tự");

        RuleFor(x => x.MaNhom)
            .NotEmpty().WithMessage("Mã nhóm hàng không được để trống");

        RuleFor(x => x.MaLoai)
            .NotEmpty().WithMessage("Mã loại hàng không được để trống");

        RuleFor(x => x.ThoiHanBaoHanh)
            .GreaterThanOrEqualTo(0).WithMessage("Thời hạn bảo hành không được âm");
    }
}
