namespace PhoneStoreAPI.Common.Constants;

public static class AppConstants
{
    public static class Roles
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
        public const string Sales = "sales";
        public const string Cashier = "cashier";
        public const string Stock = "stock";
        public const string Tech = "tech";
        public const string Customer = "customer";
    }

    public static class EmployeeStatus
    {
        public const string DangLam = "dang_lam";
        public const string DaNghi = "da_nghi";
    }

    public static class AccountStatus
    {
        public const string HoatDong = "hoat_dong";
        public const string BiKhoa = "bi_khoa";
    }

    public static class OrderStatus
    {
        public const string ChoXacNhan = "cho_xac_nhan";
        public const string DaXacNhan = "da_xac_nhan";
        public const string DangGiao = "dang_giao";
        public const string HoanThanh = "hoan_thanh";
        public const string DaHuy = "da_huy";
    }

    public static class SaleChannels
    {
        public const string TaiQuay = "tai_quay";
        public const string Online = "online";
    }

    public static class InventoryConstants
    {
        public const int CanhBaoTonToiThieu = 5;
    }

    public static class ImeiStatus
    {
        public const string ConHang = "con_hang";
        public const string DaBan = "da_ban";
        public const string TraHang = "tra_hang";
    }

    public static readonly string[] ValidPositions = new[]
    {
        "Quản lý",
        "Bán hàng",
        "Thu ngân",
        "Kho",
        "Kỹ thuật & Bảo hành"
    };
}
