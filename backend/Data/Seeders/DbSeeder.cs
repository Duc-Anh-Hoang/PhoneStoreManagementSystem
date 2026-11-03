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

            // 3. Seed NhanVien (kiểm tra độc lập)
            if (!await context.NhanViens.AnyAsync())
            {
                logger.LogInformation("Seeding NhanVien...");
                context.NhanViens.AddRange(
                    new NhanVien { MaNV = "NV001", HoTen = "Nguyễn Văn Quản Lý", ChucVu = "Quản lý", Sdt = "0901234567", Email = "admin@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                    new NhanVien { MaNV = "NV002", HoTen = "Trần Thị Thu Ngân", ChucVu = "Thu ngân", Sdt = "0912345678", Email = "thungan@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                    new NhanVien { MaNV = "NV003", HoTen = "Lê Bán Hàng", ChucVu = "Bán hàng", Sdt = "0923456789", Email = "banhang@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                    new NhanVien { MaNV = "NV004", HoTen = "Phạm Thủ Kho", ChucVu = "Kho", Sdt = "0934567890", Email = "kho@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                    new NhanVien { MaNV = "NV005", HoTen = "Hoàng Kỹ Thuật", ChucVu = "Kỹ thuật & Bảo hành", Sdt = "0945678901", Email = "kythuat@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed NhanVien thành công.");
            }

            // 4. Seed TaiKhoan (kiểm tra độc lập)
            if (!await context.TaiKhoans.AnyAsync())
            {
                logger.LogInformation("Seeding TaiKhoan...");
                string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);

                context.TaiKhoans.AddRange(
                    new TaiKhoan { UserName = "admin", PasswordHash = passwordHash, MaNV = "NV001", MaQuyen = AppConstants.Roles.Admin, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                    new TaiKhoan { UserName = "thungan", PasswordHash = passwordHash, MaNV = "NV002", MaQuyen = AppConstants.Roles.Cashier, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                    new TaiKhoan { UserName = "banhang", PasswordHash = passwordHash, MaNV = "NV003", MaQuyen = AppConstants.Roles.Sales, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed TaiKhoan admin thành công.");
            }

            // Đảm bảo các tài khoản mẫu luôn có hash BCrypt hợp lệ của mật khẩu 'admin123'
            var accounts = await context.TaiKhoans.ToListAsync();
            string validHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            bool updated = false;
            foreach (var acc in accounts)
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
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình seed dữ liệu.");
        }
    }
}
