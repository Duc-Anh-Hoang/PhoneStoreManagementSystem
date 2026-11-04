using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class ChiTietPhieuNhapConfiguration : IEntityTypeConfiguration<ChiTietPhieuNhap>
{
    public void Configure(EntityTypeBuilder<ChiTietPhieuNhap> builder)
    {
        builder.ToTable("chitietphieunhap");
        builder.HasKey(c => new { c.SoPN, c.MaHang });

        builder.Property(c => c.SoPN)
            .HasColumnName("sopn")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(c => c.MaHang)
            .HasColumnName("mahang")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.SlNhap)
            .HasColumnName("slnhap")
            .IsRequired();

        builder.Property(c => c.DonGiaNhap)
            .HasColumnName("dongia_nhap")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(c => c.PhieuNhap)
            .WithMany(p => p.ChiTietPhieuNhaps)
            .HasForeignKey(c => c.SoPN)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.MatHang)
            .WithMany()
            .HasForeignKey(c => c.MaHang)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

