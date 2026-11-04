using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class KhachHangConfiguration : IEntityTypeConfiguration<KhachHang>
{
    public void Configure(EntityTypeBuilder<KhachHang> builder)
    {
        builder.ToTable("khachhang");
        builder.HasKey(k => k.MaKH);

        builder.Property(k => k.MaKH)
            .HasColumnName("makh")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(k => k.HoTen)
            .HasColumnName("hoten")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(k => k.DiaChi)
            .HasColumnName("diachi")
            .HasMaxLength(255);

        builder.Property(k => k.Sdt)
            .HasColumnName("sdt")
            .HasMaxLength(15);

        builder.Property(k => k.Email)
            .HasColumnName("email")
            .HasMaxLength(100);
    }
}

