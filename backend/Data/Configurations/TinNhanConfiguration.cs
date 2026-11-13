using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class TinNhanConfiguration : IEntityTypeConfiguration<TinNhan>
{
    public void Configure(EntityTypeBuilder<TinNhan> builder)
    {
        builder.ToTable("tinnhan");
        builder.HasKey(e => e.MaTinNhan);

        builder.Property(e => e.MaPhienChat).HasMaxLength(36);
        builder.Property(e => e.NguoiGui).HasMaxLength(20);
        builder.Property(e => e.NguoiGuiId).HasMaxLength(36);
        builder.Property(e => e.NoiDung).HasColumnType("text");
        builder.Property(e => e.DaDoc).HasDefaultValue(false);

        builder.HasIndex(e => new { e.MaPhienChat, e.ThoiGian });

        builder.HasOne(e => e.PhienChat)
            .WithMany(p => p.TinNhans)
            .HasForeignKey(e => e.MaPhienChat)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
