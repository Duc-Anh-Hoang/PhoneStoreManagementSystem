/**
 * SAMAMOBILE.COM - PRODUCT DETAIL CONTROLLER (VERSION 6.4 - CHUẨN HOÀNG HÀ MOBILE)
 * Thiết kế giao diện chi tiết sản phẩm 100% khớp 5 Screenshots từ người dùng:
 * 1. Hero Showcase: Tên sản phẩm, SKU, Đánh giá 4.6 (10 đánh giá), Nút Thông số, So sánh, Thu cũ
 *    Gallery ảnh lớn có Prev/Next, counter badge 1/19, thumbnails (Video, Màu sắc, Specs, Info, QR)
 *    Thông số nổi bật 4 ô có icon, Cam kết sản phẩm 4 tiêu chí
 *    Phiên bản & Màu sắc có checkmark badge ở góc dưới phải
 *    Khung giá bán & Trả góp chia đôi nét đứt nền xanh #F0F6FE
 *    Cụm 3 nút CTA: Giỏ hàng (đỏ), MUA NGAY (đỏ lớn), TRẢ GÓP (xanh)
 * 2. Địa chỉ còn hàng và trải nghiệm (114 cửa hàng) có tìm kiếm, khu vực, tag Có hàng trải nghiệm, phone, Xem chi tiết, Đang sẵn hàng
 *    Sama Member Card (giảm 366.000₫ & EDU 200.000₫, mascot sóc)
 *    Thu cũ lên đời chỉ từ 26.124.000₫ (trợ giá đến 4.000.000₫)
 *    Ưu đãi thanh toán ngân hàng (TPBank, VPBank, VIB, ZaloPay, Kredivo)
 *    Sản phẩm mua kèm có filter tabs (Tất cả, Ốp lưng, Dán màn hình, Củ sạc, Sạc dự phòng)
 * 3. Bảng thông số kỹ thuật chi tiết chuẩn 2 cột Zebra Table (16 thông số kỹ thuật đầy đủ)
 * 4. Bài viết đánh giá chuyên sâu có Mục lục nội dung chính (5 mục clickable), định dạng bài viết chuẩn,
 *    nút Xem thêm / Thu gọn có lớp phủ mờ chuyển màu gradient mượt mà
 *    Video đánh giá trên tay & Gói bảo hành mở rộng SamaCare (6 ô HCare)
 * 5. Khối Đánh giá & Bình luận: Điểm số 4.6/5, 5 thanh tiến trình sao (90%, 0%, 0%, 10%, 0%),
 *    Ảnh đánh giá từ khách hàng, Form gửi bình luận nhanh 3 input + textarea
 * 6. Sticky bottom purchase bar tự động xuất hiện khi cuộn
 * 7. Tương thích thông minh & năng động với MỌI thiết bị (Điện thoại, Laptop, Tablet, Smartwatch, Tai nghe, Phụ kiện)
 */

