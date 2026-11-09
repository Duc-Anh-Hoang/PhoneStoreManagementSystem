const fs = require('fs');

function decodeHtml(str) {
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

function parseHoangHaDetail(html) {
  const result = {
    album: [],
    mauSac: [],
    video: null,
    thongSoNoiBat: {},
    rating: null
  };

  // 1. JSON-LD Schema
  const ldMatches = html.match(/<script type="application\/ld\+json">([\s\S]*?)<\/script>/gi) || [];
  let schemaImages = [];
  for (const ld of ldMatches) {
    try {
      const jsonStr = ld.replace(/<script[^>]*>|<\/script>/gi, '');
      const obj = JSON.parse(jsonStr);
      if (obj['@type'] === 'Product') {
        if (Array.isArray(obj.image)) {
          schemaImages = obj.image.map(img => img.split(';')[0]);
        }
        if (obj.aggregateRating) {
          result.rating = {
            score: Number(obj.aggregateRating.ratingValue) || 4.6,
            count: Number(obj.aggregateRating.reviewCount) || 10
          };
        }
      }
    } catch(e) {}
  }

  // 2. Video URL
  const videoMatch = html.match(/data-gallery-video-iframe[^>]*data-embed-url="([^"]+)"/) ||
                     html.match(/data-embed-url="([^"]+)"/);
  if (videoMatch) {
    result.video = decodeHtml(videoMatch[1]);
  }

  // 3. Color variants from gallery controls
  const colorControlRegex = /<button[^>]*data-gallery-color-select[^>]*aria-label="([^"]+)"[^>]*>[\s\S]*?<img[^>]*src="([^"]+)"[\s\S]*?<\/button>/gi;
  let cm;
  const seenColors = new Set();
  while ((cm = colorControlRegex.exec(html)) !== null) {
    const rawColorName = decodeHtml(cm[1]);
    const rawImg = cm[2].split(';')[0];
    const colorKey = rawColorName.toLowerCase();
    if (!seenColors.has(colorKey)) {
      seenColors.add(colorKey);
      result.mauSac.push({
        id: 'c_' + result.mauSac.length,
        ten: rawColorName,
        anh: rawImg
      });
    }
  }

  if (result.mauSac.length === 0) {
    const optRegex = /data-color-name="([^"]+)"[\s\S]*?<img[^>]*src="([^"]+)"/gi;
    let om;
    while ((om = optRegex.exec(html)) !== null) {
      const cname = decodeHtml(om[1]);
      const cimg = om[2].split(';')[0];
      if (!seenColors.has(cname.toLowerCase())) {
        seenColors.add(cname.toLowerCase());
        result.mauSac.push({
          id: 'c_' + result.mauSac.length,
          ten: cname,
          anh: cimg
        });
      }
    }
  }

  // 4. Album angles from swiper slides
  const galleryWrapperMatch = html.match(/<div[^>]*data-gallery-wrapper[^>]*>([\s\S]*?)<\/div>\s*<\/div>/i);
  const slideImgs = [];
  if (galleryWrapperMatch) {
    const imgMatches = [...galleryWrapperMatch[1].matchAll(/<img[^>]*src="([^"]+)"[^>]*alt="([^"]*)"/gi)];
    imgMatches.forEach(m => {
      const src = m[1].split(';')[0];
      if (src && !slideImgs.includes(src)) slideImgs.push(src);
    });
  }

  const allImages = [...slideImgs];
  schemaImages.forEach(img => {
    if (!allImages.includes(img)) allImages.push(img);
  });

  function getAngleLabel(url, index, total) {
    const u = url.toLowerCase();
    if (u.includes('front-s-pen') || (u.includes('front') && u.includes('pen'))) return 'Mặt trước & Bút S-Pen';
    if (u.includes('combo-s-pen') || u.includes('keyboard') || u.includes('ban-phim')) return 'Kèm bao da bàn phím';
    if (u.includes('r30') || u.includes('goc-nghieng') || u.includes('45')) return 'Góc nghiêng 45°';
    if (u.includes('l30') || u.includes('l-side') || u.includes('canh-vien')) return 'Cạnh viền & Cổng kết nối';
    if (u.includes('back') || u.includes('mat-sau') || u.includes('camera')) return 'Mặt sau & Cụm Camera';
    if (u.includes('touchpad')) return 'Bàn phím & Touchpad';
    if (u.includes('pen') || u.includes('s-pen')) return 'Bút S-Pen & Chi tiết';

    const numMatch = u.match(/-(\d+)\.(?:png|jpg|webp)/);
    const num = numMatch ? parseInt(numMatch[1], 10) : (index + 1);
    switch (num) {
      case 1: return 'Mặt trước trực diện';
      case 2: return 'Góc nghiêng 45°';
      case 3: return 'Mặt sau & Cụm Camera';
      case 4: return 'Cạnh viền & Khung máy';
      case 5: return 'Chi tiết cổng kết nối';
      case 6: return 'Góc nhìn nghiêng cạnh';
      case 7: return 'Mặt lưng góc cao';
      case 8: return 'Mặt trước mở sáng';
      default: return 'Góc nhìn ' + num;
    }
  }

  result.album = allImages.slice(0, 10).map((img, i) => ({
    label: getAngleLabel(img, i, allImages.length),
    img: img
  }));

  // 5. Quick Specs
  const quickSpecsMatch = html.match(/<section[^>]*aria-labelledby="quick-specs-title[^"]*"[\s\S]*?<dl[^>]*>([\s\S]*?)<\/dl>/i);
  if (quickSpecsMatch) {
    const dlContent = quickSpecsMatch[1];
    const items = [...dlContent.matchAll(/<dt[^>]*>([\s\S]*?)<\/dt>[\s\S]*?<dd[^>]*>([\s\S]*?)<\/dd>/gi)];
    items.forEach(it => {
      const dt = decodeHtml(it[1].replace(/<[^>]+>/g, '')).trim();
      const dd = decodeHtml(it[2].replace(/<[^>]+>/g, '')).trim();
      if (dt && dd) result.thongSoNoiBat[dt] = dd;
    });
  }

  return result;
}

module.exports = { parseHoangHaDetail, decodeHtml };
