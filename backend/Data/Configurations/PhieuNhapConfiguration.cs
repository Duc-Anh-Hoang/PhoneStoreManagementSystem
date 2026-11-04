using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class PhieuNhapConfiguration : IEntityTypeConfiguration<PhieuNhap>
{
    public void Configure(EntityTypeBuilder<PhieuNhap> builder)
    {
        builder.ToTable("phieunhap");
        builder.HasKey(p => p.SoPN);

        builder.Property(p => p.SoPN)
            .HasColumnName("sopn")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(p => p.NgayNhap)
            .HasColumnName("ngaynhap")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(p => p.MaNCC)
            .HasColumnName("mancc")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.MaNV)
            .HasColumnName("manv")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasOne(p => p.NhaCungCap)
            .WithMany()
            .HasForeignKey(p => p.MaNCC)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.NhanVien)
            .WithMany()
            .HasForeignKey(p => p.MaNV)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

