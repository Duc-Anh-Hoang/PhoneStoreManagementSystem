using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class KhuyenMaiConfiguration : IEntityTypeConfiguration<KhuyenMai>
{
    public void Configure(EntityTypeBuilder<KhuyenMai> builder)
    {
        builder.ToTable("khuyenmai");
        builder.HasKey(k => k.MaKM);

        builder.Property(k => k.MaKM)
            .HasColumnName("makm")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(k => k.TenKM)
            .HasColumnName("tenkm")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(k => k.PhanTramGiam)
            .HasColumnName("phantramgiam")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(k => k.NgayBD)
            .HasColumnName("ngaybd")
            .IsRequired();

        builder.Property(k => k.NgayKT)
            .HasColumnName("ngaykt")
            .IsRequired();

        builder.Property(k => k.TamDung)
            .HasColumnName("tamdung")
            .HasDefaultValue(false)
            .IsRequired();
    }
}

