using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class PhienChatConfiguration : IEntityTypeConfiguration<PhienChat>
{
    public void Configure(EntityTypeBuilder<PhienChat> builder)
    {
        builder.ToTable("phienchat");
        builder.HasKey(e => e.MaPhienChat);

        builder.Property(e => e.MaPhienChat).HasMaxLength(36);
        builder.Property(e => e.MaKH).HasMaxLength(10);
        builder.Property(e => e.KhachHangTen).HasMaxLength(100).HasDefaultValue("Khách vãng lai");
        builder.Property(e => e.KhachHangSdt).HasMaxLength(15);
        builder.Property(e => e.MaNV).HasMaxLength(10);
        builder.Property(e => e.TrangThai).HasMaxLength(20).HasDefaultValue("WAITING");

        builder.HasIndex(e => e.TrangThai);
        builder.HasIndex(e => e.NgayTao);

        builder.HasOne(e => e.KhachHang)
            .WithMany()
            .HasForeignKey(e => e.MaKH)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.NhanVien)
            .WithMany()
            .HasForeignKey(e => e.MaNV)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
