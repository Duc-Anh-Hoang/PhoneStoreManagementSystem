const fs = require('fs');
const path = require('path');

function decodeHtml(html) {
  if (!html) return '';
  return html
    .replace(/&#x([0-9a-fA-F]+);/g, (_, hex) => String.fromCharCode(parseInt(hex, 16)))
    .replace(/&#(\d+);/g, (_, dec) => String.fromCharCode(parseInt(dec, 10)))
    .replace(/&amp;/g, '&')
    .replace(/&quot;/g, '"')
    .replace(/&lt;/g, '<')
    .replace(/&gt;/g, '>')
    .replace(/&nbsp;/g, ' ')
    .trim();
}

function parsePrice(str) {
  if (!str) return 0;
  const cleaned = str.replace(/[^\d]/g, '');
  return parseInt(cleaned, 10) || 0;
}

function guessHex(colorName) {
  const c = (colorName || '').toLowerCase();
  if (c.includes('đen') || c.includes('black') || c.includes('dark') || c.includes('midnight')) return '#1F2937';
  if (c.includes('trắng') || c.includes('white') || c.includes('starlight')) return '#F3F4F6';
  if (c.includes('bạc') || c.includes('silver')) return '#E5E7EB';
  if (c.includes('xám') || c.includes('gray') || c.includes('grey') || c.includes('titan') || c.includes('space')) return '#9CA3AF';
  if (c.includes('vàng') || c.includes('gold') || c.includes('yellow') || c.includes('sa mạc')) return '#FBBF24';
  if (c.includes('xanh dương') || c.includes('xanh lam') || c.includes('blue') || c.includes('navy')) return '#3B82F6';
  if (c.includes('xanh lá') || c.includes('green') || c.includes('rêu')) return '#10B981';
  if (c.includes('hồng') || c.includes('pink') || c.includes('rose')) return '#EC4899';
  if (c.includes('đỏ') || c.includes('red')) return '#EF4444';
  if (c.includes('tím') || c.includes('purple') || c.includes('violet')) return '#8B5CF6';
  if (c.includes('cam') || c.includes('orange')) return '#F97316';
  return '#2563EB';
}

function guessBrand(title, url) {
  const text = (title + ' ' + url).toLowerCase();
  if (text.includes('iphone') || text.includes('apple') || text.includes('ipad') || text.includes('airpods')) return { code: 'APPLE', name: 'Apple' };
  if (text.includes('samsung') || text.includes('galaxy')) return { code: 'SAMSUNG', name: 'Samsung' };
  if (text.includes('xiaomi') || text.includes('redmi') || text.includes('poco')) return { code: 'XIAOMI', name: 'Xiaomi' };
  if (text.includes('oppo')) return { code: 'OPPO', name: 'OPPO' };
  if (text.includes('vivo')) return { code: 'VIVO', name: 'Vivo' };
  if (text.includes('realme')) return { code: 'REALME', name: 'Realme' };
  if (text.includes('honor')) return { code: 'HONOR', name: 'HONOR' };
  if (text.includes('nubia') || text.includes('redmagic')) return { code: 'NUBIA', name: 'Nubia' };
  if (text.includes('tecno')) return { code: 'TECNO', name: 'Tecno' };
  if (text.includes('nokia')) return { code: 'NOKIA', name: 'Nokia' };
  if (text.includes('itel')) return { code: 'ITEL', name: 'itel' };
  if (text.includes('anker')) return { code: 'ANKER', name: 'Anker' };
  if (text.includes('sony')) return { code: 'SONY', name: 'Sony' };
  if (text.includes('jbl')) return { code: 'JBL', name: 'JBL' };
  if (text.includes('marshall')) return { code: 'MARSHALL', name: 'Marshall' };
  if (text.includes('benco')) return { code: 'BENCO', name: 'Benco' };
  if (text.includes('inoi')) return { code: 'INOI', name: 'Inoi' };
  if (text.includes('tcl')) return { code: 'TCL', name: 'TCL' };
  if (text.includes('huawei')) return { code: 'HUAWEI', name: 'Huawei' };
  return { code: 'OTHER', name: 'Chính hãng' };
}

function guessCategory(url) {
  const p = url.toLowerCase();
  if (p.includes('/may-tinh-bang') || p.includes('/tablet')) return { code: 'TABLET', name: 'Máy tính bảng' };
  if (p.includes('/tai-nghe') || p.includes('/loa')) return { code: 'AMTHANH', name: 'Âm thanh' };
  if (p.includes('/phu-kien') || p.includes('/cu-sac') || p.includes('/sac-du-phong') || p.includes('/op-lung')) return { code: 'PHUKIEN', name: 'Phụ kiện' };
  if (p.includes('/dong-ho')) return { code: 'DONGHO', name: 'Đồng hồ thông minh' };
  return { code: 'DIENTHOAI', name: 'Điện thoại' };
}

async function fetchPage(url) {
  try {
    const res = await fetch(url, {
      signal: AbortSignal.timeout(4000),
      headers: {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36',
        'Accept': 'text/html,application/xhtml+xml',
        'Accept-Language': 'vi,en;q=0.9'
      }
    });
    if (!res.ok) return null;
    return await res.text();
  } catch (e) {
    return null;
  }
}

async function extractProduct(url) {
  const html = await fetchPage(url);
  if (!html) return null;

  // Title
  const titleMatch = html.match(/<h1[^>]*>([\s\S]*?)<\/h1>/i);
  let title = titleMatch ? decodeHtml(titleMatch[1].replace(/<[^>]+>/g, '')) : '';
  if (!title) {
    const ogTitle = html.match(/<meta property="og:title" content="([^"]+)"/i);
    title = ogTitle ? decodeHtml(ogTitle[1]) : '';
  }
  if (!title || title.length < 3) return null;

  // Image
  let anhChinh = '';
  const ogImg = html.match(/<meta property="og:image" content="([^"]+)"/i);
  if (ogImg) anhChinh = ogImg[1];

  // Versions
  const phienBan = [];
  const verRegex = /<a[^>]*data-product-version[^>]*>[\s\S]*?<strong[^>]*>([\s\S]*?)<\/strong>[\s\S]*?<span[^>]*>([\s\S]*?)<\/span>[\s\S]*?<\/a>/gi;
  let vm;
  while ((vm = verRegex.exec(html)) !== null) {
    const dungLuong = decodeHtml(vm[1].replace(/<[^>]+>/g, ''));
    const gia = parsePrice(vm[2]);
    if (dungLuong && gia > 0) {
      phienBan.push({
        id: 'v_' + dungLuong.replace(/[^\w]/g, '').toLowerCase(),
        dungLuong,
        gia,
        giaGoc: Math.round(gia * 1.08)
      });
    }
  }

  // Colors
  const mauSac = [];
  const colorBtnRegex = /<button[^>]*data-product-color[\s\S]*?data-color-name="([^"]+)"[\s\S]*?data-image="([^"]*)"/gi;
  let cm;
  while ((cm = colorBtnRegex.exec(html)) !== null) {
    const ten = decodeHtml(cm[1]);
    let img = cm[2];
    if (img && img.includes(';')) img = img.split(';')[0];
    if (ten) {
      mauSac.push({
        id: 'c_' + ten.replace(/[^\w]/g, '').toLowerCase(),
        ten,
        hex: guessHex(ten),
        anh: img || anhChinh
      });
    }
  }

  if (!anhChinh && mauSac.length > 0 && mauSac[0].anh) {
    anhChinh = mauSac[0].anh;
  }
  if (anhChinh && anhChinh.includes(';')) {
    anhChinh = anhChinh.split(';')[0];
  }

  if (!anhChinh) return null;

  // Price
  let donGia = phienBan.length > 0 ? phienBan[0].gia : 0;
  if (donGia === 0) {
    const priceTextMatch = html.match(/class="[^"]*price[^"]*"[^>]*>([\d\.,]+)\s*₫/i)
      || html.match(/class="current-product-price"[^>]*>([\s\S]*?)<\/strong>/i);
    if (priceTextMatch) {
      donGia = parsePrice(priceTextMatch[1]);
    }
  }
  if (donGia === 0) {
    const rawDigits = html.match(/(\d{1,3}(?:\.\d{3}){1,3})\s*₫/);
    if (rawDigits) donGia = parsePrice(rawDigits[1]);
  }
  if (donGia === 0) donGia = 1990000;

  if (phienBan.length === 0) {
    phienBan.push({
      id: 'std',
      dungLuong: 'Tiêu chuẩn',
      gia: donGia,
      giaGoc: Math.round(donGia * 1.08)
    });
  }

  if (mauSac.length === 0) {
    mauSac.push({
      id: 'std',
      ten: 'Mặc định',
      hex: '#2563EB',
      anh: anhChinh
    });
  }

  // Specs
  const thongSo = {};
  const screenMatch = html.match(/(?:màn hình|kích thước màn hình)[^<]*<\/div>[\s\S]*?<div[^>]*>([\s\S]*?)<\/div>/i)
    || html.match(/(\d+(?:\.\d+)?\s*(?:inch|'|"))/i);
  if (screenMatch) thongSo['Màn hình'] = decodeHtml(screenMatch[1].replace(/<[^>]+>/g, ''));

  const ramRomMatch = title.match(/(\d+GB|\d+TB)\s*\/\s*(\d+GB|\d+TB)/i);
  if (ramRomMatch) {
    thongSo['RAM / ROM'] = `${ramRomMatch[1]} / ${ramRomMatch[2]}`;
  }

  const pinMatch = html.match(/(\d{4,5}\s*mAh)/i);
  if (pinMatch) thongSo['Pin'] = pinMatch[1];

  const brand = guessBrand(title, url);
  const cat = guessCategory(url);
  const slug = url.split('/').pop().replace(/[^\w-]/g, '');

  return {
    slug,
    tenHang: title.replace(/^Điện thoại\s+/i, '').trim(),
    nhanHieu: brand.name,
    maLoai: brand.code,
    maNhom: cat.code,
    tenNhom: cat.name,
    donGia,
    giaGoc: Math.round(donGia * 1.08),
    anhChinh,
    mauSac,
    phienBan,
    thongSo
  };
}

async function main() {
  console.log('=== BẮT ĐẦU CÀO ĐẦY ĐỦ 200 SẢN PHẨM HOÀNG HÀ MOBILE ===');
  
  const sitemapRes = await fetch('https://hoanghamobile.com/sitemap.xml', {
    headers: { 'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)' }
  });
  const xml = await sitemapRes.text();
  const allLocs = [...xml.matchAll(/<loc>([\s\S]*?)<\/loc>/gi)].map(m => m[1].trim());

  // Priority categories: dien-thoai, may-tinh-bang, tai-nghe, loa, cu-sac, sac-du-phong, dong-ho-thong-minh
  const validPrefixes = [
    '/dien-thoai/',
    '/may-tinh-bang/',
    '/tai-nghe/',
    '/loa/',
    '/cu-sac/',
    '/sac-du-phong/',
    '/dong-ho-thong-minh/'
  ];

  const candidateUrls = allLocs.filter(u => {
    try {
      const p = new URL(u).pathname;
      return validPrefixes.some(prefix => p.startsWith(prefix));
    } catch {
      return false;
    }
  });

  const uniqueUrls = [...new Set(candidateUrls)];
  console.log(`Tìm thấy ${uniqueUrls.length} sản phẩm từ các danh mục chính.`);

  // Group by brand
  const brandGroups = {};
  for (const u of uniqueUrls) {
    const brand = guessBrand('', u).code;
    if (!brandGroups[brand]) brandGroups[brand] = [];
    brandGroups[brand].push(u);
  }

  // Interleave to get maximum diversity
  const shuffledUrls = [];
  let round = 0;
  while (shuffledUrls.length < uniqueUrls.length) {
    let added = false;
    for (const b of Object.keys(brandGroups)) {
      if (brandGroups[b][round]) {
        shuffledUrls.push(brandGroups[b][round]);
        added = true;
      }
    }
    if (!added) break;
    round++;
  }

  console.log(`Bắt đầu cào song song 12 luồng với timeout 4s (Mục tiêu: ĐỦ 200 SẢN PHẨM)...`);

  const TARGET_COUNT = 200;
  const results = [];
  const concurrency = 12;
  let index = 0;

  async function worker() {
    while (index < shuffledUrls.length && results.length < TARGET_COUNT) {
      const curIdx = index++;
      const url = shuffledUrls[curIdx];
      const prod = await extractProduct(url);
      if (prod && prod.donGia > 0 && prod.anhChinh) {
        prod.maHang = `MH${String(100 + results.length + 1).padStart(3, '0')}`;
        results.push(prod);
        if (results.length % 20 === 0 || results.length === TARGET_COUNT) {
          console.log(`[TIẾN ĐỘ] Đã cào thành công: ${results.length}/${TARGET_COUNT} sản phẩm`);
        }
      }
    }
  }

  const workers = Array(concurrency).fill(0).map(() => worker());
  await Promise.all(workers);

  console.log(`\n🎉 HOÀN TẤT THÀNH CÔNG: Đã cào được ${results.length} sản phẩm đa dạng mẫu mã!`);

  // 1. Save JSON
  const metaMap = {};
  for (const p of results) {
    metaMap[p.maHang] = {
      maHang: p.maHang,
      tenHang: p.tenHang,
      nhanHieu: p.nhanHieu,
      maLoai: p.maLoai,
      maNhom: p.maNhom,
      tenNhom: p.tenNhom,
      donGia: p.donGia,
      giaGoc: p.giaGoc,
      anhChinh: p.anhChinh,
      mauSac: p.mauSac,
      phienBan: p.phienBan,
      thongSo: p.thongSo
    };
  }

  const jsonPath = path.join(__dirname, '..', 'frontend', 'products-meta.json');
  fs.writeFileSync(jsonPath, JSON.stringify(metaMap, null, 2), 'utf8');
  console.log(`✅ 1. Ghi file JSON: ${jsonPath} (${Object.keys(metaMap).length} sản phẩm)`);

  // 2. Save SQL seed
  const sqlLines = [
    '-- ============================================================',
    `-- SEED ${results.length} SẢN PHẨM ĐA DẠNG MẪU MÃ (HOÀNG HÀ MOBILE)`,
    '-- ============================================================',
    ''
  ];

  const nhomUsed = [...new Set(results.map(r => JSON.stringify({ code: r.maNhom, name: r.tenNhom })))].map(s => JSON.parse(s));
  for (const n of nhomUsed) {
    sqlLines.push(`INSERT INTO NhomHang (MANHOM, TENNHOM) VALUES ('${n.code}', '${n.name}') ON CONFLICT (MANHOM) DO NOTHING;`);
  }
  sqlLines.push('');

  const brandsUsed = [...new Set(results.map(r => r.maLoai))];
  for (const b of brandsUsed) {
    const brandName = results.find(r => r.maLoai === b)?.nhanHieu || b;
    sqlLines.push(`INSERT INTO LoaiHang (MALOAI, TENLOAI) VALUES ('${b}', '${brandName}') ON CONFLICT (MALOAI) DO NOTHING;`);
  }
  sqlLines.push('');

  for (const p of results) {
    const cleanTen = p.tenHang.replace(/'/g, "''");
    sqlLines.push(`INSERT INTO MatHang (MAHANG, TENHANG, DONGIA, DVT, MANHOM, MALOAI, QUANLYIMEI, THOIHANBAOHANH) VALUES ('${p.maHang}', '${cleanTen}', ${p.donGia}, 'Cái', '${p.maNhom}', '${p.maLoai}', true, 12) ON CONFLICT (MAHANG) DO UPDATE SET TENHANG = EXCLUDED.TENHANG, DONGIA = EXCLUDED.DONGIA;`);
  }

  const sqlPath = path.join(__dirname, '..', 'database', 'seed-products-200.sql');
  fs.writeFileSync(sqlPath, sqlLines.join('\n'), 'utf8');
  console.log(`✅ 2. Ghi file SQL: ${sqlPath} (${results.length} dòng INSERT)`);
}

main().catch(err => console.error('Crawler failed:', err));
