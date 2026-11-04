using FluentValidation;
using PhoneStoreAPI.Features.Suppliers.DTOs;

namespace PhoneStoreAPI.Features.Suppliers.Validators;

public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.MaNCC)
            .NotEmpty().WithMessage("Mã nhà cung cấp không được để trống.")
            .Length(2, 10).WithMessage("Mã nhà cung cấp phải từ 2 đến 10 ký tự.");

        RuleFor(x => x.TenNCC)
            .NotEmpty().WithMessage("Tên nhà cung cấp không được để trống.")
            .Length(2, 100).WithMessage("Tên nhà cung cấp phải từ 2 đến 100 ký tự.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email không đúng định dạng.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Sdt)
            .Matches(@"^[0-9\+\-\s\(\)]{8,15}$").WithMessage("Số điện thoại không hợp lệ.")
            .When(x => !string.IsNullOrWhiteSpace(x.Sdt));
    }
}

