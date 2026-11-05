using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Configurations;

public class ChiTietHoaDonConfiguration : IEntityTypeConfiguration<ChiTietHoaDon>
{
    public void Configure(EntityTypeBuilder<ChiTietHoaDon> builder)
    {
        builder.ToTable("chitiethoadon");

        // MaCTHD là SERIAL PRIMARY KEY (DB tự sinh)
        builder.Property(c => c.MaCTHD)
            .ValueGeneratedOnAdd();
    }
}
