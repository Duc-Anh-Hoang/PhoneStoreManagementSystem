using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class NhaCungCapConfiguration : IEntityTypeConfiguration<NhaCungCap>
{
    public void Configure(EntityTypeBuilder<NhaCungCap> builder)
    {
        builder.ToTable("nhacungcap");
        builder.HasKey(n => n.MaNCC);

        builder.Property(n => n.MaNCC)
            .HasColumnName("mancc")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(n => n.TenNCC)
            .HasColumnName("tenncc")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(n => n.DiaChi)
            .HasColumnName("diachi")
            .HasMaxLength(255);

        builder.Property(n => n.Sdt)
            .HasColumnName("sdt")
            .HasMaxLength(15);

        builder.Property(n => n.Email)
            .HasColumnName("email")
            .HasMaxLength(100);
    }
}

