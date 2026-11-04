using FluentValidation;
using PhoneStoreAPI.Features.Products.Categories.DTOs;

namespace PhoneStoreAPI.Features.Products.Categories.Validators;

public class CreateNhomHangRequestValidator : AbstractValidator<CreateNhomHangRequest>
{
    public CreateNhomHangRequestValidator()
    {
        RuleFor(x => x.MaNhom)
            .NotEmpty().WithMessage("Mã nhóm hàng không được để trống")
            .Length(2, 10).WithMessage("Mã nhóm hàng phải từ 2 đến 10 ký tự")
            .Matches(@"^[A-Z0-9_]{2,10}$").WithMessage("Mã nhóm hàng chỉ gồm chữ in hoa, số và gạch dưới");

        RuleFor(x => x.TenNhom)
            .NotEmpty().WithMessage("Tên nhóm hàng không được để trống")
            .Length(2, 50).WithMessage("Tên nhóm hàng phải từ 2 đến 50 ký tự");
    }
}

public class UpdateNhomHangRequestValidator : AbstractValidator<UpdateNhomHangRequest>
{
    public UpdateNhomHangRequestValidator()
    {
        RuleFor(x => x.TenNhom)
            .NotEmpty().WithMessage("Tên nhóm hàng không được để trống")
            .Length(2, 50).WithMessage("Tên nhóm hàng phải từ 2 đến 50 ký tự");
    }
}

public class CreateLoaiHangRequestValidator : AbstractValidator<CreateLoaiHangRequest>
{
    public CreateLoaiHangRequestValidator()
    {
        RuleFor(x => x.MaLoai)
            .NotEmpty().WithMessage("Mã loại hàng không được để trống")
            .Length(2, 10).WithMessage("Mã loại hàng phải từ 2 đến 10 ký tự")
            .Matches(@"^[A-Z0-9_]{2,10}$").WithMessage("Mã loại hàng chỉ gồm chữ in hoa, số và gạch dưới");

        RuleFor(x => x.TenLoai)
            .NotEmpty().WithMessage("Tên loại hàng không được để trống")
            .Length(2, 50).WithMessage("Tên loại hàng phải từ 2 đến 50 ký tự");
    }
}

public class UpdateLoaiHangRequestValidator : AbstractValidator<UpdateLoaiHangRequest>
{
    public UpdateLoaiHangRequestValidator()
    {
        RuleFor(x => x.TenLoai)
            .NotEmpty().WithMessage("Tên loại hàng không được để trống")
            .Length(2, 50).WithMessage("Tên loại hàng phải từ 2 đến 50 ký tự");
    }
}
