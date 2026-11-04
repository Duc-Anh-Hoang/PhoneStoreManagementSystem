/**
 * CellphoneS - 241 Lê Văn Việt - MAIN CUSTOMER INTERFACE CONTROLLER
 * Điều khiển giao diện khách hàng tham khảo phong cách CellphoneS:
 * - Mega menu 3 tầng (NhomHang -> LoaiHang -> MatHang)
 * - Hero Banner Slider & Quick Tags
 * - 6 Dải sản phẩm: Điện thoại nổi bật, Hàng mới, Bán chạy, Phụ kiện, Đã xem
 * - Thanh tìm kiếm thông minh với Autocomplete và Bộ lọc nâng cao
 * - Đồng hồ đếm ngược khuyến mãi theo vw_KhuyenMaiTrangThai
 * - Tương thích song ngữ và đồng bộ phiên đăng nhập
 */

(function () {
  'use strict';

  console.log('>>> [CUSTOMER STORE] IIFE is running! readyState:', document.readyState);

  let heroCurrentSlide = 0;
  let heroSlideInterval = null;

  async function initApp() {
    console.log('>>> [CUSTOMER STORE] initApp started');
    // 1. Khởi tạo dữ liệu
    if ((window.CellphoneSData || window.StoreData)) {
      await (window.CellphoneSData || window.StoreData).init();
      console.log('>>> [CUSTOMER STORE] StoreData inited, products:', (window.CellphoneSData || window.StoreData).products.length);
    }

    // 2. Khởi tạo UI Giỏ hàng
    if ((window.CellphoneSCart || window.Cart)) {
      (window.CellphoneSCart || window.Cart).updateUI();
    }

    // 3. Render các thành phần
    renderMegaMenu();
    renderHeroSlider();
    renderShelves();
    initTramTiepSucSection();
    console.log('>>> [CUSTOMER STORE] renderShelves completed');
    initSearchAutocomplete();
    initScrollToTop();
    updateAuthHeader();

    // Lắng nghe đăng nhập thay đổi
    window.addEventListener('storage', () => {
      updateAuthHeader();
      if ((window.CellphoneSCart || window.Cart)) (window.CellphoneSCart || window.Cart).updateUI();
    });
  }

  /**
   * Cập nhật Header theo trạng thái Đăng nhập khách hàng / Quản trị
   */
  function updateAuthHeader() {
    const user = (window.CellphoneSAuth || window.Auth) ? (window.CellphoneSAuth || window.Auth).getUser() : null;
    const btnLogin = document.getElementById('btn-header-login');
    const menuStaff = document.getElementById('header-staff-menu');
    const menuCustomer = document.getElementById('header-customer-menu');
    const custName = document.getElementById('customer-header-name');
    const custPhone = document.getElementById('customer-header-phone');
    const custPoints = document.getElementById('customer-header-points');

    if (!user) {
      if (btnLogin) {
        btnLogin.classList.remove('hidden');
        btnLogin.style.display = 'inline-flex';
      }
      if (menuStaff) {
        menuStaff.classList.add('hidden');
        menuStaff.style.display = 'none';
      }
      if (menuCustomer) {
        menuCustomer.classList.add('hidden');
        menuCustomer.style.display = 'none';
      }
      return;
    }

    if (user.type === 'staff') {
      if (btnLogin) {
        btnLogin.classList.add('hidden');
        btnLogin.style.display = 'none';
      }
      if (menuStaff) {
        menuStaff.classList.remove('hidden');
        menuStaff.style.display = 'inline-flex';
        menuStaff.style.alignItems = 'center';
        menuStaff.style.flexWrap = 'nowrap';
        const roleEl = document.getElementById('staff-header-role');
        if (roleEl) roleEl.remove();
      }
      if (menuCustomer) {
        menuCustomer.classList.add('hidden');
        menuCustomer.style.display = 'none';
      }
    } else {
      // Khách hàng
      if (btnLogin) {
        btnLogin.classList.add('hidden');
        btnLogin.style.display = 'none';
      }
      if (menuStaff) {
        menuStaff.classList.add('hidden');
        menuStaff.style.display = 'none';
      }
      if (menuCustomer) {
        menuCustomer.classList.remove('hidden');
        menuCustomer.style.display = 'inline-flex';
        menuCustomer.style.alignItems = 'center';
        menuCustomer.style.flexWrap = 'nowrap';
      }
      if (custName) custName.textContent = user.hoTen || 'Thành viên';
      if (custPhone) custPhone.textContent = user.sdt || '';
      if (custPoints) custPoints.textContent = (user.diemTichLuy || 150) + ' Đ';
    }
  }

  /**
   * Hero Slider - 8 Banner Marketing Nét Chuẩn Hoàng Hà Mobile từ images/banner/
   */
  const DEFAULT_HERO_SLIDES = [
    {
      id: "slide-1",
      title: "Săn vé Galaxy Festival Concert",
      subtitle: "Cùng SAMAMOBILE",
      image: "images/banner/960301_samsung-festival-web.jpg",
      targetMaHang: "MH003",
      brand: "Samsung"
    },
    {
      id: "slide-2",
      title: "Galaxy S26 Series.",
      subtitle: "Mua Ngay",
      image: "images/banner/471331_S26-Series-1200x375.jpg",
      targetMaHang: "MH003",
      brand: "Samsung"
    },
    {
      id: "slide-3",
      title: "REDMAGIC Astra 2 Gaming",
      subtitle: "Đặt mua ngay",
      image: "images/banner/astra-2-web-083724546.jpg",
      targetMaHang: "MH002",
      brand: "Tablet"
    },
    {
      id: "slide-4",
      title: "Huawei Watch GT 7 Series",
      subtitle: "Chase the Wild",
      image: "images/banner/666649_KV-HUAWEI-WATCH-GT-7-Series-1200x375.jpg",
      targetMaHang: "MH008",
      brand: "Phụ kiện"
    },
    {
      id: "slide-5",
      title: "iPhone Duo",
      subtitle: "Đặt mua ngay hôm nay",
      image: "images/banner/iphone-duo-web-084156526.jpg",
      targetMaHang: "MH001",
      brand: "Apple"
    },
    {
      id: "slide-6",
      title: "OPPO Find X10 Series",
      subtitle: "Đặt Trước Rước Ngay",
      image: "images/banner/860851_OPPO-Find-X10-Series-web.jpg",
      targetMaHang: "MH006",
      brand: "OPPO"
    },
    {
      id: "slide-7",
      title: "iPhone 18 Pro Max",
      subtitle: "Đặt mua ngay",
      image: "images/banner/iphone-18-web-082925157.jpg",
      targetMaHang: "MH001",
      brand: "Apple"
    },
    {
      id: "slide-8",
      title: "Galaxy A18 | A08",
      subtitle: "Bền mượt cực đã",
      image: "images/banner/702904_KV-SS-A18-_-A08-1200x375-25w.jpg",
      targetMaHang: "MH004",
      brand: "Samsung"
    }
  ];

  // Quản lý Hero Slider & Dải Tab trượt liên tục từng cột (1|2|3|4 -> 2|3|4|5)
  let heroCurrentIndex = 8; // Bắt đầu ở Set 1 (index 8..15 cho 8 slides)
  let heroTotalSlides = 8;

  function getTabColWidth() {
    const viewport = document.getElementById('hero-tabs-viewport') || document.getElementById('hero-tabs-wrapper');
    if (!viewport) return 0;
    const isDesktop = window.innerWidth >= 768;
    const itemsVisible = isDesktop ? 4 : 2;
    return viewport.clientWidth / itemsVisible;
  }

  function applyTabsPosition(animate = true) {
    const tabsTrack = document.getElementById('hero-banner-tabs');
    if (!tabsTrack) return;
    const colWidth = getTabColWidth();
    const offset = heroCurrentIndex * colWidth;

    if (animate) {
      tabsTrack.style.transition = 'transform 0.45s cubic-bezier(0.25, 1, 0.5, 1)';
    } else {
      tabsTrack.style.transition = 'none';
    }
    tabsTrack.style.transform = `translateX(-${offset}px)`;
  }

  function updateActiveSlideUI() {
    const activeSlide = ((heroCurrentIndex % heroTotalSlides) + heroTotalSlides) % heroTotalSlides;
    const slides = document.querySelectorAll('.hero-fade-slide');
    const dots = document.querySelectorAll('#hero-slider-dots button');
    const tabs = document.querySelectorAll('.hero-tab-item');

    // 1. Chuyển cảnh mờ dần ảnh banner
    slides.forEach((s, i) => {
      if (i === activeSlide) s.classList.add('active-slide');
      else s.classList.remove('active-slide');
    });

    // 2. Cập nhật dots
    dots.forEach((d, i) => {
      if (i === activeSlide) {
        d.className = 'w-6 h-1.5 rounded-full bg-[#2563EB] transition-all cursor-pointer shadow-sm';
      } else {
        d.className = 'w-2 h-1.5 rounded-full bg-white/70 hover:bg-white transition-all cursor-pointer';
      }
    });

    // 3. Cập nhật active tab: Cột đầu tiên trong khung hiển thị (tại heroCurrentIndex) là active (chỉ đổi màu chữ và gạch chân dưới, không đổi màu nền)
    tabs.forEach((tab) => {
      const domIdx = parseInt(tab.getAttribute('data-dom-index'), 10);
      const title = tab.querySelector('.tab-title');
      const subtitle = tab.querySelector('.tab-subtitle');
      if (domIdx === heroCurrentIndex) {
        tab.classList.add('active-tab');
        if (title) {
          title.className = 'tab-title text-[14px] sm:text-[16px] lg:text-[18px] font-extrabold leading-tight line-clamp-2 w-full text-center min-h-[40px] flex items-center justify-center text-[#2563EB]';
        }
        if (subtitle) {
          subtitle.className = 'tab-subtitle text-xs sm:text-[13px] font-semibold leading-tight truncate w-full text-center mt-1.5 text-[#2563EB]';
        }
      } else {
        tab.classList.remove('active-tab');
        if (title) {
          title.className = 'tab-title text-[14px] sm:text-[16px] lg:text-[18px] font-bold leading-tight line-clamp-2 w-full text-center min-h-[40px] flex items-center justify-center text-slate-800';
        }
        if (subtitle) {
          subtitle.className = 'tab-subtitle text-xs sm:text-[13px] leading-tight truncate w-full text-center mt-1.5 text-slate-500';
        }
      }
    });
  }

  function startHeroSliderAuto() {
    if (heroSlideInterval) clearInterval(heroSlideInterval);
    heroSlideInterval = setInterval(() => {
      heroCurrentIndex++;
      applyTabsPosition(true);
      updateActiveSlideUI();
    }, 3000); // Tự động chuyển slide đều đặn đúng mỗi 3 giây
  }

  // Chuyển tới slide kế tiếp (dịch 1 cột: 1|2|3|4 -> ẩn 1 hiện 2|3|4|5)
  window.nextHeroSlide = function () {
    heroCurrentIndex++;
    applyTabsPosition(true);
    updateActiveSlideUI();
    startHeroSliderAuto();
  };

  // Quay lại slide trước (dịch 1 cột ngược lại)
  window.prevHeroSlide = function () {
    heroCurrentIndex--;
    applyTabsPosition(true);
    updateActiveSlideUI();
    startHeroSliderAuto();
  };

  // Alias tương thích
  window.prevTabGroup = window.prevHeroSlide;
  window.nextTabGroup = window.nextHeroSlide;

  // Chuyển trực tiếp tới slide theo index (0..7) từ dot
  window.setHeroSlide = function (targetSlideIdx) {
    const currentSlide = ((heroCurrentIndex % heroTotalSlides) + heroTotalSlides) % heroTotalSlides;
    let diff = targetSlideIdx - currentSlide;
    if (diff === 0) return;
    heroCurrentIndex += diff;
    applyTabsPosition(true);
    updateActiveSlideUI();
    startHeroSliderAuto();
  };

  // Click vào một tab cụ thể trên dải tab: dịch đưa tab đó lên vị trí cột đầu tiên (active)
  window.onTabItemClick = function (domIdx) {
    if (domIdx === heroCurrentIndex) return;
    heroCurrentIndex = domIdx;
    applyTabsPosition(true);
    updateActiveSlideUI();
    startHeroSliderAuto();
  };

  window.addEventListener('resize', () => {
    applyTabsPosition(false);
  });

  window.addEventListener('load', () => {
    applyTabsPosition(false);
  });

  document.addEventListener('visibilitychange', () => {
    if (!document.hidden) {
      const total = heroTotalSlides;
      heroCurrentIndex = (((heroCurrentIndex % total) + total) % total) + total;
      applyTabsPosition(false);
      updateActiveSlideUI();
      startHeroSliderAuto();
    }
  });

  function renderHeroSlider() {
    const slidesContainer = document.getElementById('hero-slider-container');
    const dotsContainer = document.getElementById('hero-slider-dots');
    const tabsContainer = document.getElementById('hero-banner-tabs');
    if (!slidesContainer) return;

    let slides = ((window.CellphoneSData || window.StoreData)?.config?.banners?.heroSlides);
    if (!slides || !slides.length) {
      slides = DEFAULT_HERO_SLIDES;
    }
    heroTotalSlides = slides.length; // 8

    // 1. Render các slide hiệu ứng mờ dần (Cross-fade)
    slidesContainer.innerHTML = slides.map((s, idx) => `
      <div class="hero-fade-slide cursor-pointer select-none group ${idx === 0 ? 'active-slide' : ''}" data-slide-index="${idx}" onclick="window.onBannerClick('${s.targetMaHang || 'MH001'}', '${s.brand || 'all'}')">
        <img src="${s.image}?v=4" alt="${s.title}" class="w-full h-full object-cover select-none transition-transform duration-700 group-hover:scale-[1.01]" loading="${idx === 0 ? 'eager' : 'lazy'}">
      </div>
    `).join('');

    // 2. Render dots chỉ số trên ảnh (Active là thanh capsule xanh dương #2563EB)
    if (dotsContainer) {
      dotsContainer.innerHTML = slides.map((_, idx) => `
        <button type="button" onclick="window.setHeroSlide(${idx})" class="transition-all cursor-pointer ${idx === 0 ? 'w-6 h-1.5 rounded-full bg-[#2563EB]' : 'w-2 h-1.5 rounded-full bg-white/70 hover:bg-white'}" aria-label="Slide ${idx + 1}"></button>
      `).join('');
    }

    // 3. Render 3 bộ tabs liên tục (Set 0, Set 1, Set 2) để trượt vô tận từng cột (1|2|3|4 -> 2|3|4|5)
    if (tabsContainer) {
      const tripleSlides = [...slides, ...slides, ...slides];
      tabsContainer.innerHTML = tripleSlides.map((s, domIdx) => {
        const slideIdx = domIdx % slides.length;
        const isActive = domIdx === 8;
        return `
          <button type="button" onclick="window.onTabItemClick(${domIdx})" data-dom-index="${domIdx}" data-slide-index="${slideIdx}" class="hero-tab-item w-1/2 md:w-1/4 flex-shrink-0 py-3.5 sm:py-4 px-2.5 sm:px-3 flex flex-col items-center justify-center cursor-pointer transition-colors ${isActive ? 'active-tab' : ''}">
            <span class="tab-title text-[14px] sm:text-[16px] lg:text-[18px] font-bold leading-tight line-clamp-2 w-full text-center min-h-[40px] flex items-center justify-center ${isActive ? 'text-[#2563EB] font-extrabold' : 'text-slate-800'}">${s.title}</span>
            <span class="tab-subtitle text-xs sm:text-[13px] leading-tight truncate w-full text-center mt-1.5 ${isActive ? 'text-[#2563EB] font-semibold' : 'text-slate-500'}">${s.subtitle || ''}</span>
          </button>
        `;
      }).join('');

      // Lắng nghe transitionend để chuẩn hóa vòng lặp vô tận (Set 1 <-> Set 2 <-> Set 0)
      if (!tabsContainer._hasTransitionListener) {
        tabsContainer._hasTransitionListener = true;
        tabsContainer.addEventListener('transitionend', (e) => {
          if (e && (e.target !== tabsContainer || e.propertyName !== 'transform')) return;
          const total = heroTotalSlides;
          if (heroCurrentIndex >= 2 * total || heroCurrentIndex < total) {
            heroCurrentIndex = (((heroCurrentIndex % total) + total) % total) + total;
            applyTabsPosition(false);
          }
        });
      }
    }

    // Khởi tạo vị trí ban đầu (Set 1: slide 0)
    heroCurrentIndex = heroTotalSlides; // 8
    requestAnimationFrame(() => {
      applyTabsPosition(false);
      updateActiveSlideUI();
    });
    startHeroSliderAuto();
  }

  window.onBannerClick = function(targetMaHang, brand) {
    if (targetMaHang && (window.CellphoneSProductModal || window.ProductModal)) {
      (window.CellphoneSProductModal || window.ProductModal).open(targetMaHang);
    } else if (brand && window.filterPhoneBrand) {
      window.filterPhoneBrand(brand, true);
    }
  };

  window.showStudentPromo = function() {
    const shelf = document.getElementById('promotions-shelf') || document.getElementById('products-grid');
    if (shelf) {
      shelf.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  };

  // Quản lý thông báo Realtime (sẵn sàng kết nối WebSocket / SSE / API)
  window.NotificationCenter = {
    toggle: function() {
      const pop = document.getElementById('notification-popover');
      if (pop) pop.classList.toggle('hidden');
    },
    setBadge: function(count) {
      const badge = document.getElementById('notification-badge');
      const countBadge = document.getElementById('notification-count-badge');
      if (count && count > 0) {
        if (badge) badge.classList.remove('hidden');
        if (countBadge) {
          countBadge.textContent = `${count} tin mới`;
          countBadge.classList.remove('hidden');
        }
      } else {
        if (badge) badge.classList.add('hidden');
        if (countBadge) countBadge.classList.add('hidden');
      }
    },
    render: function(notifications) {
      const listEl = document.getElementById('notification-list');
      if (!listEl) return;
      if (!Array.isArray(notifications) || notifications.length === 0) {
        listEl.innerHTML = `
          <div id="notification-empty" class="py-8 px-4 text-center text-slate-400 space-y-2">
            <i class="fa-regular fa-bell-slash text-2xl text-slate-300"></i>
            <p class="text-xs">Hiện chưa có thông báo mới</p>
          </div>
        `;
        this.setBadge(0);
        return;
      }
      this.setBadge(notifications.length);
      listEl.innerHTML = notifications.map(item => `
        <div class="p-3.5 hover:bg-slate-50 transition-colors cursor-pointer flex gap-3 items-start" onclick="${item.onClick || ''}">
          <div class="w-8 h-8 rounded-lg ${item.bgClass || 'bg-blue-100'} ${item.colorClass || 'text-[#2563EB]'} flex items-center justify-center flex-shrink-0 text-xs">
            <i class="${item.icon || 'fa-solid fa-bell'}"></i>
          </div>
          <div class="space-y-0.5 min-w-0">
            <p class="font-bold text-slate-900 text-xs truncate">${item.title || ''}</p>
            <p class="text-[11px] text-slate-500 line-clamp-2">${item.message || ''}</p>
            <span class="text-[10px] text-slate-400">${item.time || 'Vừa xong'}</span>
          </div>
        </div>
      `).join('');
    }
  };

  window.toggleNotificationPopover = function() {
    window.NotificationCenter.toggle();
  };

  document.addEventListener('click', (e) => {
    const pop = document.getElementById('notification-popover');
    const btn = document.getElementById('btn-notification-toggle');
    if (pop && !pop.classList.contains('hidden')) {
      if (btn && !btn.contains(e.target) && !pop.contains(e.target)) {
        pop.classList.add('hidden');
      }
    }
  });



  /**
   * Render các dải sản phẩm (Shelves)
   */
  function renderShelves() {
    renderPromotionsShelf();
    renderFeaturedPhonesShelf('all');
    renderNewArrivalsShelf();
    renderBestSellersShelf();
    renderAccessoriesShelf();
    renderRecentlyViewedShelf();
  }

  function renderPromotionsShelf() {
    const container = document.getElementById('promotions-list');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;

    const promos = (window.CellphoneSData || window.StoreData).promotions || [];
    const section = document.getElementById('shelf-promotions');
    if (promos.length === 0) {
      if (section) section.classList.add('hidden');
      return;
    }
    if (section) section.classList.remove('hidden');

    container.innerHTML = promos.map(km => {
      const phanTram = Number(km.phanTramGiam) || 0;
      const ngayKT = km.ngayKT ? String(km.ngayKT).substring(0, 10) : '';
      return `
        <div class="bg-white rounded-2xl border border-rose-200 p-4 shadow-sm flex items-center justify-between gap-3">
          <div class="space-y-1">
            <div class="flex items-center gap-2">
              <span class="px-2 py-0.5 rounded-md bg-[#d70018] text-white font-mono font-black text-xs">${km.maKM}</span>
              <span class="text-xs font-black text-[#d70018]">GIẢM ${phanTram}%</span>
            </div>
            <h4 class="font-bold text-xs text-navy line-clamp-1">${km.tenKM}</h4>
            <p class="text-[10px] text-slate-400">Hiệu lực đến: ${ngayKT || 'Đang diễn ra'}</p>
          </div>
          <button type="button" onclick="window.applyPromoCodeFromShelf('${km.maKM}')" class="px-3 py-1.5 rounded-xl bg-rose-50 hover:bg-rose-100 text-[#d70018] font-bold text-xs whitespace-nowrap transition-colors border border-rose-200 cursor-pointer">
            Lưu mã
          </button>
        </div>
      `;
    }).join('');
  }

  window.applyPromoCodeFromShelf = function (code) {
    if (window.CellphoneSCart || window.Cart) {
      const res = (window.CellphoneSCart || window.Cart).applyPromoCode(code);
      if (res && res.success) {
        (window.CellphoneSCart || window.Cart).openDrawer();
      }
    }
  };

  function renderFeaturedPhonesShelf(brand = 'all', shouldScroll = false) {
    const container = document.getElementById('shelf-phones-items');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;

    const items = (window.CellphoneSData || window.StoreData).getFeaturedPhones(brand);
    container.innerHTML = items.map(p => createProductCardHtml(p)).join('');

    // Cập nhật tab active trên dải sản phẩm
    const tabs = document.querySelectorAll('.tab-phone-brand');
    tabs.forEach(tab => {
      if (tab.dataset.brand === brand) {
        tab.className = 'tab-phone-brand px-3.5 py-1.5 rounded-full text-xs font-black bg-[#80ffbf] text-[#061a0f] shadow-sm transition-all cursor-pointer';
      } else {
        tab.className = 'tab-phone-brand px-3.5 py-1.5 rounded-full text-xs font-semibold bg-slate-100 hover:bg-[#eafff5] hover:text-[#064e29] text-slate-700 transition-all cursor-pointer';
      }
    });

    // Cập nhật nút active trên dải chip danh mục ngang (Horizontal Chip Bar)
    const chipBtns = document.querySelectorAll('.chip-cat-btn');
    chipBtns.forEach(btn => {
      if (btn.dataset.brand === brand) {
        btn.classList.add('bg-[#1E3A8A]', 'text-white');
        btn.classList.remove('bg-white', 'text-slate-800', 'hover:bg-blue-50', 'border-slate-200');
      } else {
        btn.classList.remove('bg-[#1E3A8A]', 'text-white');
        btn.classList.add('bg-white', 'text-slate-800', 'hover:bg-blue-50', 'border-slate-200');
      }
    });

    // Cập nhật tiêu đề dải và nút xem thêm bộ lọc
    const titleTextEl = document.getElementById('shelf-phones-title-text');
    const viewAllBtn = document.getElementById('shelf-phones-view-all');
    if (titleTextEl) {
      if (brand === 'Apple') {
        titleTextEl.textContent = 'ĐIỆN THOẠI APPLE IPHONE CHÍNH HÃNG';
      } else if (brand === 'Samsung') {
        titleTextEl.textContent = 'ĐIỆN THOẠI SAMSUNG GALAXY AI';
      } else if (brand === 'Xiaomi') {
        titleTextEl.textContent = 'ĐIỆN THOẠI XIAOMI LEICA SIÊU PHẨM';
      } else {
        titleTextEl.textContent = 'ĐIỆN THOẠI NỔI BẬT';
      }
    }
    if (viewAllBtn) {
      if (brand !== 'all') {
        const brandCount = (window.CellphoneSData || window.StoreData).getProductsByBrand ? (window.CellphoneSData || window.StoreData).getProductsByBrand(brand).length : items.length;
        viewAllBtn.innerHTML = `Xem tất cả ${brand} (${brandCount}) <i class="fa-solid fa-arrow-right text-[10px] ml-1"></i>`;
        viewAllBtn.classList.remove('hidden');
        viewAllBtn.onclick = () => window.openSearchModal('', brand);
      } else {
        viewAllBtn.classList.add('hidden');
      }
    }

    // Cuộn mượt mà xuống Dải Điện Thoại Nổi Bật khi bấm từ menu
    if (shouldScroll) {
      const shelf = document.getElementById('shelf-phones');
      if (shelf) {
        const headerOffset = 70;
        const elemPos = shelf.getBoundingClientRect().top + window.pageYOffset - headerOffset;
        window.scrollTo({ top: elemPos, behavior: 'smooth' });

        // Hiệu ứng highlight trực quan khi chuyển dải
        shelf.classList.add('ring-4', 'ring-[#80ffbf]/50', 'transition-all', 'duration-500');
        setTimeout(() => {
          shelf.classList.remove('ring-4', 'ring-[#80ffbf]/50');
        }, 1200);
      }
    }
  }
  window.filterPhoneBrand = renderFeaturedPhonesShelf;

  function renderNewArrivalsShelf() {
    const container = document.getElementById('shelf-new-items');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;
    const items = (window.CellphoneSData || window.StoreData).getNewArrivals();
    container.innerHTML = items.map(p => createProductCardHtml(p)).join('');
  }

  function renderBestSellersShelf() {
    const container = document.getElementById('shelf-bestseller-items');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;
    const items = (window.CellphoneSData || window.StoreData).getBestSellers();
    container.innerHTML = items.map(p => createProductCardHtml(p)).join('');
  }

  function renderAccessoriesShelf() {
    const container = document.getElementById('shelf-accessories-items');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;
    const items = (window.CellphoneSData || window.StoreData).getAccessories();
    container.innerHTML = items.map(p => createProductCardHtml(p)).join('');
  }

  function renderRecentlyViewedShelf() {
    const container = document.getElementById('shelf-recent-items');
    const section = document.getElementById('section-recently-viewed');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;

    const items = (window.CellphoneSData || window.StoreData).getRecentlyViewed();
    if (items.length === 0) {
      if (section) section.classList.add('hidden');
    } else {
      if (section) section.classList.remove('hidden');
      container.innerHTML = items.map(p => createProductCardHtml(p)).join('');
    }
  }

  /**
   * Tạo HTML thẻ sản phẩm chuẩn CellphoneS
   */
  function createProductCardHtml(p, options = {}) {
    const isFlashSale = options.isFlashSale || p.isFlashSale;
    const discountPercent = p.giaGoc > p.donGia 
      ? Math.round((1 - p.donGia / p.giaGoc) * 100) 
      : 0;

    const tonKho = p.tonKho !== undefined ? Number(p.tonKho) : 0;
    const stockHtml = tonKho > 0
      ? `<span class="text-emerald-600 font-semibold"><i class="fa-solid fa-circle-check text-[10px] mr-1"></i>Còn ${tonKho} máy</span>`
      : `<span class="text-rose-500 font-semibold"><i class="fa-solid fa-circle-xmark text-[10px] mr-1"></i>Tạm hết hàng</span>`;

    const bhHtml = (p.thoiHanBaoHanh && p.thoiHanBaoHanh > 0)
      ? `<span class="text-slate-500 text-[11px]"><i class="fa-solid fa-shield text-[10px] mr-0.5 text-blue-600"></i>Bảo hành ${p.thoiHanBaoHanh} tháng</span>`
      : '';

    return `
      <div class="product-card group relative flex flex-col justify-between bg-white rounded-[16px] border border-slate-200/90 p-4 sm:p-5 hover:border-blue-300 hover:shadow-[0_12px_28px_rgba(0,0,0,0.12)] hover:-translate-y-1 transition-all duration-250">
        <!-- Badges góc trên -->
        <div class="flex items-center justify-between gap-1 mb-2 z-10">
          <div class="flex flex-wrap gap-1">
            ${p.tenLoai ? `<span class="px-2 py-0.5 rounded-[6px] bg-slate-100 text-slate-700 text-[10px] font-bold uppercase">${p.tenLoai}</span>` : ''}
          </div>
          ${discountPercent > 0 ? `
            <span class="px-2 py-0.5 rounded-[6px] bg-[#FF7A00] text-white text-[10px] font-extrabold uppercase tracking-tight shadow-sm">
              Giảm ${discountPercent}%
            </span>` : ''}
        </div>

        <!-- Ảnh sản phẩm -->
        <div class="relative w-full h-40 sm:h-48 my-2 flex items-center justify-center p-2 cursor-pointer overflow-hidden" onclick="(window.CellphoneSProductModal || window.ProductModal).open('${p.maHang}')">
          ${p.anhChinh ? `
            <img src="${p.anhChinh}" alt="${p.tenHang}" class="max-w-full max-h-full object-contain group-hover:scale-105 transition-transform duration-300">
          ` : `
            <div class="w-full h-full flex flex-col items-center justify-center text-slate-300 group-hover:text-[#2563EB] transition-colors bg-slate-50/70 rounded-xl border border-dashed border-slate-200">
              <i class="fa-solid fa-mobile-screen-button text-3xl mb-1 text-slate-300 group-hover:text-[#2563EB]"></i>
              <span class="text-[10px] text-slate-400 font-medium">Chờ cập nhật ảnh</span>
            </div>
          `}
        </div>

        <!-- Tên sản phẩm -->
        <div class="mt-2 flex-1">
          <h3 class="text-xs sm:text-sm font-bold text-[#111827] hover:text-[#1E3A8A] line-clamp-2 cursor-pointer transition-colors" onclick="(window.CellphoneSProductModal || window.ProductModal).open('${p.maHang}')" title="${p.tenHang}">
            ${p.tenHang}
          </h3>

          <!-- Giá bán -->
          <div class="flex flex-wrap items-baseline gap-2 mt-2">
            <span class="text-sm sm:text-base font-black text-[#FF7A00] font-mono">${(window.CellphoneSCart || window.Cart) ? (window.CellphoneSCart || window.Cart).formatMoney(p.donGia) : p.donGia}</span>
            ${p.giaGoc > p.donGia ? `<span class="text-[11px] text-slate-400 line-through">${(window.CellphoneSCart || window.Cart).formatMoney(p.giaGoc)}</span>` : ''}
          </div>

          <!-- Tình trạng tồn kho & bảo hành -->
          <div class="mt-2 text-[11px] flex items-center justify-between gap-1">
            ${stockHtml}
            ${bhHtml}
          </div>
        </div>

        <!-- Nút hành động chuẩn: "Xem cấu hình" và "Mua ngay" -->
        <div class="mt-3.5 pt-3 border-t border-slate-100 flex items-center gap-2">
          <button type="button" onclick="(window.CellphoneSProductModal || window.ProductModal).open('${p.maHang}')" class="flex-1 h-[40px] px-2 rounded-[12px] border border-[#2563EB] text-[#2563EB] hover:bg-blue-50 font-bold text-xs transition-colors flex items-center justify-center cursor-pointer">
            Xem cấu hình
          </button>
          <button type="button" ${tonKho <= 0 ? 'disabled' : ''} onclick="window.quickBuy('${p.maHang}')" class="flex-1 h-[40px] px-2 rounded-[12px] ${tonKho > 0 ? 'bg-[#FF7A00] hover:bg-[#E06800] text-white cursor-pointer shadow-md shadow-orange-500/20' : 'bg-slate-200 text-slate-400 cursor-not-allowed'} font-extrabold text-xs transition-all flex items-center justify-center">
            ${tonKho > 0 ? 'Mua ngay' : 'Hết hàng'}
          </button>
        </div>
      </div>
    `;
  }

  window.quickBuy = function (maHang) {
    if (!(window.CellphoneSData || window.StoreData)) return;
    const p = (window.CellphoneSData || window.StoreData).getProductById(maHang);
    if (!p) return;
    if (p.tonKho <= 0) {
      if (window.CellphoneSCart || window.Cart) {
        (window.CellphoneSCart || window.Cart).showToast('Sản phẩm hiện đang tạm hết hàng!', 'warning');
      }
      return;
    }
    if (window.CellphoneSCart || window.Cart) {
      (window.CellphoneSCart || window.Cart).addItem(p, null, null, 1);
    }
    if (window.CellphoneSCheckout || window.Checkout) {
      (window.CellphoneSCheckout || window.Checkout).openCheckout();
    }
  };

  /**
   * Autocomplete Tìm Kiếm & Bộ lọc
   */
  function initSearchAutocomplete() {
    const input = document.getElementById('search-input');
    const dropdown = document.getElementById('search-autocomplete-dropdown');
    const clearBtn = document.getElementById('btn-clear-search');

    if (!input || !dropdown) return;

    input.addEventListener('input', (e) => {
      const q = e.target.value.trim();
      if (clearBtn) {
        if (q) clearBtn.classList.remove('hidden');
        else clearBtn.classList.add('hidden');
      }

      if (!q) {
        dropdown.classList.add('hidden');
        return;
      }

      const results = (window.CellphoneSData || window.StoreData) ? (window.CellphoneSData || window.StoreData).search(q).slice(0, 5) : [];
      if (results.length === 0) {
        dropdown.innerHTML = `
          <div class="p-4 text-center text-xs text-slate-500">
            Không tìm thấy sản phẩm khớp với "<strong>${q}</strong>".
            <div class="mt-2 text-[#16A34A] font-semibold cursor-pointer" onclick="window.quickSearchTag('iPhone')">Gợi ý: Tìm "iPhone", "Samsung"</div>
          </div>
        `;
      } else {
        dropdown.innerHTML = `
          <div class="p-2 border-b border-slate-100 text-[11px] font-bold text-slate-400 uppercase tracking-wider">
            Gợi ý sản phẩm (${results.length})
          </div>
          <div class="divide-y divide-slate-100">
            ${results.map(p => `
              <div class="p-2.5 flex items-center gap-3 hover:bg-slate-50 rounded-xl cursor-pointer transition-colors" onclick="(window.CellphoneSProductModal || window.ProductModal).open('${p.maHang}'); document.getElementById('search-autocomplete-dropdown').classList.add('hidden');">
                <div class="w-10 h-10 object-contain p-1 rounded-lg bg-slate-50 border border-slate-100 flex-shrink-0 flex items-center justify-center">
                  ${p.anhChinh ? `
                    <img src="${p.anhChinh}" class="w-full h-full object-contain" alt="${p.tenHang}">
                  ` : `
                    <i class="fa-solid fa-mobile-screen-button text-slate-300 text-xs"></i>
                  `}
                </div>
                <div class="flex-1 min-w-0">
                  <h4 class="text-xs font-bold text-navy truncate">${p.tenHang}</h4>
                  <div class="flex items-center gap-2 mt-0.5">
                    <span class="text-xs font-bold text-rose-600">${(window.CellphoneSCart || window.Cart).formatMoney(p.donGia)}</span>
                    ${p.giaGoc > p.donGia ? `<span class="text-[10px] text-slate-400 line-through">${(window.CellphoneSCart || window.Cart).formatMoney(p.giaGoc)}</span>` : ''}
                  </div>
                </div>
                <i class="fa-solid fa-chevron-right text-[10px] text-slate-300"></i>
              </div>
            `).join('')}
          </div>
          <div class="p-2 border-t border-slate-100 text-center">
            <button type="button" onclick="window.openSearchModal('${q}')" class="text-xs font-bold text-[#16A34A] hover:text-[#15803D]">
              Xem tất cả kết quả cho "${q}" &rarr;
            </button>
          </div>
        `;
      }
      dropdown.classList.remove('hidden');
    });

    input.addEventListener('keydown', (e) => {
      if (e.key === 'Enter') {
        const q = input.value.trim();
        dropdown.classList.add('hidden');
        window.openSearchModal(q);
      }
    });

    // Đóng dropdown khi click bên ngoài
    document.addEventListener('click', (e) => {
      if (!input.contains(e.target) && !dropdown.contains(e.target)) {
        dropdown.classList.add('hidden');
      }
    });

    if (clearBtn) {
      clearBtn.addEventListener('click', () => {
        input.value = '';
        clearBtn.classList.add('hidden');
        dropdown.classList.add('hidden');
        input.focus();
      });
    }
  }

  window.quickSearchTag = function (tag) {
    const input = document.getElementById('search-input');
    if (input) {
      input.value = tag;
      window.openSearchModal(tag);
    }
  };

  /**
   * Modal Kết quả tìm kiếm và bộ lọc (Phần A3)
   */
  window.openSearchModal = function (query = '', brand = '') {
    const modal = document.getElementById('search-results-modal');
    const input = document.getElementById('modal-search-input');
    const brandSelect = document.getElementById('filter-brand');
    if (input) input.value = query;
    if (brandSelect && brand) brandSelect.value = brand;

    if (modal) {
      modal.classList.remove('hidden');
      document.body.style.overflow = 'hidden';
      applySearchFilters();
    }
  };

  window.closeSearchModal = function () {
    const modal = document.getElementById('search-results-modal');
    if (modal) {
      modal.classList.add('hidden');
      document.body.style.overflow = '';
    }
  };

  window.applySearchFilters = function () {
    const query = (document.getElementById('modal-search-input')?.value || '').trim();
    const brand = document.getElementById('filter-brand')?.value || 'all';
    const priceRange = document.getElementById('filter-price')?.value || 'all';
    const inStock = document.getElementById('filter-instock')?.checked || false;
    const sortBy = document.getElementById('filter-sort')?.value || 'best_seller';

    if (!(window.CellphoneSData || window.StoreData)) return;

    const results = (window.CellphoneSData || window.StoreData).search(query, {
      brand: brand,
      priceRange: priceRange,
      inStockOnly: inStock,
      sortBy: sortBy
    });

    const container = document.getElementById('search-modal-results-grid');
    const countEl = document.getElementById('search-modal-count');

    if (countEl) countEl.textContent = results.length;

    if (!container) return;

    if (results.length === 0) {
      container.innerHTML = `
        <div class="col-span-full py-12 text-center text-slate-500">
          <i class="fa-solid fa-box-open text-4xl text-slate-300 mb-3 block"></i>
          <h4 class="font-bold text-sm text-navy mb-1">Không tìm thấy sản phẩm phù hợp</h4>
          <p class="text-xs text-slate-400">Hãy thử từ khóa khác hoặc bỏ bớt các bộ lọc đang chọn</p>
        </div>
      `;
    } else {
      container.innerHTML = results.map(p => createProductCardHtml(p)).join('');
    }
  };

  /**
   * Đếm ngược Khuyến Mãi (Phần A1)
   */
  function initCountdownTimers() {
    const countdownEl = document.getElementById('topbar-countdown');
    const flashCountdownEl = document.getElementById('flashsale-countdown');

    const targetDate = new Date('2026-10-31T23:59:59').getTime();

    function update() {
      const now = new Date().getTime();
      const diff = targetDate - now;

      if (diff <= 0) {
        if (countdownEl) countdownEl.textContent = '00:00:00';
        if (flashCountdownEl) flashCountdownEl.textContent = '00:00:00';
        return;
      }

      const hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
      const minutes = Math.floor((diff / (1000 * 60)) % 60);
      const seconds = Math.floor((diff / 1000) % 60);

      const str = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
      if (countdownEl) countdownEl.textContent = str;
      if (flashCountdownEl) flashCountdownEl.textContent = str;
    }

    update();
    setInterval(update, 1000);
  }

  /**
   * Back-to-Top Button
   */
  function initScrollToTop() {
    const btn = document.getElementById('btn-back-to-top');
    if (!btn) return;

    window.addEventListener('scroll', () => {
      if (window.scrollY > 400) {
        btn.classList.remove('hidden');
      } else {
        btn.classList.add('hidden');
      }
    });

    btn.addEventListener('click', () => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }

  /**
   * Render Mega Menu từ API NhomHang và LoaiHang
   */
  function renderMegaMenu() {
    const container = document.getElementById('mega-menu-flyout');
    if (!container || !(window.CellphoneSData || window.StoreData)) return;

    const store = (window.CellphoneSData || window.StoreData);
    const nhomList = store.nhomHang || [];
    const loaiList = store.loaiHang || [];

    let html = `
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 text-xs">
    `;

    nhomList.forEach(nh => {
      const loaiOfNhom = loaiList.filter(l => {
        if (l.maNhom) return l.maNhom === nh.maNhom;
        return store.products.some(p => p.maNhom === nh.maNhom && p.maLoai === l.maLoai);
      });
      let icon = 'fa-mobile-screen';
      let color = 'text-[#16A34A]';
      if (nh.maNhom === 'NH02' || nh.maNhom === 'PHUKIEN') { icon = 'fa-plug'; color = 'text-amber-600'; }
      else if (nh.maNhom === 'NH03' || nh.maNhom === 'AMTHANH') { icon = 'fa-headphones'; color = 'text-blue-600'; }
      else if (nh.maNhom === 'NH04' || nh.maNhom === 'TABLET') { icon = 'fa-tablet-screen-button'; color = 'text-indigo-600'; }
      else if (nh.maNhom === 'NH05' || nh.maNhom === 'LAPTOP') { icon = 'fa-laptop'; color = 'text-purple-600'; }

      html += `
        <div class="space-y-3">
          <div class="font-bold text-navy text-sm flex items-center gap-2 ${color} border-b border-slate-100 pb-2">
            <i class="fa-solid ${icon}"></i>
            <span>${nh.tenNhom}</span>
          </div>
          <ul class="space-y-2 text-slate-600">
            ${loaiOfNhom.map(l => `
              <li>
                <button type="button" onclick="window.filterByLoaiHang('${l.tenLoai}'); window.toggleMegaMenu();" class="hover:text-[#16A34A] font-semibold text-left">
                  ${l.tenLoai}
                </button>
              </li>
            `).join('')}
            <li>
              <button type="button" onclick="window.filterByNhomHang('${nh.maNhom}'); window.toggleMegaMenu();" class="hover:text-[#16A34A] text-[#16A34A] font-bold">
                Xem tất cả ${nh.tenNhom} &rarr;
              </button>
            </li>
          </ul>
        </div>
      `;
    });

    // Cột Tra cứu & Hỗ trợ
    html += `
      <div class="space-y-3">
        <div class="font-bold text-navy text-sm flex items-center gap-2 text-blue-600 border-b border-slate-100 pb-2">
          <i class="fa-solid fa-circle-question"></i>
          <span>Tra Cứu & Hỗ Trợ</span>
        </div>
        <ul class="space-y-2 text-slate-600">
          <li>
            <button type="button" onclick="window.CellphoneSLookup.openModal('order'); window.toggleMegaMenu();" class="hover:text-blue-600 font-semibold text-left">
              <i class="fa-solid fa-truck mr-1 text-slate-400"></i> Tra cứu đơn hàng online
            </button>
          </li>
          <li>
            <button type="button" onclick="window.CellphoneSLookup.openModal('warranty'); window.toggleMegaMenu();" class="hover:text-blue-600 font-semibold text-left">
              <i class="fa-solid fa-shield-halved mr-1 text-slate-400"></i> Tra cứu bảo hành IMEI
            </button>
          </li>
        </ul>
      </div>

      <!-- Cột S-Member -->
      <div class="space-y-3 bg-gradient-to-br from-[#F0FDF4] to-indigo-50 p-4 rounded-2xl border border-[#DCFCE7]">
        <div class="font-extrabold text-[#15803D] text-xs uppercase flex items-center gap-1.5">
          <i class="fa-solid fa-award text-amber-500"></i>
          <span>Thành Viên S-Member</span>
        </div>
        <p class="text-[11px] text-slate-600">Tích điểm đổi quà, tra cứu lịch sử mua sắm và bảo hành thiết bị.</p>
        <button type="button" onclick="window.CellphoneSCustomerPortal.openLoginModal(); window.toggleMegaMenu();" class="w-full py-2 rounded-xl bg-white hover:bg-slate-100 text-slate-900 font-extrabold text-xs text-center shadow-md border border-slate-200">
          Đăng Nhập S-Member
        </button>
      </div>
    `;

    html += `</div>`;
    container.innerHTML = html;
  }

  window.filterByLoaiHang = function (tenLoai) {
    window.openSearchModal(tenLoai, tenLoai);
  };

  window.filterByNhomHang = function (maNhom) {
    if (maNhom === 'NH01' || maNhom === 'DIENTHOAI') {
      const shelf = document.getElementById('shelf-phones');
      if (shelf) shelf.scrollIntoView({ behavior: 'smooth' });
    } else {
      const shelf = document.getElementById('shelf-accessories');
      if (shelf) shelf.scrollIntoView({ behavior: 'smooth' });
    }
  };

  /**
   * Mega Menu Toggle (Mobile & Desktop)
   */
  /**
   * =========================================================================
   * KHỐI TRẠM TIẾP SỨC ONLINE - GIÁ TỐT MỖI NGÀY (Chuẩn mẫu Hoàng Hà Mobile)
   * =========================================================================
   */
  const dealCategoriesData = {
    amthanh: [
      {
        id: 'DEAL-AT-01',
        name: 'Loa Bluetooth hiệu Alpha Works Classic V21',
        price: 2690000,
        oldPrice: 2990000,
        discount: 10,
        image: 'images/products/alpha-works-classic.png'
      },
      {
        id: 'DEAL-AT-02',
        name: 'Tai nghe AirPods Pro 3 (Chính hãng Apple)',
        price: 5790000,
        oldPrice: 6790000,
        discount: 15,
        image: 'images/products/airpods-pro-3.png'
      },
      {
        id: 'DEAL-AT-03',
        name: 'Tai nghe Sennheiser ACCENTUM Wireless',
        price: 1990000,
        oldPrice: 2390000,
        discount: 17,
        image: 'images/products/sennheiser-accentum.png'
      },
      {
        id: 'DEAL-AT-04',
        name: 'Loa di động không dây Sony ULT FIELD 1',
        price: 3290000,
        oldPrice: 3590000,
        discount: 8,
        image: 'images/products/sony-ult-field-1.png'
      },
      {
        id: 'DEAL-AT-05',
        name: 'Tai nghe chống ồn Sony WH-1000XM5 Hi-Res',
        price: 9790000,
        oldPrice: 11990000,
        discount: 18,
        image: 'images/products/airpods-pro-3.png'
      },
      {
        id: 'DEAL-AT-06',
        name: 'Loa Bluetooth JBL Charge 5 Siêu Trầm Bass',
        price: 3490000,
        oldPrice: 3990000,
        discount: 12,
        image: 'images/deal-tabs/deal-tab-audio.jpg'
      },
      {
        id: 'DEAL-AT-07',
        name: 'Tai nghe Marshall Major IV Bluetooth Cổ Điển',
        price: 3190000,
        oldPrice: 3690000,
        discount: 14,
        image: 'images/deal-tabs/deal-tab-audio.jpg'
      }
    ],
    dienthoai: [
      {
        id: 'DEAL-DT-01',
        name: 'iPhone 16 Pro 256GB - Chính Hãng VN/A',
        price: 28490000,
        oldPrice: 31990000,
        discount: 11,
        image: 'images/products/mh123.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-02',
        name: 'Samsung Galaxy S26 Ultra 12GB/256GB',
        price: 30490000,
        oldPrice: 32990000,
        discount: 8,
        image: 'images/products/mh109.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-03',
        name: 'Gaming REDMAGIC 10 PRO 16GB/512GB Quốc Tế',
        price: 17990000,
        oldPrice: 19990000,
        discount: 10,
        image: 'images/products/mh189.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-04',
        name: 'Xiaomi Redmi Note 14 Pro+ 5G 8GB/256GB',
        price: 9490000,
        oldPrice: 10990000,
        discount: 14,
        image: 'images/products/mh178.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-05',
        name: 'Xiaomi Redmi Note 14 8GB/128GB',
        price: 4990000,
        oldPrice: 5790000,
        discount: 14,
        image: 'images/products/mh161.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-06',
        name: 'TECNO POVA CURVE 2 5G 8GB/128GB',
        price: 5490000,
        oldPrice: 6490000,
        discount: 15,
        image: 'images/products/mh159.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-07',
        name: 'OPPO Find X10 Pro Max 16GB/512GB',
        price: 34990000,
        oldPrice: 37990000,
        discount: 8,
        image: 'images/products/mh129.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-08',
        name: 'REALME C75X 6GB/128GB',
        price: 4190000,
        oldPrice: 4690000,
        discount: 11,
        image: 'images/products/mh174.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-09',
        name: 'Tecno Camon 40 Pro 8GB/256GB',
        price: 5990000,
        oldPrice: 6990000,
        discount: 14,
        image: 'images/products/mh172.png',
        status: 'Sẵn hàng'
      },
      {
        id: 'DEAL-DT-10',
        name: 'HONOR 400 PRO 5G 12GB/512GB',
        price: 15990000,
        oldPrice: 17990000,
        discount: 11,
        image: 'images/products/mh152.png',
        status: 'Sẵn hàng'
      }
    ],
    tablet: [
      {
        id: 'DEAL-TB-01',
        name: 'iPad Air M2 11 inch Wi-Fi 128GB VN/A Siêu Mỏng',
        price: 15890000,
        oldPrice: 16990000,
        discount: 6,
        image: 'images/products/ipad-air-m2.webp'
      },
      {
        id: 'DEAL-TB-02',
        name: 'iPad Pro M4 11 inch Ultra Retina XDR 256GB',
        price: 27990000,
        oldPrice: 29990000,
        discount: 7,
        image: 'images/products/ipad-air-m2.webp'
      },
      {
        id: 'DEAL-TB-03',
        name: 'iPad Gen 10 10.9 inch Wi-Fi 64GB Đủ Màu',
        price: 8990000,
        oldPrice: 9990000,
        discount: 10,
        image: 'images/deal-tabs/deal-tab-tablet.jpg'
      },
      {
        id: 'DEAL-TB-04',
        name: 'Samsung Galaxy Tab S9 FE Wi-Fi 128GB Kèm Bút S-Pen',
        price: 8490000,
        oldPrice: 9990000,
        discount: 15,
        image: 'images/deal-tabs/deal-tab-tablet.jpg'
      },
      {
        id: 'DEAL-TB-05',
        name: 'Xiaomi Pad 6 8GB/256GB Màn Hình 144Hz 2.8K',
        price: 7290000,
        oldPrice: 8490000,
        discount: 14,
        image: 'images/deal-tabs/deal-tab-tablet.jpg'
      },
      {
        id: 'DEAL-TB-06',
        name: 'Bút cảm ứng Apple Pencil Pro Dành Cho iPad M4/M2',
        price: 3290000,
        oldPrice: 3490000,
        discount: 6,
        image: 'images/deal-tabs/deal-tab-tablet.jpg'
      }
    ],
    dongho: [
      {
        id: 'DEAL-DH-01',
        name: 'Apple Watch Series 10 Nhôm 42mm GPS Dây Thể Thao',
        price: 9990000,
        oldPrice: 10990000,
        discount: 9,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      },
      {
        id: 'DEAL-DH-02',
        name: 'Apple Watch Ultra 2 Titanium 49mm GPS + Cellular',
        price: 19990000,
        oldPrice: 21990000,
        discount: 9,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      },
      {
        id: 'DEAL-DH-03',
        name: 'Samsung Galaxy Watch7 40mm Bluetooth Galaxy AI',
        price: 5490000,
        oldPrice: 6490000,
        discount: 15,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      },
      {
        id: 'DEAL-DH-04',
        name: 'Huawei Watch GT 5 Pro Dây Titanium Sang Trọng',
        price: 7990000,
        oldPrice: 8990000,
        discount: 11,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      },
      {
        id: 'DEAL-DH-05',
        name: 'Xiaomi Redmi Watch 4 Màn AMOLED Pin 20 Ngày',
        price: 2190000,
        oldPrice: 2690000,
        discount: 18,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      },
      {
        id: 'DEAL-DH-06',
        name: 'Đồng hồ thể thao Garmin Forerunner 165 Music',
        price: 6990000,
        oldPrice: 7990000,
        discount: 12,
        image: 'images/deal-tabs/deal-tab-watch.jpg'
      }
    ],
    phukien: [
      {
        id: 'DEAL-PK-01',
        name: 'Củ sạc nhanh Apple 20W Type-C Chính Hãng VN/A',
        price: 490000,
        oldPrice: 590000,
        discount: 17,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      },
      {
        id: 'DEAL-PK-02',
        name: 'Cáp sạc C to C Apple Woven 1m Siêu Bền Dù',
        price: 390000,
        oldPrice: 550000,
        discount: 29,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      },
      {
        id: 'DEAL-PK-03',
        name: 'Pin dự phòng MagSafe 10000mAh Không Dây Hút Nam Châm',
        price: 690000,
        oldPrice: 890000,
        discount: 22,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      },
      {
        id: 'DEAL-PK-04',
        name: 'Ốp lưng iPhone 16 Pro Max MagSafe Trong Suốt Chống Ố',
        price: 290000,
        oldPrice: 450000,
        discount: 35,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      },
      {
        id: 'DEAL-PK-05',
        name: 'Củ sạc Anker 30W GaN Nano II Nhỏ Gọn Sạc Nhanh',
        price: 380000,
        oldPrice: 450000,
        discount: 15,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      },
      {
        id: 'DEAL-PK-06',
        name: 'Kính cường lực KingKong Chống Nhìn Trộm Cao Cấp',
        price: 150000,
        oldPrice: 220000,
        discount: 32,
        image: 'images/deal-tabs/deal-tab-accessory.jpg'
      }
    ],
    laptop: [
      {
        id: 'DEAL-LT-01',
        name: 'MacBook Air M3 13 inch 16GB/256GB Chính Hãng Apple',
        price: 26990000,
        oldPrice: 28990000,
        discount: 7,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-02',
        name: 'MacBook Pro M3 Pro 14 inch 18GB/512GB Space Black',
        price: 47990000,
        oldPrice: 49990000,
        discount: 4,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-03',
        name: 'Laptop ASUS Zenbook 14 OLED Intel Core Ultra 7 Mới',
        price: 28490000,
        oldPrice: 31990000,
        discount: 11,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-04',
        name: 'Màn hình Gaming LG UltraGear 27 inch 165Hz IPS 1ms',
        price: 4990000,
        oldPrice: 6290000,
        discount: 21,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-05',
        name: 'Màn hình Dell UltraSharp U2724D 27 inch 2K IPS Black',
        price: 9490000,
        oldPrice: 10990000,
        discount: 14,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-06',
        name: 'Laptop Dell Inspiron 15 3520 i5 1235U / 16GB / 512GB',
        price: 13990000,
        oldPrice: 15490000,
        discount: 10,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-07',
        name: 'Laptop Lenovo ThinkPad E14 Gen 5 Ryzen 5 7530U',
        price: 15490000,
        oldPrice: 17290000,
        discount: 10,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-08',
        name: 'Laptop HP Pavilion 15 Core i5 1335U / 16GB / 512GB',
        price: 16890000,
        oldPrice: 18990000,
        discount: 11,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-09',
        name: 'Màn hình ASUS ProArt PA278CV 27 inch 2K IPS Chuẩn Màu',
        price: 8690000,
        oldPrice: 9990000,
        discount: 13,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      },
      {
        id: 'DEAL-LT-10',
        name: 'Laptop Gaming Acer Nitro 5 Tiger Core i5 RTX 3050',
        price: 18990000,
        oldPrice: 21990000,
        discount: 14,
        image: 'images/deal-tabs/deal-tab-laptop.jpg'
      }
    ]
  };

  let currentDealCategory = 'dienthoai';
  let currentDealIndex = 0;

  async function loadDealProductsFromJson() {
    try {
      const res = await fetch('products-meta.json');
      if (!res.ok) return;
      const metaMap = await res.json();
      const allItems = Object.values(metaMap || {});
      if (!allItems.length) return;

      const mapCat = {
        dienthoai: allItems.filter(p => p.maNhom === 'DIENTHOAI'),
        tablet: allItems.filter(p => p.maNhom === 'TABLET'),
        amthanh: allItems.filter(p => p.maNhom === 'AMTHANH'),
        dongho: allItems.filter(p => p.maNhom === 'DONGHO'),
        phukien: allItems.filter(p => p.maNhom === 'PHUKIEN'),
        laptop: allItems.filter(p => p.maNhom === 'LAPTOP' || (p.tenHang || '').toLowerCase().includes('laptop') || (p.tenHang || '').toLowerCase().includes('macbook') || (p.tenHang || '').toLowerCase().includes('màn hình'))
      };

      const toDealItem = (p) => {
        const price = Number(p.donGia) || 0;
        const oldPrice = p.giaGoc ? Number(p.giaGoc) : (price > 0 ? Math.round(price * 1.08) : null);
        const discount = (oldPrice && oldPrice > price) ? Math.round((1 - price / oldPrice) * 100) : null;
        return {
          id: p.maHang,
          name: p.tenHang,
          price: price,
          oldPrice: oldPrice,
          discount: discount,
          image: p.anhChinh || 'images/deal-tabs/deal-tab-phone.jpg',
          status: 'Sẵn hàng',
          rawProduct: p
        };
      };

      // Cập nhật đủ cả 6 tab ngành hàng từ products-meta.json (tối thiểu 10 sản phẩm mỗi tab)
      ['dienthoai', 'tablet', 'amthanh', 'dongho', 'phukien', 'laptop'].forEach(cat => {
        if (mapCat[cat] && mapCat[cat].length > 0) {
          dealCategoriesData[cat] = mapCat[cat].map(toDealItem).slice(0, 12);
        }
      });

      // Render lại giao diện sau khi nạp xong dữ liệu đầy đủ
      renderDealProducts();
    } catch (err) {
      console.warn('Load products-meta.json error:', err);
    }
  }

  function debounce(func, wait) {
    let timeout;
    return function (...args) {
      clearTimeout(timeout);
      timeout = setTimeout(() => func.apply(this, args), wait);
    };
  }

  function initTramTiepSucSection() {
    const track = document.getElementById('deal-products-track');
    if (!track) return;
    renderDealProducts();
    loadDealProductsFromJson();
    window.addEventListener('resize', debounce(() => {
      applyDealSliderPosition();
    }, 150));
  }

  function getCardsPerView() {
    const w = window.innerWidth;
    if (w >= 1024) return 5;
    if (w >= 640) return 3;
    return 2;
  }

  function formatCurrency(amount) {
    if (amount === null || amount === undefined || isNaN(amount)) return '';
    return Number(amount).toLocaleString('vi-VN') + '₫';
  }

  function renderDealProducts() {
    const track = document.getElementById('deal-products-track');
    if (!track) return;

    const list = dealCategoriesData[currentDealCategory] || [];
    track.innerHTML = list.map(item => `
      <div class="w-1/2 sm:w-1/3 lg:w-1/5 flex-shrink-0 px-2.5 sm:px-3.5 py-2 border-r border-slate-100 last:border-r-0">
        <div class="deal-product-item h-full flex flex-col justify-between group/card select-none cursor-pointer" onclick="window.onDealProductClick('${item.id}')">
          <!-- Ảnh Sản Phẩm (Nền trắng tinh, kích thước to rõ nét) -->
          <div class="relative w-full h-44 sm:h-52 lg:h-56 mb-2 flex items-center justify-center p-0.5 overflow-hidden">
            <img src="${item.image}" alt="${item.name}" class="h-full w-full object-contain block mx-auto transition-transform duration-300 transform scale-105 group-hover/card:scale-115" loading="lazy" onerror="this.onerror=null; this.src='images/deal-tabs/deal-tab-phone.jpg';" />
          </div>

          <!-- Tiêu Đề Sản Phẩm -->
          <h4 class="text-xs sm:text-[13px] font-semibold text-slate-800 line-clamp-2 min-h-[38px] leading-snug hover:text-[#0057B7] transition-colors mb-2" title="${item.name}">
            ${item.name}
          </h4>

          <!-- Giá & Giảm Giá -->
          <div class="mb-2.5">
            <div class="flex items-baseline gap-1.5 flex-wrap">
              <span class="text-sm sm:text-base font-bold text-[#E11B1E]">${formatCurrency(item.price)}</span>
              ${item.discount ? `<span class="text-[10px] font-bold text-[#E11B1E] bg-rose-50 px-1 py-0.5 rounded border border-rose-100">-${item.discount}%</span>` : ''}
            </div>
            ${item.oldPrice ? `<div class="text-[11px] text-slate-400 line-through mt-0.5">${formatCurrency(item.oldPrice)}</div>` : '<div class="h-[18px]"></div>'}
          </div>

          <!-- Footer: Sẵn hàng + Nút thêm giỏ hàng màu xanh dương thương hiệu -->
          <div class="flex items-center justify-between pt-2 border-t border-slate-100 text-xs">
            <div class="flex items-center gap-1 text-[11px] text-emerald-600 font-normal">
              <i class="fa-regular fa-circle-check text-xs"></i>
              <span>Sẵn hàng</span>
            </div>
            <button type="button" onclick="window.addDealToCart('${item.id}', event)" class="w-7 h-7 sm:w-8 sm:h-8 rounded-lg bg-blue-50 hover:bg-[#0057B7] text-[#0057B7] hover:text-white border border-blue-200 flex items-center justify-center transition-all active:scale-90 cursor-pointer shadow-2xs" title="Thêm vào giỏ hàng">
              <i class="fa-solid fa-cart-shopping text-xs"></i>
            </button>
          </div>
        </div>
      </div>
    `).join('');

    applyDealSliderPosition();
  }

  function applyDealSliderPosition() {
    const track = document.getElementById('deal-products-track');
    if (!track) return;
    const cardsPerView = getCardsPerView();
    const list = dealCategoriesData[currentDealCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    if (currentDealIndex > maxIndex) currentDealIndex = maxIndex;
    if (currentDealIndex < 0) currentDealIndex = 0;

    const colWidthPercent = 100 / cardsPerView;
    track.style.transform = `translateX(-${currentDealIndex * colWidthPercent}%)`;
  }

  window.switchDealCategory = function (cat) {
    if (!dealCategoriesData[cat]) return;
    currentDealCategory = cat;
    currentDealIndex = 0;

    // Cập nhật tab active
    const tabs = document.querySelectorAll('.deal-tab-btn');
    tabs.forEach(btn => {
      if (btn.dataset.dealCat === cat) {
        btn.classList.add('active-deal-tab');
      } else {
        btn.classList.remove('active-deal-tab');
      }
    });

    renderDealProducts();
  };

  window.nextDealProduct = function () {
    const cardsPerView = getCardsPerView();
    const list = dealCategoriesData[currentDealCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    if (currentDealIndex < maxIndex) {
      currentDealIndex++;
    } else {
      currentDealIndex = 0;
    }
    applyDealSliderPosition();
  };

  window.prevDealProduct = function () {
    const cardsPerView = getCardsPerView();
    const list = dealCategoriesData[currentDealCategory] || [];
    const maxIndex = Math.max(0, list.length - cardsPerView);
    if (currentDealIndex > 0) {
      currentDealIndex--;
    } else {
      currentDealIndex = maxIndex;
    }
    applyDealSliderPosition();
  };

  window.addDealToCart = function (id, event) {
    if (event) event.stopPropagation();
    const list = dealCategoriesData[currentDealCategory] || [];
    const item = list.find(p => p.id === id);
    if (!item) return;

    if (window.CellphoneSCart || window.Cart) {
      (window.CellphoneSCart || window.Cart).addItem({
        maHang: item.id,
        tenHang: item.name,
        donGia: item.price,
        giaGoc: item.oldPrice || item.price,
        anhChinh: item.image,
        mauSac: 'Mặc định',
        phienBan: 'Tiêu chuẩn'
      }, 1);

      (window.CellphoneSCart || window.Cart).openDrawer();
    }
  };

  window.onDealProductClick = function (id) {
    const list = dealCategoriesData[currentDealCategory] || [];
    const item = list.find(p => p.id === id);
    if (!item) return;

    if (window.CellphoneSProductModal || window.ProductModal) {
      const modal = window.CellphoneSProductModal || window.ProductModal;
      // Nạp rawProduct vào StoreData nếu chưa có để modal đọc được full màu sắc, cấu hình
      if (item.rawProduct && (window.CellphoneSData || window.StoreData)) {
        const store = window.CellphoneSData || window.StoreData;
        if (!store.getProductById(item.id)) {
          store.products.push(item.rawProduct);
        }
      }
      modal.open(item.id);
    }
  };

  window.toggleMegaMenu = function () {
    const menu = document.getElementById('mega-menu-flyout');
    if (menu) {
      menu.classList.toggle('hidden');
    }
  };

  // Khởi động toàn bộ ứng dụng sau khi toàn bộ hằng số và hàm đã được định nghĩa
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initApp);
  } else {
    initApp();
  }

})();
