/**
 * Script: seed-json-to-db.js
 * Nhiệm vụ: Đọc toàn bộ 3.547 sản phẩm từ frontend/data/products-meta.json
 * và nạp trực tiếp vào cơ sở dữ liệu PostgreSQL:
 * - NhomHang (10 nhóm ngành hàng)
 * - LoaiHang (43 thương hiệu sản phẩm)
 * - MatHang (3.547 mã hàng chi tiết)
 * - ThuocTinh (đầy đủ các thuộc tính kỹ thuật + Màu sắc + Dung lượng + Giá niêm yết)
 * - ChiTietThuocTinh (Bóc tách chi tiết thông số, danh sách màu sắc, phiên bản dung lượng)
 * Đồng thời xuất file database/initdb/05-seed-products.sql để tự động nạp khi build Docker.
 */

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

const META_PATH = path.resolve(__dirname, '../frontend/data/products-meta.json');
const OUTPUT_SQL_PATH = path.resolve(__dirname, '../database/initdb/05-seed-products.sql');

function escapeSql(str) {
  if (str === null || str === undefined) return '';
  return String(str).replace(/'/g, "''");
}

function run() {
  console.log('🔄 Đang đọc dữ liệu từ:', META_PATH);
  if (!fs.existsSync(META_PATH)) {
    console.error('❌ Không tìm thấy file:', META_PATH);
    process.exit(1);
  }

  const raw = fs.readFileSync(META_PATH, 'utf8');
  const meta = JSON.parse(raw);
  const products = Object.values(meta);
  console.log(`📦 Tổng số sản phẩm trong JSON: ${products.length}`);

  // 1. Nhóm hàng (NhomHang)
  const nhomMap = new Map([
    ['DIENTHOAI', 'Điện thoại di động'],
    ['TABLET', 'Máy tính bảng'],
    ['LAPTOP', 'Máy tính xách tay'],
    ['PHUKIEN', 'Phụ kiện công nghệ'],
    ['AMTHANH', 'Thiết bị âm thanh'],
    ['DONGHO', 'Đồng hồ thông minh'],
    ['DIENMAY', 'Tivi & Điện máy'],
    ['SMARTHOME', 'Nhà thông minh'],
    ['HANGCU', 'Hàng cũ giá tốt'],
    ['OTHER', 'Sản phẩm khác']
  ]);

  // 2. Loại hàng / Thương hiệu (LoaiHang)
  const loaiMap = new Map();
  products.forEach(p => {
    const ml = (p.maLoai || 'OTHER').toUpperCase().trim().slice(0, 10);
    let tl = (p.nhanHieu || p.tenLoai || ml).trim().slice(0, 50);
    if (ml === 'OTHER' || (tl.toLowerCase() === 'apple' && ml !== 'APPLE')) tl = 'Hãng khác';
    if (tl.toLowerCase() === 'samsung' && ml !== 'SAMSUNG') tl = 'Hãng khác';
    if (!loaiMap.has(ml)) {
      loaiMap.set(ml, tl);
    }
  });

  // 3. Thuộc tính chuẩn hóa (ThuocTinh)
  const thuocTinhMap = new Map([
    ['Màu sắc', 'MAU'],
    ['Dung lượng bộ nhớ', 'DUNGLUONG'],
    ['Bộ nhớ trong', 'DUNGLUONG'],
    ['Dung lượng RAM', 'RAM'],
    ['RAM', 'RAM'],
    ['Kích thước màn hình', 'MANHINH'],
    ['Vi xử lý', 'VIXULY'],
    ['Camera sau', 'CAM_SAU'],
    ['Camera trước', 'CAM_TRUOC'],
    ['Hệ điều hành', 'HEDIEUHANH'],
    ['Dung lượng pin', 'PIN'],
    ['Dung lượng pin hiển thị', 'PIN'],
    ['Card đồ họa rời', 'VGA'],
    ['Chip xử lý (CPU)', 'CPU'],
    ['Chip xử lý', 'CPU'],
    ['Số hiệu CPU', 'CPU'],
    ['Ổ cứng mặc định', 'OCUNG'],
    ['Tần số quét', 'TANSOQUET'],
    ['Độ phân giải', 'DOPHANGIAI'],
    ['Giá niêm yết', 'GIAGOC']
  ]);

  // Sinh mã thuộc tính cho các thông số mở rộng khác
  let ttCounter = 1;
  products.forEach(p => {
    if (p.thongSo && typeof p.thongSo === 'object') {
      Object.keys(p.thongSo).forEach(key => {
        if (!thuocTinhMap.has(key)) {
          const code = `TT${String(ttCounter++).padStart(3, '0')}`;
          thuocTinhMap.set(key, code);
        }
      });
    }
  });

  const sqlStatements = [];
  sqlStatements.push('-- ==========================================================');
  sqlStatements.push('-- Auto-generated Seed SQL from products-meta.json');
  sqlStatements.push(`-- Total products: ${products.length}`);
  sqlStatements.push('-- ==========================================================\n');
  sqlStatements.push('BEGIN;\n');

  // Insert NhomHang
  sqlStatements.push('-- 1. Insert NhomHang');
  for (const [code, name] of nhomMap.entries()) {
    sqlStatements.push(
      `INSERT INTO NhomHang (MANHOM, TENNHOM) VALUES ('${escapeSql(code)}', '${escapeSql(name)}') ` +
      `ON CONFLICT (MANHOM) DO UPDATE SET TENNHOM = EXCLUDED.TENNHOM;`
    );
  }

  // Insert LoaiHang
  sqlStatements.push('\n-- 2. Insert LoaiHang');
  for (const [code, name] of loaiMap.entries()) {
    sqlStatements.push(
      `INSERT INTO LoaiHang (MALOAI, TENLOAI) VALUES ('${escapeSql(code)}', '${escapeSql(name)}') ` +
      `ON CONFLICT (MALOAI) DO UPDATE SET TENLOAI = EXCLUDED.TENLOAI;`
    );
  }

  // Insert ThuocTinh
  sqlStatements.push('\n-- 3. Insert ThuocTinh');
  const seenCodes = new Set();
  for (const [name, code] of thuocTinhMap.entries()) {
    if (!seenCodes.has(code)) {
      seenCodes.add(code);
      const cleanName = name.slice(0, 50);
      sqlStatements.push(
        `INSERT INTO ThuocTinh (MATHUOCTINH, TENTHUOCTINH) VALUES ('${escapeSql(code)}', '${escapeSql(cleanName)}') ` +
        `ON CONFLICT (MATHUOCTINH) DO UPDATE SET TENTHUOCTINH = EXCLUDED.TENTHUOCTINH;`
      );
    }
  }

  // Insert MatHang in batches
  sqlStatements.push('\n-- 4. Insert MatHang (Batched)');
  const BATCH_SIZE = 500;
  for (let i = 0; i < products.length; i += BATCH_SIZE) {
    const chunk = products.slice(i, i + BATCH_SIZE);
    const values = chunk.map(p => {
      const maHang = p.maHang.trim().slice(0, 10);
      const tenHang = (p.tenHang || maHang).trim().slice(0, 100);
      const donGia = Math.max(1000, Number(p.donGia) || 1000);
      const dvt = 'Cái';
      const maNhom = nhomMap.has(p.maNhom) ? p.maNhom : 'OTHER';
      const maLoai = loaiMap.has(p.maLoai?.toUpperCase()) ? p.maLoai.toUpperCase() : 'OTHER';
      const quanLyImei = Boolean(p.quanLyImei ?? ['DIENTHOAI', 'TABLET', 'LAPTOP', 'DONGHO'].includes(p.maNhom));
      const thoiHanBaoHanh = Math.max(0, Number(p.thoiHanBaoHanh) || 12);

      return `('${escapeSql(maHang)}', '${escapeSql(tenHang)}', ${donGia}, '${escapeSql(dvt)}', '${escapeSql(maNhom)}', '${escapeSql(maLoai)}', ${quanLyImei}, ${thoiHanBaoHanh})`;
    });

    sqlStatements.push(
      `INSERT INTO MatHang (MAHANG, TENHANG, DONGIA, DVT, MANHOM, MALOAI, QUANLYIMEI, THOIHANBAOHANH) VALUES\n` +
      values.join(',\n') +
      `\nON CONFLICT (MAHANG) DO UPDATE SET ` +
      `TENHANG = EXCLUDED.TENHANG, DONGIA = EXCLUDED.DONGIA, DVT = EXCLUDED.DVT, ` +
      `MANHOM = EXCLUDED.MANHOM, MALOAI = EXCLUDED.MALOAI, ` +
      `QUANLYIMEI = EXCLUDED.QUANLYIMEI, THOIHANBAOHANH = EXCLUDED.THOIHANBAOHANH;\n`
    );
  }

  // Insert ChiTietThuocTinh (Bóc tách đầy đủ cả thongSo, mauSac, phienBan và giaGoc)
  sqlStatements.push('\n-- 5. Insert ChiTietThuocTinh (Batched)');
  const specRows = [];
  products.forEach(p => {
    const maHang = p.maHang.trim().slice(0, 10);
    const addedAttrs = new Set();

    // 5.1. Thuộc tính từ thongSo
    if (p.thongSo && typeof p.thongSo === 'object') {
      for (const [key, val] of Object.entries(p.thongSo)) {
        const code = thuocTinhMap.get(key);
        if (code && val && !addedAttrs.has(code)) {
          addedAttrs.add(code);
          const cleanVal = String(val).trim().slice(0, 100);
          specRows.push(`('${escapeSql(maHang)}', '${escapeSql(code)}', '${escapeSql(cleanVal)}')`);
        }
      }
    }

    // 5.2. Thuộc tính Màu sắc (mauSac)
    if (!addedAttrs.has('MAU') && Array.isArray(p.mauSac) && p.mauSac.length > 0) {
      const colorNames = p.mauSac.map(m => m.ten).filter(Boolean);
      if (colorNames.length > 0) {
        addedAttrs.add('MAU');
        const colorVal = [...new Set(colorNames)].join(', ').slice(0, 100);
        specRows.push(`('${escapeSql(maHang)}', 'MAU', '${escapeSql(colorVal)}')`);
      }
    }

    // 5.3. Thuộc tính Dung lượng từ phiên bản (phienBan) nếu thongSo chưa có
    if (!addedAttrs.has('DUNGLUONG') && Array.isArray(p.phienBan) && p.phienBan.length > 0) {
      const versionCaps = p.phienBan.map(v => v.dungLuong).filter(d => d && d !== 'Tiêu chuẩn');
      if (versionCaps.length > 0) {
        addedAttrs.add('DUNGLUONG');
        const capVal = [...new Set(versionCaps)].join(', ').slice(0, 100);
        specRows.push(`('${escapeSql(maHang)}', 'DUNGLUONG', '${escapeSql(capVal)}')`);
      }
    }

    // 5.4. Giá niêm yết / Giá gốc (giaGoc)
    if (p.giaGoc && p.giaGoc > (p.donGia || 0) && !addedAttrs.has('GIAGOC')) {
      addedAttrs.add('GIAGOC');
      const giaGocFormatted = Number(p.giaGoc).toLocaleString('vi-VN') + ' đ';
      specRows.push(`('${escapeSql(maHang)}', 'GIAGOC', '${escapeSql(giaGocFormatted.slice(0, 100))}')`);
    }
  });

  const SPEC_BATCH = 1000;
  for (let i = 0; i < specRows.length; i += SPEC_BATCH) {
    const chunk = specRows.slice(i, i + SPEC_BATCH);
    sqlStatements.push(
      `INSERT INTO ChiTietThuocTinh (MAHANG, MATHUOCTINH, GIA_TRI) VALUES\n` +
      chunk.join(',\n') +
      `\nON CONFLICT (MAHANG, MATHUOCTINH) DO UPDATE SET GIA_TRI = EXCLUDED.GIA_TRI;\n`
    );
  }

  sqlStatements.push('COMMIT;\n');

  const fullSql = sqlStatements.join('\n');
  fs.writeFileSync(OUTPUT_SQL_PATH, fullSql, 'utf8');
  console.log(`✅ Đã xuất file SQL: ${OUTPUT_SQL_PATH} (${(fullSql.length / 1024).toFixed(1)} KB)`);
  console.log(`📊 Tổng số dòng thuộc tính chi tiết: ${specRows.length}`);

  // Thực thi trực tiếp vào PostgreSQL nếu container đang chạy
  console.log('🔄 Đang nạp vào PostgreSQL container phonestore_postgres...');
  try {
    execSync(`docker exec -i phonestore_postgres psql -U postgres -d PhoneStoreDB`, {
      input: fullSql,
      stdio: ['pipe', 'inherit', 'inherit']
    });
    console.log('🎉 Hoàn tất! Đã cập nhật đầy đủ thuộc tính vào PostgreSQL.');
  } catch (err) {
    console.error('⚠️ Lỗi thực thi qua docker exec:', err.message);
  }
}

run();
