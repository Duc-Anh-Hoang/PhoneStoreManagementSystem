using Microsoft.AspNetCore.SignalR;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Features.Warranty;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.OnlineOrders;

public class OnlineOrderService : IOnlineOrderService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly IWarrantyService _warrantyService;
    private readonly IEmailService? _emailService;
    private readonly IHubContext<OrderHub>? _hubContext;
    private readonly IShippingService? _shippingService;

    public OnlineOrderService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        IWarrantyService warrantyService,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null,
        IShippingService? shippingService = null)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _warrantyService = warrantyService;
        _emailService = emailService;
        _hubContext = hubContext;
        _shippingService = shippingService;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Chuáº©n hÃ³a SÄT ngÆ°á»i nháº­n
            var sdtNorm = PhoneHelper.NormalizeVietnamesePhone(request.SdtNguoiNhan)
                          ?? request.SdtNguoiNhan.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

            // 2. TÃ¬m hoáº·c táº¡o khÃ¡ch hÃ ng
            KhachHang? khachHang = null;
            if (!string.IsNullOrWhiteSpace(request.MaKH))
            {
                khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == request.MaKH);
            }

            if (khachHang == null)
            {
                khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == sdtNorm);
            }

            if (khachHang == null)
            {
                var newMaKH = await _codeGenerator.NextAsync("KH");
                while (await _context.KhachHangs.AnyAsync(k => k.MaKH == newMaKH)) { newMaKH = "KH" + Random.Shared.Next(10000000, 99999999); }
                khachHang = new KhachHang
                {
                    MaKH = newMaKH,
                    HoTen = request.TenNguoiNhan.Trim(),
                    Sdt = sdtNorm,
                    DiaChi = request.DiaChiGiao.Trim(),
                    Email = !string.IsNullOrWhiteSpace(request.Email) ? request.Email.Trim() : $"{sdtNorm}@cellphones.customer"
                };
                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
            }
            else if (!string.IsNullOrWhiteSpace(request.Email))
            {
                khachHang.Email = request.Email.Trim();
                await _context.SaveChangesAsync();
            }

            // 3. Kiá»ƒm tra sáº£n pháº©m vÃ  tÃ­nh tá»•ng tiá»n tá»« DonGia DB
            decimal tongTien = 0m;
            var chiTietList = new List<ChiTietDonHangOnline>();

            var groupedItems = request.Items.GroupBy(x => (x.MaHang ?? "MH001").Trim().Substring(0, Math.Min(10, (x.MaHang ?? "MH001").Trim().Length))).Select(g => new OrderItemRequest { MaHang = g.Key, TenHang = g.First().TenHang, SoLuong = g.Sum(i => i.SoLuong), DonGia = g.First().DonGia }).ToList();
            foreach (var item in groupedItems)
            {
                var matHang = await _context.MatHangs.FirstOrDefaultAsync(m => m.MaHang == item.MaHang);
                if (matHang == null)
                {
                    var tenH = !string.IsNullOrWhiteSpace(item.TenHang) ? item.TenHang.Trim() : item.MaHang;
                    var gia = item.DonGia > 0 ? item.DonGia : 5000000m;
                    matHang = new MatHang
                    {
                        MaHang = item.MaHang,
                        TenHang = tenH,
                        DonGia = gia,
                        Dvt = "CÃ¡i",
                                                MaLoai = (_context.LoaiHangs.Select(l => l.MaLoai).FirstOrDefault() ?? "APPLE"),
                        MaNhom = (_context.NhomHangs.Select(n => n.MaNhom).FirstOrDefault() ?? "DIENTHOAI"),
                        QuanLyImei = true,
                        ThoiHanBaoHanh = 12
                    };
                    _context.MatHangs.Add(matHang);
                    await _context.SaveChangesAsync();
                }

                decimal donGiaDb = matHang.DonGia > 0 ? matHang.DonGia : (item.DonGia > 0 ? item.DonGia : 5000000m);
                tongTien += donGiaDb * item.SoLuong;

                chiTietList.Add(new ChiTietDonHangOnline
                {
                    MaHang = matHang.MaHang,
                    SoLuong = item.SoLuong,
                    DonGia = donGiaDb
                });
            }

            // 4. Táº¡o DonHangOnline
            var reqMaDH = !string.IsNullOrWhiteSpace(request.MaDH) ? request.MaDH.Trim().Substring(0, Math.Min(15, request.MaDH.Trim().Length)) : null;
            // Giai đoạn 1: Dùng mã ngẫu nhiên an toàn thay cho sequence tuần tự
            var maDH = (!string.IsNullOrWhiteSpace(reqMaDH) && !await _context.DonHangOnlines.AnyAsync(d => d.MaDH == reqMaDH))
                ? reqMaDH
                : await _codeGenerator.GenerateOrderCode(code => _context.DonHangOnlines.AnyAsync(d => d.MaDH == code));
            var phuongThuc = (request.PhuongThucThanhToan ?? "cod").Trim().ToLowerInvariant() switch
            {
                "banking" or "momo" or "vnpay" or "zalopay" or "chuyen_khoan" => "chuyen_khoan",
                "card" or "the" => "the",
                _ => "cod"
            };

            // Nghiệp vụ Phương án 1: Nếu thanh toán online (chuyển khoản, ví QR, thẻ) thì khởi tạo ở trạng thái Chờ thanh toán
            // Nếu là COD (thanh toán khi nhận hàng) thì vào thẳng Chờ xác nhận
            var initialStatus = (phuongThuc == "chuyen_khoan" || phuongThuc == "the")
                ? AppConstants.OrderStatus.ChoThanhToan
                : AppConstants.OrderStatus.ChoXacNhan;

            var donHang = new DonHangOnline
            {
                MaDH = maDH,
                NgayDat = DateTime.UtcNow,
                MaKH = khachHang.MaKH,
                TenNguoiNhan = request.TenNguoiNhan.Trim(),
                SdtNguoiNhan = sdtNorm,
                DiaChiGiao = request.DiaChiGiao.Trim(),
                PhuongThucThanhToan = phuongThuc,
                TongTien = tongTien,
                TrangThai = initialStatus
            };

            _context.DonHangOnlines.Add(donHang);
            await _context.SaveChangesAsync();

            // 5. GÃ¡n MaDH cho chi tiáº¿t vÃ  lÆ°u
            foreach (var ct in chiTietList)
            {
                ct.MaDH = maDH;
                _context.ChiTietDonHangOnlines.Add(ct);
            }
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            if (_emailService != null && !string.IsNullOrWhiteSpace(khachHang.Email) && !khachHang.Email.EndsWith("@cellphones.customer"))
            {
                var emailItems = chiTietList.Select(ct => new OrderItemEmailDto
                {
                    TenHang = _context.MatHangs.FirstOrDefault(m => m.MaHang == ct.MaHang)?.TenHang ?? ct.MaHang,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                }).ToList();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendOrderConfirmationEmailAsync(
                            khachHang.Email,
                            donHang.TenNguoiNhan,
                            donHang.MaDH,
                            donHang.TongTien,
                            emailItems
                        );
                    }
                    catch { /* KhÃ´ng cháº·n luá»“ng náº¿u gá»­i mail gáº·p sá»± cá»‘ */ }
                });
            }

            
            // PhÃ¡t tÃ­n hiá»‡u Realtime qua SignalR Ä‘áº¿n AdminGroup
            if (_hubContext != null)
            {
                var hubPayload = new
                {
                    maDH = donHang.MaDH,
                    tenNguoiNhan = donHang.TenNguoiNhan,
                    sdtNguoiNhan = donHang.SdtNguoiNhan,
                    diaChiGiao = donHang.DiaChiGiao,
                    tongTien = donHang.TongTien,
                    trangThai = donHang.TrangThai,
                    phuongThucThanhToan = donHang.PhuongThucThanhToan,
                    ngayDat = donHang.NgayDat,
                    message = donHang.TrangThai == AppConstants.OrderStatus.ChoThanhToan
                        ? $"Khách hàng {donHang.TenNguoiNhan} vừa tạo đơn hàng #{donHang.MaDH} (Chờ quét mã thanh toán online {donHang.TongTien:N0}đ)"
                        : $"Khách hàng {donHang.TenNguoiNhan} ({donHang.SdtNguoiNhan}) vừa đặt đơn hàng COD mới #{donHang.MaDH} trị giá {donHang.TongTien:N0}đ",
                    items = chiTietList.Select(ct => new
                    {
                        maHang = ct.MaHang,
                        tenHang = ct.MatHang?.TenHang ?? ct.MaHang,
                        soLuong = ct.SoLuong,
                        donGia = ct.DonGia,
                        thanhTien = ct.SoLuong * ct.DonGia
                    }).ToList()
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _hubContext.Clients.Group(OrderHub.AdminGroup).SendAsync("ReceiveNewOrder", hubPayload);
                        await _hubContext.Clients.All.SendAsync("OrderCreatedNotification", hubPayload);
                    }
                    catch { }
                });
            }

            return new OrderDto
            {
                MaDH = donHang.MaDH,
                NgayDat = donHang.NgayDat,
                MaKH = donHang.MaKH,
                TenNguoiNhan = donHang.TenNguoiNhan,
                SdtNguoiNhan = donHang.SdtNguoiNhan,
                DiaChiGiao = donHang.DiaChiGiao,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                Items = chiTietList.Select(ct => new OrderItemDto
                {
                    MaHang = ct.MaHang,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.SoLuong * ct.DonGia
                }).ToList()
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderDto?> GetOrderByMaDHAsync(string maDH, string? sdt)
    {
        var query = _context.DonHangOnlines.AsNoTracking()
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.MatHang)
            .Where(d => d.MaDH == maDH);

        var order = await query.FirstOrDefaultAsync();
        if (order == null) return null;

        // Náº¿u client cung cáº¥p SÄT, pháº£i khá»›p vá»›i SdtNguoiNhan. Náº¿u khÃ´ng khá»›p tráº£ null Ä‘á»ƒ controller nÃ©m 404 (chá»‘ng dÃ² mÃ£ Ä‘Æ¡n P4-06)
        if (!string.IsNullOrWhiteSpace(sdt))
        {
            var cleanPhone = sdt.Trim().Replace(" ", "");
            if (!string.Equals(order.SdtNguoiNhan, cleanPhone, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
        }

        return new OrderDto
        {
            MaDH = order.MaDH,
            NgayDat = order.NgayDat,
            MaKH = order.MaKH,
            TenNguoiNhan = order.TenNguoiNhan,
            SdtNguoiNhan = order.SdtNguoiNhan,
            DiaChiGiao = order.DiaChiGiao,
            PhuongThucThanhToan = order.PhuongThucThanhToan,
            TongTien = order.TongTien,
            TrangThai = order.TrangThai,
            SoHD = order.SoHD,
            Items = order.ChiTietDonHangs.Select(c => new OrderItemDto
            {
                MaHang = c.MaHang,
                TenHang = c.MatHang?.TenHang ?? c.MaHang,
                SoLuong = c.SoLuong,
                DonGia = c.DonGia,
                ThanhTien = c.SoLuong * c.DonGia
            }).ToList()
        };
    }

    public async Task<List<OrderDto>> GetOrdersByPhoneAsync(string sdt)
    {
        var cleanPhone = sdt.Trim().Replace(" ", "");
        var orders = await _context.DonHangOnlines.AsNoTracking()
            .Where(d => d.SdtNguoiNhan == cleanPhone)
            .OrderByDescending(d => d.NgayDat)
            .ToListAsync();

        return orders.Select(o => new OrderDto
        {
            MaDH = o.MaDH,
            NgayDat = o.NgayDat,
            MaKH = o.MaKH,
            TenNguoiNhan = o.TenNguoiNhan,
            SdtNguoiNhan = o.SdtNguoiNhan,
            DiaChiGiao = o.DiaChiGiao,
            PhuongThucThanhToan = o.PhuongThucThanhToan,
            TongTien = o.TongTien,
            TrangThai = o.TrangThai,
            SoHD = o.SoHD
        }).ToList();
    }

    public async Task<(List<OrderDto> Items, int Total)> GetAdminOrdersAsync(
        string? trangThai, DateOnly? tuNgay, DateOnly? denNgay, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);

        var query = _context.DonHangOnlines.AsNoTracking()
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.MatHang)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "all")
        {
            query = query.Where(d => d.TrangThai == trangThai);
        }

        if (tuNgay.HasValue)
        {
            var fromUtc = tuNgay.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(d => d.NgayDat >= fromUtc);
        }

        if (denNgay.HasValue)
        {
            var toUtc = denNgay.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(d => d.NgayDat <= toUtc);
        }

        int total = await query.CountAsync();

        var list = await query
            .OrderByDescending(d => d.NgayDat)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = list.Select(o => new OrderDto
        {
            MaDH = o.MaDH,
            NgayDat = o.NgayDat,
            MaKH = o.MaKH,
            TenNguoiNhan = o.TenNguoiNhan,
            SdtNguoiNhan = o.SdtNguoiNhan,
            DiaChiGiao = o.DiaChiGiao,
            PhuongThucThanhToan = o.PhuongThucThanhToan,
            TongTien = o.TongTien,
            TrangThai = o.TrangThai,
            MaNV = o.MaNV,
            SoHD = o.SoHD,
            Items = o.ChiTietDonHangs.Select(c => new OrderItemDto
            {
                MaHang = c.MaHang,
                TenHang = c.MatHang?.TenHang ?? c.MaHang,
                SoLuong = c.SoLuong,
                DonGia = c.DonGia,
                ThanhTien = c.SoLuong * c.DonGia
            }).ToList()
        }).ToList();

        return (dtos, total);
    }

    public async Task<OrderDto> UpdateOrderStatusAsync(string maDH, string newTrangThai, string? soHDKem = null)
    {
        var maNV = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNV))
        {
            maNV = await _context.NhanViens.Select(n => n.MaNV).FirstOrDefaultAsync() ?? "NV001";
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var donHang = await _context.DonHangOnlines
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MatHang)
                .FirstOrDefaultAsync(d => d.MaDH == maDH);

            if (donHang == null)
            {
                throw new NotFoundException($"KhÃ´ng tÃ¬m tháº¥y Ä‘Æ¡n hÃ ng {maDH}");
            }

            var curr = donHang.TrangThai;

            // Kiá»ƒm tra mÃ¡y tráº¡ng thÃ¡i há»£p lá»‡ theo P4-07
            // Hỗ trợ luồng Chờ thanh toán -> Đã xác nhận / Chờ xác nhận / Đã hủy
            bool validTransition = false;
            if (curr == AppConstants.OrderStatus.ChoThanhToan && (newTrangThai == AppConstants.OrderStatus.DaXacNhan || newTrangThai == AppConstants.OrderStatus.ChoXacNhan || newTrangThai == AppConstants.OrderStatus.DaHuy)) validTransition = true;
            else if (curr == AppConstants.OrderStatus.ChoXacNhan && newTrangThai == AppConstants.OrderStatus.DaXacNhan) validTransition = true;
            else if (curr == AppConstants.OrderStatus.DaXacNhan && newTrangThai == AppConstants.OrderStatus.DangGiao) validTransition = true;
            else if (curr == AppConstants.OrderStatus.DangGiao && newTrangThai == AppConstants.OrderStatus.HoanThanh) validTransition = true;
            else if ((curr == AppConstants.OrderStatus.ChoThanhToan || curr == AppConstants.OrderStatus.ChoXacNhan || curr == AppConstants.OrderStatus.DaXacNhan) && newTrangThai == AppConstants.OrderStatus.DaHuy) validTransition = true;

            if (!validTransition)
            {
                throw new ConflictException($"KhÃ´ng thá»ƒ chuyá»ƒn tráº¡ng thÃ¡i Ä‘Æ¡n hÃ ng tá»« '{curr}' sang '{newTrangThai}'.");
            }

            // Ghi nháº­n MaNV khi xÃ¡c nháº­n Ä‘Æ¡n
            if (newTrangThai == AppConstants.OrderStatus.DaXacNhan)
            {
                donHang.MaNV = maNV;
            }

            // Xá»­ lÃ½ khi hoÃ n thÃ nh Ä‘Æ¡n:
            // Tá»± Ä‘á»™ng gÃ¡n IMEI cÅ© nháº¥t (theo phiáº¿u nháº­p), táº¡o hÃ³a Ä‘Æ¡n online, táº¡o báº£o hÃ nh
            if (newTrangThai == AppConstants.OrderStatus.HoanThanh)
            {
                string soHD = donHang.SoHD ?? soHDKem ?? string.Empty;

                if (string.IsNullOrEmpty(soHD))
                {
                    // Táº¡o hÃ³a Ä‘Æ¡n má»›i kÃªnh online
                    soHD = await _codeGenerator.NextAsync("HD");
                    var ptThanhToan = donHang.PhuongThucThanhToan == "cod" ? "tien_mat" : donHang.PhuongThucThanhToan;

                    var hoaDon = new HoaDon
                    {
                        SoHD = soHD,
                        NgayLap = DateTime.UtcNow,
                        MaKH = donHang.MaKH,
                        MaNV = maNV,
                        KenhBan = AppConstants.SaleChannels.Online,
                        TongTien = donHang.TongTien,
                        TienGiamGia = 0m,
                        PhuongThucThanhToan = ptThanhToan
                    };
                    _context.HoaDons.Add(hoaDon);
                    await _context.SaveChangesAsync();

                    // Xá»­ lÃ½ tá»«ng dÃ²ng chi tiáº¿t Ä‘Æ¡n
                    foreach (var item in donHang.ChiTietDonHangs)
                    {
                        var matHang = item.MatHang ?? await _context.MatHangs.FirstOrDefaultAsync(m => m.MaHang == item.MaHang);
                        if (matHang != null && matHang.QuanLyImei)
                        {
                            for (int i = 0; i < item.SoLuong; i++)
                            {
                                // Chá»n IMEI con_hang theo phiáº¿u nháº­p cÅ© trÆ°á»›c (FIFO dá»±a vÃ o SoPN hoáº·c MaIMEI)
                                var imei = await (from im in _context.IMEIs
                                                  join pn in _context.PhieuNhaps on im.SoPN equals pn.SoPN into pnGroup
                                                  from pn in pnGroup.DefaultIfEmpty()
                                                  where im.MaHang == item.MaHang && im.TrangThai == "con_hang"
                                                  orderby pn != null ? pn.NgayNhap : DateTime.MinValue, im.MaIMEI
                                                  select im).FirstOrDefaultAsync();

                                if (imei == null)
                                {
                                    // Tá»± Ä‘á»™ng sinh phiáº¿u nháº­p vÃ  IMEI náº¿u sáº£n pháº©m chÆ°a cÃ³ sáºµn trong kho
                                    var defaultNCC = await _context.NhaCungCaps.FirstOrDefaultAsync();
                                    string autoSoPN = await _codeGenerator.NextAsync("PN");
                                    var pn = new PhieuNhap
                                    {
                                        SoPN = autoSoPN,
                                        NgayNhap = DateTime.UtcNow,
                                        MaNCC = defaultNCC?.MaNCC ?? "NCC001",
                                        MaNV = "NV001",
                                    };
                                    _context.PhieuNhaps.Add(pn);
                                    _context.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                                    {
                                        SoPN = autoSoPN,
                                        MaHang = item.MaHang,
                                        SlNhap = 1,
                                        DonGiaNhap = item.DonGia
                                    });
                                    string autoSoImei = "86" + Random.Shared.NextInt64(1000000000000L, 9999999999999L).ToString();
                                    imei = new IMEI
                                    {
                                        MaIMEI = autoSoImei,
                                        SoIMEI = autoSoImei,
                                        MaHang = item.MaHang,
                                        SoPN = autoSoPN,
                                        TrangThai = "con_hang"
                                    };
                                    _context.IMEIs.Add(imei);
                                    await _context.SaveChangesAsync();
                                }

                                // Äá»‘i vá»›i PostgreSQL, trigger fn_trg_chitiethoadon_imei_ban tá»± Ä‘á»™ng kiá»ƒm tra 'con_hang' vÃ  cáº­p nháº­t 'da_ban'
                                if (!_context.Database.IsRelational())
                                {
                                    imei.TrangThai = "da_ban";
                                }

                                var ctHD = new ChiTietHoaDon
                                {
                                    SoHD = soHD,
                                    MaHang = item.MaHang,
                                    MaIMEI = imei.MaIMEI,
                                    SlBan = 1,
                                    DonGiaBan = item.DonGia
                                };
                                _context.ChiTietHoaDons.Add(ctHD);
                                await _context.SaveChangesAsync();

                                // Táº¡o báº£o hÃ nh
                                await _warrantyService.CreateForInvoiceAsync(soHD, imei.MaIMEI, item.MaHang);
                            }
                        }
                        else
                        {
                            var ctHD = new ChiTietHoaDon
                            {
                                SoHD = soHD,
                                MaHang = item.MaHang,
                                MaIMEI = null,
                                SlBan = item.SoLuong,
                                DonGiaBan = item.DonGia
                            };
                            _context.ChiTietHoaDons.Add(ctHD);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                donHang.SoHD = soHD;
            }

            donHang.TrangThai = newTrangThai;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Giai đoạn 6: Khi xác nhận đơn -> tạo vận đơn (lỗi vận chuyển không làm hỏng đơn)
            if (newTrangThai == AppConstants.OrderStatus.DaXacNhan && _shippingService != null)
            {
                try
                {
                    await _shippingService.CreateShipmentAsync(donHang, donHang.ChiTietDonHangs.ToList());
                }
                catch { /* Không chặn luồng xác nhận đơn nếu API vận chuyển gặp sự cố */ }
            }

            if (_hubContext != null)
            {
                string msg = newTrangThai switch
                {
                    AppConstants.OrderStatus.ChoThanhToan => $"Đơn đặt hàng #{donHang.MaDH} đang chờ quét mã thanh toán online.",
                    AppConstants.OrderStatus.DaXacNhan => $"ÄÆ¡n Ä‘áº·t hÃ ng #{donHang.MaDH} Ä‘Ã£ Ä‘Æ°á»£c Admin xÃ¡c nháº­n thÃ nh cÃ´ng! Äang chuáº©n bá»‹ Ä‘Ã³ng gÃ³i.",
                    AppConstants.OrderStatus.DangGiao => $"ÄÆ¡n Ä‘áº·t hÃ ng #{donHang.MaDH} Ä‘Ã£ Ä‘Æ°á»£c xuáº¥t kho vÃ  Ä‘ang trÃªn Ä‘Æ°á»ng giao Ä‘áº¿n báº¡n!",
                    AppConstants.OrderStatus.HoanThanh => $"ÄÆ¡n Ä‘áº·t hÃ ng #{donHang.MaDH} Ä‘Ã£ Ä‘Æ°á»£c giao thÃ nh cÃ´ng vÃ  hoÃ n táº¥t! Cáº£m Æ¡n báº¡n Ä‘Ã£ mua hÃ ng.",
                    AppConstants.OrderStatus.DaHuy => $"ÄÆ¡n Ä‘áº·t hÃ ng #{donHang.MaDH} Ä‘Ã£ Ä‘Æ°á»£c cáº­p nháº­t tráº¡ng thÃ¡i: ÄÃ£ há»§y.",
                    _ => $"ÄÆ¡n Ä‘áº·t hÃ ng #{donHang.MaDH} Ä‘Ã£ Ä‘Æ°á»£c cáº­p nháº­t tráº¡ng thÃ¡i má»›i."
                };

                var statusPayload = new
                {
                    maDH = donHang.MaDH,
                    trangThai = donHang.TrangThai,
                    soHD = donHang.SoHD,
                    statusText = msg,
                    message = msg,
                    sdt = donHang.SdtNguoiNhan,
                    tenNguoiNhan = donHang.TenNguoiNhan,
                    tongTien = donHang.TongTien,
                    updatedAt = DateTime.UtcNow
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _hubContext.Clients.All.SendAsync("OrderStatusUpdated", statusPayload);
                    }
                    catch { }
                });
            }

            return new OrderDto
            {
                MaDH = donHang.MaDH,
                NgayDat = donHang.NgayDat,
                MaKH = donHang.MaKH,
                TenNguoiNhan = donHang.TenNguoiNhan,
                SdtNguoiNhan = donHang.SdtNguoiNhan,
                DiaChiGiao = donHang.DiaChiGiao,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                MaNV = donHang.MaNV,
                SoHD = donHang.SoHD
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderDto> CancelOrderByCustomerAsync(string maDH, CancelOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(maDH))
        {
            throw new BadRequestException("MÃ£ Ä‘Æ¡n hÃ ng khÃ´ng há»£p lá»‡.");
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH.Trim());
        if (donHang == null)
        {
            throw new NotFoundException($"KhÃ´ng tÃ¬m tháº¥y Ä‘Æ¡n hÃ ng {maDH}");
        }

        // Kiá»ƒm tra sá»‘ Ä‘iá»‡n thoáº¡i ngÆ°á»i nháº­n Ä‘á»ƒ xÃ¡c thá»±c
        var sdtNorm = (request.Sdt ?? string.Empty).Trim().Replace(" ", "").Replace(".", "").Replace("-", "");
        if (!string.IsNullOrEmpty(sdtNorm) && !donHang.SdtNguoiNhan.Equals(sdtNorm, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng khá»›p vá»›i thÃ´ng tin Ä‘Æ¡n hÃ ng.");
        }

        // Chá»‰ cho phÃ©p há»§y khi Ä‘Æ¡n chÆ°a giao
        if (donHang.TrangThai == AppConstants.OrderStatus.DangGiao || donHang.TrangThai == AppConstants.OrderStatus.HoanThanh)
        {
            throw new BadRequestException("ÄÆ¡n hÃ ng Ä‘ang giao hoáº·c Ä‘Ã£ hoÃ n thÃ nh, khÃ´ng thá»ƒ tá»± há»§y trá»±c tuyáº¿n. Vui lÃ²ng liÃªn há»‡ hotline.");
        }

        if (donHang.TrangThai == AppConstants.OrderStatus.DaHuy)
        {
            throw new BadRequestException("ÄÆ¡n hÃ ng nÃ y Ä‘Ã£ Ä‘Æ°á»£c há»§y trÆ°á»›c Ä‘Ã³.");
        }

        donHang.TrangThai = AppConstants.OrderStatus.DaHuy;

        await _context.SaveChangesAsync();

        if (_hubContext != null)
        {
            string cancelMsg = $"KhÃ¡ch hÃ ng {donHang.TenNguoiNhan} ({donHang.SdtNguoiNhan}) Ä‘Ã£ há»§y Ä‘Æ¡n hÃ ng #{donHang.MaDH}" +
                               (!string.IsNullOrWhiteSpace(request.LyDo) ? $" (LÃ½ do: {request.LyDo})" : "");
            var cancelPayload = new
            {
                maDH = donHang.MaDH,
                trangThai = donHang.TrangThai,
                statusText = cancelMsg,
                message = cancelMsg,
                sdt = donHang.SdtNguoiNhan,
                tenNguoiNhan = donHang.TenNguoiNhan,
                tongTien = donHang.TongTien,
                updatedAt = DateTime.UtcNow
            };

            _ = Task.Run(async () =>
            {
                try
                {
                    await _hubContext.Clients.All.SendAsync("OrderStatusUpdated", cancelPayload);
                    await _hubContext.Clients.Group(OrderHub.AdminGroup).SendAsync("ReceiveOrderCancelled", cancelPayload);
                }
                catch { }
            });
        }

        // Gá»­i email thÃ´ng bÃ¡o há»§y náº¿u cÃ³ email khÃ¡ch
        if (_emailService != null)
        {
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == donHang.MaKH);
            if (kh != null && !string.IsNullOrWhiteSpace(kh.Email) && !kh.Email.EndsWith("@cellphones.customer"))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        string body = $"<p>Xin chÃ o <strong>{donHang.TenNguoiNhan}</strong>,</p>" +
                                      $"<p>ÄÆ¡n hÃ ng <strong>#{donHang.MaDH}</strong> Ä‘Ã£ Ä‘Æ°á»£c há»§y thÃ nh cÃ´ng theo yÃªu cáº§u cá»§a báº¡n.</p>" +
                                      (!string.IsNullOrEmpty(request.LyDo) ? $"<p>LÃ½ do há»§y: {request.LyDo}</p>" : "") +
                                      $"<p>Cáº£m Æ¡n báº¡n Ä‘Ã£ Ä‘á»“ng hÃ nh cÃ¹ng Sama Mobile!</p>";
                        await _emailService.SendEmailAsync(kh.Email, $"[Sama Mobile] ÄÃ£ há»§y Ä‘Æ¡n hÃ ng #{donHang.MaDH}", body);
                    }
                    catch { }
                });
            }
        }

        return new OrderDto
        {
            MaDH = donHang.MaDH,
            NgayDat = donHang.NgayDat,
            MaKH = donHang.MaKH,
            TenNguoiNhan = donHang.TenNguoiNhan,
            SdtNguoiNhan = donHang.SdtNguoiNhan,
            DiaChiGiao = donHang.DiaChiGiao,
            PhuongThucThanhToan = donHang.PhuongThucThanhToan,
            TongTien = donHang.TongTien,
            TrangThai = donHang.TrangThai,
            MaNV = donHang.MaNV,
            SoHD = donHang.SoHD
        };
    }


    // ─── Giai đoạn 2: Tra cứu công khai (đã mask thông tin nhạy cảm) ─────────────

    /// <summary>
    /// Tra cứu đơn hàng theo mã đơn + SĐT để xác minh, trả dữ liệu đã mask.
    /// Trả null nếu không tìm thấy hoặc SĐT không khớp (tránh lộ lý do cụ thể).
    /// </summary>
    public async Task<OrderLookupDto?> GetOrderForLookupAsync(string maDH, string sdt)
    {
        var cleanPhone = PhoneHelper.NormalizeVietnamesePhone(sdt)
                         ?? sdt.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

        var order = await _context.DonHangOnlines.AsNoTracking()
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.MatHang)
            .FirstOrDefaultAsync(d => d.MaDH == maDH);

        // Trả null nếu không tìm thấy hoặc SĐT không khớp — không tiết lộ sai mã hay sai SĐT (bảo mật P4-06)
        if (order == null) return null;
        if (!string.Equals(order.SdtNguoiNhan, cleanPhone, StringComparison.OrdinalIgnoreCase)) return null;

        // Lấy email khách hàng để mask (nếu có)
        var kh = await _context.KhachHangs.AsNoTracking()
            .FirstOrDefaultAsync(k => k.MaKH == order.MaKH);
        var emailRaw = kh?.Email?.EndsWith("@cellphones.customer") == true ? null : kh?.Email;

        // Lấy thông tin vận chuyển (Giai đoạn 6)
        OrderShippingDto? shipping = null;
        if (_shippingService != null)
        {
            try { shipping = await _shippingService.GetTrackingAsync(order.MaDH); }
            catch { /* Giữ lookup hoạt động ngay cả khi dịch vụ vận chuyển lỗi */ }
        }

        return new OrderLookupDto
        {
            MaDH = order.MaDH,
            NgayDat = order.NgayDat,
            TenNguoiNhanMask = OrderMaskHelper.MaskName(order.TenNguoiNhan),
            SdtMask = OrderMaskHelper.MaskPhone(order.SdtNguoiNhan),
            EmailMask = emailRaw != null ? OrderMaskHelper.MaskEmail(emailRaw) : null,
            DiaChiMask = OrderMaskHelper.MaskAddress(order.DiaChiGiao),
            PhuongThucThanhToan = order.PhuongThucThanhToan,
            TongTien = order.TongTien,
            TrangThai = order.TrangThai,
            Items = order.ChiTietDonHangs.Select(c => new OrderItemDto
            {
                MaHang = c.MaHang,
                TenHang = c.MatHang?.TenHang ?? c.MaHang,
                SoLuong = c.SoLuong,
                DonGia = c.DonGia,
                ThanhTien = c.SoLuong * c.DonGia
            }).ToList(),
            // Tạm thời lịch sử chỉ có 1 entry = trạng thái hiện tại
            StatusHistory = new List<OrderStatusHistoryEntry>
            {
                new() { TrangThai = order.TrangThai, ThoiGian = order.NgayDat }
            },
            Shipping = shipping
        };
    }

    // ─── Giai đoạn 4: Tra cứu danh sách đơn theo SĐT (đã mask) ─────────────────

    /// <summary>
    /// Trả danh sách đơn của SĐT đã xác thực OTP, với thông tin đã mask.
    /// </summary>
    public async Task<List<OrderLookupDto>> GetOrdersByPhoneMaskedAsync(string phone)
    {
        var cleanPhone = PhoneHelper.NormalizeVietnamesePhone(phone)
                         ?? phone.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

        var orders = await _context.DonHangOnlines.AsNoTracking()
            .Where(d => d.SdtNguoiNhan == cleanPhone)
            .OrderByDescending(d => d.NgayDat)
            .ToListAsync();

        // Lấy email từ khách hàng (chỉ cần 1 lần, dùng MaKH đầu tiên nếu có)
        string? emailRaw = null;
        if (orders.Count > 0)
        {
            var kh = await _context.KhachHangs.AsNoTracking()
                .FirstOrDefaultAsync(k => k.MaKH == orders[0].MaKH);
            emailRaw = kh?.Email?.EndsWith("@cellphones.customer") == true ? null : kh?.Email;
        }

        var result = new List<OrderLookupDto>(orders.Count);
        foreach (var o in orders)
        {
            OrderShippingDto? shipping = null;
            if (_shippingService != null)
            {
                try { shipping = await _shippingService.GetTrackingAsync(o.MaDH); }
                catch { }
            }

            result.Add(new OrderLookupDto
            {
                MaDH = o.MaDH,
                NgayDat = o.NgayDat,
                TenNguoiNhanMask = OrderMaskHelper.MaskName(o.TenNguoiNhan),
                SdtMask = OrderMaskHelper.MaskPhone(o.SdtNguoiNhan),
                EmailMask = emailRaw != null ? OrderMaskHelper.MaskEmail(emailRaw) : null,
                DiaChiMask = OrderMaskHelper.MaskAddress(o.DiaChiGiao),
                PhuongThucThanhToan = o.PhuongThucThanhToan,
                TongTien = o.TongTien,
                TrangThai = o.TrangThai,
                StatusHistory = new List<OrderStatusHistoryEntry>
                {
                    new() { TrangThai = o.TrangThai, ThoiGian = o.NgayDat }
                },
                Shipping = shipping
            });
        }

        return result;
    }
}