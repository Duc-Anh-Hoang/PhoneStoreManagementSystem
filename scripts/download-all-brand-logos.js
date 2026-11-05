const fs = require('fs');
const path = require('path');

function slugify(text) {
  return text
    .toString()
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-z0-9]+/g, '-')
    .replace(/(^-|-$)+/g, '');
}

async function getAllBrands() {
  const brands = new Map();

  const catPages = [
    '/dien-thoai-di-dong',
    '/dong-ho',
    '/laptop',
    '/tablet',
    '/man-hinh',
    '/loa-tai-nghe',
    '/phu-kien',
    '/dien-may',
    '/smart-home',
    '/kho-san-pham-cu'
  ];

  for (const cat of catPages) {
    try {
      const res = await fetch(`https://hoanghamobile.com${cat}`, {
        headers: {
          'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
        }
      });
      const html = await res.text();
      const btnMatches = [...html.matchAll(/data-facet="brand"[\s\S]*?<img[^>]*alt="([^"]+)"[^>]*src="([^"]+)"/gi)];
      for (const m of btnMatches) {
        const name = m[1].trim();
        const url = m[2].split(';')[0];
        if (!brands.has(name.toLowerCase())) {
          brands.set(name.toLowerCase(), { name, url });
        }
      }
      const titleMatches = [...html.matchAll(/title="([^"]+)"[^>]*data-facet="brand"[\s\S]*?<img[^>]*src="([^"]+)"/gi)];
      for (const m of titleMatches) {
        const name = m[1].trim();
        const url = m[2].split(';')[0];
        if (!brands.has(name.toLowerCase())) {
          brands.set(name.toLowerCase(), { name, url });
        }
      }
    } catch (e) {
      console.error(cat, e.message);
    }
  }

  // Mega Menu brands
  try {
    const res = await fetch('https://hoanghamobile.com/api/nav/mega-menu');
    const data = await res.json();
    function walk(item) {
      if (item.imageUrl && item.title) {
        const isBrand = item.imageUrl.includes('brand') || 
                        (item.link && (item.link.includes('hang-san-xuat') || item.link.includes('iphone') || item.link.includes('samsung') || item.link.includes('xiaomi')));
        if (isBrand) {
          const name = item.title.replace(/^Laptop\s+|^Máy tính bảng\s+|^Điện thoại\s+/i, '').trim();
          const url = item.imageUrl.split(';')[0];
          if (!brands.has(name.toLowerCase())) {
            brands.set(name.toLowerCase(), { name, url });
          }
        }
      }
      if (item.children) item.children.forEach(walk);
    }
    data.items.forEach(walk);
  } catch (e) {
    console.error('Mega menu error:', e);
  }

  return brands;
}

async function run() {
  const brands = await getAllBrands();
  console.log(`Found ${brands.size} candidate brands.`);

  const dirBrands = path.resolve(__dirname, '../frontend/images/brands');
  const dirBranchs = path.resolve(__dirname, '../frontend/images/branchs');

  if (!fs.existsSync(dirBrands)) fs.mkdirSync(dirBrands, { recursive: true });
  if (!fs.existsSync(dirBranchs)) fs.mkdirSync(dirBranchs, { recursive: true });

  // Blacklist generic category names that are not brands
  const blacklist = [
    'dien-thoai',
    'laptop',
    'tablet',
    'man-hinh',
    'dong-ho',
    'dong-ho-tre-em',
    'man-hinh-iphone',
    'phu-kien',
    'phu-kien-dien-thoai',
    'phu-kien-laptop',
    'tai-nghe-xiaomi',
    'may-tinh-bang',
    'robot-may-hut-bui'
  ];

  let successCount = 0;
  for (const [key, item] of brands.entries()) {
    const slug = slugify(item.name);
    if (blacklist.includes(slug) || slug.length < 2) continue;

    let imgUrl = item.url;
    if (!imgUrl.startsWith('http')) {
      imgUrl = 'https://i.hoanghamobile.vn' + imgUrl;
    }

    try {
      const res = await fetch(imgUrl, {
        headers: { 'User-Agent': 'Mozilla/5.0' }
      });
      if (!res.ok) {
        console.warn(`Failed fetching ${item.name} (${imgUrl}): ${res.status}`);
        continue;
      }
      const buffer = Buffer.from(await res.arrayBuffer());
      if (buffer.length < 100) continue;

      const destBrands = path.join(dirBrands, `${slug}.png`);
      const destBranchs = path.join(dirBranchs, `${slug}.png`);

      fs.writeFileSync(destBrands, buffer);
      fs.writeFileSync(destBranchs, buffer);
      successCount++;
      console.log(`[✓] Downloaded ${item.name} -> ${slug}.png (${buffer.length} bytes)`);
    } catch (err) {
      console.error(`Error downloading ${item.name}:`, err.message);
    }
  }

  console.log(`\nSuccessfully downloaded ${successCount} brand logos to frontend/images/brands/ and frontend/images/branchs/`);
}

run().catch(console.error);
