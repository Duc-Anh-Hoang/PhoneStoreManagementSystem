using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Data.Seeders;

public static class DbSeeder
{
    private static async Task EnsureSchemaUpgradedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            const string migrationSql = @"
CREATE SEQUENCE IF NOT EXISTS seq_PhieuNhap AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_HoaDon AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_BaoHanh AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TraHang AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TiepNhan AS BIGINT START WITH 10 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_DonHang AS BIGINT START WITH 10 INCREMENT BY 1;

SELECT setval('seq_PhieuNhap', GREATEST((SELECT last_value FROM seq_PhieuNhap), 10), true);
SELECT setval('seq_HoaDon', GREATEST((SELECT last_value FROM seq_HoaDon), 10), true);
SELECT setval('seq_BaoHanh', GREATEST((SELECT last_value FROM seq_BaoHanh), 10), true);
SELECT setval('seq_TraHang', GREATEST((SELECT last_value FROM seq_TraHang), 10), true);
SELECT setval('seq_TiepNhan', GREATEST((SELECT last_value FROM seq_TiepNhan), 10), true);
SELECT setval('seq_DonHang', GREATEST((SELECT last_value FROM seq_DonHang), 10), true);

ALTER TABLE IMEI DROP CONSTRAINT IF EXISTS imei_trangthai_check;
ALTER TABLE IMEI ADD CONSTRAINT imei_trangthai_check
    CHECK (TRANGTHAI IN ('con_hang', 'da_ban', 'tra_hang', 'cho_duyet', 'dang_sua'));

ALTER TABLE TraHang DROP CONSTRAINT IF EXISTS trahang_maimei_key;

ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS LOAIYEUCAU VARCHAR(20) NOT NULL DEFAULT 'loi_ky_thuat';
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS TRANGTHAI VARCHAR(20) NOT NULL DEFAULT 'tiep_nhan';
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS CHECKLIST_NGOAIQUAN TEXT NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS KETQUA_KIEMTRA VARCHAR(30) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS GHICHU_KIEMTRA TEXT NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_KIEMTRA VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_KIEMTRA TIMESTAMPTZ NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_DUYET VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_DUYET TIMESTAMPTZ NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS LYDO_TUCHOI VARCHAR(255) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS SOTIENHOAN DECIMAL(18,2) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MAGIAODICH VARCHAR(50) NULL;
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MAIMEI_MOI VARCHAR(20) NULL REFERENCES IMEI(MAIMEI);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS MANV_HOANTAT VARCHAR(10) NULL REFERENCES NhanVien(MANV);
ALTER TABLE TraHang ADD COLUMN IF NOT EXISTS NGAY_HOANTAT TIMESTAMPTZ NULL;

CREATE UNIQUE INDEX IF NOT EXISTS UQ_TraHang_Active_IMEI ON TraHang(MAIMEI)
    WHERE TRANGTHAI NOT IN ('tu_choi', 'da_huy');

CREATE TABLE IF NOT EXISTS TraHangLichSu (
    MALICHSU       BIGSERIAL PRIMARY KEY,
    MATR           VARCHAR(15)  NOT NULL REFERENCES TraHang(MATR) ON DELETE CASCADE,
    TRANGTHAI_CU   VARCHAR(20)  NULL,
    TRANGTHAI_MOI  VARCHAR(20)  NOT NULL,
    MANV           VARCHAR(10)  NOT NULL REFERENCES NhanVien(MANV),
    THOIDIEM       TIMESTAMPTZ  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    GHICHU         VARCHAR(255) NULL
);
CREATE INDEX IF NOT EXISTS IX_TraHangLichSu_MaTR ON TraHangLichSu(MATR);

CREATE TABLE IF NOT EXISTS phienchat (
    maphienchat VARCHAR(36) PRIMARY KEY,
    makh VARCHAR(10) NULL REFERENCES KhachHang(MAKH) ON DELETE SET NULL,
    khachhang_ten VARCHAR(100) NOT NULL DEFAULT 'Khách hàng',
    khachhang_sdt VARCHAR(15) NULL,
    manv VARCHAR(10) NULL REFERENCES NhanVien(MANV) ON DELETE SET NULL,
    trang_thai VARCHAR(20) NOT NULL DEFAULT 'WAITING',
    ngay_tao TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ngay_ket_thuc TIMESTAMPTZ NULL
);
CREATE INDEX IF NOT EXISTS IX_phienchat_trang_thai ON phienchat(trang_thai);
CREATE INDEX IF NOT EXISTS IX_phienchat_ngay_tao ON phienchat(ngay_tao);

CREATE TABLE IF NOT EXISTS tinnhan (
    matinnhan BIGSERIAL PRIMARY KEY,
    maphienchat VARCHAR(36) NOT NULL REFERENCES phienchat(maphienchat) ON DELETE CASCADE,
    nguoi_gui VARCHAR(20) NOT NULL,
    nguoi_gui_id VARCHAR(36) NULL,
    noi_dung TEXT NOT NULL,
    da_doc BOOLEAN NOT NULL DEFAULT FALSE,
    thoi_gian TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE INDEX IF NOT EXISTS IX_tinnhan_phienchat_thoigian ON tinnhan(maphienchat, thoi_gian);

INSERT INTO ThamSo (MATHAMSO, TENTHAMSO, GIATRI, DONVI) VALUES
    ('HAN_DOI_Y_NGAY', 'Hạn đổi trả đối với lý do đổi ý (nguyên seal)', 7, 'ngày')
ON CONFLICT (MATHAMSO) DO NOTHING;

CREATE OR REPLACE VIEW vw_TonKho AS
SELECT m.MAHANG, m.TENHANG, m.MANHOM, m.MALOAI, m.DONGIA,
       COALESCE(n.TONGNHAP, 0) AS TONGNHAP,
       COALESCE(b.TONGBAN, 0) AS TONGBAN,
       CASE 
         WHEN m.QUANLYIMEI = TRUE THEN COALESCE(im.TONKHO_IMEI, 0)
         ELSE COALESCE(n.TONGNHAP, 0) - COALESCE(b.TONGBAN, 0)
       END AS TONKHO
FROM MatHang m
LEFT JOIN (SELECT MAHANG, SUM(SLNHAP) AS TONGNHAP FROM ChiTietPhieuNhap GROUP BY MAHANG) n ON n.MAHANG = m.MAHANG
LEFT JOIN (SELECT MAHANG, SUM(SLBAN) AS TONGBAN FROM ChiTietHoaDon GROUP BY MAHANG) b ON b.MAHANG = m.MAHANG
LEFT JOIN (SELECT MAHANG, COUNT(*) AS TONKHO_IMEI FROM IMEI WHERE TRANGTHAI = 'con_hang' GROUP BY MAHANG) im ON im.MAHANG = m.MAHANG;

CREATE OR REPLACE VIEW vw_diemtichluy AS
SELECT 
    k.MAKH,
    k.HOTEN,
    k.SDT,
    k.EMAIL,
    COALESCE(s.TongChiTieu, 0) AS TongChiTieu,
    COALESCE(r.TongTienTra, 0) AS TongTienTra,
    GREATEST(0, FLOOR((COALESCE(s.TongChiTieu, 0) - COALESCE(r.TongTienTra, 0)) / NULLIF(p.GIATRI, 0))) AS DiemTichLuy
FROM KhachHang k
LEFT JOIN (SELECT h.MAKH, SUM(h.THANHTIEN) AS TongChiTieu
           FROM HoaDon h
           WHERE h.MAKH IS NOT NULL
           GROUP BY h.MAKH) s ON s.MAKH = k.MAKH
LEFT JOIN (SELECT h.MAKH, SUM(COALESCE(t.SOTIENHOAN, ct.SLBAN * ct.DONGIA_BAN)) AS TongTienTra
           FROM TraHang t
           JOIN HoaDon h ON h.SOHD = t.SOHD
           JOIN ChiTietHoaDon ct ON ct.SOHD = t.SOHD AND ct.MAIMEI = t.MAIMEI
           WHERE t.TRANGTHAI = 'hoan_tat' 
             AND t.HINHTHUCHOANTIEN IN ('tien_mat', 'chuyen_khoan') 
             AND h.MAKH IS NOT NULL
           GROUP BY h.MAKH) r ON r.MAKH = k.MAKH
LEFT JOIN LATERAL (SELECT GIATRI FROM ThamSo WHERE MATHAMSO = 'DIEM_MOI_VND') p ON TRUE;
";
            await context.Database.ExecuteSqlRawAsync(migrationSql);

            const string checkoutMigrationSql = @"
CREATE TABLE IF NOT EXISTS products (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS variants (
    id BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    sku VARCHAR(100) NOT NULL UNIQUE,
    color VARCHAR(50) NOT NULL,
    storage VARCHAR(50) NOT NULL,
    price BIGINT NOT NULL CHECK (price >= 0),
    stock INT NOT NULL DEFAULT 0 CHECK (stock >= 0),
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT CK_variants_stock CHECK (stock >= 0)
);

CREATE TABLE IF NOT EXISTS carts (
    id BIGSERIAL PRIMARY KEY,
    cart_token VARCHAR(64) UNIQUE NULL,
    user_id VARCHAR(50) NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS cart_items (
    id BIGSERIAL PRIMARY KEY,
    cart_id BIGINT NOT NULL REFERENCES carts(id) ON DELETE CASCADE,
    variant_id BIGINT NOT NULL REFERENCES variants(id) ON DELETE CASCADE,
    qty INT NOT NULL DEFAULT 1 CHECK (qty > 0),
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT UQ_cart_items_cart_variant UNIQUE (cart_id, variant_id)
);

CREATE TABLE IF NOT EXISTS orders (
    id BIGSERIAL PRIMARY KEY,
    order_code VARCHAR(32) NOT NULL UNIQUE,
    status VARCHAR(32) NOT NULL DEFAULT 'PENDING_PAYMENT',
    total BIGINT NOT NULL CHECK (total >= 0),
    expires_at TIMESTAMPTZ NOT NULL,
    idempotency_key VARCHAR(128) NOT NULL UNIQUE,
    recipient_name VARCHAR(100) NOT NULL,
    recipient_phone VARCHAR(20) NOT NULL,
    shipping_address VARCHAR(255) NOT NULL,
    payment_method VARCHAR(30) NOT NULL DEFAULT 'QR_PAYMENT',
    voucher_code VARCHAR(50) NULL,
    discount_amount BIGINT NOT NULL DEFAULT 0,
    shipping_fee BIGINT NOT NULL DEFAULT 0,
    user_id VARCHAR(50) NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE orders ADD COLUMN IF NOT EXISTS voucher_code VARCHAR(50) NULL;
ALTER TABLE orders ADD COLUMN IF NOT EXISTS discount_amount BIGINT NOT NULL DEFAULT 0;
ALTER TABLE orders ADD COLUMN IF NOT EXISTS shipping_fee BIGINT NOT NULL DEFAULT 0;

ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS address_hash VARCHAR(64) NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS auto_approve_at TIMESTAMPTZ NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS client_ip VARCHAR(45) NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS ghn_error TEXT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS ghn_order_code VARCHAR(50) NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS payment_status VARCHAR(30) NOT NULL DEFAULT 'unpaid';
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS risk_flag BOOLEAN NOT NULL DEFAULT FALSE;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS risk_reasons TEXT NULL;

ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS completed_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS qualified_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS max_completed_value NUMERIC(18,2) DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS failed_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS is_blacklisted BOOLEAN DEFAULT FALSE;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS updated_at TIMESTAMPTZ DEFAULT NOW();

CREATE TABLE IF NOT EXISTS customer_addresses (
    id SERIAL PRIMARY KEY,
    phone VARCHAR(15) NOT NULL,
    address_hash VARCHAR(64) NOT NULL,
    full_address VARCHAR(255) NOT NULL,
    last_delivered_at TIMESTAMPTZ DEFAULT NOW(),
    CONSTRAINT uq_customer_addresses UNIQUE (phone, address_hash)
);

CREATE TABLE IF NOT EXISTS order_settings (
    key VARCHAR(64) PRIMARY KEY,
    value TEXT NOT NULL,
    description TEXT NULL
);

INSERT INTO order_settings (key, value, description) VALUES
    ('auto_approve_delay_minutes', '30', 'Thời gian chờ tự động duyệt (phút)'),
    ('max_daily_orders_per_address', '3', 'Số đơn tối đa/ngày/địa chỉ'),
    ('max_order_amount_auto_approve', '50000000', 'Giá trị đơn tối đa cho phép tự duyệt (VNĐ)'),
    ('enable_auto_approve', 'true', 'Bật/tắt tính năng tự động duyệt')
ON CONFLICT (key) DO NOTHING;

CREATE TABLE IF NOT EXISTS order_items (
    id BIGSERIAL PRIMARY KEY,
    order_id BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    variant_id BIGINT NOT NULL REFERENCES variants(id),
    product_name_snapshot VARCHAR(255) NOT NULL,
    sku_snapshot VARCHAR(100) NOT NULL,
    price_snapshot BIGINT NOT NULL CHECK (price_snapshot >= 0),
    qty INT NOT NULL CHECK (qty > 0),
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS payments (
    id BIGSERIAL PRIMARY KEY,
    order_id BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    provider VARCHAR(50) NOT NULL,
    provider_txn_id VARCHAR(100) NOT NULL UNIQUE,
    amount BIGINT NOT NULL CHECK (amount >= 0),
    status VARCHAR(32) NOT NULL,
    raw_payload TEXT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS order_status_history (
    id BIGSERIAL PRIMARY KEY,
    order_id BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    from_status VARCHAR(32) NULL,
    to_status VARCHAR(32) NOT NULL,
    reason VARCHAR(255) NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS refund_requests (
    id BIGSERIAL PRIMARY KEY,
    order_id BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    payment_id BIGINT NULL REFERENCES payments(id),
    amount BIGINT NOT NULL CHECK (amount >= 0),
    reason VARCHAR(255) NOT NULL,
    status VARCHAR(32) NOT NULL DEFAULT 'PENDING',
    provider_refund_id VARCHAR(100) UNIQUE NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS IX_variants_product ON variants(product_id);
CREATE INDEX IF NOT EXISTS IX_orders_status ON orders(status);
CREATE INDEX IF NOT EXISTS IX_orders_expires_at ON orders(expires_at);
CREATE INDEX IF NOT EXISTS IX_payments_order ON payments(order_id);
CREATE INDEX IF NOT EXISTS IX_order_status_history_order ON order_status_history(order_id);
";
            await context.Database.ExecuteSqlRawAsync(checkoutMigrationSql);
            using var migration = typeof(DbSeeder).Assembly.GetManifestResourceStream("StoreConsistency.sql")!;
            using var reader = new StreamReader(migration);
            await context.Database.ExecuteSqlRawAsync(await reader.ReadToEndAsync());
            logger.LogInformation("Đã kiểm tra và đồng bộ schema module Giỏ hàng và Thanh toán thành công.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cảnh báo khi đồng bộ schema TraHang: {Message}", ex.Message);
        }
    }

    private static string? ResolveDatabaseSqlPath(params string[] relativeParts)
    {
        var baseRoots = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "database"),
            Path.Combine(Directory.GetCurrentDirectory(), "database"),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "database"),
            "/app/database"
        };
        foreach (var root in baseRoots)
        {
            var full = Path.GetFullPath(Path.Combine(new[] { root }.Concat(relativeParts).ToArray()));
            if (File.Exists(full)) return full;
        }
        return null;
    }

    private static async Task SeedFullCatalogAndStockIfEmptyAsync(AppDbContext context, ILogger logger)
    {
        var previousTimeout = context.Database.GetCommandTimeout();
        try
        {
            context.Database.SetCommandTimeout(180);

            // Đảm bảo cột gia_tri đủ rộng cho thông số kỹ thuật dài trong 05-seed-products.sql
            await context.Database.ExecuteSqlRawAsync("ALTER TABLE chitietthuoctinh ALTER COLUMN gia_tri TYPE TEXT;");

            var productCount = await context.MatHangs.CountAsync();
            if (productCount < 100)
            {
                var productsSqlPath = ResolveDatabaseSqlPath("initdb", "05-seed-products.sql");
                if (productsSqlPath != null)
                {
                    logger.LogInformation("Đang tự động nạp 3.547 sản phẩm từ {Path}...", productsSqlPath);
                    var sql = await File.ReadAllTextAsync(productsSqlPath);
                    await context.Database.ExecuteSqlRawAsync(sql);
                    logger.LogInformation("Đã nạp xong danh mục 3.547 sản phẩm.");
                }
            }

            var hasFullStockBatch = await context.PhieuNhaps.AnyAsync(p => p.SoPN == "PN_TONKHO_ALL");
            if (!hasFullStockBatch)
            {
                var stockSqlPath = ResolveDatabaseSqlPath("seed_full_stock.sql");
                if (stockSqlPath != null)
                {
                    await context.Database.ExecuteSqlRawAsync("""
                        INSERT INTO mathang (mahang, tenhang, dongia, dvt, manhom, maloai, quanlyimei, thoihanbaohanh)
                        VALUES
                          ('REC_DT02', 'iPhone 15 Pro Max 256GB (Thu cũ)', 26990000, 'Cái', 'DIENTHOAI', 'APPLE', true, 12),
                          ('S24256', 'Samsung Galaxy S24 256GB', 22990000, 'Cái', 'DIENTHOAI', 'SAMSUNG', true, 12),
                          ('XM14256', 'Xiaomi 14 256GB', 19990000, 'Cái', 'DIENTHOAI', 'XIAOMI', true, 12),
                          ('SAC_ANKER', 'Củ sạc nhanh Anker 20W', 350000, 'Cái', 'PHUKIEN', 'KHAC', false, 12)
                        ON CONFLICT (mahang) DO NOTHING;
                        """);
                    logger.LogInformation("Đang tự động nạp tồn kho & IMEI đầy đủ từ {Path}...", stockSqlPath);
                    var sql = await File.ReadAllTextAsync(stockSqlPath);
                    await context.Database.ExecuteSqlRawAsync(sql);
                    logger.LogInformation("Đã nạp xong tồn kho & IMEI đầy đủ.");
                }
            }

            // Bổ sung tồn kho & IMEI cho các model bị thiếu trong seed_full_stock.sql (ví dụ MH101..MH112, MH106)
            var hasGapStock = await context.PhieuNhaps.AnyAsync(p => p.SoPN == "PN_TONKHO_GAP");
            if (!hasGapStock)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    INSERT INTO PhieuNhap (SOPN, NGAYNHAP, MANCC, MANV)
                    VALUES ('PN_TONKHO_GAP', NOW() - INTERVAL '5 days', 'NCC_AP', 'NV001')
                    ON CONFLICT (SOPN) DO NOTHING;

                    INSERT INTO ChiTietPhieuNhap (SOPN, MAHANG, SLNHAP, DONGIA_NHAP)
                    SELECT 'PN_TONKHO_GAP', m.mahang, 15, GREATEST(1000, ROUND(m.dongia * 0.8))
                    FROM mathang m
                    WHERE NOT EXISTS (SELECT 1 FROM chitietphieunhap c WHERE c.mahang = m.mahang)
                    ON CONFLICT (SOPN, MAHANG) DO NOTHING;

                    INSERT INTO imei (maimei, mahang, sopn, soimei, trangthai)
                    SELECT 'IMGAP_' || m.mahang || '_' || LPAD(gs::text, 2, '0'),
                           m.mahang,
                           'PN_TONKHO_GAP',
                           '3599' || LPAD(abs(hashtext(m.mahang))::text, 9, '0') || LPAD(gs::text, 2, '0'),
                           'con_hang'
                    FROM mathang m
                    CROSS JOIN generate_series(1, 15) AS gs
                    WHERE m.quanlyimei = true
                      AND NOT EXISTS (SELECT 1 FROM imei i WHERE i.mahang = m.mahang AND i.trangthai = 'con_hang')
                    ON CONFLICT (maimei) DO NOTHING;
                    """);
                logger.LogInformation("Đã bổ sung tồn kho & IMEI cho các model còn thiếu.");
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cảnh báo khi tự động nạp catalog/tồn kho: {Message}", ex.Message);
        }
        finally
        {
            context.Database.SetCommandTimeout(previousTimeout);
        }
    }

    public static async Task SeedAsync(AppDbContext context, ILogger logger, bool seedSampleData)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogWarning("Không thể kết nối đến Database để seed dữ liệu. Vui lòng kiểm tra lại PostgreSQL container hoặc ConnectionString.");
                return;
            }

            // 0. Đảm bảo schema bảng TraHang, TraHangLichSu, IMEI constraint và Views luôn được nâng cấp đầy đủ
            await EnsureSchemaUpgradedAsync(context, logger);

            // 0b. Tự động nạp 3.549 sản phẩm (05-seed-products.sql) & tồn kho đầy đủ (seed_full_stock.sql) nếu DB chưa có
            await SeedFullCatalogAndStockIfEmptyAsync(context, logger);

            // 1. Seed ThamSo (kiểm tra độc lập)
            if (!await context.ThamSos.AnyAsync())
            {
                logger.LogInformation("Seeding ThamSo...");
                context.ThamSos.AddRange(
                    new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "Thuế VAT đã gồm trong giá bán", GiaTri = 10, DonVi = "%" },
                    new ThamSo { MaThamSo = "DIEM_MOI_VND", TenThamSo = "Số tiền chi tiêu để được 1 điểm", GiaTri = 100000, DonVi = "VND" },
                    new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả tính từ ngày lập hóa đơn", GiaTri = 30, DonVi = "ngày" },
                    new ThamSo { MaThamSo = "HAN_DOI_Y_NGAY", TenThamSo = "Hạn đổi trả đối với lý do đổi ý (nguyên seal)", GiaTri = 7, DonVi = "ngày" }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed ThamSo thành công.");
            }

            // 2. Seed PhanQuyen (kiểm tra độc lập)
            if (!await context.PhanQuyens.AnyAsync())
            {
                logger.LogInformation("Seeding PhanQuyen...");
                context.PhanQuyens.AddRange(
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên", MoTa = "Toàn quyền quản trị hệ thống" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Manager, TenQuyen = "Quản lý cửa hàng", MoTa = "Quản lý kinh doanh và nhân viên" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Sales, TenQuyen = "Nhân viên bán hàng", MoTa = "Bán hàng tại quầy và xử lý đơn online" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Cashier, TenQuyen = "Thu ngân", MoTa = "Lập hóa đơn và thanh toán" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Stock, TenQuyen = "Nhân viên kho", MoTa = "Quản lý kho và xuất nhập hàng" },
                    new PhanQuyen { MaQuyen = AppConstants.Roles.Tech, TenQuyen = "Kỹ thuật viên", MoTa = "Bảo hành và sửa chữa thiết bị" }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seed PhanQuyen thành công.");
            }

            // 3. Seed NhanVien (đảm bảo đầy đủ 6 nhân viên tương ứng 6 chức vụ)
            var defaultStaff = new List<NhanVien>
            {
                new NhanVien { MaNV = "NV001", HoTen = "Nguyễn Văn Quản Trị", ChucVu = "Quản lý", Sdt = "0901234567", Email = "admin@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV002", HoTen = "Trần Thị Thu Ngân", ChucVu = "Thu ngân", Sdt = "0912345678", Email = "thungan@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV003", HoTen = "Lê Bán Hàng", ChucVu = "Bán hàng", Sdt = "0923456789", Email = "banhang@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV004", HoTen = "Phạm Thủ Kho", ChucVu = "Kho", Sdt = "0934567890", Email = "kho@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV005", HoTen = "Hoàng Kỹ Thuật", ChucVu = "Kỹ thuật & Bảo hành", Sdt = "0945678901", Email = "kythuat@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam },
                new NhanVien { MaNV = "NV006", HoTen = "Đỗ Cửa Hàng Trưởng", ChucVu = "Quản lý", Sdt = "0956789012", Email = "quanly@cellphones.vn", NgayVaoLam = DateTime.UtcNow, TrangThai = AppConstants.EmployeeStatus.DangLam }
            };

            foreach (var staff in defaultStaff)
            {
                if (!await context.NhanViens.AnyAsync(n => n.MaNV == staff.MaNV))
                {
                    context.NhanViens.Add(staff);
                }
            }
            await context.SaveChangesAsync();

            // 4. Seed TaiKhoan (đảm bảo đủ 6 tài khoản mẫu cho cả 6 vai trò trong hệ thống)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);

            var defaultAccounts = new List<TaiKhoan>
            {
                new TaiKhoan { UserName = "admin", PasswordHash = passwordHash, MaNV = "NV001", MaQuyen = AppConstants.Roles.Admin, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "quanly", PasswordHash = passwordHash, MaNV = "NV006", MaQuyen = AppConstants.Roles.Manager, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "thungan", PasswordHash = passwordHash, MaNV = "NV002", MaQuyen = AppConstants.Roles.Cashier, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "banhang", PasswordHash = passwordHash, MaNV = "NV003", MaQuyen = AppConstants.Roles.Sales, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "kho", PasswordHash = passwordHash, MaNV = "NV004", MaQuyen = AppConstants.Roles.Stock, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow },
                new TaiKhoan { UserName = "kythuat", PasswordHash = passwordHash, MaNV = "NV005", MaQuyen = AppConstants.Roles.Tech, TrangThai = AppConstants.AccountStatus.HoatDong, NgayTao = DateTime.UtcNow }
            };

            foreach (var acc in defaultAccounts)
            {
                if (!await context.TaiKhoans.AnyAsync(t => t.UserName == acc.UserName))
                {
                    context.TaiKhoans.Add(acc);
                }
            }
            await context.SaveChangesAsync();

            // Chỉ cập nhật hash hợp lệ cho 6 tài khoản seed mặc định
            var seedUsernames = new[] { "admin", "quanly", "thungan", "banhang", "kho", "kythuat" };
            var seedAccounts = await context.TaiKhoans
                .Where(t => seedUsernames.Contains(t.UserName))
                .ToListAsync();

            string validHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            bool updated = false;
            foreach (var acc in seedAccounts)
            {
                try
                {
                    if (!BCrypt.Net.BCrypt.Verify("admin123", acc.PasswordHash))
                    {
                        acc.PasswordHash = validHash;
                        updated = true;
                    }
                }
                catch
                {
                    acc.PasswordHash = validHash;
                    updated = true;
                }
            }
            if (updated)
            {
                await context.SaveChangesAsync();
                logger.LogInformation("Đã cập nhật mật khẩu mặc định 'admin123' cho các tài khoản seed.");
            }

            // 5. Seed PhieuNhap & IMEI mẫu nếu kho trống để kiểm thử bán hàng POS
            if (seedSampleData && !await context.PhieuNhaps.AnyAsync())
            {
                logger.LogInformation("Seeding PhieuNhap & IMEI mẫu...");
                var now = DateTime.UtcNow;

                var pn1 = new PhieuNhap
                {
                    SoPN = "PN261100001",
                    NgayNhap = now.AddDays(-5),
                    MaNCC = "NCC_AP",
                    MaNV = "NV004"
                };

                var pn2 = new PhieuNhap
                {
                    SoPN = "PN261100002",
                    NgayNhap = now.AddDays(-3),
                    MaNCC = "NCC_SS",
                    MaNV = "NV004"
                };

                context.PhieuNhaps.AddRange(pn1, pn2);
                await context.SaveChangesAsync();

                context.ChiTietPhieuNhaps.AddRange(
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "IP15PM256", SlNhap = 5, DonGiaNhap = 26000000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "IP15P128", SlNhap = 5, DonGiaNhap = 22000000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100001", MaHang = "AP_PRO2", SlNhap = 10, DonGiaNhap = 4500000m },
                    new ChiTietPhieuNhap { SoPN = "PN261100002", MaHang = "S24U512", SlNhap = 5, DonGiaNhap = 28000000m }
                );
                await context.SaveChangesAsync();

                var sampleImeis = new List<IMEI>
                {
                    // iPhone 15 Pro Max 256GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_IP15PM_01", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_02", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_03", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_04", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15PM_05", MaHang = "IP15PM256", SoPN = "PN261100001", SoIMEI = "358901234567895", TrangThai = "con_hang" },

                    // iPhone 15 Pro 128GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_IP15P_01", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_02", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_03", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_04", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_IP15P_05", MaHang = "IP15P128", SoPN = "PN261100001", SoIMEI = "358911234567895", TrangThai = "con_hang" },

                    // Samsung Galaxy S24 Ultra 512GB (5 máy)
                    new IMEI { MaIMEI = "IMEI_S24U_01", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567891", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_02", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567892", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_03", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567893", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_04", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567894", TrangThai = "con_hang" },
                    new IMEI { MaIMEI = "IMEI_S24U_05", MaHang = "S24U512", SoPN = "PN261100002", SoIMEI = "359901234567895", TrangThai = "con_hang" }
                };

                context.IMEIs.AddRange(sampleImeis);
                await context.SaveChangesAsync();
            }

            // 6. Seed Hóa đơn mẫu có gắn IMEI đã bán và Phiếu đổi trả mẫu (nếu chưa có) để phục vụ kiểm thử Đổi trả & Bảo hành
            if (seedSampleData && !await context.HoaDons.AnyAsync(h => h.SoHD == "HD261100001"))
            {
                var imeiSold1 = await context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == "IMEI_IP15PM_05");
                var imeiSold2 = await context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == "IMEI_IP15P_05");
                if (imeiSold1 != null && imeiSold2 != null && imeiSold1.TrangThai == "con_hang" && imeiSold2.TrangThai == "con_hang")
                {
                    var saleDate = DateTime.UtcNow.AddDays(-2);
                    var kh = await context.KhachHangs.Select(k => k.MaKH).FirstOrDefaultAsync();

                    var hdSample = new HoaDon
                    {
                        SoHD = "HD261100001",
                        NgayLap = saleDate,
                        MaKH = kh,
                        MaNV = "NV002",
                        KenhBan = "tai_quay",
                        TongTien = 54480000m,
                        TienGiamGia = 0m,
                        PhuongThucThanhToan = "chuyen_khoan"
                    };
                    context.HoaDons.Add(hdSample);
                    await context.SaveChangesAsync();

                    context.ChiTietHoaDons.AddRange(
                        new ChiTietHoaDon { SoHD = "HD261100001", MaHang = "IP15PM256", MaIMEI = "IMEI_IP15PM_05", SlBan = 1, DonGiaBan = 29490000m },
                        new ChiTietHoaDon { SoHD = "HD261100001", MaHang = "IP15P128", MaIMEI = "IMEI_IP15P_05", SlBan = 1, DonGiaBan = 24990000m }
                    );
                    await context.SaveChangesAsync();

                    imeiSold1.TrangThai = "da_ban";
                    imeiSold2.TrangThai = "cho_duyet";
                    await context.SaveChangesAsync();

                    if (!await context.BaoHanhs.AnyAsync(b => b.MaIMEI == "IMEI_IP15PM_05"))
                    {
                        context.BaoHanhs.AddRange(
                            new BaoHanh { MaBH = "BH261100001", SoHD = "HD261100001", MaIMEI = "IMEI_IP15PM_05", NgayBD = saleDate, ThoiHanThang = 12, NgayKT = saleDate.AddMonths(12), DieuKien = "Bảo hành tiêu chuẩn 12 tháng" },
                            new BaoHanh { MaBH = "BH261100002", SoHD = "HD261100001", MaIMEI = "IMEI_IP15P_05", NgayBD = saleDate, ThoiHanThang = 12, NgayKT = saleDate.AddMonths(12), DieuKien = "Bảo hành tiêu chuẩn 12 tháng" }
                        );
                        await context.SaveChangesAsync();
                    }

                    if (!await context.TraHangs.AnyAsync())
                    {
                        var trSample = new TraHang
                        {
                            MaTR = "TR261100001",
                            SoHD = "HD261100001",
                            MaIMEI = "IMEI_IP15P_05",
                            LoaiYeuCau = "loi_ky_thuat",
                            HinhThucHoanTien = "doi_hang",
                            LyDo = "Màn hình xuất hiện sọc xanh nhẹ khi để độ sáng thấp",
                            TrangThai = "tiep_nhan",
                            NgayTR = DateTime.UtcNow.AddHours(-3),
                            MaNV = "NV003",
                            ChecklistNgoaiQuan = "Máy đẹp 99%, không trầy xước, đầy đủ hộp và cáp sạc zin"
                        };
                        context.TraHangs.Add(trSample);
                        context.Set<TraHangLichSu>().Add(new TraHangLichSu
                        {
                            MaTR = "TR261100001",
                            TrangThaiCu = null,
                            TrangThaiMoi = "tiep_nhan",
                            MaNV = "NV003",
                            ThoiDiem = DateTime.UtcNow.AddHours(-3),
                            GhiChu = "Tiếp nhận hồ sơ đổi trả tại quầy"
                        });
                        await context.SaveChangesAsync();
                    }

                }
            }

            if (!await context.Products.AnyAsync())
            {
                var p1 = new Product { Name = "iPhone 16 Pro Max", Description = "Siêu phẩm Apple 2026 với chip A18 Pro và viền titan sa mạc" };
                var p2 = new Product { Name = "Samsung Galaxy S24 Ultra", Description = "Flagship AI đỉnh cao từ Samsung với bút S-Pen và Snapdragon 8 Gen 3" };
                var p3 = new Product { Name = "Xiaomi 14 Ultra", Description = "Chuyên gia nhiếp ảnh Leica thế hệ mới với cảm biến 1 inch" };

                context.Products.AddRange(p1, p2, p3);
                await context.SaveChangesAsync();

                context.ProductVariants.AddRange(
                    new ProductVariant { ProductId = p1.Id, Sku = "IP16PM-DESERT-256", Color = "Desert Titanium", Storage = "256GB", Price = 34990000, Stock = 10 },
                    new ProductVariant { ProductId = p1.Id, Sku = "IP16PM-NATURAL-512", Color = "Natural Titanium", Storage = "512GB", Price = 40990000, Stock = 5 },
                    new ProductVariant { ProductId = p1.Id, Sku = "IP16PM-BLACK-1TB", Color = "Black Titanium", Storage = "1TB", Price = 46990000, Stock = 3 },
                    new ProductVariant { ProductId = p2.Id, Sku = "S24U-GRAY-256", Color = "Titanium Gray", Storage = "256GB", Price = 29990000, Stock = 8 },
                    new ProductVariant { ProductId = p2.Id, Sku = "S24U-BLACK-512", Color = "Titanium Black", Storage = "512GB", Price = 33990000, Stock = 4 },
                    new ProductVariant { ProductId = p3.Id, Sku = "XM14U-BLACK-512", Color = "Black", Storage = "512GB", Price = 28990000, Stock = 6 },
                    new ProductVariant { ProductId = p3.Id, Sku = "XM14U-WHITE-512", Color = "White", Storage = "512GB", Price = 28990000, Stock = 2 }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Đã seed thành công 3 sản phẩm và các variants cho module Giỏ hàng & Thanh toán.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình seed dữ liệu.");
        }
    }
}
