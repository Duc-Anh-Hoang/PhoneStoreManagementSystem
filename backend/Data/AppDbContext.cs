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
    public DbSet<PhienChat> PhienChats => Set<PhienChat>();
    public DbSet<TinNhan> TinNhans => Set<TinNhan>();

    // Bảng phục vụ tự động duyệt đơn (plan (8).md)
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<OrderStatusLog> OrderStatusLogs => Set<OrderStatusLog>();
    public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();
    public DbSet<OrderSetting> OrderSettings => Set<OrderSetting>();

    // Bảng phục vụ Giỏ hàng và Thanh toán
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PaymentRecord> Payments => Set<PaymentRecord>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<RefundRequest> RefundRequests => Set<RefundRequest>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();

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

        // Ràng buộc duy nhất & chỉ mục cho Giỏ hàng & Thanh toán
        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasIndex(v => v.Sku).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_variants_stock", "stock >= 0"));
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasIndex(c => c.CartToken).IsUnique();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasIndex(ci => new { ci.CartId, ci.VariantId }).IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(o => o.IdempotencyKey).IsUnique();
            entity.HasIndex(o => o.OrderCode).IsUnique();
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasIndex(p => p.ProviderTxnId).IsUnique();
        });

        modelBuilder.Entity<RefundRequest>(entity =>
        {
            entity.HasIndex(r => r.ProviderRefundId).IsUnique();
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasIndex(v => v.Code).IsUnique();
        });

        // Apply any configuration classes from Data/Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
