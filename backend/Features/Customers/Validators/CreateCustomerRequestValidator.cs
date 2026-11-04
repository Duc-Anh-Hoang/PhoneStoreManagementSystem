using FluentValidation;
using PhoneStoreAPI.Features.Customers.DTOs;
using PhoneStoreAPI.Features.Customers.Helpers;

namespace PhoneStoreAPI.Features.Customers.Validators;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.HoTen)
            .NotEmpty().WithMessage("Họ tên khách hàng không được để trống.")
            .Length(2, 100).WithMessage("Họ tên khách hàng phải từ 2 đến 100 ký tự.");

        RuleFor(x => x.Sdt)
            .NotEmpty().WithMessage("Số điện thoại không được để trống.")
            .Must(PhoneHelper.IsValidVietnamesePhone)
            .WithMessage("Số điện thoại không hợp lệ (yêu cầu 10 chữ số, thuộc đầu số di động Việt Nam).");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email không đúng định dạng.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

