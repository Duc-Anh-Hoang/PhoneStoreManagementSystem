using Microsoft.AspNetCore.SignalR;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
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

    public OnlineOrderService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        IWarrantyService warrantyService,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _warrantyService = warrantyService;
        _emailService = emailService;
        _hubContext = hubContext;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Chuẩn hóa SĐT người nhận
            var sdtNorm = request.SdtNguoiNhan.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

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
                        Dvt = "Cái",
                                                MaLoai = (_context.LoaiHangs.Select(l => l.MaLoai).FirstOrDefault() ?? "APPLE"),
                        MaNhom = (_context.NhomHangs.Select(n => n.MaNhom).FirstOrDefault() ?? "DIENTHOAI"),
                        QuanLyImei = true,
                        ThoiHanBaoHanh = 12
                    };
                    _context.MatHangs.Add(matHang);
                    await _context.SaveChangesAsync();
                }

                decimal donGiaDb = item.DonGia > 0 ? item.DonGia : (matHang.DonGia > 0 ? matHang.DonGia : 5000000m);
                tongTien += donGiaDb * item.SoLuong;

                chiTietList.Add(new ChiTietDonHangOnline
                {
                    MaHang = matHang.MaHang,
                    SoLuong = item.SoLuong,
                    DonGia = donGiaDb
                });
            }

            // 4. Tạo DonHangOnline
            var reqMaDH = !string.IsNullOrWhiteSpace(request.MaDH) ? request.MaDH.Trim().Substring(0, Math.Min(15, request.MaDH.Trim().Length)) : null;
            var maDH = (!string.IsNullOrWhiteSpace(reqMaDH) && !await _context.DonHangOnlines.AnyAsync(d => d.MaDH == reqMaDH)) ? reqMaDH : await _codeGenerator.NextAsync("DH");
            var donHang = new DonHangOnline
            {
                MaDH = maDH,
                NgayDat = DateTime.UtcNow,
                MaKH = khachHang.MaKH,
                TenNguoiNhan = request.TenNguoiNhan.Trim(),
                SdtNguoiNhan = sdtNorm,
                DiaChiGiao = request.DiaChiGiao.Trim(),
                PhuongThucThanhToan = (request.PhuongThucThanhToan ?? "cod").Trim().ToLowerInvariant() switch { "banking" or "momo" or "vnpay" or "zalopay" or "chuyen_khoan" => "chuyen_khoan", "card" or "the" => "the", _ => "cod" },
                TongTien = tongTien,
                TrangThai = AppConstants.OrderStatus.ChoXacNhan
            };

            _context.DonHangOnlines.Add(donHang);
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
                    message = $"Khách hàng {donHang.TenNguoiNhan} ({donHang.SdtNguoiNhan}) vừa đặt đơn hàng mới #{donHang.MaDH} trị giá {donHang.TongTien:N0}đ",
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
                throw new NotFoundException($"Không tìm thấy đơn hàng {maDH}");
            }

            var curr = donHang.TrangThai;

            // Kiểm tra máy trạng thái hợp lệ theo P4-07
            // Chỉ cho: cho_xac_nhan → da_xac_nhan, da_xac_nhan → dang_giao, dang_giao → hoan_thanh, cho_xac_nhan hoặc da_xac_nhan → da_huy. Còn lại 409
            bool validTransition = false;
            if (curr == AppConstants.OrderStatus.ChoXacNhan && newTrangThai == AppConstants.OrderStatus.DaXacNhan) validTransition = true;
            else if (curr == AppConstants.OrderStatus.DaXacNhan && newTrangThai == AppConstants.OrderStatus.DangGiao) validTransition = true;
            else if (curr == AppConstants.OrderStatus.DangGiao && newTrangThai == AppConstants.OrderStatus.HoanThanh) validTransition = true;
            else if ((curr == AppConstants.OrderStatus.ChoXacNhan || curr == AppConstants.OrderStatus.DaXacNhan) && newTrangThai == AppConstants.OrderStatus.DaHuy) validTransition = true;

            if (!validTransition)
            {
                throw new ConflictException($"Không thể chuyển trạng thái đơn hàng từ '{curr}' sang '{newTrangThai}'.");
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
                string soHD = donHang.SoHD ?? soHDKem ?? string.Empty;

                if (string.IsNullOrEmpty(soHD))
                {
                    // Tạo hóa đơn mới kênh online
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
                                    // Tự động sinh phiếu nhập và IMEI nếu sản phẩm chưa có sẵn trong kho
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

                                // Đối với PostgreSQL, trigger fn_trg_chitiethoadon_imei_ban tự động kiểm tra 'con_hang' và cập nhật 'da_ban'
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
            }

            donHang.TrangThai = newTrangThai;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

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
            throw new BadRequestException("Mã đơn hàng không hợp lệ.");
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH.Trim());
        if (donHang == null)
        {
            throw new NotFoundException($"Không tìm thấy đơn hàng {maDH}");
        }

        // Kiểm tra số điện thoại người nhận để xác thực
        var sdtNorm = (request.Sdt ?? string.Empty).Trim().Replace(" ", "").Replace(".", "").Replace("-", "");
        if (!string.IsNullOrEmpty(sdtNorm) && !donHang.SdtNguoiNhan.Equals(sdtNorm, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Số điện thoại không khớp với thông tin đơn hàng.");
        }

        // Chỉ cho phép hủy khi đơn chưa giao
        if (donHang.TrangThai == AppConstants.OrderStatus.DangGiao || donHang.TrangThai == AppConstants.OrderStatus.HoanThanh)
        {
            throw new BadRequestException("Đơn hàng đang giao hoặc đã hoàn thành, không thể tự hủy trực tuyến. Vui lòng liên hệ hotline.");
        }

        if (donHang.TrangThai == AppConstants.OrderStatus.DaHuy)
        {
            throw new BadRequestException("Đơn hàng này đã được hủy trước đó.");
        }

        donHang.TrangThai = AppConstants.OrderStatus.DaHuy;

        await _context.SaveChangesAsync();

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
            PhuongThucThanhToan = donHang.PhuongThucThanhToan,
            TongTien = donHang.TongTien,
            TrangThai = donHang.TrangThai,
            MaNV = donHang.MaNV,
            SoHD = donHang.SoHD
        };
    }

}
