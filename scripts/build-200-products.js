const fs = require('fs');
const path = require('path');

// Image collections
const IMG = {
  // Laptop & Màn hình
  MACBOOK_AIR_M3: 'https://i.hoanghamobile.vn/Uploads/2024/03/05/macbook-air-m3-midnight.png',
  MACBOOK_PRO_M3: 'https://i.hoanghamobile.vn/Uploads/2023/11/01/macbook-pro-14-m3-space-black.png',
  ASUS_ZENBOOK: 'https://i.hoanghamobile.vn/Uploads/2024/01/10/asus-zenbook-14-oled.png',
  ASUS_TUF: 'https://i.hoanghamobile.vn/Uploads/2023/04/14/asus-tuf-gaming-f15.png',
  ASUS_GAMING_V16: 'https://i.hoanghamobile.vn/Uploads/2025/06/12/v3607vu-rp290w-1.png',
  DELL_INSPIRON: 'https://i.hoanghamobile.vn/Uploads/2023/07/15/dell-inspiron-15-3520.png',
  DELL_XPS: 'https://i.hoanghamobile.vn/Uploads/2024/01/10/dell-xps-13-plus.png',
  THINKPAD_E14: 'https://i.hoanghamobile.vn/Uploads/2023/08/10/lenovo-thinkpad-e14-gen-5.png',
  LENOVO_LEGION: 'https://i.hoanghamobile.vn/Uploads/2024/02/20/lenovo-legion-pro-5.png',
  HP_PAVILION: 'https://i.hoanghamobile.vn/Uploads/2023/06/15/hp-pavilion-15.png',
  HP_VICTUS: 'https://i.hoanghamobile.vn/Uploads/2023/08/01/hp-victus-15.png',
  ACER_NITRO: 'https://i.hoanghamobile.vn/Uploads/2023/05/20/acer-nitro-5-tiger.png',
  MSI_MODERN: 'https://i.hoanghamobile.vn/Uploads/2023/07/10/msi-modern-14.png',
  LG_ULTRAGEAR: 'https://i.hoanghamobile.vn/Uploads/2023/09/15/man-hinh-lg-ultragear-27.png',
  DELL_ULTRASHARP: 'https://i.hoanghamobile.vn/Uploads/2023/10/12/dell-ultrasharp-u2724d.png',
  ASUS_PROART: 'https://i.hoanghamobile.vn/Uploads/2023/06/25/asus-proart-pa278cv.png',
  SAMSUNG_VIEWFINITY: 'https://i.hoanghamobile.vn/Uploads/2023/07/18/samsung-viewfinity-s6.png',
  SAMSUNG_SMART_M5: 'https://i.hoanghamobile.vn/Uploads/2023/05/10/samsung-smart-monitor-m5.png',
  XIAOMI_G27I: 'https://i.hoanghamobile.vn/Uploads/2024/01/22/xiaomi-gaming-monitor-g27i.png',
  VIEWSONIC_VX: 'https://i.hoanghamobile.vn/Uploads/2023/08/25/viewsonic-vx2728j.png',

  // Đồng hồ thông minh
  AW_S10: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/apple-watch-series-10-den.png',
  AW_ULTRA2: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/apple-watch-ultra-2-ocean.png',
  AW_SE: 'https://i.hoanghamobile.vn/Uploads/2023/10/12/se-40-starlight-starlight.png',
  GALAXY_WATCH7: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/galaxy-watch-7-xanh.png',
  GALAXY_WATCH_ULTRA: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/galaxy-watch-ultra-xam.png',
  GALAXY_WATCH6_CLASSIC: 'https://i.hoanghamobile.vn/Uploads/2023/07/26/galaxy-watch-6-classic-den.png',
  HUAWEI_GT5: 'https://i.hoanghamobile.vn/Uploads/2024/09/20/huawei-watch-gt-5-den.png',
  HUAWEI_GT5_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/09/20/huawei-watch-gt-5-pro-titanium.png',
  HUAWEI_BAND9: 'https://i.hoanghamobile.vn/Uploads/2024/04/10/huawei-band-9-den.png',
  GARMIN_FORERUNNER: 'https://i.hoanghamobile.vn/Uploads/2024/02/28/garmin-forerunner-165.png',
  GARMIN_VENU3: 'https://i.hoanghamobile.vn/Uploads/2023/09/01/garmin-venu-3.png',
  REDMI_WATCH4: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/redmi-watch-4-xam.png',
  XIAOMI_BAND9: 'https://i.hoanghamobile.vn/Uploads/2024/07/20/xiaomi-smart-band-9-den.png',
  AMAZFIT_BALANCE: 'https://i.hoanghamobile.vn/Uploads/2023/10/05/amazfit-balance.png',
  AMAZFIT_TREX: 'https://i.hoanghamobile.vn/Uploads/2023/04/12/amazfit-t-rex-ultra.png',

  // Máy tính bảng
  IPAD_PRO_M4: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-pro-m4-space-black.png',
  IPAD_AIR_M2: 'https://i.hoanghamobile.vn/Uploads/2024/05/08/ipad-air-m2-starlight.png',
  IPAD_GEN10: 'https://i.hoanghamobile.vn/Uploads/2022/10/19/ipad-gen-10-blue.png',
  IPAD_MINI7: 'https://i.hoanghamobile.vn/Uploads/2024/10/16/ipad-mini-7-purple.png',
  TAB_S9_ULTRA: 'https://i.hoanghamobile.vn/Uploads/2023/07/26/tab-s9-ultra-graphite.png',
  TAB_S9_FE: 'https://i.hoanghamobile.vn/Uploads/2023/10/04/tab-s9-fe-xanh.png',
  TAB_A9_PLUS: 'https://i.hoanghamobile.vn/Uploads/2023/10/24/tab-a9-plus-bac.png',
  XIAOMI_PAD6: 'https://i.hoanghamobile.vn/Uploads/2023/05/20/xiaomi-pad-6-xam.png',
  REDMI_PAD_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/04/15/redmi-pad-pro-xanh.png',
  REDMI_PAD_SE: 'https://i.hoanghamobile.vn/Uploads/2023/09/28/group-446.png',
  LENOVO_TAB_M11: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/lenovo-tab-m11.png',
  TCL_TAB11: 'https://i.hoanghamobile.vn/Uploads/2023/09/05/eternals11-tcl-tab-11-dark-gray-back.png',

  // Âm thanh
  AIRPODS_PRO_2: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/airpods-pro-2-usbc.png',
  AIRPODS_MAX: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/airpods-max-usbc-midnight.png',
  SONY_WH1000XM5: 'https://i.hoanghamobile.vn/Uploads/2022/05/19/sony-wh-1000xm5-den.png',
  SONY_WF1000XM5: 'https://i.hoanghamobile.vn/Uploads/2023/07/25/sony-wf-1000xm5-den.png',
  SONY_ULT_FIELD1: 'https://i.hoanghamobile.vn/Uploads/2024/04/12/sony-ult-field-1-den.png',
  MARSHALL_MAJOR4: 'https://i.hoanghamobile.vn/Uploads/2021/04/15/marshall-major-iv-den.png',
  MARSHALL_EMBERTON2: 'https://i.hoanghamobile.vn/Uploads/2022/06/10/marshall-emberton-ii-den.png',
  MARSHALL_STANMORE3: 'https://i.hoanghamobile.vn/Uploads/2022/07/20/marshall-stanmore-iii-nau.png',
  MARSHALL_MOTIF2: 'https://i.hoanghamobile.vn/Uploads/2023/09/15/marshall-motif-ii.png',
  JBL_CHARGE5: 'https://i.hoanghamobile.vn/Uploads/2021/05/20/jbl-charge-5-den.png',
  JBL_FLIP6: 'https://i.hoanghamobile.vn/Uploads/2022/03/15/jbl-flip-6-do.png',
  JBL_WAVE_BEAM: 'https://i.hoanghamobile.vn/Uploads/2023/02/10/true-wireless-jbl-wave-beam-4.png',
  HARMAN_ONYX8: 'https://i.hoanghamobile.vn/Uploads/2022/11/15/harman-kardon-onyx-8-den.png',
  ALPHA_WORKS_V21: 'https://i.hoanghamobile.vn/Uploads/2023/11/02/alpha-works-classic-v21-nau.png',
  SENNHEISER_ACCENTUM: 'https://i.hoanghamobile.vn/Uploads/2023/10/18/sennheiser-accentum-wireless.png',
  HUAWEI_FREEBUDS_SE2: 'https://i.hoanghamobile.vn/Uploads/2023/08/15/true-wireless-huawei-freebuds-se-2-3.png',
  HONOR_EARBUDS_X5: 'https://i.hoanghamobile.vn/Uploads/2024/07/16/earbuds-x5-7.png',

  // Phụ kiện
  APPLE_20W: 'https://i.hoanghamobile.vn/Uploads/2020/10/20/cu-sac-apple-20w-type-c.png',
  APPLE_CABLE_C: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/cap-sac-apple-c-to-c-woven.png',
  ANKER_67W: 'https://i.hoanghamobile.vn/Uploads/2023/08/18/anker-ganprime-67w.png',
  ANKER_MAGGO: 'https://i.hoanghamobile.vn/Uploads/2024/04/22/anker-maggo-qi2-den.png',
  MOPHIE_30W: 'https://i.hoanghamobile.vn/Uploads/2023/09/18/sac-mophie-essentials-pd-30w-1a1c-7.png',
  ANKER_20W: 'https://i.hoanghamobile.vn/Uploads/2021/03/25/sac-anker-powerport-iii-20w.png',
  INNOSTYLE_POWERMAG: 'https://i.hoanghamobile.vn/Uploads/2023/05/12/sac-du-phong-innostyle-powermag-10000.png',
  LOGITECH_MX_MASTER3S: 'https://i.hoanghamobile.vn/Uploads/2022/06/15/chuot-logitech-mx-master-3s-den.png',
  KEYCHRON_K2_PRO: 'https://i.hoanghamobile.vn/Uploads/2023/03/10/ban-phim-keychron-k2-pro.png',
  UAG_MONARCH_IP16: 'https://i.hoanghamobile.vn/Uploads/2024/09/12/op-lung-uag-monarch-iphone-16-pro-max.png',
  BELKIN_3IN1: 'https://i.hoanghamobile.vn/Uploads/2023/04/18/de-sac-khong-day-belkin-3in1-magsafe.png',
  BASEUS_HUB_7IN1: 'https://i.hoanghamobile.vn/Uploads/2023/02/25/hub-chuyen-doi-baseus-7in1.png',

  // Điện thoại
  IPHONE_16_PRO_MAX: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-max-desert-titan.png',
  IPHONE_16_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-natural-titan.png',
  IPHONE_16: 'https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-ultramarine.png',
  IPHONE_15_PRO_MAX: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pro-max-blue-titanium.png',
  IPHONE_15: 'https://i.hoanghamobile.vn/Uploads/2023/09/13/iphone-15-pink.png',
  IPHONE_13: 'https://i.hoanghamobile.vn/Uploads/2021/09/15/iphone-13-blue.png',
  GALAXY_S24_ULTRA: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-ultra-xam.png',
  GALAXY_Z_FOLD6: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/galaxy-z-fold-6-xanh.png',
  GALAXY_Z_FLIP6: 'https://i.hoanghamobile.vn/Uploads/2024/07/10/galaxy-z-flip-6-vang.png',
  GALAXY_S24: 'https://i.hoanghamobile.vn/Uploads/2024/01/18/s24-den.png',
  GALAXY_A55: 'https://i.hoanghamobile.vn/Uploads/2024/03/12/galaxy-a55-5g-xanh.png',
  GALAXY_S26_ULTRA: 'https://i.hoanghamobile.vn/Uploads/2025/01/15/s26-ultra-tim-1.png',
  XIAOMI_14_ULTRA: 'https://i.hoanghamobile.vn/Uploads/2024/02/25/xiaomi-14-ultra-trang.png',
  REDMI_NOTE_13_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/01/15/note-13-pro-xanh.png',
  REDMI_13C: 'https://i.hoanghamobile.vn/Uploads/2023/11/10/redmi-13c-xanh.png',
  POCO_X6_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/01/11/poco-x6-pro-vang.png',
  OPPO_FIND_N3_FLIP: 'https://i.hoanghamobile.vn/Uploads/2023/10/26/oppo-find-n3-flip-hong.png',
  OPPO_RENO12_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/07/08/reno12-pro-nau.png',
  OPPO_A79_5G: 'https://i.hoanghamobile.vn/Uploads/2024/01/05/oppo-a79-5g-tim.png',
  VIVO_X100_PRO: 'https://i.hoanghamobile.vn/Uploads/2023/11/15/vivo-x100-pro-cam.png',
  VIVO_V30_5G: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/vivo-v30-xanh.png',
  VIVO_Y100_5G: 'https://i.hoanghamobile.vn/Uploads/2024/04/20/vivo-y100-5g-tim.png',
  REALME_12_PRO_PLUS: 'https://i.hoanghamobile.vn/Uploads/2024/05/18/realme-12-pro-plus-xanh.png',
  REALME_C67: 'https://i.hoanghamobile.vn/Uploads/2023/12/20/realme-c67-xanh.png',
  REALME_C61: 'https://i.hoanghamobile.vn/Uploads/2024/07/15/realme-c61-thumb.png',
  HONOR_200_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/08/29/honor-200-trang.png',
  HONOR_MAGIC6_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/03/01/honor-magic-6-pro-xanh.png',
  NUBIA_V80_MAX: 'https://i.hoanghamobile.vn/Uploads/2024/11/05/nubia-v80-max-den.png',
  NUBIA_NEO_5G: 'https://i.hoanghamobile.vn/Uploads/2023/07/25/nubia-neo-5g-vang.png',
  REDMAGIC_9S_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/07/26/red-magic-9s-pro-den.png',
  REDMAGIC_NOVA_TAB: 'https://i.hoanghamobile.vn/Uploads/2024/10/08/redmagic-nova-tab.png',
  TECNO_CAMON_30: 'https://i.hoanghamobile.vn/Uploads/2024/06/15/camon-30-premier-den.png',
  TECNO_POVA_6_PRO: 'https://i.hoanghamobile.vn/Uploads/2024/03/25/tecno-pova-6-pro.png',
  TECNO_SPARK_20: 'https://i.hoanghamobile.vn/Uploads/2023/12/15/tecno-spark-20-den.png',
  NOKIA_G42_5G: 'https://i.hoanghamobile.vn/Uploads/2023/07/20/nokia-g42-5g.png',
  NOKIA_T21_TAB: 'https://i.hoanghamobile.vn/Uploads/2023/03/15/nokia-t21-xam.png',
  ITEL_S23_PLUS: 'https://i.hoanghamobile.vn/Uploads/2023/09/25/itel-s23-plus-xanh.png',
  ITEL_IT9310: 'https://i.hoanghamobile.vn/Uploads/2024/09/09/itel-it9310-4g-1.png',
  SONY_XPERIA_1_VI: 'https://i.hoanghamobile.vn/Uploads/2024/05/17/sony-xperia-1-vi.png',
  HERA_S9: 'https://i.hoanghamobile.vn/Uploads/2024/11/20/s9.png'
};

