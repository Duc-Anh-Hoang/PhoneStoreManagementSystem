using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class IMEIConfiguration : IEntityTypeConfiguration<IMEI>
{
    public void Configure(EntityTypeBuilder<IMEI> builder)
    {
        builder.ToTable("imei");
        builder.HasKey(i => i.MaIMEI);

        builder.Property(i => i.MaIMEI).HasColumnName("maimei").HasMaxLength(20).IsRequired();
        builder.Property(i => i.MaHang).HasColumnName("mahang").HasMaxLength(10).IsRequired();
        builder.Property(i => i.SoPN).HasColumnName("sopn").HasMaxLength(15).IsRequired();
        builder.Property(i => i.SoIMEI).HasColumnName("soimei").HasMaxLength(20).IsRequired();
        builder.Property(i => i.TrangThai).HasColumnName("trangthai").HasMaxLength(20).IsRequired();

        builder.HasIndex(i => i.SoIMEI).IsUnique();

        builder.HasOne(i => i.MatHang)
            .WithMany()
            .HasForeignKey(i => i.MaHang)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
