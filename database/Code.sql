-- ====================================================================
-- SCRIPT LOGIC VÀ NGHIỆP VỤ (POSTGRESQL DIALECT)
-- Hệ thống Quản lý Cửa hàng Điện thoại CellphoneS
-- ====================================================================

-- 1. TẠO CÁC SEQUENCE SINH MÃ TỰ ĐỘNG
CREATE SEQUENCE IF NOT EXISTS seq_PhieuNhap AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_HoaDon AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_BaoHanh AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TraHang AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_TiepNhan AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_DonHang AS BIGINT START WITH 1 INCREMENT BY 1;

-- 2. DỮ LIỆU THAM SỐ MẶC ĐỊNH
INSERT INTO ThamSo (MATHAMSO, TENTHAMSO, GIATRI, DONVI) VALUES
    ('VAT_PHANTRAM', 'Thuế VAT đã gồm trong giá bán', 10, '%'),
    ('DIEM_MOI_VND', 'Số tiền chi tiêu để được 1 điểm', 100000, 'VND'),
    ('HAN_DOITRA_NGAY', 'Hạn đổi trả tính từ ngày lập hóa đơn', 30, 'ngày')
ON CONFLICT (MATHAMSO) DO NOTHING;

-- 3. CÁC VIEW BÁO CÁO & THỐNG KÊ
-- View 3.1: Tồn kho hiện tại
CREATE OR REPLACE VIEW vw_TonKho AS
SELECT m.MAHANG, m.TENHANG, m.MANHOM, m.MALOAI,
       COALESCE(n.TONGNHAP, 0) AS TONGNHAP,
       COALESCE(b.TONGBAN, 0) AS TONGBAN,
       COALESCE(n.TONGNHAP, 0) - COALESCE(b.TONGBAN, 0) AS TONKHO
FROM MatHang m
LEFT JOIN (SELECT MAHANG, SUM(SLNHAP) AS TONGNHAP FROM ChiTietPhieuNhap GROUP BY MAHANG) n ON n.MAHANG = m.MAHANG
LEFT JOIN (SELECT MAHANG, SUM(SLBAN) AS TONGBAN FROM ChiTietHoaDon GROUP BY MAHANG) b ON b.MAHANG = m.MAHANG;

-- View 3.2: Danh sách IMEI còn hàng
CREATE OR REPLACE VIEW vw_IMEIConHang AS
SELECT i.MAIMEI, i.SOIMEI, i.MAHANG, m.TENHANG, i.SOPN, pn.NGAYNHAP
FROM IMEI i
JOIN MatHang m ON m.MAHANG = i.MAHANG
JOIN PhieuNhap pn ON pn.SOPN = i.SOPN
WHERE i.TRANGTHAI = 'con_hang';

-- View 3.3: Chi tiết hóa đơn và người mua/bán
CREATE OR REPLACE VIEW vw_HoaDonChiTiet AS
SELECT h.SOHD, h.NGAYLAP, h.KENHBAN, h.MAKH, k.HOTEN AS TENKH, h.MANV, nv.HOTEN AS TENNV,
       h.MAKM, km.TENKM, km.PHANTRAMGIAM,
       h.TONGTIEN, h.TIENGIAMGIA, h.THANHTIEN,
       ROUND(h.THANHTIEN - h.THANHTIEN / (1 + COALESCE(v.GIATRI, 0) / 100), 2) AS TIENVAT_DA_GOM,
       h.PHUONGTHUCTHANHTOAN
FROM HoaDon h
JOIN NhanVien nv ON nv.MANV = h.MANV
LEFT JOIN KhachHang k ON k.MAKH = h.MAKH
LEFT JOIN KhuyenMai km ON km.MAKM = h.MAKM
LEFT JOIN LATERAL (SELECT GIATRI FROM ThamSo WHERE MATHAMSO = 'VAT_PHANTRAM') v ON TRUE;

-- View 3.4: Chi tiết từng dòng bán hàng
CREATE OR REPLACE VIEW vw_ChiTietBanHang AS
SELECT ct.SOHD, h.NGAYLAP, h.KENHBAN, ct.MAHANG, m.TENHANG, m.MANHOM, ng.TENNHOM, m.MALOAI, lh.TENLOAI,
       ct.MAIMEI, i.SOIMEI, ct.SLBAN, ct.DONGIA_BAN, ct.SLBAN * ct.DONGIA_BAN AS THANHTIEN_DONG
