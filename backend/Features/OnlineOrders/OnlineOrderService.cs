using PhoneStoreAPI.Features.Inventory;
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

    private readonly IHttpContextAccessor? _http;
    private readonly ShippingQuoteService? _quotes;

    public OnlineOrderService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        IWarrantyService warrantyService,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null,
        IShippingService? shippingService = null,
        IOrderDecisionEngine? decisionEngine = null,
        IOrderSettingsService? settings = null, IHttpContextAccessor? http = null, ShippingQuoteService? quotes = null)
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
        _http = http;
        _quotes = quotes;
    }

    private async Task<string?> GetClientIpAsync()
    {
        var http = _http?.HttpContext;
        var peer = http?.Connection.RemoteIpAddress;
        var proxyHost = Environment.GetEnvironmentVariable("STORE_TRUSTED_PROXY");
        if (peer == null || http == null || string.IsNullOrEmpty(proxyHost)) return peer?.ToString();
        var forwarded = http.Request.Headers["X-Forwarded-For"].ToString().Split(',')[^1].Trim();
        if (!System.Net.IPAddress.TryParse(forwarded, out var client)) return peer.ToString();
        try
        {
            var proxies = await System.Net.Dns.GetHostAddressesAsync(proxyHost);
            if (proxies.Any(p => p.MapToIPv6().Equals(peer.MapToIPv6()))) return client.ToString();
        }
        catch (System.Net.Sockets.SocketException) { }
        return peer.ToString();
    }

    private bool HasMemberOtp(string? token, string phone) =>
        !string.IsNullOrWhiteSpace(token) && _http?.HttpContext is { } context &&
        OtpTokenValidator.ValidateAndExtractPhone(token, context.RequestServices) == phone;

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Chuẩn hóa SĐT người nhận
            var sdtNorm = PhoneHelper.NormalizeVietnamesePhone(request.SdtNguoiNhan)
                          ?? request.SdtNguoiNhan.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

            var phuongThuc = request.PhuongThucThanhToan?.Trim().ToLowerInvariant() switch
            {
                "banking" or "chuyen_khoan" => "chuyen_khoan",
                "cod" or "tien_mat" => "cod",
                _ => throw new BadRequestException("Đơn online hỗ trợ VietQR và thanh toán khi nhận hàng."),
            };

            if (request.Items == null || request.Items.Count == 0 ||
                request.Items.Any(i => i.SoLuong <= 0 || string.IsNullOrWhiteSpace(i.MaHang)))
                throw new BadRequestException("Đơn hàng cần ít nhất một mặt hàng với số lượng dương.");
            var groupedItems = request.Items
                .Select(item => new
                {
                    MaHang = item.MaHang.Trim() switch
                    {
                        "SAMPLE-dien-thoai-1" => "MH112",
                        "SAMPLE-laptop-1" => "MH365",
                        var code => code
                    },
                    item.SoLuong
                })
                .GroupBy(item => item.MaHang)
                .Select(group => new { MaHang = group.Key, SoLuong = group.Sum(item => item.SoLuong) }).ToList();
            var reqMaDH = string.IsNullOrWhiteSpace(request.MaDH) ? null : request.MaDH.Trim().ToUpperInvariant();
            if (reqMaDH != null)
            {
                if (reqMaDH.Length > 15)
                {
                    throw new BadRequestException("Mã đơn hàng không hợp lệ.");
                }
                if (_context.Database.IsNpgsql())
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({reqMaDH}, 1))");
                }
                var existing = await GetOrderByMaDHAsync(reqMaDH, null);
                if (existing != null)
                {
                    var previous = await _context.DonHangOnlines.AsNoTracking().SingleAsync(d => d.MaDH == reqMaDH);
                    if (previous.GhiChu != request.GhiChu?.Trim() || previous.VoucherCode != request.VoucherCode?.Trim().ToUpperInvariant() ||
                        (request.ExpectedTotal.HasValue && existing.TongTien != request.ExpectedTotal) ||
                        existing.SdtNguoiNhan != sdtNorm || existing.TenNguoiNhan != request.TenNguoiNhan.Trim() ||
                        existing.DiaChiGiao != request.DiaChiGiao.Trim() || existing.PhuongThucThanhToan != phuongThuc ||
                        existing.Items == null || existing.Items.Count != groupedItems.Count || groupedItems.Any(item =>
                            !existing.Items.Any(line => line.MaHang == item.MaHang && line.SoLuong == item.SoLuong)))
                    {
                        throw new ConflictException("Mã đơn đã được dùng cho thông tin đặt hàng khác.");
                    }
                    await transaction.CommitAsync();
                    return existing;
                }
            }

            // 2. Tìm hoặc tạo khách hàng
            KhachHang? khachHang = null;
            if (!string.IsNullOrWhiteSpace(request.MaKH))
            {
                khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == request.MaKH);
            }

            if (khachHang == null)
            {
                khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == sdtNorm);
            }

            var registeredMember = khachHang != null;
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

            // 3. Kiểm tra sản phẩm và tính tổng tiền từ DonGia DB
            decimal tongTien = 0m;
            var chiTietList = new List<ChiTietDonHangOnline>();

            await SellableStock.LockItemsAsync(_context, groupedItems.Select(item => item.MaHang));
            foreach (var item in groupedItems)
            {
                var matHang = await _context.MatHangs.FirstOrDefaultAsync(m => m.MaHang == item.MaHang)
                    ?? throw new NotFoundException($"Mặt hàng {item.MaHang} không tồn tại.");
                await SellableStock.EnsureAvailableAsync(_context, matHang, item.SoLuong);
                var donGiaDb = matHang.DonGia;
                tongTien += donGiaDb * item.SoLuong;

                chiTietList.Add(new ChiTietDonHangOnline
                {
                    MaHang = matHang.MaHang,
                    SoLuong = item.SoLuong,
                    DonGia = donGiaDb
                });
            }

            // 3b. Tính toán và xác minh ưu đãi VIP độc lập từ Backend theo chính sách chốt
            var sdtMember = !string.IsNullOrWhiteSpace(request.SdtThanhVien)
                ? (PhoneHelper.NormalizeVietnamesePhone(request.SdtThanhVien) ?? request.SdtThanhVien.Trim())
                : sdtNorm;

            var memberCustomer = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.Sdt == sdtMember);
            decimal giamGiaVip = 0m;
            string? ghiChuVip = null;

            var subtotal = tongTien;
            if (memberCustomer != null)
            {
                var invoicesSum = await _context.HoaDons.AsNoTracking()
                    .Where(h => h.MaKH == memberCustomer.MaKH)
                    .SumAsync(h => (decimal?)(h.TongTien - h.TienGiamGia)) ?? 0m;
                int vipPct = invoicesSum switch
                {
                    >= 100_000_000m => 5,
                    >= 50_000_000m => 3,
                    >= 20_000_000m => 1,
                    _ => 0
                };

                if (vipPct > 0)
                {
                    var calculatedDiscount = Math.Round(tongTien * vipPct / 100m, MidpointRounding.AwayFromZero);
                    const decimal NGUONG_DON_OTP = 15_000_000m;
                    const decimal NGUONG_GIAM_OTP = 500_000m;

                    bool needOtp = tongTien >= NGUONG_DON_OTP || calculatedDiscount >= NGUONG_GIAM_OTP;

                    if (needOtp)
                    {
                        if (request.XacNhanCskh)
                        {
                            // Đơn mua hộ / Thành viên không tiện nhận OTP: giữ ưu đãi nhưng đánh dấu cần CSKH xác nhận
                            giamGiaVip = calculatedDiscount;
                            ghiChuVip = $"[Ưu đãi VIP {vipPct}%: -{giamGiaVip:N0}đ (SĐT thành viên: {sdtMember}). Cần CSKH gọi xác nhận thành viên trước khi xuất kho]";
                        }
                        else
                        {
                            // Kiểm tra OTP Token
                            var otpValid = HasMemberOtp(request.OtpToken, sdtMember);
                            if (otpValid)
                            {
                                giamGiaVip = calculatedDiscount;
                                ghiChuVip = $"[Ưu đãi VIP {vipPct}%: -{giamGiaVip:N0}đ (Đã xác thực OTP SĐT {sdtMember})]";
                            }
                            else
                            {
                                // Không có OTP hợp lệ -> Không áp dụng giảm giá VIP
                                giamGiaVip = 0m;
                                ghiChuVip = $"[Từ chối ưu đãi VIP: Đơn hàng vượt ngưỡng nhưng chưa có mã OTP xác thực]";
                            }
                        }
                    }
                    else
                    {
                        // Đơn nhỏ / trung bình: tự áp giảm % ngay không cần OTP
                        giamGiaVip = calculatedDiscount;
                        ghiChuVip = $"[Ưu đãi VIP {vipPct}%: -{giamGiaVip:N0}đ (Áp dụng tự động)]";
                    }

                    tongTien = Math.Max(0m, tongTien - giamGiaVip);
                }
            }

            var code = request.VoucherCode?.Trim().ToUpperInvariant();
            if (request.UsePoints)
            {
                if (_context.Database.IsNpgsql())
                    await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({sdtMember}, 2))");
            }
            if (!string.IsNullOrEmpty(code))
            {
                var voucher = await _context.Vouchers.SingleOrDefaultAsync(v => v.Code == code);
                if (voucher == null || subtotal < voucher.MinOrderTotal || voucher.ExpiresAt <= DateTime.UtcNow ||
                    voucher.UsedCount >= voucher.MaxUses)
                    throw new BadRequestException("Voucher hết hạn, hết lượt hoặc chưa đạt giá trị đơn tối thiểu.");
                if (code == "HELLOSAMA" && _context.Database.IsNpgsql())
                    await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({sdtNorm}, 3))");
                if (code == "HELLOSAMA" &&
                    (await _context.DonHangOnlines.AnyAsync(d => d.SdtNguoiNhan == sdtNorm && d.TrangThai != "da_huy") ||
                     await _context.Orders.AnyAsync(o => o.RecipientPhone == sdtNorm && o.Status != "CANCELLED" && o.Status != "EXPIRED")))
                    throw new BadRequestException("HELLOSAMA chỉ áp dụng cho đơn đầu tiên.");
                if (code == "SAMAMEMBER" && (memberCustomer == null || (!registeredMember && sdtNorm == sdtMember)))
                    throw new BadRequestException("Voucher dành cho thành viên.");
                if (_context.Database.IsNpgsql())
                {
                    var used = await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE vouchers SET used_count=used_count+1, updated_at=now() WHERE id={voucher.Id} AND used_count<max_uses AND expires_at>now()");
                    if (used == 0) throw new ConflictException("Voucher đã hết lượt.");
                }
                else voucher.UsedCount++;
                tongTien = Math.Max(0, tongTien - voucher.DiscountFor(subtotal));
            }
            var points = 0;
            if (request.UsePoints)
            {
                if (memberCustomer == null || !HasMemberOtp(request.OtpToken, sdtMember))
                    throw new BadRequestException("Cần xác thực OTP thành viên để dùng điểm.");
                var unit = await _context.ThamSos.Where(x => x.MaThamSo == "DIEM_MOI_VND").Select(x => (decimal?)x.GiaTri).FirstOrDefaultAsync() ?? 100000m;
                var spent = await _context.HoaDons.Where(h => h.MaKH == memberCustomer.MaKH).SumAsync(h => h.TongTien - h.TienGiamGia);
                var redeemed = await _context.DonHangOnlines.Where(d => d.MemberPhone == sdtMember && d.TrangThai != "da_huy" && d.TrangThai != "boom" && d.TrangThai != "hoan_hang").SumAsync(d => d.PointsUsed);
                var available = Math.Floor(spent / (unit > 0 ? unit : 100000m)) - redeemed;
                points = (int)Math.Min(available, Math.Min(200m, Math.Floor(tongTien / 1000m)));
                if (points <= 0)
                    throw new BadRequestException("Số điểm khả dụng không đủ.");
                tongTien -= points * 1000m;
            }
            decimal shippingFee = 0;
            if (request.ToDistrictId.HasValue && _quotes != null)
            {
                if (request.ToDistrictId <= 0) throw new BadRequestException("Vui lòng chọn quận/huyện giao hàng.");
                var quote = await _quotes.CalculateAsync(new ShippingQuoteRequest {
                    ToDistrictId = request.ToDistrictId.Value, ToWardCode = request.ToWardCode ?? "",
                    SubtotalAfterDiscount = tongTien, Items = groupedItems.Select(i => new ShippingQuoteItem { ProductId=i.MaHang, Quantity=i.SoLuong }).ToList()
                });
                shippingFee = quote.CustomerFee;
                tongTien += shippingFee;
            }
            if (request.ExpectedTotal.HasValue && request.ExpectedTotal != tongTien)
                throw new ConflictException($"Tổng tiền hiện tại là {tongTien:N0} đ. Vui lòng cập nhật giỏ hàng và kiểm tra ưu đãi.");

            if (phuongThuc == "cod" && tongTien > (_settings == null ? 30_000_000m : await _settings.GetCodMaxValueAsync()))
            {
                throw new BadRequestException("Đơn hàng vượt hạn mức thu hộ. Vui lòng chọn chuyển khoản.");
            }

            var maDH = reqMaDH ?? await _codeGenerator.GenerateOrderCode(code => _context.DonHangOnlines.AnyAsync(d => d.MaDH == code));

            // plan (8).md: Quyết định trạng thái ban đầu bằng Engine
            var addressHash = AddressHashHelper.ComputeAddressHash(request.DiaChiGiao);
            var decisionContext = new OrderDecisionContext
            {
                Phone = sdtNorm,
                ClientIp = await GetClientIpAsync(),
                AddressHash = addressHash,
                OrderAmount = tongTien,
                PaymentMethod = phuongThuc,
                IsOnlinePaid = false
            };

            var decision = _decisionEngine != null
                ? await _decisionEngine.DecideInitialStatusAsync(decisionContext)
                : new OrderDecisionResult { InitialStatus = (phuongThuc == "chuyen_khoan" || phuongThuc == "the") ? AppConstants.OrderStatus.ChoThanhToan : AppConstants.OrderStatus.ChoXacNhan };

            if (request.XacNhanCskh)
            {
                // Đơn mua hộ chờ CSKH xác nhận SĐT thành viên -> Luôn đưa về trạng thái chờ xác nhận
                decision.InitialStatus = AppConstants.OrderStatus.ChoXacNhan;
            }

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
                ClientIp = decisionContext.ClientIp, GhiChu = request.GhiChu?.Trim(),
                ShippingFee = shippingFee, VoucherCode = code, PointsUsed = points, MemberPhone = sdtMember,
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
                Reason = !string.IsNullOrWhiteSpace(ghiChuVip)
                    ? ghiChuVip
                    : (decision.RiskFlag
                        ? $"Đơn được tạo kèm cờ rủi ro: {string.Join(", ", decision.RiskReasons)}"
                        : (decision.InitialStatus == "cho_tu_duyet" ? $"Khách tin cậy, hẹn tự duyệt sau 15 phút ({decision.AutoApproveAt:HH:mm})" : "Đơn hàng mới tạo"))
            });
            await _context.SaveChangesAsync();

            // 5. Gán MaDH cho chi tiết và lưu
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
                    catch { /* Không chặn luồng nếu gửi mail gặp sự cố */ }
                });
            }

            
            // Phát tín hiệu Realtime qua SignalR đến AdminGroup
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
                GhiChu = donHang.GhiChu,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TongTien = donHang.TongTien,
                PaymentStatus = donHang.PaymentStatus,
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

        // Nếu client cung cấp SĐT, phải khớp với SdtNguoiNhan. Nếu không khớp trả null để controller ném 404 (chống dò mã đơn P4-06)
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
            GhiChu = order.GhiChu,
            PhuongThucThanhToan = order.PhuongThucThanhToan,
            TongTien = order.TongTien,
            PaymentStatus = order.PaymentStatus,
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
            PaymentStatus = o.PaymentStatus,
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
            .ThenBy(d => d.MaDH)
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
            GhiChu = o.GhiChu,
            PhuongThucThanhToan = o.PhuongThucThanhToan,
            TongTien = o.TongTien,
            PaymentStatus = o.PaymentStatus,
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

    public async Task<OrderDto> UpdateOrderStatusAsync(string maDH, string newTrangThai, string? soHDKem = null, decimal? confirmedAmount = null, string? paymentReference = null)
    {
        var maNV = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNV) || !await _context.NhanViens.AnyAsync(n => n.MaNV == maNV))
        {
            maNV = await _context.NhanViens.Select(n => n.MaNV).FirstOrDefaultAsync() ?? "NV001";
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT 1 FROM donhangonline WHERE madh = {maDH} FOR UPDATE");
            }

            var donHang = await _context.DonHangOnlines
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MatHang)
                .FirstOrDefaultAsync(d => d.MaDH == maDH);

            if (donHang == null)
            {
                throw new NotFoundException($"Không tìm thấy đơn hàng {maDH}");
            }

            var curr = donHang.TrangThai;

            if (paymentReference != null)
            {
                if (curr != AppConstants.OrderStatus.ChoThanhToan ||
                    newTrangThai != AppConstants.OrderStatus.DaXacNhan ||
                    donHang.PhuongThucThanhToan is not ("chuyen_khoan" or "the") ||
                    confirmedAmount != donHang.TongTien || confirmedAmount <= 0)
                {
                    throw new ConflictException("Giao dịch không khớp với đơn đang chờ thanh toán.");
                }

                if (_context.Database.IsNpgsql())
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({paymentReference}, 0))");
                }

                if (await _context.OrderStatusLogs.AnyAsync(log => log.Actor == "payment" && log.Reason == paymentReference))
                {
                    throw new ConflictException("Mã giao dịch đã được ghi nhận.");
                }

                donHang.PaymentStatus = "paid";
                _context.OrderStatusLogs.Add(new OrderStatusLog
                {
                    OrderId = maDH,
                    FromStatus = curr,
                    ToStatus = newTrangThai,
                    Actor = "payment",
                    Reason = paymentReference
                });
            }

            // Kiểm tra máy trạng thái hợp lệ theo mục 4 plan (8).md
            bool validTransition = IsValidTransition(curr, newTrangThai);
            if (!validTransition)
            {
                throw new ConflictException($"Không thể chuyển trạng thái đơn hàng từ '{curr}' sang '{newTrangThai}'.");
            }

            if ((newTrangThai == AppConstants.OrderStatus.DaXacNhan || newTrangThai == AppConstants.OrderStatus.HoanThanh)
                && (donHang.PhuongThucThanhToan is "chuyen_khoan" or "the")
                && donHang.PaymentStatus != "paid")
            {
                var demoAutoConfirm = _http?.HttpContext?.RequestServices.GetService<IConfiguration>()?.GetValue<bool>("Payment:DemoAutoConfirm") == true;
                if (demoAutoConfirm)
                {
                    donHang.PaymentStatus = "paid";
                }
                else
                {
                    throw new ConflictException("Đơn hàng chưa được đối soát thanh toán.");
                }
            }

            if (newTrangThai == AppConstants.OrderStatus.DaXacNhan || newTrangThai == AppConstants.OrderStatus.HoanThanh)
            {
                await SellableStock.LockItemsAsync(_context, donHang.ChiTietDonHangs.Select(item => item.MaHang));
                if (donHang.SoHD == null)
                {
                    foreach (var item in donHang.ChiTietDonHangs)
                    {
                        await SellableStock.EnsureAvailableAsync(_context, item.MatHang!, item.SoLuong, maDH);
                    }
                }
            }

            if (newTrangThai == AppConstants.OrderStatus.DaHuy && donHang.PaymentStatus == "paid")
            {
                throw new ConflictException("Đơn đã thu tiền. Cần xử lý hoàn tiền trước khi hủy.");
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

            // Xử lý khi hoàn thành đơn:
            // Tự động gán IMEI cũ nhất (theo phiếu nhập), tạo hóa đơn online, tạo bảo hành
            if (newTrangThai == AppConstants.OrderStatus.HoanThanh)
            {
                // Kiểm tra xem SoHD có tồn tại hợp lệ trong CSDL không
                if (!string.IsNullOrWhiteSpace(soHDKem) && soHDKem != donHang.SoHD)
                {
                    throw new ConflictException("Hóa đơn phải do hệ thống lập cho đơn hàng này.");
                }
                string candidateSoHD = donHang.SoHD ?? string.Empty;
                bool invoiceExists = !string.IsNullOrEmpty(candidateSoHD) && await _context.HoaDons.AnyAsync(h => h.SoHD == candidateSoHD);
                string soHD = invoiceExists ? candidateSoHD : string.Empty;

                if (string.IsNullOrEmpty(soHD))
                {
                    // Tạo hóa đơn mới hợp lệ trong CSDL
                    soHD = await _codeGenerator.NextAsync("HD");
                    var ptThanhToan = donHang.PhuongThucThanhToan == "cod" ? "tien_mat" : donHang.PhuongThucThanhToan;

                    var gross = donHang.ChiTietDonHangs.Sum(item => item.DonGia * item.SoLuong);
                    var hoaDon = new HoaDon
                    {
                        SoHD = soHD,
                        NgayLap = DateTime.UtcNow,
                        MaKH = donHang.MaKH,
                        MaNV = maNV,
                        KenhBan = AppConstants.SaleChannels.Online,
                        TongTien = gross + donHang.ShippingFee,
                        TienGiamGia = Math.Max(0m, gross + donHang.ShippingFee - donHang.TongTien),
                        PhuongThucThanhToan = ptThanhToan
                    };
                    _context.HoaDons.Add(hoaDon);
                    await _context.SaveChangesAsync();
                    donHang.SoHD = soHD;
                    await _context.SaveChangesAsync();

                    // Xử lý từng dòng chi tiết đơn
                    foreach (var item in donHang.ChiTietDonHangs)
                    {
                        var matHang = item.MatHang ?? await _context.MatHangs.FirstOrDefaultAsync(m => m.MaHang == item.MaHang);
                        if (matHang != null && matHang.QuanLyImei)
                        {
                            for (int i = 0; i < item.SoLuong; i++)
                            {
                                // Chọn IMEI con_hang theo phiếu nhập cũ trước (FIFO dựa vào SoPN hoặc MaIMEI)
                                var imei = await (from im in _context.IMEIs
                                                  join pn in _context.PhieuNhaps on im.SoPN equals pn.SoPN into pnGroup
                                                  from pn in pnGroup.DefaultIfEmpty()
                                                  where im.MaHang == item.MaHang && im.TrangThai == "con_hang"
                                                  orderby pn != null ? pn.NgayNhap : DateTime.MinValue, im.MaIMEI
                                                  select im).FirstOrDefaultAsync();

                                if (imei == null)
                                {
                                    throw new ConflictException($"Không đủ IMEI thực tế trong kho cho mặt hàng {item.MaHang}.");
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
                                imei.TrangThai = "da_ban";
                                await _context.SaveChangesAsync();

                                // Tạo bảo hành
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
                    AppConstants.OrderStatus.DaXacNhan => $"Đơn đặt hàng #{donHang.MaDH} đã được Admin xác nhận thành công! Đang chuẩn bị đóng gói.",
                    AppConstants.OrderStatus.DangGiao => $"Đơn đặt hàng #{donHang.MaDH} đã được xuất kho và đang trên đường giao đến bạn!",
                    AppConstants.OrderStatus.HoanThanh => $"Đơn đặt hàng #{donHang.MaDH} đã được giao thành công và hoàn tất! Cảm ơn bạn đã mua hàng.",
                    AppConstants.OrderStatus.DaHuy => $"Đơn đặt hàng #{donHang.MaDH} đã được cập nhật trạng thái: Đã hủy.",
                    _ => $"Đơn đặt hàng #{donHang.MaDH} đã được cập nhật trạng thái mới."
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
                GhiChu = donHang.GhiChu,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TongTien = donHang.TongTien,
                PaymentStatus = donHang.PaymentStatus,
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
            throw new BadRequestException("Mã đơn hàng không hợp lệ.");
        }

        maDH = maDH.Trim();
        using var transaction = await _context.Database.BeginTransactionAsync();
        if (_context.Database.IsNpgsql())
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"SELECT 1 FROM donhangonline WHERE madh = {maDH} FOR UPDATE");
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH.Trim());
        if (donHang == null)
        {
            throw new NotFoundException($"Không tìm thấy đơn hàng {maDH}");
        }

        var phone = PhoneHelper.NormalizeVietnamesePhone(request.Sdt);
        if (string.IsNullOrWhiteSpace(phone) || phone != donHang.SdtNguoiNhan)
        {
            throw new BadRequestException("Số điện thoại không khớp với thông tin đơn hàng.");
        }

        if (donHang.PaymentStatus == "paid" || donHang.TrangThai is not
            ("cho_thanh_toan" or "cho_xac_nhan" or "cho_tu_duyet" or "can_kiem_tra" or "da_huy"))
        {
            throw new ConflictException("Đơn đã thu tiền hoặc đang được xử lý. Vui lòng liên hệ cửa hàng để hủy.");
        }

        donHang.TrangThai = AppConstants.OrderStatus.DaHuy;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        if (_hubContext != null)
        {
            string cancelMsg = $"Khách hàng {donHang.TenNguoiNhan} ({donHang.SdtNguoiNhan}) đã hủy đơn hàng #{donHang.MaDH}" +
                               (!string.IsNullOrWhiteSpace(request.LyDo) ? $" (Lý do: {request.LyDo})" : "");
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

        // Gửi email thông báo hủy nếu có email khách
        if (_emailService != null)
        {
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == donHang.MaKH);
            if (kh != null && !string.IsNullOrWhiteSpace(kh.Email) && !kh.Email.EndsWith("@cellphones.customer"))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        string body = $"<p>Xin chào <strong>{donHang.TenNguoiNhan}</strong>,</p>" +
                                      $"<p>Đơn hàng <strong>#{donHang.MaDH}</strong> đã được hủy thành công theo yêu cầu của bạn.</p>" +
                                      (!string.IsNullOrEmpty(request.LyDo) ? $"<p>Lý do hủy: {request.LyDo}</p>" : "") +
                                      $"<p>Cảm ơn bạn đã đồng hành cùng Sama Mobile!</p>";
                        await _emailService.SendEmailAsync(kh.Email, $"[Sama Mobile] Đã hủy đơn hàng #{donHang.MaDH}", body);
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
            GhiChu = donHang.GhiChu,
            PhuongThucThanhToan = donHang.PhuongThucThanhToan,
            TongTien = donHang.TongTien,
            PaymentStatus = donHang.PaymentStatus,
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
            from != AppConstants.OrderStatus.HoanThanh && from != "da_giao" &&
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
            ("da_giao", "hoan_thanh") => true,
            ("dang_giao", "boom") => true,
            ("dang_giao", "hoan_hang") => true,
            ("boom", "hoan_hang") => true,

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
