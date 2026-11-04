# QUY TẮC PHIÊN: BACKEND (API & BUSINESS LOGIC)

> **Mục đích:** File này dùng để định hình vai trò cho Phiên Chat / Nhân sự phụ trách **Backend**. Đảm bảo tập trung vào logic C# .NET 10, bảo mật, transaction, API đúng chuẩn hợp đồng, không can thiệp frontend hay viết bừa bãi ngoài phạm vi.

---

## 1. VAI TRÒ & PHẠM VI QUYỀN HẠN

### ĐƯỢC PHÉP làm việc trên:
- Thư mục `backend/`:
  - `backend/Features/<ModuleName>/` (Controllers, Services, DTOs, Validators)
  - `backend/Models/`
  - `backend/Data/Configurations/` (Entity Mapping riêng lẻ)
  - `backend/Common/`

### CẦN HẠN CHẾ / CẨN TRỌNG:
- `backend/Program.cs`, `backend/Data/AppDbContext.cs`, `backend/Common/AppConstants.cs`: Các file dùng chung cốt lõi. Chỉ thêm khi thật sự cần thiết và không làm hỏng cấu hình chung của người khác.

### TUYỆT ĐỐI KHÔNG ĐƯỢC:
- **KHÔNG** sửa bất kỳ file nào trong thư mục `frontend/` (không sửa HTML, JS, CSS).
- **KHÔNG** tự ý sửa cấu trúc bảng / trigger trực tiếp trong database mà không đồng bộ với phiên Test/Database.
- **KHÔNG** tự ý sửa project test `backend.Tests/` nếu không được phân công (phiên Test phụ trách).

---

## 2. QUY TẮC LẬP TRÌNH BẮT BUỘC (THEO PLAN 5)

1. **Routing & Định danh API (A1):**
   - Đặt route cụ thể theo tên tiếng Việt mà frontend đã quy ước, ví dụ: `[Route("api/mathang")]`, `[Route("api/hoadon")]`, `[Route("api/baohanh")]`.
   - **CẤM** dùng `[Route("api/[controller]")]` mặc định (vì sẽ tự sinh ra tiếng Anh như `/api/products`, làm lệch frontend).

2. **Dữ liệu trả về (Serialization & Status Code):**
   - **Thành công:** Trả trực tiếp đối tượng hoặc danh sách thô (`Ok(data)`), **TUYỆT ĐỐI KHÔNG** bọc trong `{ success: true, data: ... }` hay `ApiResponse`.
   - **Lỗi:** Trả về cấu trúc chuẩn `{ "message": "Thông báo tiếng Việt", "errors": ["..."] }` với đúng mã HTTP:
     - `400`: Vi phạm dữ liệu hoặc logic nghiệp vụ (quá hạn đổi trả, thiếu trường...).
     - `401`: Chưa đăng nhập / token hết hạn.
     - `403`: Không đủ quyền.
     - `404`: Không tìm thấy bản ghi.
     - `409`: Xung đột (trùng IMEI, trùng SĐT, IMEI đã bán, sai trạng thái flow).
     - `500`: Lỗi hệ thống chung chung, không lộ stack trace ra client.
   - **Phân trang:** Nhận query `page` (default 1), `pageSize` (default 20, max 100). Trả mảng kết quả trong body; tổng số bản ghi đưa vào header `X-Total-Count`.

3. **Ngày giờ & Bảo mật:**
   - Ngày giờ luôn dùng `DateTime.UtcNow`. PostgreSQL Npgsql sẽ văng Exception nếu lưu DateTime không phải UTC vào cột `timestamptz`.
   - DTO tuyệt đối không bao giờ trả về trường `PasswordHash`.
   - Tuyệt đối không log mật khẩu, token, hoặc số điện thoại đầy đủ vào Console / File Log.

4. **Nghiệp vụ & Transaction:**
   - Validation đầu vào bằng **FluentValidation**, đặt tại `Features/<Module>/Validators`.
   - Mọi thao tác ghi từ 2 bảng trở lên phải nằm trong một transaction (`BeginTransactionAsync`).
   - `MaNV` thực hiện giao dịch phải lấy từ Token JWT thông qua `ICurrentUser`, **CẤM** nhận `MaNV` từ body của request client.
   - Truy vấn chỉ đọc luôn dùng `.AsNoTracking()`.

5. **Phân quyền (Authorization):**
   - Gắn `[Authorize(Roles = AppConstants.Roles....)]` trên từng Action cần bảo vệ.
   - Các API tra cứu công khai (sản phẩm, khuyến mãi, tra cứu bảo hành) gắn `[AllowAnonymous]`.

---

## 3. ĐỊNH NGHĨA HOÀN THÀNH (DEFINITION OF DONE)
1. Project build thành công, không phát sinh warning mới.
2. Endpoint xuất hiện đúng và gọi thử được thành công trên Swagger UI (`/swagger`).
3. Dữ liệu trả về đúng định dạng camelCase và không bọc vỏ.
4. Xử lý đầy đủ các trường hợp ngoại lệ (ném `AppException` với status 400, 404, 409 thay vì crash 500).

---

## 4. PROMPT MẪU ĐỂ DÁN VÀO ĐẦU PHIÊN CHAT BACKEND
*(Sao chép đoạn dưới đây vào ô chat của phiên Backend)*

```text
Bạn là Kỹ sư Backend .NET 10 chuyên trách của dự án PhoneStoreManagementSystem.
Nhiệm vụ của bạn:
1. Chỉ làm việc và viết code trong thư mục `backend/` (Controllers, Services, DTOs, Validators, EF Configurations).
2. Tuyệt đối không sửa code giao diện trong thư mục `frontend/`.
3. Tuân thủ nghiêm ngặt quy ước API của Plan5:
   - Route rõ ràng, ví dụ [Route("api/mathang")].
   - Trả dữ liệu thô (mảng/object), KHÔNG bọc ApiResponse.
   - Ngày giờ UTC (DateTime.UtcNow).
   - JSON camelCase, mã lỗi tiếng Việt dạng { message, errors }.
   - Lấy MaNV từ ICurrentUser (JWT), ghi nhiều bảng phải có transaction.
   - Phân quyền bằng [Authorize(Roles = ...)].
Hãy xác nhận đã nắm rõ quy chuẩn và hỏi tôi module backend cần thực hiện tiếp theo!
```
