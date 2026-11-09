const fs = require('fs');
const path = require('path');
const { parseHoangHaDetail } = require('./hoangha-detail-parser');

const META_FILE = path.resolve(__dirname, '../frontend/data/products-meta.json');
const meta = JSON.parse(fs.readFileSync(META_FILE, 'utf8'));

// Priority list: Phones, Tablets, Laptops first
const keys = Object.keys(meta);
const prioritized = keys.sort((a, b) => {
  const pa = meta[a];
  const pb = meta[b];
  const score = (p) => {
    if (p.maNhom === 'DIENTHOAI') return 100;
    if (p.maNhom === 'TABLET') return 90;
    if (p.maNhom === 'LAPTOP') return 80;
    if (p.maNhom === 'DONGHO') return 70;
    if (p.maNhom === 'AMTHANH') return 60;
    return 10;
  };
  return score(pb) - score(pa);
});

console.log(`Starting crawl for ${prioritized.length} products...`);

let processed = 0;
let successCount = 0;
let failCount = 0;

async function fetchWithRetry(url, retries = 2) {
  for (let attempt = 1; attempt <= retries; attempt++) {
    try {
      const res = await fetch(url, {
        headers: {
          'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36'
        }
      });
      if (res.status === 200) {
        return await res.text();
      }
      if (res.status === 404) return null;
    } catch (e) {
      if (attempt === retries) throw e;
      await new Promise(r => setTimeout(r, 500));
    }
  }
  return null;
}

async function processProduct(id) {
  const prod = meta[id];
  if (!prod.slug) return;
  const url = 'https://hoanghamobile.com' + prod.slug;
  try {
    const html = await fetchWithRetry(url);
    if (!html) {
      failCount++;
      return;
    }

    const parsed = parseHoangHaDetail(html);

    // Merge parsed data into product
    if (parsed.album && parsed.album.length > 0) {
      prod.album = parsed.album;
    }
    if (parsed.mauSac && parsed.mauSac.length > 0) {
      // Keep hex if existed, or generate hex based on color name
      prod.mauSac = parsed.mauSac.map(c => {
        let hex = '#2563EB';
        const tn = c.ten.toLowerCase();
        if (tn.includes('trắng') || tn.includes('white') || tn.includes('bạc') || tn.includes('silver') || tn.includes('starlight')) hex = '#F8FAFC';
        else if (tn.includes('đen') || tn.includes('black') || tn.includes('midnight') || tn.includes('tối')) hex = '#1E293B';
        else if (tn.includes('xanh dương') || tn.includes('blue') || tn.includes('hải quân')) hex = '#38BDF8';
        else if (tn.includes('xanh lá') || tn.includes('green') || tn.includes('rêu')) hex = '#22C55E';
        else if (tn.includes('tím') || tn.includes('purple')) hex = '#A855F7';
        else if (tn.includes('vàng') || tn.includes('gold') || tn.includes('sa mạc') || tn.includes('desert')) hex = '#F59E0B';
        else if (tn.includes('đỏ') || tn.includes('red') || tn.includes('burgundy')) hex = '#EF4444';
        else if (tn.includes('hồng') || tn.includes('pink')) hex = '#EC4899';
        else if (tn.includes('xám') || tn.includes('gray') || tn.includes('titanium')) hex = '#64748B';
        return {
          id: c.id,
          ten: c.ten,
          hex: hex,
          anh: c.anh
        };
      });
    }
    if (parsed.video) {
      prod.video = parsed.video;
      prod.videoUrl = parsed.video;
    }
    if (parsed.thongSoNoiBat && Object.keys(parsed.thongSoNoiBat).length > 0) {
      prod.thongSoNoiBat = parsed.thongSoNoiBat;
    }
    if (parsed.rating) {
      prod.rating = parsed.rating;
    }

    successCount++;
  } catch (err) {
    failCount++;
  } finally {
    processed++;
    if (processed % 20 === 0 || processed === prioritized.length) {
      console.log(`[Progress] Processed: ${processed}/${prioritized.length} | Success: ${successCount} | Failed: ${failCount}`);
      fs.writeFileSync(META_FILE, JSON.stringify(meta, null, 2), 'utf8');
    }
  }
}

// Concurrency pool
async function run() {
  const CONCURRENCY = 6;
  const queue = [...prioritized];
  
  async function worker() {
    while (queue.length > 0) {
      const id = queue.shift();
      await processProduct(id);
      await new Promise(r => setTimeout(r, 80)); // Polite spacing
    }
  }

  const workers = Array.from({ length: CONCURRENCY }, () => worker());
  await Promise.all(workers);

  // Final write
  fs.writeFileSync(META_FILE, JSON.stringify(meta, null, 2), 'utf8');
  console.log(`\n🎉 COMPLETED! Total enriched: ${successCount} products saved to ${META_FILE}`);
}

run();
