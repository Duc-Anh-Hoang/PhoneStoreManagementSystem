-- ==================================================================
-- Auto-generated seed data crawled from HoangHa Mobile (1580 items)
-- Generated at: 2026-11-05T08:08:06.222Z
-- ==================================================================

-- 1. Insert NhomHang
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'DIENTHOAI')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'DIENTHOAI', N'Điện thoại');
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'TABLET')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'TABLET', N'Máy tính bảng');
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'AMTHANH')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'AMTHANH', N'Âm thanh');
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'PHUKIEN')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'PHUKIEN', N'Phụ kiện');
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'DONGHO')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'DONGHO', N'Đồng hồ');
IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'LAPTOP')
    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'LAPTOP', N'Laptop & Màn hình');

-- 2. Insert LoaiHang
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'APPLE')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'APPLE', N'Apple', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'NUBIA')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'NUBIA', N'Nubia', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'XIAOMI')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'XIAOMI', N'Xiaomi', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'SAMSUNG')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'SAMSUNG', N'Samsung', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'HONOR')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'HONOR', N'Honor', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'KHAC')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'KHAC', N'Chính hãng', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'OPPO')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'OPPO', N'OPPO', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'MASSTEL')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'MASSTEL', N'Masstel', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'VIVO')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'VIVO', N'Vivo', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'NOKIA')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'NOKIA', N'Nokia', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'TECNO')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'TECNO', N'Tecno', N'DIENTHOAI');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'HP')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'HP', N'HP', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'LENOVO')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'LENOVO', N'Lenovo', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'ASUS')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'ASUS', N'Asus', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'DELL')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'DELL', N'Dell', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'ACER')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'ACER', N'Acer', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'MSI')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'MSI', N'MSI', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'VIEWSONIC')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'VIEWSONIC', N'ViewSonic', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'LG')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'LG', N'LG', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'LOGITECH')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'LOGITECH', N'Logitech', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'UGREEN')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'UGREEN', N'Ugreen', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'BASEUS')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'BASEUS', N'Baseus', N'LAPTOP');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'HUAWEI')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'HUAWEI', N'Huawei', N'TABLET');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'GARMIN')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'GARMIN', N'Garmin', N'DONGHO');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'AMAZFIT')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'AMAZFIT', N'Amazfit', N'DONGHO');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'HARMAN')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'HARMAN', N'Harman Kardon', N'AMTHANH');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'JBL')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'JBL', N'JBL', N'AMTHANH');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'SONY')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'SONY', N'Sony', N'AMTHANH');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'MARSHALL')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'MARSHALL', N'Marshall', N'AMTHANH');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'ANKER')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'ANKER', N'Anker', N'AMTHANH');
IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'REALME')
    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'REALME', N'Realme', N'AMTHANH');

