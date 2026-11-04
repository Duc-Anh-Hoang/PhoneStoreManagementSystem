using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class ChiTietThuocTinhConfiguration : IEntityTypeConfiguration<ChiTietThuocTinh>
{
    public void Configure(EntityTypeBuilder<ChiTietThuocTinh> builder)
    {
        builder.ToTable("chitietthuoctinh");
        builder.HasKey(c => new { c.MaHang, c.MaThuocTinh });

        builder.Property(c => c.MaHang).HasColumnName("mahang").HasMaxLength(10).IsRequired();
        builder.Property(c => c.MaThuocTinh).HasColumnName("mathuoctinh").HasMaxLength(10).IsRequired();
        builder.Property(c => c.GiaTri).HasColumnName("gia_tri").HasMaxLength(100).IsRequired();

        builder.HasOne(c => c.MatHang)
            .WithMany()
            .HasForeignKey(c => c.MaHang)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ThuocTinh)
            .WithMany()
            .HasForeignKey(c => c.MaThuocTinh)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