FROM ChiTietHoaDon ct
JOIN HoaDon h ON h.SOHD = ct.SOHD
JOIN MatHang m ON m.MAHANG = ct.MAHANG
JOIN NhomHang ng ON ng.MANHOM = m.MANHOM
JOIN LoaiHang lh ON lh.MALOAI = m.MALOAI
LEFT JOIN IMEI i ON i.MAIMEI = ct.MAIMEI;

-- View 3.5: Doanh thu theo tháng
CREATE OR REPLACE VIEW vw_DoanhThuThang AS
SELECT EXTRACT(YEAR FROM NGAYLAP)::INT AS NAM, EXTRACT(MONTH FROM NGAYLAP)::INT AS THANG, KENHBAN,
       COUNT(*) AS SOHOADON,
       SUM(TONGTIEN) AS TONGTIEN,
       SUM(TIENGIAMGIA) AS TIENGIAMGIA,
       SUM(THANHTIEN) AS DOANHTHU
FROM HoaDon
GROUP BY EXTRACT(YEAR FROM NGAYLAP), EXTRACT(MONTH FROM NGAYLAP), KENHBAN;

-- View 3.6: Trạng thái khuyến mãi
CREATE OR REPLACE VIEW vw_KhuyenMaiTrangThai AS
SELECT MAKM, TENKM, PHANTRAMGIAM, NGAYBD, NGAYKT, TAMDUNG,
       CASE WHEN TAMDUNG = TRUE THEN 'tam_dung'
            WHEN CURRENT_DATE < CAST(NGAYBD AS DATE) THEN 'chua_ap_dung'
            WHEN CURRENT_DATE > CAST(NGAYKT AS DATE) THEN 'het_han'
            ELSE 'dang_ap_dung' END AS TRANGTHAI
FROM KhuyenMai;

-- View 3.7: Trạng thái bảo hành
CREATE OR REPLACE VIEW vw_BaoHanhTrangThai AS
SELECT bh.MABH, bh.SOHD, bh.MAIMEI, i.SOIMEI, i.MAHANG, m.TENHANG, h.MAKH, k.HOTEN AS TENKH,
       bh.NGAYBD, bh.THOIHANTHANG, bh.NGAYKT, bh.DIEUKIEN,
       CASE WHEN EXISTS (SELECT 1 FROM PhieuTiepNhanBaoHanh tn
                         WHERE tn.MABH = bh.MABH AND tn.TRANGTHAI IN ('tiep_nhan', 'dang_sua', 'cho_linh_kien'))
                 THEN 'dang_sua'
            WHEN CURRENT_TIMESTAMP > bh.NGAYKT THEN 'het_han'
            ELSE 'con_han' END AS TRANGTHAI,
       DATE_PART('day', bh.NGAYKT - CURRENT_TIMESTAMP)::INT AS SONGAYCONLAI
FROM BaoHanh bh
JOIN IMEI i ON i.MAIMEI = bh.MAIMEI
JOIN MatHang m ON m.MAHANG = i.MAHANG
JOIN HoaDon h ON h.SOHD = bh.SOHD
LEFT JOIN KhachHang k ON k.MAKH = h.MAKH;

-- View 3.8: Khách hàng thân thiết và điểm tích lũy
CREATE OR REPLACE VIEW vw_KhachHangThanThiet AS
SELECT k.MAKH, k.HOTEN, k.SDT,
       COALESCE(c.TONGCHITIEU, 0) AS TONGCHITIEU,
       COALESCE(r.TONGHOAN, 0) AS TONGHOAN,
       CASE WHEN p.GIATRI IS NULL OR p.GIATRI <= 0 OR COALESCE(c.TONGCHITIEU, 0) - COALESCE(r.TONGHOAN, 0) <= 0 THEN 0
            ELSE FLOOR((COALESCE(c.TONGCHITIEU, 0) - COALESCE(r.TONGHOAN, 0)) / p.GIATRI)::INT END AS DIEMTICHLUY
FROM KhachHang k
LEFT JOIN (SELECT MAKH, SUM(THANHTIEN) AS TONGCHITIEU
           FROM HoaDon
           WHERE MAKH IS NOT NULL
           GROUP BY MAKH) c ON c.MAKH = k.MAKH
