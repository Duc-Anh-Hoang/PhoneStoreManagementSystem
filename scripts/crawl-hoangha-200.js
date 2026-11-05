const fs = require('fs');
const path = require('path');

function decodeEntities(str) {
  if (!str) return '';
  return str
    .replace(/&#x([0-9a-fA-F]+);/g, (_, hex) => String.fromCodePoint(parseInt(hex, 16)))
    .replace(/&#([0-9]+);/g, (_, dec) => String.fromCodePoint(parseInt(dec, 10)))
    .replace(/&amp;/g, '&')
    .replace(/&quot;/g, '"')
    .replace(/&lt;/g, '<')
    .replace(/&gt;/g, '>')
    .replace(/&nbsp;/g, ' ')
    .trim();
}

function detectBrand(title) {
  const t = title.toLowerCase();
  if (t.includes('apple') || t.includes('iphone') || t.includes('ipad') || t.includes('macbook') || t.includes('airpods')) return { name: 'Apple', code: 'APPLE' };
  if (t.includes('samsung') || t.includes('galaxy')) return { name: 'Samsung', code: 'SAMSUNG' };
  if (t.includes('xiaomi') || t.includes('redmi') || t.includes('poco')) return { name: 'Xiaomi', code: 'XIAOMI' };
  if (t.includes('oppo') || t.includes('reno')) return { name: 'OPPO', code: 'OPPO' };
  if (t.includes('vivo')) return { name: 'Vivo', code: 'VIVO' };
  if (t.includes('realme')) return { name: 'Realme', code: 'REALME' };
  if (t.includes('honor')) return { name: 'HONOR', code: 'HONOR' };
  if (t.includes('nubia') || t.includes('redmagic')) return { name: 'Nubia', code: 'NUBIA' };
  if (t.includes('tecno') || t.includes('pova') || t.includes('camon') || t.includes('spark')) return { name: 'Tecno', code: 'TECNO' };
  if (t.includes('nokia')) return { name: 'Nokia', code: 'NOKIA' };
  if (t.includes('sony') || t.includes('xperia') || t.includes('inzone')) return { name: 'Sony', code: 'SONY' };
  if (t.includes('asus') || t.includes('zenbook') || t.includes('vivobook') || t.includes('proart') || t.includes('tuf') || t.includes('rog')) return { name: 'ASUS', code: 'ASUS' };
  if (t.includes('dell') || t.includes('vostro') || t.includes('inspiron') || t.includes('ultrasharp') || t.includes('xps')) return { name: 'Dell', code: 'DELL' };
  if (t.includes('lenovo') || t.includes('thinkpad') || t.includes('ideapad') || t.includes('legion')) return { name: 'Lenovo', code: 'LENOVO' };
  if (t.includes('hp ') || t.includes('pavilion') || t.includes('victus') || t.includes('envy')) return { name: 'HP', code: 'HP' };
  if (t.includes('acer') || t.includes('nitro') || t.includes('aspire') || t.includes('predator')) return { name: 'Acer', code: 'ACER' };
  if (t.includes('lg ') || t.includes('ultragear')) return { name: 'LG', code: 'LG' };
  if (t.includes('jbl')) return { name: 'JBL', code: 'JBL' };
  if (t.includes('marshall')) return { name: 'Marshall', code: 'MARSHALL' };
  if (t.includes('anker') || t.includes('soundcore')) return { name: 'Anker', code: 'ANKER' };
  if (t.includes('huawei')) return { name: 'Huawei', code: 'HUAWEI' };
  if (t.includes('garmin')) return { name: 'Garmin', code: 'GARMIN' };
  if (t.includes('logitech')) return { name: 'Logitech', code: 'OTHER' };
  if (t.includes('keychron')) return { name: 'Keychron', code: 'OTHER' };
  if (t.includes('belkin')) return { name: 'Belkin', code: 'OTHER' };
  if (t.includes('baseus')) return { name: 'Baseus', code: 'OTHER' };
  if (t.includes('innostyle')) return { name: 'Innostyle', code: 'OTHER' };
  if (t.includes('mophie')) return { name: 'Mophie', code: 'OTHER' };
  if (t.includes('sennheiser')) return { name: 'Sennheiser', code: 'OTHER' };
  if (t.includes('edifier')) return { name: 'Edifier', code: 'OTHER' };
  if (t.includes('alpha works')) return { name: 'Alpha Works', code: 'OTHER' };
  if (t.includes('tcl')) return { name: 'TCL', code: 'TCL' };
  if (t.includes('itel')) return { name: 'itel', code: 'ITEL' };
  if (t.includes('benco')) return { name: 'Benco', code: 'BENCO' };
  if (t.includes('inoi')) return { name: 'Inoi', code: 'INOI' };
  return { name: 'Chính hãng', code: 'OTHER' };
}

function parsePage(html, url, maNhom, tenNhom) {
  if (!html) return null;

  // Title
  const h1Match = html.match(/<h1[^>]*>([\s\S]*?)<\/h1>/i);
  let rawTitle = h1Match ? h1Match[1].replace(/<[^>]+>/g, '').trim() : '';
  const tenHang = decodeEntities(rawTitle);
  if (!tenHang || tenHang.length < 5) return null;

  // Bỏ qua trang danh mục
  const genericTitles = ['điện thoại', 'laptop', 'màn hình', 'tai nghe', 'đồng hồ', 'máy tính bảng', 'phụ kiện', 'sạc dự phòng', 'củ sạc'];
  if (genericTitles.some(g => tenHang.toLowerCase() === g || tenHang.toLowerCase() === `${g} chính hãng`)) return null;

  // Price
  let donGia = 0;
  const pData = html.match(/data-product-price>([\d\.]+)</i)?.[1];
  const pSchema = html.match(/"price":\s*(\d+(?:\.\d+)?)/i)?.[1];
  const pRegex = html.match(/(\d{1,3}(?:\.\d{3}){1,2})\s*₫/)?.[1];
  
  if (pData) donGia = parseInt(pData.replace(/\D/g, ''), 10);
  else if (pSchema) donGia = Math.round(parseFloat(pSchema));
  else if (pRegex) donGia = parseInt(pRegex.replace(/\D/g, ''), 10);

  if (!donGia || donGia < 50000) return null;

  // Old price
  let giaGoc = Math.round(donGia * 1.08);
  const pOldMatch = html.match(/data-product-last-price-value>([\d\.]+)</i)?.[1]
    || html.match(/class="[^"]*line-through[^"]*">.*?(\d{1,3}(?:\.\d{3}){1,2})\s*₫/s)?.[1];
  if (pOldMatch) {
    const rawOld = parseInt(pOldMatch.replace(/\D/g, ''), 10);
    if (rawOld > donGia) giaGoc = rawOld;
  }

  // Image
  const uploadMatches = [...html.matchAll(/\/Uploads\/[^\s"'<>]+\.(?:png|jpg|webp|jpeg)/gi)].map(m => m[0]);
  let anhChinh = '';
  for (const src of uploadMatches) {
    const lower = src.toLowerCase();
    if (lower.includes('banner') || lower.includes('logo') || lower.includes('icon') || lower.includes('avatar') || lower.includes('cat-') || lower.includes('thumb-ads')) continue;
    anhChinh = `https://i.hoanghamobile.vn${src}`;
    break;
  }
  if (!anhChinh && uploadMatches.length > 0) {
    anhChinh = `https://i.hoanghamobile.vn${uploadMatches[0]}`;
  }
  if (!anhChinh) return null;

  // Colors
  const mauSac = [];
  const colorMatches = [...html.matchAll(/data-color="([^"]+)"[^>]*>[\s\S]*?<span[^>]*>([^<]+)<\/span>/gi)];
  if (colorMatches.length > 0) {
    for (const c of colorMatches) {
      const cTen = decodeEntities(c[2].trim());
      if (cTen && !mauSac.some(m => m.ten === cTen)) {
        mauSac.push({
          id: `c_${c[1].replace(/[^\w]/g, '').toLowerCase()}`,
          ten: cTen,
          hex: '#3B82F6',
          anh: anhChinh
        });
      }
    }
  }
  if (mauSac.length === 0) {
    mauSac.push({ id: 'std', ten: 'Tiêu chuẩn', hex: '#1E293B', anh: anhChinh });
  }

  // Versions
  const phienBan = [];
  const verMatches = [...html.matchAll(/data-version="([^"]+)"[^>]*>[\s\S]*?<span[^>]*>([^<]+)<\/span>/gi)];
  if (verMatches.length > 0) {
    for (const v of verMatches) {
      const vTen = decodeEntities(v[2].trim());
      if (vTen && !phienBan.some(p => p.dungLuong === vTen)) {
        phienBan.push({
          id: `v_${v[1].replace(/[^\w]/g, '').toLowerCase()}`,
          dungLuong: vTen,
          gia: donGia,
          giaGoc: giaGoc
        });
      }
    }
  }
  if (phienBan.length === 0) {
    phienBan.push({ id: 'std', dungLuong: 'Tiêu chuẩn', gia: donGia, giaGoc: giaGoc });
  }

  // Specs
  const thongSo = {};
  const specRows = [...html.matchAll(/<tr>\s*<td[^>]*>([\s\S]*?)<\/td>\s*<td[^>]*>([\s\S]*?)<\/td>\s*<\/tr>/gi)];
  for (const r of specRows) {
    const k = decodeEntities(r[1].replace(/<[^>]+>/g, '').trim());
    const v = decodeEntities(r[2].replace(/<[^>]+>/g, '').trim());
    if (k && v && !thongSo[k] && Object.keys(thongSo).length < 6) {
      thongSo[k] = v;
    }
  }

  const brand = detectBrand(tenHang);

  return {
    tenHang,
    nhanHieu: brand.name,
    maLoai: brand.code,
    maNhom,
    tenNhom,
    donGia,
    giaGoc,
    anhChinh,
    mauSac: mauSac.slice(0, 4),
    phienBan: phienBan.slice(0, 4),
    thongSo
  };
}

async function fetchUrl(url) {
  try {
    const res = await fetch(url, {
      headers: {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36'
      },
      signal: AbortSignal.timeout(2500)
    });
    if (res.ok) return await res.text();
  } catch (e) {}
  return null;
}

async function main() {
  console.log('=== [1/3] ĐỌC DANH SÁCH URLS ĐÃ CACHE TỪ SITEMAP ===');
  const sitemapFile = path.join(__dirname, 'sitemap-urls.json');
  if (!fs.existsSync(sitemapFile)) {
    console.error('Không tìm thấy sitemap-urls.json');
    return;
  }
  const allUrls = JSON.parse(fs.readFileSync(sitemapFile, 'utf8'));
  console.log(`Đã nạp ${allUrls.length} URLs từ file cache sitemap.`);

  function getCategoryUrls(prefix) {
    return allUrls.filter(u => {
      try {
        const parts = new URL(u).pathname.split('/').filter(Boolean);
        return parts[0] === prefix && parts.length === 2 && !u.includes('hang-san-xuat');
      } catch { return false; }
    });
  }

  const categoryConfigs = [
    { maNhom: 'LAPTOP', tenNhom: 'Laptop & Màn hình', target: 30, urls: [...getCategoryUrls('laptop'), ...getCategoryUrls('man-hinh-may-tinh')] },
    { maNhom: 'DONGHO', tenNhom: 'Đồng hồ thông minh', target: 25, urls: getCategoryUrls('dong-ho-thong-minh') },
    { maNhom: 'TABLET', tenNhom: 'Máy tính bảng', target: 25, urls: getCategoryUrls('may-tinh-bang') },
    { maNhom: 'AMTHANH', tenNhom: 'Âm thanh', target: 25, urls: [...getCategoryUrls('tai-nghe'), ...getCategoryUrls('loa')] },
    { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện', target: 25, urls: [...getCategoryUrls('sac-du-phong'), ...getCategoryUrls('cu-sac'), ...getCategoryUrls('chuot'), ...getCategoryUrls('ban-phim')] },
    { maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại', target: 70, urls: [...getCategoryUrls('dien-thoai'), ...getCategoryUrls('dien-thoai-di-dong')] }
  ];

  const allProducts = [];
  const seenTitles = new Set();

  console.log('\n=== [2/3] CÀO SẢN PHẨM 100% THỰC TẾ THEO DANH MỤC ===');

  for (const config of categoryConfigs) {
    console.log(`\n--- Bắt đầu cào ${config.maNhom} (${config.tenNhom}) - Mục tiêu: ${config.target} sp (từ ${config.urls.length} URLs) ---`);
    let count = 0;
    const batchSize = 6;

    for (let i = 0; i < config.urls.length && count < config.target; i += batchSize) {
      const batch = config.urls.slice(i, i + batchSize);
      const results = await Promise.all(batch.map(async u => {
        const html = await fetchUrl(u);
        return parsePage(html, u, config.maNhom, config.tenNhom);
      }));

      for (const p of results) {
        if (p && !seenTitles.has(p.tenHang.toLowerCase())) {
          seenTitles.add(p.tenHang.toLowerCase());
          p.maHang = `MH${String(100 + allProducts.length + 1).padStart(3, '0')}`;
          allProducts.push(p);
          count++;
          console.log(`+ [${config.maNhom}] (${count}/${config.target}) ${p.tenHang} - ${p.donGia.toLocaleString('vi-VN')}đ`);
          if (count >= config.target) break;
        }
      }
    }
  }

  console.log(`\nĐã thu thập được tổng cộng ${allProducts.length} sản phẩm thực tế từ HoangHaMobile.`);

  // Nếu còn thiếu để đủ 200, lấy thêm điện thoại thực tế từ kho 349 URLs điện thoại
  if (allProducts.length < 200) {
    const needed = 200 - allProducts.length;
    console.log(`Đang cào thêm ${needed} sản phẩm từ kho điện thoại để đủ chính xác 200...`);
    const phoneUrls = [...getCategoryUrls('dien-thoai'), ...getCategoryUrls('dien-thoai-di-dong')];
    for (let i = 0; i < phoneUrls.length && allProducts.length < 200; i += 6) {
      const batch = phoneUrls.slice(i, i + 6);
      const results = await Promise.all(batch.map(async u => {
        const html = await fetchUrl(u);
        return parsePage(html, u, 'DIENTHOAI', 'Điện thoại');
      }));
      for (const p of results) {
        if (p && !seenTitles.has(p.tenHang.toLowerCase())) {
          seenTitles.add(p.tenHang.toLowerCase());
          p.maHang = `MH${String(100 + allProducts.length + 1).padStart(3, '0')}`;
          allProducts.push(p);
          console.log(`+ [DIENTHOAI-EXTRA] (${allProducts.length}/200) ${p.tenHang} - ${p.donGia.toLocaleString('vi-VN')}đ`);
          if (allProducts.length >= 200) break;
        }
      }
    }
  }

  // Cắt đúng 200 sản phẩm và đánh mã MH101 -> MH300
  const final200 = allProducts.slice(0, 200);
  final200.forEach((p, idx) => {
    p.maHang = `MH${String(100 + idx + 1).padStart(3, '0')}`;
  });

  console.log('\n=== [3/3] TỔNG HỢP VÀ LƯU TRỮ DỮ LIỆU ===');
  const catStats = {};
  final200.forEach(p => catStats[p.maNhom] = (catStats[p.maNhom] || 0) + 1);
  console.log('Thống kê 200 sản phẩm theo ngành hàng:', catStats);

  // 1. Lưu file JSON
  const metaPath = path.join(__dirname, '..', 'frontend', 'products-meta.json');
  const metaMap = {};
  for (const p of final200) {
    metaMap[p.maHang] = p;
  }
  fs.writeFileSync(metaPath, JSON.stringify(metaMap, null, 2), 'utf8');
  console.log(`✅ 1. Ghi file JSON: ${metaPath} (Đúng ${final200.length} sản phẩm 100% cào từ Hoàng Hà)`);

  // 2. Lưu file SQL seed
  const sqlLines = [
    '-- ============================================================',
    '-- SEED ĐỦ 200 SẢN PHẨM CÀO 100% TRỰC TIẾP TỪ HOÀNG HÀ MOBILE',
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

  const brandsUsed = [...new Set(final200.map(r => r.maLoai))];
  for (const b of brandsUsed) {
    const brandName = final200.find(r => r.maLoai === b)?.nhanHieu || b;
    sqlLines.push(`INSERT INTO LoaiHang (MALOAI, TENLOAI) VALUES ('${b}', '${brandName}') ON CONFLICT (MALOAI) DO NOTHING;`);
  }
  sqlLines.push('');

  for (const p of final200) {
    const cleanTen = p.tenHang.replace(/'/g, "''");
    sqlLines.push(`INSERT INTO MatHang (MAHANG, TENHANG, DONGIA, DVT, MANHOM, MALOAI, QUANLYIMEI, THOIHANBAOHANH) VALUES ('${p.maHang}', '${cleanTen}', ${p.donGia}, 'Cái', '${p.maNhom}', '${p.maLoai}', true, 12) ON CONFLICT (MAHANG) DO UPDATE SET TENHANG = EXCLUDED.TENHANG, DONGIA = EXCLUDED.DONGIA;`);
  }

  const sqlPath = path.join(__dirname, '..', 'database', 'seed-products-200.sql');
  fs.writeFileSync(sqlPath, sqlLines.join('\n'), 'utf8');
  console.log(`✅ 2. Ghi file SQL: ${sqlPath} (${final200.length} dòng INSERT)`);
}

main().catch(err => console.error('Crawler error:', err));
