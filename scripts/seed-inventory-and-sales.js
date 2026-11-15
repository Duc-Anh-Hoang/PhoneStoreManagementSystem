/**
 * Script: seed-inventory-and-sales.js
 * Nhiệm vụ:
 * 1. Nhập kho ban đầu cho ~150 mặt hàng chủ lực (điện thoại, tablet, laptop, phụ kiện, âm thanh).
 *    - Tạo PhieuNhap PN_KHO_2026
 *    - ChiTietPhieuNhap: nhập 20-50 máy mỗi mặt hàng (tổng cộng ~4.500 máy)
 *    - Sinh mã IMEI 15 chữ số hợp lệ cho các mặt hàng quản lý IMEI.
 * 2. Tạo 35-40 giao dịch bán hàng (HoaDon & ChiTietHoaDon) trên 35-40 mặt hàng khác nhau
 *    với số lượng bán từ 1 đến 28 máy để Top Selling hiển thị cực kỳ phong phú và chân thực.
 */

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

function generateImei(counter) {
  // Sinh 15 chữ số bắt đầu bằng 86 hoặc 35
  const prefix = '86' + String(counter).padStart(13, '0');
  return prefix.slice(0, 15);
}

function run() {
  console.log('🔄 Đang lấy danh sách mặt hàng từ PostgreSQL...');

  // Lấy danh sách 150 mặt hàng từ database
  const getProductsCmd = `docker exec phonestore_postgres psql -U postgres -d PhoneStoreDB -t -A -F"," -c "SELECT mahang, manhom, dongia, quanlyimei, tenhang FROM mathang ORDER BY mahang LIMIT 150;"`;
  const rawProducts = execSync(getProductsCmd, { encoding: 'utf8' }).trim().split('\n');

  const products = rawProducts.map(line => {
    const parts = line.split(',');
    return {
      maHang: parts[0],
      maNhom: parts[1],
      donGia: parseFloat(parts[2]) || 1000000,
      quanLyImei: parts[3] === 't' || parts[3] === 'true',
      tenHang: parts.slice(4).join(',')
    };
  }).filter(p => p.maHang);

  console.log(`📦 Đã chọn ${products.length} mặt hàng để nhập kho và tạo giao dịch bán hàng.`);

  const sqlStatements = [];
  sqlStatements.push('BEGIN;\n');

  // 1. Tạo Phiếu Nhập Kho ban đầu
  const soPN = 'PN_KHO_2026';
  sqlStatements.push(`-- 1. Tạo Phiếu Nhập Kho: ${soPN}`);
  sqlStatements.push(
    `INSERT INTO PhieuNhap (SOPN, NGAYNHAP, MANCC, MANV) VALUES ('${soPN}', NOW() - INTERVAL '15 days', 'NCC_AP', 'NV001') ` +
    `ON CONFLICT (SOPN) DO NOTHING;\n`
  );

  // 2. Chi Tiết Phiếu Nhập và IMEI
  let imeiGlobalIndex = 100000;
  const ctpnValues = [];
  const imeiValues = [];

  // Bản đồ lưu trữ các IMEI của từng mặt hàng để sau này trích xuất khi bán hàng
  const productImeiMap = new Map();

  products.forEach((p, idx) => {
    // Nhập từ 20 đến 50 cái mỗi model
    const slNhap = 25 + (idx % 25);
    const donGiaNhap = Math.round(p.donGia * 0.8);
    ctpnValues.push(`('${soPN}', '${p.maHang}', ${slNhap}, ${donGiaNhap})`);

    if (p.quanLyImei) {
      const imeiList = [];
      for (let i = 1; i <= slNhap; i++) {
        imeiGlobalIndex++;
        const maImei = `IM_${p.maHang}_${String(i).padStart(3, '0')}`;
        const soImei = generateImei(imeiGlobalIndex);
        imeiValues.push(`('${maImei}', '${p.maHang}', '${soPN}', '${soImei}', 'con_hang')`);
        imeiList.push(maImei);
      }
      productImeiMap.set(p.maHang, imeiList);
    }
  });

  // Batch insert ChiTietPhieuNhap
  sqlStatements.push('-- 2. Chi Tiết Phiếu Nhập');
  sqlStatements.push(
    `INSERT INTO ChiTietPhieuNhap (SOPN, MAHANG, SLNHAP, DONGIA_NHAP) VALUES\n` +
    ctpnValues.join(',\n') +
    `\nON CONFLICT (SOPN, MAHANG) DO UPDATE SET SLNHAP = EXCLUDED.SLNHAP;\n`
  );

  // Batch insert IMEI (500 per batch)
  sqlStatements.push('-- 3. Danh sách IMEI');
  const IMEI_BATCH = 500;
  for (let i = 0; i < imeiValues.length; i += IMEI_BATCH) {
    const chunk = imeiValues.slice(i, i + IMEI_BATCH);
    sqlStatements.push(
      `INSERT INTO IMEI (MAIMEI, MAHANG, SOPN, SOIMEI, TRANGTHAI) VALUES\n` +
      chunk.join(',\n') +
      `\nON CONFLICT (MAIMEI) DO UPDATE SET TRANGTHAI = EXCLUDED.TRANGTHAI;\n`
    );
  }

  // 3. Tạo 35-40 giao dịch bán hàng (HoaDon & ChiTietHoaDon)
  sqlStatements.push('-- 4. Tạo giao dịch bán hàng cho 38 mặt hàng khác nhau');
  const salesProducts = products.slice(0, 38);
  const soldImeiList = [];

  salesProducts.forEach((p, idx) => {
    const soHD = `HD_SEED_${String(idx + 1).padStart(3, '0')}`;
    // Số lượng bán giảm dần: mặt hàng đầu bán 28 máy, các mặt hàng sau bán 20, 15, 12, 10, ...
    const slBan = Math.max(1, 28 - idx);
    const donGiaBan = p.donGia;
    const tongTien = slBan * donGiaBan;
    const kenhBan = (idx % 2 === 0) ? 'online' : 'tai_quay';
    const ngayLap = `NOW() - INTERVAL '${(idx % 10) + 1} days'`;

    // Tạo HoaDon
    sqlStatements.push(
      `INSERT INTO HoaDon (SOHD, NGAYLAP, MAKH, MANV, KENHBAN, TONGTIEN, TIENGIAMGIA, PHUONGTHUCTHANHTOAN) VALUES ` +
      `('${soHD}', ${ngayLap}, 'KH002', 'NV002', '${kenhBan}', ${tongTien}, 0, 'chuyen_khoan') ` +
      `ON CONFLICT (SOHD) DO UPDATE SET TONGTIEN = EXCLUDED.TONGTIEN;`
    );

    // Tạo ChiTietHoaDon
    if (p.quanLyImei) {
      const imeis = productImeiMap.get(p.maHang) || [];
      for (let s = 0; s < slBan; s++) {
        const maImei = imeis[s] || null;
        if (maImei) soldImeiList.push(maImei);
        sqlStatements.push(
          `INSERT INTO ChiTietHoaDon (SOHD, MAHANG, MAIMEI, SLBAN, DONGIA_BAN) VALUES ` +
          `('${soHD}', '${p.maHang}', ${maImei ? `'${maImei}'` : 'NULL'}, 1, ${donGiaBan});`
        );
      }
    } else {
      sqlStatements.push(
        `INSERT INTO ChiTietHoaDon (SOHD, MAHANG, MAIMEI, SLBAN, DONGIA_BAN) VALUES ` +
        `('${soHD}', '${p.maHang}', NULL, ${slBan}, ${donGiaBan});`
      );
    }
  });

  // 4. Cập nhật trạng thái các IMEI đã bán sang 'da_ban'
  if (soldImeiList.length > 0) {
    sqlStatements.push('\n-- 5. Cập nhật IMEI đã bán sang da_ban');
    const quotedSold = soldImeiList.map(im => `'${im}'`).join(',');
    sqlStatements.push(`UPDATE IMEI SET TRANGTHAI = 'da_ban' WHERE MAIMEI IN (${quotedSold});`);
  }

  sqlStatements.push('\nCOMMIT;\n');

  const fullSql = sqlStatements.join('\n');
  const tempSqlFile = path.resolve(__dirname, '../database/seed_sales_inventory.sql');
  fs.writeFileSync(tempSqlFile, fullSql, 'utf8');
  console.log(`✅ Đã tạo file SQL: ${tempSqlFile} (${(fullSql.length / 1024).toFixed(1)} KB)`);

  console.log('🔄 Đang nạp dữ liệu nhập kho & bán hàng vào PostgreSQL...');
  execSync(`docker exec -i phonestore_postgres psql -U postgres -d PhoneStoreDB`, {
    input: fullSql,
    stdio: ['pipe', 'inherit', 'inherit']
  });

  console.log('🎉 Hoàn tất! Đã nạp thành công dữ liệu nhập kho và 38 mặt hàng bán chạy.');
}

run();
