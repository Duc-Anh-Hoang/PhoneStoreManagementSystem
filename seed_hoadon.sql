-- Tạo hóa đơn và gán vào donhangonline cho 6 đơn đã hoàn thành
INSERT INTO hoadon (sohd, ngaylap, makh, manv, kenhban, tongtien, tiengiamgia, phuongthucthanhtoan) VALUES
('HD261100025', '2026-11-12 11:30:00+00', 'KH015', 'NV001', 'online', 29490000.00, 0, 'tien_mat'),
('HD261100026', '2026-11-12 14:00:00+00', 'KH016', 'NV001', 'online', 17990000.00, 0, 'chuyen_khoan'),
('HD261100027', '2026-11-12 16:30:00+00', 'KH017', 'NV001', 'online', 34490000.00, 0, 'the'),
('HD261100028', '2026-11-11 15:00:00+00', 'KH018', 'NV001', 'online', 18490000.00, 0, 'tien_mat'),
('HD261100029', '2026-11-11 17:00:00+00', 'KH019', 'NV001', 'online', 16990000.00, 0, 'chuyen_khoan'),
('HD261100030', '2026-11-10 18:00:00+00', 'KH010', 'NV001', 'online', 670000.00, 0, 'tien_mat')
ON CONFLICT (sohd) DO NOTHING;

-- Chi tiết hóa đơn
INSERT INTO chitiethoadon (sohd, mahang, slban, dongia_ban) VALUES
('HD261100025', 'IP15PM256', 1, 29490000.00),
('HD261100026', 'S24256', 1, 17990000.00),
('HD261100027', 'MH797', 1, 34490000.00),
('HD261100028', 'XM14256', 1, 18490000.00),
('HD261100029', 'IPAD_A6', 1, 16990000.00),
('HD261100030', 'SAC_ANKER', 1, 420000.00),
('HD261100030', 'CAP_C_L', 1, 250000.00)
ON CONFLICT DO NOTHING;

-- Cập nhật sohd vào donhangonline
UPDATE donhangonline SET sohd = 'HD261100025' WHERE madh = 'DH261100025';
UPDATE donhangonline SET sohd = 'HD261100026' WHERE madh = 'DH261100026';
UPDATE donhangonline SET sohd = 'HD261100027' WHERE madh = 'DH261100027';
UPDATE donhangonline SET sohd = 'HD261100028' WHERE madh = 'DH261100028';
UPDATE donhangonline SET sohd = 'HD261100029' WHERE madh = 'DH261100029';
UPDATE donhangonline SET sohd = 'HD261100030' WHERE madh = 'DH261100030';
