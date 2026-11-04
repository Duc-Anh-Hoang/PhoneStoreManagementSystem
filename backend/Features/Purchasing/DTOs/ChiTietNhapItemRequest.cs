using System.Text.Json.Serialization;

namespace PhoneStoreAPI.Features.Purchasing.DTOs;

public class ChiTietNhapItemRequest
{
    public string MaHang { get; set; } = string.Empty;
    public int SlNhap { get; set; }
    public decimal DonGiaNhap { get; set; }

    public List<string>? Imeis { get; set; }

    /// <summary>
    /// Alias tương thích frontend admin.js gửi danhSachImei
    /// </summary>
    [JsonPropertyName("danhSachImei")]
    public List<string>? DanhSachImei { get; set; }

    public List<string> GetResolvedImeis()
    {
        var list = Imeis ?? DanhSachImei ?? new List<string>();
        return list.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
    }
}

