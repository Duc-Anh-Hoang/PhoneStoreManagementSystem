using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Features.OnlineOrders.Helpers;
using PhoneStoreAPI.Features.OnlineOrders.Services;
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
    private readonly IOrderDecisionEngine? _decisionEngine;
    private readonly IOrderSettingsService? _settings;

    public OnlineOrderService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        IWarrantyService warrantyService,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null,
        IShippingService? shippingService = null,
        IOrderDecisionEngine? decisionEngine = null,
        IOrderSettingsService? settings = null)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _warrantyService = warrantyService;
        _emailService = emailService;
        _hubContext = hubContext;
        _shippingService = shippingService;
        _decisionEngine = decisionEngine;
        _settings = settings;
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

            // plan (8).md: Quyết định trạng thái ban đầu bằng Engine
            var addressHash = AddressHashHelper.ComputeAddressHash(request.DiaChiGiao);
            var decisionContext = new OrderDecisionContext
            {
                Phone = sdtNorm,
                AddressHash = addressHash,
                OrderAmount = tongTien,
                PaymentMethod = phuongThuc,
                IsOnlinePaid = false
            };

            var decision = _decisionEngine != null
                ? await _decisionEngine.DecideInitialStatusAsync(decisionContext)
                : new OrderDecisionResult { InitialStatus = (phuongThuc == "chuyen_khoan" || phuongThuc == "the") ? AppConstants.OrderStatus.ChoThanhToan : AppConstants.OrderStatus.ChoXacNhan };

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
                TrangThai = decision.InitialStatus,
                RiskFlag = decision.RiskFlag,
                RiskReasons = decision.RiskReasons.Count > 0 ? JsonSerializer.Serialize(decision.RiskReasons) : null,
                AutoApproveAt = decision.AutoApproveAt,
                AddressHash = addressHash,
                PaymentStatus = (phuongThuc == "chuyen_khoan" || phuongThuc == "the") ? "unpaid" : "cod"
            };

            _context.DonHangOnlines.Add(donHang);
            await _context.SaveChangesAsync();

            // Ghi log trạng thái khởi tạo
            _context.OrderStatusLogs.Add(new OrderStatusLog
            {
                OrderId = maDH,
                FromStatus = null,
                ToStatus = decision.InitialStatus,
                Actor = "customer",
                Reason = decision.RiskFlag
                    ? $"Đơn được tạo kèm cờ rủi ro: {string.Join(", ", decision.RiskReasons)}"
                    : (decision.InitialStatus == "cho_tu_duyet" ? $"Khách tin cậy, hẹn tự duyệt sau 15 phút ({decision.AutoApproveAt:HH:mm})" : "Đơn hàng mới tạo")
            });
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
            RiskFlag = o.RiskFlag,
            RiskReasons = o.RiskReasons,
            AutoApproveAt = o.AutoApproveAt,
            GhnOrderCode = o.GhnOrderCode,
            GhnError = o.GhnError,
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

            // Kiểm tra máy trạng thái hợp lệ theo mục 4 plan (8).md
            bool validTransition = IsValidTransition(curr, newTrangThai);
            if (!validTransition)
            {
                throw new ConflictException($"Không thể chuyển trạng thái đơn hàng từ '{curr}' sang '{newTrangThai}'.");
            }

            // Bước 7: Hủy đơn đồng bộ với GHN
            if (newTrangThai == AppConstants.OrderStatus.DaHuy && _shippingService != null)
            {
                var (canCancel, cancelErr) = await _shippingService.CancelShipmentAsync(maDH);
                if (!canCancel)
                {
                    throw new ConflictException(cancelErr ?? "Không thể hủy đơn hàng do hệ thống vận chuyển từ chối.");
                }
            }

            // Ghi nhận MaNV khi xác nhận đơn
            if (newTrangThai == AppConstants.OrderStatus.DaXacNhan)
            {
                donHang.MaNV = maNV;
            }

            // Xá»­ lÃ½ khi hoÃ n thÃ nh Ä‘Æ¡n:
            // Tá»± Ä‘á»™ng gÃ¡n IMEI cÅ© nháº¥t (theo phiáº¿u nháº­p), táº¡o hÃ³a Ä‘Æ¡n online, táº¡o báº£o hÃ nh
            if (newTrangThai == AppConstants.OrderStatus.HoanThanh)
            {
                // Kiểm tra xem SoHD có tồn tại hợp lệ trong CSDL không
                string candidateSoHD = donHang.SoHD ?? soHDKem ?? string.Empty;
                bool invoiceExists = !string.IsNullOrEmpty(candidateSoHD) && await _context.HoaDons.AnyAsync(h => h.SoHD == candidateSoHD);
                string soHD = invoiceExists ? candidateSoHD : string.Empty;

                if (string.IsNullOrEmpty(soHD))
                {
                    // Tạo hóa đơn mới hợp lệ trong CSDL
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
                donHang.PaymentStatus = "paid";
            }

            donHang.TrangThai = newTrangThai;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Ghi log chuyển trạng thái
            _context.OrderStatusLogs.Add(new OrderStatusLog
            {
                OrderId = donHang.MaDH,
                FromStatus = curr,
                ToStatus = newTrangThai,
                Actor = string.IsNullOrWhiteSpace(maNV) ? "system" : "staff",
                Reason = $"Cập nhật trạng thái sang {newTrangThai}"
            });
            await _context.SaveChangesAsync();

            // Giai đoạn 6 & Phase 5: Khi xác nhận đơn -> tạo vận đơn GHN
            if (newTrangThai == AppConstants.OrderStatus.DaXacNhan && _shippingService != null)
            {
                try
                {
                    var shipment = await _shippingService.CreateShipmentAsync(donHang, donHang.ChiTietDonHangs.ToList());
                    if (!string.IsNullOrWhiteSpace(shipment?.TrackingCode))
                    {
                        donHang.GhnOrderCode = shipment.TrackingCode;
                        donHang.GhnError = null;
                        donHang.TrangThai = "cho_lay_hang";
                        await _context.SaveChangesAsync();

                        _context.OrderStatusLogs.Add(new OrderStatusLog
                        {
                            OrderId = donHang.MaDH,
                            FromStatus = AppConstants.OrderStatus.DaXacNhan,
                            ToStatus = "cho_lay_hang",
                            Actor = "ghn",
                            Reason = $"Tạo vận đơn GHN thành công: #{shipment.TrackingCode}"
                        });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    donHang.GhnError = ex.Message;
                    donHang.TrangThai = "loi_van_don";
                    await _context.SaveChangesAsync();

                    _context.OrderStatusLogs.Add(new OrderStatusLog
                    {
                        OrderId = donHang.MaDH,
                        FromStatus = AppConstants.OrderStatus.DaXacNhan,
                        ToStatus = "loi_van_don",
                        Actor = "ghn",
                        Reason = $"Lỗi tạo vận đơn GHN: {ex.Message}"
                    });
                    await _context.SaveChangesAsync();
                }
            }

            // Phase 6 & Mục 3.7: Cập nhật độ tin cậy khách hàng khi HOAN_THANH
            if (newTrangThai == AppConstants.OrderStatus.HoanThanh)
            {
                var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == donHang.MaKH);
                if (kh != null)
                {
                    kh.CompletedOrders += 1;
                    var trustMinVal = _settings != null ? await _settings.GetTrustMinOrderValueAsync() : 1_000_000m;
                    if (donHang.TongTien >= trustMinVal)
                    {
                        kh.QualifiedOrders += 1;
                    }
                    kh.MaxCompletedValue = Math.Max(kh.MaxCompletedValue, donHang.TongTien);
                    kh.UpdatedAt = DateTime.UtcNow;

                    if (!string.IsNullOrWhiteSpace(donHang.AddressHash))
                    {
                        var hasAddr = await _context.CustomerAddresses.AnyAsync(a => a.Phone == donHang.SdtNguoiNhan && a.AddressHash == donHang.AddressHash);
                        if (!hasAddr)
                        {
                            _context.CustomerAddresses.Add(new CustomerAddress
                            {
                                Phone = donHang.SdtNguoiNhan,
                                AddressHash = donHang.AddressHash,
                                FullAddress = donHang.DiaChiGiao,
                                LastDeliveredAt = DateTime.UtcNow
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // Phase 6 & Mục 3.7: Cập nhật vi phạm khi BOOM / HOAN_HANG
            if (newTrangThai == "boom" || newTrangThai == "hoan_hang")
            {
                var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == donHang.MaKH);
                if (kh != null)
                {
                    kh.FailedOrders += 1;
                    var blacklistThreshold = _settings != null ? await _settings.GetBlacklistAfterFailedAsync() : 2;
                    if (kh.FailedOrders >= blacklistThreshold)
                    {
                        kh.IsBlacklisted = true;
                    }
                    kh.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }

            // Mục 3.6: Trả tồn kho khi hủy đơn đã tạo hóa đơn
            if (newTrangThai == AppConstants.OrderStatus.DaHuy && !string.IsNullOrEmpty(donHang.SoHD))
            {
                var ctHds = await _context.ChiTietHoaDons.Where(c => c.SoHD == donHang.SoHD && c.MaIMEI != null).ToListAsync();
                foreach (var ct in ctHds)
                {
                    var imei = await _context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == ct.MaIMEI);
                    if (imei != null) imei.TrangThai = "con_hang";
                }
                await _context.SaveChangesAsync();
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

    private static bool IsValidTransition(string from, string to)
    {
        if (string.Equals(from, to, StringComparison.OrdinalIgnoreCase)) return true;

        if (to == AppConstants.OrderStatus.DaHuy &&
            from != AppConstants.OrderStatus.DangGiao &&
            from != AppConstants.OrderStatus.HoanThanh &&
            from != "boom" &&
            from != "hoan_hang")
        {
            return true;
        }

        return (from.ToLowerInvariant(), to.ToLowerInvariant()) switch
        {
            ("cho_thanh_toan", "da_xac_nhan") => true,
            ("cho_thanh_toan", "cho_xac_nhan") => true,
            ("cho_thanh_toan", "can_kiem_tra") => true,

            ("cho_tu_duyet", "da_xac_nhan") => true,
            ("cho_tu_duyet", "cho_xac_nhan") => true,
            ("cho_tu_duyet", "can_kiem_tra") => true,

            ("cho_xac_nhan", "da_xac_nhan") => true,
            ("can_kiem_tra", "da_xac_nhan") => true,

            ("da_xac_nhan", "cho_lay_hang") => true,
            ("da_xac_nhan", "loi_van_don") => true,
            ("da_xac_nhan", "dang_giao") => true,
            ("da_xac_nhan", "hoan_thanh") => true,

            ("loi_van_don", "da_xac_nhan") => true,
            ("loi_van_don", "cho_lay_hang") => true,

            ("cho_lay_hang", "dang_giao") => true,
            ("cho_lay_hang", "hoan_thanh") => true,
            ("dang_giao", "hoan_thanh") => true,
            ("dang_giao", "boom") => true,
            ("dang_giao", "hoan_hang") => true,

            _ => false
        };
    }

    public async Task<BulkApproveResultDto> BulkApproveOrdersAsync(List<string>? orderCodes = null)
    {
        var query = _context.DonHangOnlines.AsQueryable();
        if (orderCodes != null && orderCodes.Count > 0)
        {
            var set = orderCodes.Select(c => c.Trim().ToUpperInvariant()).ToHashSet();
            query = query.Where(d => set.Contains(d.MaDH.ToUpper()) &&
                                     (d.TrangThai == AppConstants.OrderStatus.ChoXacNhan || d.TrangThai == AppConstants.OrderStatus.ChoThanhToan));
        }
        else
        {
            // Mặc định duyệt toàn bộ đơn đang ở trạng thái chờ xác nhận
            query = query.Where(d => d.TrangThai == AppConstants.OrderStatus.ChoXacNhan);
        }

        var candidateOrders = await query.OrderBy(d => d.NgayDat).Take(100).ToListAsync();
        var result = new BulkApproveResultDto();

        foreach (var ord in candidateOrders)
        {
            try
            {
                await UpdateOrderStatusAsync(ord.MaDH, AppConstants.OrderStatus.DaXacNhan);
                result.ApprovedCount++;
                result.ApprovedCodes.Add(ord.MaDH);
            }
            catch (Exception ex)
            {
                result.FailedCount++;
                result.ErrorMessages.Add($"Đơn #{ord.MaDH}: {ex.Message}");
            }
        }

        return result;
    }
}