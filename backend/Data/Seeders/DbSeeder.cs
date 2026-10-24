using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            // 1. Seed ThamSo
            if (!await context.ThamSos.AnyAsync())
            {
                logger.LogInformation("Seeding ThamSo...");
                context.ThamSos.AddRange(
                    new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "Thuế VAT đã gồm trong giá bán", GiaTri = 10, DonVi = "%" },
                    new ThamSo { MaThamSo = "DIEM_MOI_VND", TenThamSo = "Số tiền chi tiêu để được 1 điểm", GiaTri = 100000, DonVi = "VND" },
                    new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả tính từ ngày lập hóa đơn", GiaTri = 30, DonVi = "ngày" }
                );
                await context.SaveChangesAsync();
            }

            // 2. Seed PhanQuyen
            if (!await context.PhanQuyens.AnyAsync())
            {
                logger.LogInformation("Seeding PhanQuyen...");
                context.PhanQuyens.AddRange(
                    new PhanQuyen { MaQuyen = "admin", TenQuyen = "Quản trị viên", MoTa = "Toàn quyền quản trị hệ thống" },
                    new PhanQuyen { MaQuyen = "manager", TenQuyen = "Quản lý cửa hàng", MoTa = "Quản lý kinh doanh và nhân viên" },
                    new PhanQuyen { MaQuyen = "sales", TenQuyen = "Nhân viên bán hàng", MoTa = "Bán hàng tại quầy và xử lý đơn online" },
                    new PhanQuyen { MaQuyen = "cashier", TenQuyen = "Thu ngân", MoTa = "Lập hóa đơn và thanh toán" },
                    new PhanQuyen { MaQuyen = "stock", TenQuyen = "Nhân viên kho", MoTa = "Quản lý kho và xuất nhập hàng" },
                    new PhanQuyen { MaQuyen = "tech", TenQuyen = "Kỹ thuật viên", MoTa = "Bảo hành và sửa chữa thiết bị" }
                );
                await context.SaveChangesAsync();
            }

            // 3. Seed NhanVien & TaiKhoan mặc định
            if (!await context.NhanViens.AnyAsync())
            {
                logger.LogInformation("Seeding NhanVien & TaiKhoan...");
                var adminStaff = new NhanVien
                {
                    MaNV = "NV001",
                    HoTen = "Quản Trị Viên Hệ Thống",
                    ChucVu = "Quản lý",
                    Sdt = "0901234567",
                    Email = "admin@cellphones.vn",
                    NgayVaoLam = DateTime.UtcNow,
                    TrangThai = "dang_lam"
                };
                context.NhanViens.Add(adminStaff);
                await context.SaveChangesAsync();

                // Mật khẩu mặc định: 'admin123'
                string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");

                var adminAccount = new TaiKhoan
                {
                    UserName = "admin",
                    PasswordHash = passwordHash,
                    MaNV = "NV001",
                    MaQuyen = "admin",
                    TrangThai = "hoat_dong",
                    NgayTao = DateTime.UtcNow
                };
                context.TaiKhoans.Add(adminAccount);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình Seed dữ liệu.");
        }
    }
}
