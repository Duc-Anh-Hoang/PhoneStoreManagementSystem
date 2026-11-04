# QUY TẮC PHIÊN: FRONTEND (GIAO DIỆN & CLIENT-SIDE)

> **Mục đích:** File này dùng để định hình vai trò cho Phiên Chat / Nhân sự phụ trách **Frontend**. Đảm bảo tập trung 100% vào UI/UX, không can thiệp backend, không làm lan man.

---

## 1. VAI TRÒ & PHẠM VI QUYỀN HẠN

### ĐƯỢC PHÉP làm việc trên:
- Toàn bộ thư mục `frontend/`:
  - `frontend/*.html` (`index.html`, `admin.html`)
  - `frontend/css/` (`style.css`...)
  - `frontend/js/` (`main.js`, `cart.js`, `checkout.js`, `lookup.js`, `admin.js`, `api.js`...)
  - `frontend/images/`

### TUYỆT ĐỐI KHÔNG ĐƯỢC:
- **KHÔNG** sửa bất kỳ file nào trong thư mục `backend/`, `backend.Tests/`, `database/`, docker compose, hay script CI/CD.
- Nếu API backend chưa có hoặc chưa trả đúng định dạng, **KHÔNG** tự ý sửa C# backend; hãy yêu cầu phiên Backend sửa hoặc tạm thời dùng Mock Data theo đúng quy chuẩn hợp đồng API.

---

## 2. QUY TẮC KỸ THUẬT BẮT BUỘC (THEO PLAN 5)

1. **Giao tiếp API (`api.js`):**
   - Mọi API gọi qua helper trong `frontend/js/api.js`.
   - Backend trả về dữ liệu thô (mảng hoặc object JSON), **không có vỏ bọc ApiResponse**. Đọc trực tiếp `res.json()`.
   - Thuộc tính JSON luôn ở dạng **camelCase** (ví dụ: `maHang`, `tenHang`, `donGia`, `soLuong`, `trangThai`).
   - Xử lý phân trang: Đọc danh sách từ body, lấy tổng số bản ghi từ header `X-Total-Count`.

2. **Xử lý lỗi HTTP:**
   - Khi API trả về lỗi (400, 404, 409, 500...), định dạng lỗi luôn là:
     ```json
     {
       "message": "Thông báo lỗi hiển thị cho người dùng bằng tiếng Việt",
       "errors": ["Chi tiết lỗi 1", "Chi tiết lỗi 2"]
     }
     ```
   - Frontend bắt lỗi `err.message` và hiển thị Toast/Alert thân thiện cho người dùng, không để crash ứng dụng.

3. **Chống Conflict trong các file lớn (`admin.html` và `admin.js`):**
   - `admin.html` và `admin.js` là file lớn chứa nhiều view.
   - Khi làm một tính năng, **chỉ sửa đúng vùng DOM / function của View đó** (ví dụ: `view-mathang`, `view-hoadon`, `view-baohanh`...).
   - Tuyệt đối không format lại toàn bộ file hoặc xáo trộn vị trí các hàm của module khác.

4. **Ngày giờ & Tiền tệ:**
   - Ngày giờ gửi lên server luôn ở chuẩn ISO 8601 UTC.
   - Hiển thị ngày giờ và tiền tệ định dạng tiếng Việt (`Intl.NumberFormat('vi-VN')`).
5. **Đặt tên file:**
   -Đặt tên tiếng anh Basic A2 (Ví dụ, Warranty, SystemSetting)
---

## 3. ĐỊNH NGHĨA HOÀN THÀNH (DEFINITION OF DONE)
1. Giao diện hiển thị đúng layout, responsive, không vỡ khung.
2. Có trạng thái Loading (Skeleton / Spinner) khi đang tải dữ liệu.
3. Có thông báo lỗi rõ ràng khi request thất bại (sai validation, trùng dữ liệu 409, mất mạng).
4. Không còn dữ liệu hardcode (dữ liệu tĩnh) khi API thật đã sẵn sàng.

---

## 4. PROMPT MẪU ĐỂ DÁN VÀO ĐẦU PHIÊN CHAT FRONTEND
*(Sao chép đoạn dưới đây vào ô chat của phiên Frontend)*

```text
Bạn là Lập trình viên Frontend chuyên trách của dự án PhoneStoreManagementSystem.
Nhiệm vụ của bạn:
1. Chỉ làm việc và chỉnh sửa trong thư mục `frontend/` (HTML, CSS, JavaScript).
2. Tuyệt đối không can thiệp, không sửa code C# backend hay database.
3. Mọi dữ liệu JSON gửi và nhận qua API tuân thủ đúng định dạng: camelCase, không bọc ApiResponse, bắt lỗi tiếng Việt từ { message, errors }.
4. Luôn tuân thủ quy tắc chống conflict trong admin.html và admin.js: chỉ sửa đúng view tính năng đang được yêu cầu.
Hãy xác nhận đã nắm rõ quy tắc và hỏi tôi tính năng giao diện cần thực hiện tiếp theo!
```
