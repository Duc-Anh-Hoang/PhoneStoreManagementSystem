using FluentValidation;
using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff.Validators;

public class CreateTaiKhoanRequestValidator : AbstractValidator<CreateTaiKhoanRequest>
{
    public CreateTaiKhoanRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống.")
            .Length(3, 50).WithMessage("Tên đăng nhập phải có từ 3 đến 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_.]+$").WithMessage("Tên đăng nhập chỉ chứa chữ cái, số, dấu gạch dưới và dấu chấm.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(8).WithMessage("Mật khẩu phải có tối thiểu 8 ký tự.");

        RuleFor(x => x.MaNV)
            .NotEmpty().WithMessage("Mã nhân viên không được để trống.");

        RuleFor(x => x.MaQuyen)
            .NotEmpty().WithMessage("Mã quyền không được để trống.");
    }
}

public class UpdateTaiKhoanRequestValidator : AbstractValidator<UpdateTaiKhoanRequest>
{
    public UpdateTaiKhoanRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .MinimumLength(8).When(x => !string.IsNullOrEmpty(x.NewPassword))
            .WithMessage("Mật khẩu mới phải có tối thiểu 8 ký tự.");
    }
}
