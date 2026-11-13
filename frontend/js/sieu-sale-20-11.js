/**
 * SAMAMOBILE - SIÊU SALE 20/11 TRI ÂN THẦY CÔ
 * Khối Flash Sale Điện thoại & Laptop nền đỏ rực rỡ chuẩn phong cách CellphoneS
 */
(function () {
  'use strict';

  const SALE_2011_DATA = {
    dienthoai: [
      {
        id: 'MH101',
        name: 'iPhone 18 Pro Max 256GB',
        price: 41490000,
        originalPrice: 44809200,
        discount: '-7%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
        badge: 'HSSV -500k',
        sold: 48,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH102',
        name: 'iPhone 18 Pro 256GB',
        price: 38490000,
        originalPrice: 41990000,
        discount: '-8%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-glacier-pdp-images-vi-vn-021533622.png',
        badge: 'Đặc quyền 20/11',
        sold: 35,
        total: 50,
        tag: 'Trợ giá 6Tr'
      },
      {
        id: 'MH110',
        name: 'Samsung Galaxy A27 5G 6GB/128GB',
        price: 7990000,
        originalPrice: 9490000,
        discount: '-16%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/06/24/samsung-a27-xanh-1.png',
        badge: 'Giảm sốc',
        sold: 42,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH106',
        name: 'Xiaomi Redmi Note 17 4GB/128GB',
        price: 5790000,
        originalPrice: 6990000,
        discount: '-17%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/08/17/p19-purple-back11-155153642.png',
        badge: 'HSSV -300k',
        sold: 45,
        total: 50,
        tag: 'Bán chạy'
      },
      {
        id: 'MH109',
        name: 'Samsung Galaxy A18 6GB/128GB',
        price: 6690000,
        originalPrice: 7890000,
        discount: '-15%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/10/05/a18-white-151324306.png',
        badge: 'Đặc quyền 20/11',
        sold: 39,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH104',
        name: 'Nubia Air 5G 8GB/256GB',
        price: 5490000,
        originalPrice: 6490000,
        discount: '-15%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/07/07/back-gold1-171113527.png',
        badge: 'Giá sốc',
        sold: 28,
        total: 50,
        tag: 'Bảo hành 18T'
      },
      {
        id: 'MH107',
        name: 'Xiaomi POCO C95 Pro 6GB/128GB',
        price: 5990000,
        originalPrice: 6990000,
        discount: '-14%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/30/q15ap-green-front1-170835651.png',
        badge: 'HSSV -400k',
        sold: 31,
        total: 50,
        tag: 'Pin 6000mAh'
      },
      {
        id: 'MH108',
        name: 'Samsung Galaxy A08 4GB/64GB',
        price: 4490000,
        originalPrice: 5190000,
        discount: '-13%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/10/02/a08-silver-135259154.jpg',
        badge: 'Cháy hàng',
        sold: 50,
        total: 50,
        tag: 'Chỉ còn vài máy'
      }
    ],
    laptop: [
      {
        id: 'MH296',
        name: 'MacBook Air M5 13 inch 16GB/512GB',
        price: 32990000,
        originalPrice: 36990000,
        discount: '-11%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg',
        badge: 'HSSV -1 Triệu',
        sold: 45,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH297',
        name: 'MacBook Neo 2026 8GB/256GB',
        price: 18490000,
        originalPrice: 21990000,
        discount: '-16%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/03/05/macbook-13-in-a18-pro-citrus-pdp-image-position-1-vn-vi.jpg',
        badge: 'Đặc quyền 20/11',
        sold: 49,
        total: 50,
        tag: 'Chỉ còn 1 suất'
      },
      {
        id: 'MH294',
        name: 'Laptop ASUS TUF Gaming A15 FA506NCG',
        price: 28590000,
        originalPrice: 32990000,
        discount: '-13%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/06/02/asus-tuf-1.png',
        badge: 'Tặng Balo TUF',
        sold: 41,
        total: 50,
        tag: 'RTX Gaming'
      },
      {
        id: 'MH293',
        name: 'Lenovo IdeaPad Slim 3 14IWC11 8GB/512GB',
        price: 17890000,
        originalPrice: 19990000,
        discount: '-11%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/07/16/lenovo-ideapad-slim-3-14iwc11-83rq002pvn-1-150907335.png',
        badge: 'HSSV -500k',
        sold: 38,
        total: 50,
        tag: 'Mỏng nhẹ 1.4kg'
      },
      {
        id: 'MH292',
        name: 'Laptop Lenovo LOQ 15 Gaming 15IRH11E',
        price: 25690000,
        originalPrice: 28990000,
        discount: '-11%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/10/12/lenovo-loq-15irh11e-83y9003vvn-1-171200136.png',
        badge: 'Giảm sốc 20/11',
        sold: 33,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH290',
        name: 'Laptop ASUS Vivobook 14 X1407CA-LY203W',
        price: 23490000,
        originalPrice: 26990000,
        discount: '-13%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/19/asus-vivobook-14-x1407aa-1-091045999.png',
        badge: 'HSSV & GV -800k',
        sold: 29,
        total: 50,
        tag: 'Màn OLED'
      },
      {
        id: 'MH291',
        name: 'Laptop HP AI 15-fd2050wm Core i5',
        price: 16790000,
        originalPrice: 18990000,
        discount: '-12%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/19/ly203w-1-120558555.png',
        badge: 'Tặng Chuột Wireless',
        sold: 36,
        total: 50,
        tag: 'Intel AI'
      },
      {
        id: 'MH289',
        name: 'M-SMART MBOOK 1409A Sinh Viên Giá Rẻ',
        price: 2790000,
        originalPrice: 3490000,
        discount: '-20%',
        image: 'https://i.hoanghamobile.vn/Uploads/2024/07/19/mbook-1409a-1.png',
        badge: 'Học tập online',
        sold: 47,
        total: 50,
        tag: 'Siêu rẻ'
      }
    ],
    apple: [
      {
        id: 'MH101',
        name: 'iPhone 18 Pro Max 256GB',
        price: 41490000,
        originalPrice: 44809200,
        discount: '-7%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
        badge: 'Apple 20/11',
        sold: 48,
        total: 50,
        tag: 'Trả góp 0%'
      },
      {
        id: 'MH102',
        name: 'iPhone 18 Pro 256GB',
        price: 38490000,
        originalPrice: 41990000,
        discount: '-8%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-glacier-pdp-images-vi-vn-021533622.png',
        badge: 'Đặc quyền HSSV',
        sold: 35,
        total: 50,
        tag: 'Trợ giá 6Tr'
      },
      {
        id: 'MH296',
        name: 'MacBook Air M5 13 inch 16GB/512GB',
        price: 32990000,
        originalPrice: 36990000,
        discount: '-11%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/03/04/macbook-air-13-in-m5-starlight-pdp-image-position-1-vn-vi.jpg',
        badge: 'HSSV -1 Triệu',
        sold: 45,
        total: 50,
        tag: 'Apple Silicon'
      },
      {
        id: 'MH297',
        name: 'MacBook Neo 2026 8GB/256GB',
        price: 18490000,
        originalPrice: 21990000,
        discount: '-16%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/03/05/macbook-13-in-a18-pro-citrus-pdp-image-position-1-vn-vi.jpg',
        badge: 'Giá sốc',
        sold: 49,
        total: 50,
        tag: 'Chỉ còn 1 suất'
      },
      {
        id: 'MH298',
        name: 'MacBook Pro M6 14 inch 16GB/1TB',
        price: 54990000,
        originalPrice: 59990000,
        discount: '-8%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/10/28/macbook-pro-14-inch-m6-space-black-pdp-image-position-1-vi-vn-145528508.png',
        badge: 'Đồ họa đỉnh cao',
        sold: 26,
        total: 30,
        tag: 'M6 Pro'
      },
      {
        id: 'MH103',
        name: 'iPhone Duo 256GB Màn Hình Kép',
        price: 64990000,
        originalPrice: 69990000,
        discount: '-7%',
        image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphon-duo-030228624.png',
        badge: 'Đột phá 2026',
        sold: 19,
        total: 20,
        tag: 'Cực hiếm'
      }
    ],
    all: []
  };

  // Ghép toàn bộ sản phẩm hot nhất vào tab "all"
  SALE_2011_DATA.all = [
    SALE_2011_DATA.dienthoai[0],
    SALE_2011_DATA.laptop[0],
    SALE_2011_DATA.dienthoai[1],
    SALE_2011_DATA.laptop[1],
    SALE_2011_DATA.dienthoai[2],
    SALE_2011_DATA.laptop[2],
    SALE_2011_DATA.dienthoai[3],
    SALE_2011_DATA.laptop[3]
  ];

  let currentCategory = 'dienthoai';
  let currentIndex = 0;
  let autoSlideTimer = null;

  function formatMoney(amount) {
    if (!amount || isNaN(amount)) return '0₫';
    return Number(amount).toLocaleString('vi-VN') + '₫';
  }

  function getCardsPerView() {
    const w = window.innerWidth;
    if (w >= 1024) return 5;
    if (w >= 640) return 3;
    return 2;
  }

  function renderSale2011Products() {
    const track = document.getElementById('sale2011-track');
    if (!track) return;

    const list = SALE_2011_DATA[currentCategory] || [];
    track.innerHTML = list.map(item => {
      const percentSold = Math.min(100, Math.round((item.sold / item.total) * 100));
      const isAlmostGone = item.sold >= item.total - 3;
      const progressText = isAlmostGone ? `🔥 Chỉ còn ${item.total - item.sold} suất!` : `🔥 Đã bán ${item.sold}/${item.total} suất`;

      return `
        <div class="w-1/2 sm:w-1/3 lg:w-1/5 flex-shrink-0 px-1 sm:px-2 py-2">
          <div class="bg-white rounded-2xl p-2.5 sm:p-3 border border-amber-200/60 shadow-md hover:shadow-2xl hover:-translate-y-1.5 transition-all duration-300 flex flex-col justify-between h-full group/card cursor-pointer select-none relative" onclick="window.onSale2011CardClick('${item.id}')">
            
            <!-- Badges góc trên -->
            <div class="flex items-center justify-between gap-1 mb-1">
              <span class="bg-gradient-to-r from-red-600 to-rose-600 text-white text-[10px] sm:text-[11px] font-black px-2 py-0.5 rounded-lg shadow-xs">
                ${item.discount}
              </span>
              <span class="bg-amber-50 text-amber-800 border border-amber-200 text-[10px] font-bold px-1.5 py-0.5 rounded-md truncate max-w-[100px]">
                ${item.tag}
              </span>
            </div>

            <!-- Ảnh sản phẩm -->
            <div class="relative w-full h-36 sm:h-44 flex items-center justify-center p-1 my-1 overflow-hidden">
              <img src="${item.image}" alt="${item.name}" class="max-h-full max-w-full object-contain block mx-auto transition-transform duration-300 group-hover/card:scale-105" loading="lazy" onerror="this.src='images/deal-tabs/deal-tab-phone.png';" />
            </div>

            <!-- Thông tin máy -->
            <div class="flex-1 flex flex-col justify-between pt-1">
              <h4 class="font-bold text-xs sm:text-[13px] text-slate-800 line-clamp-2 min-h-[34px] leading-tight group-hover/card:text-[#BA0B1B] transition-colors" title="${item.name}">
                ${item.name}
              </h4>

              <!-- Giá tiền -->
              <div class="mt-1.5">
                <div class="text-[#BA0B1B] font-black text-sm sm:text-base font-mono leading-none">
                  ${formatMoney(item.price)}
                </div>
                ${item.originalPrice ? `<div class="text-slate-400 line-through text-[11px] font-mono mt-0.5">${formatMoney(item.originalPrice)}</div>` : ''}
              </div>

              <!-- Thanh tiến trình Flash Sale -->
              <div class="mt-2">
                <div class="flex items-center justify-between text-[10px] font-bold text-amber-900 mb-0.5">
                  <span class="truncate">${progressText}</span>
                  <span class="text-red-600">${percentSold}%</span>
                </div>
                <div class="w-full bg-red-100 h-2 sm:h-2.5 rounded-full overflow-hidden shadow-inner">
                  <div class="bg-gradient-to-r from-amber-400 via-orange-500 to-red-600 h-full rounded-full transition-all duration-500" style="width: ${percentSold}%"></div>
                </div>
              </div>

              <!-- Đặc quyền 20/11 -->
              <div class="mt-1.5 text-[10px] font-bold text-red-700 bg-red-50/80 border border-red-200/60 rounded-lg px-1.5 py-0.5 text-center truncate">
                🎓 ${item.badge}
              </div>

              <!-- Nút Săn Ngay -->
              <button type="button" class="mt-2 w-full py-1.5 sm:py-2 rounded-xl bg-gradient-to-r from-red-600 to-amber-500 hover:from-red-500 hover:to-amber-400 text-white font-black text-xs shadow-md shadow-red-600/30 transition-all flex items-center justify-center gap-1.5 active:scale-95 cursor-pointer">
                <i class="fa-solid fa-bolt text-[11px] text-amber-200"></i>
                <span>SĂN NGAY</span>
              </button>
            </div>

          </div>
        </div>
      `;
    }).join('');

    applySale2011SliderPosition();
  }

  function applySale2011SliderPosition() {
    const track = document.getElementById('sale2011-track');
    if (!track) return;
    const cardsPerView = getCardsPerView();
    const list = SALE_2011_DATA[currentCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    if (currentIndex > maxIndex) currentIndex = maxIndex;
    if (currentIndex < 0) currentIndex = 0;

    const colWidthPercent = 100 / cardsPerView;
    track.style.transform = `translateX(-${currentIndex * colWidthPercent}%)`;
  }

  window.prevSale2011 = function () {
    const cardsPerView = getCardsPerView();
    const list = SALE_2011_DATA[currentCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    currentIndex--;
    if (currentIndex < 0) currentIndex = maxIndex;
    applySale2011SliderPosition();
  };

  window.nextSale2011 = function () {
    const cardsPerView = getCardsPerView();
    const list = SALE_2011_DATA[currentCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    currentIndex++;
    if (currentIndex > maxIndex) currentIndex = 0;
    applySale2011SliderPosition();
  };

  window.switchSale2011Category = function (cat) {
    if (!SALE_2011_DATA[cat]) return;
    currentCategory = cat;
    currentIndex = 0;

    const buttons = document.querySelectorAll('.sale2011-tab-btn');
    buttons.forEach(btn => {
      const isCurrent = btn.dataset.saleCat === cat;
      const indicator = btn.querySelector('.sale2011-tab-indicator');
      if (isCurrent) {
        btn.className = 'sale2011-tab-btn active relative group flex flex-col items-center justify-center p-1 sm:p-2.5 rounded-2xl cursor-pointer select-none transition-all duration-300 bg-gradient-to-b from-amber-400/25 to-yellow-500/10 border-2 border-amber-300 shadow-[0_4px_20px_rgba(251,191,36,0.35)] scale-102';
        if (indicator) indicator.classList.remove('opacity-0');
      } else {
        btn.className = 'sale2011-tab-btn relative group flex flex-col items-center justify-center p-1 sm:p-2.5 rounded-2xl cursor-pointer select-none transition-all duration-300 bg-black/25 hover:bg-black/40 border border-white/10 opacity-75 hover:opacity-100 hover:scale-102';
        if (indicator) indicator.classList.add('opacity-0');
      }
    });

    renderSale2011Products();
  };

  window.onSale2011CardClick = function (id) {
    window.location.href = 'pages/product-detail.html?id=' + encodeURIComponent(id);
  };

  // Đồng hồ đếm ngược ngày 20/11
  function initSale2011Countdown() {
    const hoursEl = document.getElementById('sale2011-hours');
    const minutesEl = document.getElementById('sale2011-minutes');
    const secondsEl = document.getElementById('sale2011-seconds');
    if (!hoursEl || !minutesEl || !secondsEl) return;

    // Ngày kết thúc chiến dịch: 22/11/2026 23:59:59
    const target = new Date('2026-11-22T23:59:59').getTime();

    function update() {
      const now = Date.now();
      let diff = Math.max(0, Math.floor((target - now) / 1000));

      if (diff <= 0) {
        hoursEl.textContent = '00';
        minutesEl.textContent = '00';
        secondsEl.textContent = '00';
        return;
      }

      const hours = Math.floor(diff / 3600);
      const minutes = Math.floor((diff % 3600) / 60);
      const seconds = diff % 60;

      hoursEl.textContent = String(hours).padStart(2, '0');
      minutesEl.textContent = String(minutes).padStart(2, '0');
      secondsEl.textContent = String(seconds).padStart(2, '0');
    }

    update();
    setInterval(update, 1000);
  }

  function initSale2011Section() {
    renderSale2011Products();
    initSale2011Countdown();

    window.addEventListener('resize', () => {
      applySale2011SliderPosition();
    });

    // Auto slide
    const container = document.getElementById('section-sieu-sale-20-11');
    if (container) {
      container.addEventListener('mouseenter', () => {
        if (autoSlideTimer) clearInterval(autoSlideTimer);
      });
      container.addEventListener('mouseleave', () => {
        startAutoSlide();
      });
    }
    startAutoSlide();
  }

  function startAutoSlide() {
    if (autoSlideTimer) clearInterval(autoSlideTimer);
    autoSlideTimer = setInterval(() => {
      window.nextSale2011();
    }, 4500);
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initSale2011Section);
  } else {
    initSale2011Section();
  }
})();
