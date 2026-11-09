/**
 * SAMAMOBILE.COM - RECENTLY VIEWED PRODUCTS CONTROLLER
 * Quản lý và hiển thị danh sách sản phẩm đã xem lưu trong LocalStorage:
 * - Lưu sản phẩm khi xem trang chi tiết (product-detail.html) hoặc khi bấm xem sản phẩm
 * - Hiển thị Khối "Sản phẩm đã xem" trên index.html, category.html, product-detail.html
 * - Popover thả xuống khi rê chuột/bấm vào "Sản phẩm đã xem" trên thanh top bar Header
 * - Nút xóa lịch sử xem sản phẩm
 */

(function () {
  'use strict';

  var STORAGE_KEY = 'samamobile_recently_viewed';

  // Sản phẩm mẫu hiển thị khi người dùng mới vào chưa xem sản phẩm nào
  var DEFAULT_RECOMMENDED = [
    {
      id: 'MH101',
      maHang: 'MH101',
      tenHang: 'iPhone 18 Pro Max 256GB - Chính hãng VN/A',
      donGia: 41490000,
      giaGoc: 44990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
      member_price: 40992000,
      badges: ['A20 Pro', '256GB', '6.9"']
    },
    {
      id: 'MH102',
      maHang: 'MH102',
      tenHang: 'iPhone 18 Pro 256GB - Chính hãng VN/A',
      donGia: 38490000,
      giaGoc: 41569000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-glacier-pdp-images-vi-vn-021533622.png',
      member_price: 38028000,
      badges: ['A20 Pro', '256GB', '6.3"']
    },
    {
      id: 'SAMPLE-dien-thoai-1',
      maHang: 'SAMPLE-dien-thoai-1',
      tenHang: 'Samsung Galaxy S25 Ultra - 12GB/256GB',
      donGia: 25790000,
      giaGoc: 33990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png',
      member_price: 25481000,
      badges: ['Snap 8 Elite', '5000mAh', '256GB']
    },
    {
      id: 'MH249',
      maHang: 'MH249',
      tenHang: 'Nubia V80 Max 6GB/128GB - Chính hãng',
      donGia: 3890000,
      giaGoc: 4290000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/01/08/nubia-v80-max-bac.png',
      member_price: 3835000,
      badges: ['Unisoc T7280', '5000mAh', '6.9"']
    },
    {
      id: 'SAMPLE-laptop-1',
      maHang: 'SAMPLE-laptop-1',
      tenHang: 'MacBook Air 13 M3 2024 - 16GB / 256GB SSD',
      donGia: 27490000,
      giaGoc: 31990000,
      anhChinh: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg',
      member_price: 27150000,
      badges: ['Apple M3', '16GB RAM', '256GB SSD']
    }
  ];

  // Lấy danh sách sản phẩm đã xem từ LocalStorage
  var BROKEN_IMAGE_REMAP = {
    "iphone-16-pro-natural-titan.png": "https://i.hoanghamobile.vn/Uploads/2024/09/10/iphone-16-pro-tu-nhien-1.png",
    "galaxy-s25-ultra-titan-den-1.png": "https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png",
    "galaxy-s24-ultra-xam-1.png": "https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png",
    "galaxy-s24-ultra-vang-1.png": "https://i.hoanghamobile.vn/Uploads/2024/01/16/s24-ultra-vang_638409930027889246.png",
    "ipad-gen-10-silver.png": "https://i.hoanghamobile.vn/Uploads/2025/03/06/ipad-a16-wifi-pink-pdp-image-position-1-vn-vi.png",
    "tab-s9-fe-gray-1.png": "https://i.hoanghamobile.vn/Uploads/2026/10/15/s10-fe-3-134808082.png",
    "macbook-air-m3-midnight-1.png": "https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg",
    "asus-tuf-gaming-f15-fx506hf-hn014w-1.png": "https://i.hoanghamobile.vn/Uploads/2026/06/02/asus-tuf-1.png",
    "man-hinh-gaming-asus-tuf-vg249q3a-1.png": "https://i.hoanghamobile.vn/Uploads/2026/04/06/vg249q5r-1.png",
    "dell-ultrasharp-u2724d-1.png": "https://i.hoanghamobile.vn/Uploads/2024/02/22/u2424he.png",
    "dieu-hoa-casper-inverter-tc-09is36-1.png": "https://i.hoanghamobile.vn/Uploads/MayLanh/4757/Slider/casper-tc-09is35-2-1.jpg",
    "dieu-hoa-daikin-inverter-ftkb35xvmv-1.png": "https://i.hoanghamobile.vn/Uploads/2025/04/16/ftkb25yvmv-1.png",
    "xiaomi-smart-band-9-black-1.png": "https://i.hoanghamobile.vn/Uploads/2024/11/27/n69-pink-side45.png",
    "airpods-4-1.png": "https://i.hoanghamobile.vn/Uploads/2024/09/10/airpods-4.png",
    "marshall-emberton-ii-black-and-brass-1.png": "https://i.hoanghamobile.vn/Uploads/2026/07/30/marshall-stockwell-lll-trang-1-153237088.png",
    "anker-511-nano-3-30w-a2147-1.png": "https://i.hoanghamobile.vn/Uploads/2026/03/21/a1638-den.png"
};

  window.getRecentlyViewed = function () {
    try {
      var data = localStorage.getItem(STORAGE_KEY);
      if (data) {
        var list = JSON.parse(data);
        if (Array.isArray(list) && list.length > 0) {
          var changed = false;
          list.forEach(function (item) {
            if (item && item.anhChinh) {
              for (var k in BROKEN_IMAGE_REMAP) {
                if (item.anhChinh.indexOf(k) !== -1) {
                  item.anhChinh = BROKEN_IMAGE_REMAP[k];
                  changed = true;
                  break;
                }
              }
            }
          });
          if (changed) {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(list));
          }
          return list;
        }
      }
    } catch (e) {
      console.warn('Lỗi đọc recently viewed:', e);
    }
    return [];
  };

  // Thêm sản phẩm vào danh sách đã xem
  window.addRecentlyViewed = function (prod) {
    if (!prod) return;
    try {
      var id = prod.id || prod.maHang;
      if (!id) return;

      var list = window.getRecentlyViewed();
      // Lọc bỏ nếu đã tồn tại để đưa lên đầu danh sách
      list = list.filter(function (item) {
        return (item.id !== id && item.maHang !== id);
      });

      var price = Number(prod.donGia) || Number(prod.price) || 0;
      var oldPrice = Number(prod.giaGoc) || Number(prod.oldPrice) || Math.round(price * 1.15);
      var memberPrice = prod.member_price || Math.round(price * 0.985);

      list.unshift({
        id: id,
        maHang: id,
        tenHang: prod.tenHang || prod.name || 'Sản phẩm công nghệ',
        donGia: price,
        giaGoc: oldPrice,
        member_price: memberPrice,
        anhChinh: prod.anhChinh || prod.image || 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
        badges: prod.image_badges || prod.badges || ['Chính hãng VN/A', 'Bảo hành 12T'],
        thoiGian: Date.now()
      });

      // Lưu tối đa 20 sản phẩm gần nhất
      list = list.slice(0, 20);
      localStorage.setItem(STORAGE_KEY, JSON.stringify(list));

      // Cập nhật lại giao diện
      window.renderRecentlyViewedUI();
    } catch (e) {
      console.warn('Lỗi lưu recently viewed:', e);
    }
  };

  // Xóa toàn bộ lịch sử xem
  window.clearRecentlyViewed = function () {
    if (confirm('Bạn có chắc muốn xóa danh sách các sản phẩm đã xem không?')) {
      localStorage.removeItem(STORAGE_KEY);
      window.renderRecentlyViewedUI();
    }
  };

  // Xóa 1 sản phẩm khỏi lịch sử
  window.removeRecentlyViewedItem = function (id) {
    try {
      var list = window.getRecentlyViewed();
      list = list.filter(function (item) {
        return item.id !== id && item.maHang !== id;
      });
      localStorage.setItem(STORAGE_KEY, JSON.stringify(list));
      window.renderRecentlyViewedUI();
    } catch (e) {}
  };

  // Cuộn mượt đến khối sản phẩm đã xem trên trang
  window.scrollToRecentlyViewed = function () {
    var el = document.getElementById('shelf-recently-viewed') || document.getElementById('empty-cart-recently-viewed-section');
    if (el && !el.classList.contains('hidden') && el.offsetParent !== null) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    } else {
      var isInsidePages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
      var targetUrl = (isInsidePages ? '../' : '') + 'index.html#shelf-recently-viewed';
      window.location.href = targetUrl;
    }
  };

  // Render toàn bộ giao diện: Khối trên trang + Popover thả xuống
  window.renderRecentlyViewedUI = function () {
    var rawList = window.getRecentlyViewed();
    var hasCustom = rawList.length > 0;
    var displayList = hasCustom ? rawList : DEFAULT_RECOMMENDED;

    // 1. Cập nhật Badge đếm số lượng trên thanh Header
    var badges = document.querySelectorAll('.badge-viewed-count');
    badges.forEach(function (b) {
      if (hasCustom) {
        b.textContent = rawList.length;
        b.classList.remove('hidden');
      } else {
        b.classList.add('hidden');
      }
    });

    var counters = document.querySelectorAll('.recently-viewed-counter');
    counters.forEach(function (c) {
      c.textContent = hasCustom ? rawList.length : 'Gợi ý';
    });

    // 2. Render Khối trên trang (#recently-viewed-grid) - Đồng bộ 100% với Sản phẩm tương tự
    var grids = document.querySelectorAll('.recently-viewed-grid');
    grids.forEach(function (grid) {
      grid.innerHTML = displayList.map(function (p) {
        var price = Number(p.donGia) || 0;
        var oldPrice = Number(p.giaGoc) || Math.round(price * 1.15);
        var discount = oldPrice > price ? Math.round(((oldPrice - price) / oldPrice) * 100) : 0;
        var pId = p.maHang || p.id;
        var isPages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
        var targetUrl = (isPages ? '' : 'pages/') + 'product-detail.html?id=' + encodeURIComponent(pId);
        var img = p.anhChinh || 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png';

        return `
          <div class="product-card bg-white rounded-2xl border border-slate-200/90 p-3 flex flex-col justify-between hover:border-blue-400 hover:shadow-md transition-all cursor-pointer group relative" onclick="window.location.href='${targetUrl}'">
            
            ${hasCustom ? `
              <button type="button" onclick="event.stopPropagation(); window.removeRecentlyViewedItem('${pId}')" class="absolute top-1.5 right-1.5 w-5 h-5 rounded-full bg-slate-100 hover:bg-rose-100 text-slate-400 hover:text-rose-600 flex items-center justify-center text-[9px] transition-colors z-20 shadow-xs cursor-pointer" title="Xóa khỏi lịch sử">
                <i class="fa-solid fa-xmark"></i>
              </button>
            ` : ''}

            <div class="relative w-full aspect-square flex items-center justify-center p-2 mb-2">
              <img src="${img}" alt="${p.tenHang}" class="max-w-full max-h-full object-contain group-hover:scale-105 transition-transform" loading="lazy" onerror="this.onerror=null; this.src='../images/default-phone.png';" />
              ${discount > 0 ? `
                <span class="absolute top-1 left-1 px-1.5 py-0.5 rounded-md bg-rose-500 text-white font-bold text-[9px]">-${discount}%</span>
              ` : ''}
            </div>

            <div>
              <h4 class="text-xs font-bold text-slate-800 line-clamp-2 leading-snug group-hover:text-blue-600 transition-colors" title="${p.tenHang}">${p.tenHang}</h4>
              <div class="text-xs sm:text-sm font-semibold text-rose-600 tabular-nums mt-1">${formatCurrency(price)}</div>
              ${oldPrice > price ? `
                <div class="text-[10px] text-slate-400 line-through">${formatCurrency(oldPrice)}</div>
              ` : ''}
            </div>

          </div>
        `;
      }).join('');
    });

    // 3. Render Popover Header nếu có (#recently-viewed-popover-list)
    var popoverList = document.getElementById('recently-viewed-popover-list');
    if (popoverList) {
      if (hasCustom) {
        popoverList.innerHTML = rawList.slice(0, 4).map(function (p) {
          var price = Number(p.donGia) || 0;
          var pId = p.maHang || p.id;
          var isPages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
          var targetUrl = (isPages ? '' : 'pages/') + 'product-detail.html?id=' + encodeURIComponent(pId);
          return `
            <a href="${targetUrl}" class="flex items-center gap-3 p-2.5 rounded-xl hover:bg-slate-50 transition-colors group/item">
              <img src="${p.anhChinh}" alt="${p.tenHang}" class="w-12 h-12 object-contain rounded-lg border border-slate-200 bg-white p-1 flex-shrink-0 shadow-2xs" onerror="this.src='https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png';" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-[13px] font-bold text-slate-900 line-clamp-1 group-hover/item:text-[#0057B7] transition-colors leading-snug">${p.tenHang}</p>
                <p class="text-xs sm:text-[13px] font-black text-[#E11B1E] tabular-nums mt-0.5">${formatCurrency(price)}</p>
              </div>
            </a>
          `;
        }).join('');
      } else {
        popoverList.innerHTML = `
          <div class="py-6 text-center text-slate-400">
            <i class="fa-regular fa-clock text-2xl mb-1 text-slate-300"></i>
            <p class="text-xs font-medium">Bạn chưa xem sản phẩm nào.</p>
          </div>
        `;
      }
    }
  };

  function formatCurrency(val) {
    if (!val || isNaN(val)) return '0 ₫';
    return Number(val).toLocaleString('vi-VN') + '₫';
  }

  // Khởi tạo khi DOM sẵn sàng
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', window.renderRecentlyViewedUI);
  } else {
    window.renderRecentlyViewedUI();
  }

})();
