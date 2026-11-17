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

## 2. Task 2: Quản lý Giỏ hàng

- **Nguồn sự thật giá**: Client chỉ gửi `variantId` và `qty`. Giá bán và tổng tiền luôn được truy vấn và tính toán trực tiếp từ bảng `variants` trong database.
- **Khách vãng lai và gộp giỏ (Guest Merge)**: Giỏ hàng khách vãng lai được định danh qua header `X-Cart-Token`. Khi khách đăng nhập, gọi API merge giỏ: các sản phẩm trùng variant được cộng dồn số lượng nhưng bị chặn trần bởi `MAX_QTY_PER_ORDER`. Sau khi gộp, giỏ hàng vãng lai cũ được giải phóng sạch sẽ.
- **Hệ thống cảnh báo (Cart Warnings)**: Nếu tồn kho biến động (`stock == 0` hoặc `stock < qty`), hệ thống gắn cảnh báo trực tiếp trong DTO trả về (`OUT_OF_STOCK`, `INSUFFICIENT_STOCK`) để client hiển thị cho người dùng.

## 3. Task 3: Tạo đơn và Giữ hàng (Chống bán lố)

- **Atomic Stock Deduction**: Sử dụng câu lệnh conditional update:
  `UPDATE variants SET stock = stock - :qty WHERE id = :id AND stock >= :qty`
  Nếu số dòng ảnh hưởng bằng 0 (affected == 0), transaction lập tức ROLLBACK toàn bộ và báo hết hàng.
- **Idempotency**: Kiểm tra `Idempotency-Key` ngay đầu transaction. Nếu đơn hàng đã tồn tại với key này, trả lại đơn cũ nguyên trạng, không trừ kho lần hai.
- **Snapshot dữ liệu**: Lưu trữ bất biến tên sản phẩm, mã SKU, giá bán tại thời điểm đặt đơn trong `order_items` và địa chỉ giao hàng trong `orders`.

## 4. Task 4: State Machine Đơn hàng

- **Module độc quyền đổi trạng thái**: Toàn bộ thay đổi trạng thái phải đi qua `OrderStateMachine`. Cấm tuyệt đối cập nhật cột `status` thủ công từ các nơi khác.
- **Sơ đồ chuyển đổi hợp lệ**:
  - `PENDING_PAYMENT` → `PAID`, `EXPIRED`, `CANCELLED`, `CONFIRMED` (đối với đơn COD)
  - `EXPIRED` → `PAID`, `PAID_OUT_OF_STOCK`
  - `PAID` → `CONFIRMED`, `REFUNDED`
  - `CONFIRMED` → `SHIPPING`
  - `SHIPPING` → `COMPLETED`
  - `PAID_OUT_OF_STOCK` → `REFUNDED`
- **Chống tranh chấp (Concurrency)**: Thực hiện update có điều kiện `WHERE id = :id AND status = :expectedCurrentStatus`. Nếu 2 tiến trình cùng chạy, chỉ duy nhất 1 tiến trình có affected = 1 và thắng, tiến trình còn lại nhận kết quả thất bại.

## 5. Task 5: Thanh toán QR và Webhook

- **Bảo mật**: Xác thực chữ ký HMAC-SHA256 trên raw payload webhook. Tuyệt đối không log secret key hay chữ ký ra file log. Không lưu trữ số thẻ khách hàng.
- **Xử lý lệch tiền**: So sánh `amount == order.Total`. Nếu số tiền lệch (thiếu hoặc thừa), ghi nhận bản ghi `Payment` với trạng thái `AMOUNT_MISMATCH` và đánh dấu ghi log để nhân viên đối soát xử lý thủ công, tuyệt đối không tự động xác nhận `PAID`.
- **Luồng COD**: Đơn COD có thể xác nhận trực tiếp sang `CONFIRMED`. Đối với đơn có giá trị cao (từ 20.000.000 VND trở lên), bắt buộc phải có mã OTP xác thực hợp lệ.

## 6. Task 6: Xử lý Hết hạn, Ân hạn & Thanh toán muộn (9:59 Case)

