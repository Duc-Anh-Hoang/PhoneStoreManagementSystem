using FluentValidation;
using PhoneStoreAPI.Features.Warranty.DTOs;

namespace PhoneStoreAPI.Features.Warranty.Validators;

public class CreateTiepNhanRequestValidator : AbstractValidator<CreateTiepNhanRequest>
{
    public CreateTiepNhanRequestValidator()
    {
        RuleFor(x => x.SoIMEI)
            .NotEmpty().WithMessage("Vui lòng cung cấp số IMEI cần tiếp nhận bảo hành.");

        RuleFor(x => x.TinhTrangLoi)
            .NotEmpty().WithMessage("Vui lòng mô tả tình trạng lỗi của sản phẩm.")
            .MaximumLength(255).WithMessage("Mô tả tình trạng lỗi tối đa 255 ký tự.");

        RuleFor(x => x.ChiPhi)
            .GreaterThanOrEqualTo(0).WithMessage("Chi phí tiếp nhận bảo hành không được âm.");
    }
}

public class UpdateTiepNhanRequestValidator : AbstractValidator<UpdateTiepNhanRequest>
{
    public UpdateTiepNhanRequestValidator()
    {
        RuleFor(x => x.ChiPhi)
            .GreaterThanOrEqualTo(0).When(x => x.ChiPhi.HasValue)
            .WithMessage("Chi phí sửa chữa không được âm.");

        RuleFor(x => x.KetQua)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.KetQua))
            .WithMessage("Kết quả kiểm tra/sửa chữa tối đa 255 ký tự.");
    }
}

public class UpdateTrangThaiRequestValidator : AbstractValidator<UpdateTrangThaiRequest>
{
    private static readonly string[] AllowedStatuses =
    {
        "tiep_nhan",
        "dang_sua",
        "cho_linh_kien",
        "hoan_thanh",
        "da_tra"
    };

    public UpdateTrangThaiRequestValidator()
    {
        RuleFor(x => x.TrangThai)
            .NotEmpty().WithMessage("Vui lòng cung cấp trạng thái mới.")
            .Must(s => AllowedStatuses.Contains(s))
            .WithMessage("Trạng thái không hợp lệ trong hệ thống bảo hành.");
    }
}
