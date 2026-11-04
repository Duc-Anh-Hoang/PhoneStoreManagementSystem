namespace PhoneStoreAPI.Features.Products.DTOs;

public class ProductDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public string Dvt { get; set; } = "Cái";
    public string MaNhom { get; set; } = string.Empty;
    public string? TenNhom { get; set; }
    public string MaLoai { get; set; } = string.Empty;
    public string? TenLoai { get; set; }
    public bool QuanLyImei { get; set; }
    public int ThoiHanBaoHanh { get; set; }
    public long TonKho { get; set; }
}

public class ProductDetailDto : ProductDto
{
    public List<ProductSpecDto> ThongSo { get; set; } = new();
}

public class ProductSpecDto
{
    public string MaThuocTinh { get; set; } = string.Empty;
    public string TenThuocTinh { get; set; } = string.Empty;
    public string GiaTri { get; set; } = string.Empty;
}

public class ProductQueryParameters
{
    public string? Keyword { get; set; }
    public string? MaNhom { get; set; }
    public string? MaLoai { get; set; }
    public decimal? MinGia { get; set; }
    public decimal? MaxGia { get; set; }
    public string? Sort { get; set; } // "giaTang", "giaGiam", "ten"
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class CreateProductRequest
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public string Dvt { get; set; } = "Cái";
    public string MaNhom { get; set; } = string.Empty;
    public string MaLoai { get; set; } = string.Empty;
    public bool QuanLyImei { get; set; }
    public int ThoiHanBaoHanh { get; set; }
}

public class UpdateProductRequest
{
    public string TenHang { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public string Dvt { get; set; } = "Cái";
    public string MaNhom { get; set; } = string.Empty;
    public string MaLoai { get; set; } = string.Empty;
    public bool QuanLyImei { get; set; }
    public int ThoiHanBaoHanh { get; set; }
}

public class UpdateProductSpecRequest
{
    public string MaThuocTinh { get; set; } = string.Empty;
    public string GiaTri { get; set; } = string.Empty;
}
