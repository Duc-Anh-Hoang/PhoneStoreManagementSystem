using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PhanQuyen> PhanQuyens => Set<PhanQuyen>();
    public DbSet<NhanVien> NhanViens => Set<NhanVien>();
    public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
    public DbSet<NhomHang> NhomHangs => Set<NhomHang>();
    public DbSet<LoaiHang> LoaiHangs => Set<LoaiHang>();
    public DbSet<ThuocTinh> ThuocTinhs => Set<ThuocTinh>();
    public DbSet<MatHang> MatHangs => Set<MatHang>();
    public DbSet<ChiTietThuocTinh> ChiTietThuocTinhs => Set<ChiTietThuocTinh>();
    public DbSet<NhaCungCap> NhaCungCaps => Set<NhaCungCap>();
    public DbSet<PhieuNhap> PhieuNhaps => Set<PhieuNhap>();
    public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps => Set<ChiTietPhieuNhap>();
    public DbSet<IMEI> IMEIs => Set<IMEI>();
    public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
    public DbSet<KhuyenMai> KhuyenMais => Set<KhuyenMai>();
    public DbSet<HoaDon> HoaDons => Set<HoaDon>();
    public DbSet<ChiTietHoaDon> ChiTietHoaDons => Set<ChiTietHoaDon>();
    public DbSet<BaoHanh> BaoHanhs => Set<BaoHanh>();
    public DbSet<TraHang> TraHangs => Set<TraHang>();
    public DbSet<PhieuTiepNhanBaoHanh> PhieuTiepNhanBaoHanhs => Set<PhieuTiepNhanBaoHanh>();
    public DbSet<DonHangOnline> DonHangOnlines => Set<DonHangOnline>();
    public DbSet<ChiTietDonHangOnline> ChiTietDonHangOnlines => Set<ChiTietDonHangOnline>();
    public DbSet<ThamSo> ThamSos => Set<ThamSo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite primary keys
        modelBuilder.Entity<ChiTietThuocTinh>()
            .HasKey(c => new { c.MaHang, c.MaThuocTinh });

        modelBuilder.Entity<ChiTietPhieuNhap>()
            .HasKey(c => new { c.SoPN, c.MaHang });

        modelBuilder.Entity<ChiTietDonHangOnline>()
            .HasKey(c => new { c.MaDH, c.MaHang });

        // Apply any configuration classes from Data/Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
