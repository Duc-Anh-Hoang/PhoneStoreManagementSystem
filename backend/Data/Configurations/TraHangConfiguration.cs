using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class TraHangConfiguration : IEntityTypeConfiguration<TraHang>
{
    public void Configure(EntityTypeBuilder<TraHang> builder)
    {
        builder.ToTable("trahang");
        builder.HasKey(t => t.MaTR);

        builder.HasOne(t => t.HoaDon)
            .WithMany()
            .HasForeignKey(t => t.SoHD)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.IMEI)
            .WithMany()
            .HasForeignKey(t => t.MaIMEI)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.IMEIMoi)
            .WithMany()
            .HasForeignKey(t => t.MaIMEIMoi)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.NhanVienTiepNhan)
            .WithMany()
            .HasForeignKey(t => t.MaNV)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.NhanVienKiemTra)
            .WithMany()
            .HasForeignKey(t => t.MaNVKiemTra)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.NhanVienDuyet)
            .WithMany()
            .HasForeignKey(t => t.MaNVDuyet)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.NhanVienHoanTat)
            .WithMany()
            .HasForeignKey(t => t.MaNVHoanTat)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.LichSu)
            .WithOne(l => l.TraHang)
            .HasForeignKey(l => l.MaTR)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TraHangLichSuConfiguration : IEntityTypeConfiguration<TraHangLichSu>
{
    public void Configure(EntityTypeBuilder<TraHangLichSu> builder)
    {
        builder.ToTable("trahanglichsu");
        builder.HasKey(l => l.MaLichSu);

        builder.HasOne(l => l.NhanVien)
            .WithMany()
            .HasForeignKey(l => l.MaNV)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
