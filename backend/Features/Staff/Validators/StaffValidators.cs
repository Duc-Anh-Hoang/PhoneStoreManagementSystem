using FluentValidation;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff.Validators;

public class CreateNhanVienRequestValidator : AbstractValidator<CreateNhanVienRequest>
{
    public CreateNhanVienRequestValidator()
    {
        RuleFor(x => x.HoTen)
            .NotEmpty().WithMessage("Họ tên nhân viên không được để trống.")
            .Length(2, 100).WithMessage("Họ tên nhân viên phải có độ dài từ 2 đến 100 ký tự.");

        RuleFor(x => x.ChucVu)
            .NotEmpty().WithMessage("Chức vụ không được để trống.")
            .Must(c => AppConstants.ValidPositions.Contains(c))
            .WithMessage($"Chức vụ phải là một trong các giá trị: {string.Join(", ", AppConstants.ValidPositions)}.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Định dạng email không hợp lệ.");

        RuleFor(x => x.Sdt)
            .Matches(@"^[0-9]{9,15}$").When(x => !string.IsNullOrEmpty(x.Sdt))
            .WithMessage("Số điện thoại phải từ 9 đến 15 chữ số.");
    }
}

public class UpdateNhanVienRequestValidator : AbstractValidator<UpdateNhanVienRequest>
{
    public UpdateNhanVienRequestValidator()
    {
        RuleFor(x => x.HoTen)
            .NotEmpty().WithMessage("Họ tên nhân viên không được để trống.")
            .Length(2, 100).WithMessage("Họ tên nhân viên phải có độ dài từ 2 đến 100 ký tự.");

        RuleFor(x => x.ChucVu)
            .NotEmpty().WithMessage("Chức vụ không được để trống.")
            .Must(c => AppConstants.ValidPositions.Contains(c))
            .WithMessage($"Chức vụ phải là một trong các giá trị: {string.Join(", ", AppConstants.ValidPositions)}.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Định dạng email không hợp lệ.");

        RuleFor(x => x.Sdt)
            .Matches(@"^[0-9]{9,15}$").When(x => !string.IsNullOrEmpty(x.Sdt))
            .WithMessage("Số điện thoại phải từ 9 đến 15 chữ số.");

        RuleFor(x => x.TrangThai)
            .Must(t => t == AppConstants.EmployeeStatus.DangLam || t == AppConstants.EmployeeStatus.DaNghi)
            .WithMessage("Trạng thái phải là 'dang_lam' hoặc 'da_nghi'.");
    }
}