- **Grace Period (Ân hạn 5 phút)**: Job hết hạn chỉ quét các đơn có `ExpiresAt < now - GRACE_PERIOD`. Nếu khách thanh toán lúc 9:59 (thời hạn 10 phút), đơn vẫn còn trong thời gian giữ hoặc trong grace period, job không hủy đơn và webhook xác nhận `PAID` an toàn.
- **Chủ động Query Cổng thanh toán**: Trước khi hủy đơn, job hết hạn gọi `QueryTransactionAsync`. Nếu cổng xác nhận khách đã trả tiền thành công, job chuyển đơn sang `PAID` thay vì hủy.
- **Late Webhook khi đơn đã EXPIRED**:
  - Nếu kho còn đủ hàng: Trừ lại tồn kho bằng conditional update và phục hồi đơn từ `EXPIRED` sang `PAID`.
  - Nếu kho đã bị người khác mua hết: Chuyển đơn sang `PAID_OUT_OF_STOCK`, tự động tạo bản ghi `RefundRequest` để hoàn lại tiền cho khách hàng, không để thất thoát tiền khách.

## 7. Task 7: Hủy đơn & Hoàn tiền An toàn

- **Nhả kho an toàn**: Khi hủy đơn `PENDING_PAYMENT`, kho chỉ được cộng lại khi câu lệnh đổi trạng thái `PENDING_PAYMENT → CANCELLED` có affected = 1. Gọi hủy 2 lần sẽ bị chặn và kho không bị cộng lặp.
- **Retry Hoàn tiền Idempotent**: Khi hoàn tiền qua cổng `provider.RefundAsync`, sử dụng `ProviderRefundId` làm khóa Idempotency. Nếu cổng gặp lỗi mạng (timeout), hệ thống đánh dấu `FAILED` và cho phép retry; các lần retry tiếp theo dùng cùng mã định danh, không gây hoàn tiền kép.

## 8. Task 8: Khuyến mãi, Vận chuyển & Đặc thù Điện thoại

- **Voucher Concurrency**: Lượt sử dụng voucher được trừ bằng câu lệnh nguyên tử `UPDATE vouchers SET used_count = used_count + 1 WHERE code = :code AND used_count < max_uses`. Khi vượt quá số lượt, đơn bị rollback. Khi đơn bị hủy hoặc hết hạn, lượt dùng voucher được hoàn trả tự động.
- **Phí vận chuyển**: Tự động tính toán ở server: Miễn phí vận chuyển cho đơn hàng từ 5.000.000 VND; đơn hàng dưới 5.000.000 VND tính phí 30.000 VND.
- **Quà tặng kèm (Gift Items)**: Lưu thành dòng `order_items` với đơn giá `price_snapshot = 0`, nhưng vẫn quản lý trừ kho bình thường.
- **Quản lý IMEI**: Số IMEI gắn với từng máy vật lý được gán khi xuất kho (Warehouse Dispatch), không gán tại thời điểm đặt hàng trực tuyến để tránh khóa IMEI quá sớm.

## 9. Task 9: Chống Lạm dụng

- **Rate Limiting**: Áp dụng phân vùng Rate Limiting theo địa chỉ IP (60 requests/phút) trên toàn bộ endpoints của `OrdersController` và `CartController`.
- **Cảnh báo Lạm dụng (Abuse Alert)**: Nếu cùng một số điện thoại hoặc địa chỉ nhận hàng tạo từ 3 đơn trở lên trong vòng 1 giờ, hệ thống ghi log cảnh báo mức WARNING `[ABUSE_ALERT]`.

## 10. Task 10: Đối soát & Chịu tải Cao

- **Job Đối soát Tài chính Hằng ngày**: So sánh đối chiếu tự động giữa `payments` và `orders`, phát hiện và cảnh báo các trường hợp bất thường:
  - Có tiền vào (`SUCCESS`) nhưng đơn hàng bị `CANCELLED` hoặc `EXPIRED` mà chưa có yêu cầu hoàn tiền hoàn tất.
  - Đơn hàng đánh dấu `PAID` nhưng thiếu bản ghi giao dịch thành công.
  - Số tiền thanh toán thực tế lệch với tổng giá trị đơn hàng.
- **Chịu tải Flash Sale**: Đã kiểm chứng qua test tải mô phỏng 1.000 người mua tranh 10 sản phẩm: đúng 10 đơn thành công, 990 đơn bị từ chối hết hàng, tồn kho kết thúc chính xác ở 0 và không bao giờ âm.
