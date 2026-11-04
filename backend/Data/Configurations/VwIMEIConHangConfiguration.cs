using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities.Views;

namespace PhoneStoreAPI.Data.Configurations;

public class VwIMEIConHangConfiguration : IEntityTypeConfiguration<VwIMEIConHang>
{
    public void Configure(EntityTypeBuilder<VwIMEIConHang> builder)
    {
        builder.HasNoKey();
        builder.ToView("vw_imeiconhang");

        builder.Property(v => v.MaIMEI).HasColumnName("maimei");
        builder.Property(v => v.SoIMEI).HasColumnName("soimei");
        builder.Property(v => v.MaHang).HasColumnName("mahang");
        builder.Property(v => v.TenHang).HasColumnName("tenhang");
        builder.Property(v => v.SoPN).HasColumnName("sopn");
        builder.Property(v => v.NgayNhap).HasColumnName("ngaynhap");
    }
}
