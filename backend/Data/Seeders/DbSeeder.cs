using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogWarning("Không thể kết nối đến Database để seed dữ liệu. Vui lòng kiểm tra lại PostgreSQL container hoặc ConnectionString.");
                return;
            }

            // 1. Seed ThamSo (kiểm tra độc lập)
            if (!await context.ThamSos.AnyAsync())
            {
                logger.LogInformation("Seeding ThamSo...");
                context.ThamSos.AddRange(
                    new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "Thuế VAT đã gồm trong giá bán", GiaTri = 10, DonVi = "%" },
                    new ThamSo { MaThamSo = "DIEM_MOI_VND", TenThamSo = "Số tiền chi tiêu để được 1 điểm", GiaTri = 100000, DonVi = "VND" },
                    new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả tính từ ngày lập hóa đơn", GiaTri = 30, DonVi = "ngày" }
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
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình seed dữ liệu.");
        }
    }
}