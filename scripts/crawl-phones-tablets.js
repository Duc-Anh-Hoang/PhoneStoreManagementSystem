const fs = require('fs');
const path = require('path');
const { parseHoangHaDetail } = require('./hoangha-detail-parser');

const META_FILE = path.resolve(__dirname, '../frontend/data/products-meta.json');
const meta = JSON.parse(fs.readFileSync(META_FILE, 'utf8'));

const targetKeys = Object.keys(meta).filter(k => {
  const p = meta[k];
  const isTarget = p.maNhom === 'DIENTHOAI' || p.maNhom === 'TABLET';
  const notEnriched = !p.thongSoNoiBat || Object.keys(p.thongSoNoiBat).length === 0;
  return isTarget && notEnriched;
});

console.log(`Starting crawl for ${targetKeys.length} phones & tablets...`);

let done = 0;
let success = 0;
let fail = 0;

async function fetchPage(url) {
  try {
    const res = await fetch(url, {
      headers: {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
      }
    });
    if (res.status === 200) return await res.text();
    return null;
  } catch(e) {
    return null;
  }
}

async function processItem(id) {
  const prod = meta[id];
  if (!prod.slug) return;
  const url = 'https://hoanghamobile.com' + prod.slug;
  const html = await fetchPage(url);
  if (html) {
    const parsed = parseHoangHaDetail(html);
    if (parsed.album && parsed.album.length > 0) prod.album = parsed.album;
    if (parsed.mauSac && parsed.mauSac.length > 0) {
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
        return { id: c.id, ten: c.ten, hex: hex, anh: c.anh };
      });
    }
    if (parsed.video) {
      prod.video = parsed.video;
      prod.videoUrl = parsed.video;
    }
    if (parsed.thongSoNoiBat && Object.keys(parsed.thongSoNoiBat).length > 0) {
      prod.thongSoNoiBat = parsed.thongSoNoiBat;
    }
    if (parsed.rating) prod.rating = parsed.rating;
    success++;
  } else {
    fail++;
  }
  done++;
  if (done % 15 === 0 || done === targetKeys.length) {
    console.log(`[Progress] ${done}/${targetKeys.length} (Success: ${success}, Fail: ${fail})`);
    fs.writeFileSync(META_FILE, JSON.stringify(meta, null, 2), 'utf8');
  }
}

async function run() {
  const queue = [...targetKeys];
  async function worker() {
    while (queue.length > 0) {
      const id = queue.shift();
      await processItem(id);
      await new Promise(r => setTimeout(r, 200));
    }
  }
  // Run 2 parallel workers
  await Promise.all([worker(), worker()]);
  fs.writeFileSync(META_FILE, JSON.stringify(meta, null, 2), 'utf8');
  console.log(`\nDONE! Success: ${success}, Fail: ${fail}`);
}

run();
