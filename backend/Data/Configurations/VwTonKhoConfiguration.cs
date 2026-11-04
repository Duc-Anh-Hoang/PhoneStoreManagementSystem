using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities.Views;

namespace PhoneStoreAPI.Data.Configurations;

public class VwTonKhoConfiguration : IEntityTypeConfiguration<VwTonKho>
{
    public void Configure(EntityTypeBuilder<VwTonKho> builder)
    {
        builder.HasNoKey();
        builder.ToView("vw_tonkho");

        builder.Property(v => v.MaHang).HasColumnName("mahang");
        builder.Property(v => v.TenHang).HasColumnName("tenhang");
        builder.Property(v => v.MaNhom).HasColumnName("manhom");
        builder.Property(v => v.MaLoai).HasColumnName("maloai");
        builder.Property(v => v.DonGia).HasColumnName("dongia");
        builder.Property(v => v.TongNhap).HasColumnName("tongnhap");
        builder.Property(v => v.TongBan).HasColumnName("tongban");
        builder.Property(v => v.TonKho).HasColumnName("tonkho");
    }
}