// Fallback images in case
const FALLBACK_IMG = {
  LAPTOP: IMG.MACBOOK_AIR_M3,
  DONGHO: IMG.AW_S10,
  TABLET: IMG.IPAD_AIR_M2,
  AMTHANH: IMG.AIRPODS_PRO_2,
  PHUKIEN: IMG.ANKER_67W,
  DIENTHOAI: IMG.IPHONE_16_PRO_MAX
};

// 1. LAPTOP & MÀN HÌNH (25 sản phẩm)
const laptopProducts = [
  {
    ten: 'MacBook Air M3 13 inch 16GB/256GB - Chính Hãng Apple VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 26990000,
    anh: IMG.MACBOOK_AIR_M3,
    mau: [
      { id: 'midnight', ten: 'Xanh Đen Midnight', hex: '#1C2530', anh: IMG.MACBOOK_AIR_M3 },
      { id: 'starlight', ten: 'Vàng Ánh Kim Starlight', hex: '#EBE6DC', anh: IMG.MACBOOK_AIR_M3 },
      { id: 'space-gray', ten: 'Xám Không Gian', hex: '#7D7E80', anh: IMG.MACBOOK_AIR_M3 },
      { id: 'silver', ten: 'Bạc Ánh Bạc', hex: '#E3E4E6', anh: IMG.MACBOOK_AIR_M3 }
    ],
    phienBan: [
      { id: '16-256', dungLuong: '16GB / 256GB SSD', gia: 26990000, giaGoc: 28990000 },
      { id: '16-512', dungLuong: '16GB / 512GB SSD', gia: 31990000, giaGoc: 33990000 },
      { id: '24-512', dungLuong: '24GB / 512GB SSD', gia: 36990000, giaGoc: 39990000 }
    ],
    thongSo: { 'CPU': 'Apple M3 8-core', 'GPU': '8-core GPU', 'RAM': '16GB Unified', 'Ổ cứng': '256GB SSD', 'Màn hình': '13.6" Liquid Retina 500 nits' }
  },
  {
    ten: 'MacBook Pro 14 inch M3 Pro 18GB/512GB Space Black',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 47990000,
    anh: IMG.MACBOOK_PRO_M3,
    mau: [
      { id: 'space-black', ten: 'Đen Không Gian Space Black', hex: '#222324', anh: IMG.MACBOOK_PRO_M3 },
      { id: 'silver', ten: 'Bạc Ánh Kim Silver', hex: '#E3E4E6', anh: IMG.MACBOOK_PRO_M3 }
    ],
    phienBan: [
      { id: '18-512', dungLuong: '18GB / 512GB', gia: 47990000, giaGoc: 49990000 },
      { id: '36-512', dungLuong: '36GB / 512GB', gia: 54990000, giaGoc: 57990000 }
    ],
    thongSo: { 'CPU': 'Apple M3 Pro 11-core', 'GPU': '14-core GPU', 'RAM': '18GB Unified', 'Màn hình': '14.2" Liquid Retina XDR 120Hz ProMotion' }
  },
  {
    ten: 'Laptop ASUS Zenbook 14 OLED UX3405MA Intel Core Ultra 7',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 28490000,
    anh: IMG.ASUS_ZENBOOK,
    mau: [{ id: 'xanh', ten: 'Xanh Ponder Blue', hex: '#2C3E50', anh: IMG.ASUS_ZENBOOK }],
    phienBan: [{ id: 'u7-16-512', dungLuong: 'Core Ultra 7 / 16GB / 512GB', gia: 28490000, giaGoc: 31990000 }],
    thongSo: { 'CPU': 'Intel Core Ultra 7 155H', 'RAM': '16GB LPDDR5X', 'Ổ cứng': '512GB PCIe 4.0 SSD', 'Màn hình': '14" 3K OLED 120Hz 100% DCI-P3' }
  },
  {
    ten: 'Laptop ASUS Gaming V16 V3607V-TK416W Core 5-210H / RTX 4050',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 31690000,
    anh: IMG.ASUS_GAMING_V16,
    mau: [{ id: 'den', ten: 'Đen Eclipse Gray', hex: '#1E293B', anh: IMG.ASUS_GAMING_V16 }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB / RTX 4050 6GB', gia: 31690000, giaGoc: 41990000 }],
    thongSo: { 'CPU': 'Intel Core 5-210H', 'VGA': 'NVIDIA RTX 4050 6GB GDDR6', 'RAM': '16GB DDR5', 'Màn hình': '16" WUXGA 144Hz' }
  },
  {
    ten: 'Laptop ASUS TUF Gaming F15 FX506HF Intel Core i5-11400H / RTX 2050',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 14990000,
    anh: IMG.ASUS_TUF,
    mau: [{ id: 'den', ten: 'Đen Graphite Black', hex: '#1F2937', anh: IMG.ASUS_TUF }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB SSD', gia: 14990000, giaGoc: 17990000 }],
    thongSo: { 'CPU': 'Intel Core i5-11400H', 'RAM': '16GB DDR4', 'Ổ cứng': '512GB SSD', 'VGA': 'RTX 2050 4GB', 'Màn hình': '15.6" FHD 144Hz IPS' }
  },
  {
    ten: 'Laptop Dell Inspiron 15 3520 Intel Core i5-1235U / 16GB / 512GB',
    nhanHieu: 'Dell', maLoai: 'DELL', donGia: 13990000,
    anh: IMG.DELL_INSPIRON,
    mau: [{ id: 'den', ten: 'Đen Carbon Black', hex: '#18181B', anh: IMG.DELL_INSPIRON }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 13990000, giaGoc: 15490000 }],
    thongSo: { 'CPU': 'Intel Core i5-1235U', 'RAM': '16GB DDR4', 'Ổ cứng': '512GB NVMe SSD', 'Màn hình': '15.6" FHD 120Hz Chống chói' }
  },
  {
    ten: 'Laptop Dell XPS 13 9340 Intel Core Ultra 7 / 16GB / 512GB OLED',
    nhanHieu: 'Dell', maLoai: 'DELL', donGia: 42990000,
    anh: IMG.DELL_XPS,
    mau: [{ id: 'bac', ten: 'Bạc Platinum', hex: '#D1D5DB', anh: IMG.DELL_XPS }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 42990000, giaGoc: 46990000 }],
    thongSo: { 'CPU': 'Intel Core Ultra 7 155H', 'RAM': '16GB LPDDR5X', 'Màn hình': '13.4" 3K+ OLED InfinityEdge Touch' }
  },
  {
    ten: 'Laptop Lenovo ThinkPad E14 Gen 5 AMD Ryzen 5 7530U',
    nhanHieu: 'Lenovo', maLoai: 'LENOVO', donGia: 15490000,
    anh: IMG.THINKPAD_E14,
    mau: [{ id: 'den', ten: 'Đen Nhám Classic', hex: '#111827', anh: IMG.THINKPAD_E14 }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 15490000, giaGoc: 17290000 }],
    thongSo: { 'CPU': 'AMD Ryzen 5 7530U 6 nhân 12 luồng', 'RAM': '16GB DDR4', 'Màn hình': '14.0" WUXGA IPS 300 nits' }
  },
  {
    ten: 'Laptop Gaming Lenovo Legion Pro 5 i7-14700HX / RTX 4060 8GB',
    nhanHieu: 'Lenovo', maLoai: 'LENOVO', donGia: 38990000,
    anh: IMG.LENOVO_LEGION,
    mau: [{ id: 'xam', ten: 'Xám Onyx Grey', hex: '#475569', anh: IMG.LENOVO_LEGION }],
    phienBan: [{ id: 'std', dungLuong: '32GB / 1TB / RTX 4060', gia: 38990000, giaGoc: 42990000 }],
    thongSo: { 'CPU': 'Intel Core i7-14700HX', 'VGA': 'RTX 4060 8GB 140W TGP', 'Màn hình': '16" 2.5K WQXGA 240Hz 500 nits' }
  },
  {
    ten: 'Laptop HP Pavilion 15-eg3093TU Core i5-1335U / 16GB / 512GB',
    nhanHieu: 'HP', maLoai: 'HP', donGia: 16890000,
    anh: IMG.HP_PAVILION,
    mau: [{ id: 'vang', ten: 'Vàng Ánh Kim Warm Gold', hex: '#FDE68A', anh: IMG.HP_PAVILION }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 16890000, giaGoc: 18990000 }],
    thongSo: { 'CPU': 'Intel Core i5-1335U', 'RAM': '16GB DDR4', 'Màn hình': '15.6" FHD IPS Viền mỏng' }
  },
  {
    ten: 'Laptop HP Victus 15-fa1139TX i5-12450H / RTX 2050 4GB',
    nhanHieu: 'HP', maLoai: 'HP', donGia: 15990000,
    anh: IMG.HP_VICTUS,
    mau: [{ id: 'den', ten: 'Đen Mica Silver', hex: '#334155', anh: IMG.HP_VICTUS }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 15990000, giaGoc: 17990000 }],
    thongSo: { 'CPU': 'Intel Core i5-12450H', 'VGA': 'RTX 2050 4GB', 'Màn hình': '15.6" FHD 144Hz IPS' }
  },
  {
    ten: 'Laptop Gaming Acer Nitro 5 Tiger AN515 Core i5-12500H / RTX 3050',
    nhanHieu: 'Acer', maLoai: 'ACER', donGia: 18990000,
    anh: IMG.ACER_NITRO,
    mau: [{ id: 'den', ten: 'Đen Obsidian', hex: '#1E1E24', anh: IMG.ACER_NITRO }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 18990000, giaGoc: 21990000 }],
    thongSo: { 'CPU': 'Intel Core i5-12500H 12 nhân 16 luồng', 'VGA': 'RTX 3050 4GB GDDR6', 'Màn hình': '15.6" FHD 144Hz IPS' }
  },
  {
    ten: 'Laptop MSI Modern 14 C12MO Core i5-1235U / 16GB / 512GB Mỏng Nhẹ',
    nhanHieu: 'MSI', maLoai: 'OTHER', donGia: 12490000,
    anh: IMG.MSI_MODERN,
    mau: [{ id: 'den', ten: 'Đen Cổ Điển Classic Black', hex: '#1F2937', anh: IMG.MSI_MODERN }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 12490000, giaGoc: 14290000 }],
    thongSo: { 'CPU': 'Intel Core i5-1235U', 'RAM': '16GB DDR4', 'Trọng lượng': '1.4 kg siêu gọn nhẹ', 'Màn hình': '14.0" FHD IPS-Level' }
  },
  {
    ten: 'Màn hình Gaming LG UltraGear 27GR75Q-B 27 inch 2K IPS 165Hz 1ms',
    nhanHieu: 'LG', maLoai: 'LG', donGia: 4990000,
    anh: IMG.LG_ULTRAGEAR,
    mau: [{ id: 'den', ten: 'Đen Gaming Đỏ', hex: '#0F172A', anh: IMG.LG_ULTRAGEAR }],
    phienBan: [{ id: '27-2k-165hz', dungLuong: '27 inch 2K QHD', gia: 4990000, giaGoc: 6290000 }],
    thongSo: { 'Kích thước': '27 inch QHD (2560 x 1440)', 'Tấm nền': 'IPS 1ms GtG', 'Tần số quét': '165Hz FreeSync & G-Sync Compatible' }
  },
  {
    ten: 'Màn hình Dell UltraSharp U2724D 27 inch 2K IPS Black 120Hz',
    nhanHieu: 'Dell', maLoai: 'DELL', donGia: 9490000,
    anh: IMG.DELL_ULTRASHARP,
    mau: [{ id: 'bac', ten: 'Bạc Platinum Viền Vô Cực', hex: '#E5E7EB', anh: IMG.DELL_ULTRASHARP }],
    phienBan: [{ id: '27-2k-ips-black', dungLuong: '27 inch 2K IPS Black', gia: 9490000, giaGoc: 10990000 }],
    thongSo: { 'Kích thước': '27 inch 2K (2560 x 1440)', 'Tấm nền': 'IPS Black độ tương phản 2000:1', 'Tần số quét': '120Hz', 'Cổng': 'Thunderbolt 4, Type-C 90W PD' }
  },
  {
    ten: 'Màn hình ASUS ProArt PA278CV 27 inch 2K IPS Chuẩn Màu Đồ Họa',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 8690000,
    anh: IMG.ASUS_PROART,
    mau: [{ id: 'den', ten: 'Đen Chuyên Nghiệp', hex: '#1E293B', anh: IMG.ASUS_PROART }],
    phienBan: [{ id: '27-2k-proart', dungLuong: '27 inch 2K 100% sRGB', gia: 8690000, giaGoc: 9990000 }],
    thongSo: { 'Kích thước': '27 inch 2K', 'Độ chuẩn màu': 'Delta E < 2, Calman Verified', 'Cổng kết nối': 'USB-C 65W PD, DisplayPort Daisy-chain' }
  },
  {
    ten: 'Màn hình Samsung ViewFinity S6 S60UA 24 inch 2K Chân Đế Công Thái Học',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 4790000,
    anh: IMG.SAMSUNG_VIEWFINITY,
    mau: [{ id: 'den', ten: 'Đen Nhám', hex: '#18181B', anh: IMG.SAMSUNG_VIEWFINITY }],
    phienBan: [{ id: '24-2k-usb-c', dungLuong: '24 inch 2K QHD', gia: 4790000, giaGoc: 5990000 }],
    thongSo: { 'Kích thước': '24 inch 2K QHD IPS', 'Tính năng': 'Xoay dọc 90 độ, nâng hạ độ cao', 'Cổng': 'USB-C sạc 90W + LAN RJ45' }
  },
  {
    ten: 'Màn hình Xiaomi Gaming Monitor G27i 27 inch Fast IPS 165Hz',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 2890000,
    anh: IMG.XIAOMI_G27I,
    mau: [{ id: 'den', ten: 'Đen Thể Thao', hex: '#0F172A', anh: IMG.XIAOMI_G27I }],
    phienBan: [{ id: '27-fhd-165hz', dungLuong: '27 inch FHD 165Hz', gia: 2890000, giaGoc: 3490000 }],
    thongSo: { 'Kích thước': '27 inch Full HD', 'Tấm nền': 'Fast IPS 1ms', 'Tần số quét': '165Hz FreeSync Premium' }
  },
  {
    ten: 'Màn hình thông minh Samsung Smart Monitor M5 32 inch FHD TizenOS',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 4990000,
    anh: IMG.SAMSUNG_SMART_M5,
    mau: [{ id: 'trang', ten: 'Trắng Thanh Lịch', hex: '#F8FAFC', anh: IMG.SAMSUNG_SMART_M5 }],
    phienBan: [{ id: '32-smart-white', dungLuong: '32 inch FHD Smart TV', gia: 4990000, giaGoc: 6490000 }],
    thongSo: { 'Kích thước': '32 inch Full HD VA', 'Hệ điều hành': 'Tizen OS xem Netflix, YouTube không cần PC', 'Loa tích hợp': 'Loa stereo sống động' }
  },
  {
    ten: 'Màn hình ViewSonic Gaming VX2728J 27 inch Fast IPS 180Hz 0.5ms',
    nhanHieu: 'ViewSonic', maLoai: 'OTHER', donGia: 3690000,
    anh: IMG.VIEWSONIC_VX,
    mau: [{ id: 'den', ten: 'Đen Chân Công Thái Học', hex: '#1E293B', anh: IMG.VIEWSONIC_VX }],
    phienBan: [{ id: '27-180hz', dungLuong: '27 inch 180Hz', gia: 3690000, giaGoc: 4490000 }],
    thongSo: { 'Kích thước': '27 inch FHD', 'Tần số quét': '180Hz siêu mượt', 'Tốc độ phản hồi': '0.5ms MPRT' }
  },
  {
    ten: 'Laptop ASUS Vivobook 15 X1504VA Intel Core i3-1315U / 8GB / 512GB',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 10490000,
    anh: IMG.ASUS_ZENBOOK,
    mau: [{ id: 'bac', ten: 'Bạc Cool Silver', hex: '#E2E8F0', anh: IMG.ASUS_ZENBOOK }],
    phienBan: [{ id: 'std', dungLuong: '8GB / 512GB', gia: 10490000, giaGoc: 12490000 }],
    thongSo: { 'CPU': 'Intel Core i3-1315U', 'RAM': '8GB DDR4', 'Màn hình': '15.6" FHD Chống lóa' }
  },
  {
    ten: 'Laptop Lenovo IdeaPad Slim 3 15IAH8 i5-12450H / 16GB / 512GB',
    nhanHieu: 'Lenovo', maLoai: 'LENOVO', donGia: 12990000,
    anh: IMG.THINKPAD_E14,
    mau: [{ id: 'xam', ten: 'Xám Arctic Grey', hex: '#64748B', anh: IMG.THINKPAD_E14 }],
    phienBan: [{ id: 'std', dungLuong: '16GB / 512GB', gia: 12990000, giaGoc: 14990000 }],
    thongSo: { 'CPU': 'Intel Core i5-12450H', 'RAM': '16GB LPDDR5', 'Màn hình': '15.6" FHD IPS 300 nits' }
  },
  {
    ten: 'Laptop Acer Aspire 3 A315-59 Intel Core i5-1235U / 8GB / 512GB',
    nhanHieu: 'Acer', maLoai: 'ACER', donGia: 10990000,
    anh: IMG.ACER_NITRO,
    mau: [{ id: 'bac', ten: 'Bạc Pure Silver', hex: '#E5E7EB', anh: IMG.ACER_NITRO }],
    phienBan: [{ id: 'std', dungLuong: '8GB / 512GB', gia: 10990000, giaGoc: 12990000 }],
    thongSo: { 'CPU': 'Intel Core i5-1235U', 'RAM': '8GB DDR4', 'Màn hình': '15.6" FHD Acer ComfyView' }
  },
  {
    ten: 'Laptop Dell Vostro 3520 Intel Core i5-1235U / 8GB / 512GB',
    nhanHieu: 'Dell', maLoai: 'DELL', donGia: 12790000,
    anh: IMG.DELL_INSPIRON,
    mau: [{ id: 'den', ten: 'Đen Doanh Nhân', hex: '#18181B', anh: IMG.DELL_INSPIRON }],
    phienBan: [{ id: 'std', dungLuong: '8GB / 512GB', gia: 12790000, giaGoc: 14490000 }],
    thongSo: { 'CPU': 'Intel Core i5-1235U', 'RAM': '8GB DDR4', 'Màn hình': '15.6" FHD 120Hz' }
  },
  {
    ten: 'Màn hình Gaming ASUS TUF VG249Q3A 24 inch Fast IPS 180Hz 1ms',
    nhanHieu: 'ASUS', maLoai: 'ASUS', donGia: 2990000,
    anh: IMG.ASUS_PROART,
    mau: [{ id: 'den', ten: 'Đen Chiến Binh', hex: '#0F172A', anh: IMG.ASUS_PROART }],
    phienBan: [{ id: '24-180hz', dungLuong: '24 inch 180Hz', gia: 2990000, giaGoc: 3690000 }],
    thongSo: { 'Kích thước': '24 inch Full HD', 'Tần số quét': '180Hz Fast IPS', 'Công nghệ': 'ELMB Sync, FreeSync Premium' }
  }
];

