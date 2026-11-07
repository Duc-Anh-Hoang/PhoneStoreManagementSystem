using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Seeders;

public static class DbSeeder
{
    private static async Task EnsureSchemaUpgradedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            const string migrationSql = @"
CREATE SEQUENCE IF NOT EXISTS seq_PhieuNhap AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_HoaDon AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_BaoHanh AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TraHang AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TiepNhan AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_DonHang AS BIGINT START WITH 10 INCREMENT BY 1;

SELECT setval('seq_PhieuNhap', GREATEST((SELECT last_value FROM seq_PhieuNhap), 10), true);
SELECT setval('seq_HoaDon', GREATEST((SELECT last_value FROM seq_HoaDon), 10), true);
SELECT setval('seq_BaoHanh', GREATEST((SELECT last_value FROM seq_BaoHanh), 10), true);
SELECT setval('seq_TraHang', GREATEST((SELECT last_value FROM seq_TraHang), 10), true);
SELECT setval('seq_TiepNhan', GREATEST((SELECT last_value FROM seq_TiepNhan), 10), true);
SELECT setval('seq_DonHang', GREATEST((SELECT last_value FROM seq_DonHang), 10), true);

ALTER TABLE IMEI DROP CONSTRAINT IF EXISTS imei_trangthai_check;
ALTER TABLE IMEI ADD CONSTRAINT imei_trangthai_check
    CHECK (TRANGTHAI IN ('con_hang', 'da_ban', 'tra_hang', 'cho_duyet', 'dang_sua'));

ALTER TABLE TraHang DROP CONSTRAINT IF EXISTS trahang_maimei_key;

ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS LOAIYEUCAU VARCHAR(20) NOT NULL DEFAULT 'loi_ky_thuat';
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS TRANGTHAI VARCHAR(20) NOT NULL DEFAULT 'tiep_nhan';
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS CHECKLIST_NGOAIQUAN TEXT NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS KETQUA_KIEMTRA VARCHAR(30) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS GHICHU_KIEMTRA TEXT NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_KIEMTRA VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_KIEMTRA TIMESTAMPTZ NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_DUYET VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_DUYET TIMESTAMPTZ NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS LYDO_TUCHOI VARCHAR(255) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS SOTIENHOAN DECIMAL(18,2) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MAGIAODICH VARCHAR(50) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MAIMEI_MOI VARCHAR(20) NULL REFERENCES IMEI(MAIMEI);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_HOANTAT VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_HOANTAT TIMESTAMPTZ NULL;

CREATE UNIQUE INDEX IF NOT EXISTS UQ_TraHang_Active_IMEI ON TraHang(MAIMEI)
    WHERE TRANGTHAI NOT IN ('tu_choi', 'da_huy');

CREATE TABLE IF NOT EXISTS TraHangLichSu (
    MALICHSU       BIGSERIAL PRIMARY KEY,
    MATR           VARCHAR(15)  NOT NULL REFERENCES TraHang(MATR) ON DELETE CASCADE,
    TRANGTHAI_CU   VARCHAR(20)  NULL,
    TRANGTHAI_MOI  VARCHAR(20)  NOT NULL,
    MANV           VARCHAR(10)  NOT NULL REFERENCES NhanVien(MANV),
    THOIDIEM       TIMESTAMPTZ  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    GHICHU         VARCHAR(255) NULL
);
CREATE INDEX IF NOT EXISTS IX_TraHangLichSu_MaTR ON TraHangLichSu(MATR);

INSERT INTO ThamSo (MATHAMSO, TENTHAMSO, GIATRI, DONVI) VALUES
    ('HAN_DOI_Y_NGAY', 'Hạn đổi trả đối với lý do đổi ý (nguyên seal)', 7, 'ngày')
ON CONFLICT (MATHAMSO) DO NOTHING;

CREATE OR REPLACE VIEW vw_TonKho AS
SELECT m.MAHANG, m.TENHANG, m.MANHOM, m.MALOAI, m.DONGIA,
       COALESCE(n.TONGNHAP, 0) AS TONGNHAP,
       COALESCE(b.TONGBAN, 0) AS TONGBAN,
       CASE 
         WHEN m.QUANLYIMEI = TRUE THEN COALESCE(im.TONKHO_IMEI, 0)
         ELSE COALESCE(n.TONGNHAP, 0) - COALESCE(b.TONGBAN, 0)
       END AS TONKHO
FROM MatHang m
LEFT JOIN (SELECT MAHANG, SUM(SLNHAP) AS TONGNHAP FROM ChiTietPhieuNhap GROUP BY MAHANG) n ON n.MAHANG = m.MAHANG
LEFT JOIN (SELECT MAHANG, SUM(SLBAN) AS TONGBAN FROM ChiTietHoaDon GROUP BY MAHANG) b ON b.MAHANG = m.MAHANG
LEFT JOIN (SELECT MAHANG, COUNT(*) AS TONKHO_IMEI FROM IMEI WHERE TRANGTHAI = 'con_hang' GROUP BY MAHANG) im ON im.MAHANG = m.MAHANG;

CREATE OR REPLACE VIEW vw_diemtichluy AS
SELECT 
    k.MAKH,
    k.HOTEN,
    k.SDT,
    k.EMAIL,
    COALESCE(s.TongChiTieu, 0) AS TongChiTieu,
    COALESCE(r.TongTienTra, 0) AS TongTienTra,
    GREATEST(0, FLOOR((COALESCE(s.TongChiTieu, 0) - COALESCE(r.TongTienTra, 0)) / NULLIF(p.GIATRI, 0))) AS DiemTichLuy
FROM KhachHang k
LEFT JOIN (SELECT h.MAKH, SUM(h.THANHTIEN) AS TongChiTieu
           FROM HoaDon h
           WHERE h.MAKH IS NOT NULL
           GROUP BY h.MAKH) s ON s.MAKH = k.MAKH
LEFT JOIN (SELECT h.MAKH, SUM(COALESCE(t.SOTIENHOAN, ct.SLBAN * ct.DONGIA_BAN)) AS TongTienTra
           FROM TraHang t
           JOIN HoaDon h ON h.SOHD = t.SOHD
           JOIN ChiTietHoaDon ct ON ct.SOHD = t.SOHD AND ct.MAIMEI = t.MAIMEI
           WHERE t.TRANGTHAI = 'hoan_tat' 
             AND t.HINHTHUCHOANTIEN IN ('tien_mat', 'chuyen_khoan') 
             AND h.MAKH IS NOT NULL
           GROUP BY h.MAKH) r ON r.MAKH = k.MAKH
LEFT JOIN LATERAL (SELECT GIATRI FROM ThamSo WHERE MATHAMSO = 'DIEM_MOI_VND') p ON TRUE;
";
            await context.Database.ExecuteSqlRawAsync(migrationSql);
            logger.LogInformation("Đã kiểm tra và đồng bộ schema bảng TraHang & TraHangLichSu thành công.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cảnh báo khi đồng bộ schema TraHang: {Message}", ex.Message);
        }
    }

    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogWarning("Không thể kết nối đến Database để seed dữ liệu. Vui lòng kiểm tra lại PostgreSQL container hoặc ConnectionString.");
                return;
            }

            // 0. Đảm bảo schema bảng TraHang, TraHangLichSu, IMEI constraint và Views luôn được nâng cấp đầy đủ
            await EnsureSchemaUpgradedAsync(context, logger);

            // 1. Seed ThamSo (kiểm tra độc lập)
            if (!await context.ThamSos.AnyAsync())
            {
                logger.LogInformation("Seeding ThamSo...");
                context.ThamSos.AddRange(
                    new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "Thuế VAT đã gồm trong giá bán", GiaTri = 10, DonVi = "%" },
                    new ThamSo { MaThamSo = "DIEM_MOI_VND", TenThamSo = "Số tiền chi tiêu để được 1 điểm", GiaTri = 100000, DonVi = "VND" },
                    new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả tính từ ngày lập hóa đơn", GiaTri = 30, DonVi = "ngày" },
                    new ThamSo { MaThamSo = "HAN_DOI_Y_NGAY", TenThamSo = "Hạn đổi trả đối với lý do đổi ý (nguyên seal)", GiaTri = 7, DonVi = "ngày" }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed ThamSo thành công.");
            }

            // 2. Seed PhanQuyen (kiểm tra độc lập)
            if (!await context.PhanQuyens.AnyAsync())
            {
                logger.LogInformation("Seeding PhanQuyen...");
                context.PhanQuyens.AddRange(
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên", MoTa = "Toàn quyền quản trị hệ thống" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Manager, TenQuyen = "Quản lý cửa hàng", MoTa = "Quản lý kinh doanh và nhân viên" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Sales, TenQuyen = "Nhân viên bán hàng", MoTa = "Bán hàng tại quầy và xử lý đơn online" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Cashier, TenQuyen = "Thu ngân", MoTa = "Lập hóa đơn và thanh toán" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Stock, TenQuyen = "Nhân viên kho", MoTa = "Quản lý kho và xuất nhập hàng" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Tech, TenQuyen = "Kỹ thuật viên", MoTa = "Bảo hành và sửa chữa thiết bị" }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed PhanQuyen thành công.");
            }

            // 3. Seed NhanVien (đảm bảo đầy đủ 6 nhân viên tương ứng 6 chức vụ)
            var defaultStaff = new List<NhanVien>
            {
                new NhanVien { MaNV = "NV001", HoTen = "Nguyễn Văn Quản Trị", ChucVu = "Quản lý", Sdt = "0901234567", Email = "admin@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV002", HoTen = "Trần Thị Thu Ngân", ChucVu = "Thu ngân", Sdt = "0912345678", Email = "thungan@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV003", HoTen = "Lê Bán Hàng", ChucVu = "Bán hàng", Sdt = "0923456789", Email = "banhang@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV004", HoTen = "Phạm Thủ Kho", ChucVu = "Kho", Sdt = "0934567890", Email = "kho@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV005", HoTen = "Hoàng Kỹ Thuật", ChucVu = "Kỹ thuật & Bảo hành", Sdt = "0945678901", Email = "kythuat@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV006", HoTen = "Đỗ Cửa Hàng Trưởng", ChucVu = "Quản lý", Sdt = "0956789012", Email = "quanly@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam }
            };

            foreach (var staff in defaultStaff)
            {
                if (!await context.NhanViens.AnyAsync(n => n.MaNV == staff.MaNV))
                {
                    context.NhanViens.Add(staff);
                }
            }
            await context.SaveChangesAsync();

            // 4. Seed TaiKhoan (đảm bảo đủ 6 tài khoản mẫu cho cả 6 vai trò trong hệ thống)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);

            var defaultAccounts = new List<TaiKhoan>
            {
                new TaiKhoan { UserName = "admin", PasswordHash = passwordHash, MaNV = "NV001", MaQuyen = AppConstants.Roles.Admin, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "quanly", PasswordHash = passwordHash, MaNV = "NV006", MaQuyen = AppConstants.Roles.Manager, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "thungan", PasswordHash = passwordHash, MaNV = "NV002", MaQuyen = AppConstants.Roles.Cashier, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "banhang", PasswordHash = passwordHash, MaNV = "NV003", MaQuyen = AppConstants.Roles.Sales, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "kho", PasswordHash = passwordHash, MaNV = "NV004", MaQuyen = AppConstants.Roles.Stock, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "kythuat", PasswordHash = passwordHash, MaNV = "NV005", MaQuyen = AppConstants.Roles.Tech, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow }
            };

            foreach (var acc in defaultAccounts)
            {
                if (!await context.TaiKhoans.AnyAsync(t => t.UserName == acc.UserName))
                {
                    context.TaiKhoans.Add(acc);
                }
            }
            await context.SaveChangesAsync();

            // Chỉ cập nhật hash hợp lệ cho 6 tài khoản seed mặc định
            var seedUsernames = new[] { "admin", "quanly", "thungan", "banhang", "kho", "kythuat" };
            var seedAccounts = await context.TaiKhoans
                .Where(t => seedUsernames.Contains(t.UserName))
                .ToListAsync();

            string validHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            bool updated = false;
            foreach (var acc in seedAccounts)
            {
                try
                {
                    if (!BCrypt.Net.BCrypt.Verify("admin123", acc.PasswordHash))
                    {
                        acc.PasswordHash = validHash;
                        updated = true;
                    }
                }
                catch
                {
                    acc.PasswordHash = validHash;
                    updated = true;
                }
            }
            if (updated)
            {
                await context.SaveChangesAsync();
                logger.LogInformation("Đã cập nhật mật khẩu mặc định 'admin123' cho các tài khoản seed.");
            }

            // 5. Seed PhieuNhap & IMEI mẫu nếu kho trống để kiểm thử bán hàng POS
            if (!await context.PhieuNhaps.AnyAsync())
            {
                logger.LogInformation("Seeding PhieuNhap & IMEI mẫu...");
                var now = DateTime.UtcNow;

                var pn1 = new PhieuNhap
                {
                    SoPN = "PN261100001",
                    NgayNhap = now.AddDays(-5),
                    MaNCC = "NCC_AP",
                    MaNV = "NV004"
                };

                var pn2 = new PhieuNhap
                {
                    SoPN = "PN261100002",
                    NgayNhap = now.AddDays(-3),
                    MaNCC = "NCC_SS",
                    MaNV = "NV004"
                };

                context.PhieuNhaps.AddRange(pn1, pn2);
                await context.SaveChangesAsync();

                context.ChiTietPhieuNhaps.AddRange(
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "IP15PM256", SlNhap = 5, DonGiaNhap = 26000000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "IP15P128", SlNhap = 5, DonGiaNhap = 22000000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "AP_PRO2", SlNhap = 10, DonGiaNhap = 4500000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100002", MaHang = "S24U512", SlNhap = 5, DonGiaNhap = 28000000m }
                );
                await context.SaveChangesAsync();

                var sampleImeis = new List<IMEI>
                {
                    // iPhone 15 Pro Max 256GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_IP15PM_01", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_02", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_03", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_04", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_05", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567895", TrangThai = "con_hang" },

                    // iPhone 15 Pro 128GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_IP15P_01", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_02", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_03", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_04", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_05", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567895", TrangThai = "con_hang" },

                    // Samsung Galaxy S24 Ultra 512GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_S24U_01", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_02", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_03", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_04", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_05", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567895", TrangThai = "con_hang" }
                };

                context.IMEIs.AddRange(sampleImeis);
                await context.SaveChangesAsync();
            }

            // 6. Seed Hóa đơn mẫu có gắn IMEI đã bán và Phiếu đổi trả mẫu (nếu chưa có) để phục vụ kiểm thử Đổi trả & Bảo hành
            if (!await context.HoaDons.AnyAsync(h => h.SoHD == "HD261100001"))
            {
                var imeiSold1 = await context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == "IMEI_IP15PM_05");
                var imeiSold2 = await context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == "IMEI_IP15P_05");
                if (imeiSold1 != null && imeiSold2 != null && imeiSold1.TrangThai == "con_hang" && imeiSold2.TrangThai == "con_hang")
                {
                    var saleDate = DateTime.UtcNow.AddDays(-2);
                    var kh = await context.KhachHangs.Select(k => k.MaKH).FirstOrDefaultAsync();

                    var hdSample = new HoaDon
                    {
                        SoHD = "HD261100001",
                        NgayLap = saleDate,
                        MaKH = kh,
                        MaNV = "NV002",
                        KenhBan = "tai_quay",
                        TongTien = 54480000m,
                        TienGiamGia = 0m,
                        PhuongThucThanhToan = "chuyen_khoan"
                    };
                    context.HoaDons.Add(hdSample);
                    await context.SaveChangesAsync();

                    context.ChiTietHoaDons.AddRange(
                        new ChiTietHoaDon { SoHD = "HD261100001", MaHang = "IP15PM256", MaIMEI = "IMEI_IP15PM_05", SlBan = 1, DonGiaBan = 29490000m },
                        new ChiTietHoaDon { SoHD = "HD261100001", MaHang = "IP15P128", MaIMEI = "IMEI_IP15P_05", SlBan = 1, DonGiaBan = 24990000m }
                    );
                    await context.SaveChangesAsync();

                    imeiSold1.TrangThai = "da_ban";
                    imeiSold2.TrangThai = "cho_duyet";
                    await context.SaveChangesAsync();

                    if (!await context.BaoHanhs.AnyAsync(b => b.MaIMEI == "IMEI_IP15PM_05"))
                    {
                        context.BaoHanhs.AddRange(
                            new BaoHanh { MaBH = "BH261100001", SoHD = "HD261100001", MaIMEI = "IMEI_IP15PM_05", NgayBD = saleDate, ThoiHanThang = 12, NgayKT = saleDate.AddMonths(12), DieuKien = "Bảo hành tiêu chuẩn 12 tháng" },
                            new BaoHanh { MaBH = "BH261100002", SoHD = "HD261100001", MaIMEI = "IMEI_IP15P_05", NgayBD = saleDate, ThoiHanThang = 12, NgayKT = saleDate.AddMonths(12), DieuKien = "Bảo hành tiêu chuẩn 12 tháng" }
                        );
                        await context.SaveChangesAsync();
                    }

                    if (!await context.TraHangs.AnyAsync())
                    {
                        var trSample = new TraHang
                        {
                            MaTR = "TR261100001",
                            SoHD = "HD261100001",
                            MaIMEI = "IMEI_IP15P_05",
                            LoaiYeuCau = "loi_ky_thuat",
                            HinhThucHoanTien = "doi_hang",
                            LyDo = "Màn hình xuất hiện sọc xanh nhẹ khi để độ sáng thấp",
                            TrangThai = "tiep_nhan",
                            NgayTR = DateTime.UtcNow.AddHours(-3),
                            MaNV = "NV003",
                            ChecklistNgoaiQuan = "Máy đẹp 99%, không trầy xước, đầy đủ hộp và cáp sạc zin"
                        };
                        context.TraHangs.Add(trSample);
                        context.Set<TraHangLichSu>().Add(new TraHangLichSu
                        {
                            MaTR = "TR261100001",
                            TrangThaiCu = null,
                            TrangThaiMoi = "tiep_nhan",
                            MaNV = "NV003",
                            ThoiDiem = DateTime.UtcNow.AddHours(-3),
                            GhiChu = "Tiếp nhận hồ sơ đổi trả tại quầy"
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình seed dữ liệu.");
        }
    }
}
