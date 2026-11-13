-- =============================================================================
-- Migration 04: Hệ thống tự động duyệt đơn hàng (plan (8).md)
-- =============================================================================

-- 1. Bổ sung cột cho donhangonline
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS payment_status VARCHAR(20) DEFAULT 'unpaid';
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS risk_flag BOOLEAN DEFAULT FALSE;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS risk_reasons TEXT DEFAULT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS auto_approve_at TIMESTAMPTZ DEFAULT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS ghn_order_code VARCHAR(50) DEFAULT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS ghn_error TEXT DEFAULT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS client_ip VARCHAR(50) DEFAULT NULL;
ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS address_hash VARCHAR(64) DEFAULT NULL;

-- 2. Cập nhật check constraint trangthai cho donhangonline
ALTER TABLE donhangonline DROP CONSTRAINT IF EXISTS donhangonline_trangthai_check;
ALTER TABLE donhangonline ADD CONSTRAINT donhangonline_trangthai_check CHECK (
    trangthai IN (
        'cho_thanh_toan', 'cho_tu_duyet', 'cho_xac_nhan', 'can_kiem_tra',
        'da_xac_nhan', 'cho_lay_hang', 'loi_van_don', 'dang_giao',
        'hoan_thanh', 'boom', 'hoan_hang', 'da_huy'
    )
);

-- 3. Bổ sung cột cho khachhang
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS completed_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS qualified_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS max_completed_value NUMERIC(18,2) DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS failed_orders INT DEFAULT 0;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS is_blacklisted BOOLEAN DEFAULT FALSE;
ALTER TABLE khachhang ADD COLUMN IF NOT EXISTS updated_at TIMESTAMPTZ DEFAULT NOW();

-- 4. Bảng customer_addresses
CREATE TABLE IF NOT EXISTS customer_addresses (
    id SERIAL PRIMARY KEY,
    phone VARCHAR(15) NOT NULL,
    address_hash VARCHAR(64) NOT NULL,
    full_address VARCHAR(255) NOT NULL,
    last_delivered_at TIMESTAMPTZ DEFAULT NOW(),
    CONSTRAINT uq_customer_addresses UNIQUE (phone, address_hash)
);

-- 5. Bảng order_status_logs
CREATE TABLE IF NOT EXISTS order_status_logs (
    id SERIAL PRIMARY KEY,
    order_id VARCHAR(15) NOT NULL REFERENCES donhangonline(madh) ON DELETE CASCADE,
    from_status VARCHAR(30),
    to_status VARCHAR(30) NOT NULL,
    actor VARCHAR(30) NOT NULL, -- system | staff | ghn | customer
    reason TEXT,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

-- 6. Bảng webhook_events
CREATE TABLE IF NOT EXISTS webhook_events (
    id SERIAL PRIMARY KEY,
    source VARCHAR(30) NOT NULL,
    event_id VARCHAR(100) NOT NULL,
    payload TEXT,
    received_at TIMESTAMPTZ DEFAULT NOW(),
    CONSTRAINT uq_webhook_events UNIQUE (source, event_id)
);

-- 7. Bảng order_settings
CREATE TABLE IF NOT EXISTS order_settings (
    key VARCHAR(50) PRIMARY KEY,
    value TEXT NOT NULL,
    description TEXT
);

-- Seed order_settings mặc định theo plan (8).md
INSERT INTO order_settings (key, value, description) VALUES
    ('auto_approve_enabled', 'true', 'Công tắc khẩn cấp tự duyệt đơn'),
    ('auto_approve_delay_minutes', '15', 'Thời gian chờ trước khi tự duyệt COD (phút)'),
    ('cod_hold_threshold', '5000000', 'COD từ mức này phải nhân viên gọi (VNĐ)'),
    ('cod_max_value', '30000000', 'COD vượt mức này không được phép, bắt cọc (VNĐ)'),
    ('cod_small_auto_below', '0', 'COD dưới mức này tự duyệt bỏ qua tin cậy (0 = tắt)'),
    ('trusted_min_completed', '2', 'Số đơn giá trị cao tối thiểu để thành khách tin cậy'),
    ('trust_min_order_value', '1000000', 'Đơn từ mức này mới tăng điểm tin cậy (VNĐ)'),
    ('trust_value_multiplier', '2', 'Hạn mức tự duyệt = bội số đơn lớn nhất đã hoàn thành'),
    ('trusted_lookback_months', '6', 'Khoảng thời gian xét boom (tháng)'),
    ('blacklist_after_failed', '2', 'Số lần boom để tự động blacklist'),
    ('risk_max_orders_per_window', '5', 'Số đơn tối đa trong cửa sổ rủi ro'),
    ('risk_window_minutes', '10', 'Độ dài cửa sổ rủi ro (phút)')
ON CONFLICT (key) DO NOTHING;

-- Index tối ưu hóa truy vấn
CREATE INDEX IF NOT EXISTS idx_dhol_phone_created ON donhangonline (sdtnguoinhan, ngaydat);
CREATE INDEX IF NOT EXISTS idx_dhol_status_auto ON donhangonline (trangthai, auto_approve_at);
CREATE INDEX IF NOT EXISTS idx_dhol_ghn_code ON donhangonline (ghn_order_code);
