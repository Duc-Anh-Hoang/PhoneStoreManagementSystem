using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class MatHangConfiguration : IEntityTypeConfiguration<MatHang>
{
    public void Configure(EntityTypeBuilder<MatHang> builder)
    {
        builder.ToTable("mathang");
        builder.HasKey(m => m.MaHang);

        builder.Property(m => m.MaHang).HasColumnName("mahang").HasMaxLength(10).IsRequired();
        builder.Property(m => m.TenHang).HasColumnName("tenhang").HasMaxLength(100).IsRequired();
        builder.Property(m => m.DonGia).HasColumnName("dongia").HasPrecision(18, 2).IsRequired();
        builder.Property(m => m.Dvt).HasColumnName("dvt").HasMaxLength(20).IsRequired();
        builder.Property(m => m.MaNhom).HasColumnName("manhom").HasMaxLength(10).IsRequired();
        builder.Property(m => m.MaLoai).HasColumnName("maloai").HasMaxLength(10).IsRequired();
        builder.Property(m => m.QuanLyImei).HasColumnName("quanlyimei").IsRequired();
        builder.Property(m => m.ThoiHanBaoHanh).HasColumnName("thoihanbaohanh").IsRequired();

        builder.HasOne(m => m.NhomHang)
            .WithMany()
            .HasForeignKey(m => m.MaNhom)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.LoaiHang)
            .WithMany()
            .HasForeignKey(m => m.MaLoai)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
