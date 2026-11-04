using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class ThamSoConfiguration : IEntityTypeConfiguration<ThamSo>
{
    public void Configure(EntityTypeBuilder<ThamSo> builder)
    {
        builder.ToTable("thamso");
        builder.HasKey(t => t.MaThamSo);

        builder.Property(t => t.MaThamSo)
            .HasColumnName("mathamso")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(t => t.TenThamSo)
            .HasColumnName("tenthamso")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.GiaTri)
            .HasColumnName("giatri")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.DonVi)
            .HasColumnName("donvi")
            .HasMaxLength(20);
    }
}