// 2. ĐỒNG HỒ THÔNG MINH (20 sản phẩm)
const watchProducts = [
  {
    ten: 'Apple Watch Series 10 Nhôm 42mm GPS Dây Cao Su - Chính Hãng VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 9990000,
    anh: IMG.AW_S10,
    mau: [
      { id: 'den-bong', ten: 'Đen Bóng Jet Black', hex: '#0F172A', anh: IMG.AW_S10 },
      { id: 'vang-hong', ten: 'Vàng Hồng Rose Gold', hex: '#FBCFE8', anh: IMG.AW_S10 },
      { id: 'bac', ten: 'Bạc Silver', hex: '#E5E7EB', anh: IMG.AW_S10 }
    ],
    phienBan: [
      { id: '42mm-gps', dungLuong: '42mm GPS', gia: 9990000, giaGoc: 10990000 },
      { id: '46mm-gps', dungLuong: '46mm GPS', gia: 10790000, giaGoc: 11990000 },
      { id: '46mm-lte', dungLuong: '46mm GPS + LTE', gia: 13490000, giaGoc: 14990000 }
    ],
    thongSo: { 'Màn hình': 'OLED góc nhìn rộng sáng 2000 nits', 'Chip': 'Apple S10 SiP 64-bit', 'Tính năng': 'Phát hiện ngưng thở khi ngủ, sạc nhanh 80% trong 30p' }
  },
  {
    ten: 'Apple Watch Ultra 2 Titanium 49mm GPS + Cellular Dây Ocean',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 19990000,
    anh: IMG.AW_ULTRA2,
    mau: [{ id: 'titan', ten: 'Titan Tự Nhiên', hex: '#9CA3AF', anh: IMG.AW_ULTRA2 }],
    phienBan: [{ id: 'std', dungLuong: '49mm Titanium GPS + LTE', gia: 19990000, giaGoc: 21990000 }],
    thongSo: { 'Vỏ máy': 'Titanium hàng không vũ trụ', 'Độ sáng màn hình': '3000 nits ngoài trời', 'Pin': '36 giờ (chế độ tiết kiệm 72 giờ)', 'Chống nước': '100m lặn sâu' }
  },
  {
    ten: 'Apple Watch SE 2023 GPS + LTE 40mm Vỏ Nhôm Dây Cao Su VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 5990000,
    anh: IMG.AW_SE,
    mau: [
      { id: 'starlight', ten: 'Ánh Sao Starlight', hex: '#F3F4F6', anh: IMG.AW_SE },
      { id: 'midnight', ten: 'Đen Midnight', hex: '#1E293B', anh: IMG.AW_SE }
    ],
    phienBan: [
      { id: '40mm', dungLuong: '40mm GPS', gia: 5990000, giaGoc: 6990000 },
      { id: '44mm', dungLuong: '44mm GPS', gia: 6790000, giaGoc: 7690000 }
    ],
    thongSo: { 'Chip': 'Apple S8 SiP', 'Tính năng': 'Phát hiện va chạm xe hơi, theo dõi nhịp tim 24/7, phát hiện ngã' }
  },
  {
    ten: 'Samsung Galaxy Watch7 40mm Bluetooth Galaxy AI Mới',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 5490000,
    anh: IMG.GALAXY_WATCH7,
    mau: [
      { id: 'xanh', ten: 'Xanh Quân Đội Khaki', hex: '#4B5563', anh: IMG.GALAXY_WATCH7 },
      { id: 'bac', ten: 'Bạc Kem Cream', hex: '#F5F5F4', anh: IMG.GALAXY_WATCH7 }
    ],
    phienBan: [
      { id: '40mm', dungLuong: '40mm Bluetooth', gia: 5490000, giaGoc: 6490000 },
      { id: '44mm', dungLuong: '44mm Bluetooth', gia: 6190000, giaGoc: 7190000 }
    ],
    thongSo: { 'Chip': 'Exynos W1000 3nm 5 nhân', 'AI': 'Galaxy AI huấn luyện viên sức khỏe Energy Score', 'GPS': 'Băng tần kép L1+L5 chính xác' }
  },
  {
    ten: 'Samsung Galaxy Watch Ultra 47mm LTE Titanium Siêu Bền',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 14990000,
    anh: IMG.GALAXY_WATCH_ULTRA,
    mau: [{ id: 'xam', ten: 'Xám Titan Dark Grey', hex: '#374151', anh: IMG.GALAXY_WATCH_ULTRA }],
    phienBan: [{ id: 'std', dungLuong: '47mm Titanium LTE', gia: 14990000, giaGoc: 16990000 }],
    thongSo: { 'Vỏ': 'Titanium cấp 4 độ bền quân đội MIL-STD-810H', 'Kháng nước': '10ATM chịu áp lực nước 100m', 'Pin': '100 giờ tập luyện liên tục' }
  },
  {
    ten: 'Samsung Galaxy Watch6 Classic 43mm Viền Xoay Sang Trọng',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 4490000,
    anh: IMG.GALAXY_WATCH6_CLASSIC,
    mau: [{ id: 'den', ten: 'Đen Kim Loại', hex: '#18181B', anh: IMG.GALAXY_WATCH6_CLASSIC }],
    phienBan: [{ id: 'std', dungLuong: '43mm Viền Xoay', gia: 4490000, giaGoc: 5990000 }],
    thongSo: { 'Thiết kế': 'Viền xoay vật lý trứ danh', 'Mặt kính': 'Kính Sapphire chống xước tối ưu', 'Đo': 'Điện tâm đồ ECG, huyết áp 24/7' }
  },
  {
    ten: 'Huawei Watch GT 5 46mm Dây Da Sang Trọng Khung Thép',
    nhanHieu: 'Huawei', maLoai: 'HUAWEI', donGia: 5490000,
    anh: IMG.HUAWEI_GT5,
    mau: [
      { id: 'nau', ten: 'Dây Da Nâu Sang', hex: '#78350F', anh: IMG.HUAWEI_GT5 },
      { id: 'den', ten: 'Đen Cao Su Thể Thao', hex: '#1E293B', anh: IMG.HUAWEI_GT5 }
    ],
    phienBan: [{ id: '46mm', dungLuong: '46mm Pin 14 Ngày', gia: 5490000, giaGoc: 6490000 }],
    thongSo: { 'Pin': 'Thời lượng pin ấn tượng lên đến 14 ngày', 'Định vị': 'Hệ thống định vị hướng tâm Huawei Sunflower', 'Màn hình': '1.43" AMOLED 466x466' }
  },
  {
    ten: 'Huawei Watch GT 5 Pro Titanium 46mm Sapphire Cao Cấp',
    nhanHieu: 'Huawei', maLoai: 'HUAWEI', donGia: 7990000,
    anh: IMG.HUAWEI_GT5_PRO,
    mau: [{ id: 'titan', ten: 'Titanium Dây Kim Loại', hex: '#94A3B8', anh: IMG.HUAWEI_GT5_PRO }],
    phienBan: [{ id: 'pro-titan', dungLuong: '46mm Titanium Pro', gia: 7990000, giaGoc: 8990000 }],
    thongSo: { 'Chất liệu': 'Hợp kim Titanium cấp hàng không & Kính Sapphire', 'Lặn biển': 'Hỗ trợ lặn tự do độ sâu 40m', 'Golf': 'Bản đồ 15.000 sân golf toàn cầu' }
  },
  {
    ten: 'Vòng đeo tay thông minh Huawei Band 9 Màn Hình AMOLED Siêu Mỏng',
    nhanHieu: 'Huawei', maLoai: 'HUAWEI', donGia: 890000,
    anh: IMG.HUAWEI_BAND9,
    mau: [{ id: 'den', ten: 'Đen Huyền Bí', hex: '#18181B', anh: IMG.HUAWEI_BAND9 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Tiêu Chuẩn', gia: 890000, giaGoc: 1090000 }],
    thongSo: { 'Màn hình': '1.47" AMOLED rực rỡ', 'Độ dày': '8.99mm mỏng nhẹ 14g', 'Pin': 'Thời lượng pin 14 ngày' }
  },
  {
    ten: 'Đồng hồ thể thao Garmin Forerunner 165 Music GPS Chuyên Chạy Bộ',
    nhanHieu: 'Garmin', maLoai: 'GARMIN', donGia: 6990000,
    anh: IMG.GARMIN_FORERUNNER,
    mau: [{ id: 'den', ten: 'Đen Xám Thể Thao', hex: '#334155', anh: IMG.GARMIN_FORERUNNER }],
    phienBan: [{ id: 'music', dungLuong: 'Bản Music Nghe Nhạc Offline', gia: 6990000, giaGoc: 7990000 }],
    thongSo: { 'Màn hình': 'AMOLED 1.2" sắc nét', 'Pin': '11 ngày chế độ smartwatch, 19 giờ GPS', 'Chuyên dụng': 'Kế hoạch luyện tập Garmin Coach thích ứng' }
  },
  {
    ten: 'Đồng hồ thông minh Garmin Venu 3 AMOLED Pin 14 Ngày Nghe Gọi',
    nhanHieu: 'Garmin', maLoai: 'GARMIN', donGia: 11490000,
    anh: IMG.GARMIN_VENU3,
    mau: [{ id: 'trang', ten: 'Trắng Bạc Thanh Lịch', hex: '#F1F5F9', anh: IMG.GARMIN_VENU3 }],
    phienBan: [{ id: '45mm', dungLuong: '45mm Loa & Mic', gia: 11490000, giaGoc: 12990000 }],
    thongSo: { 'Đàm thoại': 'Tích hợp loa ngoài và micro nghe gọi trên đồng hồ', 'Theo dõi': 'Theo dõi giấc ngủ ngắn Sleep Coach, năng lượng Body Battery' }
  },
  {
    ten: 'Xiaomi Redmi Watch 4 Màn Hình AMOLED 1.97 inch Khung Nhôm',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 2190000,
    anh: IMG.REDMI_WATCH4,
    mau: [{ id: 'xam', ten: 'Xám Bạc', hex: '#9CA3AF', anh: IMG.REDMI_WATCH4 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Khung Nhôm', gia: 2190000, giaGoc: 2690000 }],
    thongSo: { 'Màn hình': '1.97" AMOLED 60Hz 600 nits', 'Pin': '20 ngày sử dụng bền bỉ', 'Đàm thoại': 'Hỗ trợ đàm thoại Bluetooth rõ ràng' }
  },
  {
    ten: 'Vòng đeo tay thông minh Xiaomi Smart Band 9 Khung Kim Loại Mới',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 890000,
    anh: IMG.XIAOMI_BAND9,
    mau: [{ id: 'den', ten: 'Đen Titan Midnight', hex: '#111827', anh: IMG.XIAOMI_BAND9 }],
    phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: 890000, giaGoc: 990000 }],
    thongSo: { 'Màn hình': '1.62" AMOLED 60Hz sáng 1200 nits', 'Cảm biến': 'Cảm biến SpO2 và nhịp tim nâng cấp độ chính xác 16%' }
  },
  {
    ten: 'Amazfit Balance Đồng Hồ Thông Minh Theo Dõi Sức Khỏe AI Toàn Diện',
    nhanHieu: 'Amazfit', maLoai: 'OTHER', donGia: 5490000,
    anh: IMG.AMAZFIT_BALANCE,
    mau: [{ id: 'xam', ten: 'Xám Midnight', hex: '#475569', anh: IMG.AMAZFIT_BALANCE }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Dây Vải', gia: 5490000, giaGoc: 6290000 }],
    thongSo: { 'AI': 'Zepp Flow trợ lý giọng nói AI', 'Đo lường': 'Phân tích thành phần cơ thể BIA (mỡ, cơ, xương)', 'Pin': '14 ngày' }
  },
  {
    ten: 'Amazfit T-Rex Ultra Đồng Hồ Siêu Bền Chuẩn Quân Đội Thép Không Gỉ',
    nhanHieu: 'Amazfit', maLoai: 'OTHER', donGia: 8990000,
    anh: IMG.AMAZFIT_TREX,
    mau: [{ id: 'den', ten: 'Đen Abyss Black', hex: '#1E293B', anh: IMG.AMAZFIT_TREX }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Siêu Bền', gia: 8990000, giaGoc: 9990000 }],
    thongSo: { 'Chống chịu': 'Hoạt động ở nhiệt độ cực đoan -30°C', 'Chống nước': '10ATM lặn tự do 30m', 'Định vị': 'GPS 6 hệ thống vệ tinh tần số kép' }
  },
  {
    ten: 'Đồng hồ thông minh Honor Watch 4 Viền Nhôm Màn AMOLED Hỗ Trợ e-SIM',
    nhanHieu: 'HONOR', maLoai: 'HONOR', donGia: 3190000,
    anh: IMG.AW_SE,
    mau: [{ id: 'den', ten: 'Đen Shadow Black', hex: '#1F2937', anh: IMG.AW_SE }],
    phienBan: [{ id: 'std', dungLuong: 'Khung Nhôm', gia: 3190000, giaGoc: 3990000 }],
    thongSo: { 'Màn hình': '1.75" AMOLED 60Hz 390x450', 'Pin': 'Thời lượng pin 14 ngày', 'Đàm thoại': 'Nghe gọi Bluetooth và theo dõi thể thao 85 môn' }
  },
  {
    ten: 'Đồng hồ thể thao Garmin Vivoactive 4S 40mm GPS Kính Gorilla Glass 3',
    nhanHieu: 'Garmin', maLoai: 'GARMIN', donGia: 5290000,
    anh: IMG.GARMIN_FORERUNNER,
    mau: [{ id: 'hong', ten: 'Hồng Gold Viền Thép', hex: '#FBCFE8', anh: IMG.GARMIN_FORERUNNER }],
    phienBan: [{ id: 'std', dungLuong: '40mm', gia: 5290000, giaGoc: 6990000 }],
    thongSo: { 'Mặt kính': 'Corning Gorilla Glass 3', 'Thời lượng pin': 'Tối đa 7 ngày chế độ smartwatch', 'Theo dõi': 'Năng lượng Body Battery, SpO2' }
  },
  {
    ten: 'Đồng hồ Garmin Vivomove Sport Hybrid Kim Cơ Kết Hợp Màn Cảm Ứng',
    nhanHieu: 'Garmin', maLoai: 'GARMIN', donGia: 4290000,
    anh: IMG.GARMIN_VENU3,
    mau: [{ id: 'trang', ten: 'Trắng Bạc Tinh Tế', hex: '#F8FAFC', anh: IMG.GARMIN_VENU3 }],
    phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: 4290000, giaGoc: 5190000 }],
    thongSo: { 'Thiết kế': 'Đồng hồ cơ cổ điển kết hợp màn hình ẩn cảm ứng', 'Đo đạc': 'Đo mức độ căng thẳng Stress 24/7, nhịp thở, lượng nước uống' }
  },
  {
    ten: 'Vòng đeo tay thông minh Huawei Band 8 Siêu Nhẹ Pin Trâu 14 Ngày',
    nhanHieu: 'Huawei', maLoai: 'HUAWEI', donGia: 690000,
    anh: IMG.HUAWEI_BAND9,
    mau: [{ id: 'den', ten: 'Đen Midnight Black', hex: '#111827', anh: IMG.HUAWEI_BAND9 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Tiêu Chuẩn', gia: 690000, giaGoc: 890000 }],
    thongSo: { 'Màn hình': '1.47" AMOLED sắc nét', 'Trọng lượng': '14g siêu êm nhẹ', 'Sạc': 'Sạc nhanh 45 phút đầy pin' }
  },
  {
    ten: 'Đồng hồ thông minh Kospet Tank M3 Siêu Bền Chống Nước 5ATM',
    nhanHieu: 'Kospet', maLoai: 'OTHER', donGia: 1490000,
    anh: IMG.AMAZFIT_TREX,
    mau: [{ id: 'den', ten: 'Đen Chiến Thuật', hex: '#18181B', anh: IMG.AMAZFIT_TREX }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Thép Chống Va Đập', gia: 1490000, giaGoc: 1990000 }],
    thongSo: { 'Chuẩn quân đội': 'MIL-STD-810H chịu lực va đập cực tốt', 'Pin': 'Pin dung lượng 480mAh dùng 15 ngày' }
  }
];

// 3. MÁY TÍNH BẢNG (25 sản phẩm)
const tabletProducts = [
  {
    ten: 'iPad Pro 11 inch M4 Wi-Fi 256GB - Màn hình Tandem OLED Chính Hãng',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 27990000,
    anh: IMG.IPAD_PRO_M4,
    mau: [
      { id: 'den', ten: 'Đen Không Gian Space Black', hex: '#1F2937', anh: IMG.IPAD_PRO_M4 },
      { id: 'bac', ten: 'Bạc Ánh Kim Silver', hex: '#E5E7EB', anh: IMG.IPAD_PRO_M4 }
    ],
    phienBan: [
      { id: '256gb', dungLuong: '256GB Wi-Fi', gia: 27990000, giaGoc: 29990000 },
      { id: '512gb', dungLuong: '512GB Wi-Fi', gia: 33990000, giaGoc: 35990000 }
    ],
    thongSo: { 'Chip': 'Apple M4 tiến trình 3nm thế hệ thứ 2', 'Màn hình': '11" Ultra Retina XDR Tandem OLED 120Hz ProMotion', 'Độ mỏng': '5.3mm siêu mỏng kỷ lục' }
  },
  {
    ten: 'iPad Air 11 inch M2 Wi-Fi 128GB - Chính Hãng Apple VN/A',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 15890000,
    anh: IMG.IPAD_AIR_M2,
    mau: [
      { id: 'starlight', ten: 'Ánh Sao Starlight', hex: '#F3F4F6', anh: IMG.IPAD_AIR_M2 },
      { id: 'space-gray', ten: 'Xám Không Gian', hex: '#6B7280', anh: IMG.IPAD_AIR_M2 },
      { id: 'purple', ten: 'Tím Mộng Mơ Purple', hex: '#C084FC', anh: IMG.IPAD_AIR_M2 },
      { id: 'blue', ten: 'Xanh Dương Blue', hex: '#60A5FA', anh: IMG.IPAD_AIR_M2 }
    ],
    phienBan: [
      { id: '128gb', dungLuong: '128GB Wi-Fi', gia: 15890000, giaGoc: 16990000 },
      { id: '256gb', dungLuong: '256GB Wi-Fi', gia: 18490000, giaGoc: 19990000 }
    ],
    thongSo: { 'Chip': 'Apple M2 8-core CPU, 10-core GPU', 'Hỗ trợ': 'Bút Apple Pencil Pro, Bàn phím Magic Keyboard', 'Màn hình': '11" Liquid Retina' }
  },
  {
    ten: 'iPad Gen 10 10.9 inch Wi-Fi 64GB - Thiết Kế Viền Đều Nhiều Màu Sắc',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 8990000,
    anh: IMG.IPAD_GEN10,
    mau: [
      { id: 'xanh', ten: 'Xanh Dương Năng Động', hex: '#3B82F6', anh: IMG.IPAD_GEN10 },
      { id: 'vang', ten: 'Vàng Rực Rỡ', hex: '#FBBF24', anh: IMG.IPAD_GEN10 },
      { id: 'hong', ten: 'Hồng Duyên Dáng', hex: '#F472B6', anh: IMG.IPAD_GEN10 },
      { id: 'bac', ten: 'Bạc Bạc Thanh Lịch', hex: '#E5E7EB', anh: IMG.IPAD_GEN10 }
    ],
    phienBan: [
      { id: '64gb', dungLuong: '64GB Wi-Fi', gia: 8990000, giaGoc: 9990000 },
      { id: '256gb', dungLuong: '256GB Wi-Fi', gia: 12490000, giaGoc: 13990000 }
    ],
    thongSo: { 'Chip': 'A14 Bionic', 'Màn hình': '10.9" Liquid Retina tràn viền', 'Cổng sạc': 'USB-C tiện lợi', 'Camera': 'Camera trước Ultra Wide 12MP căn giữa' }
  },
  {
    ten: 'iPad mini 7 8.3 inch Wi-Fi 128GB Chip A17 Pro Siêu Nhỏ Gọn',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 13490000,
    anh: IMG.IPAD_MINI7,
    mau: [
      { id: 'tim', ten: 'Tím Pastel', hex: '#D8B4FE', anh: IMG.IPAD_MINI7 },
      { id: 'xanh', ten: 'Xanh Space', hex: '#38BDF8', anh: IMG.IPAD_MINI7 }
    ],
    phienBan: [{ id: '128gb', dungLuong: '128GB Wi-Fi', gia: 13490000, giaGoc: 14990000 }],
    thongSo: { 'Chip': 'A17 Pro hỗ trợ Apple Intelligence', 'Màn hình': '8.3" Liquid Retina chống chói', 'Hỗ trợ': 'Apple Pencil Pro' }
  },
  {
    ten: 'Samsung Galaxy Tab S9 Ultra Wi-Fi 12GB/256GB Màn Hình Lớn 14.6 inch',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 21990000,
    anh: IMG.TAB_S9_ULTRA,
    mau: [{ id: 'den', ten: 'Đen Graphite', hex: '#1E293B', anh: IMG.TAB_S9_ULTRA }],
    phienBan: [{ id: '256gb', dungLuong: '12GB / 256GB S-Pen', gia: 21990000, giaGoc: 24990000 }],
    thongSo: { 'Màn hình': '14.6" Dynamic AMOLED 2X 120Hz', 'Vi xử lý': 'Snapdragon 8 Gen 2 for Galaxy', 'Kháng nước': 'IP68 cả máy và bút S-Pen' }
  },
  {
    ten: 'Samsung Galaxy Tab S9 FE Wi-Fi 128GB Kèm Bút S-Pen Cảm Ứng',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 8490000,
    anh: IMG.TAB_S9_FE,
    mau: [
      { id: 'xanh', ten: 'Xanh Nhiệt Đới Mint', hex: '#6EE7B7', anh: IMG.TAB_S9_FE },
      { id: 'xam', ten: 'Xám Tự Nhiên Gray', hex: '#6B7280', anh: IMG.TAB_S9_FE }
    ],
    phienBan: [{ id: '128gb', dungLuong: '6GB / 128GB Kèm Bút', gia: 8490000, giaGoc: 9990000 }],
    thongSo: { 'Màn hình': '10.9" 90Hz mượt mà', 'Pin': '8000mAh sạc nhanh 45W', 'Chuẩn bền': 'Kháng nước bụi chuẩn IP68' }
  },
  {
    ten: 'Samsung Galaxy Tab A9+ 11 inch Wi-Fi 64GB 4 Loa Âm Vòm Dolby',
    nhanHieu: 'Samsung', maLoai: 'SAMSUNG', donGia: 4690000,
    anh: IMG.TAB_A9_PLUS,
    mau: [{ id: 'bac', ten: 'Bạc Ánh Trăng Silver', hex: '#E2E8F0', anh: IMG.TAB_A9_PLUS }],
    phienBan: [{ id: '64gb', dungLuong: '4GB / 64GB', gia: 4690000, giaGoc: 5490000 }],
    thongSo: { 'Màn hình': '11.0" 90Hz sắc nét', 'Âm thanh': 'Hệ thống 4 loa vòm Dolby Atmos', 'Chip': 'Snapdragon 695 5G' }
  },
  {
    ten: 'Xiaomi Pad 6 8GB/256GB Snapdragon 870 Màn Hình 144Hz 2.8K',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 7290000,
    anh: IMG.XIAOMI_PAD6,
    mau: [
      { id: 'xam', ten: 'Xám Không Gian Gravity Gray', hex: '#4B5563', anh: IMG.XIAOMI_PAD6 },
      { id: 'vang', ten: 'Vàng Gold Champagne', hex: '#FDE047', anh: IMG.XIAOMI_PAD6 }
    ],
    phienBan: [{ id: '8-256', dungLuong: '8GB / 256GB', gia: 7290000, giaGoc: 8490000 }],
    thongSo: { 'Màn hình': '11" 2.8K siêu nét 144Hz 7 mức điều chỉnh', 'Chip': 'Snapdragon 870 flagship', 'Pin': '8840mAh sạc nhanh 33W' }
  },
  {
    ten: 'Xiaomi Redmi Pad Pro 12.1 inch 8GB/128GB Màn Hình Lớn Pin 10.000mAh',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 6490000,
    anh: IMG.REDMI_PAD_PRO,
    mau: [{ id: 'xanh', ten: 'Xanh Đại Dương Ocean Blue', hex: '#38BDF8', anh: IMG.REDMI_PAD_PRO }],
    phienBan: [{ id: '8-128', dungLuong: '8GB / 128GB', gia: 6490000, giaGoc: 7490000 }],
    thongSo: { 'Màn hình': '12.1" 2.5K 120Hz tỷ lệ 16:10', 'Chip': 'Snapdragon 7s Gen 2 tiến trình 4nm', 'Pin': '10.000mAh dùng 3 ngày' }
  },
  {
    ten: 'Redmi Pad SE Wi-Fi 6GB/128GB Loa Dolby Atmos Nhỏ Gọn Học Tập',
    nhanHieu: 'Xiaomi', maLoai: 'XIAOMI', donGia: 3790000,
    anh: IMG.REDMI_PAD_SE,
    mau: [{ id: 'xanh', ten: 'Xanh Lá Tươi Mint Green', hex: '#86EFAC', anh: IMG.REDMI_PAD_SE }],
    phienBan: [{ id: '6-128', dungLuong: '6GB / 128GB', gia: 3790000, giaGoc: 4490000 }],
    thongSo: { 'Màn hình': '11.0" FHD+ 90Hz bảo vệ mắt chứng nhận TÜV Rheinland', 'Pin': '8000mAh', 'Loa': '4 loa âm thanh nổi vòm' }
  },
  {
    ten: 'Máy tính bảng REDMAGIC Nova Gaming 12GB/256GB Snapdragon 8 Gen 3',
    nhanHieu: 'Nubia', maLoai: 'NUBIA', donGia: 12990000,
    anh: IMG.REDMAGIC_NOVA_TAB,
    mau: [{ id: 'den', ten: 'Đen Quạt LED RGB Gaming', hex: '#0F172A', anh: IMG.REDMAGIC_NOVA_TAB }],
    phienBan: [{ id: '12-256', dungLuong: '12GB / 256GB', gia: 12990000, giaGoc: 14990000 }],
    thongSo: { 'Tản nhiệt': 'Quạt cơ học tuần hoàn tốc độ 20.000 vòng/phút', 'Chip': 'Snapdragon 8 Gen 3 Leading Version', 'Màn hình': '10.9" 2.8K 144Hz' }
  },
  {
    ten: 'Máy tính bảng Lenovo Tab M11 11 inch 4GB/128GB Kèm Bút Cảm Ứng Tab Pen',
    nhanHieu: 'Lenovo', maLoai: 'LENOVO', donGia: 4190000,
    anh: IMG.LENOVO_TAB_M11,
    mau: [{ id: 'xam', ten: 'Xám Luna Grey', hex: '#4B5563', anh: IMG.LENOVO_TAB_M11 }],
    phienBan: [{ id: '4-128', dungLuong: '4GB / 128GB + Bút Tab Pen', gia: 4190000, giaGoc: 4990000 }],
    thongSo: { 'Màn hình': '11.0" WUXGA 90Hz', 'Kèm': 'Bút cảm ứng viết vẽ Lenovo Tab Pen độ nhạy 4096 mức lực', 'Loa': '4 loa tinh chỉnh bởi Dolby Atmos' }
  },
  {
    ten: 'TCL TAB 11 Wi-Fi 4GB/128GB Màn Hình NXTVISION Chăm Sóc Mắt',
    nhanHieu: 'TCL', maLoai: 'TCL', donGia: 3190000,
    anh: IMG.TCL_TAB11,
    mau: [{ id: 'xam', ten: 'Xám Đậm Dark Gray', hex: '#374151', anh: IMG.TCL_TAB11 }],
    phienBan: [{ id: '4-128', dungLuong: '4GB / 128GB', gia: 3190000, giaGoc: 3990000 }],
    thongSo: { 'Màn hình': '10.95" 2K công nghệ hiển thị NXTVISION', 'Pin': '8000mAh hỗ trợ sạc ngược cho thiết bị khác' }
  },
  {
    ten: 'Nokia T21 LTE 4GB/128GB Vỏ Nhôm Nguyên Khối Bền Bỉ Hỗ Trợ 4G',
    nhanHieu: 'Nokia', maLoai: 'NOKIA', donGia: 3590000,
    anh: IMG.NOKIA_T21_TAB,
    mau: [{ id: 'xam', ten: 'Xám Than Chì Charcoal', hex: '#334155', anh: IMG.NOKIA_T21_TAB }],
    phienBan: [{ id: '4g-lte', dungLuong: '4GB / 128GB Có Sim 4G', gia: 3590000, giaGoc: 4290000 }],
    thongSo: { 'Thiết kế': 'Vỏ nhôm nguyên khối bền chắc chuẩn châu Âu', 'Kết nối': 'Hỗ trợ khay SIM 4G nghe gọi, lướt web mọi nơi' }
  },
  {
    ten: 'Bút cảm ứng Apple Pencil Pro Dành Cho iPad M4 / iPad Air M2',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 3290000,
    anh: IMG.IPAD_PRO_M4,
    mau: [{ id: 'trang', ten: 'Trắng Apple White', hex: '#FFFFFF', anh: IMG.IPAD_PRO_M4 }],
    phienBan: [{ id: 'std', dungLuong: 'Bút Cảm Ứng Pro', gia: 3290000, giaGoc: 3490000 }],
    thongSo: { 'Cảm ứng lực': 'Cảm biến bóp nhả Barrel roll, phản hồi xúc giác Haptic', 'Tìm kiếm': 'Tích hợp mạng lưới Tìm Apple Find My' }
  }
];

// 4. ÂM THANH ĐỈNH CAO (25 sản phẩm)
const audioProducts = [
  {
    ten: 'Tai nghe AirPods Pro 2 USB-C Chính Hãng Apple MagSafe Chống Ồn',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 5390000,
    anh: IMG.AIRPODS_PRO_2,
    mau: [{ id: 'trang', ten: 'Trắng Apple', hex: '#FFFFFF', anh: IMG.AIRPODS_PRO_2 }],
    phienBan: [{ id: 'usbc-magsafe', dungLuong: 'Hộp Sạc Type-C MagSafe', gia: 5390000, giaGoc: 6190000 }],
    thongSo: { 'Chip': 'Apple H2 khử tiếng ồn chủ động gấp 2 lần', 'Âm thanh': 'Âm thanh không gian Spatial Audio cá nhân hóa', 'Kháng nước': 'IP54 cho cả tai nghe và hộp sạc' }
  },
  {
    ten: 'Tai nghe chụp tai Apple AirPods Max USB-C Chống Ồn Chủ Động',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 12990000,
    anh: IMG.AIRPODS_MAX,
    mau: [
      { id: 'midnight', ten: 'Đen Midnight', hex: '#1E293B', anh: IMG.AIRPODS_MAX },
      { id: 'starlight', ten: 'Ánh Sao Starlight', hex: '#F3F4F6', anh: IMG.AIRPODS_MAX }
    ],
    phienBan: [{ id: 'usbc', dungLuong: 'Cổng Type-C Mới', gia: 12990000, giaGoc: 13990000 }],
    thongSo: { 'Driver': 'Driver dynamic 40mm do Apple tự thiết kế', 'Công nghệ': 'Chế độ Xuyên Âm Transparency mode, Khử tiếng ồn ANC đỉnh cao' }
  },
  {
    ten: 'Tai nghe chống ồn Sony WH-1000XM5 Hi-Res Audio Đỉnh Cao Thế Giới',
    nhanHieu: 'Sony', maLoai: 'SONY', donGia: 7690000,
    anh: IMG.SONY_WH1000XM5,
    mau: [
      { id: 'den', ten: 'Đen Nhám Chống Bám Vân Tay', hex: '#18181B', anh: IMG.SONY_WH1000XM5 },
      { id: 'bac', ten: 'Bạc Khói Silver', hex: '#D1D5DB', anh: IMG.SONY_WH1000XM5 }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Hi-Res Wireless LDAC', gia: 7690000, giaGoc: 8990000 }],
    thongSo: { 'Bộ xử lý': '2 vi xử lý V1 + QN1 điều khiển 8 micro chống ồn', 'Pin': '30 giờ nghe nhạc liên tục, sạc nhanh 3 phút nghe 3 giờ' }
  },
  {
    ten: 'Tai nghe True Wireless Sony WF-1000XM5 Khử Ồn Đỉnh Chóp',
    nhanHieu: 'Sony', maLoai: 'SONY', donGia: 5490000,
    anh: IMG.SONY_WF1000XM5,
    mau: [{ id: 'den', ten: 'Đen Piano', hex: '#111827', anh: IMG.SONY_WF1000XM5 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Đen Nhám', gia: 5490000, giaGoc: 6990000 }],
    thongSo: { 'Driver': 'Dynamic Driver X tái tạo âm trầm sâu lắng', 'Micro': 'Cảm biến dẫn truyền qua xương thu âm đàm thoại trong trẻo' }
  },
  {
    ten: 'Loa di động không dây Sony ULT FIELD 1 Siêu Trầm Bass Booming',
    nhanHieu: 'Sony', maLoai: 'SONY', donGia: 2890000,
    anh: IMG.SONY_ULT_FIELD1,
    mau: [{ id: 'den', ten: 'Đen Mạnh Mẽ', hex: '#0F172A', anh: IMG.SONY_ULT_FIELD1 }],
    phienBan: [{ id: 'std', dungLuong: 'Nút Bấm ULT BASS', gia: 2890000, giaGoc: 3490000 }],
    thongSo: { 'Tính năng': 'Nút bấm ULT kích hoạt âm bass bùng nổ', 'Kháng nước': 'IP67 chống bụi nước và chống sốc va đập', 'Pin': '12 giờ' }
  },
  {
    ten: 'Tai nghe Bluetooth Marshall Major IV Cổ Điển Pin 80 Giờ',
    nhanHieu: 'Marshall', maLoai: 'MARSHALL', donGia: 3190000,
    anh: IMG.MARSHALL_MAJOR4,
    mau: [{ id: 'den', ten: 'Đen Vân Da Cổ Điển', hex: '#18181B', anh: IMG.MARSHALL_MAJOR4 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Dây Da Gập Gọn', gia: 3190000, giaGoc: 3690000 }],
    thongSo: { 'Pin': 'Thời lượng pin vượt trội trên 80 giờ', 'Sạc': 'Hỗ trợ sạc không dây tiện lợi', 'Núm xoay': 'Núm điều khiển đa hướng Control Knob' }
  },
  {
    ten: 'Loa di động Bluetooth Marshall Emberton II Chống Nước IP67',
    nhanHieu: 'Marshall', maLoai: 'MARSHALL', donGia: 3990000,
    anh: IMG.MARSHALL_EMBERTON2,
    mau: [{ id: 'den-dong', ten: 'Đen Viền Đồng Black & Brass', hex: '#78350F', anh: IMG.MARSHALL_EMBERTON2 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Vỏ Da Đúc', gia: 3990000, giaGoc: 4690000 }],
    thongSo: { 'Âm thanh': 'Công nghệ âm thanh đa hướng True Stereophonic 360 độ', 'Pin': '30+ giờ chơi nhạc, sạc nhanh' }
  },
  {
    ten: 'Loa Bluetooth Marshall Stanmore III Âm Thanh Phòng Khách Đẳng Cấp',
    nhanHieu: 'Marshall', maLoai: 'MARSHALL', donGia: 8690000,
    anh: IMG.MARSHALL_STANMORE3,
    mau: [
      { id: 'den', ten: 'Đen Vintage', hex: '#111827', anh: IMG.MARSHALL_STANMORE3 },
      { id: 'kem', ten: 'Kem Vintage Cream', hex: '#FEF3C7', anh: IMG.MARSHALL_STANMORE3 }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Công Suất 80W RMS', gia: 8690000, giaGoc: 9990000 }],
    thongSo: { 'Công suất': '80W RMS khuấy động không gian', 'Kết nối': 'Bluetooth 5.2, Cổng RCA, Cổng AUX 3.5mm analog' }
  },
  {
    ten: 'Loa Bluetooth JBL Charge 5 Siêu Trầm Bass Kháng Nước IP67',
    nhanHieu: 'JBL', maLoai: 'JBL', donGia: 3490000,
    anh: IMG.JBL_CHARGE5,
    mau: [
      { id: 'den', ten: 'Đen Thể Thao', hex: '#1E293B', anh: IMG.JBL_CHARGE5 },
      { id: 'xanh', ten: 'Xanh Rằn Ri Camo', hex: '#365314', anh: IMG.JBL_CHARGE5 }
    ],
    phienBan: [{ id: 'std', dungLuong: 'Pin 20 Giờ Sạc Ngược Điện Thoại', gia: 3490000, giaGoc: 3990000 }],
    thongSo: { 'Củ loa': 'Củ loa dạng racetrack riêng biệt cùng loa tweeter độc lập', 'PartyBoost': 'Kết nối hàng trăm loa JBL tạo bữa tiệc âm thanh' }
  },
  {
    ten: 'Loa Bluetooth JBL Flip 6 Nhỏ Gọn Chống Nước Sâu Uy Lực',
    nhanHieu: 'JBL', maLoai: 'JBL', donGia: 2490000,
    anh: IMG.JBL_FLIP6,
    mau: [{ id: 'do', ten: 'Đỏ Rực Rỡ Fiesta Red', hex: '#DC2626', anh: IMG.JBL_FLIP6 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Đỏ Cá Tính', gia: 2490000, giaGoc: 2990000 }],
    thongSo: { 'Âm thanh': 'Hệ thống loa 2 chiều mang chất âm JBL Original Pro Sound', 'Pin': '12 giờ phát nhạc liên tục' }
  },
  {
    ten: 'Loa Bluetooth Harman Kardon Onyx Studio 8 Âm Thanh Đẳng Cấp Mỹ',
    nhanHieu: 'Harman Kardon', maLoai: 'OTHER', donGia: 5490000,
    anh: IMG.HARMAN_ONYX8,
    mau: [{ id: 'den', ten: 'Đen Quai Nhôm Đúc', hex: '#1E293B', anh: IMG.HARMAN_ONYX8 }],
    phienBan: [{ id: 'std', dungLuong: 'Công Suất 50W RMS', gia: 5490000, giaGoc: 6490000 }],
    thongSo: { 'Chất liệu': 'Vải dệt cao cấp và tay cầm nhôm tái chế sang trọng', 'Hiệu chỉnh': 'Tự động cân chỉnh âm thanh theo kích thước phòng' }
  },
  {
    ten: 'Loa Bluetooth Alpha Works Classic V21 Da Cổ Điển Âm Sâu',
    nhanHieu: 'Alpha Works', maLoai: 'OTHER', donGia: 2690000,
    anh: IMG.ALPHA_WORKS_V21,
    mau: [{ id: 'nau', ten: 'Nâu Da Bò Retro', hex: '#92400E', anh: IMG.ALPHA_WORKS_V21 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Retro Vintage', gia: 2690000, giaGoc: 2990000 }],
    thongSo: { 'Thiết kế': 'Thùng gỗ bọc da cao cấp, mặt lưới kim loại cổ điển', 'Pin': '5000mAh chơi nhạc 8 giờ liên tục' }
  },
  {
    ten: 'Tai nghe Sennheiser ACCENTUM Wireless Pin 50 Giờ Chống Ồn',
    nhanHieu: 'Sennheiser', maLoai: 'OTHER', donGia: 3290000,
    anh: IMG.SENNHEISER_ACCENTUM,
    mau: [{ id: 'den', ten: 'Đen Mờ Nhã Nhặn', hex: '#1F2937', anh: IMG.SENNHEISER_ACCENTUM }],
    phienBan: [{ id: 'std', dungLuong: 'Pin 50 Giờ', gia: 3290000, giaGoc: 3990000 }],
    thongSo: { 'Driver': 'Driver transducer 37mm đậm chất Đức', 'Pin': 'Pin siêu khủng 50 giờ, sạc 10 phút dùng 5 giờ' }
  },
  {
    ten: 'Tai nghe Bluetooth True Wireless Huawei FreeBuds SE 2 Siêu Nhẹ',
    nhanHieu: 'Huawei', maLoai: 'HUAWEI', donGia: 650000,
    anh: IMG.HUAWEI_FREEBUDS_SE2,
    mau: [{ id: 'trang', ten: 'Trắng Gốm Ceramic White', hex: '#FFFFFF', anh: IMG.HUAWEI_FREEBUDS_SE2 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản Màu Trắng', gia: 650000, giaGoc: 890000 }],
    thongSo: { 'Trọng lượng': '3.8g mỗi bên đeo như không đeo', 'Pin': '40 giờ tổng khi dùng kèm hộp sạc' }
  },
  {
    ten: 'Tai Nghe Bluetooth HONOR CHOICE Earbuds X5 Chống Ồn Chủ Động',
    nhanHieu: 'HONOR', maLoai: 'HONOR', donGia: 590000,
    anh: IMG.HONOR_EARBUDS_X5,
    mau: [{ id: 'trang', ten: 'Trắng Sữa', hex: '#F9FAFB', anh: IMG.HONOR_EARBUDS_X5 }],
    phienBan: [{ id: 'std', dungLuong: 'Bản ANC', gia: 590000, giaGoc: 790000 }],
    thongSo: { 'Khử ồn': 'Khử tiếng ồn chủ động 30dB ANC', 'Pin': '35 giờ sử dụng liên tục' }
  }
];

// 5. PHỤ KIỆN HIỆN ĐẠI (25 sản phẩm)
const accessoryProducts = [
  {
    ten: 'Củ sạc nhanh Apple 20W Type-C Chính Hãng VN/A Tiết Kiệm Pin',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 490000,
    anh: IMG.APPLE_20W,
    mau: [{ id: 'trang', ten: 'Trắng Apple', hex: '#FFFFFF', anh: IMG.APPLE_20W }],
    phienBan: [{ id: '20w-usbc', dungLuong: '20W Chuẩn Type-C', gia: 490000, giaGoc: 590000 }],
    thongSo: { 'Công suất': '20W Power Delivery chuẩn Apple', 'An toàn': 'Tự ngắt nguồn khi pin đạt 80% bảo vệ pin' }
  },
  {
    ten: 'Cáp sạc C to C Apple Woven 1m Siêu Bền Dù Dành Cho iPhone 15/16',
    nhanHieu: 'Apple', maLoai: 'APPLE', donGia: 390000,
    anh: IMG.APPLE_CABLE_C,
    mau: [{ id: 'trang', ten: 'Trắng Dây Dù Bọc Vải', hex: '#F3F4F6', anh: IMG.APPLE_CABLE_C }],
    phienBan: [{ id: '1m', dungLuong: 'Chiều dài 1m', gia: 390000, giaGoc: 550000 }],
    thongSo: { 'Chất liệu': 'Dây bọc sợi dệt Woven chống đứt gãy gập', 'Tương thích': 'Sạc nhanh tối đa 60W cho iPhone, iPad, MacBook' }
  },
  {
    ten: 'Củ sạc nhanh Anker GaNPrime 67W 3 Cổng Siêu Nhỏ Gọn Tiện Lợi',
    nhanHieu: 'Anker', maLoai: 'ANKER', donGia: 990000,
    anh: IMG.ANKER_67W,
    mau: [{ id: 'den', ten: 'Đen Kim Loại', hex: '#1E293B', anh: IMG.ANKER_67W }],
    phienBan: [{ id: '67w-3port', dungLuong: '67W (2 Type-C + 1 USB-A)', gia: 990000, giaGoc: 1290000 }],
    thongSo: { 'Công nghệ': 'GaNPrime sạc cùng lúc 3 thiết bị Laptop, Phone, Tablet', 'Bảo vệ': 'ActiveShield 2.0 kiểm soát nhiệt 3 triệu lần/ngày' }
  },
  {
    ten: 'Pin dự phòng Anker MagGo Qi2 10.000mAh Không Dây Hút Nam Châm Màn Hình LED',
    nhanHieu: 'Anker', maLoai: 'ANKER', donGia: 1290000,
    anh: IMG.ANKER_MAGGO,
    mau: [
      { id: 'den', ten: 'Đen Nhám', hex: '#18181B', anh: IMG.ANKER_MAGGO },
      { id: 'trang', ten: 'Trắng Sữa', hex: '#F9FAFB', anh: IMG.ANKER_MAGGO }
    ],
    phienBan: [{ id: '10000mah', dungLuong: '10.000mAh Qi2 15W', gia: 1290000, giaGoc: 1590000 }],
    thongSo: { 'Chứng nhận': 'Chuẩn Qi2 sạc không dây 15W hít nam châm MagSafe siêu mạnh', 'Màn hình': 'Màn hình màu thông minh hiển thị thời gian sạc và pin' }
  },
  {
    ten: 'Củ sạc mophie Essentials PD 30W 1A1C Nhỏ Gọn Siêu Nhanh',
    nhanHieu: 'Mophie', maLoai: 'OTHER', donGia: 390000,
    anh: IMG.MOPHIE_30W,
    mau: [{ id: 'trang', ten: 'Trắng Tinh Tế', hex: '#FFFFFF', anh: IMG.MOPHIE_30W }],
    phienBan: [{ id: '30w', dungLuong: '30W 2 Cổng 1C1A', gia: 390000, giaGoc: 490000 }],
    thongSo: { 'Công suất': '30W PD sạc iPhone đầy 50% trong 25 phút', 'An toàn': 'Đạt chuẩn chứng nhận an toàn chống quá nhiệt toàn cầu' }
  },
  {
    ten: 'Chuột không dây Logitech MX Master 3S Công Thái Học Chống Mỏi Tay',
    nhanHieu: 'Logitech', maLoai: 'OTHER', donGia: 2190000,
    anh: IMG.LOGITECH_MX_MASTER3S,
    mau: [{ id: 'den', ten: 'Đen Graphite Chuyên Nghiệp', hex: '#1F2937', anh: IMG.LOGITECH_MX_MASTER3S }],
    phienBan: [{ id: 'std', dungLuong: 'Cảm Biến 8000 DPI Mọi Bề Mặt', gia: 2190000, giaGoc: 2590000 }],
    thongSo: { 'Cuộn chuột': 'Bánh lăn MagSpeed cuộn 1000 dòng trong 1 giây', 'Click': 'Quiet Clicks giảm 90% tiếng ồn nhấp chuột' }
  },
  {
    ten: 'Bàn phím cơ không dây Keychron K2 Pro QMK/VIA Hot-swap Đèn RGB',
    nhanHieu: 'Keychron', maLoai: 'OTHER', donGia: 2390000,
    anh: IMG.KEYCHRON_K2_PRO,
    mau: [{ id: 'den', ten: 'Khung Nhôm Switch Red', hex: '#111827', anh: IMG.KEYCHRON_K2_PRO }],
    phienBan: [{ id: 'k2-pro-rgb', dungLuong: 'Bản Khung Nhôm Led RGB', gia: 2390000, giaGoc: 2790000 }],
    thongSo: { 'Layout': '75% nhỏ gọn 84 phím bấm', 'Hot-swap': 'Thay thế switch nhanh không cần hàn', 'Pin': 'Pin 4000mAh dùng 300 giờ' }
  },
  {
    ten: 'Ốp lưng iPhone 16 Pro Max UAG Monarch Chống Sốc Chuẩn Quân Đội',
    nhanHieu: 'UAG', maLoai: 'OTHER', donGia: 1490000,
    anh: IMG.UAG_MONARCH_IP16,
    mau: [{ id: 'carbon', ten: 'Sợi Carbon Đen Men', hex: '#18181B', anh: IMG.UAG_MONARCH_IP16 }],
    phienBan: [{ id: 'magsafe', dungLuong: 'Hỗ Trợ MagSafe', gia: 1490000, giaGoc: 1790000 }],
    thongSo: { 'Cấu trúc': '5 lớp bảo vệ kim loại, da thật và sợi carbon', 'Rơi rớt': 'Bảo vệ thiết bị rơi từ độ cao 7.6 mét' }
  },
  {
    ten: 'Đế sạc không dây 3 in 1 Belkin BoostCharge Pro MagSafe 15W',
    nhanHieu: 'Belkin', maLoai: 'OTHER', donGia: 2990000,
    anh: IMG.BELKIN_3IN1,
    mau: [{ id: 'trang', ten: 'Trắng Phong Cách Tối Giản', hex: '#FFFFFF', anh: IMG.BELKIN_3IN1 }],
    phienBan: [{ id: '3in1-15w', dungLuong: 'Sạc cùng lúc iPhone, Apple Watch, AirPods', gia: 2990000, giaGoc: 3490000 }],
    thongSo: { 'Chứng nhận': 'Chứng nhận chính thức MFi sạc nhanh MagSafe 15W', 'Chất liệu': 'Thép không gỉ cao cấp' }
  },
  {
    ten: 'Hub Chuyển Đổi Baseus Metal Gleam Series 7 in 1 Cổng HDMI 4K PD 100W',
    nhanHieu: 'Baseus', maLoai: 'OTHER', donGia: 690000,
    anh: IMG.BASEUS_HUB_7IN1,
    mau: [{ id: 'xam', ten: 'Xám Không Gian Hợp Kim Nhôm', hex: '#4B5563', anh: IMG.BASEUS_HUB_7IN1 }],
    phienBan: [{ id: '7in1', dungLuong: '7 Cổng Kết Nối', gia: 690000, giaGoc: 890000 }],
    thongSo: { 'Cổng xuất': 'HDMI 4K 60Hz, 3 x USB 3.0, Khe cắm thẻ nhớ SD/TF, Cổng Type-C PD 100W' }
  },
  {
    ten: 'Sạc Anker PowerPort III 1C 20W A2149 Chân Gập Nhỏ Gọn Siêu Bền',
    nhanHieu: 'Anker', maLoai: 'ANKER', donGia: 290000,
    anh: IMG.ANKER_20W,
    mau: [{ id: 'trang', ten: 'Trắng Sạch', hex: '#F9FAFB', anh: IMG.ANKER_20W }],
    phienBan: [{ id: '20w', dungLuong: '20W PD Chân Gập', gia: 290000, giaGoc: 350000 }],
    thongSo: { 'Kích thước': 'Nhỏ tương đương củ sạc 5W cũ của Apple', 'Công suất': '20W sạc nhanh toàn diện' }
  },
  {
    ten: 'Pin sạc dự phòng Innostyle PowerMag 2-in-1 10.000mAh Hít Nam Châm Chân Đỡ',
    nhanHieu: 'Innostyle', maLoai: 'OTHER', donGia: 790000,
    anh: IMG.INNOSTYLE_POWERMAG,
    mau: [{ id: 'xanh', ten: 'Xanh Titan Xám', hex: '#64748B', anh: IMG.INNOSTYLE_POWERMAG }],
    phienBan: [{ id: '10000mah', dungLuong: '10.000mAh Có Giá Đỡ', gia: 790000, giaGoc: 990000 }],
    thongSo: { 'Chân đế': 'Tích hợp chân chống gập mở xem phim rảnh tay', 'Sạc kép': 'Sạc không dây MagSafe 15W + Cáp sạc PD 20W' }
  },
  {
    ten: 'Kính cường lực KingKong Chống Nhìn Trộm Cao Cấp Cho Mọi Dòng Máy',
    nhanHieu: 'KingKong', maLoai: 'OTHER', donGia: 150000,
    anh: IMG.APPLE_CABLE_C,
    mau: [{ id: 'den', ten: 'Chống Nhìn Trộm 28 Độ', hex: '#000000', anh: IMG.APPLE_CABLE_C }],
    phienBan: [{ id: 'std', dungLuong: 'Kính Cường Lực 9D', gia: 150000, giaGoc: 220000 }],
    thongSo: { 'Bảo mật': 'Góc nhìn nghiêng 28 độ chống soi nhìn trộm tin nhắn', 'Độ cứng': 'Chuẩn độ cứng 9H chống trầy xước va đập tối đa' }
  },
  {
    ten: 'Ốp lưng Spigen Liquid Air Chống Sốc Vân Lục Giác Nhập Khẩu Hàn Quốc',
    nhanHieu: 'Spigen', maLoai: 'OTHER', donGia: 350000,
    anh: IMG.UAG_MONARCH_IP16,
    mau: [{ id: 'den', ten: 'Đen Mờ Vân Lục Giác', hex: '#18181B', anh: IMG.UAG_MONARCH_IP16 }],
    phienBan: [{ id: 'std', dungLuong: 'Ốp Dẻo Chống Sốc', gia: 350000, giaGoc: 450000 }],
    thongSo: { 'Công nghệ': 'Đệm khí Air Cushion độc quyền tại 4 góc chống sốc', 'Xuất xứ': 'Sản xuất trực tiếp tại Hàn Quốc' }
  },
  {
    ten: 'Cáp sạc Baseus Crystal Shine C to C 100W Sạc Nhanh Laptop Dài 2m',
    nhanHieu: 'Baseus', maLoai: 'OTHER', donGia: 160000,
    anh: IMG.APPLE_CABLE_C,
    mau: [{ id: 'xanh', ten: 'Xanh Đậm Dây Bện Dù', hex: '#1E3A8A', anh: IMG.APPLE_CABLE_C }],
    phienBan: [{ id: '2m', dungLuong: 'Chiều Dài 2m 100W', gia: 160000, giaGoc: 220000 }],
    thongSo: { 'Công suất': 'Chịu tải 100W 5A sạc nhanh MacBook Pro và laptop Windows', 'Đầu cắm': 'Hợp kim nhôm bóng sáng chống gỉ sét' }
  }
];

// Helper để chuyển format
function mapToFinal(p, maNhom, tenNhom, idx) {
  return {
    maHang: `MH${String(100 + idx).padStart(3, '0')}`,
    tenHang: p.ten,
    nhanHieu: p.nhanHieu,
    maLoai: p.maLoai,
    maNhom: maNhom,
    tenNhom: tenNhom,
    donGia: p.donGia,
    giaGoc: p.giaGoc || Math.round(p.donGia * 1.08),
    anhChinh: p.anh || FALLBACK_IMG[maNhom],
    mauSac: p.mau || [{ id: 'std', ten: 'Tiêu chuẩn', hex: '#1E293B', anh: p.anh || FALLBACK_IMG[maNhom] }],
    phienBan: p.phienBan || [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: p.donGia, giaGoc: p.giaGoc || Math.round(p.donGia * 1.08) }],
    thongSo: p.thongSo || {}
  };
}

// 6. TẢI DỮ LIỆU ĐIỆN THOẠI CHÍNH HÃNG VÀ TỔNG HỢP 200 MẶT HÀNG
const allFinal = [];

// Thêm 25 Laptops & Màn hình (MH101 -> MH125)
laptopProducts.forEach(p => {
  allFinal.push(mapToFinal(p, 'LAPTOP', 'Laptop & Màn hình', allFinal.length + 1));
});

// Thêm 20 Đồng hồ thông minh (MH126 -> MH145)
watchProducts.forEach(p => {
  allFinal.push(mapToFinal(p, 'DONGHO', 'Đồng hồ thông minh', allFinal.length + 1));
});

// Thêm 15 Máy tính bảng (MH146 -> MH160)
tabletProducts.forEach(p => {
  allFinal.push(mapToFinal(p, 'TABLET', 'Máy tính bảng', allFinal.length + 1));
});

// Thêm 15 Âm thanh đỉnh cao (MH161 -> MH175)
audioProducts.forEach(p => {
  allFinal.push(mapToFinal(p, 'AMTHANH', 'Âm thanh', allFinal.length + 1));
});

// Thêm 15 Phụ kiện hiện đại (MH176 -> MH190)
accessoryProducts.forEach(p => {
  allFinal.push(mapToFinal(p, 'PHUKIEN', 'Phụ kiện', allFinal.length + 1));
});

// Còn lại: 110 Điện thoại đa dạng các hãng (MH191 -> MH300) để hoàn thành đúng 200 sản phẩm
// Load điện thoại từ existing products-meta.json nếu có
const existingPath = path.join(__dirname, '..', 'frontend', 'products-meta.json');
let existingPhones = [];
if (fs.existsSync(existingPath)) {
  try {
    const old = JSON.parse(fs.readFileSync(existingPath, 'utf8'));
    existingPhones = Object.values(old).filter(p => p.maNhom === 'DIENTHOAI' || !p.maNhom);
  } catch {}
}

let phoneIdx = 0;
while (allFinal.length < 200) {
  if (phoneIdx < existingPhones.length) {
    const ep = existingPhones[phoneIdx];
    allFinal.push({
      ...ep,
      maHang: `MH${String(100 + allFinal.length + 1).padStart(3, '0')}`,
      maNhom: 'DIENTHOAI',
      tenNhom: 'Điện thoại'
    });
  } else {
    // Generate phone fallback
    const idNum = allFinal.length + 1;
    allFinal.push({
      maHang: `MH${String(100 + idNum).padStart(3, '0')}`,
      tenHang: `Điện thoại Flagship Serie ${idNum} - Chính Hãng`,
      nhanHieu: 'Samsung',
      maLoai: 'SAMSUNG',
      maNhom: 'DIENTHOAI',
      tenNhom: 'Điện thoại',
      donGia: 12990000,
      giaGoc: 14990000,
      anhChinh: IMG.GALAXY_S24_ULTRA,
      mauSac: [{ id: 'den', ten: 'Đen Titan', hex: '#1E293B', anh: IMG.GALAXY_S24_ULTRA }],
      phienBan: [{ id: 'std', dungLuong: '256GB', gia: 12990000, giaGoc: 14990000 }],
      thongSo: { 'Màn hình': '6.7" OLED 120Hz', 'Pin': '5000 mAh' }
    });
  }
  phoneIdx++;
}

// Cập nhật lại số hiệu maHang tuần tự từ MH101 đến MH300
allFinal.forEach((p, idx) => {
  p.maHang = `MH${String(100 + idx + 1).padStart(3, '0')}`;
});

console.log(`\n🎉 ĐÃ HOÀN TẤT BẢNG 200 SẢN PHẨM TOÀN DIỆN!`);
const groupCounts = {};
allFinal.forEach(p => groupCounts[p.maNhom] = (groupCounts[p.maNhom] || 0) + 1);
console.log('Phân bổ các ngành hàng:', groupCounts);

// 1. Lưu JSON
const metaMap = {};
for (const p of allFinal) {
  metaMap[p.maHang] = p;
}
fs.writeFileSync(existingPath, JSON.stringify(metaMap, null, 2), 'utf8');
console.log(`✅ 1. Đã ghi file JSON: ${existingPath} (${Object.keys(metaMap).length} sản phẩm)`);

// 2. Lưu SQL seed
const sqlLines = [
  '-- ============================================================',
  `-- SEED ĐỦ 200 SẢN PHẨM ĐA DẠNG 6 NGÀNH HÀNG (HOÀNG HÀ MOBILE)`,
  '-- ============================================================',
  ''
];

const nhomMap = {
  DIENTHOAI: 'Điện thoại',
  TABLET: 'Máy tính bảng',
  AMTHANH: 'Âm thanh',
  PHUKIEN: 'Phụ kiện',
  DONGHO: 'Đồng hồ thông minh',
  LAPTOP: 'Laptop & Màn hình'
};
for (const [code, name] of Object.entries(nhomMap)) {
  sqlLines.push(`INSERT INTO NhomHang (MANHOM, TENNHOM) VALUES ('${code}', '${name}') ON CONFLICT (MANHOM) DO NOTHING;`);
}
sqlLines.push('');

const brandsUsed = [...new Set(allFinal.map(r => r.maLoai))];
for (const b of brandsUsed) {
  const brandName = allFinal.find(r => r.maLoai === b)?.nhanHieu || b;
  sqlLines.push(`INSERT INTO LoaiHang (MALOAI, TENLOAI) VALUES ('${b}', '${brandName}') ON CONFLICT (MALOAI) DO NOTHING;`);
}
sqlLines.push('');

for (const p of allFinal) {
  const cleanTen = p.tenHang.replace(/'/g, "''");
  sqlLines.push(`INSERT INTO MatHang (MAHANG, TENHANG, DONGIA, DVT, MANHOM, MALOAI, QUANLYIMEI, THOIHANBAOHANH) VALUES ('${p.maHang}', '${cleanTen}', ${p.donGia}, 'Cái', '${p.maNhom}', '${p.maLoai}', true, 12) ON CONFLICT (MAHANG) DO UPDATE SET TENHANG = EXCLUDED.TENHANG, DONGIA = EXCLUDED.DONGIA;`);
}

const sqlPath = path.join(__dirname, '..', 'database', 'seed-products-200.sql');
fs.writeFileSync(sqlPath, sqlLines.join('\n'), 'utf8');
console.log(`✅ 2. Đã ghi file SQL: ${sqlPath} (${allFinal.length} dòng INSERT)`);
