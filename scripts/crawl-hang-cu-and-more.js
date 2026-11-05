const fs = require('fs');
const path = require('path');

function decodeHtmlEntities(str) {
  if (!str) return '';
  return str
    .replace(/&#x([0-9a-fA-F]+);/g, (_, hex) => String.fromCharCode(parseInt(hex, 16)))
    .replace(/&#(\d+);/g, (_, dec) => String.fromCharCode(parseInt(dec, 10)))
    .replace(/&quot;/g, '"')
    .replace(/&apos;/g, "'")
    .replace(/&amp;/g, '&')
    .replace(/&lt;/g, '<')
    .replace(/&gt;/g, '>')
    .replace(/&nbsp;/g, ' ')
    .trim();
}

function detectBrand(title) {
  const t = title.toLowerCase();
  if (t.includes('iphone') || t.includes('ipad') || t.includes('macbook') || t.includes('apple') || t.includes('airpods') || t.includes('apple watch')) {
    return { maLoai: 'APPLE', nhanHieu: 'Apple' };
  }
  if (t.includes('samsung') || t.includes('galaxy')) return { maLoai: 'SAMSUNG', nhanHieu: 'Samsung' };
  if (t.includes('xiaomi') || t.includes('redmi') || t.includes('poco') || t.includes('mova') || t.includes('roborock') || t.includes('dreame')) return { maLoai: 'XIAOMI', nhanHieu: 'Xiaomi' };
  if (t.includes('oppo')) return { maLoai: 'OPPO', nhanHieu: 'OPPO' };
  if (t.includes('vivo')) return { maLoai: 'VIVO', nhanHieu: 'Vivo' };
  if (t.includes('realme')) return { maLoai: 'REALME', nhanHieu: 'Realme' };
  if (t.includes('garmin')) return { maLoai: 'GARMIN', nhanHieu: 'Garmin' };
  if (t.includes('amazfit')) return { maLoai: 'AMAZFIT', nhanHieu: 'Amazfit' };
  if (t.includes('huawei')) return { maLoai: 'HUAWEI', nhanHieu: 'Huawei' };
  if (t.includes('marshall')) return { maLoai: 'MARSHALL', nhanHieu: 'Marshall' };
  if (t.includes('jbl')) return { maLoai: 'JBL', nhanHieu: 'JBL' };
  if (t.includes('sony')) return { maLoai: 'SONY', nhanHieu: 'Sony' };
  if (t.includes('harman')) return { maLoai: 'HARMAN', nhanHieu: 'Harman Kardon' };
  if (t.includes('asus') || t.includes('rog')) return { maLoai: 'ASUS', nhanHieu: 'Asus' };
  if (t.includes('acer')) return { maLoai: 'ACER', nhanHieu: 'Acer' };
  if (t.includes('dell')) return { maLoai: 'DELL', nhanHieu: 'Dell' };
  if (t.includes('hp ') || t.startsWith('hp')) return { maLoai: 'HP', nhanHieu: 'HP' };
  if (t.includes('lenovo')) return { maLoai: 'LENOVO', nhanHieu: 'Lenovo' };
  if (t.includes('msi')) return { maLoai: 'MSI', nhanHieu: 'MSI' };
  if (t.includes('viewsonic')) return { maLoai: 'VIEWSONIC', nhanHieu: 'ViewSonic' };
  if (t.includes('lg ') || t.startsWith('lg')) return { maLoai: 'LG', nhanHieu: 'LG' };
  if (t.includes('tp-link') || t.includes('tplink')) return { maLoai: 'TPLINK', nhanHieu: 'TP-Link' };
  if (t.includes('sandisk')) return { maLoai: 'SANDISK', nhanHieu: 'SanDisk' };
  if (t.includes('anker')) return { maLoai: 'ANKER', nhanHieu: 'Anker' };
  if (t.includes('baseus')) return { maLoai: 'BASEUS', nhanHieu: 'Baseus' };
  if (t.includes('ugreen')) return { maLoai: 'UGREEN', nhanHieu: 'Ugreen' };
  if (t.includes('logitech')) return { maLoai: 'LOGITECH', nhanHieu: 'Logitech' };
  if (t.includes('zagg')) return { maLoai: 'ZAGG', nhanHieu: 'ZAGG' };
  if (t.includes('kingbull') || t.includes('mipow')) return { maLoai: 'MIPOW', nhanHieu: 'MiPow' };
  if (t.includes('uag')) return { maLoai: 'UAG', nhanHieu: 'UAG' };
  if (t.includes('spigen')) return { maLoai: 'SPIGEN', nhanHieu: 'Spigen' };

  return { maLoai: 'KHAC', nhanHieu: 'Chính hãng' };
}

function extractVersion(title) {
  const capMatch = title.match(/(\d+\s*(?:GB|TB))/i);
  return capMatch ? capMatch[1].toUpperCase().replace(/\s+/, '') : 'Tiêu chuẩn';
}

function extractColors(title, imgUrl) {
  const colorMap = [
    { name: 'Đen', hex: '#1F2937', keywords: ['đen', 'black', 'midnight', 'dark'] },
    { name: 'Trắng', hex: '#F3F4F6', keywords: ['trắng', 'white', 'starlight', 'silver', 'bạc'] },
    { name: 'Xanh dương', hex: '#2563EB', keywords: ['xanh dương', 'blue', 'navy', 'cyan'] },
    { name: 'Xanh lá', hex: '#10B981', keywords: ['xanh lá', 'green', 'mint'] },
    { name: 'Vàng', hex: '#F59E0B', keywords: ['vàng', 'gold', 'yellow'] },
    { name: 'Tím', hex: '#8B5CF6', keywords: ['tím', 'purple'] },
    { name: 'Hồng', hex: '#EC4899', keywords: ['hồng', 'pink'] },
    { name: 'Titan Tự Nhiên', hex: '#9CA3AF', keywords: ['natural titanium', 'titan tự nhiên', 'titan'] },
    { name: 'Đỏ', hex: '#EF4444', keywords: ['đỏ', 'red'] }
  ];

  const t = title.toLowerCase();
  const matched = colorMap.find(c => c.keywords.some(k => t.includes(k)));
  if (matched) {
    return [{ id: 'c_default', ten: matched.name, hex: matched.hex, anh: imgUrl }];
  }
  return [{ id: 'c_default', ten: 'Tiêu chuẩn', hex: '#3B82F6', anh: imgUrl }];
}

const CATEGORIES = [
  // Hàng cũ giá tốt
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/dien-thoai' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/laptop' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/may-tinh-bang' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/man-hinh' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/dong-ho-thong-minh' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/tai-nghe' },
  { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt', url: '/kho-san-pham-cu/loa' },

  // Phụ kiện chuyên sâu
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/the-nho-usb' },
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/op-lung-dien-thoai' },
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/phu-kien/mieng-dan-man-hinh' },
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/phu-kien/thiet-bi-mang' },
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/phu-kien/day-deo-dong-ho' },
  { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện hiện đại', url: '/do-choi-cong-nghe/quat-cam-tay-mini' }
];

async function crawlCategory(cat, globalSeenHref) {
  const products = [];
  let page = 1;

  while (page <= 5) {
    const fullUrl = `https://hoanghamobile.com${cat.url}?partial=listing&p=${page}`;
    try {
      const res = await fetch(fullUrl, {
        headers: {
          'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36',
          'X-Requested-With': 'XMLHttpRequest',
          'Accept': 'text/html, */*; q=0.01',
          'Referer': `https://hoanghamobile.com${cat.url}`
        }
      });
      if (!res.ok) break;
      const html = await res.text();
      const articles = html.split('<article').slice(1);
      if (articles.length === 0) break;

      let newCount = 0;
      for (const rawArt of articles) {
        const art = '<article' + rawArt.split('</article>')[0];
        
        // Match link and title: flexible matching for regular or kho-san-pham-cu
        let href = '';
        let title = '';
        
        const m1 = art.match(/<a[^>]*href="([^"]+)"[^>]*aria-label="([^"]+)"/);
        if (m1) {
          href = m1[1];
          title = decodeHtmlEntities(m1[2]).trim();
        } else {
          const m2 = art.match(/<a[^>]*class="[^"]*line-clamp-2[^"]*"[^>]*href="([^"]+)"[^>]*>([\s\S]*?)<\/a>/);
          if (m2) {
            href = m2[1];
            title = decodeHtmlEntities(m2[2]).trim();
          }
        }

        if (!href || !title) continue;
        if (globalSeenHref.has(href)) continue;
        globalSeenHref.add(href);

        // image
        const imgMatch = art.match(/<img[^>]*src="([^"]+)"/);
        let img = imgMatch ? imgMatch[1].split(';')[0] : '';
        if (img && !img.startsWith('http')) {
          img = 'https://i.hoanghamobile.vn' + img;
        }

        // price
        const priceMatch = art.match(/text-danger-900[^>]*>[\s\S]*?([0-9.,]{4,})/);
        const lastPriceMatch = art.match(/line-through[^>]*>\s*([0-9.,]{4,})/);
        let price = priceMatch ? parseInt(priceMatch[1].replace(/[.,]/g, ''), 10) : 0;
        let lastPrice = lastPriceMatch && lastPriceMatch[1].trim() ? parseInt(lastPriceMatch[1].replace(/[.,]/g, ''), 10) : 0;
        
        if (!price || price === 0) {
          // fallback data-product-price
          const pMatch = art.match(/data-product-price[^>]*>\s*([0-9.,]+)/);
          if (pMatch) price = parseInt(pMatch[1].replace(/[.,]/g, ''), 10);
        }
        if (!price || price === 0) continue;
        if (!lastPrice || lastPrice <= price) {
          lastPrice = Math.round(price * 1.08);
        }

        // specs
        const specs = {};
        const specDlMatch = art.match(/<dl[^>]*>([\s\S]*?)<\/dl>/i);
        if (specDlMatch) {
          const items = [...specDlMatch[1].matchAll(/title="([^:]+):\s*([^"]+)"/g)];
          items.forEach(it => {
            const k = decodeHtmlEntities(it[1].trim());
            const v = decodeHtmlEntities(it[2].trim());
            specs[k] = v;
          });
        }

        const brand = detectBrand(title);
        const version = extractVersion(title);
        const colors = extractColors(title, img);

        products.push({
          tenHang: title,
          nhanHieu: brand.nhanHieu,
          maLoai: brand.maLoai,
          maNhom: cat.maNhom,
          tenNhom: cat.tenNhom,
          donGia: price,
          giaGoc: lastPrice,
          anhChinh: img,
          mauSac: colors,
          phienBan: [
            {
              id: 'v_default',
              dungLuong: version,
              gia: price,
              giaGoc: lastPrice
            }
          ],
          thongSo: specs,
          slug: href
        });
        newCount++;
      }

      if (newCount === 0) break;
      page++;
      await new Promise(r => setTimeout(r, 350));
    } catch (e) {
      console.error(`Error on ${cat.url} page ${page}:`, e.message);
      break;
    }
  }

  console.log(`[CRAWL] ${cat.tenNhom} (${cat.url}): ${products.length} products added.`);
  return products;
}

async function run() {
  console.log('Loading existing products-meta.json...');
  const metaPath = path.resolve(__dirname, '../frontend/products-meta.json');
  const existingMeta = JSON.parse(fs.readFileSync(metaPath, 'utf8'));
  const existingList = Object.values(existingMeta);
  console.log(`Current products count: ${existingList.length}`);

  const globalSeenHref = new Set();
  existingList.forEach(p => {
    if (p.slug) globalSeenHref.add(p.slug);
  });

  const allProducts = [...existingList];

  for (const cat of CATEGORIES) {
    const list = await crawlCategory(cat, globalSeenHref);
    allProducts.push(...list);
    await new Promise(r => setTimeout(r, 500));
  }

  console.log(`Total combined products: ${allProducts.length}`);

  // Re-assign sequential maHang MH101 -> MH...
  const metaObj = {};
  allProducts.forEach((p, idx) => {
    const maHang = `MH${101 + idx}`;
    p.maHang = maHang;
    metaObj[maHang] = p;
  });

  // Save to frontend/products-meta.json
  fs.writeFileSync(metaPath, JSON.stringify(metaObj, null, 2), 'utf8');
  console.log(`Saved ${allProducts.length} products to ${metaPath}`);

  // Generate database/seed-products-200.sql
  generateSql(allProducts);
}

function generateSql(products) {
  const nhomHangList = [
    { maNhom: 'DIENTHOAI', tenNhom: 'Điện thoại' },
    { maNhom: 'TABLET', tenNhom: 'Máy tính bảng' },
    { maNhom: 'AMTHANH', tenNhom: 'Âm thanh' },
    { maNhom: 'PHUKIEN', tenNhom: 'Phụ kiện' },
    { maNhom: 'DONGHO', tenNhom: 'Đồng hồ' },
    { maNhom: 'LAPTOP', tenNhom: 'Laptop & Màn hình' },
    { maNhom: 'DIENMAY', tenNhom: 'Tivi, Điện máy' },
    { maNhom: 'SMARTHOME', tenNhom: 'Smart home, Camera' },
    { maNhom: 'HANGCU', tenNhom: 'Hàng cũ giá tốt' }
  ];

  const loaiHangMap = new Map();
  products.forEach(p => {
    if (!loaiHangMap.has(p.maLoai)) {
      loaiHangMap.set(p.maLoai, { maLoai: p.maLoai, tenLoai: p.nhanHieu, maNhom: p.maNhom });
    }
  });

  let sql = `-- ==================================================================\n`;
  sql += `-- Auto-generated seed data crawled from HoangHa Mobile (${products.length} items)\n`;
  sql += `-- Generated at: ${new Date().toISOString()}\n`;
  sql += `-- ==================================================================\n\n`;

  sql += `-- 1. Insert NhomHang\n`;
  for (const nh of nhomHangList) {
    sql += `IF NOT EXISTS (SELECT 1 FROM NhomHang WHERE MaNhom = N'${nh.maNhom}')\n`;
    sql += `    INSERT INTO NhomHang (MaNhom, TenNhom) VALUES (N'${nh.maNhom}', N'${nh.tenNhom}');\n`;
  }
  sql += `\n`;

  sql += `-- 2. Insert LoaiHang\n`;
  for (const [_, lh] of loaiHangMap.entries()) {
    sql += `IF NOT EXISTS (SELECT 1 FROM LoaiHang WHERE MaLoai = N'${lh.maLoai}')\n`;
    sql += `    INSERT INTO LoaiHang (MaLoai, TenLoai, MaNhom) VALUES (N'${lh.maLoai}', N'${lh.tenLoai}', N'${lh.maNhom}');\n`;
  }
  sql += `\n`;

  sql += `-- 3. Insert MatHang\n`;
  for (const p of products) {
    const safeTen = p.tenHang.replace(/'/g, "''");
    const safeHieu = (p.nhanHieu || 'Chính hãng').replace(/'/g, "''");
    const safeAnh = (p.anhChinh || '').replace(/'/g, "''");
    const donGia = p.donGia || 0;
    const giaNhap = Math.round(donGia * 0.85);

    sql += `IF NOT EXISTS (SELECT 1 FROM MatHang WHERE MaHang = N'${p.maHang}')\n`;
    sql += `    INSERT INTO MatHang (MaHang, TenHang, NhanHieu, DonGia, GiaNhap, SoLuongTon, DonViTinh, MaLoai, HinhAnh, TrangThai)\n`;
    sql += `    VALUES (N'${p.maHang}', N'${safeTen}', N'${safeHieu}', ${donGia}, ${giaNhap}, 50, N'Chiếc', N'${p.maLoai}', N'${safeAnh}', 1);\n`;
  }

  const sqlPath = path.resolve(__dirname, '../database/seed-products-200.sql');
  fs.writeFileSync(sqlPath, sql, 'utf8');
  console.log(`Saved ${products.length} SQL inserts to ${sqlPath}`);
}

run().catch(console.error);