-- 3. Insert MatHang
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH101')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH101', N'iPhone 18 Pro Max 256GB', N'Apple', 41490000, 35266500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH102')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH102', N'iPhone 18 Pro 256GB', N'Apple', 38490000, 32716500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-glacier-pdp-images-vi-vn-021533622.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH103')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH103', N'iPhone Duo 256GB', N'Apple', 64990000, 55241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphon-duo-030228624.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH104')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH104', N'Nubia Air 5G 8GB/256GB', N'Nubia', 5490000, 4666500, 50, N'Chiếc', N'NUBIA', N'https://i.hoanghamobile.vn/Uploads/2026/07/07/back-gold1-171113527.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH105')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH105', N'Xiaomi Redmi 17 4G 6GB/256GB', N'Xiaomi', 5990000, 5091500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/01/q15a-green-back1-091121733.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH106')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH106', N'Xiaomi Redmi Note 17 4GB/128GB', N'Xiaomi', 5790000, 4921500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/17/p19-purple-back11-155153642.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH107')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH107', N'Xiaomi POCO C95 Pro 6GB/128GB', N'Xiaomi', 5990000, 5091500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/09/30/q15ap-green-front1-170835651.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH108')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH108', N'Samsung Galaxy A08 4GB/64GB', N'Samsung', 4490000, 3816500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/a08-silver-135259154.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH109')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH109', N'Samsung Galaxy A18 6GB/128GB', N'Samsung', 6690000, 5686500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/05/a18-white-151324306.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH110')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH110', N'Samsung Galaxy A27 5G - 6GB/128GB', N'Samsung', 7990000, 6791500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/06/24/samsung-a27-xanh-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH111')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH111', N'Samsung Galaxy A57 5G - 8GB/128GB', N'Samsung', 10990000, 9341500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/03/17/a57-violet-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH112')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH112', N'Samsung Galaxy S25 Ultra - 12GB/256GB', N'Samsung', 25790000, 21921500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH113')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH113', N'Samsung Galaxy S26 Ultra - 12GB/256GB', N'Samsung', 30890000, 26256500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH114')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH114', N'Samsung Galaxy Z Fold8 5G 12GB/256GB', N'Samsung', 39990000, 33991500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/samsung-galaxy-z-fold8-tim-8-153436602.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH115')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH115', N'Samsung Galaxy S26 FE 5G 8GB/128GB', N'Samsung', 15890000, 13506500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/08/21/sm-s741-galaxys26fe-front-graphite-115909936.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH116')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH116', N'Samsung Galaxy A17 LTE 8GB/128GB', N'Samsung', 5290000, 4496500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/03/galaxy-a17-lte-light-blue-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH117')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH117', N'Samsung Galaxy S26 Ultra - 16GB/1TB', N'Samsung', 41990000, 35691500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH118')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH118', N'Samsung Galaxy A17 5G 8GB/128GB', N'Samsung', 7190000, 6111500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/08/15/thumb-a17-5g-blue.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH119')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH119', N'Samsung Galaxy S26 Plus - 12GB/256GB', N'Samsung', 24490000, 20816500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-plus-xanh-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH120')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH120', N'Samsung Galaxy A07 5G 4GB/128GB', N'Samsung', 5090000, 4326500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/01/15/a07-5g-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH121')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH121', N'Samsung Galaxy S26 Ultra - 12GB/512GB', N'Samsung', 35090000, 29826500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH122')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH122', N'HONOR X6e 4GB/256GB', N'Honor', 6990000, 5941500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/11/02/Gallery/716805_dien-thoai-di-dong-honor-x6e-4gb-256gb-den%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH123')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH123', N'Samsung Galaxy Z Flip8 5G 12GB/256GB', N'Samsung', 26990000, 22941500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/samsung-galaxy-z-flip8-hong-1-155547554.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH124')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH124', N'Samsung Galaxy Z Fold8 5G 12GB/512GB', N'Samsung', 46990000, 39941500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/samsung-galaxy-z-fold8-tim-8-153436602.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH125')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH125', N'Samsung Galaxy Z Fold8 Ultra 5G 12GB/256GB', N'Samsung', 45690000, 38836500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/samsung-galaxy-z-fold8-ultra-tim-4-154212817.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH126')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH126', N'Samsung Galaxy Z Fold8 Ultra 5G 12GB/512GB', N'Samsung', 51890000, 44106500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/samsung-galaxy-z-fold8-ultra-tim-4-154212817.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH127')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH127', N'Hera S9 - 2.8 inch', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/20/s9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH128')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH128', N'Xiaomi POCO M7 8GB/256GB', N'Xiaomi', 5490000, 4666500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/07/o19ap-silver-front-r1-wallpaper-111_639137522035097764.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH129')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH129', N'iPhone 16 Pro Max 512GB', N'Apple', 36690000, 31186500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-sa-mac-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH130')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH130', N'iPhone 16 Pro Max 256GB', N'Apple', 30790000, 26171500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-sa-mac-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH131')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH131', N'Xiaomi Redmi Note 14 8GB/128GB', N'Xiaomi', 5290000, 4496500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/01/04/redmi-note-14-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH132')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH132', N'OPPO Find X10 5G 12GB/256GB', N'OPPO', 34990000, 29741500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/295605_Combo%20Product%20-%20Find%20X10%20-%20Glow%20Coral.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH133')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH133', N'OPPO Find X10 Pro Max 16GB/512GB', N'OPPO', 49990000, 42491500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/785794_Combo%20Product%20-%20Find%20X10%20Pro%20Max%20-%20Lunar%20White.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH134')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH134', N'iPhone 17 Pro Max 256GB', N'Apple', 33990000, 28891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-max-cosmic-orange-pdp-image-position-1-cosmic-orange-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH135')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH135', N'TCL 40 NXTPAPER (8GB/256GB)', N'Chính hãng', 3590000, 3051500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/11/18/tcl-40-nxtpaper-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH136')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH136', N'OSCAL TIGER 12 (8GB/128GB) NFC', N'Chính hãng', 4390000, 3731500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/oscal-tiger-12-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH137')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH137', N'OSCAL TIGER 10 (8+8GB/256GB)', N'Chính hãng', 3590000, 3051500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/19/tiger10-black-20231007111011245.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH138')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH138', N'OSCAL MODERN 8 (8+8GB/256GB)', N'Chính hãng', 3990000, 3391500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/19/oscal-8-3.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH139')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH139', N'HONOR X5c Plus 4GB/64GB', N'Honor', 3490000, 2966500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/01/16/honor-x5c-plus-xanh.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH140')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH140', N'Xiaomi Redmi Note 17 Pro Max 5G 8GB/512GB', N'Xiaomi', 14590000, 12401500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/17/q16u-cloud-blush-white-back11-161455838.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH141')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH141', N'Xiaomi Redmi Note 17 Pro 5G 6GB/256GB', N'Xiaomi', 9290000, 7896500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/17/q16-blue-back-111-160301724.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH142')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH142', N'Xiaomi Redmi Note 17 5G 6GB/128GB', N'Xiaomi', 7390000, 6281500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/17/p19a-blue-front-11-155553849.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH143')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH143', N'Philips Xenium E527 4G', N'Chính hãng', 850000, 722500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/06/19/xanh.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH144')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH144', N'XOR X2 PRIME GOLD', N'Chính hãng', 485000000, 412250000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/01/25/oo.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH145')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH145', N'XOR TITANIUM X2 EBONY BUNDLE', N'Chính hãng', 196000000, 166600000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/03/11/enbody.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH146')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH146', N'Masstel izi 12 4G', N'Masstel', 450000, 382500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2022/03/14/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH147')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH147', N'Masstel Fami 12 4G', N'Masstel', 650000, 552500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2022/03/14/fami.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH148')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH148', N'Izi 10 4G', N'Chính hãng', 500000, 425000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/05/16/image-removebg-preview-19.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH149')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH149', N'Masstel Fami 60 4G', N'Masstel', 740000, 629000, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2022/05/16/image-removebg-preview-21.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH150')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH150', N'iPhone 14 128GB', N'Apple', 14790000, 12571500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/07/18/14-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH151')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH151', N'TCL 30 SE (4GB/128GB)', N'Chính hãng', 2490000, 2116500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/08/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH152')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH152', N'TCL 305 - CRUZE LITE', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/08/tcl-305.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH153')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH153', N'TCL 30 - PASSAT', N'Chính hãng', 2790000, 2371500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-30.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH154')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH154', N'Vivo Y16 4GB/128GB', N'Vivo', 3450000, 2932500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2022/09/15/image-removebg-preview-34.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH155')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH155', N'Masstel Izi 15', N'Masstel', 450000, 382500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2023/05/25/izi-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH156')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH156', N'Masstel Izi 30', N'Masstel', 600000, 510000, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2023/05/25/izi-30-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH157')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH157', N'Nokia 105 4G Pro', N'Nokia', 670000, 569500, 50, N'Chiếc', N'NOKIA', N'https://i.hoanghamobile.vn/Uploads/2023/06/28/105pro-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH158')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH158', N'Nokia 110 4G Pro', N'Nokia', 740000, 629000, 50, N'Chiếc', N'NOKIA', N'https://i.hoanghamobile.vn/Uploads/2023/06/28/110pro-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH159')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH159', N'OPPO Reno10 5G (8GB/256GB)', N'OPPO', 5990000, 5091500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2023/07/21/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH160')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH160', N'TCL 408 (4GB/64GB)', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/24/tcl-408-blue-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH161')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH161', N'TCL 408 (4GB/128GB)', N'Chính hãng', 2490000, 2116500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/24/tcl-408-blue-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH162')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH162', N'TCL 40 SE (4GB/128GB)', N'Chính hãng', 2790000, 2371500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/24/tcl-40-se-purple-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH163')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH163', N'TCL 40 SE (6GB/256GB)', N'Chính hãng', 2390000, 2031500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/24/tcl-40-se-purple-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH164')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH164', N'iPhone 15 128GB', N'Apple', 20990000, 17841500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pink-pure-back-iphone-15-pink-pure-front-2up-screen-usen.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH165')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH165', N'iPhone 15 Plus 128GB', N'Apple', 17590000, 14951500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-plus-blue-pure-back-iphone-15-plus-blue-pure-front-2up-screen-usen.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH166')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH166', N'iPhone 15 256GB', N'Apple', 24290000, 20646500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pink-pure-back-iphone-15-pink-pure-front-2up-screen-usen.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH167')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH167', N'iPhone 15 Plus 256GB', N'Apple', 21690000, 18436500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-plus-blue-pure-back-iphone-15-plus-blue-pure-front-2up-screen-usen.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH168')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH168', N'HTC Wildfire E3 lite (4GB/64GB)', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/12/htc-wildfire-e3-lite-grey.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH169')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH169', N'INOI A72 (4GB/128GB)', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/inoi-a72_638411669980430858.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH170')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH170', N'INOI A83 (6GB/128GB)', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/inoi-a83.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH171')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH171', N'INOI Note 13s (8GB/256GB)', N'Chính hãng', 2890000, 2456500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/inoi-note-13s.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH172')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH172', N'INOI 288S', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/inoi-288s.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH173')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH173', N'Masstel izi T2 4G', N'Masstel', 490000, 416500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2024/02/16/masstel-izi-t2-4g-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH174')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH174', N'Masstel Fami 12S 4G', N'Masstel', 650000, 552500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2024/02/16/masstel-fami-12s-4g-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH175')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH175', N'TCL 406S (4GB/64GB)', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/tcl-406s-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH176')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH176', N'Mobell M139 4G', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/26/m139-new-red-05.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH177')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH177', N'Mobell M331 4G', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/26/m331.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH178')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH178', N'Mobell M539 4G', N'Chính hãng', 640000, 544000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/26/m539.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH179')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH179', N'Mobell F209 4G', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/26/f209.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH180')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH180', N'Mobell F309 4G', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/26/f309.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH181')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH181', N'Nokia 220 4G', N'Nokia', 980000, 833000, 50, N'Chiếc', N'NOKIA', N'https://i.hoanghamobile.vn/Uploads/2024/06/10/nokia-220-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH182')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH182', N'Nokia 3210 4G', N'Nokia', 1490000, 1266500, 50, N'Chiếc', N'NOKIA', N'https://i.hoanghamobile.vn/Uploads/2024/06/10/nokia-320-4g-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH183')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH183', N'iPhone 16 128GB', N'Apple', 24090000, 20476500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-mong-ket.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH184')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH184', N'Itel it8010 4G', N'Chính hãng', 290000, 246500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/16/it8010-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH185')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH185', N'INOI A54 4GB/256GB', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/07/inoi-a54.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH186')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH186', N'Masstel izi 16 4G', N'Masstel', 450000, 382500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2024/09/06/izi-16-black-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH187')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH187', N'HMD 105 4G', N'Chính hãng', 640000, 544000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/06/vox-basic-hmd-vox-basic-r-front-black-00000.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH188')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH188', N'iPhone 16 Plus 128GB', N'Apple', 24090000, 20476500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-luu-ly.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH189')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH189', N'iPhone 16 Pro 128GB', N'Apple', 24990000, 21241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH190')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH190', N'Điện thoại Itel it9211 - 4G', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/it9211-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH191')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH191', N'iPhone 16 256GB', N'Apple', 23890000, 20306500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-mong-ket.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH192')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH192', N'iPhone 16 512GB', N'Apple', 29990000, 25491500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-mong-ket.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH193')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH193', N'iPhone 16 Plus 256GB', N'Apple', 26990000, 22941500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-luu-ly.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH194')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH194', N'iPhone 16 Plus 512GB', N'Apple', 29390000, 24981500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-luu-ly.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH195')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH195', N'iPhone 16 Pro Max 1TB', N'Apple', 42390000, 36031500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-sa-mac-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH196')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH196', N'INOI 244', N'Chính hãng', 410000, 348500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/01/inoi-244-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH197')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH197', N'INOI 240', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/01/inoi-240-modern-blue-859cc38b37.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH198')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH198', N'Điện thoại HTC Wildfire E5 Plus (6+6GB/128GB)', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/dien-thoai-htc-wildfire-e5-plus-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH199')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH199', N'Điện thoại di động TCL 503 3GB/64GB', N'Chính hãng', 1850000, 1572500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/tcl-503-gray-1-04327edabf.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH200')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH200', N'Điện thoại di động TCL 505 4GB/128GB', N'Chính hãng', 2690000, 2286500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/356741392.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH201')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH201', N'REDMAGIC 10 Pro 12GB/256GB Bản Quốc Tế Chính Hãng', N'Chính hãng', 17990000, 15291500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/22/anh-sp-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH202')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH202', N'REDMAGIC 10 Pro 16GB/512GB Bản Quốc Tế Chính Hãng', N'Chính hãng', 21990000, 18691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/redmagic-pro-10-thumb-ads.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH203')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH203', N'iPhone 16e 128GB', N'Apple', 13490000, 11466500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/02/20/iphone-16e-trang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH204')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH204', N'iPhone 16e 256GB', N'Apple', 18350000, 15597500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/02/20/iphone-16e-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH205')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH205', N'iPhone 16e 512GB', N'Apple', 19990000, 16991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/02/20/iphone-16e-trang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH206')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH206', N'Benco V91 - 4GB/128GB', N'Chính hãng', 2390000, 2031500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/09/benco-v91-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH207')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH207', N'REDMAGIC 10S PRO 12GB/256GB', N'Chính hãng', 18490000, 15716500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/21/redmagic-10s-pro-den-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH208')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH208', N'Điện thoại Gaming REDMAGIC 10S PRO 16GB/512GB', N'Chính hãng', 22990000, 19541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/21/redmagic-silver-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH209')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH209', N'iPhone 17 256GB', N'Apple', 27990000, 23791500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/17/iphone-17-thuong.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH210')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH210', N'Samsung Galaxy S25 FE 8GB/128GB', N'Samsung', 12990000, 11041500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/03/galaxy-s25-fe-navy-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH211')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH211', N'iPhone 17 512GB', N'Apple', 34590000, 29401500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-mist-blue-pdp-image-position-1-mist-blue-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH212')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH212', N'iPhone 17 Pro 256GB', N'Apple', 31490000, 26766500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/17/iphone-17-pro.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH213')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH213', N'iPhone 17 Pro 512GB', N'Apple', 37990000, 32291500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-deep-blue-pdp-image-position-1-deep-blue-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH214')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH214', N'iPhone 17 Pro 1TB', N'Apple', 44490000, 37816500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-max-silver-pdp-image-position-1-silver-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH215')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH215', N'iPhone 17 Pro Max 512GB', N'Apple', 40490000, 34416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-max-silver-pdp-image-position-1-silver-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH216')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH216', N'iPhone 17 Pro Max 1TB', N'Apple', 46990000, 39941500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-max-silver-pdp-image-position-1-silver-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH217')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH217', N'iPhone Air 1TB', N'Apple', 31990000, 27191500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-air-sky-blue-pdp-image-position-1-sky-blue-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH218')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH218', N'iPhone Air 256GB', N'Apple', 23390000, 19881500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/17/iphone-air.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH219')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH219', N'iPhone Air 512GB', N'Apple', 29490000, 25066500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-air-sky-blue-pdp-image-position-1-sky-blue-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH220')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH220', N'iPhone 17 Pro Max 2TB', N'Apple', 59990000, 50991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/iphone-17-pro-max-silver-pdp-image-position-1-silver-color-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH221')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH221', N'HONOR Magic V5', N'Honor', 33490000, 28466500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/09/23/mkt-maybach-identity-pictures-id-dawn-gold-rear-jpg-800-20250622.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH222')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH222', N'Xiaomi Redmi 15C 4GB/128GB', N'Xiaomi', 3580000, 3043000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/10/17/p15a-blue-front-wallpaper-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH223')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH223', N'HONOR X7d 4G 8GB/256GB', N'Honor', 6390000, 5431500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/10/27/x7d-vang.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH224')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH224', N'Xiaomi POCO M7 6GB/128GB', N'Xiaomi', 4690000, 3986500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/07/o19ap-silver-front-r1-wallpaper-111_639137522035097764.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH225')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH225', N'HONOR X9d 12GB/512GB', N'Honor', 12290000, 10446500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/martin-identity-pictures-sunrise-gold-hq.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH226')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH226', N'Điện thoại Gaming REDMAGIC 11 PRO 12GB/256GB', N'Chính hãng', 19290000, 16396500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/03/redmagic-11-pro-cryo-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH227')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH227', N'Điện thoại Gaming REDMAGIC 11 PRO 16GB/512GB', N'Chính hãng', 23290000, 19796500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/03/redmagic-11-pro-subzero-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH228')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH228', N'OPPO Reno15 5G 12GB+256GB', N'OPPO', 14990000, 12741500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/combo-product-reno15-5g-white-104711120.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH229')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH229', N'OPPO Reno15 F 5G 8GB+256GB', N'OPPO', 10990000, 9341500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/combo-product-reno15-f-blue-104637703.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH230')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH230', N'Xiaomi Redmi Note 15 5G 6GB/128GB', N'Xiaomi', 5690000, 4836500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/12/25/p17purple-back-0916-01-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH231')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH231', N'Xiaomi 17 Ultra 16GB/512GB', N'Xiaomi', 29490000, 25066500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/02/14/p1-green-back-r121.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH232')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH232', N'Samsung Galaxy S26 - 12GB/256GB', N'Samsung', 20890000, 17756500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-trang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH233')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH233', N'HONOR X5c Plus 4GB/128GB', N'Honor', 3790000, 3221500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/01/16/honor-x5c-plus-xanh.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH234')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH234', N'HONOR X7d 4G 8GB/128GB', N'Honor', 5990000, 5091500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/10/27/x7d-vang.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH235')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH235', N'Samsung Galaxy S26 Plus - 12GB/512GB', N'Samsung', 28950000, 24607500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-plus-xanh-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH236')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH236', N'HONOR X8d - 8GB/128GB', N'Honor', 7390000, 6281500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/02/27/luna-id-light-blue-front-left-rgb-png-en-hq-800-202511191.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH237')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH237', N'iPhone 17e 256GB', N'Apple', 20590000, 17501500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/iphone-17e-soft-pink-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH238')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH238', N'iPhone 17e 512GB', N'Apple', 26490000, 22516500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/iphone-17e-black-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH239')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH239', N'Samsung Galaxy A37 5G - 8GB/128GB', N'Samsung', 9890000, 8406500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/03/17/a37-white-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH240')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH240', N'Samsung Galaxy S26 Ultra - 12GB/512GB (BHĐT)', N'Samsung', 31990000, 27191500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH241')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH241', N'Điện thoại Gaming REDMAGIC 11 Air 12GB/256GB', N'Chính hãng', 13990000, 11891500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/redmagic-11-air-white-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH242')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH242', N'Điện thoại Gaming REDMAGIC 11 Air 16GB/512GB', N'Chính hãng', 16990000, 14441500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/redmagic-11-air-black-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH243')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH243', N'OPPO Find N6 16GB/512GB', N'OPPO', 57890000, 49206500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/03/23/find-n6-combo-product-orange.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH244')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH244', N'Điện thoại OPPO A6 Pro (8GB+128GB)', N'OPPO', 8190000, 6961500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2025/10/23/oppo-a6-pro-hong-4g.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH245')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH245', N'TECNO SPARK Go 3 4GB+4GB/64GB', N'Tecno', 3490000, 2966500, 50, N'Chiếc', N'TECNO', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/tecno-spark-go-3-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH246')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH246', N'HONOR X7d 5G 8GB/256GB', N'Honor', 7190000, 6111500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/10/27/x7d-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH247')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH247', N'HONOR X9d 12GB/256GB', N'Honor', 10790000, 9171500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/martin-identity-pictures-sunrise-gold-hq.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH248')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH248', N'OPPO Find X9s 12GB/256GB', N'OPPO', 21090000, 17926500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/04/24/find-x9s-xam.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH249')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH249', N'Nubia V80 Max 4GB/128GB', N'Nubia', 3590000, 3051500, 50, N'Chiếc', N'NUBIA', N'https://i.hoanghamobile.vn/Uploads/2026/01/07/thumb-ads-v80-max.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH250')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH250', N'Nubia V80 Design 4GB/128GB', N'Nubia', 3690000, 3136500, 50, N'Chiếc', N'NUBIA', N'https://i.hoanghamobile.vn/Uploads/2026/01/07/thumb-ads-v80.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH251')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH251', N'Xiaomi 17T 5G 12GB/512GB', N'Xiaomi', 15750000, 13387500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/12/p12a-violet-back-r51.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH252')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH252', N'HONOR 600 8GB/256GB', N'Honor', 15690000, 13336500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/06/01/h600-da-cam-13.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH253')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH253', N'Xiaomi 17T PRO 5G 12GB/512GB', N'Xiaomi', 20190000, 17161500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/12/p12u-deep-blue-back-r11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH254')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH254', N'TECNO SPARK 50 (4+4GB/128GB)', N'Tecno', 4790000, 4071500, 50, N'Chiếc', N'TECNO', N'https://i.hoanghamobile.vn/Uploads/2026/05/14/tecno-spark-50-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH255')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH255', N'OPPO A6c (4GB+64GB)', N'OPPO', 4490000, 3816500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/a6c-tim.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH256')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH256', N'OPPO A6c (4GB+128GB)', N'OPPO', 5490000, 4666500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/a6c-nau.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH257')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH257', N'Điện thoại Gaming REDMAGIC 11S PRO 12GB/256GB', N'Chính hãng', 22990000, 19541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/19/nubia-redmagic-11s-pro-890c2d21bb.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH258')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH258', N'Điện thoại Gaming REDMAGIC 11S PRO 16GB/512GB', N'Chính hãng', 25990000, 22091500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/19/redmagic-trang_639174758974408290.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH259')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH259', N'HONOR X9d 8GB/256GB', N'Honor', 10490000, 8916500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/martin-identity-pictures-sunrise-gold-hq.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH260')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH260', N'Xiaomi Redmi 17 5G 4GB/128GB', N'Xiaomi', 5090000, 4326500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/01/q15-orange-back1-090747790.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH261')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH261', N'TECNO POVA CURVE 2 5G (8+8GB/128GB)', N'Tecno', 8390000, 7131500, 50, N'Chiếc', N'TECNO', N'https://i.hoanghamobile.vn/Uploads/2026/08/05/tecno-pova-curve-tim-1-091638524.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH262')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH262', N'OPPO A6 8GB/256GB', N'OPPO', 11490000, 9766500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/oppo-a6-hong-161353395.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH263')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH263', N'iPhone 18 Pro Max 1TB', N'Apple', 60490000, 51416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH264')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH264', N'iPhone 18 Pro Max 2TB', N'Apple', 79990000, 67991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH265')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH265', N'iPhone 18 Pro 512GB', N'Apple', 44990000, 38241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-glacier-pdp-images-vi-vn-021533622.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH266')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH266', N'iPhone 18 Pro 1TB', N'Apple', 57490000, 48866500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH267')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH267', N'iPhone Duo 1TB', N'Apple', 84490000, 71816500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphon-duo-030228624.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH268')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH268', N'iPhone Duo 2TB', N'Apple', 103990000, 88391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphon-duo-030228624.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH269')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH269', N'iPhone 18 Pro 2TB', N'Apple', 77190000, 65611500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH270')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH270', N'Honor 600s 5G 6GB/128GB', N'Honor', 8990000, 7641500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/10/08/honor-600s-tim-085734382.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH271')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH271', N'OPPO A7 Pro 5G 6GB/256GB', N'OPPO', 12990000, 11041500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/a7-pro-5g-xanh-144220999.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH272')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH272', N'Nubia V80 Max 6GB/128GB', N'Nubia', 3890000, 3306500, 50, N'Chiếc', N'NUBIA', N'https://i.hoanghamobile.vn/Uploads/2026/01/07/thumb-ads-v80-max.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH273')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH273', N'Honor 400 12GB/256GB', N'Honor', 11190000, 9511500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/07/04/honor-400-thumb-ai.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH274')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH274', N'Honor 400 Lite 5G 12GB/256GB', N'Honor', 7390000, 6281500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/07/04/400-lite-thumb-ai.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH275')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH275', N'OPPO Reno14 5G 12GB+256GB', N'OPPO', 13690000, 11636500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/reno14-5g-combo-product-white-001-104723988.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH276')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH276', N'TECNO POVA 7 (8+8GB/128GB)', N'Tecno', 4390000, 3731500, 50, N'Chiếc', N'TECNO', N'https://i.hoanghamobile.vn/Uploads/2025/06/11/tecno-pova-7-geek-black-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH277')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH277', N'TECNO POVA 7 (8+8GB/256GB)', N'Tecno', 4990000, 4241500, 50, N'Chiếc', N'TECNO', N'https://i.hoanghamobile.vn/Uploads/2025/06/11/tecno-pova-7-geek-black-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH278')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH278', N'Reno14 F 5G 8GB+256GB', N'Chính hãng', 9490000, 8066500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/reno14-f-combo-product-blue-104700156.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH279')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH279', N'OSCAL TIGER 12 (12GB/256GB) NFC', N'Chính hãng', 4990000, 4241500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/oscal-tiger-12-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH280')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH280', N'Xiaomi Redmi A7 Pro 4GB/128GB (NFC)', N'Xiaomi', 3790000, 3221500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/03/26/p25-black-front-wallpaper_639101191235209787.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH281')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH281', N'OPPO A6t Pro (8GB+128GB)', N'OPPO', 8690000, 7386500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/a6t-pro-tim-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH282')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH282', N'iPhone Duo 512GB', N'Apple', 71490000, 60766500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphon-duo-030228624.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH283')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH283', N'OPPO Reno16 F 5G 8GB+256GB', N'OPPO', 14590000, 12401500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/combo-product-dream-purple-reno-16f-rgb-104648868.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH284')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH284', N'iPhone 18 Pro Max 512GB', N'Apple', 47990000, 40791500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH285')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH285', N'Điện thoại Viettel T2 - 4G (DÙNG MỌI LOẠI SIM)', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/20/t2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH286')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH286', N'Hera V9', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/v9-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH287')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH287', N'Xiaomi Redmi A7 3GB/64GB', N'Xiaomi', 3290000, 2796500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/06/20/c3zr-purple-back1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH288')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH288', N'Samsung Galaxy Z Fold7 5G 12GB/256GB', N'Samsung', 37490000, 31866500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/thumb-fold7-xanh-navy.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH289')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH289', N'M-SMART MBOOK 1409A', N'Chính hãng', 2790000, 2371500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/19/mbook-1409a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH290')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH290', N'Laptop ASUS Vivobook 14 X1407CA-LY203W', N'Vivo', 23490000, 19966500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/09/19/asus-vivobook-14-x1407aa-1-091045999.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH291')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH291', N'Laptop HP AI 15-fd2050wm D4JK5UA', N'HP', 16790000, 14271500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2026/09/19/ly203w-1-120558555.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH292')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH292', N'Laptop Lenovo LOQ 15IRH11E-83Y9003VVN', N'Lenovo', 25690000, 21836500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/10/12/lenovo-loq-15irh11e-83y9003vvn-1-171200136.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH293')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH293', N'Lenovo IdeaPad Slim 3 14IWC11 83RQ002PVN (I5-320/8GB/512GB/14 inch/Win11)', N'Lenovo', 17890000, 15206500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/07/16/lenovo-ideapad-slim-3-14iwc11-83rq002pvn-1-150907335.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH294')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH294', N'Laptop ASUS TUF Gaming A15 FA506NCG-HN329W', N'Asus', 28590000, 24301500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/06/02/asus-tuf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH295')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH295', N'Laptop Colorful X15 XS22 (i5 12450H/16GB/512GB/RTX3050 4GB/15.6 inch)', N'Chính hãng', 25890000, 22006500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/09/08/colorful-x15-xs22-1-155134573.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH296')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH296', N'MacBook Air M5 13 inch 10CPU 8GPU 16GB/512GB', N'Apple', 32990000, 28041500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH297')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH297', N'MacBook Neo 2026 8GB/256GB', N'Apple', 18490000, 15716500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/macbook-13-in-a18-pro-citrus-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH298')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH298', N'MacBook Pro M6 14 inch 12CPU 12GPU 16GB/1TB', N'Apple', 54990000, 46741500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/macbook-pro-14-inch-m6-space-black-pdp-image-position-1-vi-vn-145528508.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH299')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH299', N'MacBook Pro M6 14 inch 12CPU 12GPU 24GB/1TB', N'Apple', 60490000, 51416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/macbook-pro-14-inch-m6-space-black-pdp-image-position-1-vi-vn-145528508.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH300')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH300', N'MacBook Pro M6 14 inch 12CPU 12GPU 32GB/1TB', N'Apple', 65990000, 56091500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/macbook-pro-14-inch-m6-space-black-pdp-image-position-1-vi-vn-145528508.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH301')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH301', N'Laptop ASUS Vivobook 15 X1504MA-BQ1348W', N'Vivo', 20490000, 17416500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/10/24/Gallery/701885_asus-x1504ma-bq1348w-blue%20(2).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH302')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH302', N'MacBook Air M5 13 inch 10CPU 10GPU 16GB/1TB', N'Apple', 39490000, 33566500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH303')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH303', N'MacBook Air M5 15 inch 10CPU 10GPU 16GB/512GB', N'Apple', 40490000, 34416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-15-in-m5-midnight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH304')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH304', N'MacBook Air M5 13 inch 10CPU 10GPU 16GB/512GB', N'Apple', 36990000, 31441500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH305')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH305', N'MacBook Air M5 13 inch 10CPU 8GPU 16GB/512GB - Sạc nhanh 70W', N'Apple', 34490000, 29316500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH306')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH306', N'MacBook Air M5 13 inch 10CPU 10GPU 24GB/512GB', N'Apple', 37990000, 32291500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/11/05/Gallery/137272_macbook-air-m5-13-inch-10cpu-10gpu-24gb-512gb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH307')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH307', N'MacBook Pro 14 inch M5 10CPU 10GPU (16GB/1TB)', N'Apple', 53490000, 45466500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/macbook-pro-14-inch-m5-ai-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH308')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH308', N'MacBook Neo 2026 8GB/512GB', N'Apple', 21190000, 18011500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/macbook-13-in-a18-pro-citrus-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH309')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH309', N'MacBook Pro PROLED XDR 14 inch M5 Pro 15CPU 16GPU 24GB/1TB', N'Apple', 79990000, 67991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/424959_macbook-pro-proled-xdr-14-inch-m5-pro-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH310')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH310', N'MacBook Pro PROLED XDR 14 inch M5 Pro 18CPU 20GPU 48GB/1TB', N'Apple', 101990000, 86691500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/424959_macbook-pro-proled-xdr-14-inch-m5-pro-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH311')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH311', N'MacBook Pro PROLED XDR 14 inch M5 Max 18CPU 32GPU 36GB/2TB', N'Apple', 123990000, 105391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/467454_macbook-pro-proled-xdr-14-inch-m5-max-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH312')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH312', N'MacBook Pro PROLED XDR 16 inch M5 Pro 18CPU 20GPU 24GB/1TB', N'Apple', 92990000, 79041500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/846873_macbook-pro-proled-xdr-16-inch-m5-pro-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH313')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH313', N'MacBook Pro PROLED XDR 16 inch M5 Pro 18CPU 20GPU 48GB/1TB', N'Apple', 109490000, 93066500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/846873_macbook-pro-proled-xdr-16-inch-m5-pro-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH314')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH314', N'MacBook Pro PROLED XDR 16 inch M5 Max 18CPU 32GPU 36GB/2TB', N'Apple', 130990000, 111341500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/758345_macbook-pro-proled-xdr-16-inch-m5-max-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH315')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH315', N'MacBook Pro PROLED XDR 16 inch M5 Max 18CPU 40GPU 48GB/2TB', N'Apple', 145990000, 124091500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/Gallery/758345_macbook-pro-proled-xdr-16-inch-m5-max-silver%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH316')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH316', N'Laptop ASUS Gaming V16 V3607V-TK416W', N'Asus', 30690000, 26086500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/06/12/v3607vu-rp290w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH317')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH317', N'Laptop Dell Pro 14 Essential PV14250 71092353', N'Dell', 27900000, 23715000, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/pv14250-71084489-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH318')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH318', N'Laptop ASUS Vivobook 14 X1404MA-EB028W', N'Vivo', 21690000, 18436500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/09/25/asus-vivobook-14-x1404ma-eb028w-1-120015163.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH319')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH319', N'Laptop ASUS Vivobook Go 14 E1404FA-EB012W', N'Vivo', 17490000, 14866500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/04/29/eb012w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH320')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH320', N'Laptop ASUS Zenbook 14 UX3405CA-ST1713W', N'Asus', 29290000, 24896500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/29/ux3405ca-st1713w-1-111954761.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH321')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH321', N'Laptop ASUS TUF Gaming A15 FA506NCG-HN396W', N'Asus', 25290000, 21496500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/06/02/asus-tuf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH322')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH322', N'Laptop Dell DC15250 P112F010-5', N'Dell', 16890000, 14356500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/02/dell-15-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH323')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH323', N'ASUS ExpertBook P1403CVAY-308D4256-50W', N'Asus', 14990000, 12741500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/asus-expertbook-p1403cvay-308d4256-50w-4-162058968.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH324')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH324', N'Laptop Lenovo LOQ 15IRX10 83JE00PEVN', N'Lenovo', 37490000, 31866500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/03/10/83je00pevn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH325')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH325', N'Dell Pro 15 PV15250 (i5-1334U/8GB/512GB/15.6 inch/Đen) - NK chính hãng', N'Dell', 15490000, 13166500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/29/laptopdell-pro-15-pv15250-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH326')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH326', N'Dell Pro 15 PV 15250 P112F013', N'Dell', 13690000, 11636500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/29/laptopdell-pro-15-pv15250-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH327')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH327', N'Dell DC15250 P112F010 (i5-1334U/8/512G/15.6 inch touch/Win11) NK chính hãng', N'Dell', 16890000, 14356500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/02/dell-15-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH328')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH328', N'Asus Vivobook Go E1504FA BQ340W (R5-7520U/16GB/512GB/15.6 inch/W11/Bạc)', N'Vivo', 20690000, 17586500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/06/24/laptop-asus-vivobook-e1504fa-bq340w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH329')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH329', N'Laptop Dell Latitude 3450 (i3-1315U/8GB/256GB/14"/W11/Đen) - NK chính hãng', N'Dell', 13790000, 11721500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/18/laptop-dell-latitude-3450-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH330')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH330', N'Dell 15 DC15250 6641028 (I5-1334U/8G/512G/15.6 inch/Đen) - NK chính hãng', N'Dell', 14690000, 12486500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/06/02/dell-15-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH331')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH331', N'Laptop ASUS ExpertBook P1 P1503CVA-C3U08-50W', N'Asus', 15690000, 13336500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/05/05/p1503cva-c3u08-50w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH332')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH332', N'Laptop ASUS Vivobook 14 X1407CA-LY008W', N'Vivo', 22890000, 19456500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/05/05/x1407ca-ly008w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH333')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH333', N'Laptop Acer Aspire Lite 14 AL14-52M-52UU', N'Acer', 18590000, 15801500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2026/04/18/acer-n23g2-al14-52m-52uu.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH334')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH334', N'Laptop ASUS Vivobook X1404VA-EB355W', N'Vivo', 21690000, 18436500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/asus-vivobook-x1404va-eb355w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH335')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH335', N'Laptop Lenovo LOQ 15IRH11E-83Y9003YVN', N'Lenovo', 27990000, 23791500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/10/12/lenovo-loq-15irh11e-83y9003vvn-1-171200136.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH336')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH336', N'Laptop Lenovo IdeaPad Slim 3 14IPH11 83UQ000JVN', N'Lenovo', 23690000, 20136500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/05/20/83uq000jvn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH337')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH337', N'Laptop ASUS TUF Gaming F16 FX608JHI-TU209W', N'Asus', 40990000, 34841500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/05/09/fx608jhi-tu209w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH338')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH338', N'Laptop ASUS TUF Gaming F16 FX608JMI-TU241W', N'Asus', 43290000, 36796500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/05/09/fx608jmi-tu241w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH339')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH339', N'Laptop Dell 15 DC15250 71092480', N'Dell', 24290000, 20646500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/05/09/dc15250-71092480-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH340')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH340', N'Laptop MSI Modern 15 H C2RMG-292VN', N'MSI', 18990000, 16141500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/02/09/c2rmg-292vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH341')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH341', N'Laptop Lenovo IdeaPad Slim 3 14IRH10 83K00009VN', N'Lenovo', 24290000, 20646500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/02/27/imghunt-com-20260227-52114-51728-laptop-lenovo-ideapad-slim-3-14irh10-83k00007vn-2.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH342')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH342', N'Laptop Dell 14 DC14250 DC4C5386W', N'Dell', 24890000, 21156500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/04/29/dc4c5386w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH343')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH343', N'Laptop Asus Vivobook 14 M1405NAQ-LY010W', N'Vivo', 18990000, 16141500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/07/10/ly010w-tach-1-113309510.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH344')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH344', N'Laptop ASUS ExpertBook B1 BM1403CDA-S61611W', N'Asus', 18790000, 15971500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/03/24/bm1403cda-s61611w-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH345')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH345', N'ASUS Vivobook Go 14 E1404FA-EB935W', N'Vivo', 19990000, 16991500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/08/11/asus-vivobook-go-14-e1404fa-eb935w-1-165016227.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH346')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH346', N'ASUS Vivobook 14 M1407KA-LY849W (Ryzen AI 5-330/16GB/512GB/14"/Win11/Bạc)', N'Vivo', 21290000, 18096500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2026/08/04/asus-vivobook-14-m1407ka-ly849w-1-170427536.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH347')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH347', N'ASUS S3407C SF913W (Ultra 5-225H/16GB RAM/512GB/14 inch/Win11/Xám)', N'Asus', 25990000, 22091500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/08/04/asus-vivobook-s14-s3407ca-sf913w-1-170649384.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH348')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH348', N'ASUS Vivobook S14 S3407VA-LY256W (Core 7-240H/16GB/512GB/14 inch/W11/Bạc)', N'Vivo', 26490000, 22516500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/s3407va-ly046w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH349')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH349', N'Laptop ASUS Vivobook S14 S3407VA LY146W', N'Vivo', 21390000, 18181500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/s3407va-ly046w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH350')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH350', N'HP Pavilion 15-eg3094TU (8C5L5PA)', N'HP', 17290000, 14696500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/8c5l5pa-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH351')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH351', N'HP Pavilion 15-EG3092TU (8C5L3PA)', N'HP', 19490000, 16566500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2024/07/29/8c5l3pa-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH352')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH352', N'Laptop MSI Thin 15 B13UC-3247VN', N'MSI', 19290000, 16396500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/10/25/b13uc-3247vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH353')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH353', N'Laptop HP OmniBook 5 16-ag1068AU BZ7T0PA', N'HP', 25890000, 22006500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/hp-omnibook-5-16-ag1068au-bz7t0pa-1-161658685.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH354')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH354', N'HP VICTUS 15 FA2452TX-D44VLPA', N'HP', 27990000, 23791500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2026/08/11/hp-victus-15-fa2452tx-d44vlpa-1-165033948.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH355')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH355', N'Laptop Lenovo IdeaPad Slim 5 15ARP10 83J3001DVN', N'Lenovo', 24490000, 20816500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/83j3001dvn-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH356')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH356', N'Laptop ASUS ExpertBook P1403CVAY-508D4256-50W', N'Asus', 18490000, 15716500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/03/10/p1403cva-c5h16-50w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH357')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH357', N'Laptop ASUS ExpertBook P1403CVAY-516D4-50W', N'Asus', 22490000, 19116500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/asus-expertbook-p1403cvay-308d4256-50w-4-162058968.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH358')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH358', N'Laptop ASUS ExpertBook P1403CVAY-716D4-50W', N'Asus', 26690000, 22686500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/asus-expertbook-p1403cvay-308d4256-50w-4-162058968.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH359')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH359', N'Laptop Asus ExpertBook P1403CVA-C5H08-50W', N'Asus', 18690000, 15886500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/03/10/p1403cva-c5h16-50w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH360')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH360', N'Lenovo V14 Gen 5 83HD005JVN (Core i5-13420H/16GB/512GB/14 inch/Windows 11)', N'Lenovo', 22890000, 19456500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/07/16/lenovo-v14-gen-5-83hd005jvn-1-150843677.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH361')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH361', N'MacBook Pro 14" M4 Max 36GB/1TB', N'Apple', 78990000, 67141500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH362')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH362', N'MacBook Pro 16" M4 Pro 24GB/512GB', N'Apple', 63990000, 54391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH363')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH363', N'MacBook Pro 16" M4 Max 48GB/1TB', N'Apple', 99990000, 84991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH364')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH364', N'MacBook Pro 16" M4 Pro 48GB/512GB', N'Apple', 73990000, 62891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH365')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH365', N'MacBook Air M4 13 inch 16GB/512GB', N'Apple', 28790000, 24471500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-13-inch-m4-non-ai-mar25-silver-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH366')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH366', N'MacBook Air M4 13 inch 24GB/512GB', N'Apple', 33990000, 28891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-13-inch-m4-non-ai-mar25-sky-blue-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH367')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH367', N'MacBook Air M4 15 inch 16GB/256GB', N'Apple', 28590000, 24301500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-15-inch-m4-non-ai-mar25-midnight-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH368')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH368', N'MacBook Air M4 15 inch 16GB/512GB', N'Apple', 33790000, 28721500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-15-inch-m4-non-ai-mar25-sky-blue-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH369')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH369', N'MacBook Air M4 15 inch 24GB/512GB', N'Apple', 38890000, 33056500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-15-inch-m4-non-ai-mar25-sky-blue-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH370')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH370', N'MacBook Pro 14 inch M5 10CPU 10GPU (16GB/512GB)', N'Apple', 49490000, 42066500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/macbook-pro-14-inch-m5-ai-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH371')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH371', N'DELL 15 DC15250 CPH995', N'Dell', 25390000, 21581500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/05/09/dc15250-71092479-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH372')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH372', N'MacBook Pro M5 Pro 14 inch 15CPU 16GPU 24GB/1TB', N'Apple', 67490000, 57366500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/mab191-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH373')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH373', N'MacBook Pro M5 14 inch 10CPU 10GPU 32GB/1TB', N'Apple', 54990000, 46741500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/mab191-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH374')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH374', N'MacBook Pro M5 Pro 14 inch 18CPU 20GPU 24GB/2TB', N'Apple', 87490000, 74366500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/mab191-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH375')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH375', N'MacBook Pro M5 Max 14 inch 18CPU 32GPU 36GB/2TB', N'Apple', 96990000, 82441500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/mab191-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH376')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH376', N'MacBook Pro M5 Pro 16 inch 18CPU 20GPU 24GB/1TB', N'Apple', 80490000, 68416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-pro-16-in-m5-pro-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH377')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH377', N'MacBook Pro M5 Pro 16 inch 18CPU 20GPU 48GB/1TB', N'Apple', 97490000, 82866500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-pro-16-in-m5-pro-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH378')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH378', N'MacBook Pro M5 Max 16 inch 18CPU 32GPU 36GB/2TB', N'Apple', 118490000, 100716500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-pro-16-in-m5-max-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH379')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH379', N'MacBook Pro M5 Max 16 inch 18CPU 40GPU 48GB/2TB', N'Apple', 119490000, 101566500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-pro-16-in-m5-max-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH380')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH380', N'Laptop ASUS TUF Gaming FX607VJR-TU320W', N'Asus', 29690000, 25236500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/05/29/fx607vj-rl034w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH381')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH381', N'Laptop Lenovo IdeaPad Slim 5 14AKP10 83HX00F5VN', N'Lenovo', 32490000, 27616500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/lenovo-ideapad-slim-5-14akp10-83hx00f5vn-4-160319778.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH382')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH382', N'Laptop Lenovo IdeaPad Slim 5 14IMH10 83V6001GVN', N'Lenovo', 22490000, 19116500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/83v6001gvn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH383')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH383', N'MacBook Air M5 13 inch 10CPU 10GPU 24GB/1TB', N'Apple', 44990000, 38241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH384')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH384', N'MacBook Air M5 15 inch 10CPU 10GPU 16GB/1TB', N'Apple', 49490000, 42066500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-15-in-m5-midnight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH385')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH385', N'MacBook Air M5 15 inch 10CPU 10GPU 24GB/1TB', N'Apple', 54490000, 46316500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-15-in-m5-midnight-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH386')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH386', N'MacBook Air M4 13 inch 16GB/256GB', N'Apple', 25090000, 21326500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/macbook-air-13-inch-m4-non-ai-mar25-midnight-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH387')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH387', N'MacBook Pro 14" M4 16GB/512GB', N'Apple', 36890000, 31356500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH388')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH388', N'MacBook Pro 14" M4 16GB/1TB', N'Apple', 43990000, 37391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH389')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH389', N'MacBook Pro 14" M4 Pro 24GB/512GB', N'Apple', 46890000, 39856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/macbook-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH390')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH390', N'MacBook Pro 14 inch M5 10CPU 10GPU (24GB/1TB)', N'Apple', 58990000, 50141500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/macbook-pro-14-inch-m5-ai-silver-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH391')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH391', N'MacBook Pro M5 Pro 14 inch 15CPU 16GPU 24GB/2TB', N'Apple', 81490000, 69266500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/04/mab191-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH392')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH392', N'Dell Inspiron 14 5440 (N4I5211W1)', N'Dell', 22990000, 19541500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2024/05/28/dell-inspiron-14-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH393')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH393', N'DELL 15 DC15250(CPH992) i5-1334U/16G/512GB/15.6 inch/W11/OFFICE_2024/Bạc', N'Dell', 21290000, 18096500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/05/09/dc15250-71092479-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH394')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH394', N'Laptop Acer Gaming Predator Helios Neo 16 PHN16-I31-74MN NH.U4SSV.002', N'Acer', 64990000, 55241500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2026/05/19/phn16-i31-74mn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH395')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH395', N'Laptop Acer Gaming Predator Helios Neo 16 PHN16-I31-72XE NH.U4RSV.001', N'Acer', 69990000, 59491500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2026/05/19/phn16-i31-72xe-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH396')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH396', N'Laptop Acer Gaming Predator Helios Neo 16 PHN16-I31-50H7 NH.U4SSV.001', N'Acer', 59990000, 50991500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2026/05/19/phn16-i31-50h7-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH397')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH397', N'Laptop Dell 15 DC15250 DC5I7952W1', N'Dell', 22990000, 19541500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2026/01/26/dc5i7952w1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH398')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH398', N'Màn hình Xiaomi Gaming G24i (ELA5625EU)', N'Xiaomi', 2440000, 2074000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/01/08/ela5625eu.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH399')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH399', N'Màn hình MSI PRO MP242L', N'MSI', 1750000, 1487500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/12/02/mp242l-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH400')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH400', N'Màn hình gaming MSI MAG 255XF', N'MSI', 3900000, 3315000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/02/27/255xf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH401')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH401', N'Màn hình ViewSonic VA2215-H', N'ViewSonic', 1500000, 1275000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/05/05/va2215-h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH402')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH402', N'Màn hình MSI MAG 275QF E20', N'MSI', 4290000, 3646500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/02/04/mag-275qf-e20-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH403')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH403', N'Màn hình MSI PRO MP251 E14L', N'MSI', 2150000, 1827500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/pro-mp251-e14l-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH404')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH404', N'Màn hình di động VSP VP1560FS1', N'Chính hãng', 1680000, 1428000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/20/vp1560fs1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH405')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH405', N'Màn hình MSI PRO MP275Q E14', N'MSI', 3750000, 3187500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/06/24/mp275q-14-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH406')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH406', N'Màn hình LG UltraGear 25G550B-B (24.5 inch/FHD/IPS/300Hz/1ms)', N'LG', 4790000, 4071500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/man-hinh-choi-game-ultragear-25inch-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH407')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH407', N'Màn hình LG UltraGear 27G550B-B (27 inch/FHD/IPS/300Hz/1ms)', N'LG', 5249000, 4461650, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/man-hinh-choi-game-ultragear-27inch-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH408')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH408', N'Màn hình ASUS ProArt PA247CV', N'Asus', 4590000, 3901500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/04/27/pa247cv-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH409')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH409', N'Màn hình ASUS ProArt PA248QV', N'Asus', 4790000, 4071500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/04/27/pa248qv-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH410')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH410', N'Màn hình ASUS ProArt PA278QV', N'Asus', 7290000, 6196500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/04/27/pa278qv-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH411')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH411', N'Màn hình Dell 27 (P2723D) - (27"/QHD/IPS/60Hz)', N'Dell', 6580000, 5593000, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2025/05/29/p2723d.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH412')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH412', N'Màn hình máy tính Samsung ViewFinity S8 LS27B800PXEXXV 27 inch 4K/IPS', N'Samsung', 7000000, 5950000, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2022/08/31/image-removebg-preview-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH413')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH413', N'Màn hình ViewSonic VA2732-H', N'ViewSonic', 2350000, 1997500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2022/10/25/image-removebg-preview-2022-10-25t105515-606.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH414')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH414', N'Màn hình ViewSonic VA3209-2K-MHD (32"/QHD/IPS/75Hz)', N'ViewSonic', 4690000, 3986500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2022/10/27/image-removebg-preview-2022-10-27t153915-410.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH415')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH415', N'Giá đỡ màn hình máy tính North Bayou G40 (22 - 40 inch) màu đen', N'Chính hãng', 740000, 629000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH416')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH416', N'Giá đỡ 2 màn hình máy tính gắn cạnh bàn M052 (17-27 inch)', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH417')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH417', N'Giá đỡ màn hình máy tính North Bayou F80 (17-30 inch)', N'Chính hãng', 399000, 339150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH418')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH418', N'Giá đỡ màn hình máy tính North Bayou F100A (22-35 inch)', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH419')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH419', N'Giá đỡ 2 màn hình máy tính North Bayou F160 (17-27 inch)', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH420')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH420', N'Giá đỡ màn hình máy tính North Bayou F195A (22-32 inch)', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH421')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH421', N'Giá đỡ màn hình máy tính gắn cạnh bàn M051 (17-27 inch)', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH422')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH422', N'Giá đỡ màn hình máy tính kèm giá đỡ laptop M074 (17-32 inch)', N'Chính hãng', 560000, 476000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH423')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH423', N'Giá đỡ màn hình máy tính XL03 (17-32 inch)', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/01/image-removebg-preview-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH424')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH424', N'Màn hình GIGABYTE G27QCA-EK (27"/2K/VA/165Hz)', N'Chính hãng', 5590000, 4751500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/27/image-removebg-preview-25.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH425')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH425', N'Màn hình Acer VG240YS (23.8"/FHD/IPS/165Hz)', N'Acer', 2890000, 2456500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2023/02/01/image-removebg-preview-27.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH426')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH426', N'Màn hình MSI G2712 (27"/FHD/IPS/170Hz)', N'MSI', 2990000, 2541500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2023/03/09/msi-g2712-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH427')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH427', N'Màn hình di động ViewSonic VG1655 (15.6"/FHD/IPS/6.5ms/60Hz)', N'ViewSonic', 4790000, 4071500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2023/05/15/vg1655-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH428')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH428', N'Màn hình di động ViewSonic TD1655', N'ViewSonic', 5800000, 4930000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2023/05/15/td1655-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH429')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH429', N'Màn hình ASUS TUF GAMING VG27VH1B (27"/FHD/VA/144Hz/2 loa)', N'Asus', 3590000, 3051500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/06/22/vg27vh1b-00.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH430')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH430', N'Màn hình ASUS TUF Gaming VG279Q3A (27"/FHD/IPS/180Hz)', N'Asus', 3990000, 3391500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/08/07/vg279q3a-0.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH431')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH431', N'Màn hình MSI G274F (27"/FHD/IPS/180Hz)', N'MSI', 3190000, 2711500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2023/10/19/msi-optix-g274f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH432')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH432', N'Màn hình LG 27MR400-B (27"/FHD/IPS/100Hz)', N'LG', 2500000, 2125000, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2023/12/19/27mr400-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH433')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH433', N'Màn hình MSI PRO MP275 (27"/FHD/IPS/100Hz)', N'MSI', 2380000, 2023000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/02/01/mp275.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH434')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH434', N'Màn hình VSP IP2510W1 (24.5"/FHD/IPS/100Hz)', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/02/20/ip2510w1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH435')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH435', N'Màn hình VSP V2203H (21.5"/FHD/VA/75Hz)', N'Chính hãng', 1390000, 1181500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/02/20/v2203h-black-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH436')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH436', N'Màn hình Gaming VSP IP2718G (27"/FHD/IPS/180Hz)', N'Chính hãng', 2290000, 1946500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/02/20/ip2718g-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH437')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH437', N'Màn hình Dell 27 (E2723H) - (27"/FHD/VA/60Hz)', N'Dell', 3480000, 2958000, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2024/02/21/e2723h.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH438')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH438', N'Màn hình Dell UltraSharp 24 (U2424HE) - (23.8"/FHD/IPS/120Hz)', N'Dell', 6390000, 5431500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2024/02/22/u2424he.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH439')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH439', N'Màn hình Dell UltraSharp 24 (U2424H) - (23.8"/FHD/IPS/120Hz)', N'Dell', 5230000, 4445500, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2024/02/22/u2424h.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH440')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH440', N'Màn hình MSI G2712F (27"/FHD/IPS/180Hz)', N'MSI', 3290000, 2796500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/03/19/g2712f.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH441')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH441', N'Màn hình di động MSI PRO MP161 E2 (15.6"/FHD/IPS/60Hz/Có loa)', N'MSI', 2650000, 2252500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/03/28/msi-pro-mp161-e2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH442')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH442', N'Màn hình VSP IP2518V1 (25"INCH/FHD/Fast IPS/180Hz/0.5ms/Đen)', N'Chính hãng', 2490000, 2116500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/22/ip2518v1-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH443')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH443', N'Màn hình VSP IP2718F1 (27"INCH/FHD/Fast IPS/180Hz/0.5ms/Đen)', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/22/ip2718f1-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH444')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH444', N'Màn hình MSI PRO MP225 (21.5 INCH/FHD/IPS/100HZ/1MS)', N'MSI', 1580000, 1343000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/04/25/mp225-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH445')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH445', N'Màn hình Gaming Xiaomi G34WQi (ELA5454EU)', N'Xiaomi', 6850000, 5822500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/05/03/g34wqi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH446')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH446', N'MÀN HÌNH VSP IP2706SG (27 INCH/FHD/IPS/100HZ/1MS)', N'Chính hãng', 2090000, 1776500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/16/man-hinh-vsp-ip2706sg-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH447')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH447', N'Màn hình VSP IP2510W2 (24.5 INCH/FHD/FAST IPS/180HZ/1MS)', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/30/man-hinh-vsp-ip2510w2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH448')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH448', N'Màn hình gaming ViewSonic VX2528', N'ViewSonic', 2990000, 2541500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/06/03/gaming-viewsonic-vx2528-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH449')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH449', N'Màn hình gaming ViewSonic VX2528J', N'ViewSonic', 2990000, 2541500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/06/03/gaming-viewsonic-vx2528j-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH450')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH450', N'Màn hình MSI PRO MP341CQW', N'MSI', 6290000, 5346500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/06/04/msi-pro-mp341cqw-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH451')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH451', N'Màn hình LG 27GS50F-B', N'LG', 2800000, 2380000, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/06/28/27gs50f-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH452')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH452', N'Màn hình Gaming MSI G244F E2', N'MSI', 2490000, 2116500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/01/08/g244f-e2.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH453')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH453', N'Màn Hình VSP IP2420H', N'Chính hãng', 1650000, 1402500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/30/ip2420h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH454')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH454', N'Màn hình gaming EDRA EGM25F100P', N'Chính hãng', 1650000, 1402500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/30/edra-egm25f100p-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH455')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH455', N'Màn hình MSI PRO MP275Q', N'MSI', 3350000, 2847500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/08/03/msi-pro-mp275q-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH456')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH456', N'LG UltraGear 24GS60F-B', N'LG', 3050000, 2592500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/08/20/24gs60f-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH457')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH457', N'Màn hình LG UltraGear 27GS60F-B', N'LG', 3300000, 2805000, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/08/20/27gs60f-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH458')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH458', N'LG UltraGear 27GS75Q-B', N'LG', 5790000, 4921500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/08/20/27gs75q-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH459')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH459', N'Màn hình MSI PRO MP251 E2', N'MSI', 2050000, 1742500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/01/08/mp251-e2.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH460')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH460', N'Giá treo 2 màn hình North Bayou G35 màu xám (22-32inch)', N'Chính hãng', 899000, 764150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/30/nb-g35-xam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH461')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH461', N'Giá treo 2 màn hình gắn bàn EZ2 (10 inch - 27 inch)', N'Chính hãng', 399000, 339150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/30/ez2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH462')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH462', N'Giá treo màn hình North Bayou M2 Màu xám (17-27 inch)', N'Chính hãng', 349000, 296650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/30/nb-m2-xam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH463')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH463', N'Màn hình EDRA EGM24F100H', N'Chính hãng', 1650000, 1402500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/06/egm24f100h-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH464')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH464', N'Màn hình VSP VU279Q1', N'Chính hãng', 4490000, 3816500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/09/vsp-vu279q1-pink-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH465')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH465', N'Màn hình Gaming MSI MAG 275F', N'MSI', 2990000, 2541500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/09/20/mag-275f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH466')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH466', N'Màn Hình Gaming MSI MAG 275QF', N'MSI', 4190000, 3561500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/09/20/mag-275qf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH467')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH467', N'Màn hình VSP IP2512S1', N'Chính hãng', 1850000, 1572500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/08/ip2512s1.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH468')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH468', N'Giá treo 2 màn hình gắn bàn EZ3 (17-27inch)', N'Chính hãng', 460000, 391000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/28/ez3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH469')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH469', N'Màn hình Samsung Odyssey G5 G55C LS32CG552EEXXV', N'Samsung', 5350000, 4547500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2024/12/02/ls32cg552eexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH470')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH470', N'Màn hình ViewSonic VA2708-2K-MHD', N'ViewSonic', 3450000, 2932500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/12/02/va2708-2k-mhd-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH471')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH471', N'Màn hình ViewSonic VX2758A-2K-PRO-2', N'ViewSonic', 4650000, 3952500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/12/02/vx2758a-2k-pro-2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH472')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH472', N'Màn hình ViewSonic VX2758A-2K-PRO-3', N'ViewSonic', 5900000, 5015000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/12/02/vx2758a-2k-pro-3-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH473')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH473', N'Màn Hình LG 24MS550-B', N'LG', 2100000, 1785000, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/12/09/24ms550-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH474')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH474', N'Màn hình LG 27MS500-B', N'LG', 2670000, 2269500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/12/09/27ms500-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH475')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH475', N'Màn hình LG UltraGear 27GS60QC-B', N'LG', 3990000, 3391500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/12/09/27gs60qc-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH476')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH476', N'Màn hình LG UltraGear 27GS65F-B', N'LG', 3650000, 3102500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2024/12/16/27gs65f-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH477')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH477', N'Màn hình MSI MAG 276CXF', N'MSI', 3690000, 3136500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/12/10/276cxf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH478')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH478', N'Màn hình di động ViewSonic VX1654', N'ViewSonic', 4490000, 3816500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2024/12/27/viewsonic-vx1654-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH479')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH479', N'Màn hình VSP IP2518S2', N'Chính hãng', 2290000, 1946500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/04/ip-2518s2-1-1-2-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH480')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH480', N'Màn hình VSP IP2518S4', N'Chính hãng', 2580000, 2193000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/04/vsp-ip2518s4-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH481')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH481', N'Màn hình VSP G2718Q1', N'Chính hãng', 3590000, 3051500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/04/g2718q1-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH482')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH482', N'Màn hình VSP V2510F1', N'Chính hãng', 1580000, 1343000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/04/vsp-v2510f1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH483')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH483', N'Màn hình EDRA EGM27Q165P', N'Chính hãng', 3450000, 2932500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/egm27q165p.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH484')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH484', N'Màn hình EDRA EGM27U60P', N'Chính hãng', 3790000, 3221500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/egm27u60p-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH485')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH485', N'Màn hình EDRA EGM27F240P', N'Chính hãng', 2800000, 2380000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/egm27f240p.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH486')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH486', N'Màn hình Edra EGM27Q100P', N'Chính hãng', 2900000, 2465000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/egm27q100p-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH487')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH487', N'Màn hình EDRA EGM27F240VA', N'Chính hãng', 2400000, 2040000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/07/egm27f240va.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH488')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH488', N'Màn hình EDRA EGM25F180P', N'Chính hãng', 1900000, 1615000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/555-25f100p-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH489')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH489', N'Màn hình Edra EGM27F180PV', N'Chính hãng', 2300000, 1955000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/05/egm27f180pv-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH490')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH490', N'Màn hình MSI PRO MP225V', N'MSI', 1480000, 1258000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/02/11/mp225v-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH491')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH491', N'Màn hình ASUS VY279HGR', N'Asus', 2490000, 2116500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/02/17/vy249hgr-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH492')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH492', N'Màn hình ASUS TUF VG27AQ1A', N'Asus', 4590000, 3901500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/02/17/vg27aq1a-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH493')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH493', N'Màn hình gaming AOC 27G11E/74', N'Chính hãng', 3290000, 2796500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/21/27g11ze274.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH494')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH494', N'Màn hình Gaming AOC 24G4E/74', N'Chính hãng', 2690000, 2286500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/20/24g4e74-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH495')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH495', N'Màn hình di động AOC 16T10/74', N'Chính hãng', 2790000, 2371500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/20/16t1074-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH496')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH496', N'Màn hình Dell E2425H', N'Dell', 2680000, 2278000, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2025/02/25/e2425h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH497')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH497', N'Màn hình Dell E2725H', N'Dell', 2800000, 2380000, 50, N'Chiếc', N'DELL', N'https://i.hoanghamobile.vn/Uploads/2025/02/25/e2725h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH498')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH498', N'Màn hình Gaming LG 27GR93U-B', N'LG', 9650000, 8202500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/02/27/27gr93u-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH499')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH499', N'Màn hình Xiaomi A27Qi (ELA5812EU)', N'Xiaomi', 3590000, 3051500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/03/04/p27qca-ragl.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH500')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH500', N'Màn hình gaming EDRA EGM25F200H (24.5 inch/FHD/IPS/200Hz)', N'Chính hãng', 2000000, 1700000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/egm25f200h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH501')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH501', N'Màn hình ViewSonic VA2414-H', N'ViewSonic', 1800000, 1530000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/03/25/va2414-h-0.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH502')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH502', N'Màn hình Lenovo L22e-40 (67AFKACBVN)', N'Lenovo', 1690000, 1436500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/04/18/67afkacbvn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH503')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH503', N'Màn hình Lenovo L27q-4A -67BFGAC6VN', N'Lenovo', 4290000, 3646500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/04/18/67bfgac6vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH504')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH504', N'Màn hình Lenovo Legion Y27qf-30-67A7GAC3VN', N'Lenovo', 7290000, 6196500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/04/18/67a7gac3vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH505')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH505', N'Màn hình ViewSonic VX2457A-HD-PRO', N'ViewSonic', 2290000, 1946500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/04/24/vx2457a-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH506')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH506', N'Màn hình EDRA EGM24F120PV (23.8 inch/FHD/IPS/120Hz/1ms)', N'Chính hãng', 1750000, 1487500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/egm24f100pv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH507')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH507', N'Màn hình MSI MAG 274QF X24', N'MSI', 5100000, 4335000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/07/01/mag-274qf-x24-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH508')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH508', N'Màn hình di động ViewSonic VA1650', N'ViewSonic', 2400000, 2040000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/07/25/va1650-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH509')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH509', N'Màn hình MSI PRO MP272L', N'MSI', 2250000, 1912500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/08/04/mp272l.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH510')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH510', N'Màn hình ViewSonic VA2532-H', N'ViewSonic', 1950000, 1657500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/08/08/va2532-h-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH511')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH511', N'Màn hình ASUS TUF VG259QMR5A', N'Asus', 4390000, 3731500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/08/08/vg259qmr5a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH512')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH512', N'Màn hình Asus TUF VG27AQ5A', N'Asus', 4990000, 4241500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/08/08/vg27aq5a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH513')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH513', N'Màn hình Gaming MSI MAG 244C', N'MSI', 2250000, 1912500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/09/03/mag-244c-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH514')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH514', N'Màn hình Asus TUF VG279Q5R', N'Asus', 3100000, 2635000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/09/03/vg279q5r.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH515')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH515', N'Màn hình Samsung LS24D300GAEXXV', N'Samsung', 2070000, 1759500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ls24d300gaexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH516')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH516', N'Màn hình Samsung LS27D300GAEXXV', N'Samsung', 2570000, 2184500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ls27d300gaexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH517')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH517', N'Màn hình Samsung LS34C500GAEXXV', N'Samsung', 6250000, 5312500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ls34c500gaexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH518')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH518', N'Màn hình Samsung Odyssey G3 G30D LS24DG302EEXXV', N'Samsung', 2450000, 2082500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ls24dg302eexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH519')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH519', N'Màn hình Samsung Odyssey G3 G30D LS27DG302EEXXV', N'Samsung', 2790000, 2371500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ls27dg302eexxv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH520')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH520', N'Màn hình ViewSonic VG3820C', N'ViewSonic', 11590000, 9851500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/09/17/vg3820c-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH521')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH521', N'Màn hình LG UltraWide 29U531A-W', N'LG', 4750000, 4037500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/10/01/29u531a-w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH522')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH522', N'Màn Hình LG 27UP850K-W', N'LG', 8590000, 7301500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/10/01/27up850k-w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH523')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH523', N'Màn Hình LG UltraGear 27GP750-B', N'LG', 3950000, 3357500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/10/01/27gp750-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH524')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH524', N'Màn hình ASUS TUF VG279QM5A', N'Asus', 3550000, 3017500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/10/01/vg279qm5a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH525')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH525', N'Màn hình di động ASUS ZenScreen MB169CK', N'Asus', 2650000, 2252500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/10/01/mb169ck-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH526')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH526', N'Màn hình ASUS TUF VG259Q5A', N'Asus', 2690000, 2286500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/10/02/vg259q5a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH527')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH527', N'Màn hình Gaming MSI MAG 245F X24', N'MSI', 2750000, 2337500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/10/08/mag-245f-x24-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH528')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH528', N'Màn hình MSI PRO MP272LKL', N'MSI', 2240000, 1904000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/10/13/mp272lkl-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH529')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH529', N'Màn hình MSI PRO MP273L E14', N'MSI', 2470000, 2099500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/10/13/mp273l-e14-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH530')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH530', N'Màn hình MSI PRO MP243L E14', N'MSI', 2100000, 1785000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/10/13/mp243l-e14-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH531')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH531', N'Màn hình di động MSI Pro MP165 E6 (15.6 inch/FHD/IPS/60Hz/4ms/loa/USB-C)', N'MSI', 2550000, 2167500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/11/19/mp165-e6-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH532')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH532', N'Màn hình Gaming MSI MAG 275CQF E18', N'MSI', 3790000, 3221500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/11/19/mag-275cqf-e18-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH533')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH533', N'Màn hình MSI MAG 255F E20', N'MSI', 2650000, 2252500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/12/01/msi-mag-255f-e20-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH534')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH534', N'Xiaomi A27Qi 2026 (ELA6568EU)', N'Xiaomi', 3690000, 3136500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/12/26/ela6568eu-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH535')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH535', N'Màn hình Asus ProArt PA248QFV', N'Asus', 4590000, 3901500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/12/26/pa248qfv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH536')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH536', N'Màn hình Asus ProArt PA278QGV', N'Asus', 8480000, 7208000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/12/26/pa278qgv-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH537')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH537', N'Màn hình VSP IP2408SG', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/30/ip2408sg.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH538')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH538', N'Màn hình ViewSonic VX2479A-HD-Pro', N'ViewSonic', 2590000, 2201500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/02/04/vx2479a-hd-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH539')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH539', N'Màn hình Gigabyte GS24F14', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/26/imghunt-gigabyte-20260226-47380.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH540')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH540', N'Màn hình Gigabyte GS27FA', N'Chính hãng', 2700000, 2295000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/26/imghunt-gigabyte-20260226-40173.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH541')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH541', N'Màn hình Gigabyte GS25F2A', N'Chính hãng', 2690000, 2286500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/26/imghunt-gigabyte-20260226-46067.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH542')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH542', N'Màn hình Gigabyte G25F2A', N'Chính hãng', 3000000, 2550000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/26/imghunt-gigabyte-20260226-46312.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH543')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH543', N'Màn hình MSI PRO MP225 E12VL', N'MSI', 1580000, 1343000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/03/05/pro-mp225-e12vl-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH544')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH544', N'Màn hình LG 22U401A-B', N'LG', 2090000, 1776500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/22u401a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH545')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH545', N'Màn hình LG 24U411A-B', N'LG', 2270000, 1929500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/24u411a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH546')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH546', N'Màn hình LG 27U411A-B', N'LG', 2770000, 2354500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/27u411a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH547')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH547', N'Màn hình Gaming LG UltraGear 27G440A-B', N'LG', 3650000, 3102500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/27g440a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH548')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH548', N'Màn hình MSI MAG 255F X24', N'MSI', 2870000, 2439500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/03/14/mag-255f-x24-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH549')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH549', N'Màn hình LG UltraGear 27G610A-B', N'LG', 5190000, 4411500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/27g610a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH550')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH550', N'Màn hình LG UltraGear 27G640A-B', N'LG', 6990000, 5941500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/03/20/27g640a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH551')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH551', N'Giá Đỡ Màn Hình VSP VA80-1G (17 - 32 inch - 9KG)', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/23/vsp-va80-1g.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH552')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH552', N'Giá Đỡ Màn Hình ARM Đôi VSP VA80-2G (17 - 32 inch - 9KG)', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/23/vsp-va80-1g.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH553')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH553', N'Giá đỡ màn hình VSP VG70-1G (17 - 42 inch - 13KG)', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/23/vsp-vg70-1g-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH554')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH554', N'Giá đỡ màn hình VSP VG60-1G (17- 42 inch - 13KG)', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/23/vsp-vg60-1g-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH555')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH555', N'Màn hình Asus TUF VG249Q5R', N'Asus', 2550000, 2167500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/vg249q5r-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH556')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH556', N'Màn hình di động VSP VP1560FST1', N'Chính hãng', 3000000, 2550000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/20/vp1560fst1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH557')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH557', N'Màn hình di động VSP GP1614WS1', N'Chính hãng', 3000000, 2550000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/20/gp1614ws1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH558')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH558', N'Màn hình MSI MAG 272F X24', N'MSI', 3290000, 2796500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/05/20/msi-mag-272f-x24-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH559')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH559', N'Màn hình ViewSonic VX2536A', N'ViewSonic', 3890000, 3306500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vx2536a-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH560')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH560', N'Màn hình ViewSonic VX24G30-W', N'ViewSonic', 2890000, 2456500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vx24g30-w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH561')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH561', N'Màn Hình ViewSonic VP2766-2K', N'ViewSonic', 6790000, 5771500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vp2766-2k-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH562')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH562', N'Màn hình ViewSonic VX24G30-PK', N'ViewSonic', 3000000, 2550000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vx24g30-pk-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH563')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH563', N'Màn hình ViewSonic VX24G30-BL', N'ViewSonic', 3000000, 2550000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vx24g30-bl-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH564')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH564', N'Màn Hình ViewSonic VP2766-2K-W', N'ViewSonic', 6790000, 5771500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/viewsonic-vp2766-2k-w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH565')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH565', N'Màn hình ViewSonic VX2425-HD-PRO', N'ViewSonic', 2250000, 1912500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/05/28/man-viewsonic-vx2425-hd-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH566')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH566', N'Màn hình ViewSonic XG2735-2K', N'ViewSonic', 4750000, 4037500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/07/14/xg2735-2k-1-171433461.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH567')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH567', N'Màn hình LG UltraGear 27G523B-B', N'LG', 3649000, 3101650, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/lg-ultragear-27g523b-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH568')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH568', N'Màn hình LG UltraWide 34U511A-B', N'LG', 6590000, 5601500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/34u511a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH569')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH569', N'Màn hình LG 32U631A-B', N'LG', 5690000, 4836500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/32u631a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH570')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH570', N'Màn hình ViewSonic TD1617A-2K', N'ViewSonic', 6790000, 5771500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/06/30/td1617a-2k-1-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH571')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH571', N'Màn hình ViewSonic VA27G1-H', N'ViewSonic', 2490000, 2116500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/09/30/viewsonic-va27g1-h-1-102351294.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH572')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH572', N'Màn hình gaming ViewSonic VX24G31A', N'ViewSonic', 3000000, 2550000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/09/29/viewsonic-vx24g31a-1-172139411.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH573')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH573', N'Màn hình kép di động ViewSonic 14 inch VG1457', N'ViewSonic', 7890000, 6706500, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2026/09/30/viewsonic-14-inch-vg1457-1-102144235.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH574')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH574', N'Màn hình MSI MAG 244F', N'MSI', 2590000, 2201500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/05/12/mag-244f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH575')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH575', N'Màn hình gaming ViewSonic VX2758A-2K-PRO-4', N'ViewSonic', 4400000, 3740000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/05/16/vx2758a-2k-pro-4-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH576')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH576', N'Màn hình ViewSonic VA2708-4K-MHD', N'ViewSonic', 5100000, 4335000, 50, N'Chiếc', N'VIEWSONIC', N'https://i.hoanghamobile.vn/Uploads/2025/05/19/va2708-4k-mhd-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH577')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH577', N'Màn hình Xiaomi A27UI (ELA6221EU)', N'Xiaomi', 6760000, 5746000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/a27ui-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH578')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH578', N'Màn Hình ASUS VZ24EHF', N'Asus', 2330000, 1980500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/vz24ehf-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH579')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH579', N'Màn hình ASUS VA249HG', N'Asus', 2150000, 1827500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/va249hg-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH580')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH580', N'Màn hình ASUS VA279HG', N'Asus', 2650000, 2252500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/va279hg-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH581')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH581', N'Màn hình VSP IP2405F', N'Chính hãng', 1650000, 1402500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/ip2405f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH582')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH582', N'Màn hình VSP IP2705F', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/ip2705f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH583')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH583', N'Màn hình VSP G2710Q1', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/g2710q1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH584')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH584', N'Màn hình LG UtraGear 27GS95QE-B', N'LG', 15890000, 13506500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/27gs95qe-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH585')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH585', N'Màn hình LG UltraGear 45GS95QE-B', N'LG', 29490000, 25066500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/05/28/45gs95qe-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH586')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH586', N'Màn Hình LG 27UP600K-W', N'LG', 5490000, 4666500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/05/31/27up600k-w-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH587')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH587', N'Màn hình MSI Pro MP251L E2', N'MSI', 1990000, 1691500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/06/10/mp251l-e2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH588')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH588', N'Màn hình MSI MAG 274F', N'MSI', 3100000, 2635000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2025/06/10/mag-274f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH589')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH589', N'Màn hình VSP IP2205F', N'Chính hãng', 1380000, 1173000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/06/21/ip2205f-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH590')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH590', N'Màn hình VSP IP2404S', N'Chính hãng', 1700000, 1445000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/06/21/ip2404s-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH591')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH591', N'Màn hình VSP IP2702S', N'Chính hãng', 1900000, 1615000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/06/21/ip2702s-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH592')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH592', N'Màn hình LG 24U631A-B', N'LG', 3990000, 3391500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/06/28/24u631a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH593')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH593', N'Màn hình LG 27U631A-B', N'LG', 3980000, 3383000, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2025/06/28/27u631a-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH594')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH594', N'Màn hình LG UltraGear 25G523B-B (24.5 inch/FHD/IPS/200Hz/1ms)', N'LG', 3190000, 2711500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/man-hinh-choi-game-ultragear-25inch-1_639179799458077623.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH595')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH595', N'Màn hình di động VSP VP1560FS1 (15.6 inch/FHD/IPS/62HZ/5MS)', N'Chính hãng', 1680000, 1428000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/29/man-hinh-portable-vsp-vp1560fs1-1_639183459850295351.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH596')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH596', N'Màn hình MSI PRO MP341CQW E12', N'MSI', 5990000, 5091500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/04/15/msi-pro-mp341-cq-e12-trang.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH597')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH597', N'Màn hình Xiaomi A24i 2026 (ELA6416EU)', N'Xiaomi', 2290000, 1946500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/04/15/man-hinh-xiaomi-a24i-2026-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH598')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH598', N'Màn hình MSI PRO MP341CQ E12', N'MSI', 5950000, 5057500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/04/15/msi-pro-mp341cq-e12-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH599')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH599', N'Xiaomi A27i 2026 (ELA6410EU)', N'Xiaomi', 2490000, 2116500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/03/30/ela6410eu-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH600')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH600', N'Màn hình MSI MAG 342CQRF E20 (34 inch/UWQHD/Rapid VA/200Hz/0.5ms)', N'MSI', 7790000, 6621500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/07/01/man-hinh-msi-mag-342cqrf-e20-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH601')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH601', N'Màn hình MSI MD342CQP (34 inch/UWQHD/VA/120Hz/1ms/loa/USB-C/cong/Đen)', N'MSI', 7490000, 6366500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/07/01/man-hinh-msi-md342cqp-black-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH602')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH602', N'Màn hình MSI MD342CQPW (34 inch/UWQHD/VA/120Hz/1ms/loa/USB-C/cong/Trắng)', N'MSI', 7490000, 6366500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/07/01/man-hinh-msi-md342cqpw-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH603')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH603', N'PC Mini MSI Cubi NUC AI+ 2MG-019BVN (936-B20611-019)', N'MSI', 21000000, 17850000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/10/2mg-019bvn-5-100337539.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH604')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH604', N'PC MSI PRO DP180 I5 (9S6-B0A741-1482)', N'MSI', 21990000, 18691500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/10/msi-pro-dp180-i5-9s6-b0a741-1482-1-111909999.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH605')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH605', N'Mac Mini M4 2024 16GB/512GB', N'Chính hãng', 25490000, 21666500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/04/mac-mini-m4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH606')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH606', N'PC GAMING MSI MAG Infinite E1 AI Gaming 14-238VN 9S6-B91111-238', N'MSI', 28990000, 24641500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/08/ninite-e1-3-112032182.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH607')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH607', N'PC Mini MSI Cubi NUC AI 1UMG 936-B20911-062', N'MSI', 17990000, 15291500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/08/msi-cubi-nuc-ai-1umg-936-b20911-062-5-112302464.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH608')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH608', N'PC Mini MSI Cubi NUC AI 1UMG 936-B20911-069', N'MSI', 13990000, 11891500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/08/msi-cubi-nuc-ai-1umg-936-b20911-062-5-112302464.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH609')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH609', N'PC Msi Cubi NUC AI + 2MG-020BVN (936-B20611-020)', N'MSI', 16590000, 14101500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/10/2mg-019bvn-5-100337539.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH610')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH610', N'PC Mini MSI CUBI NUC AI 1UMG-016BVN (936-B20911-016)', N'MSI', 14990000, 12741500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/08/msi-cubi-nuc-ai-1umg-936-b20911-062-5-112302464.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH611')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH611', N'Mac Mini M6 12CPU 12GPU 16GB/256GB', N'Chính hãng', 24490000, 20816500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-mini-m6-pdp-image-position-1-vn-vi-111221082.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH612')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH612', N'Bộ Bàn phím, Chuột không dây Bluetooth Rapoo 8000M - Chính hãng', N'Chính hãng', 370000, 314500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/21/8000m-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH613')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH613', N'Bàn phím cơ gaming Rapoo V530', N'Chính hãng', 850000, 722500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/06/v530-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH614')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH614', N'Bàn phím cơ Gaming Rapoo V500 Pro (Vàng Xanh/Brown Switch)', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/10/v500-pro-blue-yellow.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH615')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH615', N'Bàn phím cơ Gaming Rapoo V500 Pro (Trắng Cerulean/Blue Switch)', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/10/v500-pro-cerulean.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH616')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH616', N'Bàn phím cơ Gaming Rapoo V500 Pro (Hồng Trắng/Brown Switch)', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/10/v500-pro-pink-white.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH617')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH617', N'Bàn phím có dây Rapoo NK1900', N'Chính hãng', 149000, 126650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/12/27/nk1900-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH618')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH618', N'Bàn phím có dây Rapoo NK1800 - Chính hãng', N'Chính hãng', 140000, 119000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/21/nk1800-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH619')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH619', N'RAM Laptop Lexar 16GB DDR5 4800MHz (LD5DS016G-B4800GSST)', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/14/ddr5sodimm-slider-16gb-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH620')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH620', N'Chuột không dây Elecom M-IR07DR - Chính hãng', N'Chính hãng', 179000, 152150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/11/25/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH621')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH621', N'Chuột Gaming có dây Rapoo V16 Pro', N'Chính hãng', 268000, 227800, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/14/rapoo-v16-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH622')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH622', N'Chuột chơi game có dây ASUS TUF M3 Gen II', N'Asus', 420000, 357000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/09/19/tuf-m3-gen2-0.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH623')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH623', N'Chuột không dây Rapoo M219 Silent', N'Chính hãng', 160000, 136000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/22/rapoo-m219-silent-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH624')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH624', N'Chuột gaming không dây Rapoo V30W', N'Chính hãng', 710000, 603500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/06/v30w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH625')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH625', N'Bàn phím cơ Gaming Rapoo V500 Pro (Đen)', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/09/7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH626')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH626', N'Bàn phím cơ Gaming Rapoo V500 Pro (Trắng Xanh/Blue Switch)', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/10/v500-pro-blue-white_638325245831566833.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH627')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH627', N'iMac M6 24 inch 2026 12CPU 12GPU 24GB/512GB', N'Chính hãng', 57990000, 49291500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/345544_imac-m6-24-inch-2026-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH628')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH628', N'iMac M6 24 inch 2026 12CPU 12GPU 16GB/512GB', N'Chính hãng', 52490000, 44616500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/345544_imac-m6-24-inch-2026-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH629')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH629', N'iMac M6 24 inch 2026 12CPU 12GPU 16GB/256GB Nano', N'Chính hãng', 52490000, 44616500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/345544_imac-m6-24-inch-2026-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH630')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH630', N'iMac M6 24 inch 2026 12CPU 12GPU 16GB/256GB', N'Chính hãng', 46990000, 39941500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/345544_imac-m6-24-inch-2026-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH631')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH631', N'iMac M6 24 inch 2026 10CPU 10GPU 16GB/256GB', N'Chính hãng', 41490000, 35266500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/28/Gallery/345544_imac-m6-24-inch-2026-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH632')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH632', N'Microsoft 365 Bus Std Retail All Languages 1YR (KLQ-00209)', N'Chính hãng', 3890000, 3306500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/28/microsoft-365-klq-00209.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH633')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH633', N'Microsoft Office Professional 2021 Online (269-17185)', N'Chính hãng', 9490000, 8066500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/28/microsoft-office-professional-2021.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH634')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH634', N'Microsoft Windows Home 11 64bit All Lng (KW9-00664)', N'Chính hãng', 3290000, 2796500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/31/microsoft-windows-home-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH635')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH635', N'Ram Laptop Kingston (KVR48S40BD8-32) 32GB (1x32GB) DDR5 4800MHz', N'Chính hãng', 3190000, 2711500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/13/kingston-kvr48s40bd8-32-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH636')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH636', N'Chuột Gaming không dây Rapoo VT200 Wireless', N'Chính hãng', 679000, 577150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/12/27/vt200-1_638392724225174239.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH637')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH637', N'Microsoft Office 365 Personal + AI Copilot', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/23/en-m365-personal-holidayfy24-posa-fob-rgb-1157.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH638')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH638', N'RAM Laptop Kingston (KVR48S40BS6-8) 8GB (1X8GB) DDR5 4800MHZ - Chính hãng', N'Chính hãng', 1890000, 1606500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/16/image-removebg-preview-54.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH639')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH639', N'Microsoft Office 365 Family + AI Copilot (12 tháng X 6 User)', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/23/en-m365-family-holidayfy24-posa-x23-45163-01-fob-rgb-1157.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH640')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH640', N'Ram Laptop Corsair Vengeance (CMSX16GX5M1A4800C40) 16GB (1x16GB) DDR5', N'Chính hãng', 2090000, 1776500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/21/image-removebg-preview-97.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH641')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH641', N'Ram Laptop Lexar (LD4AS016G-B3200GSST) 16GB (1x16GB) DDR4 3200Mhz', N'Chính hãng', 950000, 807500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/24/image-removebg-preview-2022-10-24t161355-608.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH642')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH642', N'Ram Laptop Lexar (LD4AS032G-B3200GSST) 32GB (1x32GB) DDR4 3200Mhz', N'Chính hãng', 1780000, 1513000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/24/image-removebg-preview-2022-10-24t162016-200.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH643')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH643', N'Ổ cứng SSD WD SN570 Blue 1TB M.2 2280 PCIe NVMe 3x4 - Chính hãng', N'Chính hãng', 2280000, 1938000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/29/image-removebg-preview-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH644')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH644', N'Ổ cứng SSD Lexar NM610PRO M.2 2280 PCIe Gen3x4 NVMe (1TB) - Chính hãng', N'Chính hãng', 1150000, 977500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/28/nm610pro-slider-1tb-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH645')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH645', N'Ổ cứng SSD Lexar NM610PRO M.2 2280 PCIe Gen3x4 NVMe (500GB) - Chính hãng', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/28/nm610pro-slider-500gb-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH646')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH646', N'RAM Laptop Lexar 16GB DDR5 5600MHz (LD5S16G56C46ST-BGS)', N'Chính hãng', 1890000, 1606500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/03/11/80576-ram-laptop-lexar-ld5ds016g-b4800gsst-16gb-1x16gb-ddr5-4800mhz-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH647')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH647', N'Microsoft Win Pro 11 64b All-LngPK Lic Online (FQC-10572)', N'Chính hãng', 5130000, 4360500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/31/microsoft-win-pro-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH648')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH648', N'Office Home 2024', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/24/office-home-2024-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH649')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH649', N'Ổ cứng SSD MSI SPATIUM M450 500GB', N'MSI', 1490000, 1266500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/o-cung-ssd-msi-spatium-m450-500gb-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH650')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH650', N'Ổ cứng SSD MSI SPATIUM M450 1TB', N'MSI', 2290000, 1946500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/o-cung-ssd-msi-spatium-m450-500gb-5.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH651')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH651', N'Ổ cứng SSD MSI SPATIUM S270 480GB', N'MSI', 830000, 705500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/msi-s270-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH652')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH652', N'Ổ cứng SSD MSI SPATIUM S270 960GB', N'MSI', 1600000, 1360000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/11/13/1024.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH653')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH653', N'Ổ cứng SSD MSI SPATIUM S270 240GB SATA III 2.5 inch', N'MSI', 490000, 416500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/11/16/o-cung-ssd-msi-spatium-s270-240gb-sata-iii-2-5-inch-1.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH654')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH654', N'Ổ cứng di động HDD Seagate One Touch 2TB STKY2000400', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/18/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH655')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH655', N'Ổ cứng di động HDD Seagate One Touch 2TB STKY2000404', N'Chính hãng', 2299000, 1954150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/18/image-removebg-preview-22.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH656')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH656', N'Ổ cứng di động SEAGATE ONE TOUCH 2TB 2.5 USB 3.0 - ĐỎ STKY2000403', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/seagate-one-touch-red-ha5.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH657')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH657', N'Ổ cứng di động SEAGATE ONE TOUCH 2TB 2.5 USB 3.0 - Hồng STKY2000405', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/11/seagate-one-touch-pink-ha1.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH658')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH658', N'Phủ phím Innostyle keyguard navigator cho Macbook Pro 13" (M1 2020)', N'Apple', 79000, 67150, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2022/02/14/image-removebg-preview-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH659')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH659', N'Phủ phím Innostyle keyguard navigator cho Macbook Air 13" (M1 2020)', N'Apple', 79000, 67150, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2022/02/14/image-removebg-preview-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH660')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH660', N'Bộ vệ sinh JRC 5in1 Multi-Functional Nano Cleaning fluid', N'Chính hãng', 170000, 144500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/16/jrc-5in1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH661')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH661', N'Bộ Vệ Sinh Macbook JRC 2in1', N'Apple', 90000, 76500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/05/16/jrc-2in1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH662')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH662', N'Chuột chơi game có dây ASUS TUF M3 - Chính hãng', N'Asus', 340000, 289000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2022/12/31/image-removebg-preview-47.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH663')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH663', N'Chuột Chơi game Asus ROG Gladius II Core (P507) (USB/RGB/Black)', N'Asus', 760000, 646000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/01/04/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH664')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH664', N'Chuột Gaming Asus ROG Keris Wireless AimPoint - Chính hãng', N'Asus', 2250000, 1912500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/05/17/p709-1_638199130472226096.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH665')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH665', N'Chuột Gaming không dây Asus ROG Harpe Ace Aim Lab Edition - Chính hãng', N'Asus', 2790000, 2371500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/05/17/p713-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH666')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH666', N'Bàn phím không dây Logitech Pebble keys 2 K380s', N'Logitech', 790000, 671500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/09/12/k380s-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH667')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH667', N'Chuột gaming có dây MSI FORGE GM100', N'MSI', 249000, 211650, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/msi-forge-gm100-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH668')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH668', N'Bàn phím cơ gaming MSI FORGE GK100', N'MSI', 389000, 330650, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/msi-forge-gk100-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH669')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH669', N'Bàn phím cơ gaming MSI FORGE GK300', N'MSI', 980000, 833000, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/msi-forge-gk300-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH670')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH670', N'Chuột không dây Genius Silent NX-8008S', N'Chính hãng', 159000, 135150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/genius-silent-nx-8008s-8.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH671')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH671', N'Chuột ThinkPad Essential Wireless Mouse_4X30M56887', N'Lenovo', 490000, 416500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2024/12/10/4x30m56887-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH672')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH672', N'Bộ bàn phím chuột không dây Logitech MK240 Nano Wireless', N'Logitech', 490000, 416500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/mk240-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH673')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH673', N'Bộ bàn phím chuột không dây Logitech MK220 Wireless', N'Logitech', 420000, 357000, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/mk220-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH674')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH674', N'Bộ bàn phím chuột không dây Logitech MK235 Wireless', N'Logitech', 475000, 403750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/mk235-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH675')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH675', N'Bộ bàn phím chuột không dây Logitech MK345 (920-012926)', N'Logitech', 655000, 556750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/mk345-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH676')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH676', N'Bàn phím không dây Logitech K270 Wireless (USB)', N'Logitech', 415000, 352750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/k270-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH677')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH677', N'Chuột game không dây Logitech G304 Đen (USB)', N'Logitech', 740000, 629000, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/04/01/logitech-g304-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH678')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH678', N'Chuột game Logitech G402 Hyperion Fury (USB/đen)', N'Logitech', 635000, 539750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/04/01/logitech-g402-hyperion-fury-ultra-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH679')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH679', N'Bàn phím giả cơ Logitech G213 Prodigy RGB Gaming', N'Logitech', 920000, 782000, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/04/01/g213-prodigy-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH680')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH680', N'Bàn phím cơ gaming Edra EK398 Alpha  (USB/ABS/Led Rainbow)', N'Chính hãng', 478000, 406300, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/29/ek398-alpha.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH681')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH681', N'Bàn phím không dây EDRA EK507', N'Chính hãng', 546000, 464100, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/edra-ek375v2-alpha.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH682')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH682', N'Bàn phím cơ gaming EDRA EK375v2 Alpha (Linear switch)', N'Chính hãng', 540000, 459000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/edra-ek375v2-alpha_638817919402295017.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH683')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH683', N'Chuột Gaming không dây Edra EM623W (USB/Bluetooth/RGB)', N'Chính hãng', 268000, 227800, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/em623w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH684')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH684', N'Chuột không dây EDRA EM608W (TRẮNG)', N'Chính hãng', 68000, 57800, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/em608w.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH685')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH685', N'Chuột có dây Edra EM606 (USB/Led) (ĐEN)', N'Chính hãng', 77000, 65450, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/em606-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH686')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH686', N'Bàn phím có dây Edra EK506 (USB/Led)', N'Chính hãng', 148000, 125800, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/ek506-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH687')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH687', N'Chuột Edra EM601 v2 đen (USB)', N'Chính hãng', 78000, 66300, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/em601-v2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH688')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH688', N'Chuột không dây Logitech M650 Signature (USB/Bluetooth/Pin AA) Size L', N'Logitech', 650000, 552500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/07/07/signature-m650-1-photoroom.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH689')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH689', N'Chuột không dây Logitech M190', N'Logitech', 290000, 246500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2026/03/25/m190-xanh-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH690')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH690', N'Chuột Gaming có dây MSI Force GM320', N'MSI', 290000, 246500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/04/29/gm320-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH691')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH691', N'Chuột game không dây Logitech G304 Trắng (USB)', N'Logitech', 740000, 629000, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2025/04/01/logitech-g304-trang-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH692')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH692', N'Cáp HDMI to HDMI LENTION HH20-P2 1.5M hỗ trợ 4K - Chính hãng', N'Chính hãng', 125000, 106250, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/22/hh20-p2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH693')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH693', N'Cáp HDMI to HDMI LENTION HH20-P2 2M hỗ trợ 4K - Chính hãng', N'Chính hãng', 140000, 119000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/22/hh20-p2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH694')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH694', N'Cáp chuyển đổi Ugreen HDMI 8K 1m HD175', N'Ugreen', 130000, 110500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2025/04/03/anh-sp-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH695')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH695', N'Cáp chuyển đổi Ugreen HDMI 8K 1.5m HD175', N'Ugreen', 150000, 127500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2025/04/03/anh-sp-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH696')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH696', N'Cáp chuyển đổi Ugreen HDMI 8K 2m HD175', N'Ugreen', 170000, 144500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2025/04/03/anh-sp-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH697')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH697', N'Cáp chuyển đổi Ugreen HDMI 8K 3m HD175', N'Ugreen', 200000, 170000, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2025/04/03/anh-sp-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH698')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH698', N'iMac M4 24 inch 2024 16GB/256GB', N'Chính hãng', 33590000, 28551500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/04/imac-2024.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH699')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH699', N'iMac M4 24 inch 2024 16GB/512GB', N'Chính hãng', 43990000, 37391500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/04/imac-2024.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH700')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH700', N'Mac Studio Chip M4 Max 14CPU - 32GPU - 36GB - 512GB', N'Chính hãng', 57990000, 49291500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/mac-studio-m3-ultra-m4-non-ai-mar25-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH701')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH701', N'Mac Studio Chip M4 Max 16CPU - 40GPU - 48GB - 512GB', N'Chính hãng', 70490000, 59916500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/mac-studio-m3-ultra-m4-non-ai-mar25-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH702')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH702', N'Mac Studio Chip M3 Ultra 28CPU - 60GPU - 96GB - 1TB', N'Chính hãng', 115990000, 98591500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/mac-studio-m3-ultra-m4-non-ai-mar25-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH703')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH703', N'Mac Studio Chip M3 Ultra 32CPU - 80GPU - 96GB - 1TB', N'Chính hãng', 153490000, 130466500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/mac-studio-m3-ultra-m4-non-ai-mar25-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH704')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH704', N'PC Msi Cubi NUC AI+ 2MG-027BVN (936-B20611-027)', N'MSI', 19990000, 16991500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2026/08/10/2mg-019bvn-5-100337539.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH705')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH705', N'Mac Mini M6 12CPU 12GPU 16GB/512GB', N'Chính hãng', 29990000, 25491500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-mini-m6-pdp-image-position-1-vn-vi-111221082.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH706')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH706', N'Mac Mini M6 12CPU 12GPU 24GB/512GB', N'Chính hãng', 35490000, 30166500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-mini-m6-pdp-image-position-1-vn-vi-111221082.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH707')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH707', N'Mac Mini M5 Pro 15CPU 16GPU 24GB/512GB', N'Chính hãng', 47790000, 40621500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-mini-m5-pro-pdp-image-position-1-vn-vi-121615332.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH708')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH708', N'Mac Studio M5 Max 18CPU 32GPU 36GB/512GB', N'Chính hãng', 68990000, 58641500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-studio-m5-max-aug26-pdp-image-position-1-vn-vi-135055428.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH709')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH709', N'Mac Studio M5 Ultra 28CPU 64GPU 96GB/1TB', N'Chính hãng', 158990000, 135141500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/mac-studio-m5-ultra-aug26-pdp-image-position-1-vn-vi-135322468.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH710')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH710', N'Bàn di chuột Apple Magic Trackpad 2', N'Apple', 2990000, 2541500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2022/08/20/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH711')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH711', N'Bàn phím cơ Acer Predator Aethon 301TKL', N'Acer', 590000, 501500, 50, N'Chiếc', N'ACER', N'https://i.hoanghamobile.vn/Uploads/2026/08/07/acer-predator-aethon-301tkl-1-164445754.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH712')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH712', N'Bàn phím Colorful MECH QY98 White', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/09/14/b-n-ph-m-colorful-4-092608110.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH713')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH713', N'Giá đỡ Laptop Havit ST7304', N'Chính hãng', 170000, 144500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/03/gia-do-laptop-havit-st7304-1-091427195.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH714')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH714', N'Bộ dán MacBook Air 13'' 2020 3M INNOSTYLE DIAMOND GUARD 6 IN 1', N'Apple', 199000, 169150, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2021/11/20/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH715')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH715', N'Bộ dán MacBook Pro 13'' 2020 3M INNOSTYLE DIAMOND GUARD 6 IN 1', N'Apple', 199000, 169150, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2021/11/20/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH716')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH716', N'Bộ vệ sinh Laptop/Màn hình Tika 3in1', N'Chính hãng', 110000, 93500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/06/19/image-photoroom.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH717')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH717', N'Dung dịch làm sạch Innostyle Clearguard Hight Gloss cho Laptop', N'Chính hãng', 79000, 67150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/15/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH718')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH718', N'Giá để laptop Orico PFB-A24', N'Chính hãng', 170000, 144500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/14/gia-do-laptop-a3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH719')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH719', N'Chuột không dây Logitech M185 Wireless', N'Logitech', 225000, 191250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/logitech-m185-wireless-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH720')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH720', N'Chuột Bluetooth Silent Logitech M240', N'Logitech', 335000, 284750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/silent-logitech-m240-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH721')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH721', N'Chuột không dây Logitech MX Anywhere 3S', N'Logitech', 1530000, 1300500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/logitech-mx-anywhere-3s-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH722')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH722', N'Chuột game Logitech G102 GEN2', N'Logitech', 390000, 331500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/logitech-g102-gen2-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH723')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH723', N'Chuột không dây Xiaomi Wireless Mouse 3', N'Xiaomi', 490000, 416500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/06/20/xiaomi-wireless-mouse-3-grey-righttop.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH724')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH724', N'Cáp HDMI to HDMI 8K60Hz LENTION HH21-M1 1.5M - Chính hãng', N'Chính hãng', 210000, 178500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/22/hh21-m1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH725')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH725', N'Cáp HDMI to HDMI Baseus Enjoyment Series 4K 0.75M', N'Baseus', 120000, 102000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2022/11/15/image-removebg-preview-85.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH726')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH726', N'iMac M1 2021 24 inch 8GB/512GB', N'Chính hãng', 25990000, 22091500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/04/23/pink.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH727')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH727', N'Quạt tản nhiệt cho laptop H36', N'Chính hãng', 120000, 102000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/14/32672-h36.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH728')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH728', N'Đế tản nhiệt cho laptop N99', N'Chính hãng', 190000, 161500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/14/n99-c2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH729')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH729', N'PHỦ PHÍM INNOSTYLE TPU KEYBOARD TRANSPARENT MACBOOK PRO 14/16" 2021/AIR 13"', N'Apple', 75000, 63750, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/12/30/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH730')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH730', N'Phủ phím Innostyle keyguard Crystal Clear cho Macbook Air 13'''' (M1 2020)', N'Apple', 75000, 63750, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2022/04/12/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH731')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH731', N'Bộ bàn phím kèm chuột không dây MicroPack Lifestyle KM-269W', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/15/micropack-lifestyle-km-269w-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH732')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH732', N'Chuột không dây HP Z3700 - Chính hãng', N'HP', 179000, 152150, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2023/02/08/hp-z3700-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH733')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH733', N'Combo Bàn phím + Chuột không dây Rapoo X1800 Pro - Chính hãng', N'Chính hãng', 310000, 263500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/05/11/2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH734')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH734', N'Cáp HDMI to HDMI 8K60Hz LENTION HH21-M1 3M', N'Chính hãng', 320000, 272000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/05/10/cap-hdmi-to-hdmi-8k60hz-lention-hh21-m1-3m-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH735')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH735', N'Cáp Displayport dài 1.8M Lention DD20-P1-1.8M', N'Chính hãng', 130000, 110500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/03/dis4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH736')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH736', N'Chuột không dây Innostyle wireless mouse Black', N'Chính hãng', 190000, 161500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/11/cava.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH737')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH737', N'Phần mềm diệt virus Kaspersky Plus 5 thiết bị/năm', N'Chính hãng', 900000, 765000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/24/kaspersky-plus.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH738')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH738', N'Phần mềm diệt virus Kaspersky Premium', N'Chính hãng', 440000, 374000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/24/kaspersky-premium.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH739')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH739', N'Phần mềm diệt virus Kaspersky Standard 1 thiết bị/năm', N'Chính hãng', 230000, 195500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/kaspersky-standard.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH740')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH740', N'Phần mềm diệt virus Kaspersky Standard 3 thiết bị/năm', N'Chính hãng', 430000, 365500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/kaspersky-standard.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH741')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH741', N'Phần mềm diệt virus Kaspersky Standard 5 thiết bị/năm', N'Chính hãng', 540000, 459000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/23/kaspersky-standard.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH742')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH742', N'Phần mềm diệt virus Kaspersky Plus 1 thiết bị/năm', N'Chính hãng', 360000, 306000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/24/kaspersky-plus.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH743')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH743', N'Phần mềm diệt virus Kaspersky Plus 3 thiết bị/năm', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/24/kaspersky-plus.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH744')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH744', N'Chuột không dây HYPER HyperSpace Bluetooth Mouse - HS2100GL', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/10/chuot-hyper.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH745')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH745', N'Bàn phím không dây HYPER HyperSpace Bluetooth Wireless Keyboard - HS2310US', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/10/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH746')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH746', N'Bộ chuyển Lention CU808D USB-C sang DisplayPort dài 1.8m (8K 60Hz)', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/03/cu808-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH747')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH747', N'Box Ổ Cứng SSD Hyperdrive Next USB 4 - 40Gbps Nvme SSD Enclosure Black', N'Chính hãng', 3450000, 2932500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/12/09/hd5001gl-hero-v1-r1-2000x2000-1024x1024-600x600-1.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH748')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH748', N'RAM Laptop Kingston (KVR32S22D8-32) 32GB (1X32GB) DDR4 3200MHZ', N'Chính hãng', 1890000, 1606500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/30/kvr32s22d8-32-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH749')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH749', N'Chuột không dây Logitech M650 Signature (USB/Bluetooth/Pin AA) Size M', N'Logitech', 625000, 531250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/logitech-m650-signature-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH750')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH750', N'Chuột Gaming có dây Razer DeathAdder V3 (RZ01-04640100-R3M1) - Chính hãng', N'Chính hãng', 1850000, 1572500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/28/razer-deathadder-v3-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH751')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH751', N'Chuột không dây Rapoo M160 - Chính hãng', N'Chính hãng', 179000, 152150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/16/m160-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH752')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH752', N'Chuột có dây MSI M88 - Chính hãng', N'MSI', 129000, 109650, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2023/05/20/m88-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH753')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH753', N'Chuột có dây MSI M99 - Chính hãng', N'MSI', 470000, 399500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2023/05/20/m99-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH754')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH754', N'Cáp HDMI to HDMI 8K60Hz LENTION HH21-M1 2M', N'Chính hãng', 230000, 195500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/05/10/cap-hdmi-to-hdmi-8k60hz-lention-hh2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH755')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH755', N'Chuột có dây Rapoo N100 (Đen) - Chính hãng', N'Chính hãng', 79000, 67150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/21/n100-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH756')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH756', N'Chuột có dây Logitech B100 (910-006605) (USB) - Chính hãng', N'Logitech', 85000, 72250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2023/03/07/logitechb100-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH757')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH757', N'Ram Laptop G.Skill Ripjaws DDR5 16GB 4800MHz 1.2v F5-4800S4039A16GX1-RS', N'Chính hãng', 1630000, 1385500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/15/ddr5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH758')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH758', N'RAM Laptop G.Skill Ripjaws DDR4 8GB 3200MHz 1.2v F4-3200C22S-8GRS', N'Chính hãng', 680000, 578000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/15/ddr4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH759')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH759', N'Ram Laptop G.Skill Ripjaws DDR4 16GB 3200MHz 1.2v F4-3200C22S-16GRS', N'Chính hãng', 1080000, 918000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/15/ddr4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH760')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH760', N'Ram Laptop G.Skill Ripjaws DDR4 32GB 3200MHz 1.2v F4-3200C22S-32GRS', N'Chính hãng', 1680000, 1428000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/15/ddr4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH761')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH761', N'Ổ cứng SSD WD Green 480GB SATA 2.5 inch WDS480G3G0A', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/08/wds480g3g0a-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH762')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH762', N'Chuột game không dây Asus TUF M4 Wireless (USB)', N'Asus', 930000, 790500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/01/04/image-removebg-preview-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH763')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH763', N'Ổ cứng SSD Gigabyte 256GB M.2 2280 PCIe NVMe Gen 3x4 (GP-GSM2NE3256GNTD)', N'Chính hãng', 520000, 442000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/27/image-removebg-preview-33.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH764')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH764', N'Ổ cứng SSD Gigabyte 500GB M.2 2280 PCIe NVMe Gen 3x4 - (GAG325E500G)', N'Chính hãng', 940000, 799000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/01/11/image-removebg-preview-51.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH765')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH765', N'Ổ cứng SSD Gigabyte 1TB M.2 2280 PCIe NVMe Gen 3x4 (GAG325E1TB)', N'Chính hãng', 2910000, 2473500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/27/image-removebg-preview-37.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH766')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH766', N'Ổ cứng SSD Lexar NM620 1TB M.2 2280 PCIe 3.0x4 - (LNM620X001T-RNNNG)', N'Chính hãng', 2730000, 2320500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/29/image-removebg-preview-42.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH767')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH767', N'Chuột bluetooth yên tĩnh BlueLED ELECOM M-TM10BB/EC - Chính Hãng', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/23/image-removebg-preview-2022-11-23t162235-562.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH768')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH768', N'Bàn phím có dây Logitech K120 - Chính hãng', N'Logitech', 165000, 140250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2022/10/03/k120-gallery-01-new-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH769')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH769', N'Chuột không dây Silent Rapoo M300 - Chính hãng', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/21/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH770')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH770', N'Ổ cứng SSD WD Green 240GB SATA 2.5 inch - Chính hãng', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH771')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH771', N'Ổ cứng SSD WD Green 240GB M.2 2280 - Chính hãng', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH772')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH772', N'Ổ cứng SSD WD SN350 Green 240GB M.2 2280 PCIe NVMe 3x4', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH773')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH773', N'Ổ cứng SSD WD Blue 250GB SATA 2.5 inch - Chính hãng', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-13.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH774')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH774', N'Ổ cứng SSD WD SN570 Blue 250GB M.2 2280 PCIe NVMe 3x4 - Chính hãng', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-14.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH775')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH775', N'Ổ cứng SSD WD SN570 Blue 500GB M.2 2280 PCIe NVMe 3x4 - Chính hãng', N'Chính hãng', 1180000, 1003000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/13/image-removebg-preview-15.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH776')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH776', N'Ổ cứng SSD Kingston SNVS 250G NVMe M.2 2280 PCIe Gen 3 x 4', N'Chính hãng', 710000, 603500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH777')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH777', N'Ổ cứng SSD Kingston SKC600 512GB 2.5 inch SATA 3', N'Chính hãng', 1720000, 1462000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH778')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH778', N'Ram Laptop Corsair Vengeance (CMSX4GX4M1A2400C16) 4GB (1x4GB) DDR4 2400MHz', N'Chính hãng', 470000, 399500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-15.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH779')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH779', N'Ram laptop CORSAIR (CMSX8GX4M1A2400C16) 8GB (1x8GB) DDR4 2400MHz', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-16.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH780')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH780', N'RAM Laptop Kingston (KVR16LS11/4WP) 4GB (1x4GB) DDR3 1600Mhz - Chính hãng', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-18.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH781')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH781', N'RAM Laptop Kingston (KVR16LS11/8WP) 8GB (1x8GB) DDR3 1600Mhz - Chính hãng', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-20.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH782')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH782', N'RAM Laptop Kingston (KVR32S22S6/4) 4GB (1x4GB) DDR4 3200Mhz - Chính hãng', N'Chính hãng', 470000, 399500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/ram-laptop-kingston-ddr4-4gb-3200mhz-1-2v-kvr32s22s6-4-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH783')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH783', N'Ổ cứng SSD Kingston A400 120GB 2.5 inch SATA3', N'Chính hãng', 430000, 365500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH784')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH784', N'Ổ cứng SSD Kingston A400 240GB 2.5 inch SATA3', N'Chính hãng', 500000, 425000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH785')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH785', N'Ổ cứng SSD Kingston A400 480GB 2.5 inch SATA3', N'Chính hãng', 1120000, 952000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH786')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH786', N'Ổ cứng SSD Kingston A400 120G M.2 2280', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/12/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH787')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH787', N'Chuột không dây Logitech Signature M650 - Chính hãng', N'Logitech', 625000, 531250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2022/08/18/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH788')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH788', N'Chuột không dây Logitech M325 - Chính hãng', N'Logitech', 389000, 330650, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2022/08/04/m325-dark-grey-gallery-1-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH789')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH789', N'Chuột Gaming Razer DeathAdder V2 X HyperSpeed-Wireless Ergonomic RZ01', N'Chính hãng', 1080000, 918000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/28/chuot-gaming-khong-day-razer-deathadder-v2-x-hyperspeed-rz01-04130100-r3a1-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH790')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH790', N'Chuột Gaming Razer Pro Click Mini-Wireless Productivity_RZ01-03990100-R3A18', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/28/image-removebg-preview-5_637946156108940803.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH791')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH791', N'Chuột không dây Logitech Pebble M350', N'Logitech', 470000, 399500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2022/07/23/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH792')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH792', N'Chuột vi tính không dây TARGUS AMB582AP-51 - Chính hãng', N'Chính hãng', 450000, 382500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/01/07/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH793')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH793', N'Chuột không dây Logitech B175 - Chính hãng', N'Logitech', 190000, 161500, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2021/11/26/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH794')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH794', N'Chuột không dây Logitech M331 - Chính hãng', N'Logitech', 355000, 301750, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2021/11/26/chuot-khong-day-logitech-m331-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH795')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH795', N'Chuột không dây Logitech M221 Chính hãng', N'Logitech', 285000, 242250, 50, N'Chiếc', N'LOGITECH', N'https://i.hoanghamobile.vn/Uploads/2021/11/26/221-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH796')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH796', N'Samsung Galaxy Tab S12 Ultra Wi-Fi 12GB/256GB', N'Samsung', 34990000, 29741500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH797')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH797', N'Samsung Galaxy Tab S12 Plus 5G 12GB/256GB', N'Samsung', 34490000, 29316500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-plus-1-161156166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH798')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH798', N'Samsung Galaxy Tab S10 FE Wifi 8GB/128GB', N'Samsung', 11190000, 9511500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/15/s10-fe-3-134808082.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH799')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH799', N'Samsung Galaxy Tab A11 Wi-Fi 4GB/64GB', N'Samsung', 3550000, 3017500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-a11-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH800')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH800', N'Máy tính bảng Lenovo Tab ZAEH0147VN', N'Lenovo', 3790000, 3221500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/11/06/lenovo-tab-zaeh0147vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH801')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH801', N'iPad A16 Wifi 128GB', N'Apple', 11890000, 10106500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-wifi-pink-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH802')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH802', N'iPad Pro M5 11 inch Wi-Fi (256GB)', N'Apple', 32990000, 28041500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/20/ipad-pro-11-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH803')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH803', N'Xiaomi REDMI Pad 2 9.7 inch 4G 4GB/64GB', N'Xiaomi', 4690000, 3986500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/04/24/p85x-gray-front1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH804')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH804', N'Máy tính bảng Huawei MatePad 11.5 PaperMatte 8GB/256GB', N'Huawei', 9590000, 8151500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/30/matepad-11-5-papermatte-taoxingzhi-mouse.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH805')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH805', N'Honor Pad X8b 6GB/128GB', N'Honor', 6690000, 5686500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2026/06/01/nadal2-14.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH806')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH806', N'Máy tính bảng REDMAGIC Astra 2 Gaming 12GB/256GB', N'Chính hãng', 19990000, 16991500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/redmagic-astra-2-gaming-trang-5-110809021.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH807')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH807', N'iPad Air M4 11 inch Wi-Fi 128GB', N'Apple', 18990000, 16141500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-11-in-m4-wifi-purple-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH808')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH808', N'Redmi Pad 2 Wi-Fi (4GB+256GB)', N'Xiaomi', 5790000, 4921500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/06/12/o84-purple-back-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH809')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH809', N'Máy tính bảng HUAWEI MatePad 11.5 2026 (8GB+128GB)', N'Huawei', 9990000, 8491500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/06/24/huawei-matepad-11-5-2026-8gb-128gb-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH810')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH810', N'Máy tính bảng HUAWEI MatePad Air 8GB/256GB', N'Huawei', 21990000, 18691500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/09/huawei-matepad-air-1-143044048.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH811')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH811', N'Lenovo Tab M9 (3GB/32GB)', N'Lenovo', 2290000, 1946500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/01/09/lenovo-tab-m9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH812')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH812', N'HUAWEI MatePad 11.5 S 2026', N'Huawei', 11990000, 10191500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/huawei-matepad-11-5-s-2026-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH813')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH813', N'iPad Gen 10 10.9" WiFi 64GB', N'Apple', 7990000, 6791500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/03/27/vn-ipa-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH814')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH814', N'iPad Gen 10 10.9" WiFi 256GB', N'Apple', 10590000, 9001500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/03/27/vn-ipa-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH815')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH815', N'iPad Gen 10 10.9" 5G 64GB', N'Apple', 10990000, 9341500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/03/27/vn-ipa-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH816')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH816', N'iPad Gen 10 10.9" 5G 256GB', N'Apple', 14290000, 12146500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/03/27/vn-ipa-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH817')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH817', N'Lenovo Tab P11 Plus (4GB/64GB) - Chính hãng', N'Lenovo', 4390000, 3731500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2023/02/21/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH818')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH818', N'Lenovo Tab M10 Gen 3 (3GB/32GB) - Chính hãng', N'Lenovo', 2890000, 2456500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2023/03/28/lenovotabm10gen3-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH819')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH819', N'OPPO Pad Air (Màu tím) - 128GB - Chính hãng', N'OPPO', 7990000, 6791500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2023/04/12/oppo-pad-air-tim-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH820')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH820', N'Nokia T21 LTE (4GB/128GB) - Chính hãng', N'Nokia', 3990000, 3391500, 50, N'Chiếc', N'NOKIA', N'https://i.hoanghamobile.vn/Uploads/2023/06/28/t21-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH821')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH821', N'HTC A103 - 10" - 4G LTE - (4GB/64GB) - Chính hãng', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/24/htc-a103-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH822')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH822', N'TCL TAB 10 LTE Gen 2 (4GB/64GB) - Chính hãng', N'Chính hãng', 3790000, 3221500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/05/tcl-tab-10-gen-2-glacier-blue-front-back.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH823')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH823', N'TCL TAB 11 Wi-Fi (4GB/128GB) - Chính hãng', N'Chính hãng', 4290000, 3646500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/23/tcl-tab-11-purple-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH824')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH824', N'TCL TAB 10L Gen 2 (3GB/32GB) - Chính hãng', N'Chính hãng', 2090000, 1776500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/05/freyr-tcl-tab10l-gen-2-black-front-back.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH825')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH825', N'Lenovo Tab M11 ZADB0134VN (4GB/64GB)', N'Lenovo', 3990000, 3391500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2024/03/12/lenovo-tab-m11-zadb0134vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH826')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH826', N'Lenovo Tab M8 Gen 4 - 4G - ZABV0108VN (3GB/ 32GB/ Xám)', N'Lenovo', 2390000, 2031500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2024/05/09/zabv0108vn.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH827')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH827', N'Lenovo Tab M9 - 4G/LTE (4GB/64GB)', N'Lenovo', 3190000, 2711500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2023/10/23/lenovo-tab-m9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH828')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH828', N'iPad Mini 7 8.3" WiFi 128GB', N'Apple', 14290000, 12146500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/11/04/ipad-mini-7-wifi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH829')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH829', N'HONOR Pad X9 (4+128GB)', N'Honor', 3890000, 3306500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2024/11/15/honor-pad-x9-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH830')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH830', N'Honor Pad X8a (4+128GB)', N'Honor', 4090000, 3476500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2024/12/03/honor-pad-x8a-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH831')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH831', N'REDMAGIC Nova Gaming Tablet 16GB/512GB', N'Chính hãng', 17990000, 15291500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/06/redmagic-nova-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH832')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH832', N'Lenovo Tab M11 (4GB/128GB) (ZADB0066VN)', N'Lenovo', 4990000, 4241500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/01/09/lenovo-m11-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH833')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH833', N'iPad A16 5G 128GB', N'Apple', 15990000, 13591500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-cellular-blue-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH834')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH834', N'iPad A16 Wifi 256GB', N'Apple', 14790000, 12571500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-wifi-silver-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH835')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH835', N'iPad A16 5G 256GB', N'Apple', 18990000, 16141500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-cellular-silver-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH836')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH836', N'iPad A16 Wifi 512GB', N'Apple', 19990000, 16991500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-wifi-silver-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH837')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH837', N'iPad A16 5G 512GB', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-cellular-pink-pdp-image-position-1-vn-vi.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH838')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH838', N'Samsung Galaxy Tab S10 FE 5G 8GB/128GB', N'Samsung', 13490000, 11466500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/15/s10-fe-1-134807846.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH839')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH839', N'Máy tính bảng Huawei MatePad 11.5 inch (6GB/128GB)', N'Huawei', 6360000, 5406000, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/30/matepad-11-5-bartokr.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH840')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH840', N'Samsung Galaxy Tab S10 Lite Wifi 6GB/128GB', N'Samsung', 8490000, 7216500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-s10-lite-gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH841')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH841', N'Máy tính bảng Huawei MatePad Pro 12.2 inch 2025 (12GB/512GB)', N'Huawei', 21490000, 18266500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/29/mkt-morandi-green-tablet-white-glide-keyboard-pencil-04-ultra-hd-hq-png-20250328.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH842')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH842', N'Samsung Galaxy Tab S11 Wi-Fi 12GB/128GB', N'Samsung', 19150000, 16277500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-s11-gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH843')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH843', N'Samsung Galaxy Tab S11 5G 12GB/128GB', N'Samsung', 21150000, 17977500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-s11-gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH844')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH844', N'Samsung Galaxy Tab S11 Ultra 5G 12GB/256GB', N'Samsung', 30990000, 26341500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-s11-ultra-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH845')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH845', N'Máy tính bảng Honor Pad X7 Wifi (4GB/64GB)', N'Honor', 2990000, 2541500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/09/16/2-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH846')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH846', N'Samsung Galaxy Tab A11 LTE 4GB/64GB', N'Samsung', 4550000, 3867500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-a11-gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH847')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH847', N'Máy tính bảng Redmi Pad 2 Pro 5G 6GB/128GB', N'Xiaomi', 8290000, 7046500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/10/31/p83-5g-grey-back-r1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH848')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH848', N'iPad Pro M5 11 inch Wi-Fi (512GB)', N'Apple', 39490000, 33566500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/20/ipad-pro-11-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH849')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH849', N'iPad Pro M5 11 inch 5G (256GB)', N'Apple', 39290000, 33396500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/20/ipad-pro-11-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH850')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH850', N'iPad Pro M5 11 inch 5G (512GB)', N'Apple', 44990000, 38241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/20/ipad-pro-11-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH851')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH851', N'iPad Pro M5 13 inch Wi-Fi (256GB)', N'Apple', 42990000, 36541500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/ipad-pro-13-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH852')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH852', N'iPad Pro M5 13 inch Wi-Fi (512GB)', N'Apple', 49490000, 42066500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/ipad-pro-13-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH853')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH853', N'iPad Pro M5 13 inch 5G (256GB)', N'Apple', 48590000, 41301500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/ipad-pro-13-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH854')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH854', N'iPad Pro M5 13 inch 5G (512GB)', N'Apple', 54490000, 46316500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/10/21/ipad-pro-13-in-m5-cellular-space-black-pdp-image-position-2-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH855')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH855', N'Máy tính bảng REDMAGIC Astra Gaming 12GB/256GB', N'Chính hãng', 14490000, 12316500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/29/redmagic-astra-gaming-tablet-bac-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH856')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH856', N'Máy tính bảng REDMAGIC Astra Gaming 16GB/512GB', N'Chính hãng', 17990000, 15291500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/29/redmagic-astra-gaming-tablet-bac-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH857')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH857', N'Samsung Galaxy Tab A11 Plus 5G - 6GB/128GB', N'Samsung', 7790000, 6621500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-a11-plus-silver.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH858')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH858', N'Samsung Galaxy Tab A11 Plus Wi-Fi 8GB/256GB', N'Samsung', 8390000, 7131500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/tab-a11-plus-gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH859')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH859', N'Lenovo Tab One (ZAF00020VN)', N'Lenovo', 2490000, 2116500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/12/30/zaf00020vn-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH860')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH860', N'Lenovo Tab One (ZAF10022VN)', N'Lenovo', 3390000, 2881500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2025/12/30/zaf00020vn-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH861')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH861', N'Máy tính bảng HUAWEI MatePad 12 X (12GB/256GB)', N'Huawei', 13390000, 11381500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/12/31/ka-thumbnail.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH862')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH862', N'Máy tính bảng Xiaomi Pad 8 8GB/128GB', N'Xiaomi', 10690000, 9086500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/01/27/xiaomi-pad-8-demo.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH863')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH863', N'Máy tính bảng Xiaomi Pad 8 Pro 8GB/128GB', N'Xiaomi', 15190000, 12911500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/01/27/xiaomi-pad-8-pro-demo.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH864')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH864', N'iPad Air M4 11 inch Wi-Fi 256GB', N'Apple', 21990000, 18691500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-11-in-m4-wifi-purple-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH865')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH865', N'iPad Air M4 11 inch Wi-Fi 512GB', N'Apple', 27490000, 23366500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-11-in-m4-wifi-purple-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH866')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH866', N'iPad Air M4 11 inch Wi-Fi 1TB', N'Apple', 30690000, 26086500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-11-in-m4-wifi-purple-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH867')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH867', N'iPad Air M4 11 inch 5G 128GB', N'Apple', 23590000, 20051500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-11-in-m4-cellular-purple-pdp-image-position-1-th-en.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH868')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH868', N'iPad Air M4 11 inch 5G 256GB', N'Apple', 25990000, 22091500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-11-in-m4-cellular-purple-pdp-image-position-1-th-en.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH869')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH869', N'iPad Air M4 11 inch 5G 512GB', N'Apple', 31490000, 26766500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-11-in-m4-cellular-purple-pdp-image-position-1-th-en.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH870')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH870', N'iPad Air M4 11 inch 5G 1TB', N'Apple', 34890000, 29656500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-11-in-m4-cellular-purple-pdp-image-position-1-th-en.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH871')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH871', N'iPad Air M4 13 inch Wi-Fi 128GB', N'Apple', 24590000, 20901500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-13-in-m4-wifi-blue-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH872')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH872', N'iPad Air M4 13 inch Wi-Fi 256GB', N'Apple', 26990000, 22941500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-13-in-m4-wifi-blue-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH873')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH873', N'iPad Air M4 13 inch Wi-Fi 512GB', N'Apple', 30490000, 25916500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-13-in-m4-wifi-blue-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH874')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH874', N'iPad Air M4 13 inch Wi-Fi 1TB', N'Apple', 36090000, 30676500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/ipad-air-13-in-m4-wifi-blue-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH875')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH875', N'iPad Air M4 13 inch 5G 128GB', N'Apple', 28490000, 24216500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-13-in-m4-cellular-starlight-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH876')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH876', N'iPad Air M4 13 inch 5G 256GB', N'Apple', 31590000, 26851500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-13-in-m4-cellular-starlight-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH877')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH877', N'iPad Air M4 13 inch 5G 512GB', N'Apple', 34690000, 29486500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-13-in-m4-cellular-starlight-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH878')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH878', N'iPad Air M4 13 inch 5G 1TB', N'Apple', 40290000, 34246500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/ipad-air-13-in-m4-cellular-starlight-pdp-image-position-1-th-en.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH879')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH879', N'Xiaomi REDMI Pad 2 9.7 inch Wifi 4GB/64GB', N'Xiaomi', 4290000, 3646500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/04/24/p85-gray-front1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH880')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH880', N'Máy tính bảng LENOVO TAB(ZAEJ0145VN) G85/4G/64G/10.1T/LTE/WL/5100mAh/2Y/Xám', N'Lenovo', 4490000, 3816500, 50, N'Chiếc', N'LENOVO', N'https://i.hoanghamobile.vn/Uploads/2026/06/29/zaej0145vn-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH881')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH881', N'Máy tính bảng REDMAGIC Astra 2 Gaming 16GB/512GB', N'Chính hãng', 22990000, 19541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/redmagic-astra-2-gaming-trang-5-110809021.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH882')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH882', N'Samsung Galaxy Tab S12 Plus Wi-Fi 12GB/256GB', N'Samsung', 31790000, 27021500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-plus-1-161156166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH883')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH883', N'Samsung Galaxy Tab S12 Ultra 5G 12GB/256GB', N'Samsung', 39490000, 33566500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH884')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH884', N'iPad Mini 8 WiFi 256GB', N'Apple', 22290000, 18946500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/27/Gallery/192135_ipad-mini-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH885')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH885', N'iPad Mini 8 WiFi 256GB Nano', N'Apple', 25090000, 21326500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/27/Gallery/192135_ipad-mini-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH886')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH886', N'iPad Mini 8 WiFi 512GB', N'Apple', 27890000, 23706500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/27/Gallery/192135_ipad-mini-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH887')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH887', N'iPad Mini 8 WiFi 512GB Nano', N'Apple', 30690000, 26086500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/10/27/Gallery/192135_ipad-mini-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH888')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH888', N'Huawei MatePad SE 11 inch (4GB/128GB)', N'Huawei', 4990000, 4241500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/30/huawei-matepad-se.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH889')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH889', N'Apple Watch SE 3 - GPS, 40mm - Vỏ Nhôm Dây Cao Su S/M', N'Apple', 6090000, 5176500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH890')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH890', N'Đồng hồ thông minh Xiaomi Redmi Watch 6 Active', N'Xiaomi', 1190000, 1011500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/15/watch-6-active-9-095429234.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH891')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH891', N'Apple Watch Series 12 - GPS, 42mm - Viền Nhôm Dây Cao Su S/M', N'Apple', 11090000, 9426500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH892')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH892', N'Đồng hồ thông minh Huawei Watch GT 7 46mm', N'Huawei', 4790000, 4071500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/18/huawei-watch-gt-7-46mm-xanh-1-163605556.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH893')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH893', N'Đồng hồ thông minh Huawei Watch GT 7 Pro 46mm', N'Huawei', 7490000, 6366500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/18/huawei-watch-gt-7-pro-46mm-den-1-162953927.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH894')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH894', N'Đồng hồ thông minh Huawei Watch GT 7 41mm', N'Huawei', 4790000, 4071500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/18/huawei-watch-gt-7-41mm-1-161720927.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH895')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH895', N'Vòng đeo tay thông minh Xiaomi Band 11', N'Xiaomi', 1490000, 1266500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/10/01/vong-deo-tay-thong-minh-xiaomi-band-11-hong-3-140140234.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH896')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH896', N'Samsung Galaxy Watch9 40mm BT (L340)', N'Samsung', 8990000, 7641500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/l340-7-151912827.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH897')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH897', N'Samsung Galaxy Watch9 44mm BT (L350)', N'Samsung', 9990000, 8491500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/l350-9-160938157.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH898')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH898', N'Samsung Galaxy Watch Ultra2 LTE (L715)', N'Samsung', 16990000, 14441500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/21/samsung-galaxy-watch-ultra-2-lte-6-143053987.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH899')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH899', N'Đồng hồ thông minh Huawei GT 6 41mm - Màu Tím dây cao su', N'Huawei', 3990000, 3391500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/konsu-purple-product-image-left-30-en-jpg-20250813.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH900')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH900', N'Apple Watch Series 12 - GPS, 46mm - Viền Nhôm Dây Cao Su S/M', N'Apple', 12490000, 10616500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH901')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH901', N'Apple Watch Series 12 - GPS, 46mm - Viền Nhôm Dây Thép S/M', N'Apple', 13990000, 11891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH902')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH902', N'Apple Watch Series 12 - GPS, 42mm - Viền Nhôm Dây Thép', N'Apple', 12490000, 10616500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH903')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH903', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Nhôm Dây Cao Su S/M', N'Apple', 13990000, 11891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH904')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH904', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Nhôm Dây Cao Su M/L', N'Apple', 15490000, 13166500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH905')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH905', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Nhôm Dây Thép', N'Apple', 15490000, 13166500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH906')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH906', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Nhôm Dây Thép S/M', N'Apple', 16990000, 14441500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH907')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH907', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Gốm Dây Cao Su S/M', N'Apple', 26790000, 22771500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-42mm-gps-plus-cellular-night-blue-ceramic-174208553.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH908')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH908', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Gốm Dây Cao Su S/M', N'Apple', 28290000, 24046500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-42mm-gps-plus-cellular-night-blue-ceramic-174208553.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH909')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH909', N'Apple Watch Series 12 - GPS, 42mm - Viền Nhôm Dây Cao Su M/L', N'Apple', 11090000, 9426500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH910')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH910', N'Apple Watch Series 12 - GPS, 46mm - Viền Nhôm Dây Cao Su M/L', N'Apple', 12490000, 10616500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH911')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH911', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Nhôm Dây Cao Su M/L', N'Apple', 13990000, 11891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH912')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH912', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Nhôm Dây Cao Su S/M', N'Apple', 15490000, 13166500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-light-gold-aluminum-sport-band-sand-pdp-images-vi-vn-155643600.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH913')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH913', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Gốm Dây Cao Su M/L', N'Apple', 26790000, 22771500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-42mm-gps-plus-cellular-night-blue-ceramic-174208553.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH914')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH914', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Gốm Dây Cao Su M/L', N'Apple', 28290000, 24046500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-42mm-gps-plus-cellular-night-blue-ceramic-174208553.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH915')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH915', N'Apple Watch Series 12 - GPS, 46mm - Viền Nhôm Dây Thép M/L', N'Apple', 13990000, 11891500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH916')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH916', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Nhôm Dây Thép M/L', N'Apple', 16990000, 14441500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-series-12-46mm-gps-dark-bronze-aluminum-milanese-160603228.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH917')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH917', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Titan Dây Cao Su S/M', N'Apple', 20490000, 17416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104207556-112814166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH918')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH918', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Titan Dây Thép', N'Apple', 21990000, 18691500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104650785-112814197.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH919')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH919', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Titan Dây Thép S/M', N'Apple', 23490000, 19966500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104650785-112814197.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH920')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH920', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Titan Dây Cao Su S/M', N'Apple', 21990000, 18691500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104207556-112814166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH921')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH921', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Titan Dây Thép M/L', N'Apple', 23490000, 19966500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104650785-112814197.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH922')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH922', N'Apple Watch Series 12 - GPS + Cellular, 42mm - Viền Titan Dây Cao Su M/L', N'Apple', 20490000, 17416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104207556-112814166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH923')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH923', N'Apple Watch Series 12 - GPS + Cellular, 46mm - Viền Titan Dây Cao Su M/L', N'Apple', 21990000, 18691500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/11/apple-1-104207556-112814166.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH924')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH924', N'Đồng hồ thông minh Xiaomi Watch 5', N'Xiaomi', 5490000, 4666500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/o61-black-frontright.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH925')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH925', N'Đồng hồ thông minh Huawei Watch GT 6 Milanese', N'Huawei', 5690000, 4836500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/12/31/konsu-gold-product-image-left-30-en-png-800px-20250813.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH926')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH926', N'Đồng hồ thông minh Huawei GT 6 46mm - Màu Nâu dây da', N'Huawei', 4290000, 3646500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/atum-grey-product-image-left-30-en-jpg-20250807.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH927')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH927', N'Đồng hồ thông minh Huawei GT 6 46mm - Màu Xanh lá dây woven', N'Huawei', 4290000, 3646500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/atum-green-product-image-left-30-en-jpg-20250807.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH928')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH928', N'Đồng hồ thông minh Huawei GT 6 41mm - Màu Nâu dây da', N'Huawei', 4290000, 3646500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/konsu-brown-product-image-left-30-en-jpg-20250819.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH929')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH929', N'Đồng hồ thông minh Huawei GT 6 41mm - Màu Trắng dây da', N'Huawei', 4290000, 3646500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/konsu-white-product-image-left-30-en-jpg-20250813.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH930')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH930', N'Vòng đeo tay thông minh Xiaomi Band 11 - Ceramic', N'Xiaomi', 2190000, 1861500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/10/01/vong-deo-tay-thong-minh-xiaomi-band-11-ceramic-trang-2-140139437.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH931')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH931', N'Apple Watch Series 8 - GPS + Cellular, 45mm - Viền thép dây cao su', N'Apple', 9990000, 8491500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2022/09/11/image-removebg-preview-1_637984976450783767.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH932')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH932', N'Apple Watch SE 2024 - GPS, 40mm - Vỏ Nhôm Dây Cao Su', N'Apple', 4990000, 4241500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/thumb-watch-se-2024-lte-44mm-anh-sao.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH933')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH933', N'Apple Watch SE 2024 - GPS, 44mm - Vỏ Nhôm Dây Cao Su', N'Apple', 5990000, 5091500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/thumb-watch-se-2024-lte-44mm-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH934')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH934', N'Apple Watch SE 2024 - GPS + Cellular, 40mm - Vỏ Nhôm Dây Cao Su', N'Apple', 6890000, 5856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/thumb-watch-se-2024-lte-44mm-bac.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH935')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH935', N'Apple Watch SE 2024 - GPS + Cellular, 44mm - Vỏ Nhôm Dây Cao Su', N'Apple', 7690000, 6536500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/thumb-watch-se-2024-lte-44mm-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH936')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH936', N'Apple Watch Ultra 3 - GPS + Cellular, 49mm - Viền Titan Dây Alpine', N'Apple', 19290000, 16396500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap115e-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH937')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH937', N'Apple Watch Series 11 - GPS, 42mm - Viền Nhôm Dây Cao Su', N'Apple', 8990000, 7641500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-watch-series-11-46mm-gps-rose-gold-aluminum-sport-band-light-blush-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH938')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH938', N'Apple Watch SE 3 - GPS, 44mm - Vỏ Nhôm Dây Cao Su dây S/M', N'Apple', 6990000, 5941500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH939')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH939', N'Apple Watch SE 3 - GPS + Cellular, 40mm - Vỏ Nhôm Dây Cao Su S/M', N'Apple', 7690000, 6536500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH940')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH940', N'Apple Watch SE 3 - GPS + Cellular, 44mm - Vỏ Nhôm Dây Cao Su M/L', N'Apple', 8590000, 7301500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH941')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH941', N'Apple Watch Ultra 3 - GPS + Cellular, 49mm - Viền Titan Dây Trail S/M', N'Apple', 22790000, 19371500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-3_638931124707823349.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH942')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH942', N'Apple Watch Ultra 3 - GPS + Cellular, 49mm - Viền Titan Dây Ocean', N'Apple', 22990000, 19541500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-3_638931136755245502.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH943')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH943', N'Apple Watch Ultra 3 - GPS + Cellular, 49mm - Viền Titan Dây Thép Milan', N'Apple', 25990000, 22091500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-1_638931140359198361.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH944')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH944', N'Apple Watch Series 11 - GPS, 46mm - Viền Nhôm Dây Cao Su M/L', N'Apple', 9990000, 8491500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-watch-series-11-46mm-gps-rose-gold-aluminum-sport-band-light-blush-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH945')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH945', N'Apple Watch Series 11 - GPS + Cellular, 46mm - Viền Nhôm Dây Cao Su', N'Apple', 12990000, 11041500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/11/ap5d90-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH946')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH946', N'Apple Watch Series 11 - GPS + Cellular, 42mm - Viền Nhôm Dây Cao Su', N'Apple', 11990000, 10191500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/11/apple-4_638931816857796753.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH947')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH947', N'Apple Watch Series 11 - GPS + Cellular, 46mm - Viền Titan Dây Thép Milan', N'Apple', 21290000, 18096500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/11/apple-4_638931857388735852.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH948')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH948', N'Apple Watch Series 11 - GPS + Cellular, 42mm - Viền Titan Dây Thép Milan', N'Apple', 18990000, 16141500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/11/ap09db-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH949')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH949', N'Apple Watch SE 3 - GPS, 44mm - Vỏ Nhôm Dây Cao Su dây M/L', N'Apple', 6890000, 5856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH950')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH950', N'Apple Watch Ultra 3 - GPS + Cellular, 49mm - Viền Titan Dây Trail M/L', N'Apple', 22790000, 19371500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-3_638931124707823349.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH951')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH951', N'Apple Watch Series 11 - GPS, 46mm - Viền Nhôm Dây Cao Su S/M', N'Apple', 9990000, 8491500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/apple-watch-series-11-46mm-gps-rose-gold-aluminum-sport-band-light-blush-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH952')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH952', N'Đồng hồ thông minh Garmin Fenix 9 - 43MM', N'Garmin', 26290000, 22346500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/garmin-fenix-9-43mm-xam-soi-1-155509647.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH953')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH953', N'Đồng hồ thông minh Garmin Fenix 9 - 47MM', N'Garmin', 26290000, 22346500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/garmin-fenix-9-47mm-den-1-161753138.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH954')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH954', N'Đồng hồ thông minh Garmin Fenix 9 - 51MM', N'Garmin', 28990000, 24641500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/garmin-fenix-9-51mm-den-1-164438393.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH955')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH955', N'Đồng hồ thông minh Garmin Fenix 9 Pro - 43MM', N'Garmin', 30290000, 25746500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/garmin-fenix-9-pro-43mm-vang-kem-1-170249439.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH956')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH956', N'Đồng hồ thông minh Garmin Fenix 9 Pro - 47MM', N'Garmin', 28990000, 24641500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/26/garmin-fenix-9-pro-47mm-xam-1-171527634.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH957')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH957', N'Đồng hồ thông minh Garmin Fenix 9 Pro Solar- 47MM', N'Garmin', 28990000, 24641500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/garmin-fenix-9-pro-solar-1-084431828.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH958')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH958', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Alpine S', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-alpine-150104198.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH959')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH959', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Milanese S', N'Apple', 26890000, 22856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-milanese-loop-natural-pdp-images-vi-vn-150138017.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH960')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH960', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Ocean', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-black-titanium-ocean-band-translucent-black-pdp-images-vi-vn-150210033.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH961')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH961', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Trail S/M', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-black-titanium-trail-loop-burgundy-pdp-images-vi-vn-150232376.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH962')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH962', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Alpine M', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-alpine-150104198.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH963')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH963', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Alpine L', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-alpine-150104198.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH964')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH964', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Trail M/L', N'Apple', 23990000, 20391500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-black-titanium-trail-loop-burgundy-pdp-images-vi-vn-150232376.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH965')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH965', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Milanese M', N'Apple', 26890000, 22856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-milanese-loop-natural-pdp-images-vi-vn-150138017.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH966')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH966', N'Apple Watch Ultra 4 - GPS + Cellular, 49mm - Viền Titan Dây Milanese L', N'Apple', 26890000, 22856500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/apple-watch-ultra-4-49mm-gps-plus-cellular-natural-titanium-milanese-loop-natural-pdp-images-vi-vn-150138017.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH967')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH967', N'Đồng hồ thông minh Huawei Watch GT 7 41mm - Dây da', N'Huawei', 6990000, 5941500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/18/huawei-watch-gt-7-41mm-xanh-1-162132977.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH968')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH968', N'Apple Watch SE 3 - GPS, 40mm - Vỏ Nhôm Dây Cao Su M/L', N'Apple', 6090000, 5176500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH969')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH969', N'Apple Watch SE 3 - GPS + Cellular, 44mm - Vỏ Nhôm Dây Cao Su S/M', N'Apple', 8590000, 7301500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH970')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH970', N'Apple Watch SE 3 - GPS + Cellular, 40mm - Vỏ Nhôm Dây Cao Su M/L', N'Apple', 7690000, 6536500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/ap5e45-1.JPG', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH971')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH971', N'Đồng hồ Garmin Fenix 6 Pro Solar, Cobalt Blue w/Whitestone band, GPS SEA', N'Garmin', 13990000, 11891500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2020/10/27/cobalt.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH972')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH972', N'Đồng hồ Garmin Fenix 6S Pro Solar - Chính hãng FPT', N'Garmin', 11490000, 9766500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2020/09/23/Garmin%20Fenix%20v6S%20Pro%20Solar_637364811208524039.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH973')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH973', N'Đồng hồ Garmin Fenix 6S Sapphire - Chính hãng FPT', N'Garmin', 10990000, 9341500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2020/10/27/Garmin%206s%20saphire%20-%20Rose%20Gold%201.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH974')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH974', N'Đồng hồ Garmin Fenix 6X Sapphire, Carbon Gray - Chính hãng FPT', N'Garmin', 15690000, 13336500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2020/09/23/Garmin%20Fenix%206X%20Sapphire,%20Carbon%20Gray.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH975')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH975', N'Đồng hồ định vị trẻ em myAlo K84', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/03/09/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH976')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH976', N'Đồng hồ Oppo Watch 41mm dây silicone', N'OPPO', 3490000, 2966500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2021/06/05/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH977')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH977', N'Vòng đeo tay OPPO Band', N'OPPO', 590000, 501500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2021/06/09/image-removebg-preview-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH978')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH978', N'Đồng hồ Garmin Venu 2 - Chính hãng FPT', N'Garmin', 5390000, 4581500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2021/06/24/image-removebg-preview-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH979')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH979', N'Đồng hồ Garmin Venu 2S - Chính hãng FPT', N'Garmin', 5990000, 5091500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2021/06/25/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH980')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH980', N'Đồng hồ Garmin Forerunner 55 - Chính hãng FPT', N'Garmin', 2590000, 2201500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/09/image-removebg-preview_637799988894127992.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH981')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH981', N'Đồng hồ Asus Vivowatch SP', N'Vivo', 4990000, 4241500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2021/10/21/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH982')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH982', N'Đồng hồ Garmin Venu 2 Plus, GPS', N'Garmin', 7990000, 6791500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/01/06/image-removebg-preview-20.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH983')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH983', N'Đồng hồ SoundPEATS Watch 2', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/01/11/image-removebg-preview-28.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH984')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH984', N'Đồng hồ Garmin Fenix 7 - Silver with Black Band', N'Garmin', 12490000, 10616500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/image-removebg-preview-1_637806917940667893.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH985')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH985', N'Đồng hồ Garmin Fenix 7 Saphire Solar', N'Garmin', 15990000, 13591500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/fenix-7-saphie-solar-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH986')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH986', N'Đồng hồ Garmin Fenix 7 Sapphire Solar Dây Titanium', N'Garmin', 21990000, 18691500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/garmin-fenix-7-sapphire-solar-day-titanium-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH987')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH987', N'Đồng hồ Garmin Fenix 7X, Sapphire, Solar, Sea GPS', N'Garmin', 13990000, 11891500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/garmin-fenix-7x-sapphire-solar-sea-gps-chinh-hang-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH988')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH988', N'Đồng hồ Fenix 7X Sapphire Solar Carbon', N'Chính hãng', 21990000, 18691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/f-nix-7x-sapphire-solar-4-titan.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH989')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH989', N'Đồng hồ Garmin Epix Gen 2', N'Garmin', 13990000, 11891500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/image-removebg-preview-1_637806943509838586.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH990')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH990', N'Đồng hồ Garmin Epix Gen 2, Back,Gray Carbon DLC Titanium', N'Garmin', 15990000, 13591500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/epix-gen2-carbon-titan.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH991')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH991', N'Đồng hồ Garmin Epix Gen 2, Trắng Carrera, Titanium với Dây Silicone, SEA', N'Garmin', 14990000, 12741500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/17/trang.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH992')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH992', N'Đồng hồ Garmin Fenix 7S Sapphire Solar , Hồng vàng', N'Garmin', 19990000, 16991500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/18/dong-ho-thong-minh-garmin-fenix-7s-saphire-solar-hong-vang-chinh-hang-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH993')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH993', N'Đồng hồ Garmin Fenix 7S Saphire Solar', N'Garmin', 16850000, 14322500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/02/18/dong-ho-thong-minh-garmin-fenix-7s-saphire-solar-chinh-hang-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH994')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH994', N'Đồng hồ Garmin Fenix 6, Sapphire, Black DLC with Heathered Red Nylon Band', N'Garmin', 13990000, 11891500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/03/24/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH995')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH995', N'Đồng hồ Garmin Fenix 6S - Sapphire, Light Gold with Shale Gray Leather Band', N'Garmin', 12750000, 10837500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/03/24/garmin-fenix-6s-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH996')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH996', N'Đồng hồ Garmin Instinct 2S, Xám Graphite, SEA', N'Garmin', 5250000, 4462500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/03/26/instinct-2s-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH997')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH997', N'Đồng hồ Garmin Instinct 2S, Solar Standard Edition', N'Garmin', 6990000, 5941500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/03/26/garmin-instinct-2-solar-standard-edition-mist-gray-jpg-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH998')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH998', N'Đồng hồ Garmin Instinct 2, Xám Graphite, SEA', N'Garmin', 5250000, 4462500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/03/26/instinct-2s-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH999')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH999', N'Đồng hồ Garmin Fenix 6X - Sapphire, Đen DLC với Dây da nâu, GPS, SEA', N'Garmin', 15490000, 13166500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/fenix-6x-sapphire.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1000')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1000', N'Đồng hồ Huawei Watch Kids 4 Pro', N'Huawei', 2740000, 2329000, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2022/05/13/aslan-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1001')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1001', N'Vòng đeo tay thông minh Garmin Vivosmart 5 - Chính hãng', N'Vivo', 3250000, 2762500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2022/05/23/image-removebg-preview-36.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1002')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1002', N'Đồng hồ Huawei Band 7', N'Huawei', 749000, 636650, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2022/06/13/vong-deo-tay-thong-minh-huawei-band-7-chinh-hang-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1003')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1003', N'Đồng hồ Garmin Forerunner 255S', N'Garmin', 6890000, 5856500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/06/20/255s-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1004')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1004', N'Đồng hồ Garmin Forerunner 255S Music', N'Garmin', 7690000, 6536500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/06/20/255s-music-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1005')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1005', N'Đồng hồ Garmin Forerunner 955', N'Garmin', 9490000, 8066500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/06/20/garmin-forerunner-955-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1006')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1006', N'Đồng hồ Garmin Forerunner 955 Solar', N'Garmin', 10250000, 8712500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/06/20/garmin-forerunner-955-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1007')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1007', N'Đồng hồ Forerunner 255 Music', N'Chính hãng', 7490000, 6366500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/garmin-forerunner-255-music.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1008')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1008', N'Đồng hồ Amazfit Bip 3 Pro', N'Amazfit', 830000, 705500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2022/07/06/ef2d1bbd-d045-4c4d-8571-3d6d2b00d7bc.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1009')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1009', N'Đồng hồ Amazfit T-Rex 2', N'Amazfit', 3990000, 3391500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/amazfit-t-rex-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1010')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1010', N'Đồng hồ Masstel Smart Hero 10 4G', N'Masstel', 1520000, 1292000, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/masstel-smart-hero-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1011')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1011', N'Vòng đeo tay Fitbit Inspire 3', N'Chính hãng', 2290000, 1946500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/fitbit-inspire-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1012')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1012', N'Đồng hồ Garmin Venu SQ 2', N'Garmin', 3600000, 3060000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/09/20/image-removebg-preview-2022-09-20t090349-627.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1013')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1013', N'Đồng hồ Garmin Venu SQ 2 Music', N'Garmin', 4590000, 3901500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2022/09/20/image-removebg-preview-2022-09-20t091108-813.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1014')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1014', N'Đồng hồ Masstel Smart Hero 20 4G', N'Masstel', 1790000, 1521500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2023/01/11/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1015')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1015', N'Đồng hồ Garmin Vivomove Trend', N'Vivo', 7290000, 6196500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2023/02/14/v1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1016')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1016', N'Đồng hồ Garmin Forerunner 965', N'Garmin', 11990000, 10191500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/03/10/garmin-forerunner-965-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1017')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1017', N'Đồng hồ Garmin Forerunner 265', N'Garmin', 8490000, 7216500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/03/10/garmin-forerunner-265-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1018')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1018', N'Đồng hồ Garmin Forerunner 265S', N'Garmin', 8490000, 7216500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/03/15/garmin-forerunner-265s-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1019')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1019', N'Đồng hồ Kieslect KS', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/19/kieslect-ks-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1020')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1020', N'Đồng hồ Kieslect Kr Pro', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/19/kieslect-kr-pro-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1021')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1021', N'Đồng hồ Kieslect K11 Pro', N'Chính hãng', 750000, 637500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/19/kieslect-k11-pro-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1022')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1022', N'Đồng hồ Kieslect Lora', N'Chính hãng', 450000, 382500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/27/kieslect-lora-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1023')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1023', N'Đồng hồ Garmin Fenix 7S Pro - Sapphire Solar', N'Garmin', 14990000, 12741500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/garmin-fenix-7s-pro-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1024')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1024', N'Đồng hồ Garmin Fenix 7 Pro - Sapphire Solar', N'Garmin', 14990000, 12741500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/garmin-fenix-7-pro-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1025')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1025', N'Đồng hồ Garmin Epix Pro (Gen 2) – Sapphire 42mm', N'Garmin', 19590000, 16651500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/epix-pro-gen-2-sapphire-42mm-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1026')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1026', N'Đồng hồ Garmin Epix Pro (Gen 2) – Sapphire 47mm', N'Garmin', 18990000, 16141500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/epix-pro-gen-2-sapphire-47mm-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1027')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1027', N'Đồng hồ Garmin Epix Pro (Gen 2) – Sapphire 51mm', N'Garmin', 23690000, 20136500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/epix-pro-gen-2-sapphire-51mm-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1028')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1028', N'Đồng hồ Garmin Fenix 7X Pro - Sapphire Solar', N'Garmin', 18500000, 15725000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/06/15/fenix-7x-pro-sapphire-solar-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1029')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1029', N'Đồng hồ KIESLECT LORA 2', N'Chính hãng', 1150000, 977500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/14/kieslect-lora-2-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1030')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1030', N'Đồng hồ KIESLECT KS PRO', N'Chính hãng', 760000, 646000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/14/kieslect-ks-pro-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1031')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1031', N'Đồng hồ Amazfit Cheetah Pro', N'Amazfit', 3990000, 3391500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2023/08/23/amazfit-cheetah-pro-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1032')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1032', N'Đồng hồ Amazfit Cheetah Round', N'Amazfit', 2990000, 2541500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2023/08/23/amazfit-cheetah-round-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1033')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1033', N'Đồng hồ Amazfit Cheetah Square', N'Amazfit', 2990000, 2541500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2023/08/23/amazfit-cheetah-square-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1034')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1034', N'Đồng hồ Garmin Venu 3', N'Garmin', 9830000, 8355500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2023/08/29/venu-3-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1035')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1035', N'Đồng hồ Garmin Venu 3S', N'Garmin', 9830000, 8355500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/01/23/garmin-venu-3s.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1036')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1036', N'Đồng hồ MyKID 4G LITE', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/08/31/mykid4glite-h.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1037')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1037', N'Đồng hồ Huawei Watch GT4 41mm', N'Huawei', 3990000, 3391500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2023/09/15/gt4-41mm-1_638303869283912023.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1038')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1038', N'Đồng hồ HONOR Watch GS3 45.9mm', N'Honor', 1650000, 1402500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2023/10/25/gs3-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1039')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1039', N'Đồng hồ KIESLECT KS Mini', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/31/kieslect-ks-mini-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1040')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1040', N'Đồng hồ KIESLECT KR 2', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/05/untitled-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1041')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1041', N'Đồng hồ định vị trẻ em myALO K74', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/11/29/myalo-k74-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1042')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1042', N'Đồng hồ Amazfit Balance', N'Amazfit', 3890000, 3306500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/pdp-balance-beauty-features-gallery-1080x1080amazfit-balance-sideview-44f9784e-7dab-4ccf-9288-43f80607f191-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1043')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1043', N'Đồng hồ Amazfit Active Edge', N'Amazfit', 2540000, 2159000, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2024/01/10/amazfit-active-edge-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1044')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1044', N'Đồng hồ Garmin Lily 2 Silicone', N'Garmin', 6190000, 5261500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/01/15/garmin-lily-2-silicone-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1045')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1045', N'Đồng hồ Kieslect Ks2', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/16/kieslect-ks-2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1046')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1046', N'Đồng hồ Gloryfit LC304', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/01/gloryfit-lc304_638554477965427596.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1047')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1047', N'Đồng hồ Gloryfit Q31', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/03/04/gloryfit-q31-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1048')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1048', N'Đồng hồ Gloryfit S8 Pro', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/03/04/gloryfit-s8-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1049')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1049', N'Đồng hồ Garmin Forerunner 165 Music', N'Garmin', 5490000, 4666500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/03/14/garmin-forerunner-165-music-22.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1050')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1050', N'Đồng hồ Garmin Forerunner 165', N'Garmin', 3990000, 3391500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/03/14/garmin-forerunner-165-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1051')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1051', N'Vòng đeo tay Huawei Band 9', N'Huawei', 650000, 552500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/07/26/huawei-band-9.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1052')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1052', N'Đồng hồ Huawei Watch Fit 3', N'Huawei', 1990000, 1691500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/05/16/huawei-watch-fit-3-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1053')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1053', N'Đồng hồ BWOO WA-03', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/03/bwoo-wa-03-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1054')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1054', N'Đồng hồ SoundPEATS Watch 4', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/30/soundpeats-watch-4-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1055')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1055', N'Đồng hồ HUAWEI Watch GT4 41mm - Màu Xanh', N'Huawei', 3190000, 2711500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/06/27/gt-4-41mm-xanh-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1056')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1056', N'Đồng hồ Garmin Vivoactive 5', N'Vivo', 5690000, 4836500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/garmin-vivoactive-5-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1057')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1057', N'Đồng hồ Wonlex CT19', N'Chính hãng', 950000, 807500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/16/wonlex-ct19-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1058')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1058', N'Đồng hồ thông minh Kieslect Actor', N'Chính hãng', 1090000, 926500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/15/kieslect-actor-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1059')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1059', N'Đồng hồ thông minh Kieslect KS3', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/22/ks3-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1060')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1060', N'Đồng hồ thông minh Honor Watch 4', N'Honor', 3490000, 2966500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2024/08/22/honor-watch-4-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1061')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1061', N'Goly K10', N'Chính hãng', 1090000, 926500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/18/goly-k10-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1062')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1062', N'Đồng hồ thông minh Black Shark S1 Classic', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/23/black-shark-s1-classic-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1063')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1063', N'Đồng hồ thông minh Garmin Fenix 8 51mm Sapphire AMOLED Edition', N'Garmin', 26698000, 22693300, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/08/31/fenix8-51-of-titanium-sparkorange-front-left-activitylist-trail-run.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1064')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1064', N'Đồng hồ thông minh Garmin Fenix 8 47mm AMOLED Edition - Glass', N'Garmin', 22524000, 19145400, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/08/31/fenix8-47-of-slategray-black-front-left-muscle-map.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1065')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1065', N'Đồng hồ thông minh Garmin Fenix 8 47mm  Sapphire AMOLED Edition', N'Garmin', 25032000, 21277200, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/08/31/fenix8-51-of-carbongray-black-front-left-wf-47-51mm-sapphire-bare.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1066')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1066', N'Đồng hồ thông minh Garmin Fenix 8 43mm AMOLED Edition - Glass', N'Garmin', 22524000, 19145400, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/08/31/fenix8-43-of-stainless-whitestone-front-left-climb-in-activity.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1067')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1067', N'Đồng hồ thông minh Garmin Fenix 8 43mm Sapphire AMOLED Edition', N'Garmin', 25032000, 21277200, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/09/04/f-nix-8-43mm-sapphire-amoled-edition-soft-gold-ssfog-gray-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1068')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1068', N'Đồng hồ thông minh Garmin Fenix 8 51mm Sapphire Solar Edition', N'Garmin', 26698000, 22693300, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/09/04/fenix-8-51mm-sapphrine-solar-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1069')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1069', N'Đồng hồ thông minh Garmin Fenix 8 47mm Sapphire Solar Edition', N'Garmin', 25032000, 21277200, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/09/04/fenix-8-47mm-sapphire-solar-edition-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1070')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1070', N'Đồng hồ thông minh Garmin Fenix E 47mm', N'Garmin', 15990000, 13591500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/09/04/fenix-e-47mm-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1071')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1071', N'Đồng hồ thông minh Amazfit T-Rex 3', N'Amazfit', 5290000, 4496500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2024/09/14/amazfit-t-rex-3-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1072')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1072', N'Đồng hồ thông minh Xiaomi Redmi Watch 5 Active', N'Xiaomi', 880000, 748000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/09/17/n65a-black-frontright45-wallpaper.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1073')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1073', N'Đồng hồ thông minh Xiaomi Redmi Watch 5 Lite', N'Xiaomi', 1370000, 1164500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/09/23/xiaomi-redmi-watch-5-lite-gold-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1074')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1074', N'Đồng hồ thông minh Huawei GT 5 - 46mm', N'Huawei', 4090000, 3476500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/09/25/huawei-watch-gt-5-blue-46mm-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1075')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1075', N'Đồng hồ thông minh Huawei GT 5 - 41mm', N'Huawei', 4090000, 3476500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/09/23/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1076')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1076', N'Đồng hồ thông minh Huawei GT 5 Pro - 46mm', N'Huawei', 7990000, 6791500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/09/25/huawei-watch-gt-5-pro-titanium-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1077')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1077', N'Đồng hồ thông minh Huawei GT 5 Pro - 42mm', N'Huawei', 7990000, 6791500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/09/25/huawei-watch-gt-5-pro-ceramic-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1078')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1078', N'Đồng hồ HUAWEI WATCH D2', N'Huawei', 7840000, 6664000, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/10/11/huawei-watch-d2-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1079')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1079', N'Đồng hồ thông minh Garmin Lily 2 Active', N'Garmin', 7320000, 6222000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2024/10/11/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1080')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1080', N'Đồng hồ thông minh Kospet Tank T3 Ultra', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/14/kospet-tank-t3-ultra-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1081')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1081', N'Đồng hồ thông minh Kospet Tank M3 Ultra', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/14/kospet-tank-m3-ultra-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1082')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1082', N'HONOR Band 9', N'Honor', 390000, 331500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2024/10/30/honor-band-9-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1083')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1083', N'Đồng hồ thông minh Zobo NovaDeli', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/05/zobo-novadeli.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1084')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1084', N'Đồng hồ thông minh Kieslect Pura', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/11/kieslect-pura-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1085')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1085', N'Vòng đeo tay thông minh Xiaomi Band 9 Active', N'Xiaomi', 560000, 476000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/11/27/n69-pink-side45.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1086')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1086', N'Đồng hồ thông minh Riversong Motive 9 Max SW903', N'Chính hãng', 1550000, 1317500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/17/sw903.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1087')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1087', N'Đồng hồ thông minh Riversong Motive 9 Pro SW901', N'Chính hãng', 960000, 816000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/17/9-pro.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1088')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1088', N'Đồng hồ thông minh Riversong Motive 9E SW905', N'Chính hãng', 960000, 816000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/17/sw905.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1089')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1089', N'Đồng hồ thông minh Garmin Instinct 3 45MM Amoled', N'Garmin', 9655000, 8206750, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/02/12/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1090')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1090', N'Đồng hồ thông minh Garmin Instinct 3 50MM Amoled', N'Garmin', 10760000, 9146000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/02/12/garmin-instinct-3-45mm-amoled-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1091')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1091', N'Đồng hồ thông minh Garmin Approach S50 - 43MM', N'Garmin', 9720000, 8262000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/02/12/garmin-approach-s50-43mm-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1092')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1092', N'Đồng hồ thông minh Xiaomi Watch S4', N'Xiaomi', 3290000, 2796500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/27/o62-black-left45.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1093')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1093', N'Đồng hồ thông minh Amazfit Active 2', N'Amazfit', 2490000, 2116500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/04/01/amazfit-active-2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1094')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1094', N'Đồng hồ thông minh Riversong Motive 10GT SW1001', N'Chính hãng', 1450000, 1232500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/11/sw1001-xanh-la-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1095')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1095', N'Vòng đeo tay thông minh Huawei Band 10 - Khung viền Polymer', N'Huawei', 650000, 552500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/03/14/mkt-nora-basic-angle-plastic-frame-bleck-left30-en-20250121.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1096')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1096', N'Vòng đeo tay thông minh Huawei Band 10 - Khung viền hợp kim nhôm', N'Huawei', 750000, 637500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/03/17/gt-10-blue-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1097')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1097', N'Đồng hồ định vị trẻ em myALO K64', N'Chính hãng', 1390000, 1181500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/21/dong-ho-dinh-vi-tre-em-myalo-k64-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1098')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1098', N'Đồng hồ thể thao định vị GPS SUUNTO RACE (Viền Thép)', N'Chính hãng', 8490000, 7216500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/25/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1099')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1099', N'Đồng hồ thể thao định vị GPS SUUNTO RACE S (Viền Titanium)', N'Chính hãng', 8990000, 7641500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/25/gps-suunto-race-s-vien-titanium.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1100')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1100', N'Đồng hồ thể thao định vị GPS SUUNTO RACE S  (Viền Thép)', N'Chính hãng', 8990000, 7641500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/25/d.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1101')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1101', N'HONOR CHOICE Watch 2i', N'Honor', 950000, 807500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/04/02/honor-choice-infowear-watch-2i-black-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1102')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1102', N'Gloryfit Q30T', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/08/gloryfit-q30t-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1103')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1103', N'Đồng hồ thông minh Garmin Approach S70 - 42 mm', N'Garmin', 13350000, 11347500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/04/10/garmin-approach-s70-4mm-trang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1104')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1104', N'Đồng hồ thông minh Mibro Lite 3 Pro', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/09/lite-3-pro-gold-l.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1105')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1105', N'Đồng hồ thông minh Mibro GS Pro', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/11/anh-sp.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1106')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1106', N'Đồng hồ thông minh Amazfit Active 2 Premium Sapphire (dây da)', N'Amazfit', 2990000, 2541500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/04/25/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1107')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1107', N'Đồng hồ thông minh Mibro Watch A3', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/29/mibro-watch-a3-beige-r.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1108')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1108', N'Đồng hồ thông minh Garmin Vivoactive 6', N'Vivo', 8290000, 7046500, 50, N'Chiếc', N'VIVO', N'https://i.hoanghamobile.vn/Uploads/2025/05/05/vivoactive-6-pink-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1109')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1109', N'Đồng hồ thông minh Amazfit Active 2 Square', N'Amazfit', 2890000, 2456500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/rome-45-right.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1110')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1110', N'Đồng hồ thông minh Kieslect KR 3', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/kr3-gold-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1111')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1111', N'Đồng hồ thông minh Kieslect Lora 3', N'Chính hãng', 1890000, 1606500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/lora-3-pink-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1112')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1112', N'Samsung Watch8 44mm BT (L330)', N'Samsung', 6890000, 5856500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/watch8-xam-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1113')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1113', N'Đồng hồ thông minh Amazfit Balance 2', N'Amazfit', 6990000, 5941500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/07/31/amazfit-balance-2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1114')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1114', N'Đồng hồ báo thức tích hợp RADIO LEXON FLIP+', N'Chính hãng', 190000, 161500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/08/19/dong-ho-bao-thuc-tich-hop-radio-lexon-flip-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1115')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1115', N'Đồng hồ thông minh Garmin Venu 4 - 45MM', N'Garmin', 14240000, 12104000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1116')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1116', N'Đồng hồ thông minh Garmin Venu 4 - 41MM', N'Garmin', 14240000, 12104000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/image-removebg-preview-21.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1117')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1117', N'Đồng hồ thông minh Xiaomi Watch S4 41mm - Đen', N'Xiaomi', 4290000, 3646500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/09/23/o63-black-45.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1118')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1118', N'Đồng hồ thông minh Amazfit T-Rex 3 Pro 48mm', N'Amazfit', 8990000, 7641500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/09/24/t-rex-3-pro-black-gold-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1119')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1119', N'Vòng đeo tay thông minh Xiaomi Band 10 Glimmer (Phiên bản giới hạn)', N'Xiaomi', 2390000, 2031500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/09/24/o66-golden-right-45-up.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1120')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1120', N'Đồng hồ thông minh HUAWEI WATCH Ultimate 2 - Màu Đen', N'Huawei', 15990000, 13591500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/15/huawei-watch-ultimate-2-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1121')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1121', N'Đồng hồ thông minh HUAWEI WATCH Ultimate 2', N'Huawei', 21990000, 18691500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/10/15/huawei-watch-ultimate-2-xanh-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1122')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1122', N'Đồng hồ thông minh Kieslect Elite 2', N'Chính hãng', 1750000, 1487500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/27/kieslect-ai-smartwatch-elite2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1123')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1123', N'Đồng hồ thông minh Xiaomi Watch S4 41mm - Trắng dây Da', N'Xiaomi', 4990000, 4241500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/09/23/o63-white-45.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1124')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1124', N'Xiaomi Watch S4 - Rainbow', N'Xiaomi', 3790000, 3221500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/03/01/xiaomi-watch-s4-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1125')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1125', N'Đồng hồ thông minh Huawei GT 6 Pro 46mm - Titanium', N'Huawei', 10490000, 8916500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/09/22/atum-pro-titanium-product-image-left-30-en-jpg-20250813.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1126')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1126', N'Đồng hồ thông minh Amazfit T-Rex 3 Pro 44mm', N'Amazfit', 8990000, 7641500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/12/17/t-rex-3-44mm.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1127')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1127', N'Đồng hồ thông minh Honma x Huawei Watch GT 6 Pro', N'Huawei', 8990000, 7641500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/12/31/atum-pro-homa-product-image-left-30-en-jpg-20250813_639027783324355580.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1128')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1128', N'Amazfit Active Max', N'Amazfit', 3850000, 3272500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2026/01/15/45-88b5482c0abb4e8cb7372f96ef1b7ed1-1024x1024.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1129')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1129', N'Đồng hồ thông minh Kieslect KR Ultra 3', N'Chính hãng', 1590000, 1351500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/kr3-white-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1130')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1130', N'Vòng đeo tay thông minh HUAWEI Band 11', N'Huawei', 890000, 756500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/02/12/huawei-band-11-cao-su-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1131')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1131', N'Vòng đeo tay thông minh HUAWEI Band 11 - Khung hợp kim nhôm', N'Huawei', 990000, 841500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/02/12/huawei-band-11-nhom-be-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1132')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1132', N'Vòng đeo tay thông minh HUAWEI Band 11 Pro', N'Huawei', 1430000, 1215500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/02/12/huawei-band-11-pro-xanh-la-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1133')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1133', N'Đồng hồ thông minh HUAWEI WATCH Ultimate 2 - Green Edition', N'Huawei', 21990000, 18691500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/03/13/ultimate2-green-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1134')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1134', N'Đồng hồ thông minh Amazfit Active 3 Premium', N'Amazfit', 4490000, 3816500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2026/03/26/amazfit-active-3-xanhduong-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1135')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1135', N'Samsung Galaxy Watch9 44mm LTE (L355)', N'Samsung', 10790000, 9171500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/l355-1-152849113.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1136')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1136', N'Samsung Galaxy Watch9 40mm LTE (L345)', N'Samsung', 10290000, 8746500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/l345-7-151548124.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1137')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1137', N'Vòng đeo tay thông minh Xiaomi Band 11 Active', N'Xiaomi', 890000, 756500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/08/15/xiaomi-band-11-active-3-095821374.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1138')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1138', N'Đồng hồ thông minh Huawei Watch Fit 4', N'Huawei', 2540000, 2159000, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/05/16/fit-4-perplr-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1139')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1139', N'Đồng hồ thông minh Huawei Watch Fit 4 Pro', N'Huawei', 3990000, 3391500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/05/16/fit-4-pro-blue-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1140')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1140', N'Đồng hồ thông minh Amazfit Bip 6', N'Amazfit', 1890000, 1606500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/05/26/amazfit-bip-6-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1141')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1141', N'Samsung Watch8 40mm LTE (L325)', N'Samsung', 6890000, 5856500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/watch8-xam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1142')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1142', N'Đồng hồ Garmin Forerunner 570 (47mm)', N'Garmin', 12340000, 10489000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/06/09/forerunner-570-47mm.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1143')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1143', N'Đồng hồ Garmin Forerunner 570 (42mm)', N'Garmin', 12340000, 10489000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/06/09/forerunner-570-42mm-w-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1144')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1144', N'Đồng hồ Garmin Forerunner 970', N'Garmin', 17660000, 15011000, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2025/06/09/forerunner-970-gold-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1145')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1145', N'Đồng hồ thông minh Huawei Watch 5 - 46mm', N'Huawei', 6690000, 5686500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/06/16/mkt-rates-purple-product-image-left-30-en-jpg-800px-20250403.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1146')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1146', N'Đồng hồ thông minh Huawei Watch 5 - 42mm', N'Huawei', 6990000, 5941500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/06/16/mkt-soc-beige-product-image-left-30-en-jpg-800px-20250403.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1147')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1147', N'Samsung Watch8 Classic 46mm BT (L500)', N'Samsung', 8690000, 7386500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/watch8-classic-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1148')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1148', N'Vòng đeo tay thông minh Xiaomi Band 10', N'Xiaomi', 1070000, 909500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/06/27/o66-pink-right45.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1149')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1149', N'Samsung Watch Ultra 2025 LTE', N'Samsung', 12990000, 11041500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/07/09/watch8-ultra-xam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1150')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1150', N'Vòng đeo tay thông minh Xiaomi Band 10 - Khung Ceramic', N'Xiaomi', 1550000, 1317500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/06/27/o66-ceramic-right45.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1151')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1151', N'Đồng hồ định vị trẻ em Masstel Hero 16i', N'Masstel', 1390000, 1181500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2026/06/11/masstel-hero-16i-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1152')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1152', N'Garmin Forerunner 170 Music', N'Garmin', 9770000, 8304500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/05/14/garmin-forerunner-70-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1153')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1153', N'Vòng đeo tay thông minh Xiaomi Band 10 Pro', N'Xiaomi', 1990000, 1691500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-band-10-pro-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1154')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1154', N'Đồng hồ thông minh Xiaomi Watch S5 46mm - Dây Woven', N'Xiaomi', 5090000, 4326500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-watch-s5-46mm-day-woven-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1155')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1155', N'Đồng hồ thông minh Xiaomi Watch S5 46mm - Dây da', N'Xiaomi', 5090000, 4326500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-watch-s5-46mm-day-da-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1156')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1156', N'Đồng hồ thông minh Xiaomi Watch S5 46mm - Dây cao su', N'Xiaomi', 4590000, 3901500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-watch-s5-46mm-day-cao-su-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1157')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1157', N'Đồng hồ HUAWEI Watch GT Runner 2', N'Huawei', 6290000, 5346500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/03/21/huawei-gt-runner-2-cam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1158')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1158', N'Garmin Forerunner 170', N'Garmin', 8250000, 7012500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/05/14/garmin-forerunner-70-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1159')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1159', N'Garmin Forerunner 70', N'Garmin', 6830000, 5805500, 50, N'Chiếc', N'GARMIN', N'https://i.hoanghamobile.vn/Uploads/2026/05/13/garmin-forerunner-70-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1160')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1160', N'Vòng đeo tay thông minh Xiaomi Band 10 Pro - Viền gốm', N'Xiaomi', 2590000, 2201500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-band-10-pro-vien-gom-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1161')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1161', N'Đồng hồ định vị trẻ em Tammi Watch Kid Plus', N'Chính hãng', 1070000, 909500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/15/tammi-watch-kid-plus-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1162')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1162', N'Đồng hồ định vị trẻ em Tammi Watch Kid Max', N'Chính hãng', 1320000, 1122000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/15/tammi-watch-kid-max-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1163')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1163', N'Đồng hồ định vị trẻ em Masstel Hero 30', N'Masstel', 1890000, 1606500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2026/06/10/masstel-hero-30-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1164')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1164', N'Đồng hồ định vị trẻ em Masstel Hero Air', N'Masstel', 1590000, 1351500, 50, N'Chiếc', N'MASSTEL', N'https://i.hoanghamobile.vn/Uploads/2026/06/11/hero-air-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1165')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1165', N'HUAWEI Watch Fit 5 Pro', N'Huawei', 4990000, 4241500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/05/12/huawei-watch-fit-5-pro-den-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1166')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1166', N'HUAWEI Watch Fit 5 - Dây Nylon (NFC)', N'Huawei', 3290000, 2796500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/05/12/huawei-watch-fit-5-bac-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1167')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1167', N'HUAWEI Watch Fit 5', N'Huawei', 2990000, 2541500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/05/12/huawei-watch-fit-5-den-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1168')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1168', N'Loa Harman Kardon Go+Play 3', N'Harman Kardon', 8590000, 7301500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2023/09/29/loa-harman-kardon-go-play-3-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1169')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1169', N'Tai nghe AirPods Pro 3', N'Apple', 5790000, 4921500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/11/03/Gallery/899842_airpod-pro-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1170')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1170', N'Tai nghe Redmi Buds 8 Active', N'Xiaomi', 690000, 586500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/02/27/p79-white-box-open-front.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1171')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1171', N'Loa Bluetooth JBL Charge 6', N'JBL', 4590000, 3901500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2025/05/05/jbl-charge-6-blk-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1172')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1172', N'Tai nghe Xiaomi Buds 6', N'Xiaomi', 2850000, 2422500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/05/27/xiaomi-buds-6-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1173')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1173', N'Loa di động không dây Sony ULT FIELD 3', N'Sony', 3290000, 2796500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/07/11/ult-field-3-hero1-forestgray-whiteback-large.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1174')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1174', N'Tai nghe AirPods 5 - Sạc có dây', N'Apple', 3690000, 3136500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/aipo-5-024212048.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1175')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1175', N'Loa Bluetooth hiệu Alpha Works AW-W89', N'Chính hãng', 2690000, 2286500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/24/alpha-works-aw-w89-den-rose-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1176')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1176', N'Tai nghe Huawei Freebuds Neo', N'Huawei', 2490000, 2116500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/09/23/huawei-freebuds-neo-bac-2-162330893.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1177')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1177', N'Loa Karaoke Arirang MB2CW', N'Chính hãng', 6190000, 5261500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/11/mb2cw-nau-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1178')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1178', N'Samsung Galaxy Buds Core', N'Samsung', 890000, 756500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2025/08/18/buds-core-onyx-black-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1179')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1179', N'Loa Bluetooth Harman Kardon Aura Studio 5', N'Harman Kardon', 8490000, 7216500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2026/03/17/harman-kardon-aura-studio-5-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1180')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1180', N'Samsung Galaxy Buds4 Pro', N'Samsung', 6050000, 5142500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/buds4-pro-trang-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1181')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1181', N'Loa Bluetooth JBL Go 5', N'JBL', 1170000, 994500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/ls-jbl-go5-front-red-0016-x2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1182')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1182', N'Loa Bluetooth Di Động Marshall Stockwell III', N'Marshall', 6990000, 5941500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2026/07/30/marshall-stockwell-lll-trang-1-153237088.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1183')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1183', N'Loa Marshall Acton IV', N'Marshall', 8990000, 7641500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2026/08/22/10-111349347.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1184')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1184', N'Loa Sony ULT TOWER 5 (SRS-ULT500)', N'Sony', 12990000, 11041500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/09/08/tower-5-1-104030875.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1185')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1185', N'Tai Nghe Enger Pro2 ANC EG50 TWS', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/11/05/Gallery/538635_tai-nghe-enger-pro2-anc-model-eg50tws%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1186')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1186', N'Tai Nghe Enger EG10TWS', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/11/05/Gallery/810822_tai-nghe-enger-eg10tws-eg10-tws%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1187')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1187', N'Tai nghe Bluetooth Enger TWS mini EG20TWS', N'Chính hãng', 220000, 187000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/11/05/Gallery/653400_tai-nghe-bluetooth-enger-tws-mini-hieu-enger-eg20tws%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1188')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1188', N'Tai nghe chụp tai chống ồn không dây SONY WH-1000XM4C', N'Sony', 5990000, 5091500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/10/22/sony-wh-1000xm4c-tim-2-103118132.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1189')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1189', N'Loa Karaoke Mini HAVIT SK819BT', N'Chính hãng', 300000, 255000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/22/sk819bt-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1190')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1190', N'Loa Karaoke Mini HAVIT SK894BT', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/22/loa-karaoke-mini-havit-sk894bt-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1191')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1191', N'Loa Bluetooth Retro Alpha Works Classic V60', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/26/retro-alpha-works-classic-v60-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1192')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1192', N'Tai nghe Bluetooth True Wireless Huawei FreeBuds SE 2', N'Huawei', 490000, 416500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2023/10/09/true-wireless-huawei-freebuds-se-2-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1193')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1193', N'Tai nghe chụp tai (Không dây) SENNHEISER Accentum Wireless', N'Chính hãng', 2990000, 2541500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/09/sennheiser-accentum-wireles-2_638324662146683664.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1194')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1194', N'Tai nghe chụp tai (Không dây) Edifier W820NB Plus', N'Chính hãng', 1150000, 977500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/09/edifier-w820nb-plus-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1195')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1195', N'Tai nghe Bluetooth True Wireless JBL Wave Beam', N'JBL', 1190000, 1011500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2023/10/13/true-wireless-jbl-wave-beam-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1196')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1196', N'Tai nghe Bluetooth Baseus Bowie WM02', N'Baseus', 349000, 296650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2023/10/25/baseus-bowie-wm02-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1197')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1197', N'Tai nghe có dây Baseus Encok H19', N'Baseus', 129000, 109650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2023/10/25/baseus-encok-h19.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1198')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1198', N'Tai nghe Bluetooth SoundPEATS Clear', N'Chính hãng', 480000, 408000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/25/bluetooth-soundpeats-clear-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1199')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1199', N'Tai nghe Bluetooth True Wireless Marshall Minor 3', N'Marshall', 2750000, 2337500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/h-true-wireless-marshall-minor-3-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1200')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1200', N'Tai nghe không dây Marshall Motif II ANC', N'Marshall', 4860000, 4131000, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/marshall-motif-ii-anc-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1201')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1201', N'Tai nghe Bluetooth chụp tai Marshall Major 4', N'Marshall', 3290000, 2796500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/marshall-major-4-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1202')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1202', N'Loa Bluetooth Marshall Acton III', N'Marshall', 7070000, 6009500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/loa-bluetooth-marshall-acton-iii-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1203')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1203', N'Loa Bluetooth Marshall Stanmore III', N'Marshall', 10610000, 9018500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/10/26/loa-bluetooth-marshall-stanmore-iii-chinh-hang-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1204')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1204', N'Tai nghe chụp tai (Không dây) Havit H630BT', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/12/04/havit-h630bt-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1205')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1205', N'Tai nghe chụp tai (Không dây) JBL LIVE 670NC', N'JBL', 2090000, 1776500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2023/12/13/live-670nc-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1206')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1206', N'Tai nghe chụp tai (Không dây) Anker soundcore Q20i', N'Anker', 690000, 586500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2023/12/13/anker-soundcore-q20i-blue.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1207')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1207', N'Loa Olike S3', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/12/13/loa-bluetooth-olike-s3-1-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1208')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1208', N'Tai nghe không dây Sennheiser CX Plus - Chính hãng', N'Chính hãng', 2890000, 2456500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/10/14/image-removebg-preview-1_637698066968304969.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1209')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1209', N'Tai nghe Bluetooth True Wireless Marshall Minor IV - Chính Hãng ASH', N'Marshall', 3050000, 2592500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2024/07/15/marshall-minor-iv.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1210')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1210', N'Tai nghe AirPods 4', N'Apple', 3190000, 2711500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/airpods-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1211')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1211', N'Bộ loa karaoke di động Arirang AM2', N'Chính hãng', 3390000, 2881500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/26/arirang-am2-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1212')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1212', N'Bộ loa karaoke di động Arirang MKS2.5', N'Chính hãng', 3190000, 2711500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/28/background.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1213')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1213', N'Loa Sony ULT TOWER 7 (SRS-ULT700)', N'Sony', 17990000, 15291500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/09/08/tower-7-1-104157739.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1214')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1214', N'Tai nghe AirPods 5 - Sạc không dây', N'Apple', 4190000, 3561500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/10/aipo-5-024212048.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1215')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1215', N'Loa di động Marshall Kilburn III', N'Marshall', 9440000, 8024000, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2025/07/23/01-marshall-marshall-kilburn-iii-portable-bluetooth-speaker-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1216')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1216', N'Loa Bluetooth Marshall Woburn III - Chính Hãng ASH', N'Marshall', 15990000, 13591500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2023/12/08/marshall-woburn-iii-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1217')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1217', N'Tai nghe Bluetooth chụp tai Marshall Major V - Chính Hãng ASH', N'Marshall', 3800000, 3230000, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2024/06/04/tai-nghe-bluetooth-marshall-major-v-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1218')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1218', N'Tai nghe không dây EarFun Air Mini 2', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/05/earfun-air-mini-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1219')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1219', N'Tai nghe AirPods Pro 2', N'Apple', 5390000, 4581500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/10/27/airpods-pro-2nd-gen-usb-c-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1220')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1220', N'Tai nghe AirPods 4 ANC', N'Apple', 4190000, 3561500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/airpods-4_638615358810000422.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1221')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1221', N'Tai nghe AirPods Max', N'Apple', 11990000, 10191500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2024/09/10/airpodmax.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1222')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1222', N'Tai nghe AirPods Max 2', N'Apple', 14990000, 12741500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/03/16/airpods-max-2-orange-pdp-image-position-1-vn-vi.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1223')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1223', N'Tai nghe Bluetooth True Wireless JBL Live Beam 4', N'JBL', 4990000, 4241500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/10/02/bluetooth-true-wireless-jbl-live-beam-4-bac-1-172917846.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1224')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1224', N'Xiaomi Mi Basic - Chính hãng', N'Xiaomi', 129000, 109650, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2020/09/23/Tai%20nghe%20Xiaomi%20Mi%20Basic%20.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1225')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1225', N'Tai nghe Xiaomi Buds 3T Pro - Chính hãng', N'Xiaomi', 990000, 841500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2022/03/18/xiaomi-buds-3t-pro-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1226')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1226', N'Tai nghe Asus ROG Cetra True Wireless (90YH03G1-B5UA00) - Chính hãng', N'Asus', 1590000, 1351500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/01/04/image-removebg-preview-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1227')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1227', N'Tai nghe không dây Huawei Freebuds 5 - Chính hãng', N'Huawei', 1990000, 1691500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2023/05/26/huawei-freebuds-5-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1228')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1228', N'Tai nghe Asus ROG Cetra True Wireless (90YH03X1-B5UA00) - Chính hãng', N'Asus', 1690000, 1436500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/05/29/b5ua00-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1229')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1229', N'Tai nghe Xiaomi Redmi Buds 5', N'Xiaomi', 650000, 552500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/01/08/redmi-buds-5-blue-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1230')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1230', N'Tai nghe Chụp tai (Không dây) SENNHEISER Accentum Plus Wireless', N'Chính hãng', 4290000, 3646500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/02/15/sennheiser-accentum-plus-wireless-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1231')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1231', N'Tai nghe không dây Redmi Buds 4 Active', N'Xiaomi', 390000, 331500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/03/26/redmi-buds-4-active-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1232')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1232', N'Tai nghe OPEN EAR MONSTER AC500', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/03/open-ear-monster-ac500-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1233')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1233', N'Tai nghe Monster N-Lite 203 AirLinks', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/03/monster-n-lite-203-airlinks-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1234')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1234', N'Tai nghe SENNHEISER MOMENTUM TRUE WIRELESS 4', N'Chính hãng', 5290000, 4496500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/04/sennheiser-momentum-true-wireless-4-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1235')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1235', N'Tai nghe chụp tai OneOdio A70', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/12/oneodio-a70-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1236')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1236', N'Tai nghe chụp tai OneOdio Pro 10', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/12/oneodio-pro-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1237')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1237', N'Tai nghe chụp tai OneOdio Pro C', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/12/tai-nghe-chup-tai-oneodio-pro-c-chinh-hang.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1238')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1238', N'Tai nghe OPPO Enco Buds2 Pro', N'OPPO', 890000, 756500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2024/04/15/cauchy-passport-misty-grey-04-b-earphone-cover-opening-combination-b.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1239')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1239', N'Tai nghe Bluetooth Chụp Tai Baseus Bass 35 Max', N'Baseus', 550000, 467500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/04/23/baseus-bass-35-max-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1240')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1240', N'Tai nghe Bluetooth True Wireless Huawei Freebuds Pro 3', N'Huawei', 3490000, 2966500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/10/11/huawei-freebuds-pro-3-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1241')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1241', N'Tai nghe Bluetooth True Wireless JBL Tour Pro 3', N'JBL', 5880000, 4998000, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/11/18/true-wireless-jbl-tour-pro-3-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1242')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1242', N'Tai nghe True Wireless JBL Tune Beam 2', N'JBL', 2340000, 1989000, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/11/19/01-ls-jbl-tune-beam-2-product-image-hero.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1243')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1243', N'Tai nghe True Wireless JBL Wave Beam 2', N'JBL', 1360000, 1156000, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/11/19/08-jbl-wave-beam-2-product-image-case-open-pink.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1244')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1244', N'Tai nghe Redmi Buds 6', N'Xiaomi', 980000, 833000, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/12/27/redmi-buds-6-o77-green-caseopen-front.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1245')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1245', N'Tai nghe gaming EDRA EH416PRO USB LED RGB', N'Chính hãng', 398000, 338300, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/eh416pro-usb-led-rgb-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1246')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1246', N'Tai nghe gaming EDRA EH404 USB LED RGB', N'Chính hãng', 248000, 210800, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/02/edra-eh404-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1247')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1247', N'Tai nghe Bluetooth UEELR YW03', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/09/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1248')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1248', N'Tai nghe HP HyperX Cloud Stinger Core II (683L9AA)', N'HP', 490000, 416500, 50, N'Chiếc', N'HP', N'https://i.hoanghamobile.vn/Uploads/2025/07/01/hp-hyperx-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1249')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1249', N'Tai nghe Sennheiser ACCENTUM Open', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/14/senn-accentum-open-tw-case-open-iso-black-v1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1250')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1250', N'Tai nghe Bluetooth Honor Choice Earbuds Clip', N'Honor', 1990000, 1691500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2025/07/18/honor-choice-earbuds-clip-tim-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1251')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1251', N'Tai Nghe Thể Thao Amazfit Up Open-Ear Earbuds', N'Amazfit', 1290000, 1096500, 50, N'Chiếc', N'AMAZFIT', N'https://i.hoanghamobile.vn/Uploads/2025/07/31/amazfit-up-open-ear-earbuds-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1252')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1252', N'Tai nghe Bluetooth Open-Ear SoundPEATS PearlClip Pro', N'Chính hãng', 950000, 807500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/08/08/1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1253')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1253', N'Tai nghe Bluetooth SoundPEATS POP Clip', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/08/18/1-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1254')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1254', N'Tai nghe HUAWEI FreeBuds SE 4 ANC', N'Huawei', 790000, 671500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/08/25/mkt-fuji-id-hq-cn-rgb-png-800_638917172351520605.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1255')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1255', N'Tai nghe Bluetooth SoundPEATS Air5 Pro', N'Chính hãng', 1360000, 1156000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/03/1-2-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1256')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1256', N'Tai nghe Xiaomi OpenWear Stereo Pro', N'Xiaomi', 3890000, 3306500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/09/23/o74-top-space-black-lid-opened.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1257')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1257', N'Tai nghe TWS HAVIT LIFE NC01T TW984', N'Chính hãng', 330000, 280500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/24/tw984-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1258')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1258', N'Tai nghe Headphone Bluetooth HAVIT H612BT Pro', N'Chính hãng', 420000, 357000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/24/h612bt-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1259')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1259', N'Tai nghe TWS HAVIT TW991 Pro', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/30/tw991-pro-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1260')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1260', N'Tai nghe Bluetooth HAVIT Space S1 H670BT', N'Chính hãng', 1450000, 1232500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/18/1758012307942-artboard-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1261')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1261', N'Tai nghe bluetooth hiệu Havit, model OWSFIT 02A', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/18/ows915-black-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1262')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1262', N'Tai nghe bluetooth hiệu Havit, model TW992', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/18/tw992-10.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1263')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1263', N'Tai nghe HUAWEI FreeBuds 7i', N'Huawei', 1790000, 1521500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2025/11/21/mkt-conch-id-hq-cn-png.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1264')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1264', N'Tai nghe Bluetooth SoundPeats Air5 Pro+', N'Chính hãng', 2060000, 1751000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/26/soundpeats-air5-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1265')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1265', N'Tai nghe Bluetooth Headphone HAVIT i62', N'Chính hãng', 320000, 272000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/01/havit-i62-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1266')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1266', N'Tai nghe không dây hiệu Baseus Bass BS2 Lite', N'Baseus', 290000, 246500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/01/14/os-a0010a07-bass-bs2-lite-true-wireless-earbuds-n-a-image-white-background-image-800-800-bass-bs2-3.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1267')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1267', N'Tai nghe không dây chống ồn hiệu Baseus Bowie H1s', N'Baseus', 689000, 585650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/01/15/3-photoroom.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1268')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1268', N'Tai nghe bluetooth hiệu Havit H603BT', N'Chính hãng', 260000, 221000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/15/1758011749217-artboard-5.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1269')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1269', N'Tai nghe bluetooth hiệu SoundPeats C30', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/15/4-e1763971964265-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1270')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1270', N'Tai nghe HUAWEI FreeClip 2', N'Huawei', 3790000, 3221500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/01/19/huawei-freeclip-2-white-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1271')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1271', N'Tai Nghe TWS HAVIT TW907 SE', N'Chính hãng', 179000, 152150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/29/raw-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1272')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1272', N'Tai Nghe TWS Nakamichi Hue-Tulip', N'Chính hãng', 290000, 246500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/30/nakamichi-hue-tulip.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1273')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1273', N'Tai Nghe Headphone Bluetooth Hue-Cherry', N'Chính hãng', 380000, 323000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/30/hue-cherry-black.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1274')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1274', N'Tai nghe Bluetooth Baseus Eli Fit Open-Ear', N'Baseus', 480000, 408000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/02/04/baseus-eli-fit-open-ear-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1275')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1275', N'Samsung Galaxy Buds4', N'Samsung', 4330000, 3680500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/buds4-den-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1276')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1276', N'Tai nghe Redmi Buds 8 Pro', N'Xiaomi', 1790000, 1521500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/02/25/p76-box-open-white.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1277')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1277', N'Tai nghe HUAWEI FreeArc', N'Huawei', 1750000, 1487500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/03/27/huawei-freearc-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1278')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1278', N'Tai nghe bluetooth SoundPeats Air6 HS', N'Chính hãng', 660000, 561000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/16/tai-nghe-bluetooth-soundpeats-air6-hs.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1279')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1279', N'Tai nghe chụp tai Marshall Milton A.N.C', N'Marshall', 5390000, 4581500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2026/06/16/tai-nghe-chup-tai-marshall-milton-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1280')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1280', N'Tai nghe Bluetooth chụp tai JBL Tune 530BT', N'JBL', 1190000, 1011500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/jbl-tune-530bt-18.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1281')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1281', N'Tai nghe chụp tai Sennheiser Momentum 5 Wireless', N'Chính hãng', 11490000, 9766500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/13/senn-momentum-5-wireless-denim-product-image-isofront-150724841.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1282')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1282', N'Tai nghe Sennheiser Momentum True Wireless 5', N'Chính hãng', 7990000, 6791500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/22/sennheiser-momentum-true-wireless-5-xanh-1-113920734.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1283')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1283', N'Tai nghe TWS HAVIT TW917', N'Chính hãng', 359000, 305150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/14/tw917-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1284')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1284', N'Tai nghe Bluetooth True Wireless EarFun Air Life', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/14/earfun-air-life.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1285')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1285', N'Tai nghe Bluetooth True Wireless Soundcore K20i', N'Chính hãng', 620000, 527000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/05/26/k20i-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1286')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1286', N'Tai nghe Bluetooth True Wireless Baseus Bowie E16', N'Baseus', 270000, 229500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/02/28/bowie-e16-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1287')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1287', N'Tai nghe không dây Bluetooth Stargo Hero', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/24/stargo-hero-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1288')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1288', N'Tai nghe bluetooth hiệu Havit, model LIFE 04T', N'Chính hãng', 230000, 195500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/11/18/tw888-30.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1289')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1289', N'Tai Nghe TWS Nakamichi TW016', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/30/nakamichi-tw016-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1290')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1290', N'Tai nghe không dây Baseus Bass EP10 NC', N'Baseus', 340000, 289000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/06/25/baseus-bass-ep10-nc-in-ear-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1291')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1291', N'HARMAN/KARDON AURA STUDIO 3 - Chính Hãng', N'Harman Kardon', 5990000, 5091500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2020/09/24/HARMAN-%20KARDON%20AURA%20STUDIO%203.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1292')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1292', N'Harman Kardon Soundstick 4 - Chính hãng', N'Harman Kardon', 7590000, 6451500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2020/09/23/Loa%20Bluetooth%20Harman%20Kardon%20Soundstick%204.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1293')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1293', N'Loa Oppo OBMC03 - Chính hãng', N'OPPO', 490000, 416500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2020/09/23/Loa%20Bluetooth%20Oppo%20OBMC03.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1294')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1294', N'Harman Kardon GO + PLAY MINI - Chính hãng', N'Harman Kardon', 4790000, 4071500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2020/09/24/Loa%20Harman%20Kardon%20GO%20PLAY%20MINI.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1295')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1295', N'Harman Kardon Onyx Studio 6 - Chính Hãng', N'Harman Kardon', 3390000, 2881500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2020/09/24/Loa%20Harman%20Kardon%20Onyx%20Studio%206.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1296')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1296', N'Loa Bluetooth LG XBoom Go PL2 - chính hãng', N'LG', 550000, 467500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2021/04/23/image-removebg-preview-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1297')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1297', N'Loa JBL PartyBox On-The-Go - Chính hãng', N'JBL', 5490000, 4666500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2020/12/30/on-the-go.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1298')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1298', N'Loa JBL Charge 5 - Chính hãng', N'JBL', 3290000, 2796500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2021/03/09/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1299')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1299', N'Loa Bluetooth JBL Xtreme 3 - Chính hãng', N'JBL', 3990000, 3391500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2021/04/08/image-removebg-preview-15.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1300')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1300', N'Loa Harman Kardon Onyx Studio 7 - Chính hãng (TBH)', N'Harman Kardon', 3990000, 3391500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2021/07/07/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1301')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1301', N'Loa Bluetooth realme Pocket - Chính hãng', N'Realme', 290000, 246500, 50, N'Chiếc', N'REALME', N'https://i.hoanghamobile.vn/Uploads/2021/09/29/image-removebg-preview-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1302')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1302', N'Loa Bluetooth SoundPEATS Halo - Chính Hãng', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/11/08/image-removebg-preview_637719770631418336.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1303')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1303', N'Loa Bluetooth Tribit Xsound Surf - Chính Hãng', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/11/23/loa-bluetooth-tribit-xsound-surf-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1304')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1304', N'Loa Bluetooth Tribit Xsound Go - Chính hãng', N'Chính hãng', 730000, 620500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/11/24/image-removebg-preview-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1305')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1305', N'Micro Karaoke tích hợp loa Bluetooth Monster M97', N'Chính hãng', 1790000, 1521500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/22/micro-karaoke-tich-hop-loa-bluetooth-monster-m97-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1306')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1306', N'Loa Monster Adventurer Max - Chính hãng', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/22/image-removebg-preview-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1307')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1307', N'Loa bluetooth Monster Adventurer Force - Chính Hãng', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/22/monster-adventurer-force-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1308')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1308', N'Loa Bluetooth LG Xboom 360 RP4 - Chính Hãng', N'LG', 3990000, 3391500, 50, N'Chiếc', N'LG', N'https://i.hoanghamobile.vn/Uploads/2022/03/29/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1309')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1309', N'Loa Monster SuperStar S110 - Chính hãng', N'Chính hãng', 1100000, 935000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/12/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1310')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1310', N'Loa tháp Samsung Sound Tower ST40B/XV - Chính Hãng', N'Samsung', 6490000, 5516500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2022/05/26/image-removebg-preview-42.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1311')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1311', N'Loa Bluetooth JBL FLIP 6 - Chính Hãng', N'JBL', 2450000, 2082500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2022/06/30/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1312')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1312', N'Loa di động SONY SRS-XG300 - Chính Hãng', N'Sony', 3990000, 3391500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2022/08/01/image-removebg-preview-22.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1313')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1313', N'Loa Bluetooth FOOMEE HH16 - Chính hãng', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/08/17/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1314')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1314', N'Loa Bluetooth FOOMEE HH17 - Chính hãng', N'Chính hãng', 290000, 246500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/08/17/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1315')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1315', N'Loa Bluetooth Harman Kardon ONYX STUDIO 8 - Chính Hãng (Đã Kích Hoạt)', N'Harman Kardon', 4990000, 4241500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2022/10/03/image-removebg-preview-64.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1316')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1316', N'Loa Bluetooth Devia Kintone Fabric - Chính hãng', N'Chính hãng', 150000, 127500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/image-removebg-preview-2022-11-24t152320-039.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1317')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1317', N'Loa Bluetooth Devia Kintone Mini Waterproof Lanyard', N'Chính hãng', 170000, 144500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/image-removebg-preview-2022-11-24t152614-492.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1318')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1318', N'Loa Bluetooth JBL PULSE 5  - Chính Hãng', N'JBL', 6350000, 5397500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2022/12/15/image-removebg-preview-2022-12-15t162629-691.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1319')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1319', N'Loa Bluetooth Sumico MSP10 - Chính Hãng', N'Chính hãng', 4290000, 3646500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/01/12/image-removebg-preview-12.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1320')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1320', N'Loa Sony SRS-XB100 - Chính Hãng', N'Sony', 1090000, 926500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/07/25/loa-sony-srs-xb100-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1321')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1321', N'Loa Harman Kardon Aura Studio 4 - Chính Hãng', N'Harman Kardon', 5990000, 5091500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2023/08/02/loa-harman-kardon-aura-studio-4-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1322')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1322', N'Loa Bluetooth Alpha Works W38', N'Chính hãng', 1090000, 926500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/alpha-works-w38-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1323')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1323', N'Loa Bluetooth Alpha Works W88', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/01/18/alpha-works-w88-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1324')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1324', N'Loa Karaoke di dộng SoundPEATS Hi Singing', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/02/15/soundpeats-hi-singing-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1325')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1325', N'Loa Monster Musicbox 40W', N'Chính hãng', 1350000, 1147500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/03/musicbox-40w-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1326')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1326', N'Loa Monster Musicbox Go 80W ( Kèm 2 Mic )', N'Chính hãng', 3290000, 2796500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/03/loa-monster-musicbox-go-80w-kem-2-mic-chinh-hang-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1327')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1327', N'Loa Sumico Bella 42 100W (Kèm 2 Mic )', N'Chính hãng', 3990000, 3391500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/03/loa-sumico-bella-42-100w-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1328')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1328', N'Loa Olike S4', N'Chính hãng', 290000, 246500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/04/15/loa-olike.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1329')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1329', N'Loa Bluetooth JBL Go4', N'JBL', 890000, 756500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/05/06/loa-bluetooth-jbl-go4-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1330')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1330', N'Loa Bluetooth JBL Xtreme 4', N'JBL', 7790000, 6621500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/05/06/loa-bluetooth-jbl-xtreme-4-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1331')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1331', N'Loa Bluetooth JBL PartyBox Club 120', N'JBL', 9090000, 7726500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/05/06/partybox-club-120-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1332')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1332', N'Loa Bluetooth PartyBox Stage 320', N'Chính hãng', 15600000, 13260000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/06/partybox-stage-320.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1333')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1333', N'Loa Bluetooth JBL CLIP 5', N'JBL', 1790000, 1521500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/05/06/loa-bluetooth-jbl-clip-5-chinh-hang-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1334')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1334', N'Loa Bluetooth Alpha Works AW-Ride', N'Chính hãng', 309000, 262650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/29/alpha-works-ride-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1335')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1335', N'Loa Karaoke di động Arirang MB2 Pro+', N'Chính hãng', 5790000, 4921500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/06/13/loa-karaoke-di-dong-arirang-mb2-pro-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1336')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1336', N'Loa Karaoke di động Arirang Classic I', N'Chính hãng', 4990000, 4241500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/06/13/loa-karaoke-arirang-classic-i-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1337')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1337', N'Loa Bluetooth Nowgo F1 Lamp Speaker', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/06/20/nowgo-f1-lamp-speaker-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1338')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1338', N'Loa Bluetooth Sony ULT Field 1', N'Sony', 2090000, 1776500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/06/25/sony-ult-field-1-cam-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1339')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1339', N'Loa Karaoke Sony ULT Field 7', N'Sony', 9190000, 7811500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/06/25/loa-bluetooth-sony-ult-field-7-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1340')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1340', N'Loa Karaoke Sony ULT Tower 10', N'Sony', 21790000, 18521500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/06/25/loa-karaoke-sony-ult-tower-10-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1341')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1341', N'Loa Karaoke di động Arirang MKS2', N'Chính hãng', 2590000, 2201500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/09/arirang-mks2-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1342')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1342', N'Loa Bluetooth Baseus AEQUR V2', N'Baseus', 490000, 416500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/08/02/whatsapp-image-2024-08-02-at-11-10-02-am.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1343')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1343', N'Loa Bluetooth Baseus AEQUR VO20', N'Baseus', 790000, 671500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/08/02/loa-bluetooth-baseus-aequr-vo20-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1344')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1344', N'Loa Bluetooth Sounarc P1', N'Chính hãng', 255000, 216750, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/14/bluetooth-sounarc-p1-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1345')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1345', N'Loa Bluetooth Sounarc P2', N'Chính hãng', 360000, 306000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/14/bluetooth-sounarc-p2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1346')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1346', N'Loa Bluetooth Sounarc P3', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/15/sounarc-p3-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1347')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1347', N'Loa Bluetooth Sounarc P4', N'Chính hãng', 820000, 697000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/15/sounarc-p4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1348')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1348', N'Loa Bluetooth Marshall Willen II - Chính Hãng ASH', N'Marshall', 2700000, 2295000, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2024/08/28/marshall-willen-ii-22.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1349')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1349', N'Loa Bluetooth Marshall Emberton III - Chính Hãng ASH', N'Marshall', 4770000, 4054500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2024/08/29/emberton-iii-cream-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1350')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1350', N'Loa Bluetooth Anker Soundcore 3', N'Anker', 1090000, 926500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2024/09/14/anker-soundcore-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1351')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1351', N'Loa Olike S5', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/25/loa-olike-s5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1352')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1352', N'Loa di động SOUNARC XR3 mini', N'Chính hãng', 440000, 374000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/08/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1353')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1353', N'Loa Xiaomi Bluetooth Speaker Mini', N'Xiaomi', 790000, 671500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/12/05/xiaomi-bluetooth-speaker-mini.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1354')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1354', N'Harman Kardon Onyx Studio 9', N'Harman Kardon', 6490000, 5516500, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2024/12/23/harman-kardon-onyx-studio-9-den-1.gif', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1355')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1355', N'Bộ loa di động Arirang MB2s', N'Chính hãng', 6990000, 5941500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/07/arirang-mb2s.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1356')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1356', N'Bộ loa di động Arirang AM1', N'Chính hãng', 5590000, 4751500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/07/arirang-am1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1357')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1357', N'Loa hiệu ALPHA WORKS AW- SONIK PLUS', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/20/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1358')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1358', N'Bộ loa karaoke di động Arirang AM3', N'Chính hãng', 3650000, 3102500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/26/arirang-am3-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1359')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1359', N'Loa Bluetooth Olike S6', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/16/loa-bluetooth-olike-s6-dh.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1360')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1360', N'Loa Bluetooth Olike S2', N'Chính hãng', 150000, 127500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/16/loa-olike-s2-thumb.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1361')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1361', N'Loa Bluetooth JBL Flip 7', N'JBL', 3270000, 2779500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2025/05/05/jbl-flip-7-front-blue-011-x1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1362')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1362', N'Loa di động JBL Go Essential 2', N'JBL', 990000, 841500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2025/07/14/jbl-go-essential-2-hero-blue-014-x1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1363')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1363', N'Loa JBL Partybox Encore 2 (Kèm 2 Mic)', N'JBL', 11490000, 9766500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2025/07/14/ls-jbl-partybox-encore-2-two-mics-034-x2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1364')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1364', N'Loa di động Marshall Middleton II', N'Marshall', 8090000, 6876500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2025/07/23/middleton-ii-black-trans-5_638888659301268010.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1365')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1365', N'Loa Bluetooth kiêm đồng hồ báo thức JBL Horizon 3', N'JBL', 3490000, 2966500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2025/08/01/jbl-horizon-3-front-grey-0012-x2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1366')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1366', N'Loa Bluetooth Xiaomi Sound Party', N'Xiaomi', 2790000, 2371500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2026/02/24/7983291e0ef5f619369728397ed6d33b.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1367')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1367', N'Loa Bluetooth Alpha Works AW-W39', N'Chính hãng', 1490000, 1266500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/24/3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1368')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1368', N'Loa Bluetooth Innoyo InnoSound S1', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/10/03/innosound-s1-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1369')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1369', N'Loa Bluetooth Soul Storm Joy', N'Chính hãng', 340000, 289000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/01/soul-storm-joy.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1370')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1370', N'Loa Bluetooth Soul Storm Mini', N'Chính hãng', 239000, 203150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/12/01/soul-storm-mini-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1371')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1371', N'Loa Bluetooth Nakamichi NBS-116', N'Chính hãng', 349000, 296650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/30/nakamichi-nbs-116-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1372')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1372', N'Loa Bluetooth Alpha Works TREK 12', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/10/awtrek12blu-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1373')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1373', N'Loa Bluetooth Arirang BTA-1525', N'Chính hãng', 480000, 408000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/11/bta-1525-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1374')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1374', N'Loa Bluetooth Arirang BTA-3625', N'Chính hãng', 1790000, 1521500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/11/bta-3625-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1375')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1375', N'Loa JBL Partybox On The Go 2', N'JBL', 9990000, 8491500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/jblpartyotg2-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1376')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1376', N'Loa Party Alpha Works GENBEAT 100', N'Chính hãng', 4590000, 3901500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/genbeat100blk-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1377')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1377', N'Loa Party Alpha Works GENBEAT 120', N'Chính hãng', 6190000, 5261500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/06/genbeat120blk-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1378')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1378', N'Bộ loa karaoke di động Arirang BRAVO 8', N'Chính hãng', 6150000, 5227500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/09/loa-arirang-bravo-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1379')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1379', N'Loa Bluetooth JBL Boombox 4', N'JBL', 13900000, 11815000, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/jbl-boombox-4-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1380')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1380', N'Loa Marshall Stanmore IV', N'Marshall', 12990000, 11041500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2026/08/22/standmore4-3-111726653.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1381')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1381', N'Bộ loa kiểm âm Arirang BA6', N'Chính hãng', 1900000, 1615000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/25/bo-loa-kiem-am-ba6-1-151928115.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1382')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1382', N'Loa Sony ULT TOWER MAX', N'Sony', 39990000, 33991500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/09/08/tower-max-13-105021877.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1383')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1383', N'Loa di động không dây Sony ULT FIELD 5', N'Sony', 6490000, 5516500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/07/11/ult-field-5-hero1-black-whiteback-mid.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1384')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1384', N'Loa Karaoke Sony ULT Tower 9', N'Sony', 24990000, 21241500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/06/17/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1385')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1385', N'Loa vi tính Xiaomi Desktop Speaker', N'Xiaomi', 1190000, 1011500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/06/20/loa-xiaomi-photoroom.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1386')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1386', N'Loa Bluetooth Harman Kardon Aura Studio 5 WiFi', N'Harman Kardon', 10900000, 9265000, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/loa-harman-kardon-aura-studio-5-wifi-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1387')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1387', N'Loa Bluetooth Harman Kardon SOUNDSTICK 5', N'Harman Kardon', 9900000, 8415000, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/loa-bluetooth-harmankardon-soundstick-5-black-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1388')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1388', N'Loa Bluetooth Harman/kardon SOUNDSTICK 5 WiFi', N'Harman Kardon', 11500000, 9775000, 50, N'Chiếc', N'HARMAN', N'https://i.hoanghamobile.vn/Uploads/2026/07/02/loa-bluetooth-harmankardon-soundstick-5-white-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1389')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1389', N'Tai nghe có dây Baseus Encok H17', N'Baseus', 129000, 109650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/05/14/baseus-encok-h17-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1390')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1390', N'Tai nghe không dây Marshall Monitor III A.N.C - Chính hãng ASH', N'Marshall', 9990000, 8491500, 50, N'Chiếc', N'MARSHALL', N'https://i.hoanghamobile.vn/Uploads/2024/11/11/marshall-monitor-iii-a-n-c-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1391')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1391', N'Tai nghe Bluetooth True Wireless Huawei Freebuds 6i', N'Huawei', 990000, 841500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/06/14/wireless-huawei-freebuds-6i-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1392')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1392', N'Tai nghe Bluetooth thể thao SoundPEATS Gofree 2+', N'Chính hãng', 1050000, 892500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/06/17/soundpeats-gofree-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1393')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1393', N'Tai Nghe Bluetooth HONOR CHOICE Earbuds X5', N'Honor', 390000, 331500, 50, N'Chiếc', N'HONOR', N'https://i.hoanghamobile.vn/Uploads/2024/07/16/earbuds-x5-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1394')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1394', N'Tai nghe JBL Live Beam 3', N'JBL', 3490000, 2966500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2024/07/19/jbl-live-beam-3-tim.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1395')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1395', N'Tai nghe không dây Redmi Buds 6 Active', N'Xiaomi', 435000, 369750, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/08/01/redmi-buds-6-active-w-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1396')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1396', N'Tai nghe Redmi Buds 6 Play', N'Xiaomi', 290000, 246500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/09/17/redmi-buds-6-play-black-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1397')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1397', N'Tai nghe Redmi Buds 6 Pro', N'Xiaomi', 1530000, 1300500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/12/27/redmi-bud-6-pro-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1398')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1398', N'Tai nghe Huawei Freebuds Pro 4', N'Huawei', 2990000, 2541500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2024/12/30/freebuds-pro-4-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1399')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1399', N'Tai nghe không dây Riversong Airfly L3 EA227', N'Chính hãng', 240000, 204000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/17/airfly-l3-ea227-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1400')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1400', N'Tai nghe không dây Riversong Airfly L7 EA316', N'Chính hãng', 320000, 272000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/01/17/ea227-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1401')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1401', N'Tai nghe Headphone Bluetooth HAVIT H668BT', N'Chính hãng', 520000, 442000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/14/havit-h668bt-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1402')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1402', N'Tai nghe Bluetooth Open-Ear HAVIT OWS929', N'Chính hãng', 370000, 314500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/30/ows929-trang-6-145758129.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1403')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1403', N'Loa JBL Partybox 330', N'JBL', 16890000, 14356500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2026/10/05/loa-jbl-partybox-stage-330-1-144326186.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1404')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1404', N'Tai nghe không có khung trùm qua đầu hiệu ALPHA WORKS AW-CURVE 100', N'Chính hãng', 220000, 187000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/03/20/alpha-works-curve-100-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1405')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1405', N'Tai nghe KZ GALE', N'Chính hãng', 280000, 238000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/30/kz-gale-1-145428019.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1406')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1406', N'Tai nghe có dây cổng Type-C Baseus Encok C18', N'Baseus', 149000, 126650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/07/30/baseus-encok-c18-1-145941080.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1407')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1407', N'Tai nghe Headphone Bluetooth HAVIT H633BT', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/14/havit-h633bt-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1408')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1408', N'Tai nghe dây Yookie YK66', N'Chính hãng', 39000, 33150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/12/07/tai-nghe-day-yookie-yk66-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1409')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1409', N'Tai nghe Devia Kintone - Chính hãng', N'Chính hãng', 69000, 58650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/image-removebg-preview-2022-11-24t150810-551.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1410')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1410', N'Tai nghe Bluetooth True Wireless Black Shark T9', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/23/true-wireless-black-shark-t9-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1411')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1411', N'Tai nghe Bluetooth True Wireless Gaming Black Shark T11', N'Chính hãng', 430000, 365500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/08/23/tai-nghe-bluetooth-true-wireless-gaming-black-shark-t11-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1412')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1412', N'Tai nghe Bluetooth SoundPEATS Clear-Pods', N'Chính hãng', 430000, 365500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/25/soundpeats-clearpods-den-2-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1413')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1413', N'Tai nghe Bluetooth chụp tai SoundPEATS Space', N'Chính hãng', 820000, 697000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/25/soundpeats-space-9.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1414')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1414', N'True Wireless Baseus AirGo 1 Ring Open-Ear', N'Baseus', 460000, 391000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/06/25/baseus-airgo-1-ring-open-ear-den-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1415')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1415', N'Tai nghe Bluetooth True Wireless Havit TW958 Pro', N'Chính hãng', 380000, 323000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/10/30/tw958-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1416')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1416', N'Tai nghe chụp tai (Có dây) Havit HV - H2218d - Chính hãng', N'Chính hãng', 150000, 127500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/29/image-removebg-preview-2022-11-29t091307-618.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1417')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1417', N'Tai nghe True Wireless chống ồn YAMAHA TW-E7B - Chính hãng', N'Chính hãng', 3290000, 2796500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-35.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1418')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1418', N'Tai nghe Pioneer SE-C3T - Chính Hãng', N'Chính hãng', 280000, 238000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/08/08/2985-tai-nghe-pioneer-se-c3t-black-songlongmedia-1-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1419')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1419', N'Tai nghe MONSTER CLARITY 550 LT - Chính Hãng', N'Chính hãng', 1890000, 1606500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/13/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1420')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1420', N'Tai nghe Bluetooth JBL Live 220BT - Chính hãng', N'JBL', 690000, 586500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2021/07/05/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1421')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1421', N'Tai nghe Bluetooth thể thao SoundPEATS Breezy', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/25/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1422')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1422', N'Tai nghe Bluetooth chụp tai SoundPEATS Space Pro', N'Chính hãng', 1050000, 892500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/03/1-2-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1423')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1423', N'Tai nghe Bluetooth thể thao SoundPEATS RunFree Lite 2', N'Chính hãng', 509000, 432650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/17/tai-nghe-bluetooth-the-thao-soundpeats-runfree-lite-2.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1424')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1424', N'Tai nghe Bluetooth  SoundPEATS Air3 Pro - Chính Hãng', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/06/08/soundpeats-air3-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1425')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1425', N'Baseus Eli Sport 1 Open-Ear', N'Baseus', 950000, 807500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/06/25/baseus-eli-sport-1-open-ear-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1426')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1426', N'Tai nghe có dây Baseus Encok CZ11', N'Baseus', 89000, 75650, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/05/17/baseus-encok-cz11-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1427')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1427', N'Tai nghe Bluetooth SoundPEATS Air5 Lite', N'Chính hãng', 820000, 697000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/03/1-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1428')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1428', N'Loa Di Động Retro Alpha Works W99 (kèm 2 Micro)', N'Chính hãng', 7990000, 6791500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/23/loa-di-ng-retro-alpha-works-w99-black-2-155959095.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1429')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1429', N'Tai nghe không dây Havit TW947 - Chính Hãng', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/06/image-removebg-preview-2022-12-06t155433-831.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1430')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1430', N'Tai nghe chụp tai (Không dây) Havit H2590BT Pro - Chính Hãng', N'Chính hãng', 249000, 211650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/04/01/tai-nghe-chup-tai-havit-h2590bt-pro-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1431')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1431', N'Tai nghe Bluetooth thể thao Havit OWS902', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/09/havit-ows902-2_638561363197548253.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1432')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1432', N'Tai nghe chụp tai có dây Gaming Havit H2040D', N'Chính hãng', 249000, 211650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/17/tai-nghe-chup-tai-co-day-gaming-havit-h2040d-1.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1433')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1433', N'Tai Nghe TWS HAVIT TW982 Life 01T', N'Chính hãng', 240000, 204000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/28/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1434')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1434', N'Tai Nghe TWS HAVIT TW931', N'Chính hãng', 220000, 187000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/28/tai-nghe-tws-havit-tw931.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1435')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1435', N'Tai Nghe TWS SOUL S-Live 30 - Chính Hãng', N'Chính hãng', 330000, 280500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/05/26/tws-soul-s-live-30-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1436')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1436', N'HUAWEI Freebuds Pro 5', N'Huawei', 4490000, 3816500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2026/04/02/freebuds-pro-5-xam.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1437')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1437', N'Loa thanh Xiaomi Soundbar Pro 2.0 QBH4344EU', N'Xiaomi', 2590000, 2201500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/09/30/loa-thanh-xiaomi-soundbar-pro-2-0-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1438')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1438', N'Loa Thanh Xiaomi Soundbar 2.0 QBH4286EU', N'Xiaomi', 1050000, 892500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/05/20/xiaomi-soundbar.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1439')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1439', N'Tai nghe Wireless On-ear Belkin SOUNDFORM Mini', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/04/29/tai-nghe-wireless-on-ear-belkin-soundform-mini-8.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1440')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1440', N'Loa thanh Soundbar Samsung HWT420 - 150W', N'Samsung', 1090000, 926500, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2024/12/10/loa-thanh-soundbar-samsung-hwt420.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1441')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1441', N'Tai nghe Bluetooth chụp tai Alpha Works Flex 680', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/10/28/alpha-works-flex-680-bl-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1442')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1442', N'Tai nghe có dây Xiaomi Type-C Earphones', N'Xiaomi', 165000, 140250, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2024/09/17/n79z-black-800x800.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1443')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1443', N'Tai nghe Bluetooth thể thao SoundPEATS HearFit', N'Chính hãng', 770000, 654500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/07/25/soundpeats-hearfit-8.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1444')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1444', N'Anker Soundcore H30i', N'Anker', 590000, 501500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2024/06/18/anker-soundcore-h30i-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1445')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1445', N'Tai nghe Bluetooth chụp tai JBL T520BT', N'JBL', 820000, 697000, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2023/10/12/jbl-tune-520bt-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1446')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1446', N'Tai nghe chụp tai (Có dây) gaming Rapoo VH500C - Chính hãng', N'Chính hãng', 480000, 408000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/07/vh500c-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1447')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1447', N'Tai nghe chụp tai (Có dây) Gaming Rapoo VH360 - Chính hãng', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/07/vh360-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1448')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1448', N'Tai nghe chụp tai (Có dây) gaming Rapoo VH710 - Chính hãng', N'Chính hãng', 680000, 578000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/07/vh710-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1449')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1449', N'Tai nghe True Wireless Defunc True Lite - Chính Hãng', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/07/12/defunc-true-lite-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1450')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1450', N'Tai nghe True Wireless Defunc TRUE GO SLIM - Chính Hãng', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/07/12/defunc-true-go-slim-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1451')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1451', N'Tai nghe chụp tai (Không dây) JBL Tour ONE M2 - Chính Hãng', N'JBL', 6490000, 5516500, 50, N'Chiếc', N'JBL', N'https://i.hoanghamobile.vn/Uploads/2023/07/05/tai-nghe-jbl-tour-one-m2-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1452')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1452', N'Tai nghe chụp tai (Có dây) Gaming MSI H991 - Chính hãng', N'MSI', 990000, 841500, 50, N'Chiếc', N'MSI', N'https://i.hoanghamobile.vn/Uploads/2023/06/16/h991-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1453')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1453', N'Tai nghe Bluetooth myALO Z-One Pro - Chính hãng', N'Chính hãng', 240000, 204000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/14/tai-nghe-bluetooth-myalo-z-one-pro-3.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1454')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1454', N'Tai nghe Bluetooth myALO X-One - Chính hãng', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/03/tai-nghe-bluetooth-myalo-x-one-b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1455')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1455', N'Tai nghe chụp tai (Có dây) Sennheiser HD 400S - Chính Hãng', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/12/tai-nghe-sennheiser-hd-400s-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1456')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1456', N'Tai nghe chụp tai (Không dây) SENNHEISER HD 450BT - Chính Hãng', N'Chính hãng', 2790000, 2371500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/05/15/sennheiser-hd-450bt-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1457')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1457', N'Tai nghe không dây OPPO Enco Buds 2 - chính hãng', N'OPPO', 690000, 586500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2023/03/24/oppo-buds-2-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1458')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1458', N'Tai nghe KZ EDX Pro - Chính Hãng', N'Chính hãng', 269000, 228650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/03/06/tai-nghe-kz-edx-pro-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1459')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1459', N'Tai nghe KZ ZSN Pro X - Chính Hãng', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/03/06/tai-nghe-kz-zsn-pro-x-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1460')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1460', N'Tai nghe chụp tai Gaming Razer BlackShark V2 X-Wired - RZ04-03240700-R3M1', N'Chính hãng', 1080000, 918000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/10/t0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1461')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1461', N'Tai nghe chụp tai (Không dây) Razer Opus X-Active Noise Cancellation', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/10/h1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1462')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1462', N'Tai nghe True Wireless Monster Mission V1 - Chính Hãng', N'Chính hãng', 2400000, 2040000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/11/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1463')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1463', N'Tai nghe không dây Huawei Freebuds 5i - Chính hãng', N'Huawei', 790000, 671500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2023/01/30/image-removebg-preview-17.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1464')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1464', N'Tai nghe chơi game Asus ROG Cetra II Core - Chính Hãng', N'Asus', 880000, 748000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2022/12/31/image-removebg-preview-45.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1465')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1465', N'Tai nghe ASUS TUF Gaming H1 - Chính Hãng', N'Asus', 740000, 629000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2023/01/04/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1466')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1466', N'Tai nghe Razer Hammerhead True Wireless 2021 RZ12-03820100-R3A - Chính hãng', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/17/image-removebg-preview-2022-12-17t091043-051.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1467')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1467', N'Tai nghe trẻ em Oaxis myFirst BC Wireless Lite - Chính Hãng', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/12/08/image-removebg-preview-2022-12-08t155847-697.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1468')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1468', N'Tai nghe Bluetooth SoundPEATS Air3 Deluxe HS', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/image-removebg-preview-2022-11-24t154005-377.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1469')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1469', N'Tai nghe nhét tai Devia Smart Earpods có Mic', N'Chính hãng', 99000, 84150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/45fd359a1ddcc5332b547c8723e5faaf-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1470')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1470', N'Tai nghe nhét tai Devia Cổng Lightning', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/download-15-9-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1471')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1471', N'Tai nghe Devia Smart Series Dual Speakers 3,5mm', N'Chính hãng', 150000, 127500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/24/image-removebg-preview-2022-11-24t151926-644.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1472')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1472', N'Tai nghe Bluetooth SoundPEATS Air3 Deluxe - Chính Hãng', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/10/17/image-removebg-preview-43.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1473')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1473', N'Tai nghe SENNHEISER CX True Wireless - Chính Hãng', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/30/image-removebg-preview-2022-09-30t165100-885.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1474')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1474', N'Tai nghe SENNHEISER CX 80S - Chính Hãng', N'Chính hãng', 549000, 466650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/30/image-removebg-preview-2022-09-30t165810-725.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1475')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1475', N'Tai nghe chụp tai (Không dây) SENNHEISER HD 350BT - Chính Hãng', N'Chính hãng', 1550000, 1317500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/30/image-removebg-preview-2022-09-30t172908-082.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1476')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1476', N'Tai nghe không dây YAMAHA EP-E30A - Chính Hãng', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-37.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1477')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1477', N'Tai nghe không dây YAMAHA EP-E50A - Chính Hãng', N'Chính hãng', 499000, 424150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-46.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1478')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1478', N'Tai nghe không dây Xiaomi Redmi Buds 4 Pro - Chính hãng', N'Xiaomi', 1190000, 1011500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1479')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1479', N'Tai nghe True Wireless YAMAHA TW-E5B - Chính Hãng', N'Chính hãng', 2190000, 1861500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-26.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1480')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1480', N'Tai nghe True Wireless thể thao YAMAHA TW-ES5A - Chính Hãng', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/09/22/image-removebg-preview-30.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1481')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1481', N'Tai nghe chụp tai (không dây) SENNHEISER MOMENTUM 4 WIRELESS - Chính hãng', N'Chính hãng', 5890000, 5006500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/08/24/image-removebg-preview-53.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1482')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1482', N'Tai nghe Oppo Enco Air 2 Pro - Chính hãng', N'OPPO', 1190000, 1011500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2022/08/15/image-removebg-preview-41.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1483')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1483', N'Tai nghe không dây Huawei Freebuds SE - chính hãng', N'Huawei', 590000, 501500, 50, N'Chiếc', N'HUAWEI', N'https://i.hoanghamobile.vn/Uploads/2022/07/27/image-removebg-preview-3_637945390476320522.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1484')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1484', N'Tai nghe Chụp tai Razer Opus X-Active Noise Cancellation-Trắng(Mercury)', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/28/image-removebg-preview-17.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1485')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1485', N'TAI NGHE TRUE WIRELESS DENON AH - C630W - Chính hãng', N'Chính hãng', 1650000, 1402500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/26/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1486')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1486', N'Tai nghe Razer Hammerhead PRO V2 - Chính Hãng', N'Chính hãng', 750000, 637500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/26/image-removebg-preview-9.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1487')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1487', N'Tai Nghe Bluetooth Razer Hammerhead True Wireless X RZ12 - 03830100 - R3A1', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/26/image-removebg-preview-11.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1488')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1488', N'Tai nghe chụp tai Razer BlackShark V2 X - Wired Gaming Headset - RZ04', N'Chính hãng', 1250000, 1062500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/26/image-removebg-preview-13.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1489')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1489', N'Tai nghe nhét tai Rapoo EP28 - Chính hãng', N'Chính hãng', 139000, 118150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/06/image-removebg-preview-23.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1490')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1490', N'Tai nghe SENNHEISER MOMENTUM TRUE WIRELESS 3 - Chính Hãng', N'Chính hãng', 5090000, 4326500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/06/13/image-removebg-preview-1_637907271514095381.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1491')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1491', N'Tai nghe Bluetooth  SoundPEATS Free2 Classic - Chính Hãng', N'Chính hãng', 420000, 357000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/05/24/image-removebg-preview-38.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1492')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1492', N'Tai nghe truyền dẫn xương Shokz Openrun - Chính Hãng', N'Chính hãng', 3490000, 2966500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/05/31/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1493')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1493', N'Tai nghe MONSTER LIFE ONE - Chính hãng', N'Chính hãng', 1200000, 1020000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/13/opera-snapshot-2022-04-05-140517-hdradio-vn.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1494')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1494', N'Tai nghe Bluetooth PIONEER SE-S3BT(B)CZU - Chính Hãng', N'Chính hãng', 750000, 637500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/03/28/63581-tai-nghe-bluetooth-pioneer-se-s3bt-b-czu-mau-den.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1495')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1495', N'Tai Nghe Bluetooth Earbuds  SoundPEATS Mac 2 - Chính Hãng', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/05/mac2-01-768x768.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1496')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1496', N'Tai nghe trẻ em Oaxis Myfirst Headphones Wireless', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/07/image-removebg-preview-13.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1497')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1497', N'Tai Nghe Bluetooth Motorola MotoBuds 100 - Chính Hãng', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/04/07/image-removebg-preview-15.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1498')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1498', N'Tai  nghe Bluetooth TWS OPPO ENCO Air 2- chính hãng', N'OPPO', 790000, 671500, 50, N'Chiếc', N'OPPO', N'https://i.hoanghamobile.vn/Uploads/2022/03/09/enco-air-2-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1499')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1499', N'Tai nghe Bluetooth QCY T1C - Chính Hãng', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/11/tai-nghe-bluetooth-qcy-t1c-chinh-hang-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1500')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1500', N'Tai nghe không dây Haylou Moripods T33 - Chính Hãng', N'Chính hãng', 560000, 476000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/02/11/tai-nghe-khong-day-haylou-moripods-t33-chinh-hang-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1501')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1501', N'Tai Nghe chụp tai (Không dây) Motorola XT220 - Chính Hãng', N'Chính hãng', 650000, 552500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/12/22/image-removebg-preview-27.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1502')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1502', N'Tai nghe chụp tai (Có dây) Motorola Pulse Bass 200 Wired - Chính Hãng', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/12/22/pb200-11-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1503')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1503', N'Tai Nghe Chụp Tai Dành Cho Game Thủ SoundPEATS G1 - Chính Hãng', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/10/27/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1504')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1504', N'Tai nghe Bluetooth Haylou GT6 - Chính hãng', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/11/08/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1505')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1505', N'Tai nghe có dây Pioneer SE-CL502T - Chính Hãng', N'Chính hãng', 179000, 152150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/10/20/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1506')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1506', N'Tai nghe trẻ em Oaxis myFirst Headphones Bone Conductor Wireless', N'Chính hãng', 599000, 509150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/09/13/image-removebg-preview-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1507')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1507', N'Tai nghe Bluetooth True Wireless Soundpeats Truengine 3 SE - Chính hãng', N'Chính hãng', 800000, 680000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/04/08/image-removebg-preview-18.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1508')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1508', N'Tai nghe Bluetooth Soundpeats TrueShift 2- Chính hãng', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/03/03/image-removebg-preview-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1509')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1509', N'Tai nghe Bluetooth Soundpeats TrueBuds - Chính hãng', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/03/03/image-removebg-preview-5.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1510')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1510', N'True Wireless iWalk Amour Airbuds (sạc không dây) - BTA003 - Chính hãng', N'Chính hãng', 950000, 807500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2020/11/09/tai-nghe-true-wireless-iwalk-amour-airbuds.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1511')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1511', N'Tai nghe chống ồn không dây Sony WH-1000XM6', N'Sony', 9790000, 8321500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/05/21/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1512')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1512', N'Tai nghe không dây chống ồn Sony WF-1000XM6', N'Sony', 7290000, 6196500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/02/27/wf-1000xm6-silver-caseopen2-large.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1513')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1513', N'Tai nghe chụp tai (Không dây) Sony WH-1000XM5 - Chính Hãng', N'Sony', 6250000, 5312500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2022/05/23/image-removebg-preview-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1514')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1514', N'Tai nghe không dây Sony WF-1000XM5 - Chính Hãng', N'Sony', 5290000, 4496500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/07/25/sony-wf-1000xm5-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1515')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1515', N'Tai nghe chụp tai không dây SONY WH-CH730N', N'Sony', 2990000, 2541500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/09/25/ch730-8-155550084.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1516')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1516', N'Tai nghe chụp tai (Không dây) SONY WH-CH720N - Chính Hãng', N'Sony', 2150000, 1827500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/04/14/sony-wh-ch720n-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1517')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1517', N'Tai nghe chụp tai (không dây) SONY WH-CH520 - Chính Hãng', N'Sony', 890000, 756500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/04/14/image-removebg-preview-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1518')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1518', N'Tai nghe Bluetooth chụp tai Sony ULT900N Wear', N'Sony', 3590000, 3051500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/04/23/bluetooth-chup-tai-sony-ult900-wea-6.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1519')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1519', N'Tai nghe chống ồn không dây Sony WF-C710N', N'Sony', 1890000, 1606500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/05/09/wf-c710n-glassblue-case-open-large.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1520')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1520', N'Tai nghe không dây Sony Clip WF-LC900', N'Sony', 4490000, 3816500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/01/29/linkbudsclip-lavender-caseopen2-mid.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1521')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1521', N'Tai nghe chụp tai Sony 1000X THE COLLEXION', N'Sony', 16990000, 14441500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/1000x-the-colletion-w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1522')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1522', N'Tai nghe chụp tai không dây SONY WH-CH530', N'Sony', 1190000, 1011500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/09/25/ch530-3-5-153559961.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1523')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1523', N'Sony MDR-ZX110AP - Chính hãng', N'Sony', 420000, 357000, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2020/09/21/Artboard%201%20copy%2044.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1524')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1524', N'Sony MDR-EX155AP - Chính hãng', N'Sony', 290000, 246500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2020/09/21/Artboard%201%20copy%2042.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1525')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1525', N'Sony MDR-ZX310AP - Chính hãng', N'Sony', 630000, 535500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2020/09/21/Artboard%201%20copy%2045.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1526')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1526', N'Sony MDR-EX15AP - Chính hãng', N'Sony', 180000, 153000, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2020/09/24/MDR-EX15AP.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1527')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1527', N'Tai nghe chống ồn không dây Sony WF-C700N - Chính Hãng', N'Sony', 1990000, 1691500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/04/15/wf-c700n-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1528')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1528', N'Tai nghe Bluetooth True Wireless Gaming Sony INZONE Buds', N'Sony', 3650000, 3102500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/11/14/gaming-sony-inzone-buds-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1529')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1529', N'Tai nghe Gaming chụp tai không dây Sony INZONE H5', N'Sony', 2490000, 2116500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2023/11/14/sony-inzone-h5-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1530')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1530', N'Tai nghe chụp tai Gaming Sony Inzone H7', N'Sony', 1990000, 1691500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/02/28/tai-nghe-gaming-sony-inzone-h7-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1531')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1531', N'Tai nghe Bluetooth True Wireless Sony WF-C510', N'Sony', 890000, 756500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2024/09/14/sony-wf-c510-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1532')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1532', N'Tai nghe Sony nhét tai có dây cổng USB-C IER-EX15C', N'Sony', 449000, 381650, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2025/10/22/sony-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1533')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1533', N'Tai nghe Gaming có dây SONY INZONE H3 MDR-G300', N'Sony', 1090000, 926500, 50, N'Chiếc', N'SONY', N'https://i.hoanghamobile.vn/Uploads/2026/07/18/sony-inzone-h3-mdr-g300-1-110414465.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1534')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1534', N'Ốp Lưng TechWoven MagSafe cho iPhone 18 Pro Max', N'Apple', 1640000, 1394000, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/19/techwoven-magsafe-cho-iphone-18-pro-max-tia-4-115309259.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1535')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1535', N'Ốp Lưng Silicon MagSafe cho iPhone 18 Pro Max', N'Apple', 1490000, 1266500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/09/19/silicon-magsafe-cho-iphone-18-pro-max-xanh-biec-4-114033523.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1536')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1536', N'Ốp Lưng Silicon MagSafe cho iPhone 15 Pro Max', N'Apple', 590000, 501500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/10/02/apple-silicone-magsafe-case-15-pro-max-orange.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1537')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1537', N'Ốp Lưng Trong Suốt MagSafe cho iPhone 15 Pro', N'Apple', 490000, 416500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/22/clear-case-iphone-15-pro-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1538')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1538', N'Miếng dán CL ZEELOT (2.5D+) SOLIDsleek - iP15 Pro Max 6.7" - Retina Clear', N'Chính hãng', 130000, 110500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/19/solidsleek-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1539')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1539', N'Miếng dán CL ZEELOT (2.5D+) SOLIDsleek - iPhone 15 Pro 6.1" - Retina Clear', N'Apple', 99000, 84150, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/19/solidsleek-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1540')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1540', N'Miếng dán CL ZEELOT (2.5D+) SOLIDsleek - iPhone 15 6.1" - Retina Clear', N'Apple', 175000, 148750, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/09/19/solidsleek-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1541')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1541', N'Sạc mophie Essentials PD 30W 1A1C', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/18/sac-mophie-essentials-pd-30w-1a1c-7.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1542')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1542', N'Thay màn hình Pisen iPhone 11 Pro Max', N'Apple', 1090000, 926500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2025/02/10/manhinhiphone-11-pro-max-pisen1-3776.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1543')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1543', N'Thay pin Pisen iPad mini 2', N'Apple', 390000, 331500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/pin-ipad-pisen-mini-2-142939932.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1544')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1544', N'Pin Energizer Iphone 15 Plus ECA15PL4383P', N'Apple', 1190000, 1011500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/07/22/pin-energizer-eca133550p-1-113403797.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1545')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1545', N'Thay màn hình GX OLED iPhone 17 Pro Max', N'Apple', 8990000, 7641500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/08/05/gx-oled-iphone-17-pro-max-145741719.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1546')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1546', N'Thay màn hình GX OLED iPhone 16 Plus', N'Apple', 4190000, 3561500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/08/05/gx-oled-iphone-16-plus-145513402.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1547')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1547', N'Pin Pisen iPhone 16 dung lượng chuẩn 3561mAh', N'Apple', 1290000, 1096500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/07/23/pin-pisen-cho-iphone-1-102023372.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1548')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1548', N'Pin Pisen iPhone 16 Pro dung lượng chuẩn 3582mAh', N'Apple', 1690000, 1436500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/07/23/pin-pisen-cho-iphone-1-102023372.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1549')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1549', N'Pin Pisen iPhone 16 Pro Max dung lượng chuẩn 4685mAh', N'Apple', 1790000, 1521500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2026/07/23/pin-pisen-cho-iphone-1-102023372.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1550')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1550', N'Cổng chuyển đổi Baseus UltraJoy Series 7-Port HUB Docking Station', N'Baseus', 420000, 357000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/17/cong-chuyen-doi-baseus-ultrajoy-series-7-port-hub-docking-station-1-152600553.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1551')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1551', N'Hub chuyển đổi Lention USB-C 6in1 C16s', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/29/lention-usb-c-6in1-c16s-2-101116904.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1552')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1552', N'Chuyển đổi cổng Pisen Lightning to Dual Lightning LS-QMS-E003', N'Chính hãng', 99000, 84150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/27/chuyen-doi-cong-pisen-lightning-to-dual-lightning-ls-qms-e003-1-160328369.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1553')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1553', N'Cổng chuyển OTG Lightning to Type C Model OT01', N'Chính hãng', 99000, 84150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/27/ot01-1-161946564.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1554')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1554', N'Cổng chuyển OTG SIO Type C to Lightning Model OT02', N'Chính hãng', 99000, 84150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/27/ot02-1-161947048.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1555')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1555', N'Cổng chuyển OTG SIO Lightning to Type C 480 Mbps Model OT03', N'Chính hãng', 99000, 84150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/27/ot03-1-161947189.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1556')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1556', N'Cổng chuyển OTG SIO USB A – Type C Model OT04', N'Chính hãng', 120000, 102000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/27/ot04-1-161947376.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1557')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1557', N'Hub chuyển đổi Lention USB-C 7 trong 1 CE18HCR', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/23/hub-chuy-n-i-lention-ce18hcr-1-143534284.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1558')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1558', N'Bộ phát Wifi di động 4G TP Link M7350 - Chính hãng', N'Chính hãng', 1360000, 1156000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/03/03/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1559')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1559', N'Router Wifi TP-Link Archer C54 băng tần kép AC1200 - Chính hãng', N'Chính hãng', 409000, 347650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/12/16/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1560')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1560', N'USB Wifi TP-Link T3U Plus Băng tần kép AC1300 - Chính hãng', N'Chính hãng', 370000, 314500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/03/11/image-removebg-preview-16.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1561')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1561', N'Thiết bị thu sóng USB Wi-Fi Asus AC53 Nano băng tần kép AC1200 - Chính hãng', N'Asus', 270000, 229500, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/11/25/04-asus-usb-wifi-usb-ac53-nano-hai-bang-tan-ac1200.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1562')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1562', N'Router Wifi 4G TP-Link Archer MR100 300Mbps - Chính hãng', N'Chính hãng', 860000, 731000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/02/23/mr100-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1563')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1563', N'Router Wifi TP-LINK Tốc độ 300Mbps chuẩn N - TL - WR840N - Chính hãng', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/18/image-removebg-preview_637937398960473547.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1564')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1564', N'Router Wifi Chuẩn N 300Mbps Asus RT-N12+ - Chính hãng', N'Asus', 280000, 238000, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/11/25/rt-n12-asus-wifi-router-01.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1565')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1565', N'Router wifi ASUS RT-AC750L - Chính hãng', N'Asus', 475000, 403750, 50, N'Chiếc', N'ASUS', N'https://i.hoanghamobile.vn/Uploads/2025/11/25/rt-ac750l-05.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1566')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1566', N'Sạc dự phòng UGREEN 10000mAh 55W PB727 tích hợp cáp Type C', N'Ugreen', 790000, 671500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/10/21/ugreen-10000mah-55w-pb727-1-160819717.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1567')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1567', N'Sạc Dự Phòng AUKEY Spark Mega 27600mAh 140W PB-Y63', N'Chính hãng', 1500000, 1275000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/19/aukey-spark-mega-27600mah-140w-pb-y63-1-110255692.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1568')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1568', N'Sạc Dự Phòng AUKEY 20000mAh 35W PB-Y92L', N'Chính hãng', 850000, 722500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/19/aukey-20000mah-35w-pb-y92l-1-103948772.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1569')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1569', N'Sạc Dự Phòng AUKEY 10000mAh 20W PB-Y95', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/29/sac-du-phong-aukey-10000mah-20w-pb-y95-grey-1-164921060.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1570')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1570', N'Sạc Dự Phòng Không Dây AUKEY 10000mAh 20W PB-MS22', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/19/aukey-10000mah-20w-pb-ms22-xam-1-091519938.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1571')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1571', N'Sạc dự phòng POWERCUBE by MIPOW Ultra-Mini Aluminum 10000 mAh', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/13/powercube-mipow-x-miffy-ultra-mini-aluminum-10000-mah-burgundy-104519667.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1572')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1572', N'Sạc dự phòng POWERCUBE Mipow x Miffy Ultra-Mini Aluminum 10000 mAh', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/10/13/powercube-mipow-x-miffy-ultra-mini-aluminum-10000-mah-102056294.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1573')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1573', N'Sạc dự phòng hiệu Baseus EnerFill FM12 10000mAh 22.5W', N'Baseus', 750000, 637500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/10/12/sac-du-phong-hieu-baseus-enerfill-fm12-10000mah-22-5w-1-133422082.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1574')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1574', N'Sạc không dây 15W INNOSTYLE MAGEASE WIRELESS CHARGER', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/09/15/sac-15w-innostyle-magease-wireless-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1575')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1575', N'Củ sạc chính hãng Apple 20W Type-C', N'Apple', 450000, 382500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/10/02/20w.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1576')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1576', N'Cáp Lightning Mophie 1M Black/White - Chính hãng', N'Chính hãng', 190000, 161500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2020/09/19/C&#xE1', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1577')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1577', N'Cáp Innostyle Jazzy Usb-A To Usb-C 1.2M Hỗ Trợ Sạc Nhanh 15W - Chính Hãng', N'Chính hãng', 45000, 38250, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2020/09/25/6_637366414340727657.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1578')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1578', N'Sạc Innostyle MINIGO 2 USB A 12W Smart Ai Charging - Chính Hãng', N'Chính hãng', 160000, 136000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/09/16/cu-sac-143851326.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1579')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1579', N'Cáp Innostyle Jazzy Usb-A To Micro 1.2M Công Suất 10W - Chính Hãng', N'Chính hãng', 35000, 29750, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2020/09/25/5_637366412600127644.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1580')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1580', N'Sạc mophie 45W Dynamic Charger With 60W Max', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/11/03/Gallery/589880_sac-mophie-45w-dynamic-charger-with-60w-max-white%20(1).png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1581')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1581', N'Bộ phụ kiện Gopro Accessory Kit - Chính Hãng', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/06/01/whatsapp-image-2022-06-01-at-11-42-23-1.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1582')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1582', N'Chân máy tripod từ tính gấp gọn Ulanzi MA60', N'Chính hãng', 420000, 357000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/20/ulanzi-ma60-1-103146117.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1583')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1583', N'Chân máy tripod máy ảnh tự động mở Ulanzi MT-80', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/20/ulanzi-mt80-1-100620019.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1584')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1584', N'Gậy chụp ảnh tripod từ tính Ulanzi MT85', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/20/ulanzi-mt85-black-1-092837113.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1585')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1585', N'Micro có dây Boston Acoustics, BAM1', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/micro-co-day-bam-1-1-093315331.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1586')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1586', N'Gậy chụp ảnh Tripod WiWU Wi-SE015 (có đèn LED Ring & Xoay 360)', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/21/tripod-wiwu-wi-se015-1-101732800.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1587')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1587', N'Gậy chụp ảnh Tripod WIWU Wi-SE012', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/21/tripod-wiwu-wi-se012-1-095341859.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1588')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1588', N'Gậy chụp ảnh Tripod Mini Wiwu Wi-SE011 (có đèn chiếu sáng)', N'Chính hãng', 290000, 246500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/07/21/tripod-mini-wiwu-wi-se011-1-092150659.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1589')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1589', N'Vòng giữ điện thoại PITAKA Grip 3', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/14/avatar-220804-1-png-500x350.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1590')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1590', N'Củ sạc ô tô Baseus GoTrip DP1 C+U 30W', N'Baseus', 170000, 144500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/29/baseus-gotrip-dp1-c-u-30w-9-164605876.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1591')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1591', N'Sạc ô tô PISEN QUICK Car Tiny 20W 1C1A CL1200 TP-C77', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/pisen-quick-car-tiny-20w-1c1a-cl1200-tp-c77-150804610.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1592')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1592', N'Miếng dán cường lực màn hình đồng hồ Araree Core Glass 47mm', N'Chính hãng', 49000, 41650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/araree-core-glass-galaxy-watch-6-classic-43mm-1-145101764.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1593')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1593', N'Miếng dán cường lực màn hình đồng hồ Araree Core Glass 43mm', N'Chính hãng', 49000, 41650, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/araree-core-glass-galaxy-watch-6-classic-43mm-1-145101764.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1594')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1594', N'Miếng dán màn hình đồng hồ Araree Core Glass - Galaxy Watch 5th/6th 40mm', N'Samsung', 49000, 41650, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/cuong-luc-araree-watch5-6-1-091506557.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1595')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1595', N'Sạc Pisen Dual USB Car 2.4A 12W', N'Chính hãng', 90000, 76500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/18/s-c-pisen-dual-usb-car-2-4a-12w-1-160730537.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1596')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1596', N'Miếng dán cường lực màn hình Araree Core Glass - Galaxy Watch', N'Samsung', 49000, 41650, 50, N'Chiếc', N'SAMSUNG', N'https://i.hoanghamobile.vn/Uploads/2026/08/18/dan-man-hinh-araree-core-glass-galaxy-watch-1-084542743.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1597')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1597', N'Sạc dự phòng Baseus EnerCore CR11 kèm cáp Type C 10000mAh 45W', N'Baseus', 590000, 501500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/25/baseus-enercore-cr11-10000mah-45w-160527766.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1598')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1598', N'Sạc dự phòng Baseus EnerCore CR11 kèm cáp Type C 20000mAh 67W', N'Baseus', 790000, 671500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/17/sac-du-phong-hieu-baseus-enercore-cr11-kem-cap-type-c-20000mah-67w-154108222.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1599')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1599', N'Sạc dự phòng Baseus Magnetic 20W 10.000 mAh', N'Baseus', 450000, 382500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2023/12/19/baseus-magnetic-mini-20w-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1600')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1600', N'Sạc dự phòng Anker Nano 2C1A 45W 10000mah A1638', N'Anker', 1090000, 926500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/03/21/a1638-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1601')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1601', N'Sạc dự phòng mophie Essentials Snap+ powerstation with Stand 10,000mAh', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/33-3-png-500x350.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1602')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1602', N'Sạc dự phòng mophie Essentials Snap+ 10,000mAh Metallic - 401116530', N'Chính hãng', 1190000, 1011500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/mophie-111-png-500x350.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1603')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1603', N'Sạc dự phòng Xiaomi Power Bank 10000mAh 22.5W Lite', N'Xiaomi', 490000, 416500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/sac-du-phong-xiaomi-ultra-slim-power-bank-5000mah.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1604')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1604', N'Sạc dự phòng HyperJuice Wireless 5000mah iPhone 12 (HJ-WL61TC) - Chính hãng', N'Apple', 350000, 297500, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2021/04/08/image-removebg-preview-17.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1605')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1605', N'Pin dự phòng Energizer 10000mAh/3.7V Li-Polymer - QP10000PQBK - Chính hãng', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2021/05/05/image-removebg-preview-13.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1606')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1606', N'Pin dự phòng Baseus Bipow màn hình LCD 10000mAh 15W - Chính Hãng', N'Baseus', 199000, 169150, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2022/06/08/image-removebg-preview-78.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1607')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1607', N'Pin sạc dự phòng Pisen Pro Magsafe Power Station, D110-PD-1, 10000mAh, 22.5', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/07/27/image-removebg-preview-29.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1608')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1608', N'Pin sạc dự phòng Innostyle PowerMag Slim 15W (WIRELESS) PD/QC3.0 20W 10000m', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/08/16/image-removebg-preview-42.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1609')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1609', N'Pin sạc dự phòng Baseus Qpow2 màn hình số 10.000mAh 30W', N'Baseus', 450000, 382500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/11/11/annotation-2024-11-11-113247.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1610')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1610', N'Sạc DP Innostyle Powermag duo 2-in-1 10000mAh cho iPhone + Apple Watch', N'Apple', 545000, 463250, 50, N'Chiếc', N'APPLE', N'https://i.hoanghamobile.vn/Uploads/2023/12/13/powermag-duo-2-in-1-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1611')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1611', N'Sạc dự phòng Baseus Airpow Fast Charge 20000mAh 20W', N'Baseus', 390000, 331500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/06/10/baseus-airpow-fast-charge-20000mah-20w.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1612')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1612', N'Sạc dự phòng Energizer 10,000mAh /3.7V Li-Polymer - QM10002PQ', N'Chính hãng', 645000, 548250, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/03/26/qm10002pq.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1613')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1613', N'Sạc dự phòng Energizer 5,000mAh /3.7V Li-Polymer - QM5000', N'Chính hãng', 450000, 382500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/03/26/qm5000-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1614')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1614', N'Pin dự phòng Magsafe Stargo Star X 10.000mAh 22.5W', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/24/magsafe-stargo-star-x-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1615')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1615', N'Sạc dự phòng Innostyle Powermag 15W 2 IN 1 Stand 10000 mAh PD 20W', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/11/09/image-removebg-preview-64.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1616')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1616', N'Sạc dự phòng Innostyle Powersmart+PD 20W 10000Mah OutPut 1C2A (3 cổng ra)', N'Chính hãng', 199000, 169150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/12/25/innostyle-powersmart.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1617')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1617', N'Sạc dự phòng 9Fit Lite 20,000mAh/3.7V Li-Polymer', N'Chính hãng', 310000, 263500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/18/9fit-lite-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1618')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1618', N'Sạc dự phòng 9Fit Lite 10,000mAh /3.7V Li-Polymer', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/07/28/sac-du-phong-9fit-lite-10-000mah-3-7v-li-polymer-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1619')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1619', N'Sạc Dự Phòng Innostyle Powersmart 10000Mah 12W – IP12SA', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/08/07/ip12-black.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1620')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1620', N'Sạc dự phòng Yeslink kèm cáp YYC001 (Có chứng chỉ CCC)', N'Chính hãng', 320000, 272000, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/08/15/z6900030873523-6ab3eee0f2c9a61b30a58bef3091dc93-scaled.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1621')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1621', N'Sạc dự phòng mophie Essentials Snap + 5,000mAh', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/09/10/avatar15-png-500x350.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1622')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1622', N'Sạc Dự Phòng Baseus EnerFill FC11 22.5W 20000mAh - with Dual Built-in Cable', N'Baseus', 650000, 552500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2025/10/17/1759313924560-ce1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1623')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1623', N'Pin sạc dự phòng không dây Baseus Nomos Qi2 10000mAh 45W', N'Baseus', 690000, 586500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2025/12/24/5-d5bb3537d72c488ba2e1b71dfd02f956-1024x1024.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1624')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1624', N'Sạc dự phòng StarGO HyperCharge 100W 25000mAh', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/05/stargo-hypercharge-100w-25000mah-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1625')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1625', N'Pin dự phòng BMX Solid-State Magsafe 5000mAh Qi2 có màn hình LED', N'Chính hãng', 1790000, 1521500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/22/b939-05-si-hero-cord-copy.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1626')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1626', N'Pin dự phòng BMX Solid-State Magsafe 10.000mAh Qi2 có màn hình LED', N'Chính hãng', 2490000, 2116500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/01/22/b939-10-or-hero-copy.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1627')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1627', N'Pin dự phòng Baseus Enerfill 10.000mAh 15W FP11', N'Baseus', 250000, 212500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/01/27/baseus-enerfill-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1628')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1628', N'Sạc dự phòng hiệu Baseus Airpow Fast Charge 10000mAh 20W', N'Baseus', 270000, 229500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/01/29/1769573356018-0-4.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1629')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1629', N'Sạc dự phòng Innostyle MultiCharge Go 22.5W 10000mAh kèm 3 cáp', N'Chính hãng', 450000, 382500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/02/07/innostyle-multicharge-go.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1630')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1630', N'Sạc dự phòng Innostyle MultiCharge Go 20000 mAh (kèm 4 cáp) - MCG20K', N'Chính hãng', 890000, 756500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/03/mcg20k.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1631')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1631', N'Sạc dự phòng Energizer 10,000mAh /3.85V Li-Polymer - UE10091PQGY', N'Chính hãng', 590000, 501500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/03/14/ue10091pqgy-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1632')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1632', N'Sạc dự phòng Anker Zolo 1C1A 45W 20000mah A1681', N'Anker', 790000, 671500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/06/15/pin-du-phong-anker-hong.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1633')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1633', N'Sạc dự phòng Anker Zolo 1C1A 165W 25000mah A1695', N'Anker', 1750000, 1487500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/03/21/a1695-den.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1634')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1634', N'Pin dự phòng Stargo Star X2 20.000mAh 22.5W (Có chứng chỉ CCC)', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2025/02/24/stargo-star-x2-0.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1635')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1635', N'Sạc hít từ tính Innostyle Transimag 15W (Wireless) PD 20W 10000mAh - IQ20', N'Chính hãng', 545000, 463250, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/05/29/innostyle-transimag-10000mah-2.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1636')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1636', N'Pin sạc dự phòng Golf 20.000mAh tích hợp dây L206', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/06/03/golf-20-000mah-tich-hop-day-l206-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1637')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1637', N'Sạc Dự Phòng Baseus EnerFill FC41 20000mAh 100W màn hình led', N'Baseus', 1090000, 926500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/19/baseus-enerfill-fc41-20000mah-100w-black-1-105327634.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1638')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1638', N'Sạc dự phòng Baseus EnerFill FC41 kèm 2 cáp USB-C 25000mAh 145W', N'Baseus', 1390000, 1181500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/09/22/sac-du-phong-baseus-enerfill-fc41-digital-display-with-dual-built-in-usb-c-cables-20000mah-100w-1-153606688.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1639')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1639', N'Pin sạc dự phòng Baseus Bipow màn hình LCD 10000mAh 20W', N'Baseus', 245000, 208250, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/07/17/baseus-bipow-man-hinh-lcd-10000mah-20w.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1640')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1640', N'Sạc dự phòng Baseus PicoGo AM41 Ultra-Slim Qi2 Magnetic 10000mAh 27W', N'Baseus', 750000, 637500, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2026/10/09/baseus-picogo-am41-ultra-slim-qi2-magnetic-10000mah-27w-den-110537034.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1641')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1641', N'Sạc dự phòng POWERPULSE 10000mAh 20W', N'Chính hãng', 250000, 212500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/powerpulse-i-10000mah-20w-1-160807554.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1642')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1642', N'Sạc dự phòng Energizer 10000mAh 3.7V Li-Polymer QE10009PQBK', N'Chính hãng', 299000, 254150, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/19/energizer-10-000mah-3-7v-li-polymer-qe10009pqbk-1-155346179.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1643')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1643', N'Sạc dự phòng Xiaomi Magnetic Power Bank 5000mAh', N'Xiaomi', 650000, 552500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/sac-du-phong-xiaomi-magnetic-power-bank-5000mah.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1644')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1644', N'Sạc dự phòng Xiaomi 165W Power Bank 10000mAh (Integrated Cable)', N'Xiaomi', 1490000, 1266500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/sac-du-phong-xiaomi-165w-power-bank-10000mah-integrated-cable.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1645')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1645', N'Sạc dự phòng Xiaomi Ultra Slim Power Bank 5000mAh', N'Xiaomi', 690000, 586500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/xiaomi-ultra-slim-power-bank-5000mah.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1646')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1646', N'Pin dự phòng Innostyle PowerMini II 10000mAh', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/11/06/amazon-powergo-mini-01-600x600.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1647')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1647', N'Pin dự phòng Pisen Quick - PowerLink Tiny 30W (L & C)', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/20/pin-du-phong-pisen-quick-powerlink-tiny-30w-l-c-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1648')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1648', N'Sạc dự phòng Energizer PD65W 27,000mAh/3.7V Li-Polymer - XP27001PD', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2024/09/17/xp27001pd-4.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1649')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1649', N'Sạc dự phòng Baseus Magnetic Mini Fast Charge Type-C Edition 10000mAh 30W', N'Baseus', 620000, 527000, 50, N'Chiếc', N'BASEUS', N'https://i.hoanghamobile.vn/Uploads/2024/09/05/image-removebg-preview.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1650')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1650', N'Sạc dự phòng Energizer 10,000mAh /3.7V Li-Polymer - QE10013PQ', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/30/qe10013pq-3.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1651')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1651', N'Sạc dự phòng Innostyle Powermax PD 20W 20000mAh IP20KPDBLK - Chính hãng', N'Chính hãng', 390000, 331500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2023/06/09/innostyle-powermax-pd-20w-20000mah-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1652')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1652', N'Sạc dự phòng Xiaomi 18W Power Bank 30000mAh', N'Xiaomi', 690000, 586500, 50, N'Chiếc', N'XIAOMI', N'https://i.hoanghamobile.vn/Uploads/2025/02/14/xiaomi-18w-power-bank-30000mah.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1653')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1653', N'Sạc Anker MagGo 2 Pro Polymer 10000mAh Magnetic Qi2 25W Type C PD 45W A110R', N'Anker', 2390000, 2031500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/09/18/sac-du-phong-anker-maggo-2-pro-polymer-10000mah-khong-day-magnetic-qi2-25w-type-c-pd-45w-a110r-1-085750889.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1654')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1654', N'Sạc dự phòng Anker Zolo 1C1A 35W 10000 - A110L', N'Anker', 800000, 680000, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/anker-zolo-1c1a-35w-10000-a110l-black-093145295.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1655')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1655', N'Sạc dự phòng Anker Zolo 22.5W 20000 - A110E', N'Anker', 750000, 637500, 50, N'Chiếc', N'ANKER', N'https://i.hoanghamobile.vn/Uploads/2026/08/27/anker-zolo-22-5w-20000-a110e-black-090424704.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1656')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1656', N'Sạc dự phòng Energizer 10000mAh 3.7V Li-Polymer - UE10050CCBK', N'Chính hãng', 350000, 297500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/21/energizer-ue10050ccbk-1-105026038.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1657')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1657', N'Sạc dự phòng Energizer 10.000mAh 22.5W Li-Polymer - UE10095PQ', N'Chính hãng', 550000, 467500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/21/energizer-10-000mah-22-5w-li-polymer-ue10095pq-1-101259215.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1658')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1658', N'Pin Blaupunkt BPB2212 10.000mAh PD22.5W màn hình Led', N'Chính hãng', 230000, 195500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/08/06/1-145720161.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1659')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1659', N'Pin sạc dự phòng 20000mAh CUKTECH PB200N 55W', N'Chính hãng', 690000, 586500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/06/08/da0daa38-9316-4d10-9c05-b3f5316acf66_639165331122270976.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1660')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1660', N'Sạc dự phòng Cuktech 15 Air có màn hình 15000mAh 100W PB150S', N'Chính hãng', 1475000, 1253750, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/05/26/pin-du-phong-cuktech-pb150s-15000mah-100w.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1661')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1661', N'Sạc dự phòng không dây 10000mAh Ugreen PB561', N'Ugreen', 650000, 552500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/ugreen-pb561-be.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1662')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1662', N'Sạc dự phòng UGREEN PB761 Qi2 Magsafe 10000mAh', N'Ugreen', 890000, 756500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/pb761-qi2-magsafe-10000mah.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1663')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1663', N'Sạc dự phòng UGREEN PB724 12000mAh 100W 35526B', N'Ugreen', 1190000, 1011500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/pb724.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1664')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1664', N'Sạc dự phòng 20000mAh Ugreen PB721 2C1A 130W', N'Ugreen', 1390000, 1181500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/ugreen-pb721-2c1a-130w-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1665')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1665', N'Sạc dự phòng 25000mAh 200W Ugreen PB722 35525B', N'Ugreen', 1690000, 1436500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/ugreen-pb722-35525b-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1666')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1666', N'Sạc Dự Phòng 20000mAh Ugreen PB505 tích hợp cáp sạc', N'Ugreen', 690000, 586500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/ugreen-pb505-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1667')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1667', N'Sạc Dự Phòng 10000mAh UGREEN PB511 với Cáp USB-C Tích Hợp', N'Ugreen', 790000, 671500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/ugreen-pb511-den-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1668')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1668', N'Sạc dự phòng không dây 10000mAh UGREEN PB764', N'Ugreen', 1150000, 977500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/13/pb764-tim-1_639116891303349864.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1669')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1669', N'Pin Sạc Dự Phòng Năng Lượng Cao UGREEN PB312 – 20000mAh Sạc Nhanh Hai Chiều', N'Ugreen', 490000, 416500, 50, N'Chiếc', N'UGREEN', N'https://i.hoanghamobile.vn/Uploads/2026/04/11/pb312-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1670')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1670', N'Sạc dự phòng Cuktech 20000mAh Power Bank Ultra 210W PB200U', N'Chính hãng', 1690000, 1436500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/11/cuktech-20000mah-power-bank-ultra-210w-pb200u-1.png', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1671')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1671', N'Sạc dự phòng Cuktech CP24 20000mAh (Built-in Type-C và Lightning) LPB200NL', N'Chính hãng', 850000, 722500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/11/pin-du-phong-cuktech-cp24-power-bank-20-000mah-2-cap-built-in-type-c-lightning-mfi-lpb200nl-2.jpeg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1672')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1672', N'Sạc dự phòng Cuktech 20 Power Bank 25000mAh 210W P23', N'Chính hãng', 1990000, 1691500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/sac-du-phong-cuktech-25000mah-210-2.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1673')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1673', N'Sạc dự phòng Cuktech 15 SE Power Bank 20000mAh 85W PB200', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/cuktech-15-se-power-bank-20-000mah-150w-pd3.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1674')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1674', N'Sạc dự phòng Cuktech CP13 10000mAh 33W (cáp Type-C Build-In) LPB100', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/cuktech-cp13-10-000mah-power-bank-30w-lpb100.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1675')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1675', N'Sạc dự phòng Cuktech 15 Power Bank 20000mAh 150W PB200P', N'Chính hãng', 1290000, 1096500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/11/pin-sac-du-phong-p19-cuktech-p-series-20000mah-150w-power-bank-pb200p.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1676')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1676', N'Sạc dự phòng Cuktech Mini Power Bank 10000mAh PB1055', N'Chính hãng', 790000, 671500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/cuktech-pb1055.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1677')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1677', N'Sạc dự phòng Cuktech Plus Power Bank 15000mAh 150W PB150P', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/cuktech-plus-power-bank-pb150p-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1678')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1678', N'Sạc dự phòng Cuktech 10000mAh Power Bank 30W PB100S', N'Chính hãng', 490000, 416500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/cuktech-10000mah-power-bank-30w-pb100s-1.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1679')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1679', N'Sạc dự phòng Cuktech Power Bank 10000mAh 150W PB100P', N'Chính hãng', 990000, 841500, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2026/04/10/pin-sac-du-phong-cuktech-pb100.jpg', 1);
IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'MH1680')
    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)
    VALUES (N'MH1680', N'Pin sạc dự phòng Golf Candy 10.000mAh - G80 - Chính hãng', N'Chính hãng', 145000, 123250, 50, N'Chiếc', N'KHAC', N'https://i.hoanghamobile.vn/Uploads/2022/01/04/fd204af496bdf6254b6472157421dd03-f3506ae7240a4a1f87718512aaecddfc-1024x1024-removebg-preview.png', 1);
