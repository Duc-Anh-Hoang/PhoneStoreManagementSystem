using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.DTOs;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Models.Entities;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Features.OnlineOrders;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.Customers;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IThamSoProvider _thamSoProvider;
    private readonly IValidator<CreateCustomerRequest> _createValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateValidator;
    private readonly IMemoryCache? _cache;
    private readonly IOtpService? _otpService;

    public CustomerService(
        AppDbContext context,
        ICurrentUser currentUser,
        IThamSoProvider thamSoProvider,
        IValidator<CreateCustomerRequest> createValidator,
        IValidator<UpdateCustomerRequest> updateValidator,
        IMemoryCache? cache = null,
        IOtpService? otpService = null)
    {
        _context = context;
        _currentUser = currentUser;
        _thamSoProvider = thamSoProvider;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _cache = cache;
        _otpService = otpService;
    }

    public async Task<(List<CustomerDto> Items, int TotalCount)> GetAllAsync(string? keyword = null, int? page = null, int? pageSize = null)
    {
        var query = _context.KhachHangs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(k =>
                k.MaKH.ToLower().Contains(kw) ||
                k.HoTen.ToLower().Contains(kw) ||
                (k.Sdt != null && k.Sdt.Contains(kw)));
        }

        query = query.OrderBy(k => k.MaKH);
        var totalCount = await query.CountAsync();

        List<KhachHang> customers;
        if (page.HasValue && pageSize.HasValue && page.Value > 0 && pageSize.Value > 0)
        {
            var pSize = Math.Min(pageSize.Value, 100);
            customers = await query
                .Skip((page.Value - 1) * pSize)
                .Take(pSize)
                .ToListAsync();
        }
        else
        {
            customers = await query.ToListAsync();
        }

        var customerIds = customers.Select(c => c.MaKH).ToList();
        var spendingMap = await GetCustomerSpendingMapAsync(customerIds);
        var diemMoiVnd = await GetDiemMoiVndAsync();

        var result = customers.Select(c =>
        {
            var spending = spendingMap.TryGetValue(c.MaKH, out var val) ? val : 0m;
            var points = diemMoiVnd > 0 && spending > 0 ? (int)(spending / diemMoiVnd) : 0;
            return new CustomerDto
            {
                MaKH = c.MaKH,
                HoTen = c.HoTen,
                DiaChi = c.DiaChi,
                Sdt = c.Sdt,
                Email = c.Email,
                TongChiTieu = spending,
                DiemTichLuy = points
            };
        }).ToList();

        return (result, totalCount);
    }

    public async Task<CustomerDto> GetByIdAsync(string id)
    {
        var c = await _context.KhachHangs
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.MaKH.ToUpper() == id.Trim().ToUpper());

        if (c == null)
        {
            throw new NotFoundException($"Không tìm thấy khách hàng '{id}'.");
        }

        var spendingMap = await GetCustomerSpendingMapAsync(new List<string> { c.MaKH });
        var spending = spendingMap.TryGetValue(c.MaKH, out var val) ? val : 0m;
        var diemMoiVnd = await GetDiemMoiVndAsync();
        var points = diemMoiVnd > 0 && spending > 0 ? (int)(spending / diemMoiVnd) : 0;

        return new CustomerDto
        {
            MaKH = c.MaKH,
            HoTen = c.HoTen,
            DiaChi = c.DiaChi,
            Sdt = c.Sdt,
            Email = c.Email,
            TongChiTieu = spending,
            DiemTichLuy = points
        };
    }

    public async Task<CustomerDto> GetByPhoneAsync(string sdt)
    {
        var normalizedPhone = PhoneHelper.NormalizeVietnamesePhone(sdt);
        if (string.IsNullOrEmpty(normalizedPhone))
        {
            throw new BadRequestException("Số điện thoại tra cứu không hợp lệ.");
        }

        var c = await _context.KhachHangs
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Sdt == normalizedPhone);

        if (c == null)
        {
            throw new NotFoundException($"Không tìm thấy khách hàng với số điện thoại '{sdt}'.");
        }

        // Kiểm tra quyền: nếu là tài khoản khách hàng thì chỉ xem thông tin của chính mình
        if (_currentUser.IsAuthenticated && _currentUser.Role == AppConstants.Roles.Customer)
        {
            if (!string.IsNullOrEmpty(_currentUser.MaKH) && !_currentUser.MaKH.Equals(c.MaKH, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenException("Bạn chỉ có quyền xem thông tin của chính mình.");
            }
        }

        var spendingMap = await GetCustomerSpendingMapAsync(new List<string> { c.MaKH });
        var spending = spendingMap.TryGetValue(c.MaKH, out var val) ? val : 0m;
        var diemMoiVnd = await GetDiemMoiVndAsync();
        var points = diemMoiVnd > 0 && spending > 0 ? (int)(spending / diemMoiVnd) : 0;

        return new CustomerDto
        {
            MaKH = c.MaKH,
            HoTen = c.HoTen,
            DiaChi = c.DiaChi,
            Sdt = c.Sdt,
            Email = c.Email,
            TongChiTieu = spending,
            DiemTichLuy = points
        };
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var normalizedPhone = PhoneHelper.NormalizeVietnamesePhone(request.Sdt)!;

        // Trùng số điện thoại (409)
        var phoneExists = await _context.KhachHangs.AnyAsync(k => k.Sdt == normalizedPhone);
        if (phoneExists)
        {
            throw new ConflictException($"Số điện thoại '{normalizedPhone}' đã được đăng ký bởi khách hàng khác.");
        }

        string maKH;
        if (!string.IsNullOrWhiteSpace(request.MaKH))
        {
            maKH = request.MaKH.Trim().ToUpper();
            var codeExists = await _context.KhachHangs.AnyAsync(k => k.MaKH.ToUpper() == maKH);
            if (codeExists)
            {
                throw new ConflictException($"Mã khách hàng '{request.MaKH}' đã tồn tại.");
            }
        }
        else
        {
            maKH = await GenerateNextCustomerCodeAsync();
        }

        var entity = new KhachHang
        {
            MaKH = maKH,
            HoTen = request.HoTen.Trim(),
            Sdt = normalizedPhone,
            Email = request.Email?.Trim(),
            DiaChi = request.DiaChi?.Trim()
        };

        _context.KhachHangs.Add(entity);
        await _context.SaveChangesAsync();

        return new CustomerDto
        {
            MaKH = entity.MaKH,
            HoTen = entity.HoTen,
            DiaChi = entity.DiaChi,
            Sdt = entity.Sdt,
            Email = entity.Email,
            TongChiTieu = 0,
            DiemTichLuy = 0
        };
    }

    public async Task<CustomerDto> UpdateAsync(string id, UpdateCustomerRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var entity = await _context.KhachHangs
            .FirstOrDefaultAsync(k => k.MaKH.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy khách hàng '{id}'.");
        }

        var normalizedPhone = PhoneHelper.NormalizeVietnamesePhone(request.Sdt)!;

        // Kiểm tra trùng SĐT với khách hàng khác (409)
        var duplicatePhone = await _context.KhachHangs
            .AnyAsync(k => k.MaKH.ToUpper() != id.Trim().ToUpper() && k.Sdt == normalizedPhone);

        if (duplicatePhone)
        {
            throw new ConflictException($"Số điện thoại '{normalizedPhone}' đã được sử dụng bởi khách hàng khác.");
        }

        entity.HoTen = request.HoTen.Trim();
        entity.Sdt = normalizedPhone;
        entity.Email = request.Email?.Trim();
        entity.DiaChi = request.DiaChi?.Trim();

        await _context.SaveChangesAsync();

        var spendingMap = await GetCustomerSpendingMapAsync(new List<string> { entity.MaKH });
        var spending = spendingMap.TryGetValue(entity.MaKH, out var val) ? val : 0m;
        var diemMoiVnd = await GetDiemMoiVndAsync();
        var points = diemMoiVnd > 0 && spending > 0 ? (int)(spending / diemMoiVnd) : 0;

        return new CustomerDto
        {
            MaKH = entity.MaKH,
            HoTen = entity.HoTen,
            DiaChi = entity.DiaChi,
            Sdt = entity.Sdt,
            Email = entity.Email,
            TongChiTieu = spending,
            DiemTichLuy = points
        };
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.KhachHangs
            .FirstOrDefaultAsync(k => k.MaKH.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy khách hàng '{id}'.");
        }

        // 409 Conflict nếu khách đã có hóa đơn hoặc đơn hàng online
        var hasInvoice = await _context.HoaDons.AnyAsync(h => h.MaKH != null && h.MaKH.ToUpper() == id.Trim().ToUpper());
        if (hasInvoice)
        {
            throw new ConflictException($"Không thể xóa khách hàng '{id}' vì đã có hóa đơn mua hàng.");
        }

        var hasOnlineOrder = await _context.DonHangOnlines.AnyAsync(d => d.MaKH.ToUpper() == id.Trim().ToUpper());
        if (hasOnlineOrder)
        {
            throw new ConflictException($"Không thể xóa khách hàng '{id}' vì đã có đơn hàng online.");
        }

        _context.KhachHangs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private async Task<string> GenerateNextCustomerCodeAsync()
    {
        // Tự sinh mã KH + 3 chữ số tiếp theo (KH001, KH002...)
        var existingCodes = await _context.KhachHangs
            .AsNoTracking()
            .Where(k => k.MaKH.StartsWith("KH"))
            .Select(k => k.MaKH)
            .ToListAsync();

        int maxNum = 0;
        foreach (var code in existingCodes)
        {
            var numPart = code.Substring(2);
            if (int.TryParse(numPart, out int num) && num > maxNum)
            {
                maxNum = num;
            }
        }

        return $"KH{(maxNum + 1):D3}";
    }

    private async Task<decimal> GetDiemMoiVndAsync()
    {
        try
        {
            var val = await _thamSoProvider.GetAsync("DIEM_MOI_VND");
            return val > 0 ? val : 100000m;
        }
        catch
        {
            return 100000m;
        }
    }

    private async Task<Dictionary<string, decimal>> GetCustomerSpendingMapAsync(List<string> customerIds)
    {
        if (customerIds.Count == 0) return new Dictionary<string, decimal>();

        var invoices = await _context.HoaDons
            .AsNoTracking()
            .Where(h => h.MaKH != null && customerIds.Contains(h.MaKH))
            .GroupBy(h => h.MaKH!)
            .Select(g => new
            {
                MaKH = g.Key,
                TongChiTieu = g.Sum(h => h.TongTien - h.TienGiamGia)
            })
            .ToDictionaryAsync(x => x.MaKH, x => x.TongChiTieu);

        return invoices;
    }

    public async Task<MembershipLookupResponse> LookupMembershipAsync(MembershipLookupRequest request, string clientIp)
    {
        // 1. Chống lạm dụng (Rate Limiting theo IP: max 15 lần/phút)
        if (_cache != null && !string.IsNullOrWhiteSpace(clientIp))
        {
            var rateKey = $"lookup_rate:{clientIp}";
            var count = _cache.Get<int>(rateKey);
            if (count >= 15)
            {
                throw new BadRequestException("Bạn đã gửi yêu cầu tra cứu quá nhiều lần. Vui lòng thử lại sau 1 phút.");
            }
            _cache.Set(rateKey, count + 1, TimeSpan.FromMinutes(1));
        }

        var normalizedPhone = PhoneHelper.NormalizeVietnamesePhone(request.Sdt);
        if (string.IsNullOrEmpty(normalizedPhone))
        {
            return new MembershipLookupResponse
            {
                Exists = false,
                HangThanhVien = "Thành viên",
                DiscountPct = 0,
                DiscountAmount = 0,
                TenVietTat = string.Empty,
                SdtMasked = MaskPhone(request.Sdt),
                RequiresOtp = false
            };
        }

        var customer = await _context.KhachHangs
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Sdt == normalizedPhone);

        if (customer == null)
        {
            return new MembershipLookupResponse
            {
                Exists = false,
                HangThanhVien = "Thành viên",
                DiscountPct = 0,
                DiscountAmount = 0,
                TenVietTat = string.Empty,
                SdtMasked = MaskPhone(normalizedPhone),
                RequiresOtp = false
            };
        }

        // 2. Tính tổng chi tiêu lịch sử
        var spendingMap = await GetCustomerSpendingMapAsync(new List<string> { customer.MaKH });
        var tongChiTieu = spendingMap.TryGetValue(customer.MaKH, out var val) ? val : 0m;
        var diemMoiVnd = await GetDiemMoiVndAsync();
        var points = diemMoiVnd > 0 && tongChiTieu > 0 ? (int)(tongChiTieu / diemMoiVnd) : 0;

        // 3. Phân hạng & Tỉ lệ chiết khấu
        string hangThanhVien;
        int discountPct;
        if (tongChiTieu >= 100_000_000m)
        {
            hangThanhVien = "Kim Cương";
            discountPct = 5;
        }
        else if (tongChiTieu >= 50_000_000m)
        {
            hangThanhVien = "Vàng";
            discountPct = 3;
        }
        else if (tongChiTieu >= 20_000_000m)
        {
            hangThanhVien = "Bạc";
            discountPct = 1;
        }
        else
        {
            hangThanhVien = "Thành viên";
            discountPct = 0;
        }

        decimal discountAmount = Math.Round(request.CartTotal * discountPct / 100m, 0);

        // 4. Kiểm tra điều kiện bắt buộc OTP theo chính sách đã chốt:
        // - Đơn nhỏ/trung bình (< 15M và giảm < 500k): KHÔNG OTP
        // - Đơn >= 15M hoặc mức giảm >= 500k: BẮT BUỘC OTP
        // - Tiêu điểm tích lũy / Voucher sinh nhật: BẮT BUỘC OTP
        const decimal NGUONG_DON_HANG_OTP = 15_000_000m;
        const decimal NGUONG_GIAM_GIA_OTP = 500_000m;

        bool requiresOtp = false;
        string? otpReason = null;

        if (request.UsePoints || !string.IsNullOrWhiteSpace(request.VoucherCode))
        {
            requiresOtp = true;
            otpReason = "Sử dụng điểm tích lũy hoặc Voucher gắn với tài khoản thành viên";
        }
        else if (request.CartTotal >= NGUONG_DON_HANG_OTP)
        {
            requiresOtp = true;
            otpReason = "Đơn hàng giá trị từ 15 triệu đ cần xác thực OTP để đảm bảo an toàn";
        }
        else if (discountAmount >= NGUONG_GIAM_GIA_OTP)
        {
            requiresOtp = true;
            otpReason = "Mức giảm ưu đãi VIP từ 500.000 đ trở lên cần xác thực OTP";
        }

        return new MembershipLookupResponse
        {
            Exists = true,
            HangThanhVien = hangThanhVien,
            DiscountPct = discountPct,
            DiscountAmount = discountAmount,
            TenVietTat = MaskName(customer.HoTen),
            SdtMasked = MaskPhone(normalizedPhone),
            RequiresOtp = requiresOtp,
            OtpReason = otpReason,
            DiemTichLuyHienCo = points
        };
    }

    public async Task<OtpRequestResult> SendMembershipOtpAsync(string sdt, string clientIp)
    {
        var normalized = PhoneHelper.NormalizeVietnamesePhone(sdt) ?? sdt;
        if (_otpService == null)
        {
            return new OtpRequestResult { Blocked = false, Message = "Dịch vụ OTP chưa được kích hoạt.", Code = "123456" };
        }
        return await _otpService.RequestOtpAsync(normalized);
    }

    public async Task<OtpVerifyResult> VerifyMembershipOtpAsync(string sdt, string code)
    {
        var normalized = PhoneHelper.NormalizeVietnamesePhone(sdt) ?? sdt;
        if (_otpService == null)
        {
            return new OtpVerifyResult { Success = true, Token = "demo-otp-token-" + Guid.NewGuid() };
        }
        return await _otpService.VerifyOtpAsync(normalized, code);
    }

    private static string MaskName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return "Khách hàng";
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            var word = parts[0];
            return word.Length <= 2 ? word + "***" : word.Substring(0, 1) + "***" + word.Substring(word.Length - 1);
        }
        return string.Join(" ", parts.Select(p => p.Length <= 1 ? p : p[0] + "***"));
    }

    private static string MaskPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length < 7) return "***";
        return phone.Substring(0, 4) + "***" + phone.Substring(phone.Length - 3);
    }
}
