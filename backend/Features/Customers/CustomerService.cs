using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.DTOs;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Customers;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IThamSoProvider _thamSoProvider;
    private readonly IValidator<CreateCustomerRequest> _createValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateValidator;

    public CustomerService(
        AppDbContext context,
        ICurrentUser currentUser,
        IThamSoProvider thamSoProvider,
        IValidator<CreateCustomerRequest> createValidator,
        IValidator<UpdateCustomerRequest> updateValidator)
    {
        _context = context;
        _currentUser = currentUser;
        _thamSoProvider = thamSoProvider;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
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
}
