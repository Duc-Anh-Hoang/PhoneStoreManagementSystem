using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Sales.Returns;

public class ReturnsService : IReturnsService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ISystemSettingService _systemSettingService;

    public ReturnsService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        ISystemSettingService systemSettingService)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _systemSettingService = systemSettingService;
    }

    private string GetCurrentUserId()
    {
        var maNV = _currentUser.MaNV;
        if (!string.IsNullOrWhiteSpace(maNV)) return maNV;
        return _context.NhanViens.Select(n => n.MaNV).FirstOrDefault() ?? "NV001";
    }

    public async Task<TraHangDto> CreateTraHangAsync(CreateTraHangRequest request)
    {
        var maNV = GetCurrentUserId();

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Xác định IMEI
            IMEI? imeiRecord = null;
            if (!string.IsNullOrWhiteSpace(request.MaIMEI))
            {
                imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == request.MaIMEI);
            }
            else if (!string.IsNullOrWhiteSpace(request.SoIMEI))
            {
                imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.SoIMEI == request.SoIMEI);
            }

            if (imeiRecord == null)
            {
                throw new NotFoundException("Không tìm thấy thông tin IMEI yêu cầu trả.");
            }

            // 2. Kiểm tra trạng thái IMEI (phải là da_ban)
            if (imeiRecord.TrangThai != "da_ban")
            {
                if (imeiRecord.TrangThai == "con_hang")
                {
                    throw new ConflictException($"Máy mang IMEI {imeiRecord.SoIMEI} chưa được bán (đang còn trong kho), không thể tiếp nhận đổi trả.");
                }
                throw new ConflictException($"IMEI {imeiRecord.SoIMEI} đang ở trạng thái '{imeiRecord.TrangThai}', không thể tiếp nhận đổi trả.");
            }

            // 3. Tìm hoặc xác minh số hóa đơn
            string soHD = (request.SoHD ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(soHD))
            {
                var ctGoc = await _context.ChiTietHoaDons
                    .OrderByDescending(c => c.MaCTHD)
                    .FirstOrDefaultAsync(ct => ct.MaIMEI == imeiRecord.MaIMEI);
                if (ctGoc == null)
                {
                    throw new BadRequestException($"IMEI {imeiRecord.SoIMEI} chưa từng được xuất bán trên hệ thống.");
                }
                soHD = ctGoc.SoHD;
            }

            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.SoHD == soHD);
            if (hoaDon == null)
            {
                throw new NotFoundException($"Không tìm thấy hóa đơn {soHD}");
            }

            var thuocHoaDon = await _context.ChiTietHoaDons
                .AnyAsync(ct => ct.SoHD == soHD && ct.MaIMEI == imeiRecord.MaIMEI);
            if (!thuocHoaDon)
            {
                throw new BadRequestException($"IMEI {imeiRecord.SoIMEI} không thuộc hóa đơn {soHD}.");
            }

            // 4. Kiểm tra hạn đổi trả
            string loaiYeuCau = string.Equals(request.LoaiYeuCau, "doi_y", StringComparison.OrdinalIgnoreCase) ? "doi_y" : "loi_ky_thuat";
            decimal hanNgay = 30m;
            if (loaiYeuCau == "doi_y")
            {
                hanNgay = await _systemSettingService.GetAsync("HAN_DOI_Y_NGAY");
                if (hanNgay <= 0) hanNgay = 7m;
            }
            else
            {
                hanNgay = await _systemSettingService.GetAsync("HAN_DOITRA_NGAY");
                if (hanNgay <= 0) hanNgay = 30m;
            }

            var now = DateTime.UtcNow;
            var daysPassed = (now - hoaDon.NgayLap).TotalDays;
            if (daysPassed > (double)hanNgay)
            {
                throw new BadRequestException($"Đã quá hạn đổi trả ({hanNgay} ngày kể từ ngày lập hóa đơn {soHD}).");
            }

            // 5. Kiểm tra hồ sơ đang mở hoặc đã hoàn tất
            var phieuTrung = await _context.TraHangs
                .AnyAsync(t => t.MaIMEI == imeiRecord.MaIMEI && t.TrangThai != "tu_choi" && t.TrangThai != "da_huy");
            if (phieuTrung)
            {
                throw new ConflictException($"IMEI {imeiRecord.SoIMEI} đã có hồ sơ đổi trả đang mở hoặc đã hoàn tất.");
            }

            // 6. Kiểm tra máy có đang trong phiếu tiếp nhận bảo hành chưa trả không
            var dangBaoHanh = await (from tn in _context.PhieuTiepNhanBaoHanhs
                                     join bh in _context.BaoHanhs on tn.MaBH equals bh.MaBH
                                     where bh.MaIMEI == imeiRecord.MaIMEI &&
                                           (tn.TrangThai == "tiep_nhan" || tn.TrangThai == "dang_sua" || tn.TrangThai == "cho_linh_kien")
                                     select tn.MaTN).AnyAsync();
            if (dangBaoHanh)
            {
                throw new ConflictException($"IMEI {imeiRecord.SoIMEI} hiện đang có phiếu bảo hành chưa trả máy, không thể tiếp nhận đổi trả.");
            }

            // 7. Cập nhật IMEI sang cho_duyet
            imeiRecord.TrangThai = "cho_duyet";
            _context.IMEIs.Update(imeiRecord);

            // Tính toán trước số tiền hoàn thực tế từ hóa đơn gốc
            decimal? soTienHoanBanDau = null;
            if (request.HinhThucHoanTien != "doi_hang")
            {
                var ctGoc = await _context.ChiTietHoaDons.FirstOrDefaultAsync(c => c.SoHD == soHD && c.MaIMEI == imeiRecord.MaIMEI);
                if (ctGoc != null)
                {
                    decimal donGia = ctGoc.DonGiaBan * ctGoc.SlBan;
                    if (hoaDon.TongTien > 0 && hoaDon.TienGiamGia > 0)
                    {
                        decimal tileGiam = hoaDon.TienGiamGia / hoaDon.TongTien;
                        soTienHoanBanDau = Math.Round(donGia * (1m - tileGiam), 0);
                    }
                    else
                    {
                        soTienHoanBanDau = donGia;
                    }
                }
            }

            // 8. Tạo phiếu TraHang trạng thái tiep_nhan
            var maTR = await _codeGenerator.NextAsync("TR");
            var traHang = new TraHang
            {
                MaTR = maTR,
                SoHD = soHD,
                MaIMEI = imeiRecord.MaIMEI,
                LoaiYeuCau = loaiYeuCau,
                HinhThucHoanTien = request.HinhThucHoanTien,
                SoTienHoan = soTienHoanBanDau,
                LyDo = request.LyDo.Trim(),
                TrangThai = "tiep_nhan",
                NgayTR = now,
                MaNV = maNV,
                ChecklistNgoaiQuan = request.ChecklistNgoaiQuan
            };

            _context.TraHangs.Add(traHang);

            // 9. Ghi nhật ký TraHangLichSu
            var lichSu = new TraHangLichSu
            {
                MaTR = maTR,
                TrangThaiCu = null,
                TrangThaiMoi = "tiep_nhan",
                MaNV = maNV,
                ThoiDiem = now,
                GhiChu = $"Tiếp nhận hồ sơ đổi trả: {request.LyDo.Trim()}"
            };
            _context.Set<TraHangLichSu>().Add(lichSu);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(maTR);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<TraHangDto> KiemTraAsync(string maTR, KiemTraTraHangRequest request)
    {
        var maNV = GetCurrentUserId();
        var traHang = await _context.TraHangs
            .Include(t => t.IMEI)
            .FirstOrDefaultAsync(t => t.MaTR == maTR);

        if (traHang == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        if (traHang.TrangThai != "tiep_nhan")
        {
            throw new ConflictException($"Phiếu {maTR} đang ở trạng thái '{traHang.TrangThai}', chỉ có thể kiểm tra kỹ thuật khi ở trạng thái 'tiep_nhan'.");
        }

        // Ràng buộc vai trò: Kỹ thuật viên không được là người tiếp nhận (ngoại trừ admin)
        // check admin
        bool isAdmin = _currentUser.IsInRole(AppConstants.Roles.Admin) || string.Equals(_currentUser.Role, AppConstants.Roles.Admin, StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && string.Equals(traHang.MaNV, maNV, StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenException("Kỹ thuật viên kiểm tra máy không được trùng với nhân viên tiếp nhận hồ sơ.");
        }

        var now = DateTime.UtcNow;
        string trangThaiCu = traHang.TrangThai;
        traHang.TrangThai = "da_kiem_tra";
        traHang.KetQuaKiemTra = request.KetQuaKiemTra;
        traHang.GhiChuKiemTra = request.GhiChuKiemTra.Trim();
        traHang.MaNVKiemTra = maNV;
        traHang.NgayKiemTra = now;

        _context.Set<TraHangLichSu>().Add(new TraHangLichSu
        {
            MaTR = maTR,
            TrangThaiCu = trangThaiCu,
            TrangThaiMoi = "da_kiem_tra",
            MaNV = maNV,
            ThoiDiem = now,
            GhiChu = $"Kỹ thuật kiểm tra: {request.KetQuaKiemTra}. {request.GhiChuKiemTra.Trim()}"
        });

        await _context.SaveChangesAsync();
        return await GetByIdAsync(maTR);
    }

    public async Task<TraHangDto> DuyetAsync(string maTR, DuyetTraHangRequest request)
    {
        var maNV = GetCurrentUserId();
        var traHang = await _context.TraHangs
            .Include(t => t.IMEI)
            .FirstOrDefaultAsync(t => t.MaTR == maTR);

        if (traHang == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        if (traHang.TrangThai != "da_kiem_tra")
        {
            throw new ConflictException($"Phiếu {maTR} đang ở trạng thái '{traHang.TrangThai}', chỉ có thể duyệt khi ở trạng thái 'da_kiem_tra'.");
        }

        // check admin
        bool isAdmin = _currentUser.IsInRole(AppConstants.Roles.Admin) || string.Equals(_currentUser.Role, AppConstants.Roles.Admin, StringComparison.OrdinalIgnoreCase);
        if (!isAdmin)
        {
            if (string.Equals(traHang.MaNV, maNV, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(traHang.MaNVKiemTra, maNV, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenException("Người duyệt phải khác nhân viên tiếp nhận và kỹ thuật viên kiểm tra.");
            }
        }

        // Bảng quy tắc duyệt:
        if (request.ChapNhan)
        {
            if (traHang.KetQuaKiemTra == "hu_hong_do_nguoi_dung" && !isAdmin)
            {
                throw new BadRequestException("Máy bị hư hỏng do người dùng không đủ điều kiện duyệt đổi trả (chỉ Quản trị viên cấp cao mới có quyền ngoại lệ).");
            }
            if (traHang.KetQuaKiemTra == "khong_loi" && traHang.LoaiYeuCau != "doi_y" && !isAdmin)
            {
                throw new BadRequestException("Máy không có lỗi và không phải lý do đổi ý thì không đủ điều kiện duyệt.");
            }
        }

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                string trangThaiCu = traHang.TrangThai;

                if (request.ChapNhan)
            {
                traHang.TrangThai = "da_duyet";
                traHang.MaNVDuyet = maNV;
                traHang.NgayDuyet = now;
                traHang.LyDoTuChoi = null;

                _context.Set<TraHangLichSu>().Add(new TraHangLichSu
                {
                    MaTR = maTR,
                    TrangThaiCu = trangThaiCu,
                    TrangThaiMoi = "da_duyet",
                    MaNV = maNV,
                    ThoiDiem = now,
                    GhiChu = "Quản lý đã duyệt hồ sơ đổi trả."
                });
            }
            else
            {
                traHang.TrangThai = "tu_choi";
                traHang.MaNVDuyet = maNV;
                traHang.NgayDuyet = now;
                traHang.LyDoTuChoi = request.LyDoTuChoi?.Trim();

                // Trả máy về tay khách: cho_duyet -> da_ban
                if (traHang.IMEI != null)
                {
                    traHang.IMEI.TrangThai = "da_ban";
                    _context.IMEIs.Update(traHang.IMEI);
                }

                _context.Set<TraHangLichSu>().Add(new TraHangLichSu
                {
                    MaTR = maTR,
                    TrangThaiCu = trangThaiCu,
                    TrangThaiMoi = "tu_choi",
                    MaNV = maNV,
                    ThoiDiem = now,
                    GhiChu = $"Từ chối hồ sơ: {request.LyDoTuChoi?.Trim()}"
                });
            }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(maTR);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<TraHangDto> HoanTatAsync(string maTR, HoanTatTraHangRequest request)
    {
        var maNV = GetCurrentUserId();
        var traHang = await _context.TraHangs
            .Include(t => t.IMEI)
            .FirstOrDefaultAsync(t => t.MaTR == maTR);

        if (traHang == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        if (traHang.TrangThai != "da_duyet")
        {
            throw new ConflictException($"Phiếu {maTR} đang ở trạng thái '{traHang.TrangThai}', chỉ có thể hoàn tất khi đã ở trạng thái 'da_duyet'.");
        }

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                string trangThaiCu = traHang.TrangThai;

                // Tính tiền hoàn thực tế khách đã trả trên hóa đơn
            var chiTietGoc = await _context.ChiTietHoaDons
                .FirstOrDefaultAsync(ct => ct.SoHD == traHang.SoHD && ct.MaIMEI == traHang.MaIMEI);
            var hoaDonGoc = await _context.HoaDons.FirstOrDefaultAsync(h => h.SoHD == traHang.SoHD);

            decimal soTienHoan = 0m;
            if (chiTietGoc != null)
            {
                decimal donGia = chiTietGoc.DonGiaBan * chiTietGoc.SlBan;
                if (hoaDonGoc != null && hoaDonGoc.TongTien > 0 && hoaDonGoc.TienGiamGia > 0)
                {
                    decimal tileGiam = hoaDonGoc.TienGiamGia / hoaDonGoc.TongTien;
                    soTienHoan = Math.Round(donGia * (1m - tileGiam), 0);
                }
                else
                {
                    soTienHoan = donGia;
                }
            }

            if (traHang.HinhThucHoanTien == "doi_hang")
            {
                IMEI? imeiMoi = null;
                // Nếu nhân viên chọn cụ thể IMEI máy mới tương đương từ form:
                if (!string.IsNullOrWhiteSpace(request.SoIMEIMoi) || !string.IsNullOrWhiteSpace(request.MaIMEIMoi))
                {
                    imeiMoi = await _context.IMEIs.FirstOrDefaultAsync(im =>
                        (im.SoIMEI == request.SoIMEIMoi || im.MaIMEI == request.MaIMEIMoi) &&
                        im.TrangThai == "con_hang");

                    if (imeiMoi == null)
                    {
                        throw new BadRequestException("Số IMEI máy mới được chọn không tồn tại hoặc không còn hàng trong kho.");
                    }
                }
                else
                {
                    // Tự động chọn theo FIFO cùng mặt hàng
                    imeiMoi = await (from im in _context.IMEIs
                                     join pn in _context.PhieuNhaps on im.SoPN equals pn.SoPN
                                     where im.MaHang == traHang.IMEI!.MaHang && im.TrangThai == "con_hang"
                                     orderby pn.NgayNhap ascending, im.MaIMEI ascending
                                     select im).FirstOrDefaultAsync();
                }

                if (imeiMoi == null)
                {
                    throw new ConflictException($"Không còn máy cùng loại '{traHang.IMEI?.MaHang}' trong kho để thực hiện đổi máy. Vui lòng chuyển sang hình thức hoàn tiền.");
                }

                // Chốt đổi máy mới
                imeiMoi.TrangThai = "da_ban";
                _context.IMEIs.Update(imeiMoi);
                traHang.MaIMEIMoi = imeiMoi.MaIMEI;

                // Tạo bảo hành cho máy mới
                var matHang = await _context.MatHangs.FindAsync(imeiMoi.MaHang);
                int thoiHan = matHang?.ThoiHanBaoHanh ?? 12;
                var maBH = await _codeGenerator.NextAsync("BH");
                _context.BaoHanhs.Add(new BaoHanh
                {
                    MaBH = maBH,
                    SoHD = traHang.SoHD,
                    MaIMEI = imeiMoi.MaIMEI,
                    NgayBD = now,
                    ThoiHanThang = thoiHan,
                    NgayKT = now.AddMonths(thoiHan),
                    DieuKien = "Bảo hành 1 đổi 1 chính hãng theo hóa đơn gốc " + traHang.SoHD
                });
            }
            else
            {
                traHang.SoTienHoan = soTienHoan;
                traHang.MaGiaoDich = request.MaGiaoDich?.Trim();
            }

            // Kết thúc bảo hành máy cũ
            var bhCu = await _context.BaoHanhs.FirstOrDefaultAsync(b => b.MaIMEI == traHang.MaIMEI);
            if (bhCu != null)
            {
                bhCu.NgayKT = now;
                _context.BaoHanhs.Update(bhCu);
            }

            // Chuyển trạng thái máy cũ: dang_sua nếu lỗi nsx, ngược lại tra_hang
            if (traHang.IMEI != null)
            {
                traHang.IMEI.TrangThai = (traHang.KetQuaKiemTra == "loi_nsx") ? "dang_sua" : "tra_hang";
                _context.IMEIs.Update(traHang.IMEI);
            }

            traHang.TrangThai = "hoan_tat";
            traHang.MaNVHoanTat = maNV;
            traHang.NgayHoanTat = now;

            _context.Set<TraHangLichSu>().Add(new TraHangLichSu
            {
                MaTR = maTR,
                TrangThaiCu = trangThaiCu,
                TrangThaiMoi = "hoan_tat",
                MaNV = maNV,
                ThoiDiem = now,
                GhiChu = traHang.HinhThucHoanTien == "doi_hang"
                    ? $"Hoàn tất đổi máy mới IMEI {traHang.MaIMEIMoi}."
                    : $"Hoàn tất hoàn tiền ({traHang.HinhThucHoanTien}): {soTienHoan:N0} VND."
            });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(maTR);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<TraHangDto> HuyAsync(string maTR, HuyTraHangRequest request)
    {
        var maNV = GetCurrentUserId();
        var traHang = await _context.TraHangs
            .Include(t => t.IMEI)
            .FirstOrDefaultAsync(t => t.MaTR == maTR);

        if (traHang == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        if (traHang.TrangThai != "tiep_nhan" && traHang.TrangThai != "da_kiem_tra")
        {
            throw new ConflictException($"Không thể hủy hồ sơ đã duyệt hoặc hoàn tất (trạng thái hiện tại: '{traHang.TrangThai}').");
        }

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                string trangThaiCu = traHang.TrangThai;

                traHang.TrangThai = "da_huy";
            traHang.LyDoTuChoi = $"Hủy hồ sơ: {request.LyDo.Trim()}";

            // Trả máy về tay khách
            if (traHang.IMEI != null)
            {
                traHang.IMEI.TrangThai = "da_ban";
                _context.IMEIs.Update(traHang.IMEI);
            }

            _context.Set<TraHangLichSu>().Add(new TraHangLichSu
            {
                MaTR = maTR,
                TrangThaiCu = trangThaiCu,
                TrangThaiMoi = "da_huy",
                MaNV = maNV,
                ThoiDiem = now,
                GhiChu = $"Hủy hồ sơ: {request.LyDo.Trim()}"
            });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(maTR);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<TraHangDto> DoiHinhThucAsync(string maTR, DoiHinhThucRequest request)
    {
        var maNV = GetCurrentUserId();
        var traHang = await _context.TraHangs.FirstOrDefaultAsync(t => t.MaTR == maTR);
        if (traHang == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        if (traHang.TrangThai != "da_duyet")
        {
            throw new ConflictException("Chỉ có thể đổi hình thức xử lý khi hồ sơ đang ở trạng thái 'da_duyet'.");
        }

        string hinhThucCu = traHang.HinhThucHoanTien;
        traHang.HinhThucHoanTien = request.HinhThucMoi;

        _context.Set<TraHangLichSu>().Add(new TraHangLichSu
        {
            MaTR = maTR,
            TrangThaiCu = traHang.TrangThai,
            TrangThaiMoi = traHang.TrangThai,
            MaNV = maNV,
            ThoiDiem = DateTime.UtcNow,
            GhiChu = $"Quản lý đổi hình thức từ '{hinhThucCu}' sang '{request.HinhThucMoi}'. Lý do: {request.LyDo?.Trim() ?? "Không có"}"
        });

        await _context.SaveChangesAsync();
        return await GetByIdAsync(maTR);
    }

    public async Task<TraCuuImeiTraHangDto> TraCuuImeiAsync(string soIMEI)
    {
        if (string.IsNullOrWhiteSpace(soIMEI))
        {
            throw new BadRequestException("Số IMEI không được để trống.");
        }

        var imei = await _context.IMEIs
            .Include(i => i.MatHang)
            .FirstOrDefaultAsync(i => i.SoIMEI == soIMEI || i.MaIMEI == soIMEI);

        if (imei == null)
        {
            throw new NotFoundException($"Không tìm thấy IMEI '{soIMEI}' trên hệ thống.");
        }

        var ct = await _context.ChiTietHoaDons
            .Include(c => c.HoaDon)
            .OrderByDescending(c => c.MaCTHD)
            .FirstOrDefaultAsync(c => c.MaIMEI == imei.MaIMEI);

        if (ct == null || ct.HoaDon == null)
        {
            return new TraCuuImeiTraHangDto
            {
                SoIMEI = imei.SoIMEI,
                MaHang = imei.MaHang,
                TenHang = imei.MatHang?.TenHang ?? "-",
                CoTheTiepNhan = false,
                LyDoKhongHopLe = "IMEI chưa từng được xuất bán trên hóa đơn."
            };
        }

        var hd = ct.HoaDon;
        decimal donGia = ct.DonGiaBan * ct.SlBan;
        decimal soTienThucTra = donGia;
        if (hd.TongTien > 0 && hd.TienGiamGia > 0)
        {
            decimal tileGiam = hd.TienGiamGia / hd.TongTien;
            soTienThucTra = Math.Round(donGia * (1m - tileGiam), 0);
        }

        decimal hanDoiTra = await _systemSettingService.GetAsync("HAN_DOITRA_NGAY");
        if (hanDoiTra <= 0) hanDoiTra = 30m;

        decimal hanDoiY = await _systemSettingService.GetAsync("HAN_DOI_Y_NGAY");
        if (hanDoiY <= 0) hanDoiY = 7m;

        var now = DateTime.UtcNow;
        var ngayHetHanDoiTra = hd.NgayLap.AddDays((double)hanDoiTra);
        var ngayHetHanDoiY = hd.NgayLap.AddDays((double)hanDoiY);

        bool conHanDoiTra = now <= ngayHetHanDoiTra;
        bool conHanDoiY = now <= ngayHetHanDoiY;

        bool coPhieuMo = await _context.TraHangs
            .AnyAsync(t => t.MaIMEI == imei.MaIMEI && t.TrangThai != "tu_choi" && t.TrangThai != "da_huy");

        bool dangSuaChua = await (from tn in _context.PhieuTiepNhanBaoHanhs
                                  join bh in _context.BaoHanhs on tn.MaBH equals bh.MaBH
                                  where bh.MaIMEI == imei.MaIMEI &&
                                        (tn.TrangThai == "tiep_nhan" || tn.TrangThai == "dang_sua" || tn.TrangThai == "cho_linh_kien")
                                  select tn.MaTN).AnyAsync();

        string? lyDoKhongHopLe = null;
        if (imei.TrangThai != "da_ban")
        {
            if (imei.TrangThai == "con_hang")
            {
                lyDoKhongHopLe = "Máy này chưa được bán (đang còn trong kho), không thể tiếp nhận đổi trả.";
            }
            else
            {
                lyDoKhongHopLe = $"IMEI không ở trạng thái 'da_ban' (hiện tại: {imei.TrangThai}).";
            }
        }
        else if (!conHanDoiTra)
        {
            lyDoKhongHopLe = $"Đã quá hạn đổi trả ({hanDoiTra} ngày).";
        }
        else if (coPhieuMo)
        {
            lyDoKhongHopLe = "IMEI đã có phiếu đổi trả đang mở hoặc đã hoàn tất.";
        }
        else if (dangSuaChua)
        {
            lyDoKhongHopLe = "IMEI đang có phiếu bảo hành chưa trả máy.";
        }

        return new TraCuuImeiTraHangDto
        {
            SoIMEI = imei.SoIMEI,
            MaHang = imei.MaHang,
            TenHang = imei.MatHang?.TenHang ?? "-",
            SoHD = hd.SoHD,
            NgayMua = hd.NgayLap,
            DonGiaBan = donGia,
            SoTienThucTra = soTienThucTra,
            HanDoiTraNgay = hanDoiTra,
            HanDoiYNgay = hanDoiY,
            NgayHetHanDoiTra = ngayHetHanDoiTra,
            NgayHetHanDoiY = ngayHetHanDoiY,
            ConHanDoiTra = conHanDoiTra,
            ConHanDoiY = conHanDoiY,
            CoTheTiepNhan = string.IsNullOrEmpty(lyDoKhongHopLe),
            LyDoKhongHopLe = lyDoKhongHopLe
        };
    }

    public async Task<TraHangDto> GetByIdAsync(string maTR)
    {
        var t = await _context.TraHangs
            .Include(x => x.IMEI)
                .ThenInclude(i => i!.MatHang)
            .Include(x => x.IMEIMoi)
            .Include(x => x.NhanVienTiepNhan)
            .Include(x => x.NhanVienKiemTra)
            .Include(x => x.NhanVienDuyet)
            .Include(x => x.NhanVienHoanTat)
            .Include(x => x.LichSu)
                .ThenInclude(l => l.NhanVien)
            .FirstOrDefaultAsync(x => x.MaTR == maTR);

        if (t == null) throw new NotFoundException($"Không tìm thấy phiếu đổi trả {maTR}");

        decimal? soTienHoan = t.SoTienHoan;
        if ((soTienHoan == null || soTienHoan <= 0) && t.HinhThucHoanTien != "doi_hang")
        {
            var ct = await _context.ChiTietHoaDons.FirstOrDefaultAsync(c => c.SoHD == t.SoHD && c.MaIMEI == t.MaIMEI);
            if (ct != null)
            {
                var hd = await _context.HoaDons.FirstOrDefaultAsync(h => h.SoHD == t.SoHD);
                decimal donGia = ct.DonGiaBan * ct.SlBan;
                if (hd != null && hd.TongTien > 0 && hd.TienGiamGia > 0)
                {
                    decimal tileGiam = hd.TienGiamGia / hd.TongTien;
                    soTienHoan = Math.Round(donGia * (1m - tileGiam), 0);
                }
                else
                {
                    soTienHoan = donGia;
                }
            }
        }

        return new TraHangDto
        {
            MaTR = t.MaTR,
            SoHD = t.SoHD,
            MaIMEI = t.MaIMEI,
            SoIMEI = t.IMEI?.SoIMEI,
            MaHang = t.IMEI?.MaHang,
            TenHang = t.IMEI?.MatHang?.TenHang,
            LoaiYeuCau = t.LoaiYeuCau,
            HinhThucHoanTien = t.HinhThucHoanTien,
            LyDo = t.LyDo,
            TrangThai = t.TrangThai,
            NgayTR = t.NgayTR,
            MaNV = t.MaNV,
            TenNV = t.NhanVienTiepNhan?.HoTen,
            ChecklistNgoaiQuan = t.ChecklistNgoaiQuan,
            KetQuaKiemTra = t.KetQuaKiemTra,
            GhiChuKiemTra = t.GhiChuKiemTra,
            MaNVKiemTra = t.MaNVKiemTra,
            TenNVKiemTra = t.NhanVienKiemTra?.HoTen,
            NgayKiemTra = t.NgayKiemTra,
            MaNVDuyet = t.MaNVDuyet,
            TenNVDuyet = t.NhanVienDuyet?.HoTen,
            NgayDuyet = t.NgayDuyet,
            LyDoTuChoi = t.LyDoTuChoi,
            SoTienHoan = soTienHoan ?? t.SoTienHoan,
            MaGiaoDich = t.MaGiaoDich,
            MaIMEIMoi = t.MaIMEIMoi,
            SoIMEIMoi = t.IMEIMoi?.SoIMEI,
            MaNVHoanTat = t.MaNVHoanTat,
            TenNVHoanTat = t.NhanVienHoanTat?.HoTen,
            NgayHoanTat = t.NgayHoanTat,
            LichSu = t.LichSu.OrderByDescending(l => l.ThoiDiem).Select(l => new TraHangLichSuDto
            {
                MaLichSu = l.MaLichSu,
                TrangThaiCu = l.TrangThaiCu,
                TrangThaiMoi = l.TrangThaiMoi,
                MaNV = l.MaNV,
                TenNV = l.NhanVien?.HoTen,
                ThoiDiem = l.ThoiDiem,
                GhiChu = l.GhiChu
            }).ToList()
        };
    }

    public async Task<(List<TraHangDto> Items, int Total)> GetTraHangsAsync(
        string? trangThai,
        string? loaiYeuCau,
        string? hinhThuc,
        DateOnly? tuNgay,
        DateOnly? denNgay,
        string? keyword,
        int page,
        int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);

        var query = _context.TraHangs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(t => t.TrangThai == trangThai.Trim());
        }

        if (!string.IsNullOrWhiteSpace(loaiYeuCau))
        {
            query = query.Where(t => t.LoaiYeuCau == loaiYeuCau.Trim());
        }

        if (!string.IsNullOrWhiteSpace(hinhThuc))
        {
            query = query.Where(t => t.HinhThucHoanTien == hinhThuc.Trim());
        }

        if (tuNgay.HasValue)
        {
            var fromUtc = tuNgay.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(t => t.NgayTR >= fromUtc);
        }

        if (denNgay.HasValue)
        {
            var toUtc = denNgay.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(t => t.NgayTR <= toUtc);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(t => t.MaTR.ToLower().Contains(kw) ||
                                     t.SoHD.ToLower().Contains(kw) ||
                                     t.MaIMEI.ToLower().Contains(kw) ||
                                     t.LyDo.ToLower().Contains(kw));
        }

        int total = await query.CountAsync();

        var list = await query
            .OrderByDescending(t => t.NgayTR)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var imeiIds = list.Select(l => l.MaIMEI).Distinct().ToList();
        var imeis = await _context.IMEIs.AsNoTracking()
            .Include(i => i.MatHang)
            .Where(i => imeiIds.Contains(i.MaIMEI))
            .ToDictionaryAsync(i => i.MaIMEI);

        var nvIds = list.Select(l => l.MaNV).Distinct().ToList();
        var nvs = await _context.NhanViens.AsNoTracking()
            .Where(n => nvIds.Contains(n.MaNV))
            .ToDictionaryAsync(n => n.MaNV);

        var dtos = list.Select(t =>
        {
            imeis.TryGetValue(t.MaIMEI, out var imei);
            nvs.TryGetValue(t.MaNV, out var nv);
            return new TraHangDto
            {
                MaTR = t.MaTR,
                SoHD = t.SoHD,
                MaIMEI = t.MaIMEI,
                SoIMEI = imei?.SoIMEI,
                MaHang = imei?.MaHang,
                TenHang = imei?.MatHang?.TenHang,
                LoaiYeuCau = t.LoaiYeuCau,
                HinhThucHoanTien = t.HinhThucHoanTien,
                LyDo = t.LyDo,
                TrangThai = t.TrangThai,
                NgayTR = t.NgayTR,
                MaNV = t.MaNV,
                TenNV = nv?.HoTen,
                ChecklistNgoaiQuan = t.ChecklistNgoaiQuan,
                KetQuaKiemTra = t.KetQuaKiemTra,
                GhiChuKiemTra = t.GhiChuKiemTra,
                MaNVKiemTra = t.MaNVKiemTra,
                NgayKiemTra = t.NgayKiemTra,
                MaNVDuyet = t.MaNVDuyet,
                NgayDuyet = t.NgayDuyet,
                LyDoTuChoi = t.LyDoTuChoi,
                SoTienHoan = t.SoTienHoan,
                MaGiaoDich = t.MaGiaoDich,
                MaIMEIMoi = t.MaIMEIMoi,
                MaNVHoanTat = t.MaNVHoanTat,
                NgayHoanTat = t.NgayHoanTat
            };
        }).ToList();

        return (dtos, total);
    }
}