LEFT JOIN (SELECT h.MAKH, SUM(ct.DONGIA_BAN) AS TONGHOAN
           FROM TraHang t
           JOIN HoaDon h ON h.SOHD = t.SOHD
           JOIN ChiTietHoaDon ct ON ct.SOHD = t.SOHD AND ct.MAIMEI = t.MAIMEI
           WHERE t.HINHTHUCHOANTIEN IN ('tien_mat', 'chuyen_khoan') AND h.MAKH IS NOT NULL
           GROUP BY h.MAKH) r ON r.MAKH = k.MAKH
LEFT JOIN LATERAL (SELECT GIATRI FROM ThamSo WHERE MATHAMSO = 'DIEM_MOI_VND') p ON TRUE;

-- 4. TRIGGERS
-- Trigger 4.1: Kiểm tra tính hợp lệ khi thêm IMEI
CREATE OR REPLACE FUNCTION fn_trg_imei_kiemtra()
RETURNS TRIGGER AS $$
DECLARE
    v_quanly BOOLEAN;
    v_slnhap INT;
    v_cur_count INT;
BEGIN
    SELECT QUANLYIMEI INTO v_quanly FROM MatHang WHERE MAHANG = NEW.MAHANG;
    IF v_quanly = FALSE THEN
        RAISE EXCEPTION 'Mặt hàng % không quản lý theo IMEI.', NEW.MAHANG;
    END IF;

    SELECT SLNHAP INTO v_slnhap FROM ChiTietPhieuNhap WHERE SOPN = NEW.SOPN AND MAHANG = NEW.MAHANG;
    SELECT COUNT(*) INTO v_cur_count FROM IMEI WHERE SOPN = NEW.SOPN AND MAHANG = NEW.MAHANG;
    IF v_cur_count > v_slnhap THEN
        RAISE EXCEPTION 'Số lượng IMEI vượt quá số lượng đã nhập trong phiếu nhập.';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_imei_kiemtra ON IMEI;
CREATE TRIGGER trg_imei_kiemtra
AFTER INSERT OR UPDATE ON IMEI
FOR EACH ROW EXECUTE FUNCTION fn_trg_imei_kiemtra();

-- Trigger 4.2: Tự động tính tổng tiền hóa đơn khi thay đổi ChiTietHoaDon
CREATE OR REPLACE FUNCTION fn_trg_chitiethoadon_tongtien()
RETURNS TRIGGER AS $$
DECLARE
    v_sohd VARCHAR(15);
    v_tongtien DECIMAL(18,2);
    v_phantramgiam DECIMAL(5,2);
BEGIN
    v_sohd := COALESCE(NEW.SOHD, OLD.SOHD);

    SELECT COALESCE(SUM(SLBAN * DONGIA_BAN), 0) INTO v_tongtien
    FROM ChiTietHoaDon
    WHERE SOHD = v_sohd;

    SELECT COALESCE(km.PHANTRAMGIAM, 0) INTO v_phantramgiam
    FROM HoaDon h
    LEFT JOIN KhuyenMai km ON km.MAKM = h.MAKM
    WHERE h.SOHD = v_sohd;

    UPDATE HoaDon
    SET TONGTIEN = v_tongtien,
        TIENGIAMGIA = ROUND(v_tongtien * v_phantramgiam / 100, 2)
    WHERE SOHD = v_sohd;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_chitiethoadon_tongtien ON ChiTietHoaDon;
CREATE TRIGGER trg_chitiethoadon_tongtien
AFTER INSERT OR UPDATE OR DELETE ON ChiTietHoaDon
FOR EACH ROW EXECUTE FUNCTION fn_trg_chitiethoadon_tongtien();

-- 5. STORED PROCEDURES NGHIỆP VỤ
-- Procedure: Thống kê doanh thu theo khoảng thời gian
CREATE OR REPLACE PROCEDURE sp_ThongKeDoanhThu(
    IN p_tu_ngay TIMESTAMPTZ,
    IN p_den_ngay TIMESTAMPTZ,
    OUT p_tong_doanh_thu DECIMAL(18,2),
    OUT p_so_don_hang INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    SELECT COALESCE(SUM(THANHTIEN), 0), COUNT(*)
    INTO p_tong_doanh_thu, p_so_don_hang
    FROM HoaDon
    WHERE NGAYLAP >= p_tu_ngay AND NGAYLAP <= p_den_ngay;
END;
$$;
