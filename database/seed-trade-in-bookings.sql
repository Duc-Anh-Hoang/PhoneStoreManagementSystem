-- Phiếu mẫu để kiểm tra màn Phiếu thu cũ trên môi trường thử nghiệm.
-- Chạy sau khi backend đã tạo bảng trade_in_bookings.
BEGIN;

INSERT INTO trade_in_bookings (code, name, phone, branch, target_product, used_device)
VALUES
    ('TC-00000000000040008000000000000001', 'Khách mẫu 01', '0900000001',
     '241 Lê Văn Việt, TP. Thủ Đức, TP.HCM',
     jsonb_build_object('name', 'iPhone 15 128GB'),
     jsonb_build_object('brand', 'Apple', 'model', 'iPhone 12', 'capacity', '64GB')),
    ('TC-00000000000040008000000000000002', 'Khách mẫu 02', '0900000002',
     '241 Lê Văn Việt, TP. Thủ Đức, TP.HCM',
     jsonb_build_object('name', 'Samsung Galaxy A27 5G 128GB'),
     jsonb_build_object('brand', 'Samsung', 'model', 'Samsung Galaxy A52', 'capacity', '128GB')),
    ('TC-00000000000040008000000000000003', 'Khách mẫu 03', '0900000003',
     '241 Lê Văn Việt, TP. Thủ Đức, TP.HCM',
     jsonb_build_object('name', 'Xiaomi Redmi Note 17 128GB'),
     jsonb_build_object('brand', 'Xiaomi', 'model', 'Xiaomi Redmi Note 11', 'capacity', '128GB'))
ON CONFLICT (code) DO NOTHING;

COMMIT;
