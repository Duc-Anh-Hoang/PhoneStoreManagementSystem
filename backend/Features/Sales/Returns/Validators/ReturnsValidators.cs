using FluentValidation;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns.Validators;

public class CreateTraHangRequestValidator : AbstractValidator<CreateTraHangRequest>
{
    private static readonly string[] ValidRefundMethods = { "tien_mat", "chuyen_khoan", "doi_hang" };
    private static readonly string[] ValidRequestTypes = { "loi_ky_thuat", "doi_y" };

    public CreateTraHangRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.MaIMEI) || !string.IsNullOrWhiteSpace(x.SoIMEI))
            .WithMessage("Phải cung cấp MaIMEI hoặc SoIMEI để xác định máy trả");

        RuleFor(x => x.LoaiYeuCau)
            .NotEmpty().WithMessage("Loại yêu cầu không được để trống")
            .Must(t => ValidRequestTypes.Contains(t))
            .WithMessage("Loại yêu cầu phải là loi_ky_thuat hoặc doi_y");

        RuleFor(x => x.LyDo)
            .NotEmpty().WithMessage("Lý do trả hàng không được để trống")
            .MaximumLength(255).WithMessage("Lý do trả hàng tối đa 255 ký tự");

        RuleFor(x => x.HinhThucHoanTien)
            .NotEmpty().WithMessage("Hình thức hoàn tiền không được để trống")
            .Must(m => ValidRefundMethods.Contains(m))
            .WithMessage("Hình thức hoàn tiền phải là tien_mat, chuyen_khoan hoặc doi_hang");
    }
}

public class KiemTraTraHangRequestValidator : AbstractValidator<KiemTraTraHangRequest>
{
    private static readonly string[] ValidResults = { "loi_nsx", "khong_loi", "hu_hong_do_nguoi_dung" };

    public KiemTraTraHangRequestValidator()
    {
        RuleFor(x => x.KetQuaKiemTra)
            .NotEmpty().WithMessage("Kết quả kiểm tra không được để trống")
            .Must(r => ValidResults.Contains(r))
            .WithMessage("Kết quả kiểm tra phải là loi_nsx, khong_loi hoặc hu_hong_do_nguoi_dung");

        RuleFor(x => x.GhiChuKiemTra)
            .NotEmpty().WithMessage("Ghi chú kiểm tra của kỹ thuật viên là bắt buộc");
    }
}

public class DuyetTraHangRequestValidator : AbstractValidator<DuyetTraHangRequest>
{
    public DuyetTraHangRequestValidator()
    {
        When(x => !x.ChapNhan, () =>
        {
            RuleFor(x => x.LyDoTuChoi)
                .NotEmpty().WithMessage("Lý do từ chối không được để trống khi từ chối hồ sơ")
                .MaximumLength(255).WithMessage("Lý do từ chối tối đa 255 ký tự");
        });
    }
}

public class HuyTraHangRequestValidator : AbstractValidator<HuyTraHangRequest>
{
    public HuyTraHangRequestValidator()
    {
        RuleFor(x => x.LyDo)
            .NotEmpty().WithMessage("Lý do hủy hồ sơ không được để trống")
            .MaximumLength(255).WithMessage("Lý do hủy tối đa 255 ký tự");
    }
}
