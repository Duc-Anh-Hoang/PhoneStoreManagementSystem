ALTER TABLE donhangonline ADD COLUMN IF NOT EXISTS ghichu text,
    ADD COLUMN IF NOT EXISTS shipping_fee numeric(18,2) NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS voucher_code varchar(50),
    ADD COLUMN IF NOT EXISTS points_used integer NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS member_phone varchar(15);
DO $migration$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema=current_schema()
               AND table_name='donhangonline' AND column_name='risk_flag' AND data_type <> 'boolean') THEN
        ALTER TABLE donhangonline ALTER COLUMN risk_flag DROP DEFAULT;
        ALTER TABLE donhangonline ALTER COLUMN risk_flag TYPE boolean
            USING lower(risk_flag::text) NOT IN ('none','false','0','');
        ALTER TABLE donhangonline ALTER COLUMN risk_flag SET DEFAULT false;
    END IF;
END;
$migration$;
ALTER TABLE donhangonline DROP CONSTRAINT IF EXISTS donhangonline_trangthai_check;
ALTER TABLE donhangonline ADD CONSTRAINT donhangonline_trangthai_check CHECK
    (trangthai IN ('cho_thanh_toan','cho_tu_duyet','cho_xac_nhan','can_kiem_tra','da_xac_nhan',
                   'cho_lay_hang','loi_van_don','dang_giao','da_giao','hoan_thanh','boom','hoan_hang','da_huy'));
CREATE TABLE IF NOT EXISTS vouchers (
    id bigserial PRIMARY KEY, code varchar(50) NOT NULL UNIQUE,
    discount_amount bigint NOT NULL CHECK (discount_amount >= 0),
    min_order_total bigint NOT NULL DEFAULT 0, max_uses integer NOT NULL,
    used_count integer NOT NULL DEFAULT 0, expires_at timestamptz NOT NULL,
    created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz NOT NULL DEFAULT now()
);
ALTER TABLE vouchers ADD COLUMN IF NOT EXISTS discount_percent integer NOT NULL DEFAULT 0;
INSERT INTO vouchers(code,discount_amount,discount_percent,min_order_total,max_uses,expires_at)
VALUES ('HELLOSAMA',0,5,0,2147483647,'9999-12-31'),('SAMAMEMBER',100000,0,500000,2147483647,'9999-12-31'),
       ('GIAM100K',100000,0,1000000,2147483647,'9999-12-31'),('FREE50K',50000,0,300000,2147483647,'9999-12-31')
ON CONFLICT(code) DO NOTHING;
CREATE TABLE IF NOT EXISTS trade_in_bookings (
    code varchar(40) PRIMARY KEY, name varchar(100) NOT NULL, phone varchar(15) NOT NULL,
    branch varchar(200) NOT NULL, target_product jsonb NOT NULL, used_device jsonb NOT NULL,
    status text NOT NULL DEFAULT 'PENDING_CONFIRMATION', created_at timestamptz NOT NULL DEFAULT now()
);
CREATE OR REPLACE FUNCTION fn_trg_chitiethoadon_tongtien() RETURNS trigger AS $$
DECLARE
    invoice_id varchar(15) := COALESCE(NEW.sohd, OLD.sohd);
    merchandise numeric;
    fee numeric;
    discount numeric;
BEGIN
    SELECT COALESCE(SUM(slban * dongia_ban),0) INTO merchandise FROM chitiethoadon WHERE sohd=invoice_id;
    SELECT d.shipping_fee, GREATEST(0, sum(i.soluong*i.dongia) + d.shipping_fee - d.tongtien)
        INTO fee, discount FROM donhangonline d JOIN chitietdonhangonline i USING(madh)
        WHERE d.sohd=invoice_id GROUP BY d.madh;
    IF NOT FOUND THEN
        fee := 0;
        SELECT round(merchandise * COALESCE(k.phantramgiam,0)/100,2) INTO discount
            FROM hoadon h LEFT JOIN khuyenmai k USING(makm) WHERE h.sohd=invoice_id;
    END IF;
    UPDATE hoadon SET tongtien=merchandise+fee, tiengiamgia=least(merchandise+fee,discount) WHERE sohd=invoice_id;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Each store item has a stable checkout product and default variant. Stock is read from the store ledger.
ALTER TABLE products ADD COLUMN IF NOT EXISTS store_item_code varchar(10) UNIQUE;
CREATE OR REPLACE FUNCTION sync_store_catalog() RETURNS trigger AS $$
DECLARE checkout_id bigint;
BEGIN
    INSERT INTO products(name,store_item_code) VALUES(NEW.tenhang,NEW.mahang)
    ON CONFLICT(store_item_code) DO UPDATE SET name=excluded.name, updated_at=now()
    RETURNING id INTO checkout_id;
    INSERT INTO variants(product_id,sku,color,storage,price,stock)
    VALUES(checkout_id,'STORE:'||NEW.mahang,'','',round(NEW.dongia),0)
    ON CONFLICT(sku) DO UPDATE SET price=excluded.price, updated_at=now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
DROP TRIGGER IF EXISTS sync_checkout_catalog ON mathang;
CREATE TRIGGER sync_checkout_catalog AFTER INSERT OR UPDATE OF tenhang,dongia ON mathang
FOR EACH ROW EXECUTE FUNCTION sync_store_catalog();
INSERT INTO products(name,store_item_code) SELECT tenhang,mahang FROM mathang
ON CONFLICT(store_item_code) DO UPDATE SET name=excluded.name WHERE products.name<>excluded.name;
INSERT INTO variants(product_id,sku,color,storage,price,stock)
SELECT p.id,'STORE:'||m.mahang,'','',round(m.dongia),0 FROM mathang m JOIN products p ON p.store_item_code=m.mahang
ON CONFLICT(sku) DO UPDATE SET price=excluded.price WHERE variants.price<>excluded.price;

CREATE OR REPLACE FUNCTION restore_online_voucher() RETURNS trigger AS $$
BEGIN
    IF NEW.trangthai='da_huy' AND OLD.trangthai<>'da_huy' AND NEW.voucher_code IS NOT NULL THEN
        UPDATE vouchers SET used_count=greatest(0,used_count-1),updated_at=now() WHERE code=NEW.voucher_code;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
DROP TRIGGER IF EXISTS restore_online_voucher ON donhangonline;
CREATE TRIGGER restore_online_voucher AFTER UPDATE OF trangthai ON donhangonline
FOR EACH ROW EXECUTE FUNCTION restore_online_voucher();

ALTER TABLE phanquyen ADD COLUMN IF NOT EXISTS quyen_co_so varchar(10)
    CHECK (quyen_co_so IN ('manager','sales','cashier','stock','tech'));
ALTER TABLE nhanvien DROP CONSTRAINT IF EXISTS ck_nhanvien_chucvu;

CREATE OR REPLACE FUNCTION validate_staff_position() RETURNS trigger AS $$
BEGIN
    IF NEW.chucvu NOT IN ('Quản lý','Bán hàng','Thu ngân','Kho','Kỹ thuật & Bảo hành')
       AND NOT EXISTS (SELECT 1 FROM phanquyen WHERE quyen_co_so IS NOT NULL AND tenquyen=NEW.chucvu) THEN
        RAISE EXCEPTION 'Chức vụ không có trong danh sách' USING ERRCODE='23514';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
DROP TRIGGER IF EXISTS validate_staff_position ON nhanvien;
CREATE TRIGGER validate_staff_position BEFORE INSERT OR UPDATE OF chucvu ON nhanvien
FOR EACH ROW EXECUTE FUNCTION validate_staff_position();
