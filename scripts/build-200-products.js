const fs = require('fs');
const path = require('path');

// 1. Load existing 89 scraped items
const existingPath = path.join(__dirname, '..', 'frontend', 'products-meta.json');
let existingMeta = {};
if (fs.existsSync(existingPath)) {
  try {
    existingMeta = JSON.parse(fs.readFileSync(existingPath, 'utf8'));
  } catch {}
}

const existingList = Object.values(existingMeta);
console.log(`Đang có ${existingList.length} sản phẩm cào gốc từ Hoàng Hà Mobile.`);

// Diverse catalog additions to complete 200 items
const catalogTemplates = [
  // --- APPLE IPHONE ---
  {
    ten: 'iPhone 16 Pro Max 256GB - Chính Hãng VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 34990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-desert-titan.png',
    mau: [
      { id: 'titan-sa-mac', ten: 'Titan Sa Mạc', hex: '#C5A98A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-desert-titan.png' },
      { id: 'titan-tu-nhien', ten: 'Titan Tự Nhiên', hex: '#9CA3AF', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-natural-titan.png' },
      { id: 'titan-trang', ten: 'Titan Trắng', hex: '#F3F4F6', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-white-titan.png' },
      { id: 'titan-den', ten: 'Titan Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-black-titan.png' }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '256GB', gia: 34990000, giaGoc: 36990000 },
      { id: '512gb', dungLuong: '512GB', gia: 40990000, giaGoc: 42990000 },
      { id: '1tb', dungLuong: '1TB', gia: 46990000, giaGoc: 49990000 }
    ],
    thongSo: { 'Màn hình': '6.9" Super Retina XDR OLED 120Hz', 'Chip': 'Apple A18 Pro 3nm', 'RAM / ROM': '8GB / 256GB', 'Pin': '4685 mAh', 'Camera': '48MP + 48MP + 12MP (5x Zoom)' }
  },
  {
    ten: 'iPhone 16 Pro 128GB - Chính Hãng VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 28490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-natural-titan.png',
    mau: [
      { id: 'titan-tu-nhien', ten: 'Titan Tự Nhiên', hex: '#9CA3AF', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-natural-titan.png' },
      { id: 'titan-sa-mac', ten: 'Titan Sa Mạc', hex: '#C5A98A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-desert-titan.png' },
      { id: 'titan-den', ten: 'Titan Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-black-titan.png' }
    ],
    phienBan: [
      { id: '128gb', dungLuong: '128GB', gia: 28490000, giaGoc: 29990000 },
      { id: '256gb', dungLuong: '256GB', gia: 31490000, giaGoc: 33490000 },
      { id: '512gb', dungLuong: '512GB', gia: 37490000, giaGoc: 39990000 }
    ],
    thongSo: { 'Màn hình': '6.3" ProMotion 120Hz', 'Chip': 'Apple A18 Pro', 'RAM / ROM': '8GB / 128GB', 'Pin': '3582 mAh' }
  },
  {
    ten: 'iPhone 16 128GB - Chính Hãng VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 21990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-ultramarine.png',
    mau: [
      { id: 'xanh-luu-ly', ten: 'Xanh Lưu Ly', hex: '#3B82F6', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-ultramarine.png' },
      { id: 'hong', ten: 'Hồng', hex: '#F472B6', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pink.png' },
      { id: 'xanh-ngoc', ten: 'Xanh Mòng Két', hex: '#10B981', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-teal.png' },
      { id: 'den', ten: 'Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-black.png' }
    ],
    phienBan: [
      { id: '128gb', dungLuong: '128GB', gia: 21990000, giaGoc: 22990000 },
      { id: '256gb', dungLuong: '256GB', gia: 24990000, giaGoc: 25990000 }
    ],
    thongSo: { 'Màn hình': '6.1" OLED Super Retina', 'Chip': 'Apple A18 3nm', 'RAM / ROM': '8GB / 128GB', 'Pin': '3561 mAh' }
  },
  {
    ten: 'iPhone 15 Pro Max 256GB - Chính Hãng',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 29490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pro-max-blue-titanium.png',
    mau: [
      { id: 'blue-titan', ten: 'Titan Xanh', hex: '#1E3A8A', anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pro-max-blue-titanium.png' },
      { id: 'natural-titan', ten: 'Titan Tự Nhiên', hex: '#9CA3AF', anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pro-max-natural-titanium.png' }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '256GB', gia: 29490000, giaGoc: 34990000 },
      { id: '512gb', dungLuong: '512GB', gia: 34990000, giaGoc: 40990000 }
    ],
    thongSo: { 'Màn hình': '6.7" ProMotion 120Hz', 'Chip': 'Apple A17 Pro', 'RAM / ROM': '8GB / 256GB', 'Pin': '4422 mAh' }
  },
  {
    ten: 'iPhone 15 128GB - Chính Hãng',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 18990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pink.png',
    mau: [
      { id: 'pink', ten: 'Hồng Nhạt', hex: '#FBCFE8', anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pink.png' },
      { id: 'black', ten: 'Đen Midnight', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-black.png' }
    ],
    phienBan: [
      { id: '128gb', dungLuong: '128GB', gia: 18990000, giaGoc: 22990000 }
    ],
    thongSo: { 'Màn hình': '6.1" OLED Dynamic Island', 'Chip': 'A16 Bionic', 'RAM / ROM': '6GB / 128GB', 'Pin': '3349 mAh' }
  },
  {
    ten: 'iPhone 13 128GB - Chính Hãng VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 13490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2021/09/15/iphone-13-blue.png',
    mau: [
      { id: 'blue', ten: 'Xanh Dương', hex: '#2563EB', anh: 'https://i.hoanghamobile.vn/Uploads/2021/09/15/iphone-13-blue.png' },
      { id: 'midnight', ten: 'Đen Midnight', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2021/09/15/iphone-13-midnight.png' }
    ],
    phienBan: [{ id: '128gb', dungLuong: '128GB', gia: 13490000, giaGoc: 16990000 }],
    thongSo: { 'Màn hình': '6.1" OLED Super Retina', 'Chip': 'A15 Bionic', 'RAM / ROM': '4GB / 128GB', 'Pin': '3240 mAh' }
  },

  // --- SAMSUNG GALAXY ---
  {
    ten: 'Samsung Galaxy S24 Ultra 12GB/256GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 27990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-xam.png',
    mau: [
      { id: 'titan-xam', ten: 'Titan Xám', hex: '#6B7280', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-xam.png' },
      { id: 'titan-den', ten: 'Titan Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-den.png' },
      { id: 'titan-vang', ten: 'Titan Vàng', hex: '#FDE047', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-vang.png' },
      { id: 'titan-tim', ten: 'Titan Tím', hex: '#A855F7', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-tim.png' }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '12GB/256GB', gia: 27990000, giaGoc: 33990000 },
      { id: '512gb', dungLuong: '12GB/512GB', gia: 31990000, giaGoc: 37490000 }
    ],
    thongSo: { 'Màn hình': '6.8" QHD+ Dynamic AMOLED 2X 120Hz', 'Chip': 'Snapdragon 8 Gen 3 for Galaxy', 'RAM / ROM': '12GB / 256GB', 'Pin': '5000 mAh', 'Bút S-Pen': 'Tích hợp sẵn' }
  },
  {
    ten: 'Samsung Galaxy Z Fold6 12GB/256GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 41990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-fold-6-xanh.png',
    mau: [
      { id: 'xanh-navy', ten: 'Xanh Navy', hex: '#1E3A8A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-fold-6-xanh.png' },
      { id: 'xam-metal', ten: 'Xám Metal', hex: '#9CA3AF', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-fold-6-xam.png' }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '12GB/256GB', gia: 41990000, giaGoc: 43990000 },
      { id: '512gb', dungLuong: '12GB/512GB', gia: 45990000, giaGoc: 47990000 }
    ],
    thongSo: { 'Màn hình chính': '7.6" Dynamic AMOLED 2X', 'Màn phụ': '6.3"', 'Chip': 'Snapdragon 8 Gen 3', 'RAM / ROM': '12GB / 256GB', 'Pin': '4400 mAh' }
  },
  {
    ten: 'Samsung Galaxy Z Flip6 12GB/256GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 25990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-flip-6-xanh.png',
    mau: [
      { id: 'xanh-mint', ten: 'Xanh Mint', hex: '#A7F3D0', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-flip-6-xanh.png' },
      { id: 'vang-solar', ten: 'Vàng Solar', hex: '#FEF08A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/z-flip-6-vang.png' }
    ],
    phienBan: [{ id: '256gb', dungLuong: '12GB/256GB', gia: 25990000, giaGoc: 28990000 }],
    thongSo: { 'Màn hình': '6.7" Dynamic AMOLED 2X', 'Màn phụ': '3.4" Super AMOLED', 'Chip': 'Snapdragon 8 Gen 3', 'Pin': '4000 mAh' }
  },
  {
    ten: 'Samsung Galaxy A55 5G 8GB/128GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 9490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a55-xanh-ice.png',
    mau: [
      { id: 'ice-blue', ten: 'Xanh Iceblue', hex: '#BAE6FD', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a55-xanh-ice.png' },
      { id: 'navy', ten: 'Xanh Navy', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a55-navy.png' }
    ],
    phienBan: [
      { id: '8-128', dungLuong: '8GB/128GB', gia: 9490000, giaGoc: 9990000 },
      { id: '8-256', dungLuong: '8GB/256GB', gia: 10490000, giaGoc: 11490000 }
    ],
    thongSo: { 'Màn hình': '6.6" Super AMOLED 120Hz', 'Chip': 'Exynos 1480 4nm', 'RAM / ROM': '8GB / 128GB', 'Pin': '5000 mAh 25W' }
  },
  {
    ten: 'Samsung Galaxy A35 5G 8GB/128GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 7690000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a35-vang.png',
    mau: [
      { id: 'vang-chanh', ten: 'Vàng Chanh', hex: '#FEF9C3', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a35-vang.png' },
      { id: 'xanh-ice', ten: 'Xanh Iceblue', hex: '#BAE6FD', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/11/a35-xanh.png' }
    ],
    phienBan: [{ id: '8-128', dungLuong: '8GB/128GB', gia: 7690000, giaGoc: 8290000 }],
    thongSo: { 'Màn hình': '6.6" Super AMOLED 120Hz', 'Chip': 'Exynos 1380', 'Pin': '5000 mAh' }
  },
  {
    ten: 'Samsung Galaxy A15 8GB/128GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 4690000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/16/a15-xanh-duong.png',
    mau: [
      { id: 'blue', ten: 'Xanh Dương', hex: '#60A5FA', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/16/a15-xanh-duong.png' },
      { id: 'black', ten: 'Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/16/a15-den.png' }
    ],
    phienBan: [{ id: '8-128', dungLuong: '8GB/128GB', gia: 4690000, giaGoc: 4990000 }],
    thongSo: { 'Màn hình': '6.5" Super AMOLED 90Hz', 'Chip': 'Helio G99', 'Pin': '5000 mAh' }
  },

  // --- XIAOMI / REDMI / POCO ---
  {
    ten: 'Xiaomi 14 Ultra 16GB/512GB Leica Camera',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 29990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/23/xiaomi-14-ultra-den.png',
    mau: [
      { id: 'den', ten: 'Đen Da Thuần Chay', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/23/xiaomi-14-ultra-den.png' },
      { id: 'trang', ten: 'Trắng Gốm', hex: '#F9FAFB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/23/xiaomi-14-ultra-trang.png' }
    ],
    phienBan: [{ id: '16-512', dungLuong: '16GB/512GB', gia: 29990000, giaGoc: 32990000 }],
    thongSo: { 'Màn hình': '6.73" WQHD+ AMOLED 120Hz 3000 nits', 'Chip': 'Snapdragon 8 Gen 3', 'Camera': '4 ống kính Leica 50MP cảm biến 1 inch', 'Pin': '5000 mAh 90W' }
  },
  {
    ten: 'Xiaomi 14T Pro 12GB/512GB',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 16990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/27/14t-pro-titan-xam.png',
    mau: [
      { id: 'titan-xam', ten: 'Titan Xám', hex: '#6B7280', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/27/14t-pro-titan-xam.png' },
      { id: 'titan-den', ten: 'Titan Đen', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2024/09/27/14t-pro-titan-den.png' }
    ],
    phienBan: [
      { id: '12-256', dungLuong: '12GB/256GB', gia: 15490000, giaGoc: 16990000 },
      { id: '12-512', dungLuong: '12GB/512GB', gia: 16990000, giaGoc: 17990000 }
    ],
    thongSo: { 'Màn hình': '6.67" AMOLED 144Hz', 'Chip': 'Dimensity 9300+ 4nm', 'Camera': 'Leica Summilux', 'Sạc': '120W HyperCharge' }
  },
  {
    ten: 'Xiaomi Redmi Note 13 Pro 8GB/128GB',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 6490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-pro-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Rừng Sâu', hex: '#059669', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-pro-xanh.png' },
      { id: 'den', ten: 'Đen Bán Dạ', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-pro-den.png' }
    ],
    phienBan: [
      { id: '8-128', dungLuong: '8GB/128GB', gia: 6490000, giaGoc: 7290000 },
      { id: '8-256', dungLuong: '8GB/256GB', gia: 7290000, giaGoc: 7990000 }
    ],
    thongSo: { 'Màn hình': '6.67" AMOLED 1.5K 120Hz', 'Camera': '200MP OIS', 'Chip': 'Helio G99-Ultra', 'Sạc': '67W' }
  },
  {
    ten: 'Xiaomi Redmi Note 13 6GB/128GB',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 4490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-vang.png',
    mau: [
      { id: 'vang', ten: 'Vàng Hoàng Hôn', hex: '#FCD34D', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-vang.png' },
      { id: 'den', ten: 'Đen', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-den.png' }
    ],
    phienBan: [{ id: '6-128', dungLuong: '6GB/128GB', gia: 4490000, giaGoc: 4890000 }],
    thongSo: { 'Màn hình': '6.67" AMOLED 120Hz', 'Camera': '108MP', 'Pin': '5000 mAh 33W' }
  },
  {
    ten: 'POCO X6 Pro 5G 12GB/512GB',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 8990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/12/poco-x6-pro-vang.png',
    mau: [
      { id: 'vang', ten: 'Vàng POCO', hex: '#EAB308', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/12/poco-x6-pro-vang.png' },
      { id: 'den', ten: 'Đen', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/12/poco-x6-pro-den.png' }
    ],
    phienBan: [{ id: '12-512', dungLuong: '12GB/512GB', gia: 8990000, giaGoc: 9990000 }],
    thongSo: { 'Màn hình': '6.67" Flow AMOLED 120Hz', 'Chip': 'Dimensity 8300-Ultra', 'Sạc': '67W Turbo' }
  },

  // --- OPPO ---
  {
    ten: 'OPPO Find N3 5G 16GB/512GB Màn Hình Gập',
    nhanHieu: 'OPPO', maLoai: 'OPPO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 41990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/10/26/find-n3-vang.png',
    mau: [
      { id: 'vang', ten: 'Vàng Cát', hex: '#FDE68A', anh: 'https://i.hoanghamobile.vn/Uploads/2023/10/26/find-n3-vang.png' },
      { id: 'den', ten: 'Đen Da Cổ Điển', hex: '#18181B', anh: 'https://i.hoanghamobile.vn/Uploads/2023/10/26/find-n3-den.png' }
    ],
    phienBan: [{ id: '16-512', dungLuong: '16GB/512GB', gia: 41990000, giaGoc: 44990000 }],
    thongSo: { 'Màn hình': '7.82" OLED 120Hz gập không nếp gấp', 'Camera': 'Hasselblad 48MP', 'Chip': 'Snapdragon 8 Gen 2' }
  },
  {
    ten: 'OPPO Reno12 Pro 5G 12GB/512GB AI Phone',
    nhanHieu: 'OPPO', maLoai: 'OPPO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 16990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-pro-nau.png',
    mau: [
      { id: 'nau', ten: 'Nâu Tinh Tế', hex: '#78350F', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-pro-nau.png' },
      { id: 'bac', ten: 'Bạc Vũ Trụ', hex: '#E5E7EB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-pro-bac.png' }
    ],
    phienBan: [{ id: '12-512', dungLuong: '12GB/512GB', gia: 16990000, giaGoc: 18990000 }],
    thongSo: { 'Màn hình': '6.7" OLED cong vô cực', 'Chip': 'Dimensity 7300-Energy', 'AI': 'OPPO AI Smart Phone' }
  },
  {
    ten: 'OPPO Reno12 5G 12GB/256GB',
    nhanHieu: 'OPPO', maLoai: 'OPPO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 11990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-hong.png',
    mau: [
      { id: 'hong', ten: 'Hồng Hoàng Hôn', hex: '#F472B6', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-hong.png' },
      { id: 'bac', ten: 'Bạc Gợn Sóng', hex: '#CBD5E1', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-bac.png' }
    ],
    phienBan: [{ id: '12-256', dungLuong: '12GB/256GB', gia: 11990000, giaGoc: 12990000 }],
    thongSo: { 'Màn hình': '6.7" OLED 120Hz', 'Sạc': 'SuperVOOC 80W', 'Pin': '5000 mAh' }
  },
  {
    ten: 'OPPO A79 5G 8GB/256GB',
    nhanHieu: 'OPPO', maLoai: 'OPPO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 6990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/04/a79-5g-tim.png',
    mau: [
      { id: 'tim', ten: 'Tím Lông Vũ', hex: '#C084FC', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/04/a79-5g-tim.png' },
      { id: 'den', ten: 'Đen Huyền Bí', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/04/a79-5g-den.png' }
    ],
    phienBan: [{ id: '8-256', dungLuong: '8GB/256GB', gia: 6990000, giaGoc: 7490000 }],
    thongSo: { 'Màn hình': '6.72" FHD+ 90Hz Sunlight', 'Loa': 'Kép Stereo 300% Ultra Volume' }
  },

  // --- VIVO & REALME ---
  {
    ten: 'Vivo V30 5G 12GB/512GB Aura Light Vòng Sáng',
    nhanHieu: 'Vivo', maLoai: 'VIVO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 12990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/vivo-v30-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Thủy Tinh', hex: '#38BDF8', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/vivo-v30-xanh.png' },
      { id: 'den', ten: 'Đen Ác Mộng', hex: '#0F172A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/vivo-v30-den.png' }
    ],
    phienBan: [{ id: '12-512', dungLuong: '12GB/512GB', gia: 12990000, giaGoc: 13990000 }],
    thongSo: { 'Màn hình': '6.78" AMOLED cong 1.5K 120Hz', 'Chip': 'Snapdragon 7 Gen 3 4nm', 'Đèn': 'Aura Light 3.0' }
  },
  {
    ten: 'Vivo Y100 8GB/128GB',
    nhanHieu: 'Vivo', maLoai: 'VIVO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 5990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/18/vivo-y100-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Kim Cương', hex: '#6EE7B7', anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/18/vivo-y100-xanh.png' },
      { id: 'den', ten: 'Đen Kim Cương', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/18/vivo-y100-den.png' }
    ],
    phienBan: [{ id: '8-128', dungLuong: '8GB/128GB', gia: 5990000, giaGoc: 6490000 }],
    thongSo: { 'Màn hình': '6.67" AMOLED 120Hz', 'Sạc': '80W FlashCharge 30 phút' }
  },
  {
    ten: 'Realme 12 Pro+ 5G 12GB/512GB Ống Kính Tiềm Vọng',
    nhanHieu: 'Realme', maLoai: 'REALME', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 12490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/18/realme-12-pro-plus-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Biển Sâu Da', hex: '#0284C7', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/18/realme-12-pro-plus-xanh.png' },
      { id: 'vang', ten: 'Vàng Cát Sa Mạc', hex: '#FEF08A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/18/realme-12-pro-plus-vang.png' }
    ],
    phienBan: [{ id: '12-512', dungLuong: '12GB/512GB', gia: 12490000, giaGoc: 13990000 }],
    thongSo: { 'Camera': '64MP Periscope Telephoto 120x Zoom', 'Thiết kế': 'Đồng hồ cao cấp Ollivier Saveo' }
  },
  {
    ten: 'Realme C67 8GB/128GB',
    nhanHieu: 'Realme', maLoai: 'REALME', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 4790000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/26/realme-c67-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Ốc Đảo', hex: '#86EFAC', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/26/realme-c67-xanh.png' },
      { id: 'den', ten: 'Đen Đá', hex: '#18181B', anh: 'https://i.hoanghamobile.vn/Uploads/2023/12/26/realme-c67-den.png' }
    ],
    phienBan: [{ id: '8-128', dungLuong: '8GB/128GB', gia: 4790000, giaGoc: 5290000 }],
    thongSo: { 'Camera': '108MP 3x In-sensor Zoom', 'Chip': 'Snapdragon 685 6nm' }
  },

  // --- HONOR & NUBIA & TECNO ---
  {
    ten: 'HONOR 200 5G 12GB/256GB Studio Chân Dung Harcourt',
    nhanHieu: 'HONOR', maLoai: 'HONOR', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 11990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/08/29/honor-200-trang.png',
    mau: [
      { id: 'trang', ten: 'Trắng Ánh Trăng', hex: '#F9FAFB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/08/29/honor-200-trang.png' },
      { id: 'den', ten: 'Đen Huyền Vũ', hex: '#0F172A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/08/29/honor-200-den.png' }
    ],
    phienBan: [{ id: '12-256', dungLuong: '12GB/256GB', gia: 11990000, giaGoc: 12990000 }],
    thongSo: { 'Camera': 'Chân dung Studio Harcourt Paris 50MP OIS', 'Chip': 'Snapdragon 7 Gen 3', 'Sạc': '100W SuperCharge' }
  },
  {
    ten: 'HONOR X9b 5G 12GB/256GB Chống Rơi Vỡ Toàn Diện',
    nhanHieu: 'HONOR', maLoai: 'HONOR', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 7990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/10/honor-x9b-cam.png',
    mau: [
      { id: 'cam', ten: 'Cam Bình Minh Da', hex: '#EA580C', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/10/honor-x9b-cam.png' },
      { id: 'den', ten: 'Đen Bóng Đêm', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2024/01/10/honor-x9b-den.png' }
    ],
    phienBan: [{ id: '12-256', dungLuong: '12GB/256GB', gia: 7990000, giaGoc: 8990000 }],
    thongSo: { 'Độ bền': 'Màn hình Ultra-Bounce chống rơi vỡ 360 độ 5 sao SGS', 'Pin': '5800 mAh dùng 3 ngày' }
  },
  {
    ten: 'Nubia RedMagic 9S Pro 16GB/512GB Gaming Phone',
    nhanHieu: 'Nubia', maLoai: 'NUBIA', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 21990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/26/red-magic-9s-pro-den.png',
    mau: [
      { id: 'den', ten: 'Sleet Đen Trong Suốt', hex: '#0F172A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/26/red-magic-9s-pro-den.png' },
      { id: 'bac', ten: 'Snowfall Bạc Băng', hex: '#E2E8F0', anh: 'https://i.hoanghamobile.vn/Uploads/2024/07/26/red-magic-9s-pro-bac.png' }
    ],
    phienBan: [{ id: '16-512', dungLuong: '16GB/512GB', gia: 21990000, giaGoc: 23990000 }],
    thongSo: { 'Chip': 'Snapdragon 8 Gen 3 Leading Version 3.4GHz', 'Tản nhiệt': 'Quạt chủ động ICE 13.5 22.000 RPM', 'Pin': '6500 mAh 80W' }
  },
  {
    ten: 'Nubia Neo 2 5G 8GB/256GB Gaming Giá Rẻ',
    nhanHieu: 'Nubia', maLoai: 'NUBIA', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 4890000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/03/nubia-neo-2-vang.png',
    mau: [
      { id: 'vang', ten: 'Vàng Mecha Hoàng Gia', hex: '#FACC15', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/03/nubia-neo-2-vang.png' },
      { id: 'den', ten: 'Xám Cơ Khí', hex: '#4B5563', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/03/nubia-neo-2-xam.png' }
    ],
    phienBan: [{ id: '8-256', dungLuong: '8GB/256GB', gia: 4890000, giaGoc: 5490000 }],
    thongSo: { 'Trigger': 'Nút bấm vai cảm ứng Dual Gaming Shoulder Triggers', 'Pin': '6000 mAh 33W' }
  },
  {
    ten: 'Tecno Camon 30 Premier 5G 12GB/512GB',
    nhanHieu: 'Tecno', maLoai: 'TECNO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 11490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/06/15/camon-30-premier-den.png',
    mau: [
      { id: 'den', ten: 'Đen Da Cao Cấp', hex: '#18181B', anh: 'https://i.hoanghamobile.vn/Uploads/2024/06/15/camon-30-premier-den.png' },
      { id: 'bac', ten: 'Bạc Tuyết', hex: '#E5E7EB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/06/15/camon-30-premier-bac.png' }
    ],
    phienBan: [{ id: '12-512', dungLuong: '12GB/512GB', gia: 11490000, giaGoc: 12990000 }],
    thongSo: { 'Camera': 'Hệ thống 4 camera Sony 50MP', 'Chip': 'Dimensity 8200 Ultimate' }
  },
  {
    ten: 'Tecno Spark 20 Pro+ 8GB/256GB',
    nhanHieu: 'Tecno', maLoai: 'TECNO', maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại',
    gia: 5190000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/25/spark-20-pro-plus-vang.png',
    mau: [
      { id: 'vang', ten: 'Vàng Cát Lún', hex: '#FEF08A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/25/spark-20-pro-plus-vang.png' },
      { id: 'den', ten: 'Đen Ánh Tinh Vân', hex: '#0F172A', anh: 'https://i.hoanghamobile.vn/Uploads/2024/03/25/spark-20-pro-plus-den.png' }
    ],
    phienBan: [{ id: '8-256', dungLuong: '8GB/256GB', gia: 5190000, giaGoc: 5690000 }],
    thongSo: { 'Màn hình': '6.78" AMOLED cong đôi 120Hz', 'Camera': '108MP 3x Lossless' }
  },

  // --- MÁY TÍNH BẢNG (TABLET) ---
  {
    ten: 'iPad Pro M4 11 inch 256GB Wifi',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 27990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-pro-m4-den.png',
    mau: [
      { id: 'den', ten: 'Đen Không Gian', hex: '#1F2937', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-pro-m4-den.png' },
      { id: 'bac', ten: 'Bạc', hex: '#E5E7EB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-pro-m4-bac.png' }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '256GB', gia: 27990000, giaGoc: 28990000 },
      { id: '512gb', dungLuong: '512GB', gia: 33490000, giaGoc: 34990000 }
    ],
    thongSo: { 'Màn hình': '11" Ultra Retina XDR Tandem OLED', 'Chip': 'Apple M4', 'Độ mỏng': '5.3 mm siêu mỏng nhẹ' }
  },
  {
    ten: 'iPad Air 6 M2 11 inch 128GB Wifi',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 16490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-air-m2-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Lam', hex: '#93C5FD', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-air-m2-xanh.png' },
      { id: 'tim', ten: 'Tím Khói', hex: '#DDD6FE', anh: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-air-m2-tim.png' }
    ],
    phienBan: [{ id: '128gb', dungLuong: '128GB', gia: 16490000, giaGoc: 17490000 }],
    thongSo: { 'Màn hình': '11" Liquid Retina', 'Chip': 'Apple M2 8-core CPU', 'Bút': 'Apple Pencil Pro' }
  },
  {
    ten: 'iPad Gen 10 10.9 inch 64GB Wifi',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 8990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2022/10/19/ipad-gen-10-xanh.png',
    mau: [
      { id: 'xanh', ten: 'Xanh Lam', hex: '#60A5FA', anh: 'https://i.hoanghamobile.vn/Uploads/2022/10/19/ipad-gen-10-xanh.png' },
      { id: 'hong', ten: 'Hồng', hex: '#F472B6', anh: 'https://i.hoanghamobile.vn/Uploads/2022/10/19/ipad-gen-10-hong.png' }
    ],
    phienBan: [{ id: '64gb', dungLuong: '64GB', gia: 8990000, giaGoc: 10990000 }],
    thongSo: { 'Màn hình': '10.9" Retina True Tone', 'Chip': 'A14 Bionic', 'Cổng': 'USB-C' }
  },
  {
    ten: 'Samsung Galaxy Tab S9 Ultra 12GB/256GB Kèm Bút S-Pen',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 24990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/07/26/tab-s9-ultra-den.png',
    mau: [{ id: 'den', ten: 'Đen Graphite', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2023/07/26/tab-s9-ultra-den.png' }],
    phienBan: [{ id: '256gb', dungLuong: '12GB/256GB', gia: 24990000, giaGoc: 32990000 }],
    thongSo: { 'Màn hình': '14.6" Dynamic AMOLED 2X 120Hz', 'Kháng nước': 'IP68 cả máy và bút S-Pen' }
  },
  {
    ten: 'Samsung Galaxy Tab A9+ Wifi 4GB/64GB',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 4790000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/10/24/tab-a9-plus-bac.png',
    mau: [{ id: 'bac', ten: 'Bạc Ánh Kim', hex: '#E5E7EB', anh: 'https://i.hoanghamobile.vn/Uploads/2023/10/24/tab-a9-plus-bac.png' }],
    phienBan: [{ id: '64gb', dungLuong: '4GB/64GB', gia: 4790000, giaGoc: 5490000 }],
    thongSo: { 'Màn hình': '11.0" 90Hz', 'Loa': '4 loa Dolby Atmos', 'Chip': 'Snapdragon 695 5G' }
  },
  {
    ten: 'Xiaomi Pad 6 8GB/128GB',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', maNhom: 'TABLET', tenNhom: 'Máy tính bảng',
    gia: 7990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/08/11/pad-6-xanh.png',
    mau: [{ id: 'xanh', ten: 'Xanh Dương', hex: '#93C5FD', anh: 'https://i.hoanghamobile.vn/Uploads/2023/08/11/pad-6-xanh.png' }],
    phienBan: [{ id: '8-128', dungLuong: '8GB/128GB', gia: 7990000, giaGoc: 8990000 }],
    thongSo: { 'Màn hình': '11" 2.8K 144Hz 7 mức tần số quét', 'Chip': 'Snapdragon 870 7nm' }
  },

  // --- ÂM THANH (TAI NGHE & LOA) ---
  {
    ten: 'Tai nghe Bluetooth Apple AirPods Pro 2 USB-C MagSafe',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'AMTHANH', tenNhom: 'Âm thanh',
    gia: 5490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/airpods-pro-2-usb-c.png',
    mau: [{ id: 'white', ten: 'Trắng', hex: '#FFFFFF', anh: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/airpods-pro-2-usb-c.png' }],
    phienBan: [{ id: 'std', dungLuong: 'USB-C MagSafe', gia: 5490000, giaGoc: 6190000 }],
    thongSo: { 'Chống ồn': 'Active Noise Cancellation thế hệ 2', 'Chip': 'Apple H2', 'Pin': '30 giờ kèm hộp sạc' }
  },
  {
    ten: 'Tai nghe Bluetooth Apple AirPods 3 MagSafe',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'AMTHANH', tenNhom: 'Âm thanh',
    gia: 3990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2021/10/19/airpods-3.png',
    mau: [{ id: 'white', ten: 'Trắng', hex: '#FFFFFF', anh: 'https://i.hoanghamobile.vn/Uploads/2021/10/19/airpods-3.png' }],
    phienBan: [{ id: 'std', dungLuong: 'Hộp sạc Lightning/MagSafe', gia: 3990000, giaGoc: 4490000 }],
    thongSo: { 'Âm thanh': 'Spatial Audio đa chiều', 'Pin': '30 giờ' }
  },
  {
    ten: 'Tai nghe Chống Ồn Sony WH-1000XM5 Cao Cấp',
    nhanHieu: 'Sony', maLoai: 'SONY', maNhom: 'AMTHANH', tenNhom: 'Âm thanh',
    gia: 6990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2022/05/20/wh-1000xm5-den.png',
    mau: [
      { id: 'den', ten: 'Đen Tuyển', hex: '#18181B', anh: 'https://i.hoanghamobile.vn/Uploads/2022/05/20/wh-1000xm5-den.png' },
      { id: 'bac', ten: 'Bạc Bạch Kim', hex: '#E5E7EB', anh: 'https://i.hoanghamobile.vn/Uploads/2022/05/20/wh-1000xm5-bac.png' }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: 6990000, giaGoc: 8490000 }],
    thongSo: { 'Chống ồn': 'Bộ xử lý tích hợp V1 + QN1 8 mic', 'Pin': '30 giờ sạc nhanh 3 phút được 3 giờ' }
  },
  {
    ten: 'Loa Bluetooth Marshall Emberton II Chính Hãng',
    nhanHieu: 'Marshall', maLoai: 'MARSHALL', maNhom: 'AMTHANH', tenNhom: 'Âm thanh',
    gia: 3890000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2022/06/20/marshall-emberton-ii-den.png',
    mau: [
      { id: 'black-brass', ten: 'Đen Đồng (Black and Brass)', hex: '#78350F', anh: 'https://i.hoanghamobile.vn/Uploads/2022/06/20/marshall-emberton-ii-den.png' },
      { id: 'cream', ten: 'Kem (Cream)', hex: '#FEF3C7', anh: 'https://i.hoanghamobile.vn/Uploads/2022/06/20/marshall-emberton-ii-kem.png' }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: 3890000, giaGoc: 4690000 }],
    thongSo: { 'Âm thanh': 'True Stereophonic 360 độ', 'Pin': '30+ giờ chơi nhạc', 'Kháng nước': 'IP67' }
  },
  {
    ten: 'Loa Bluetooth JBL Charge 5 Công Suất Khủng',
    nhanHieu: 'JBL', maLoai: 'JBL', maNhom: 'AMTHANH', tenNhom: 'Âm thanh',
    gia: 3490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2021/04/15/jbl-charge-5-den.png',
    mau: [
      { id: 'den', ten: 'Đen', hex: '#111827', anh: 'https://i.hoanghamobile.vn/Uploads/2021/04/15/jbl-charge-5-den.png' },
      { id: 'do', ten: 'Đỏ Thể Thao', hex: '#DC2626', anh: 'https://i.hoanghamobile.vn/Uploads/2021/04/15/jbl-charge-5-do.png' }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: 3490000, giaGoc: 3990000 }],
    thongSo: { 'Công suất': '40W RMS củ loa woofer riêng biệt', 'Pin': '20 giờ kiêm sạc dự phòng cho điện thoại' }
  },

  // --- PHỤ KIỆN CHÍNH HÃNG ---
  {
    ten: 'Củ Sạc Nhanh Apple 20W Type-C Chính Hãng',
    nhanHieu: 'Apple', maLoai: 'APPLE', maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện',
    gia: 490000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2020/11/04/cu-sac-apple-20w.png',
    mau: [{ id: 'white', ten: 'Trắng', hex: '#FFFFFF', anh: 'https://i.hoanghamobile.vn/Uploads/2020/11/04/cu-sac-apple-20w.png' }],
    phienBan: [{ id: 'std', dungLuong: '20W PD', gia: 490000, giaGoc: 690000 }],
    thongSo: { 'Cổng': 'USB-C Power Delivery', 'Công suất': '20W', 'Tương thích': 'iPhone 8 đến iPhone 16' }
  },
  {
    ten: 'Pin Sạc Dự Phòng Anker MagGo Qi2 10.000mAh Không Dây 15W',
    nhanHieu: 'Anker', maLoai: 'ANKER', maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện',
    gia: 1290000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/22/anker-maggo-qi2-den.png',
    mau: [
      { id: 'den', ten: 'Đen', hex: '#18181B', anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/22/anker-maggo-qi2-den.png' },
      { id: 'trang', ten: 'Trắng', hex: '#F9FAFB', anh: 'https://i.hoanghamobile.vn/Uploads/2024/04/22/anker-maggo-qi2-trang.png' }
    ],
    phienBan: [{ id: 'std', dungLuong: '10.000 mAh', gia: 1290000, giaGoc: 1590000 }],
    thongSo: { 'Chuẩn sạc': 'Qi2 15W sạc nhanh nam châm MagSafe', 'Màn hình': 'Hiển thị phần trăm pin thông minh' }
  },
  {
    ten: 'Củ Sạc Nhanh Anker 67W 3 Cổng GaNPrime Siêu Nhỏ Gọn',
    nhanHieu: 'Anker', maLoai: 'ANKER', maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện',
    gia: 990000,
    anh: 'https://i.hoanghamobile.vn/Uploads/2023/08/18/anker-ganprime-67w.png',
    mau: [{ id: 'den', ten: 'Đen Kim Loại', hex: '#1E293B', anh: 'https://i.hoanghamobile.vn/Uploads/2023/08/18/anker-ganprime-67w.png' }],
    phienBan: [{ id: 'std', dungLuong: '67W 2C1A', gia: 990000, giaGoc: 1290000 }],
    thongSo: { 'Cổng sạc': '2 x USB-C + 1 x USB-A', 'Công nghệ': 'GaNPrime sạc cùng lúc 3 thiết bị Laptop, Phone' }
  }
];

// Generate variations to easily and authentically reach 200 items
const brandsPool = [
  { code: 'APPLE', name: 'Apple', prefix: 'iPhone', models: ['14', '14 Plus', '14 Pro', '13 Pro Max', '12', '11 64GB', '11 128GB', 'SE 2022', '12 Pro'] },
  { code: 'SAMSUNG', name: 'Samsung', prefix: 'Galaxy', models: ['S23 Ultra 512GB', 'S23 FE', 'S23 128GB', 'A54 5G', 'A25 5G', 'A05s', 'A05', 'M34 5G', 'M54 5G', 'Z Fold5 512GB', 'Z Flip5'] },
  { code: 'XIAOMI', name: 'Xiaomi', prefix: 'Redmi', models: ['Note 12 Pro', 'Note 12 4G', '13C 6GB/128GB', '12 8GB/128GB', 'A3 4GB/128GB', 'K70 Pro', 'K70 Ultra', 'Pad SE 8GB/256GB'] },
  { code: 'OPPO', name: 'OPPO', prefix: 'OPPO', models: ['Reno11 Pro 5G', 'Reno11 F 5G', 'A58 6GB/128GB', 'A38 4GB/128GB', 'A18 4GB/64GB', 'Find N2 Flip'] },
  { code: 'VIVO', name: 'Vivo', prefix: 'Vivo', models: ['V29 5G', 'V29e 8GB/256GB', 'Y27 6GB/128GB', 'Y17s 4GB/128GB', 'Y03 4GB/64GB', 'X100 Pro'] },
  { code: 'REALME', name: 'Realme', prefix: 'Realme', models: ['11 Pro 5G', '11 4G', 'C55 6GB/128GB', 'C53 6GB/128GB', 'C51 4GB/128GB', 'GT Neo 5'] },
  { code: 'HONOR', name: 'HONOR', prefix: 'HONOR', models: ['90 5G 12GB/512GB', '90 Lite 5G', 'X8b 8GB/512GB', 'X7b 8GB/256GB', 'X6a 4GB/128GB', 'Magic6 Pro'] },
  { code: 'NUBIA', name: 'Nubia', prefix: 'Nubia', models: ['V60 Design 6GB/256GB', 'V60 4GB/128GB', 'Neo 5G 8GB/256GB', 'RedMagic 8S Pro', 'Focus 5G 6GB/256GB'] },
  { code: 'TECNO', name: 'Tecno', prefix: 'Tecno', models: ['Pova 6 Pro 5G', 'Pova 5 8GB/128GB', 'Spark 20 8GB/128GB', 'Spark 20C', 'Spark Go 2024', 'Pop 8'] },
  { code: 'NOKIA', name: 'Nokia', prefix: 'Nokia', models: ['G42 5G', 'G22 Tự Sửa Chữa', 'C32 4GB/64GB', '110 4G Pro', '105 4G Pro', '215 4G', '3210 4G'] },
  { code: 'ITEL', name: 'itel', prefix: 'itel', models: ['S23+ Màn Cong AMOLED', 'S23 8GB/128GB', 'P40 4GB/64GB Pin 6000mAh', 'A70 4GB/128GB', 'it2163 Pin Trâu'] },
  { code: 'SONY', name: 'Sony', prefix: 'Xperia', models: ['1 VI Flagship 4K', '10 VI Nhỏ Gọn', '5 V 8GB/128GB'] }
];

// Combine existing scraped items + templates + brands to reach 200 items
const finalProducts = [];

// Step A: Add scraped products
for (const p of existingList) {
  if (finalProducts.length >= 200) break;
  finalProducts.push(p);
}

// Step B: Add specific template products if not already included
for (const t of catalogTemplates) {
  if (finalProducts.length >= 200) break;
  const exists = finalProducts.some(p => p.tenHang.toLowerCase().includes(t.ten.toLowerCase().slice(0, 15)));
  if (!exists) {
    finalProducts.push({
      maHang: `MH${String(100 + finalProducts.length + 1).padStart(3, '0')}`,
      slug: t.ten.toLowerCase().replace(/[^\w-]/g, '-').slice(0, 40),
      tenHang: t.ten,
      nhanHieu: t.nhanHieu,
      maLoai: t.maLoai,
      maNhom: t.maNhom,
      tenNhom: t.tenNhom,
      donGia: t.gia,
      giaGoc: Math.round(t.gia * 1.08),
      anhChinh: t.anh,
      mauSac: t.mau,
      phienBan: t.phienBan,
      thongSo: t.thongSo
    });
  }
}

// Step C: Auto-generate realistic models for each brand until reaching exactly 200
let brandIdx = 0;
let round = 1;
while (finalProducts.length < 200) {
  const b = brandsPool[brandIdx % brandsPool.length];
  const modelIdx = Math.floor(brandIdx / brandsPool.length) % b.models.length;
  const baseModel = b.models[modelIdx];
  const tag = round > 1 ? ` (Bản ${round})` : '';
  const fullName = `${b.prefix} ${baseModel}${tag}`;
  
  const exists = finalProducts.some(p => p.tenHang.toLowerCase() === `${fullName} - Chính Hãng`.toLowerCase());
  if (!exists) {
    const isHighEnd = fullName.includes('Pro') || fullName.includes('Ultra') || fullName.includes('Fold') || fullName.includes('Flagship');
    const isBudget = fullName.includes('C') || fullName.includes('A0') || fullName.includes('Go') || fullName.includes('Pop') || fullName.includes('4G');
    let price = isHighEnd ? 21990000 : (isBudget ? 2490000 : 7990000);
    price += (finalProducts.length % 7) * 350000;

    const sampleImages = {
      APPLE: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-natural-titan.png',
      SAMSUNG: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-xam.png',
      XIAOMI: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-pro-xanh.png',
      OPPO: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-pro-nau.png',
      VIVO: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/vivo-v30-xanh.png',
      REALME: 'https://i.hoanghamobile.vn/Uploads/2024/05/18/realme-12-pro-plus-xanh.png',
      HONOR: 'https://i.hoanghamobile.vn/Uploads/2024/08/29/honor-200-trang.png',
      NUBIA: 'https://i.hoanghamobile.vn/Uploads/2024/07/26/red-magic-9s-pro-den.png',
      TECNO: 'https://i.hoanghamobile.vn/Uploads/2024/06/15/camon-30-premier-den.png',
      NOKIA: 'https://i.hoanghamobile.vn/Uploads/2023/07/20/nokia-g42-5g.png',
      ITEL: 'https://i.hoanghamobile.vn/Uploads/2024/09/09/itel-it9310-4g-1.png',
      SONY: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/sony-xperia-1-vi.png'
    };

    finalProducts.push({
      maHang: `MH${String(100 + finalProducts.length + 1).padStart(3, '0')}`,
      slug: fullName.toLowerCase().replace(/[^\w-]/g, '-'),
      tenHang: `${fullName} - Chính Hãng`,
      nhanHieu: b.name,
      maLoai: b.code,
      maNhom: 'DIENTHOAI',
      tenNhom: 'Điện thoại',
      donGia: price,
      giaGoc: Math.round(price * 1.08),
      anhChinh: sampleImages[b.code] || sampleImages.APPLE,
      mauSac: [
        { id: 'den', ten: 'Đen Huyền Bí', hex: '#1F2937', anh: sampleImages[b.code] || sampleImages.APPLE },
        { id: 'bac', ten: 'Bạc Ánh Kim', hex: '#E5E7EB', anh: sampleImages[b.code] || sampleImages.APPLE }
      ],
      phienBan: [
        { id: 'std', dungLuong: 'Tiêu chuẩn', gia: price, giaGoc: Math.round(price * 1.08) }
      ],
      thongSo: {
        'Màn hình': isHighEnd ? '6.7" OLED 120Hz' : '6.5" 90Hz',
        'Bộ nhớ': isHighEnd ? '256GB' : '128GB',
        'Pin': '5000 mAh'
      }
    });
  }
  brandIdx++;
  if (brandIdx % (brandsPool.length * 3) === 0) {
    round++;
  }
}

// Re-index maHang sequentially from MH101 to MH300
finalProducts.forEach((p, idx) => {
  p.maHang = `MH${String(100 + idx + 1).padStart(3, '0')}`;
});

console.log(`\n🎉 ĐÃ TẠO ĐỦ CHÍNH XÁC ${finalProducts.length} SẢN PHẨM ĐA DẠNG!`);

// 1. Save JSON
const metaMap = {};
for (const p of finalProducts) {
  metaMap[p.maHang] = p;
}
fs.writeFileSync(existingPath, JSON.stringify(metaMap, null, 2), 'utf8');
console.log(`✅ 1. Ghi file JSON: ${existingPath} (${Object.keys(metaMap).length} sản phẩm)`);

// 2. Save SQL seed
const sqlLines = [
  '-- ============================================================',
  `-- SEED ĐỦ 200 SẢN PHẨM ĐA DẠNG MẪU MÃ (HOÀNG HÀ MOBILE)`,
  '-- ============================================================',
  ''
];

// Insert NhomHang
const nhomMap = {
  DIENTHOAI: 'Điện thoại',
  TABLET: 'Máy tính bảng',
  AMTHANH: 'Âm thanh',
  PHUKIEN: 'Phụ kiện',
  DONGHO: 'Đồng hồ thông minh'
};
for (const [code, name] of Object.entries(nhomMap)) {
  sqlLines.push(`INSERT INTO NhomHang (MANHOM, TENNHOM) VALUES ('${code}', '${name}') ON CONFLICT (MANHOM) DO NOTHING;`);
}
sqlLines.push('');

// Insert LoaiHang
const brandsUsed = [...new Set(finalProducts.map(r => r.maLoai))];
for (const b of brandsUsed) {
  const brandName = finalProducts.find(r => r.maLoai === b)?.nhanHieu || b;
  sqlLines.push(`INSERT INTO LoaiHang (MALOAI, TENLOAI) VALUES ('${b}', '${brandName}') ON CONFLICT (MALOAI) DO NOTHING;`);
}
sqlLines.push('');

for (const p of finalProducts) {
  const cleanTen = p.tenHang.replace(/'/g, "''");
  sqlLines.push(`INSERT INTO MatHang (MAHANG, TENHANG, DONGIA, DVT, MANHOM, MALOAI, QUANLYIMEI, THOIHANBAOHANH) VALUES ('${p.maHang}', '${cleanTen}', ${p.donGia}, 'Cái', '${p.maNhom || 'DIENTHOAI'}', '${p.maLoai}', true, 12) ON CONFLICT (MAHANG) DO UPDATE SET TENHANG = EXCLUDED.TENHANG, DONGIA = EXCLUDED.DONGIA;`);
}

const sqlPath = path.join(__dirname, '..', 'database', 'seed-products-200.sql');
fs.writeFileSync(sqlPath, sqlLines.join('\n'), 'utf8');
console.log(`✅ 2. Ghi file SQL: ${sqlPath} (${finalProducts.length} dòng INSERT)`);
