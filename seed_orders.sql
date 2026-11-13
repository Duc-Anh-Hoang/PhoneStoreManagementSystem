-- 1. Thêm khách hàng mẫu
INSERT INTO khachhang (makh, hoten, sdt, diachi, email) VALUES
('KH010', 'Trần Bảo Long', '0901234501', '120 Hai Bà Trưng, Quận 1, TP. HCM', 'long.tran@gmail.com'),
('KH011', 'Lê Ngọc Mai', '0901234502', '45 Lê Duẩn, Quận 1, TP. HCM', 'mai.le@gmail.com'),
('KH012', 'Phạm Quốc Bảo', '0901234503', '78 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'bao.pham@gmail.com'),
('KH013', 'Hoàng Thu Thảo', '0901234504', '200 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'thao.hoang@gmail.com'),
('KH014', 'Vũ Đức Thịnh', '0901234505', '88 Võ Văn Ngân, TP. Thủ Đức, TP. HCM', 'thinh.vu@gmail.com'),
('KH015', 'Đỗ Thanh Hà', '0901234506', '15 Quang Trung, Gò Vấp, TP. HCM', 'ha.do@gmail.com'),
('KH016', 'Bùi Anh Tuấn', '0901234507', '34 Điện Biên Phủ, Bình Thạnh, TP. HCM', 'tuan.bui@gmail.com'),
('KH017', 'Ngô Hải Yến', '0901234508', '51 Hoàng Văn Thụ, Phú Nhuận, TP. HCM', 'yen.ngo@gmail.com'),
('KH018', 'Đinh Trọng Hưng', '0901234509', '99 Nguyễn Văn Cừ, Quận 5, TP. HCM', 'hung.dinh@gmail.com'),
('KH019', 'Lý Gia Hân', '0901234510', '142 Trần Hưng Đạo, Quận 1, TP. HCM', 'han.ly@gmail.com')
ON CONFLICT (makh) DO NOTHING;

-- 2. Thêm 25 đơn hàng online phủ đủ các trạng thái và ngày đặt
INSERT INTO donhangonline (madh, ngaydat, makh, tennguoinhan, sdtnguoinhan, diachigiao, phuongthucthanhtoan, tongtien, trangthai, manv) VALUES
('DH261100010', NOW() - INTERVAL '1 hour', 'KH010', 'Trần Bảo Long', '0901234501', '120 Hai Bà Trưng, Quận 1, TP. HCM', 'cod', 29490000.00, 'cho_xac_nhan', NULL),
('DH261100011', NOW() - INTERVAL '2 hours', 'KH011', 'Lê Ngọc Mai', '0901234502', '45 Lê Duẩn, Quận 1, TP. HCM', 'chuyen_khoan', 24990000.00, 'cho_xac_nhan', NULL),
('DH261100012', NOW() - INTERVAL '3 hours', 'KH012', 'Phạm Quốc Bảo', '0901234503', '78 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'the', 32410000.00, 'cho_xac_nhan', NULL),
('DH261100013', NOW() - INTERVAL '5 hours', 'KH013', 'Hoàng Thu Thảo', '0901234504', '200 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'cod', 19790000.00, 'cho_xac_nhan', NULL),
('DH261100014', NOW() - INTERVAL '8 hours', 'KH014', 'Vũ Đức Thịnh', '0901234505', '88 Võ Văn Ngân, TP. Thủ Đức, TP. HCM', 'chuyen_khoan', 5910000.00, 'cho_xac_nhan', NULL),

('DH261100015', NOW() - INTERVAL '10 hours', 'KH015', 'Đỗ Thanh Hà', '0901234506', '15 Quang Trung, Gò Vấp, TP. HCM', 'cod', 31990000.00, 'da_xac_nhan', 'NV001'),
('DH261100016', NOW() - INTERVAL '12 hours', 'KH016', 'Bùi Anh Tuấn', '0901234507', '34 Điện Biên Phủ, Bình Thạnh, TP. HCM', 'chuyen_khoan', 18490000.00, 'da_xac_nhan', 'NV001'),
('DH261100017', NOW() - INTERVAL '15 hours', 'KH017', 'Ngô Hải Yến', '0901234508', '51 Hoàng Văn Thụ, Phú Nhuận, TP. HCM', 'the', 16990000.00, 'da_xac_nhan', 'NV001'),
('DH261100018', NOW() - INTERVAL '18 hours', 'KH018', 'Đinh Trọng Hưng', '0901234509', '99 Nguyễn Văn Cừ, Quận 5, TP. HCM', 'cod', 17990000.00, 'da_xac_nhan', 'NV001'),
('DH261100019', NOW() - INTERVAL '20 hours', 'KH019', 'Lý Gia Hân', '0901234510', '142 Trần Hưng Đạo, Quận 1, TP. HCM', 'chuyen_khoan', 29490000.00, 'da_xac_nhan', 'NV001'),

('DH261100020', NOW() - INTERVAL '1 day 2 hours', 'KH010', 'Trần Bảo Long', '0901234501', '120 Hai Bà Trưng, Quận 1, TP. HCM', 'cod', 25240000.00, 'dang_giao', 'NV001'),
('DH261100021', NOW() - INTERVAL '1 day 5 hours', 'KH011', 'Lê Ngọc Mai', '0901234502', '45 Lê Duẩn, Quận 1, TP. HCM', 'chuyen_khoan', 31990000.00, 'dang_giao', 'NV001'),
('DH261100022', NOW() - INTERVAL '1 day 8 hours', 'KH012', 'Phạm Quốc Bảo', '0901234503', '78 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'the', 13690000.00, 'dang_giao', 'NV001'),
('DH261100023', NOW() - INTERVAL '1 day 12 hours', 'KH013', 'Hoàng Thu Thảo', '0901234504', '200 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'cod', 5490000.00, 'dang_giao', 'NV001'),
('DH261100024', NOW() - INTERVAL '1 day 18 hours', 'KH014', 'Vũ Đức Thịnh', '0901234505', '88 Võ Văn Ngân, TP. Thủ Đức, TP. HCM', 'chuyen_khoan', 20210000.00, 'dang_giao', 'NV001'),

('DH261100025', NOW() - INTERVAL '2 days', 'KH015', 'Đỗ Thanh Hà', '0901234506', '15 Quang Trung, Gò Vấp, TP. HCM', 'cod', 29490000.00, 'hoan_thanh', 'NV001'),
('DH261100026', NOW() - INTERVAL '2 days 4 hours', 'KH016', 'Bùi Anh Tuấn', '0901234507', '34 Điện Biên Phủ, Bình Thạnh, TP. HCM', 'chuyen_khoan', 17990000.00, 'hoan_thanh', 'NV001'),
('DH261100027', NOW() - INTERVAL '2 days 10 hours', 'KH017', 'Ngô Hải Yến', '0901234508', '51 Hoàng Văn Thụ, Phú Nhuận, TP. HCM', 'the', 34490000.00, 'hoan_thanh', 'NV001'),
('DH261100028', NOW() - INTERVAL '3 days', 'KH018', 'Đinh Trọng Hưng', '0901234509', '99 Nguyễn Văn Cừ, Quận 5, TP. HCM', 'cod', 18490000.00, 'hoan_thanh', 'NV001'),
('DH261100029', NOW() - INTERVAL '3 days 6 hours', 'KH019', 'Lý Gia Hân', '0901234510', '142 Trần Hưng Đạo, Quận 1, TP. HCM', 'chuyen_khoan', 16990000.00, 'hoan_thanh', 'NV001'),
('DH261100030', NOW() - INTERVAL '4 days', 'KH010', 'Trần Bảo Long', '0901234501', '120 Hai Bà Trưng, Quận 1, TP. HCM', 'cod', 670000.00, 'hoan_thanh', 'NV001'),

('DH261100031', NOW() - INTERVAL '1 day', 'KH011', 'Lê Ngọc Mai', '0901234502', '45 Lê Duẩn, Quận 1, TP. HCM', 'cod', 29490000.00, 'da_huy', NULL),
('DH261100032', NOW() - INTERVAL '2 days 1 hour', 'KH012', 'Phạm Quốc Bảo', '0901234503', '78 Nguyễn Thị Minh Khai, Quận 3, TP. HCM', 'chuyen_khoan', 31990000.00, 'da_huy', NULL),
('DH261100033', NOW() - INTERVAL '3 days 2 hours', 'KH013', 'Hoàng Thu Thảo', '0901234504', '200 Cách Mạng Tháng 8, Quận 10, TP. HCM', 'the', 17990000.00, 'da_huy', NULL),
('DH261100034', NOW() - INTERVAL '5 days', 'KH014', 'Vũ Đức Thịnh', '0901234505', '88 Võ Văn Ngân, TP. Thủ Đức, TP. HCM', 'cod', 5490000.00, 'da_huy', NULL)
ON CONFLICT (madh) DO NOTHING;

-- 3. Chi tiết các sản phẩm trong đơn hàng
INSERT INTO chitietdonhangonline (madh, mahang, soluong, dongia) VALUES
('DH261100010', 'IP15PM256', 1, 29490000.00),
('DH261100011', 'IP15P128', 1, 24990000.00),
('DH261100012', 'S24U512', 1, 31990000.00),
('DH261100012', 'SAC_ANKER', 1, 420000.00),
('DH261100013', 'IP15128', 1, 19790000.00),
('DH261100014', 'AP_PRO2', 1, 5490000.00),
('DH261100014', 'SAC_ANKER', 1, 420000.00),
('DH261100015', 'S24U512', 1, 31990000.00),
('DH261100016', 'XM14256', 1, 18490000.00),
('DH261100017', 'IPAD_A6', 1, 16990000.00),
('DH261100018', 'S24256', 1, 17990000.00),
('DH261100019', 'IP15PM256', 1, 29490000.00),
('DH261100020', 'IP15P128', 1, 24990000.00),
('DH261100020', 'CAP_C_L', 1, 250000.00),
('DH261100021', 'S24U512', 1, 31990000.00),
('DH261100022', 'MH275', 1, 13690000.00),
('DH261100023', 'AP_PRO2', 1, 5490000.00),
('DH261100024', 'IP15128', 1, 19790000.00),
('DH261100024', 'SAC_ANKER', 1, 420000.00),
('DH261100025', 'IP15PM256', 1, 29490000.00),
('DH261100026', 'S24256', 1, 17990000.00),
('DH261100027', 'MH797', 1, 34490000.00),
('DH261100028', 'XM14256', 1, 18490000.00),
('DH261100029', 'IPAD_A6', 1, 16990000.00),
('DH261100030', 'SAC_ANKER', 1, 420000.00),
('DH261100030', 'CAP_C_L', 1, 250000.00),
('DH261100031', 'IP15PM256', 1, 29490000.00),
('DH261100032', 'S24U512', 1, 31990000.00),
('DH261100033', 'S24256', 1, 17990000.00),
('DH261100034', 'AP_PRO2', 1, 5490000.00)
ON CONFLICT (madh, mahang) DO NOTHING;

-- 4. Vận đơn GHN cho các đơn đang giao / đã xác nhận / hoàn thành
INSERT INTO donhang_vandon (madh, carrier, tracking_code, shipping_status, expected_delivery, is_demo, events_json) VALUES
('DH261100015', 'GHN', 'GHNMOCK7A91X0', 'Chờ lấy hàng', NOW() + INTERVAL '2 days', true, 
 '[{"Time":"2026-11-13T10:00:00Z","Status":"ready_to_pick","Description":"Đã tạo vận đơn trên GHN","Location":"Kho SamaMobile - 241 Lê Văn Việt"}]'),
('DH261100016', 'GHN', 'GHNMOCK8B12Y1', 'Chờ lấy hàng', NOW() + INTERVAL '2 days', true, 
 '[{"Time":"2026-11-13T11:00:00Z","Status":"ready_to_pick","Description":"Đã tạo vận đơn trên GHN","Location":"Kho SamaMobile - 241 Lê Văn Việt"}]'),
('DH261100020', 'GHN', 'GHNMOCK1C23Z2', 'Đang giao hàng', NOW() + INTERVAL '1 day', true, 
 '[{"Time":"2026-11-12T14:00:00Z","Status":"ready_to_pick","Description":"Đã tạo vận đơn","Location":"Kho SamaMobile"},{"Time":"2026-11-12T18:00:00Z","Status":"picking","Description":"Tài xế GHN đã lấy hàng","Location":"Bưu cục Thủ Đức"},{"Time":"2026-11-13T08:00:00Z","Status":"delivering","Description":"Shipper đang giao hàng đến bạn","Location":"Quận 1 - Bưu tá: Nguyễn Văn A (098111222)"}]'),
('DH261100021', 'GHN', 'GHNMOCK2D34W3', 'Đang giao hàng', NOW() + INTERVAL '1 day', true, 
 '[{"Time":"2026-11-12T15:00:00Z","Status":"ready_to_pick","Description":"Đã tạo vận đơn","Location":"Kho SamaMobile"},{"Time":"2026-11-13T09:00:00Z","Status":"delivering","Description":"Shipper đang giao hàng","Location":"Quận 1"}]'),
('DH261100025', 'GHN', 'GHNMOCK3E45V4', 'Đã giao thành công', NOW() - INTERVAL '1 day', true, 
 '[{"Time":"2026-11-11T10:00:00Z","Status":"ready_to_pick","Description":"Đã tạo vận đơn","Location":"Kho SamaMobile"},{"Time":"2026-11-11T16:00:00Z","Status":"delivering","Description":"Đang giao","Location":"Gò Vấp"},{"Time":"2026-11-12T11:30:00Z","Status":"delivered","Description":"Khách đã nhận hàng và ký tên","Location":"Gò Vấp"}]')
ON CONFLICT (madh) DO NOTHING;
