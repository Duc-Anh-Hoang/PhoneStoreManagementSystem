# DECISIONS.md - Ghi nhận Giả định & Quyết định Kiến trúc

Tài liệu này ghi lại các giả định và quyết định kỹ thuật theo quy tắc trong plan checkout & giỏ hàng.

## 1. Task 1: Schema & Thiết kế Dữ liệu

- **Kiểu dữ liệu tiền tệ**: Sử dụng kiểu số nguyên `long` trong C# và `BIGINT` trong PostgreSQL cho mọi trường tiền tệ (`price`, `total`, `amount`), đại diện chính xác cho đơn vị VNĐ, tuyệt đối không dùng float/double để tránh sai số số học.
- **Ràng buộc toàn vẹn cơ sở dữ liệu**:
  - `variants.stock >= 0` qua CHECK constraint trong DB và kiểm tra số dòng ảnh hưởng khi update.
  - `orders.idempotency_key` có UNIQUE index chống trùng lặp đơn hàng từ client retry.
  - `payments.provider_txn_id` có UNIQUE index chống ghi nhận nhiều lần một giao dịch thanh toán từ webhook retry.
  - `cart_items(cart_id, variant_id)` có UNIQUE index để gom số lượng, tránh nhân đôi bản ghi.
- **Quan hệ bảng**:
  - `products` 1-N `variants`.
  - `carts` 1-N `cart_items` (kết nối với `variants`).
  - `orders` 1-N `order_items` (lưu snapshot `product_name_snapshot`, `sku_snapshot`, `price_snapshot`).
  - `orders` 1-N `payments`, `order_status_history`, `refund_requests`.
- **Cấu hình tham số mặc định**:
  - `RESERVATION_TTL`: 10 phút.
  - `GRACE_PERIOD`: 5 phút.
  - `MAX_QTY_PER_ORDER`: 2 chiếc cho mỗi item trong đơn hàng.
  - `EXPIRE_JOB_INTERVAL`: 1 phút.
- **Quản lý trạng thái**:
  - State Machine độc quyền điều khiển trạng thái đơn: `PENDING_PAYMENT`, `PAID`, `EXPIRED`, `PAID_OUT_OF_STOCK`, `CONFIRMED`, `SHIPPING`, `COMPLETED`, `REFUNDED`, `CANCELLED`.
  - Mọi thao tác cập nhật trạng thái đều là Conditional Update (`WHERE status = @oldStatus`) và kiểm tra affected rows = 1.
