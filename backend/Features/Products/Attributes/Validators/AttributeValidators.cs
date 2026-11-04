using FluentValidation;
using PhoneStoreAPI.Features.Products.Attributes.DTOs;

namespace PhoneStoreAPI.Features.Products.Attributes.Validators;

public class CreateThuocTinhRequestValidator : AbstractValidator<CreateThuocTinhRequest>
{
    public CreateThuocTinhRequestValidator()
    {
        RuleFor(x => x.MaThuocTinh)
            .NotEmpty().WithMessage("Mã thuộc tính không được để trống")
            .Length(2, 10).WithMessage("Mã thuộc tính phải từ 2 đến 10 ký tự")
            .Matches(@"^[A-Z0-9_]{2,10}$").WithMessage("Mã thuộc tính chỉ gồm chữ in hoa, số và gạch dưới");

        RuleFor(x => x.TenThuocTinh)
            .NotEmpty().WithMessage("Tên thuộc tính không được để trống")
            .Length(2, 50).WithMessage("Tên thuộc tính phải từ 2 đến 50 ký tự");
    }
}

public class UpdateThuocTinhRequestValidator : AbstractValidator<UpdateThuocTinhRequest>
{
    public UpdateThuocTinhRequestValidator()
    {
        RuleFor(x => x.TenThuocTinh)
            .NotEmpty().WithMessage("Tên thuộc tính không được để trống")
            .Length(2, 50).WithMessage("Tên thuộc tính phải từ 2 đến 50 ký tự");
    }
}