(function () {
  'use strict';

  var BROKEN_IMAGE_REMAP = {
    "iphone-16-pro-natural-titan.png": "https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png",
    "galaxy-s25-ultra-titan-den-1.png": "https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png",
    "galaxy-s24-ultra-xam-1.png": "https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png",
    "galaxy-s24-ultra-vang-1.png": "https://i.hoanghamobile.vn/Uploads/2024/01/16/s24-ultra-vang_638409930027889246.png",
    "ipad-gen-10-silver.png": "https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-wifi-pink-pdp-image-position-1-vn-vi.png",
    "tab-s9-fe-gray-1.png": "https://i.hoanghamobile.vn/Uploads/2026/10/15/s10-fe-3-134808082.png",
    "macbook-air-m3-midnight-1.png": "https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg",
    "macbook-air-13-in-m5-space-gray-pdp-image-position-1-vn-vi.jpg": "https://i.hoanghamobile.vn/Uploads/2026/10/28/macbook-pro-14-inch-m6-space-black-pdp-image-position-1-vi-vn-145528508.png",
    "asus-tuf-gaming-f15-fx506hf-hn014w-1.png": "https://i.hoanghamobile.vn/Uploads/2026/06/02/asus-tuf-1.png",
    "man-hinh-gaming-asus-tuf-vg249q3a-1.png": "https://i.hoanghamobile.vn/Uploads/2026/04/06/vg249q5r-1.png",
    "dell-ultrasharp-u2724d-1.png": "https://i.hoanghamobile.vn/Uploads/2024/02/22/u2424he.png",
    "dieu-hoa-casper-inverter-tc-09is36-1.png": "https://i.hoanghamobile.vn/Uploads/MayLanh/4757/Slider/casper-tc-09is35-2-1.jpg",
    "dieu-hoa-daikin-inverter-ftkb35xvmv-1.png": "https://i.hoanghamobile.vn/Uploads/2025/04/16/ftkb25yvmv-1.png",
    "xiaomi-smart-band-9-black-1.png": "https://i.hoanghamobile.vn/Uploads/2024/11/27/n69-pink-side45.png",
    "airpods-4-1.png": "https://i.hoanghamobile.vn/Uploads/2024/09/10/airpods-4.png",
    "marshall-emberton-ii-black-and-brass-1.png": "https://i.hoanghamobile.vn/Uploads/2026/07/30/marshall-stockwell-lll-trang-1-153237088.png",
    "anker-511-nano-3-30w-a2147-1.png": "https://i.hoanghamobile.vn/Uploads/2026/03/21/a1638-den.png"
  };

  function resolveAssetPath(path) {
    if (!path) return '';
    for (const [brokenFile, fixedUrl] of Object.entries(BROKEN_IMAGE_REMAP)) {
      if (path.includes(brokenFile)) return fixedUrl;
    }
    if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:') || path.startsWith('//')) {
      return path;
    }
    const clean = path.replace(/^\.\.\//, '').replace(/^\//, '');
    const isInsidePages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
    return isInsidePages ? ('../' + clean) : clean;
  }

  async function fetchJsonSafely(relPath) {
    const clean = relPath.replace(/^\.\.\//, '').replace(/^\//, '');
    const candidates = [
      resolveAssetPath(clean),
      '../' + clean,
      clean,
      './' + clean,
      '../../' + clean,
      '/frontend/' + clean
    ];
    const unique = [...new Set(candidates)];
    for (const url of unique) {
      try {
        const res = await fetch(url);
        if (res.ok) {
          return await res.json();
        }
      } catch (e) {}
    }
    return null;
  }

  var currentProduct = null;
  var selectedVersion = null;
  var selectedColor = null;
  var allProductsMeta = {};
  var currentGalleryIndex = 0;
  var galleryImages = [];
  var isArticleExpanded = false;

  // Danh mục 114 chi nhánh của SAMAMOBILE chuẩn Hoàng Hà Mobile (Screenshot 2)
  var STORE_LOCATIONS = [
    { city: 'hanoi', name: '194 Lê Duẩn, Phường Văn Miếu - Quốc Tử Giám, Hà Nội', phone: '0962066208', stock: true, demo: true },
    { city: 'hanoi', name: '196 Quang Trung, Phường Hà Đông, Hà Nội', phone: '0375866186', stock: true, demo: true },
    { city: 'thanhhoa', name: '222 Trần Phú, Phường Hạc Thành, Thanh Hóa', phone: '0888203536', stock: true, demo: true },
    { city: 'hanoi', name: '28 Trần Phú, Phường Hà Đông, Hà Nội', phone: '0903202328', stock: true, demo: true },
    { city: 'hcm', name: '232 Nguyễn Thái Học, Phường Quy Nhơn Nam, TP.HCM', phone: '0898198383', stock: true, demo: true },
    { city: 'hanoi', name: '122 Thái Hà, Phường Đống Đa, Hà Nội', phone: '0973790122', stock: true, demo: true },
    { city: 'hcm', name: '241 Lê Văn Việt, Tăng Nhơn Phú B, TP. Thủ Đức, TP.HCM', phone: '18006868', stock: true, demo: true },
    { city: 'hcm', name: '55 Trần Quang Khải, Phường Tân Định, Quận 1, TP.HCM', phone: '0908889977', stock: true, demo: true },
    { city: 'danang', name: '153 Hàm Nghi, Quận Thanh Khê, Đà Nẵng', phone: '0905556677', stock: true, demo: true },
    { city: 'danang', name: '580 Điện Biên Phủ, Quận Thanh Khê, Đà Nẵng', phone: '0905123789', stock: true, demo: true },
    { city: 'haiphong', name: 'Số 258 Đường Tô Hiệu, Phường Lê Chân, Hải Phòng', phone: '0961791516', stock: true, demo: true },
    { city: 'cantho', name: '221 Đường 30 Tháng 4, Phường Hưng Lợi, Ninh Kiều, Cần Thơ', phone: '0939998877', stock: true, demo: true },
    { city: 'hcm', name: '1060 Quang Trung, Phường 8, Gò Vấp, TP.HCM', phone: '0903332211', stock: true, demo: true },
    { city: 'hcm', name: '347 Hoàng Văn Thụ, Phường 2, Tân Bình, TP.HCM', phone: '0902223344', stock: true, demo: true },
    { city: 'hanoi', name: '348 Hồ Tùng Mậu, Phường Cầu Diễn, Hà Nội', phone: '0965868348', stock: true, demo: true },
    { city: 'hanoi', name: '382 Nguyễn Văn Cừ, Phường Bồ Đề, Hà Nội', phone: '0915963222', stock: true, demo: true },
    { city: 'hanoi', name: '392 Cầu Giấy, Phường Cầu Giấy, Hà Nội', phone: '0968323399', stock: true, demo: true }
  ];

  // Sản phẩm nền mặc định chuẩn Hoàng Hà Samsung S26 Ultra (Khớp 100% Ảnh 1)
  var BUILTIN_PRODUCTS = {
    'SAMPLE-dien-thoai-1': {
      maHang: 'SAMPLE-dien-thoai-1',
      sku: 'S948B256TRANG',
      tenHang: 'Samsung Galaxy S26 Ultra',
      nhanHieu: 'Samsung',
      tenLoai: 'Điện thoại',
      tenNhom: 'Điện thoại',
      donGia: 30490000,
      giaGoc: 40990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-trang-1.png',
      album: [
        { label: 'Mặt trước & S-Pen', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-trang-1.png' },
        { label: 'Góc nghiêng Xanh', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-xanh-1.png' },
        { label: 'Mặt sau & 4 Camera', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-den-1.png' },
        { label: 'Toàn cảnh viền Titan', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' },
        { label: 'Cận cảnh Camera Zoom', img: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' }
      ],
      phienBan: [
        { id: 'v_256', dungLuong: 'S26 Ultra 12GB/256GB', gia: 30490000, giaGoc: 40990000 },
        { id: 'v_512', dungLuong: 'S26 Ultra 12GB/512GB', gia: 35490000, giaGoc: 44990000 },
        { id: 'v_1tb', dungLuong: 'S26 Ultra 16GB/1TB', gia: 41990000, giaGoc: 51990000 }
      ],
      mauSac: [
        { id: 'c_trang', ten: 'Trắng', hex: '#FFFFFF', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-trang-1.png' },
        { id: 'c_xanh', ten: 'Xanh Dương', hex: '#38BDF8', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-xanh-1.png' },
        { id: 'c_den', ten: 'Đen', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-den-1.png' },
        { id: 'c_tim', ten: 'Tím', hex: '#6366F1', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' }
      ],
      thongSoNoiBat: {
        'Kích thước màn hình': '6.9\"',
        'Bộ nhớ trong': '256GB',
        'Vi xử lý': 'Snap 8 Elite Gen 5',
        'Dung lượng pin hiển thị': '5000mAh'
      },
      thongSoChiTiet: {
        'Kích thước màn hình': '6.9\"',
        'Độ phân giải camera': 'Camera siêu rộng 50MP, Camera góc rộng 200 MP, Camera Tele (5x) 50MP, Camera Tele (3x) 10MP',
        'Độ phân giải camera trước': '12 MP',
        'Vi xử lý': 'Snapdragon 8 Elite Gen 5 dành cho Galaxy (3nm)',
        'Hệ điều hành': 'Android 16',
        'RAM': '12GB',
        'Bộ nhớ trong': '256GB',
        'Kết Nối NFC': 'Có',
        'Mạng di động': '2G, 3G, 4G, 5G',
        'Wi-Fi': 'Có (Wi-Fi 7)',
        'Cổng kết nối/sạc': 'Type-C',
        'Bluetooth': 'Có (5.4)',
        'Kích thước': 'Dài 163.6mm - Ngang 78.1mm - Dày 7.9mm',
        'Trọng lượng': '214g',
        'Dung lượng pin sản phẩm': '5000mAh',
        'Kháng nước, kháng bụi': 'IP68'
      }
    },
    'MH796': {
      maHang: 'MH796',
      sku: 'TABS12U12256',
      tenHang: 'Samsung Galaxy Tab S12 Ultra Wi-Fi 12GB/256GB',
      nhanHieu: 'Samsung',
      tenLoai: 'Máy tính bảng',
      tenNhom: 'Máy tính bảng',
      category: 'tablet',
      donGia: 34990000,
      giaGoc: 39990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png',
      album: [
        { label: 'Mặt trước & Bút', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png' },
        { label: 'Kèm bàn phím', img: 'https://i.hoanghamobile.vn/Uploads/2025/10/30/galaxy-tab-s11-ultra-2.png' },
        { label: 'Góc nghiêng 45°', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-plus-1-161156166.png' },
        { label: 'Bàn phím & Touchpad', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/08/ban-phim-samsung-galaxy-tab-s12-plus-1-110353435.png' },
        { label: 'Bút S-Pen & Cạnh viền', img: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' }
      ],
      phienBan: [
        { id: 'v_256', dungLuong: '12GB/256GB', gia: 34990000, giaGoc: 39990000 },
        { id: 'v_512', dungLuong: '16GB/512GB', gia: 38990000, giaGoc: 44990000 }
      ],
      mauSac: [
        { id: 'c_xam', ten: 'Xám Titan', hex: '#64748B', anh: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png' },
        { id: 'c_bac', ten: 'Bạc Ánh Kim', hex: '#E2E8F0', anh: 'https://i.hoanghamobile.vn/Uploads/2025/10/30/galaxy-tab-s11-ultra-2.png' }
      ],
      thongSoNoiBat: {
        'Kích thước màn hình': '14.6" Dynamic AMOLED 2X',
        'Bộ nhớ trong': '256GB / RAM 12GB',
        'Vi xử lý': 'MediaTek Dimensity 9500 (3nm)',
        'Dung lượng pin hiển thị': '11.600mAh (Sạc 45W)'
      },
      thongSoChiTiet: {
        'Kích thước màn hình': '14.6 inch Dynamic AMOLED 2X, 120Hz, HDR10+',
        'Độ phân giải màn hình': '1848 x 2960 pixels, tỷ lệ 16:10',
        'Độ phân giải camera': 'Camera chính 13MP, Camera góc siêu rộng 8MP',
        'Độ phân giải camera trước': 'Camera kép 12MP (Rộng & Siêu rộng)',
        'Vi xử lý': 'MediaTek Dimensity 9500 (3nm đỉnh cao đa nhiệm)',
        'Hệ điều hành': 'Android 16, One UI 8.0 (Galaxy AI)',
        'RAM': '12GB LPDDR5X',
        'Bộ nhớ trong': '256GB UFS 4.0 (hỗ trợ thẻ MicroSD lên đến 1.5TB)',
        'Kết Nối NFC': 'Không',
        'Mạng di động': 'Wi-Fi 7 (802.11be), 3 băng tần',
        'Wi-Fi': 'Có (Wi-Fi 7)',
        'Cổng kết nối/sạc': 'Type-C USB 3.2 Gen 1 (DisplayPort out)',
        'Bluetooth': 'Có (Bluetooth 5.4)',
        'Kích thước': 'Dài 326.4mm - Ngang 208.6mm - Dày 5.4mm',
        'Trọng lượng': '723g',
        'Dung lượng pin sản phẩm': '11.600mAh, sạc nhanh 45W',
        'Kháng nước, kháng bụi': 'IP68 cho cả máy tính bảng và bút S-Pen'
      }
    },
    'MH001': {
      maHang: 'MH001',
      sku: 'IP18PM256TN',
      tenHang: 'iPhone 18 Pro Max 256GB - Chính hãng VN/A',
      nhanHieu: 'Apple',
      tenLoai: 'Điện thoại',
      tenNhom: 'Điện thoại',
      donGia: 41790000,
      giaGoc: 44990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
      album: [
        { label: 'Mặt trước & Sau', img: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png' },
        { label: 'Góc nghiêng Trắng', img: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' },
        { label: 'Cụm 3 Camera 48MP', img: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-sa-mac-1.png' },
        { label: 'Viền Titan & Cổng sạc', img: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png' }
      ],
      phienBan: [
        { id: 'v_256', dungLuong: '256GB', gia: 41790000, giaGoc: 44990000 },
        { id: 'v_512', dungLuong: '512GB', gia: 47990000, giaGoc: 51990000 },
        { id: 'v_1tb', dungLuong: '1TB', gia: 59990000, giaGoc: 63990000 },
        { id: 'v_2tb', dungLuong: '2TB', gia: 78990000, giaGoc: 82990000 }
      ],
      mauSac: [
        { id: 'c_den', ten: 'Đen', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' },
        { id: 'c_bac', ten: 'Bạc', hex: '#E2E8F0', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' },
        { id: 'c_burgundy', ten: 'Đỏ Burgundy', hex: '#5B1E31', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png' },
        { id: 'c_bang_thanh', ten: 'Băng Thanh', hex: '#B0C4DE', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' }
      ],
      thongSoNoiBat: {
        'Kích thước màn hình': '6.9\" Super Retina XDR',
        'Bộ nhớ trong': '256GB NVMe',
        'Vi xử lý': 'Apple A20 Pro',
        'Dung lượng pin hiển thị': '4850mAh'
      },
      thongSoChiTiet: {
        'Kích thước màn hình': '6.9\" Super Retina XDR OLED, ProMotion 120Hz, 2500 nits',
        'Độ phân giải camera': 'Camera chính 48MP Fusion, Camera siêu rộng 48MP, Camera Tele 5x 12MP',
        'Độ phân giải camera trước': '12 MP TrueDepth Autofocus',
        'Vi xử lý': 'Apple A20 Pro (3nm, 6 nhân CPU & 6 nhân GPU)',
        'Hệ điều hành': 'iOS 19 (Apple Intelligence Tiếng Việt)',
        'RAM': '8GB',
        'Bộ nhớ trong': '256GB NVMe',
        'Kết Nối NFC': 'Có',
        'Mạng di động': '5G sub-6 GHz, eSIM & SIM vật lý',
        'Wi-Fi': 'Có (Wi-Fi 7)',
        'Cổng kết nối/sạc': 'Type-C USB 3 (10Gb/s)',
        'Bluetooth': 'Có (Bluetooth 5.4)',
        'Kích thước': 'Dài 163.0mm - Ngang 77.6mm - Dày 8.25mm',
        'Trọng lượng': '227g',
        'Dung lượng pin sản phẩm': '4850mAh, Sạc nhanh MagSafe 25W',
        'Kháng nước, kháng bụi': 'IP68 (độ sâu 6 mét trong 30 phút)'
      }
    }
  };

  // Khởi tạo trang
  async function init() {
    setupMainHeader();
    setupStickyPurchaseBar();
    setupCommentForm();
    await loadProductsData();
    resolveAndRenderProduct();
  }

  // Header helpers
  function setupMainHeader() {
    window.toggleMegaMenu = function () {
      const menu = document.getElementById('mega-menu-flyout');
      if (menu) menu.classList.toggle('hidden');
    };

    window.toggleNotificationPopover = function () {
      const pop = document.getElementById('notification-popover');
      if (pop) pop.classList.toggle('hidden');
    };

    window.quickSearchTag = function (tag) {
      window.location.href = 'category.html?q=' + encodeURIComponent(tag);
    };

    window.filterPhoneBrand = function (brand) {
      window.location.href = 'category.html?type=dienthoai&brand=' + encodeURIComponent(brand);
    };

    window.applySearchFilters = function () {
      const input = document.getElementById('search-input');
      const val = input ? input.value.trim() : '';
      if (val) window.location.href = 'category.html?q=' + encodeURIComponent(val);
    };

    const searchInput = document.getElementById('search-input');
    const clearBtn = document.getElementById('btn-clear-search');
    if (searchInput) {
      searchInput.addEventListener('input', function () {
        if (clearBtn) clearBtn.classList.toggle('hidden', !this.value.trim());
      });
      searchInput.addEventListener('keydown', function (e) {
        if (e.key === 'Enter') {
          e.preventDefault();
          window.applySearchFilters();
        }
      });
    }
    if (clearBtn && searchInput) {
      clearBtn.addEventListener('click', function () {
        searchInput.value = '';
        clearBtn.classList.add('hidden');
        searchInput.focus();
      });
    }
  }

  async function loadProductsData() {
    try {
      const data = await fetchJsonSafely('data/products-meta.json');
      if (data) allProductsMeta = data;
    } catch (e) {
      console.warn('Could not load products-meta.json', e);
    }
  }

  function getQueryParam(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
  }

  function resolveAndRenderProduct() {
    const rawId = getQueryParam('id') || getQueryParam('maHang');
    const id = rawId ? rawId.trim() : '';
    const dashId = id.replace(/\s+/g, '-');
    const slug = getQueryParam('slug');

    let prod = null;

    if (id && allProductsMeta[id]) prod = allProductsMeta[id];
    else if (dashId && allProductsMeta[dashId]) prod = allProductsMeta[dashId];
    else if (id && BUILTIN_PRODUCTS[id]) prod = BUILTIN_PRODUCTS[id];
    else if (dashId && BUILTIN_PRODUCTS[dashId]) prod = BUILTIN_PRODUCTS[dashId];

    if (!prod) {
      try {
        const stored = sessionStorage.getItem('current_view_product');
        if (stored) {
          const item = JSON.parse(stored);
          if (item && (!id || item.id === id || item.maHang === id || item.id === dashId || item.maHang === dashId)) {
            prod = item.rawProduct || item;
          }
        }
      } catch (e) {}
    }

    if (!prod && slug) {
      prod = Object.values(allProductsMeta).find(p => p.slug && p.slug.includes(slug));
    } else if (!prod && id) {
      prod = Object.values(allProductsMeta).find(p => p.maHang === id || p.id === id);
    }

    // Mặc định chuẩn Samsung Galaxy S26 Ultra (Khớp ảnh 1 của User)
    if (!prod) {
      prod = BUILTIN_PRODUCTS['SAMPLE-dien-thoai-1'];
    }

    currentProduct = normalizeProduct(prod);
    renderProductDetail(currentProduct);
  }

  // Phát hiện danh mục và chuẩn hóa toàn bộ dữ liệu cấu hình
  function detectProductCategory(p) {
    const catStr = ((p.maNhom || '') + ' ' + (p.tenNhom || '') + ' ' + (p.tenLoai || '') + ' ' + (p.tenHang || '')).toLowerCase();
    if (catStr.includes('laptop') || catStr.includes('macbook') || catStr.includes('mbook') || catStr.includes('asus tuf') || catStr.includes('dell')) return 'laptop';
    if (catStr.includes('tablet') || catStr.includes('ipad') || catStr.includes('tab s')) return 'tablet';
    if (catStr.includes('watch') || catStr.includes('đồng hồ') || catStr.includes('smart band')) return 'watch';
    if (catStr.includes('airpods') || catStr.includes('tai nghe') || catStr.includes('loa') || catStr.includes('âm thanh') || catStr.includes('marshall') || catStr.includes('sony')) return 'audio';
    if (catStr.includes('sạc') || catStr.includes('cáp') || catStr.includes('ốp lưng') || catStr.includes('pin dự phòng') || catStr.includes('phụ kiện') || catStr.includes('anker')) return 'accessory';
    if (catStr.includes('tivi') || catStr.includes('màn hình') || catStr.includes('tv')) return 'screen';
    return 'phone';
  }

  function normalizeProduct(p) {
    const donGia = Number(p.donGia) || 30490000;
    const giaGoc = Number(p.giaGoc) || Math.round(donGia * 1.25);
    const anh = resolveAssetPath(p.anhChinh || p.image || 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png');
    const category = detectProductCategory(p);
    const pName = p.tenHang || 'Samsung Galaxy S26 Ultra';
    const pNameLower = pName.toLowerCase();

    // 1. Phân loại Phiên bản
    let phienBan = p.phienBan && p.phienBan.length > 0 ? p.phienBan : [];
    if (phienBan.length <= 1) {
      if (category === 'phone') {
        if (pNameLower.includes('s26') || pNameLower.includes('s25') || pNameLower.includes('s24')) {
          phienBan = [
            { id: 'v_256', dungLuong: '12GB/256GB', gia: donGia, giaGoc: giaGoc },
            { id: 'v_512', dungLuong: '12GB/512GB', gia: Math.round(donGia * 1.16), giaGoc: Math.round(giaGoc * 1.12) },
            { id: 'v_1tb', dungLuong: '16GB/1TB', gia: Math.round(donGia * 1.37), giaGoc: Math.round(giaGoc * 1.28) }
          ];
        } else if (pNameLower.includes('iphone')) {
          phienBan = [
            { id: 'v_256', dungLuong: '256GB', gia: donGia, giaGoc: giaGoc },
            { id: 'v_512', dungLuong: '512GB', gia: Math.round(donGia * 1.15), giaGoc: Math.round(giaGoc * 1.12) },
            { id: 'v_1tb', dungLuong: '1TB', gia: Math.round(donGia * 1.43), giaGoc: Math.round(giaGoc * 1.35) },
            { id: 'v_2tb', dungLuong: '2TB', gia: Math.round(donGia * 1.88), giaGoc: Math.round(giaGoc * 1.75) }
          ];
        } else {
          phienBan = [
            { id: 'v_std', dungLuong: 'Tiêu chuẩn', gia: donGia, giaGoc: giaGoc }
          ];
        }
      } else if (category === 'laptop') {
        phienBan = [
          { id: 'v_16_512', dungLuong: '16GB/512GB', gia: donGia, giaGoc: giaGoc },
          { id: 'v_24_1tb', dungLuong: '24GB/1TB', gia: Math.round(donGia * 1.25), giaGoc: Math.round(giaGoc * 1.2) }
        ];
      } else if (category === 'tablet') {
        phienBan = [
          { id: 'v_128', dungLuong: '128GB Wi-Fi', gia: donGia, giaGoc: giaGoc },
          { id: 'v_256', dungLuong: '256GB Wi-Fi', gia: Math.round(donGia * 1.15), giaGoc: Math.round(giaGoc * 1.12) }
        ];
      } else {
        phienBan = [
          { id: 'v_std', dungLuong: 'Tiêu chuẩn', gia: donGia, giaGoc: giaGoc }
        ];
      }
    }

    // 2. Phân loại Màu sắc
    let mauSac = p.mauSac && p.mauSac.length > 0 ? p.mauSac : [];
    if (mauSac.length === 0 || (mauSac.length === 1 && mauSac[0].ten === 'Tiêu chuẩn')) {
      if (pNameLower.includes('s26') || pNameLower.includes('s25') || pNameLower.includes('s24')) {
        mauSac = [
          { id: 'c_trang', ten: 'Trắng', hex: '#FFFFFF', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' },
          { id: 'c_xanh', ten: 'Xanh Dương', hex: '#38BDF8', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' },
          { id: 'c_den', ten: 'Đen', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' },
          { id: 'c_tim', ten: 'Tím', hex: '#6366F1', anh: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' }
        ];
      } else if (pNameLower.includes('iphone 18') || pNameLower.includes('iphone 16') || pNameLower.includes('iphone')) {
        mauSac = [
          { id: 'c_den', ten: 'Đen', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' },
          { id: 'c_bac', ten: 'Bạc', hex: '#E2E8F0', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' },
          { id: 'c_burgundy', ten: 'Đỏ Burgundy', hex: '#5B1E31', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png' },
          { id: 'c_bang_thanh', ten: 'Băng Thanh', hex: '#B0C4DE', anh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' }
        ];
      } else {
        mauSac = [
          { id: 'c_default', ten: 'Mặc định', hex: '#2563EB', anh: anh }
        ];
      }
    }
    mauSac = mauSac.map(c => Object.assign({}, c, { anh: resolveAssetPath(c.anh || anh) }));

    // 3. Thông số nổi bật 4 ô có Icon theo danh mục
    let thongSoNoiBat = p.thongSoNoiBat;
    if (!thongSoNoiBat) {
      if (category === 'laptop') {
        thongSoNoiBat = {
          'Kích thước màn hình': p.thongSo?.['Màn hình'] || '15.3" Liquid Retina',
          'Vi xử lý': p.thongSo?.['Số hiệu CPU'] || 'Apple M3 / Core i7',
          'RAM & Ổ cứng': p.thongSo?.['RAM'] ? (p.thongSo['RAM'] + ' / 512GB') : '16GB / 512GB',
          'Trọng lượng pin': '1.51 kg • 70Wh'
        };
      } else if (category === 'tablet') {
        thongSoNoiBat = {
          'Kích thước màn hình': p.thongSo?.['Màn hình'] || '11" Tandem OLED',
          'Bộ nhớ trong': p.thongSo?.['Bộ nhớ trong'] || '256GB',
          'Vi xử lý': p.thongSo?.['Vi xử lý'] || 'Apple M4 / 8 Gen 2',
          'Dung lượng pin hiển thị': '8340mAh (30W)'
        };
      } else if (category === 'audio') {
        thongSoNoiBat = {
          'Thời lượng pin': p.thongSo?.['Thời gian sử dụng'] || '6h / 30h hộp sạc',
          'Chống ồn': p.thongSo?.['Chống ồn'] || 'ANC chủ động 2X',
          'Kháng nước': 'IP54 chống nước',
          'Kết nối': 'Bluetooth 5.3'
        };
      } else if (category === 'watch') {
        thongSoNoiBat = {
          'Kích thước mặt': '46mm AMOLED',
          'Thời lượng pin': 'Đến 14 ngày',
          'Kháng nước': '5 ATM (50 mét)',
          'Tương thích': 'Android & iOS'
        };
      } else if (category === 'accessory') {
        thongSoNoiBat = {
          'Công suất': '30W / 65W GaN',
          'Cổng ra': 'Type-C PD 3.0',
          'Công nghệ': 'GaN III tản mát',
          'Bảo hành': '18 tháng 1 đổi 1'
        };
      } else {
        // Điện thoại
        thongSoNoiBat = {
          'Kích thước màn hình': p.thongSo?.['Kích thước màn hình'] || '6.9\"',
          'Bộ nhớ trong': p.thongSo?.['Bộ nhớ trong'] || '256GB',
          'Vi xử lý': p.thongSo?.['Vi xử lý'] || 'Snap 8 Elite Gen 5',
          'Dung lượng pin hiển thị': p.thongSo?.['Dung lượng pin'] || '5000mAh'
        };
      }
    }

    // 4. Bảng thông số kỹ thuật chi tiết chuẩn 16 hàng (Screenshot 3)
    let thongSoChiTiet = p.thongSoChiTiet;
    if (!thongSoChiTiet) {
      if (category === 'laptop') {
        thongSoChiTiet = {
          'Kích thước màn hình': '15.3 inch Liquid Retina (2880 x 1864), 500 nits, dải màu P3',
          'Vi xử lý': 'Apple M3 (8-core CPU với 4 lõi hiệu năng cao & 4 lõi tiết kiệm)',
          'Card đồ họa (GPU)': '10-core GPU kiến trúc mới, hỗ trợ Ray Tracing',
          'Hệ điều hành': 'macOS Sequoia chính hãng',
          'RAM': '16GB Unified Memory',
          'Ổ cứng lưu trữ': '512GB SSD tốc độ cao',
          'Cổng kết nối': '2x Thunderbolt 4 / USB 4, Cổng sạc MagSafe 3, Jack tai nghe 3.5mm',
          'Bàn phím & Touchpad': 'Magic Keyboard tích hợp Touch ID, bàn di chuột Force Touch',
          'Webcam': '1080p FaceTime HD Camera',
          'Âm thanh': 'Hệ thống 6 loa stereo với Spatial Audio & Dolby Atmos',
          'Wi-Fi': 'Wi-Fi 6E (802.11ax)',
          'Bluetooth': 'Bluetooth 5.3',
          'Kích thước': 'Dày 1.15 cm - Dài 34.04 cm - Rộng 23.76 cm',
          'Trọng lượng': '1.51 kg',
          'Dung lượng pin': '66.5Wh Lithium-polymer, thời lượng đến 18 giờ',
          'Bảo hành': '12 tháng chính hãng tại trung tâm Apple Authorized'
        };
      } else if (category === 'audio') {
        thongSoChiTiet = {
          'Loại tai nghe': 'True Wireless In-ear không dây thông minh',
          'Chipset xử lý': 'Apple H2 / DSP Custom Hi-Res Audio 24-bit',
          'Công nghệ chống ồn': 'Chống ồn chủ động ANC 2X, Chế độ Xuyên âm thích ứng Adaptive',
          'Công nghệ âm thanh': 'Spatial Audio cá nhân hóa theo dõi cử động đầu, EQ thích ứng',
          'Thời lượng pin tai nghe': 'Lên đến 6 giờ nghe liên tục (bật ANC)',
          'Thời lượng hộp sạc': 'Lên đến 30 giờ khi kết hợp hộp sạc MagSafe USB-C',
          'Sạc nhanh': '5 phút sạc trong hộp nghe được 1 giờ',
          'Cổng kết nối/sạc': 'Type-C, Sạc không dây MagSafe, chuẩn Qi',
          'Bluetooth': 'Bluetooth 5.3 độ trễ cực thấp',
          'Microphone': 'Hệ thống micro kép dạng chùm sóng khử tạp âm gió',
          'Kháng nước & bụi': 'IP54 cho cả tai nghe và hộp sạc',
          'Trọng lượng': 'Tai nghe: 5.3g mỗi bên - Hộp sạc: 50.8g',
          'Tương thích': 'iOS, iPadOS, macOS, watchOS, Android, Windows',
          'Bảo hành': '12 tháng chính hãng'
        };
      } else {
        // Điện thoại chuẩn Screenshot 3
        thongSoChiTiet = {
          'Kích thước màn hình': p.thongSo?.['Kích thước màn hình'] || '6.9\"',
          'Độ phân giải camera': p.thongSo?.['Camera sau'] || 'Camera siêu rộng 50MP, Camera góc rộng 200 MP, Camera Tele (5x) 50MP, Camera Tele (3x) 10MP',
          'Độ phân giải camera trước': p.thongSo?.['Camera trước'] || '12 MP',
          'Vi xử lý': p.thongSo?.['Vi xử lý'] || 'Snapdragon 8 Elite Gen 5 dành cho Galaxy (3nm)',
          'Hệ điều hành': p.thongSo?.['Hệ điều hành'] || 'Android 16',
          'RAM': '12GB',
          'Bộ nhớ trong': p.thongSo?.['Bộ nhớ trong'] || '256GB',
          'Kết Nối NFC': 'Có',
          'Mạng di động': '2G, 3G, 4G, 5G',
          'Wi-Fi': 'Có (Wi-Fi 7)',
          'Cổng kết nối/sạc': 'Type-C',
          'Bluetooth': 'Có (5.4)',
          'Kích thước': 'Dài 163.6mm - Ngang 78.1mm - Dày 7.9mm',
          'Trọng lượng': '214g',
          'Dung lượng pin sản phẩm': p.thongSo?.['Dung lượng pin'] || '5000mAh',
          'Kháng nước, kháng bụi': 'IP68'
        };
      }
    }

    // SKU
    let sku = p.sku;
    if (!sku) {
      if (pNameLower.includes('s26')) sku = 'S948B256TRANG';
      else if (pNameLower.includes('s25')) sku = 'S938BZKCXEV';
      else if (pNameLower.includes('iphone')) sku = 'IP18PM256TN';
      else sku = (p.maHang || 'SP') + '256' + Math.floor(Math.random() * 90 + 10);
    }

    // 5. Album ảnh nhiều góc độ
    let album = p.album;
    if (!album || !Array.isArray(album) || album.length === 0) {
      if (pNameLower.includes('tab s12') || pNameLower.includes('tab s11')) {
        album = [
          { label: 'Mặt trước & Bút', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-ultra-2-161156363.png' },
          { label: 'Kèm bàn phím', img: 'https://i.hoanghamobile.vn/Uploads/2025/10/30/galaxy-tab-s11-ultra-2.png' },
          { label: 'Góc nghiêng 45°', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/galaxy-tab-s12-plus-1-161156166.png' },
          { label: 'Bàn phím & Touchpad', img: 'https://i.hoanghamobile.vn/Uploads/2026/10/08/ban-phim-samsung-galaxy-tab-s12-plus-1-110353435.png' },
          { label: 'Bút S-Pen & Cạnh viền', img: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' }
        ];
      } else if (pNameLower.includes('s26') || pNameLower.includes('s25') || pNameLower.includes('s24')) {
        album = [
          { label: 'Mặt trước & S-Pen', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-trang-1.png' },
          { label: 'Góc nghiêng Xanh', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-xanh-1.png' },
          { label: 'Mặt sau & 4 Camera', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-den-1.png' },
          { label: 'Toàn cảnh viền Titan', img: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png' },
          { label: 'Cận cảnh Camera Zoom', img: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' }
        ];
      } else if (pNameLower.includes('iphone 18') || pNameLower.includes('iphone 16') || pNameLower.includes('iphone 15')) {
        album = [
          { label: 'Mặt trước & Sau', img: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png' },
          { label: 'Góc nghiêng Trắng', img: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-glacier-pdp-images-vi-vn-021233298.png' },
          { label: 'Cụm 3 Camera 48MP', img: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-sa-mac-1.png' },
          { label: 'Viền Titan & Cổng sạc', img: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png' }
        ];
      } else if (category === 'laptop' || pNameLower.includes('macbook')) {
        album = [
          { label: 'Mở máy trực diện', img: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg' },
          { label: 'Cạnh bên siêu mỏng', img: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-midnight-pdp-image-position-1-vn-vi.jpg' },
          { label: 'Bàn phím & Touchpad', img: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-space-gray-pdp-image-position-1-vn-vi.jpg' },
          { label: 'Nắp lưng kim loại', img: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-silver-pdp-image-position-1-vn-vi.jpg' }
        ];
      } else {
        album = [{ label: 'Mặt trước', img: anh }];
        if (Array.isArray(mauSac) && mauSac.length > 1) {
          mauSac.slice(1).forEach(c => {
            if (c.anh && !album.some(a => a.img === c.anh)) {
              album.push({ label: 'Màu ' + c.ten, img: c.anh });
            }
          });
        }
      }
    }

    return {
      maHang: p.maHang || 'SP' + Math.floor(Math.random() * 9000 + 1000),
      sku: sku,
      tenHang: pName,
      nhanHieu: p.nhanHieu || 'Chính hãng',
      tenLoai: p.tenLoai || 'Điện thoại',
      tenNhom: p.tenNhom || 'Điện thoại',
      category: category,
      donGia: donGia,
      giaGoc: giaGoc,
      anhChinh: anh,
      album: album,
      mauSac: mauSac,
      phienBan: phienBan,
      thongSoNoiBat: thongSoNoiBat,
      thongSoChiTiet: thongSoChiTiet
    };
  }

  function formatCurrency(val) {
    if (!val || isNaN(val)) return '0₫';
    return Number(val).toLocaleString('vi-VN') + '₫';
  }

  function formatProductVariantTitle(baseTitle, variantName) {
    if (!baseTitle) return 'Sản phẩm';
    const v = (variantName || '').trim();
    if (!v || v === 'Tiêu chuẩn' || v === 'Mặc định') return baseTitle;
    const capRegex = /(\d+\s*GB\s*\/\s*\d+\s*(?:GB|TB)|\b(?:64|128|256|512)\s*GB\b|\b[12]\s*TB\b)/i;
    if (capRegex.test(baseTitle)) {
      return baseTitle.replace(capRegex, v);
    }
    if (!baseTitle.toLowerCase().includes(v.toLowerCase())) {
      return baseTitle + ' ' + v;
    }
    return baseTitle;
  }

  function buildProductGallery(prod) {
    const list = [];
    const pushImg = (u) => {
      if (u && typeof u === 'string' && u.trim() && !list.includes(u.trim())) {
        list.push(u.trim());
      }
    };

    // 1. Lấy tất cả ảnh góc độ từ album
    if (Array.isArray(prod.album) && prod.album.length > 0) {
      prod.album.forEach(item => {
        const u = typeof item === 'string' ? item : item.img;
        pushImg(u);
      });
    }

    // 2. Lấy thêm ảnh các màu sắc nếu chưa có trong album
    if (Array.isArray(prod.mauSac)) {
      prod.mauSac.forEach(c => {
        pushImg(c.anh);
      });
    }

    // 3. Fallback ảnh chính nếu list vẫn rỗng
    if (list.length === 0 && prod.anhChinh) {
      pushImg(prod.anhChinh);
    }

    return list;
  }

  function renderProductDetail(prod) {
    selectedVersion = prod.phienBan[0];
    selectedColor = prod.mauSac[0];

    document.title = `${prod.tenHang} - SAMAMOBILE.COM`;

    if (window.addRecentlyViewed) {
      window.addRecentlyViewed(prod);
    }

    // 1. Breadcrumbs
    const bcCat = document.getElementById('bc-category');
    const bcProd = document.getElementById('bc-product-name');
    if (bcCat) bcCat.textContent = prod.tenNhom || prod.tenLoai || 'Điện thoại';
    if (bcProd) bcProd.textContent = prod.tenHang;

    // 2. Header Tên, SKU & Rating
    const titleEl = document.getElementById('product-title');
    const skuEl = document.getElementById('product-sku');
    if (titleEl) titleEl.textContent = prod.tenHang;
    if (skuEl) skuEl.textContent = prod.sku;

    // 3. Carousel ảnh
    galleryImages = buildProductGallery(prod);
    currentGalleryIndex = 0;
    updateGalleryView();

    // 4. Thumbnails tính năng
    renderGalleryThumbnails(prod);

    // 5. Thông số nổi bật 4 ô có icon
    renderKeySpecsGrid(prod.thongSoNoiBat, prod.category);

    // 6. Bộ chọn Phiên bản & Màu sắc
    renderVersionSelector(prod);
    renderColorSelector(prod);

    // 7. Cập nhật Giá bán & Trả góp
    updatePriceDisplay();

    // 8. Địa chỉ còn hàng (114 cửa hàng)
    renderStoreLocations('all', '');

    // 9. Bảng thông số chi tiết (Screenshot 3)
    renderDetailedSpecsTable(prod.thongSoChiTiet);

    // 10. Bài viết đánh giá chuyên sâu & Mục lục
    renderReviewArticle(prod);

    // 11. Ảnh đánh giá từ khách hàng & Bình luận
    renderCustomerPhotos(prod);
    renderCommentsThread();

    // 12. Phụ kiện mua kèm & Tương tự
    renderAddons('all');
    renderSimilarProducts();

    // 13. Sticky bottom bar
    updateStickyBar();

    // 14. Đồng bộ đáy 2 bên: Đáy cột phải = Đáy cột trái chuẩn xác 100%
    syncStoreCardHeight();
    setTimeout(syncStoreCardHeight, 80);
    setTimeout(syncStoreCardHeight, 250);
    setTimeout(syncStoreCardHeight, 600);
  }

  // Gallery Navigation
  window.prevGalleryImage = function () {
    if (galleryImages.length === 0) return;
    currentGalleryIndex = (currentGalleryIndex - 1 + galleryImages.length) % galleryImages.length;
    updateGalleryView();
  };

  window.nextGalleryImage = function () {
    if (galleryImages.length === 0) return;
    currentGalleryIndex = (currentGalleryIndex + 1) % galleryImages.length;
    updateGalleryView();
  };

  function updateGalleryView() {
    const mainImg = document.getElementById('main-product-image');
    const badge = document.getElementById('gallery-counter-badge');
    if (mainImg && galleryImages[currentGalleryIndex]) {
      mainImg.src = galleryImages[currentGalleryIndex];
    }
    if (badge) {
      badge.textContent = `${currentGalleryIndex + 1}/${galleryImages.length}`;
    }

    // Cập nhật viền active của Thumbnail Tab
    const tabItems = document.querySelectorAll('.thumb-tab-item');
    currentThumbnailTabs.forEach((tab, i) => {
      const el = tabItems[i];
      if (!el) return;
      if (tab.type === 'img') {
        const isAct = tab.targetIndex === currentGalleryIndex;
        if (isAct) {
          el.classList.add('active');
          el.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'nearest' });
        } else {
          el.classList.remove('active');
        }
      } else {
        el.classList.remove('active');
      }
    });
  }

  // Thumbnails dưới ảnh lớn (Video, Các góc độ trong album, Màu sắc, Thông số kỹ thuật, Bài viết, QR)
  // Thumbnails dưới ảnh lớn (Video, Màu sắc theo JSON, Các góc độ trong album, Thông số kỹ thuật, Bài viết, QR)
  var currentThumbnailTabs = [];
  function renderGalleryThumbnails(prod) {
    const container = document.getElementById('gallery-thumbnails');
    if (!container) return;

    currentThumbnailTabs = [
      { id: 't_video', label: 'Video', icon: 'fa-solid fa-circle-play', type: 'video', action: 'video' }
    ];

    const hasMultipleColors = Array.isArray(prod.mauSac) && prod.mauSac.length > 1;

    // 1. Đối với sản phẩm có nhiều màu (Điện thoại, Tablet, v.v.):
    // ƯU TIÊN hiển thị thumbnail tương ứng từng MÀU SẮC trong JSON (chuẩn Hoàng Hà Mobile)
    if (hasMultipleColors) {
      prod.mauSac.forEach(c => {
        const imgUrl = resolveAssetPath(c.anh || '');
        if (!imgUrl) return;
        let targetIdx = galleryImages.indexOf(imgUrl);
        if (targetIdx === -1) {
          galleryImages.push(imgUrl);
          targetIdx = galleryImages.length - 1;
        }
        currentThumbnailTabs.push({
          id: 't_color_' + c.id,
          label: c.ten,
          img: imgUrl,
          colorId: c.id,
          type: 'img',
          targetIndex: targetIdx
        });
      });
    }

    // 2. Thêm các góc chụp / phụ kiện kèm theo từ album (nếu có ý nghĩa và chưa bị trùng màu)
    if (Array.isArray(prod.album) && prod.album.length > 0) {
      prod.album.forEach((ang, idx) => {
        const imgUrl = resolveAssetPath(typeof ang === 'string' ? ang : ang.img);
        if (!imgUrl) return;
        const rawLbl = (typeof ang === 'string' ? ('Góc ' + (idx + 1)) : (ang.label || ('Góc ' + (idx + 1)))).trim();

        // Bỏ qua các nhãn mã số kỹ thuật rác (như "Góc nhìn 15036955")
        if (/^Góc nhìn \d+/i.test(rawLbl)) return;

        // Bỏ qua nếu ảnh này hoặc nhãn này đã được thêm trong danh sách màu sắc ở trên
        if (currentThumbnailTabs.some(t => t.img === imgUrl || (t.label && t.label.toLowerCase() === rawLbl.toLowerCase()))) return;

        let targetIdx = galleryImages.indexOf(imgUrl);
        if (targetIdx === -1) {
          galleryImages.push(imgUrl);
          targetIdx = galleryImages.length - 1;
        }
        currentThumbnailTabs.push({
          id: 't_angle_' + idx,
          label: rawLbl,
          img: imgUrl,
          type: 'img',
          targetIndex: targetIdx
        });
      });
    }

    // 3. Fallback nếu chỉ có 1 màu và chưa có ảnh nào trong thumbnail
    if (currentThumbnailTabs.length === 1) {
      const fallbackMain = resolveAssetPath(prod.anhChinh) || (galleryImages.length > 0 ? galleryImages[0] : '');
      if (fallbackMain) {
        let targetIdx = galleryImages.indexOf(fallbackMain);
        if (targetIdx === -1) {
          galleryImages.unshift(fallbackMain);
          targetIdx = 0;
        }
        const lbl = (prod.mauSac && prod.mauSac[0] && prod.mauSac[0].ten !== 'Tiêu chuẩn') ? prod.mauSac[0].ten : 'Mặt trước';
        currentThumbnailTabs.push({
          id: 't_main_0',
          label: lbl,
          img: fallbackMain,
          type: 'img',
          targetIndex: targetIdx
        });
      }
    }

    // 4. Các nút tiện ích cuối hàng chuẩn Hoàng Hà Mobile (Screenshot 1 & 2)
    currentThumbnailTabs.push(
      { id: 't_specs', label: 'Thông số kỹ thuật', icon: 'fa-solid fa-gear', type: 'specs', action: 'specs' },
      { id: 't_info', label: 'Thông tin sản phẩm', icon: 'fa-solid fa-file-lines', type: 'info', action: 'info' },
      { id: 't_qr', label: 'Xem trên điện thoại', icon: 'fa-solid fa-qrcode', type: 'qr', action: 'qr' }
    );

    const fallbackImg = resolveAssetPath(prod.anhChinh) || 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-trang-1.png';

    container.innerHTML = currentThumbnailTabs.map((tab, idx) => {
      const isAct = tab.type === 'img' && tab.targetIndex === currentGalleryIndex;
      let visual = '';
      if (tab.type === 'video') {
        visual = `<i class="fa-solid fa-circle-play text-2xl sm:text-[26px] text-[#0057B7] group-hover:scale-110 transition-transform"></i>`;
      } else if (tab.type === 'specs') {
        visual = `<i class="fa-solid fa-gear text-2xl text-slate-700 group-hover:text-blue-600 group-hover:rotate-45 transition-all"></i>`;
      } else if (tab.type === 'info') {
        visual = `<i class="fa-solid fa-file-lines text-2xl text-slate-700 group-hover:text-blue-600 transition-colors"></i>`;
      } else if (tab.type === 'qr') {
        visual = `<i class="fa-solid fa-qrcode text-2xl text-slate-700 group-hover:text-blue-600 transition-colors"></i>`;
      } else {
        visual = `<img src="${tab.img}" alt="${tab.label}" class="w-10 h-10 sm:w-11 sm:h-11 object-contain group-hover:scale-105 transition-transform" onerror="this.onerror=null; this.src='${fallbackImg}';" />`;
      }

      return `
        <button type="button" onclick="window.selectThumbnailTab(${idx})" class="thumb-tab-item group flex flex-col items-center flex-shrink-0 cursor-pointer focus:outline-none text-center ${isAct ? 'active' : ''}" title="${tab.label}">
          <div class="thumb-box w-16 h-16 sm:w-[68px] sm:h-[68px] rounded-2xl flex items-center justify-center p-1.5 transition-all border border-slate-200 bg-white group-hover:border-slate-300 group-hover:bg-slate-50/70">
            ${visual}
          </div>
          <span class="thumb-label mt-1.5 text-[10.5px] leading-tight text-center w-full max-w-[76px] sm:max-w-[82px] line-clamp-2 transition-colors font-medium text-slate-600 group-hover:text-slate-900 break-words">
            ${tab.label}
          </span>
        </button>
      `;
    }).join('');
  }

  window.selectThumbnailTab = function (idx) {
    const tab = currentThumbnailTabs[idx];
    if (!tab) return;

    if (tab.action === 'specs') {
      const specsSec = document.getElementById('detailed-specs-section');
      if (specsSec) {
        specsSec.scrollIntoView({ behavior: 'smooth' });
        specsSec.classList.add('ring-2', 'ring-blue-400');
        setTimeout(() => specsSec.classList.remove('ring-2', 'ring-blue-400'), 1500);
      }
    } else if (tab.action === 'info') {
      const infoSec = document.getElementById('review-article-section');
      if (infoSec) {
        infoSec.scrollIntoView({ behavior: 'smooth' });
      }
    } else if (tab.action === 'video') {
      window.openVideoModal();
    } else if (tab.action === 'qr') {
      window.openQrModal();
    } else {
      currentGalleryIndex = tab.targetIndex !== undefined ? tab.targetIndex : 0;
      updateGalleryView();
      if (tab.colorId && window.selectColor) {
        window.selectColor(tab.colorId);
      }
    }
  };

  // Video & QR Modals (Chuẩn Hoàng Hà Mobile)
  window.openVideoModal = function () {
    const modal = document.getElementById('video-review-modal');
    const iframe = document.getElementById('video-modal-iframe');
    const title = document.getElementById('video-modal-title');
    if (!modal) return;
    if (title && currentProduct) {
      title.textContent = `Video Trên Tay & Đánh Giá: ${currentProduct.tenHang}`;
    }
    if (iframe) {
      if (currentProduct && (currentProduct.video || currentProduct.videoUrl)) {
        let vUrl = currentProduct.video || currentProduct.videoUrl;
        if (!vUrl.includes('autoplay=1')) {
          vUrl += (vUrl.includes('?') ? '&' : '?') + 'autoplay=1';
        }
        iframe.src = vUrl;
      } else {
        const query = encodeURIComponent((currentProduct ? currentProduct.tenHang : '') + ' review trên tay');
        iframe.src = `https://www.youtube.com/embed?listType=search&list=${query}&autoplay=1`;
      }
    }
    modal.classList.remove('hidden');
  };

  window.closeVideoModal = function () {
    const modal = document.getElementById('video-review-modal');
    const iframe = document.getElementById('video-modal-iframe');
    if (modal) modal.classList.add('hidden');
    if (iframe) iframe.src = '';
  };

  window.openQrModal = function () {
    const modal = document.getElementById('qr-code-modal');
    const img = document.getElementById('qr-modal-image');
    const nameEl = document.getElementById('qr-modal-product-name');
    if (!modal) return;
    if (nameEl && currentProduct) {
      nameEl.textContent = currentProduct.tenHang;
    }
    if (img) {
      const curUrl = window.location.href;
      img.src = `https://api.qrserver.com/v1/create-qr-code/?size=220x220&data=${encodeURIComponent(curUrl)}`;
    }
    modal.classList.remove('hidden');
  };

  window.closeQrModal = function () {
    const modal = document.getElementById('qr-code-modal');
    if (modal) modal.classList.add('hidden');
  };

  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
      window.closeVideoModal();
      window.closeQrModal();
    }
  });

  document.addEventListener('click', function (e) {
    const videoModal = document.getElementById('video-review-modal');
    if (videoModal && !videoModal.classList.contains('hidden') && e.target === videoModal) {
      window.closeVideoModal();
    }
    const qrModal = document.getElementById('qr-code-modal');
    if (qrModal && !qrModal.classList.contains('hidden') && e.target === qrModal) {
      window.closeQrModal();
    }
  });

  // Thông số nổi bật 4 ô có icon chuẩn Screenshot 1
  function renderKeySpecsGrid(specs, category) {
    const grid = document.getElementById('key-specs-grid');
    if (!grid) return;

    const entries = Object.entries(specs || {});
    const icons = {
      'Kích thước màn hình': 'fa-solid fa-mobile-screen',
      'Màn hình': 'fa-solid fa-laptop',
      'Bộ nhớ trong': 'fa-solid fa-hard-drive',
      'Vi xử lý': 'fa-solid fa-microchip',
      'Dung lượng pin hiển thị': 'fa-solid fa-battery-full',
      'RAM & Ổ cứng': 'fa-solid fa-memory',
      'Trọng lượng pin': 'fa-solid fa-weight-scale',
      'Thời lượng pin': 'fa-solid fa-battery-three-quarters',
      'Chống ồn': 'fa-solid fa-shield-halved',
      'Kháng nước': 'fa-solid fa-droplet',
      'Kết nối': 'fa-solid fa-bluetooth-b',
      'Kích thước mặt': 'fa-solid fa-stopwatch',
      'Tương thích': 'fa-solid fa-mobile-screen',
      'Công suất': 'fa-solid fa-bolt',
      'Cổng ra': 'fa-solid fa-plug',
      'Công nghệ': 'fa-solid fa-shield-halved',
      'Bảo hành': 'fa-solid fa-certificate'
    };

    grid.innerHTML = entries.slice(0, 4).map(([key, val]) => {
      const icon = icons[key] || 'fa-solid fa-sliders';
      return `
        <div class="p-3 rounded-xl border border-slate-200/90 bg-slate-50/70 flex flex-col justify-between min-h-[76px] hover:border-blue-300 transition-colors">
          <div class="text-[11px] text-slate-500 font-medium">${key}</div>
          <div class="flex items-center gap-2 mt-1">
            <i class="${icon} text-slate-700 text-sm flex-shrink-0"></i>
            <span class="font-bold text-[#0F172A] text-xs sm:text-sm tracking-tight truncate">${val}</span>
          </div>
        </div>
      `;
    }).join('');
  }

  // Bộ chọn Phiên bản có checkmark xanh ở góc dưới phải (Chuẩn Hoàng Hà Mobile Screenshot)
  function renderVersionSelector(prod) {
    const container = document.getElementById('product-versions-list');
    if (!container) return;

    const cols = prod.phienBan && prod.phienBan.length === 2 ? 'grid-cols-2' : 'grid-cols-3';
    container.className = `grid ${cols} gap-2.5`;

    container.innerHTML = prod.phienBan.map(v => {
      const isSelected = selectedVersion && selectedVersion.id === v.id;
      return `
        <div class="variant-card relative p-2 sm:p-2.5 rounded-xl border-2 cursor-pointer transition-all flex flex-col justify-center items-center text-center overflow-hidden min-w-0 ${isSelected ? 'selected' : 'border-slate-200 hover:border-slate-300 bg-white'}" onclick="window.selectVersion('${v.id}')">
          <div class="font-bold variant-name-text text-slate-900 leading-snug whitespace-nowrap">${v.dungLuong}</div>
          <div class="variant-price-text text-xs font-semibold text-rose-600 mt-1 tabular-nums whitespace-nowrap">${formatCurrency(v.gia)}</div>
          
          <div class="check-badge ${isSelected ? 'flex' : 'hidden'} absolute bottom-0 right-0 w-3.5 h-3.5 sm:w-4 sm:h-4 bg-[#008774] text-white rounded-tl-md items-center justify-center text-[8px] sm:text-[9px]">
            <i class="fa-solid fa-check"></i>
          </div>
        </div>
      `;
    }).join('');
  }

  window.selectVersion = function (vId) {
    if (!currentProduct) return;
    const v = currentProduct.phienBan.find(item => item.id === vId);
    if (v) {
      selectedVersion = v;
      renderVersionSelector(currentProduct);
      updatePriceDisplay();
      updateStickyBar();
    }
  };

  // Bộ chọn Màu sắc có checkmark xanh ở góc dưới phải (Chuẩn Hoàng Hà Mobile Screenshot)
  function renderColorSelector(prod) {
    const container = document.getElementById('product-colors-list');
    if (!container) return;

    const cols = prod.mauSac && prod.mauSac.length === 2 ? 'grid-cols-2' : 'grid-cols-3';
    container.className = `grid ${cols} gap-2.5`;

    container.innerHTML = prod.mauSac.map(c => {
      const isSelected = selectedColor && selectedColor.id === c.id;
      return `
        <div class="color-card relative p-2 rounded-xl border-2 cursor-pointer transition-all flex items-center gap-2 overflow-hidden min-w-0 ${isSelected ? 'selected' : 'border-slate-200 hover:border-slate-300 bg-white'}" onclick="window.selectColor('${c.id}')" title="${c.ten}">
          <img src="${c.anh}" alt="${c.ten}" class="w-6 h-6 sm:w-7 sm:h-7 object-contain rounded flex-shrink-0" onerror="this.src='https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png';" />
          <div class="font-semibold color-name-text text-slate-900 whitespace-nowrap truncate min-w-0 flex-1 text-left">${c.ten}</div>
          
          <div class="check-badge ${isSelected ? 'flex' : 'hidden'} absolute bottom-0 right-0 w-3.5 h-3.5 sm:w-4 sm:h-4 bg-[#008774] text-white rounded-tl-md items-center justify-center text-[8px] sm:text-[9px]">
            <i class="fa-solid fa-check"></i>
          </div>
        </div>
      `;
    }).join('');
  }

  window.selectColor = function (cId) {
    if (!currentProduct) return;
    const c = currentProduct.mauSac.find(item => item.id === cId);
    if (c) {
      selectedColor = c;
      renderColorSelector(currentProduct);
      const imgIdx = galleryImages.indexOf(c.anh);
      if (imgIdx !== -1) {
        currentGalleryIndex = imgIdx;
      }
      updateGalleryView();
      updateStickyBar();
    }
  };

  // Cập nhật giá bán to rõ & tính toán các khối khuyến mãi
  function updatePriceDisplay() {
    const priceEl = document.getElementById('product-sale-price');
    const oldPriceEl = document.getElementById('product-original-price');
    const discountEl = document.getElementById('product-discount-badge');
    const installTextEl = document.getElementById('product-installment-text');
    const ctaInstallSub = document.getElementById('cta-installment-subtext');
    const memberDiscEl = document.getElementById('member-discount-val');
    const memberEduEl = document.getElementById('member-edu-val');
    const tradeInEstEl = document.getElementById('trade-in-estimate-price');

    const curPrice = selectedVersion ? selectedVersion.gia : currentProduct.donGia;
    const curOldPrice = selectedVersion ? selectedVersion.giaGoc : currentProduct.giaGoc;

    if (priceEl) priceEl.textContent = formatCurrency(curPrice);
    if (oldPriceEl) oldPriceEl.textContent = formatCurrency(curOldPrice);

    if (discountEl && curOldPrice > curPrice) {
      const pct = Math.round(((curOldPrice - curPrice) / curOldPrice) * 100);
      discountEl.textContent = `-${pct}%`;
      discountEl.classList.remove('hidden');
    }

    // Trả góp: chia ~8.5 tháng
    const monthlyInstall = Math.round(curPrice / 8.57);
    if (installTextEl) {
      installTextEl.innerHTML = `Hoặc Trả góp 0% chỉ từ <strong class="text-[#0057B7] tabular-nums font-bold whitespace-nowrap">${formatCurrency(monthlyInstall)}</strong>`;
    }
    if (ctaInstallSub) {
      ctaInstallSub.textContent = `Trả góp 0% chỉ từ ${formatCurrency(monthlyInstall)}`;
    }

    // Sama Member: 1.2% tối đa 366.000₫
    const memDisc = Math.min(366000, Math.round(curPrice * 0.012));
    if (memberDiscEl) memberDiscEl.textContent = formatCurrency(memDisc);

    // Sama EDU: 0.8% tối đa 200.000₫
    const eduDisc = Math.min(200000, Math.round(curPrice * 0.007));
    if (memberEduEl) memberEduEl.textContent = formatCurrency(eduDisc);

    // Thu cũ lên đời: ~85% giá máy
    const tradeInEst = Math.round(curPrice * 0.856);
    if (tradeInEstEl) tradeInEstEl.textContent = formatCurrency(tradeInEst);
  }

  // Render Địa chỉ còn hàng và trải nghiệm (Screenshot 2)
  function renderStoreLocations(city, query) {
    const container = document.getElementById('store-list-container');
    const countEl = document.getElementById('store-count-number');
    if (!container) return;

    let filtered = STORE_LOCATIONS;
    if (city && city !== 'all') {
      filtered = filtered.filter(s => s.city === city);
    }
    if (query && query.trim() !== '') {
      const q = query.toLowerCase().trim();
      filtered = filtered.filter(s => s.name.toLowerCase().includes(q) || s.phone.includes(q));
    }

    if (countEl) countEl.textContent = filtered.length;

    if (filtered.length === 0) {
      container.innerHTML = `
        <div class="p-6 text-center text-slate-500 text-xs">
          Không tìm thấy chi nhánh nào phù hợp với từ khóa "${query}".
        </div>
      `;
      return;
    }

    container.innerHTML = filtered.map(store => `
      <div class="p-3 sm:p-3.5 rounded-xl border border-slate-200/90 bg-white hover:border-blue-400 transition-colors flex flex-col sm:flex-row sm:items-center justify-between gap-2.5">
        <div class="space-y-1.5 flex-1 pr-2">
          <div class="font-semibold text-slate-900 text-xs sm:text-sm leading-snug">${store.name}</div>
          <div class="flex items-center gap-2 pt-0.5">
            <a href="tel:${store.phone}" class="px-2.5 py-1 rounded-md bg-[#008774] hover:bg-teal-700 text-white font-bold text-[11px] inline-flex items-center gap-1 transition-colors">
              <i class="fa-solid fa-phone text-[9px]"></i> ${store.phone}
            </a>
            <button type="button" onclick="alert('Địa chỉ: ${store.name}. Giờ mở cửa: 08:00 - 21:30 hàng ngày.')" class="px-2.5 py-1 rounded-md bg-slate-800 hover:bg-slate-900 text-white font-bold text-[11px] transition-colors cursor-pointer">
              Xem chi tiết
            </button>
          </div>
        </div>

        <div class="flex items-center gap-1.5 text-[#008774] text-xs font-semibold self-start sm:self-center whitespace-nowrap">
          <i class="fa-solid fa-circle text-[8px]"></i> Đang sẵn hàng
        </div>
      </div>
    `).join('');

    syncStoreCardHeight();
  }

  window.filterStoreLocations = function () {
    const city = document.getElementById('store-filter-city').value;
    const query = document.getElementById('store-filter-search').value;
    renderStoreLocations(city, query);
    syncStoreCardHeight();
  };

  // Đồng bộ chiều cao khối "Địa chỉ còn hàng" (trái) khớp 100% với đáy của cột phải
  function syncStoreCardHeight() {
    const rightCol = document.getElementById('product-detail-right-col');
    const leftCard = document.getElementById('store-locations-card');
    const storeList = document.getElementById('store-list-container');
    if (!rightCol || !leftCard || !storeList) return;

    if (window.innerWidth >= 1024) {
      // Lấy chiều cao thực tế của toàn bộ cột phải (từ card 1 đến hết card 4 Sản phẩm mua kèm)
      const rightH = rightCol.offsetHeight;
      if (rightH > 150) {
        // Gán chiều cao thẻ bên trái bằng chuẩn xác chiều cao cột phải
        leftCard.style.height = rightH + 'px';
        leftCard.style.maxHeight = rightH + 'px';

        const headerEl = leftCard.querySelector('.store-card-header');
        const filterEl = leftCard.querySelector('.store-card-filters');
        const hH = headerEl ? headerEl.offsetHeight : 52;
        const fH = filterEl ? filterEl.offsetHeight : 56;
        const pad = window.innerWidth >= 640 ? 48 : 40;
        const margin = 32;

        const availableH = rightH - hH - fH - pad - margin;
        const finalH = Math.max(260, availableH);
        storeList.style.height = finalH + 'px';
        storeList.style.maxHeight = finalH + 'px';
      }
    } else {
      leftCard.style.height = 'auto';
      leftCard.style.maxHeight = 'none';
      storeList.style.height = '460px';
      storeList.style.maxHeight = '460px';
    }
  }

  // Lắng nghe thay đổi kích thước & ResizeObserver để đáy 2 bên luôn thẳng hàng tuyệt đối
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', syncStoreCardHeight);
    window.addEventListener('load', syncStoreCardHeight);

    if (window.ResizeObserver) {
      setTimeout(function () {
        const rightCol = document.getElementById('product-detail-right-col');
        if (rightCol) {
          const ro = new ResizeObserver(function () {
            syncStoreCardHeight();
          });
          ro.observe(rightCol);
        }
      }, 100);
    }
  }

  // Render Bảng Thông Số Chi Tiết Chuẩn 2 Cột Zebra Table (Screenshot 3)
  function renderDetailedSpecsTable(specs) {
    const tbody = document.getElementById('detailed-specs-tbody');
    if (!tbody) return;

    const entries = Object.entries(specs || {});
    if (entries.length === 0) {
      tbody.innerHTML = `<tr><td colspan="2" class="p-4 text-center text-slate-500 text-xs">Đang cập nhật thông số chi tiết...</td></tr>`;
      return;
    }

    tbody.innerHTML = entries.map(([key, val]) => `
      <tr>
        <td class="align-top font-semibold text-slate-700 text-xs sm:text-[13px] bg-slate-50/70 w-[35%] p-3 border-b border-slate-200">${key}</td>
        <td class="align-top text-slate-800 text-xs sm:text-[13px] p-3 border-b border-slate-200 leading-relaxed">${val}</td>
      </tr>
    `).join('');
  }

  // Render Bài Viết Đánh Giá Chuyên Sâu có Mục Lục (Screenshot 4)
  function renderReviewArticle(prod) {
    const articleEl = document.getElementById('review-article-content');
    const headEl = document.getElementById('article-main-heading');
    if (!articleEl) return;

    const title = prod.tenHang;
    if (headEl) headEl.textContent = `Đặc điểm nổi bật của ${title}: Đỉnh cao công nghệ`;

    const img1 = (galleryImages && galleryImages[0]) ? galleryImages[0] : prod.anhChinh;
    const img2 = (galleryImages && galleryImages[1]) ? galleryImages[1] : img1;

    articleEl.innerHTML = `
      <div id="toc-specs" class="space-y-3">
        <h3 class="text-base sm:text-lg font-bold text-slate-900">
          1. Thông số kỹ thuật chi tiết của ${title} có gì nổi bật?
        </h3>
        <p>
          <strong>${title}</strong> đánh dấu bước tiến vượt bậc về công nghệ, kết hợp hoàn hảo giữa vật liệu cao cấp và cấu hình phần cứng đỉnh cao. Màn hình sắc nét với tần số quét 120Hz mang đến trải nghiệm thị giác mượt mà trong từng thao tác vuốt chạm. Cụm vi xử lý thế hệ mới tối ưu hóa hiệu suất AI, xử lý mượt mà mọi tác vụ đồ họa nặng nề và làm việc đa nhiệm.
        </p>
      </div>

      <div class="rounded-2xl overflow-hidden border border-slate-200 my-4 bg-white text-center">
        <div class="flex items-center justify-center p-6 bg-slate-50/60">
          <img src="${img1}" alt="${title}" class="max-h-[340px] w-auto object-contain" />
        </div>
        <div class="p-2.5 text-center text-xs text-slate-500 bg-slate-50 border-t border-slate-200 font-medium">
          Thiết kế sang trọng và hiện đại trên ${title} chính hãng
        </div>
      </div>

      <div id="toc-release" class="space-y-3">
        <h3 class="text-base sm:text-lg font-bold text-slate-900">
          2. ${title} ra mắt khi nào và phân khúc hướng tới?
        </h3>
        <p>
          Được công bố chính thức tại sự kiện công nghệ toàn cầu, <strong>${title}</strong> được định vị ở phân khúc cao cấp hướng đến người dùng đòi hỏi khắt khe về độ tin cậy, tốc độ xử lý và thiết kế tinh xảo. Sản phẩm nhanh chóng thu hút sự chú ý từ cộng đồng yêu công nghệ trên toàn thế giới.
        </p>
      </div>

      <div id="toc-price" class="space-y-3">
        <h3 class="text-base sm:text-lg font-bold text-slate-900">
          3. ${title} có giá bao nhiêu và chính sách bảo hành?
        </h3>
        <p>
          Tại hệ thống <strong>SAMAMOBILE.COM</strong>, <strong>${title}</strong> hiện đang được phân phối với mức giá ưu đãi cực tốt cùng chương trình trợ giá thu cũ lên đời đến 4.000.000₫. Khách hàng hoàn toàn yên tâm với chính sách bảo hành chính hãng 12 tháng, 1 đổi 1 trong 30 ngày đầu tiên nếu phát sinh lỗi từ nhà sản xuất.
        </p>
      </div>

      <div class="rounded-2xl overflow-hidden border border-slate-200 my-4 bg-white text-center">
        <div class="flex items-center justify-center p-6 bg-slate-50/60">
          <img src="${img2}" alt="${title}" class="max-h-[300px] w-auto object-contain" />
        </div>
        <div class="p-2.5 text-center text-xs text-slate-500 bg-slate-50 border-t border-slate-200 font-medium">
          Trải nghiệm cầm nắm chắc chắn và màu sắc cuốn hút của ${title}
        </div>
      </div>

      <div id="toc-design" class="space-y-3">
        <h3 class="text-base sm:text-lg font-bold text-slate-900">
          4. Thiết kế ngoại quan và trải nghiệm màn hình hiển thị
        </h3>
        <p>
          Khung viền được hoàn thiện tỉ mỉ mang lại cảm giác cầm nắm vô cùng chắc chắn và thoải mái. Tấm nền hiển thị độ sáng cao cho khả năng hiển thị ngoài trời rõ nét, độ tương phản sâu và màu sắc chân thực.
        </p>
      </div>

      <div id="toc-performance" class="space-y-3">
        <h3 class="text-base sm:text-lg font-bold text-slate-900">
          5. Hiệu năng xử lý và thời lượng sử dụng pin thực tế
        </h3>
        <p>
          Viên pin dung lượng lớn kết hợp với công nghệ quản lý năng lượng thông minh giúp thiết bị hoạt động bền bỉ suốt ngày dài mà không lo ngắt quãng. Cổng sạc nhanh giúp nạp đầy năng lượng chỉ trong thời gian ngắn, sẵn sàng đồng hành cùng bạn trong mọi chuyến đi.
        </p>
      </div>
    `;
  }

  // Toggle Thu Gọn / Xem Thêm bài viết có fade gradient
  window.toggleReviewArticle = function () {
    const wrapper = document.getElementById('review-article-wrapper');
    const overlay = document.getElementById('article-fade-overlay');
    const textEl = document.getElementById('btn-toggle-article-text');
    const iconEl = document.getElementById('btn-toggle-article-icon');

    if (!wrapper) return;
    isArticleExpanded = !isArticleExpanded;

    if (isArticleExpanded) {
      wrapper.classList.remove('collapsed');
      wrapper.classList.add('expanded');
      if (overlay) overlay.classList.add('hidden');
      if (textEl) textEl.textContent = 'Thu gọn';
      if (iconEl) iconEl.className = 'fa-solid fa-chevron-up text-xs';
    } else {
      wrapper.classList.remove('expanded');
      wrapper.classList.add('collapsed');
      if (overlay) overlay.classList.remove('hidden');
      if (textEl) textEl.textContent = 'Xem thêm';
      if (iconEl) iconEl.className = 'fa-solid fa-chevron-down text-xs';
      document.getElementById('review-article-section')?.scrollIntoView({ behavior: 'smooth' });
    }
  };

  // Render Ảnh đánh giá từ khách hàng (Screenshot 5)
  function renderCustomerPhotos(prod) {
    const container = document.getElementById('customer-photos-grid');
    if (!container) return;

    const sampleCustomerPhotos = [
      'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png',
      'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png',
      'https://i.hoanghamobile.vn/Uploads/2024/01/16/s24-ultra-vang_638409930027889246.png',
      'https://i.hoanghamobile.vn/Uploads/2026/01/08/nubia-v80-max-bac.png',
      'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png'
    ];

    container.innerHTML = sampleCustomerPhotos.map((src, i) => `
      <div class="w-18 h-18 sm:w-20 sm:h-20 rounded-xl border border-slate-200 overflow-hidden flex-shrink-0 bg-white p-1 hover:border-blue-400 transition-colors cursor-pointer shadow-2xs" onclick="alert('Xem ảnh đánh giá số ${i + 1} của khách hàng!')">
        <img src="${src}" alt="Customer photo ${i + 1}" class="w-full h-full object-contain" />
      </div>
    `).join('');
  }

  // Render Bình luận & Đánh giá (Screenshot 5)
  function renderCommentsThread() {
    const container = document.getElementById('comments-thread-container');
    if (!container) return;

    container.innerHTML = `
      <div class="p-4 bg-slate-50/80 rounded-2xl border border-slate-200/90 space-y-3">
        <div class="flex items-center justify-between">
          <div class="flex items-center gap-2.5">
            <div class="w-8 h-8 rounded-full bg-blue-100 text-[#0057B7] font-bold flex items-center justify-center text-xs">AK</div>
            <div>
              <div class="font-bold text-xs sm:text-sm text-slate-900 flex items-center gap-2">
                <span>Anh Khang</span>
                <span class="px-2 py-0.5 rounded-full bg-emerald-100 text-emerald-800 text-[10px] font-semibold">Đã mua tại SamaMobile</span>
              </div>
              <div class="text-[10px] text-slate-400">2 ngày trước</div>
            </div>
          </div>
          <div class="text-amber-400 text-xs flex gap-0.5">
            <i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i>
          </div>
        </div>
        <p class="text-xs sm:text-sm text-slate-700 leading-relaxed">
          Máy dùng cực kỳ thích, màn hình phẳng 6.9 inch vuốt chạm rất đã tay. Pin trâu on-screen hơn 8 tiếng liên tục, camera zoom sắc nét. Giao hàng tại Thủ Đức chỉ trong 45 phút, đóng gói cẩn thận!
        </p>
        
        <!-- Phản hồi QTV -->
        <div class="ml-4 sm:ml-6 p-3 bg-white rounded-xl border border-blue-100 space-y-1.5">
          <div class="flex items-center gap-2">
            <span class="px-1.5 py-0.5 bg-[#0057B7] text-white text-[9px] font-bold rounded uppercase">QTV</span>
            <span class="font-bold text-xs text-blue-900">SamaMobile Chăm Sóc Khách Hàng</span>
            <span class="text-[10px] text-slate-400">• 1 ngày trước</span>
          </div>
          <p class="text-xs text-slate-600">
            Dạ SamaMobile cảm ơn anh Khang rất nhiều đã tin tưởng lựa chọn sản phẩm! Chúc anh luôn có những trải nghiệm thật tuyệt vời cùng máy ạ. Khi cần hỗ trợ bất kỳ tính năng nào anh liên hệ tổng đài 1800 6868 nhé!
          </p>
        </div>
      </div>
    `;
  }

  // Setup Form gửi bình luận nhanh
  function setupCommentForm() {
    const btn = document.getElementById('btn-submit-comment');
    const input = document.getElementById('comment-input');
    const authorName = document.getElementById('comment-author-name');
    const authorPhone = document.getElementById('comment-author-phone');

    if (btn && input) {
      btn.addEventListener('click', function () {
        const val = input.value.trim();
        const nameVal = authorName ? authorName.value.trim() : 'Khách hàng';
        if (!val) {
          alert('Vui lòng nhập nội dung đánh giá của bạn.');
          input.focus();
          return;
        }

        const container = document.getElementById('comments-thread-container');
        if (container) {
          const newCommentHtml = `
            <div class="p-4 bg-blue-50/50 rounded-2xl border border-blue-200/90 space-y-2 animate-in fade-in duration-300">
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2.5">
                  <div class="w-8 h-8 rounded-full bg-blue-600 text-white font-bold flex items-center justify-center text-xs">
                    ${nameVal.charAt(0).toUpperCase()}
                  </div>
                  <div>
                    <div class="font-bold text-xs sm:text-sm text-slate-900">${nameVal}</div>
                    <div class="text-[10px] text-blue-600 font-semibold">Vừa xong (Đang xét duyệt)</div>
                  </div>
                </div>
                <div class="text-amber-400 text-xs flex gap-0.5">
                  <i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i>
                </div>
              </div>
              <p class="text-xs sm:text-sm text-slate-800 leading-relaxed font-medium">
                ${val}
              </p>
            </div>
          `;
          container.insertAdjacentHTML('afterbegin', newCommentHtml);
        }

        alert('Cảm ơn bạn đã gửi đánh giá! Bình luận sẽ được Quản trị viên xét duyệt và hiển thị công khai.');
        input.value = '';
        if (authorName) authorName.value = '';
        if (authorPhone) authorPhone.value = '';
      });
    }
  }

  // Render Sản phẩm mua kèm theo danh mục filter tabs (Screenshot 2)
  var ALL_ADDONS = [
    { id: 'ADD-01', cat: 'case', name: 'Ốp lưng từ tính MagSafe Chống va đập', price: 190000, img: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/ip16-xanh-mong-ket.png' },
    { id: 'ADD-02', cat: 'glass', name: 'Kính cường lực Full màn hình 9H', price: 150000, img: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png' },
    { id: 'ADD-03', cat: 'charger', name: 'Củ sạc nhanh 45W Type-C SamaSuperFast', price: 390000, img: 'https://i.hoanghamobile.vn/Uploads/2026/03/21/a1638-den.png' },
    { id: 'ADD-04', cat: 'battery', name: 'Pin sạc dự phòng 10.000mAh 30W Qi2', price: 490000, img: 'https://i.hoanghamobile.vn/Uploads/2026/03/21/a1638-den.png' }
  ];

  window.filterAddons = function (cat) {
    const tabs = document.querySelectorAll('.addon-tab');
    tabs.forEach(t => {
      t.className = 'addon-tab px-3 py-1 rounded-full bg-slate-100 text-slate-700 hover:bg-slate-200 flex-shrink-0 cursor-pointer';
    });
    if (event && event.target) {
      event.target.className = 'addon-tab px-3 py-1 rounded-full bg-slate-900 text-white flex-shrink-0 cursor-pointer';
    }
    renderAddons(cat);
  };

  function renderAddons(cat) {
    const container = document.getElementById('addons-container');
    if (!container) return;

    const list = cat === 'all' ? ALL_ADDONS : ALL_ADDONS.filter(it => it.cat === cat);

    container.innerHTML = list.map(item => `
      <div class="p-2.5 rounded-xl border border-slate-200/90 bg-white hover:border-blue-400 transition-colors flex items-center justify-between gap-3">
        <img src="${item.img}" alt="${item.name}" class="w-10 h-10 object-contain rounded-lg flex-shrink-0" onerror="this.src='../images/default-phone.png';" />
        <div class="min-w-0 flex-1">
          <div class="text-xs font-bold text-slate-800 line-clamp-1">${item.name}</div>
          <div class="text-xs font-semibold text-rose-600 tabular-nums mt-0.5">${formatCurrency(item.price)}</div>
        </div>
        <button type="button" onclick="alert('Đã thêm phụ kiện ${item.name} vào giỏ!')" class="px-2.5 py-1 rounded-lg text-xs font-bold text-blue-600 border border-blue-200 hover:bg-blue-50 flex-shrink-0 cursor-pointer">
          + Thêm
        </button>
      </div>
    `).join('');

    syncStoreCardHeight();
  }

  // Render Sản phẩm tương tự
  function renderSimilarProducts() {
    const grid = document.getElementById('similar-products-grid');
    if (!grid) return;

    const items = [
      { id: 'MH108', name: 'Samsung Galaxy A08 4GB/64GB', price: 4490000, oldPrice: 4890000, img: resolveAssetPath('images/products/phone-mh108.png') },
      { id: 'MH109', name: 'Samsung Galaxy A18 6GB/128GB', price: 6690000, oldPrice: 7190000, img: resolveAssetPath('images/products/phone-mh109.png') },
      { id: 'MH104', name: 'Nubia Air 5G 8GB/256GB', price: 5490000, oldPrice: 5990000, img: 'https://i.hoanghamobile.vn/Uploads/2026/07/07/back-gold1-171113527.png' },
      { id: 'MH112', name: 'Xiaomi Redmi Note 17 6GB/128GB', price: 4890000, oldPrice: 5290000, img: resolveAssetPath('images/products/phone-mh112.png') },
      { id: 'MH114', name: 'OPPO A68 5G 8GB/128GB', price: 5690000, oldPrice: 6190000, img: resolveAssetPath('images/products/phone-mh114.png') }
    ];

    grid.innerHTML = items.map(p => `
      <div class="bg-white rounded-2xl border border-slate-200/90 p-3 flex flex-col justify-between hover:border-blue-400 hover:shadow-md transition-all cursor-pointer group" onclick="window.location.href='product-detail.html?id=${p.id}'">
        <div class="relative w-full aspect-square flex items-center justify-center p-2 mb-2">
          <img src="${p.img}" alt="${p.name}" class="max-w-full max-h-full object-contain group-hover:scale-105 transition-transform" onerror="this.src='../images/default-phone.png';" />
          <span class="absolute top-1 left-1 px-1.5 py-0.5 rounded-md bg-rose-500 text-white font-bold text-[9px]">-8%</span>
        </div>
        <div>
          <h4 class="text-xs font-bold text-slate-800 line-clamp-2 leading-snug group-hover:text-blue-600 transition-colors">${p.name}</h4>
          <div class="text-xs sm:text-sm font-semibold text-rose-600 tabular-nums mt-1">${formatCurrency(p.price)}</div>
          <div class="text-[10px] text-slate-400 line-through">${formatCurrency(p.oldPrice)}</div>
        </div>
      </div>
    `).join('');
  }

  // Đồng bộ Sticky Bottom Bar (Screenshots 1, 2, 3, 4, 5)
  function updateStickyBar() {
    if (!currentProduct) return;
    const thumb = document.getElementById('sticky-bar-thumb');
    const title = document.getElementById('sticky-bar-title');
    const variant = document.getElementById('sticky-bar-variant');
    const price = document.getElementById('sticky-bar-price');

    const curPrice = selectedVersion ? selectedVersion.gia : currentProduct.donGia;
    const verText = selectedVersion ? selectedVersion.dungLuong : 'Tiêu chuẩn';
    const colText = selectedColor ? selectedColor.ten : 'Trắng';

    if (thumb) thumb.src = selectedColor ? selectedColor.anh : currentProduct.anhChinh;
    if (title) title.textContent = formatProductVariantTitle(currentProduct.tenHang, verText);
    if (variant) variant.textContent = `Phân loại: ${colText} - ${verText}`;
    if (price) price.textContent = formatCurrency(curPrice);
  }

  function setupStickyPurchaseBar() {
    const bar = document.getElementById('sticky-purchase-bar');
    if (!bar) return;

    window.addEventListener('scroll', function () {
      if (window.scrollY > 400) {
        bar.classList.remove('translate-y-full');
      } else {
        bar.classList.add('translate-y-full');
      }
    });
  }

  // Mua ngay & Thêm giỏ hàng
  window.handleMainBuyNow = function () {
    if (!currentProduct) return;
    window.handleAddToCart(true);
  };

  window.handleAddToCart = function (openCheckout) {
    if (!currentProduct) return;

    const vName = selectedVersion ? selectedVersion.dungLuong : 'Tiêu chuẩn';
    const cName = selectedColor ? selectedColor.ten : 'Trắng';
    const priceVal = Number(selectedVersion ? selectedVersion.gia : currentProduct.donGia) || 0;
    const oldPriceVal = Number(selectedVersion ? selectedVersion.giaGoc : (currentProduct.giaGoc || currentProduct.donGia)) || priceVal;
    const imgVal = selectedColor ? selectedColor.anh : currentProduct.anhChinh;

    const displayTitle = formatProductVariantTitle(currentProduct.tenHang, vName);
    const cartItem = {
      id: currentProduct.maHang + '_' + vName + '_' + cName,
      maHang: currentProduct.maHang,
      tenHang: displayTitle,
      name: displayTitle,
      donGia: priceVal,
      giaGoc: oldPriceVal,
      hinhAnh: imgVal,
      soLuong: 1,
      mauSac: cName,
      phienBan: vName,
      price: priceVal,
      oldPrice: oldPriceVal,
      image: imgVal,
      qty: 1,
      colorName: cName,
      variantName: vName
    };

    if (window.CellphoneSCart || window.Cart) {
      const cart = window.CellphoneSCart || window.Cart;
      cart.addItem(cartItem);
      if (openCheckout) {
        window.location.href = 'cart.html';
      } else {
        cart.openDrawer();
      }
    } else {
      try {
        const raw = localStorage.getItem('cellphones_cart');
        const list = raw ? JSON.parse(raw) : [];
        const existing = list.find(it => it.id === cartItem.id);
        if (existing) existing.qty = (existing.qty || 1) + 1;
        else list.push(cartItem);
        localStorage.setItem('cellphones_cart', JSON.stringify(list));
      } catch (e) {}

      if (openCheckout) {
        window.location.href = 'cart.html';
      } else {
        alert('Đã thêm "' + currentProduct.tenHang + '" vào giỏ hàng thành công!');
      }
    }
  };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
