namespace PhoneStoreAPI.Features.Sales.Returns.DTOs;

public class CreateTraHangRequest
{
    public string? SoHD { get; set; }
    public string? MaIMEI { get; set; }
    public string? SoIMEI { get; set; }
    public string LoaiYeuCau { get; set; } = "loi_ky_thuat"; // loi_ky_thuat, doi_y
    public string HinhThucHoanTien { get; set; } = "tien_mat"; // tien_mat, chuyen_khoan, doi_hang
    public string LyDo { get; set; } = string.Empty;
    public string? ChecklistNgoaiQuan { get; set; }
}

public class KiemTraTraHangRequest
{
    public string KetQuaKiemTra { get; set; } = string.Empty; // loi_nsx, khong_loi, hu_hong_do_nguoi_dung
    public string GhiChuKiemTra { get; set; } = string.Empty;
}

public class DuyetTraHangRequest
{
    public bool ChapNhan { get; set; }
    public string? LyDoTuChoi { get; set; }
}

public class HoanTatTraHangRequest
{
    public string? MaGiaoDich { get; set; }
    public string? SoIMEIMoi { get; set; }
    public string? MaIMEIMoi { get; set; }
}

public class DoiHinhThucRequest
{
    public string HinhThucMoi { get; set; } = "tien_mat"; // tien_mat, chuyen_khoan
    public string? LyDo { get; set; }
}

public class HuyTraHangRequest
{
    public string LyDo { get; set; } = string.Empty;
}

public class TraHangLichSuDto
{
    public long MaLichSu { get; set; }
    public string? TrangThaiCu { get; set; }
    public string TrangThaiMoi { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string? TenNV { get; set; }
    public DateTime ThoiDiem { get; set; }
    public string? GhiChu { get; set; }
}

public class TraHangDto
{
    public string MaTR { get; set; } = string.Empty;
    public string SoHD { get; set; } = string.Empty;
    public string MaIMEI { get; set; } = string.Empty;
    public string? SoIMEI { get; set; }
    public string? MaHang { get; set; }
    public string? TenHang { get; set; }
    public string LoaiYeuCau { get; set; } = string.Empty;
    public string HinhThucHoanTien { get; set; } = string.Empty;
    public string LyDo { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayTR { get; set; }
    public string MaNV { get; set; } = string.Empty;
    public string? TenNV { get; set; }

    public string? ChecklistNgoaiQuan { get; set; }
    public string? KetQuaKiemTra { get; set; }
    public string? GhiChuKiemTra { get; set; }
    public string? MaNVKiemTra { get; set; }
    public string? TenNVKiemTra { get; set; }
    public DateTime? NgayKiemTra { get; set; }

    public string? MaNVDuyet { get; set; }
    public string? TenNVDuyet { get; set; }
    public DateTime? NgayDuyet { get; set; }
    public string? LyDoTuChoi { get; set; }

    public decimal? SoTienHoan { get; set; }
    public string? MaGiaoDich { get; set; }
    public string? MaIMEIMoi { get; set; }
    public string? SoIMEIMoi { get; set; }
    public string? MaNVHoanTat { get; set; }
    public string? TenNVHoanTat { get; set; }
    public DateTime? NgayHoanTat { get; set; }

    public List<TraHangLichSuDto> LichSu { get; set; } = new();
}

public class TraCuuImeiTraHangDto
{
    public string SoIMEI { get; set; } = string.Empty;
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public string SoHD { get; set; } = string.Empty;
    public DateTime NgayMua { get; set; }
    public decimal DonGiaBan { get; set; }
    public decimal SoTienThucTra { get; set; }
    public decimal HanDoiTraNgay { get; set; }
    public decimal HanDoiYNgay { get; set; }
    public DateTime NgayHetHanDoiTra { get; set; }
    public DateTime NgayHetHanDoiY { get; set; }
    public bool ConHanDoiTra { get; set; }
    public bool ConHanDoiY { get; set; }
    public bool CoTheTiepNhan { get; set; }
    public string? LyDoKhongHopLe { get; set; }
}
