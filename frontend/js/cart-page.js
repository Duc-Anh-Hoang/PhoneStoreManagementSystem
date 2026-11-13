// =========================================================================
// SAMAMOBILE - CART & ORDER MANAGEMENT PAGE CONTROLLER (THUẦN FRONTEND)
// Quản lý giỏ hàng, thanh toán VietQR & Ví điện tử, theo dõi đơn hàng & đơn hủy
// =========================================================================

(function () {
  'use strict';

  const STORAGE_KEY = 'cellphones_cart';
  const ORDERS_STORAGE_KEY = 'samamobile_user_orders';
  const RECENTLY_VIEWED_KEY = 'samamobile_recently_viewed';

  const VOUCHERS = {
    'SAMAMEMBER': { code: 'SAMAMEMBER', discount: 100000, type: 'fixed', minOrder: 500000, desc: 'Giảm 100.000đ cho hội viên Sama' },
    'HELLOSAMA': { code: 'HELLOSAMA', discount: 5, type: 'percent', minOrder: 0, desc: 'Giảm 5% cho đơn hàng đầu tiên' },
    'GIAM100K': { code: 'GIAM100K', discount: 100000, type: 'fixed', minOrder: 1000000, desc: 'Giảm 100.000đ đơn từ 1 triệu' },
    'FREE50K': { code: 'FREE50K', discount: 50000, type: 'fixed', minOrder: 300000, desc: 'Tặng voucher 50.000đ phụ kiện' }
  };

  const BANK_CONFIGS = {
    'MB': { name: 'MBBank (Ngân hàng Quân Đội)', sepayBank: 'MBBank', accountNo: '0901234567', accountName: 'CONG TY TNHH SAMA MOBILE' },
    'VCB': { name: 'Vietcombank (Ngoại Thương VN)', sepayBank: 'Vietcombank', accountNo: '1029384756', accountName: 'CONG TY TNHH SAMA MOBILE' },
    'TCB': { name: 'Techcombank (Kỹ Thương VN)', sepayBank: 'Techcombank', accountNo: '1903847561', accountName: 'CONG TY TNHH SAMA MOBILE' },
    'ACB': { name: 'ACB (Ngân hàng Á Châu)', sepayBank: 'ACB', accountNo: '246813579', accountName: 'CONG TY TNHH SAMA MOBILE' }
  };

  const RECOMMENDED_DEALS = [
    {
      id: 'REC_DT01',
      maHang: 'REC_DT01',
      tenHang: 'iPhone 16 Pro Max 256GB Titan Sa Mạc',
      name: 'iPhone 16 Pro Max 256GB Titan Sa Mạc',
      price: 34490000,
      donGia: 34490000,
      oldPrice: 36990000,
      giaGoc: 36990000,
      image: 'https://i.hoanghamobile.vn/Uploads/2026/09/10/iphone-18-pro-max-burgundy-pdp-images-vi-vn-021130982.png',
      discount: '-7%',
      category: 'dien-thoai',
      badge: 'Bán chạy nhất'
    },
    {
      id: 'REC_DT02',
      maHang: 'REC_DT02',
      tenHang: 'Samsung Galaxy S24 Ultra 12GB/256GB',
      name: 'Samsung Galaxy S24 Ultra 12GB/256GB',
      price: 26990000,
      donGia: 26990000,
      oldPrice: 33990000,
      giaGoc: 33990000,
      image: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png',
      discount: '-21%',
      category: 'dien-thoai',
      badge: 'Galaxy AI'
    },
    {
      id: 'REC_DT03',
      maHang: 'REC_DT03',
      tenHang: 'Xiaomi 14 Ultra 16GB/512GB Leica',
      name: 'Xiaomi 14 Ultra 16GB/512GB Leica',
      price: 27990000,
      donGia: 27990000,
      oldPrice: 32990000,
      giaGoc: 32990000,
      image: 'https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/none/env:production/plain/https://cellphones.com.vn/media/catalog/product/x/i/xiaomi-14-ultra_1__1.png',
      discount: '-15%',
      category: 'dien-thoai',
      badge: 'Leica Optics'
    },
    {
      id: 'REC_PK01',
      maHang: 'REC_PK01',
      tenHang: 'AirPods Pro 2 USB-C Chống Ồn Chủ Động',
      name: 'AirPods Pro 2 USB-C Chống Ồn Chủ Động',
      price: 5390000,
      donGia: 5390000,
      oldPrice: 6190000,
      giaGoc: 6190000,
      image: 'https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/none/env:production/plain/https://cellphones.com.vn/media/catalog/product/a/p/apple-airpods-pro-2-usb-c_1.png',
      discount: '-13%',
      category: 'phu-kien',
      badge: 'Âm thanh vòm'
    }
  ];

  function getActiveBankConfig() {
    try {
      const saved = localStorage.getItem('samamobile_custom_bank_config');
      if (saved) {
        const parsed = JSON.parse(saved);
        if (parsed && parsed.accountNo) return parsed;
      }
    } catch (e) {}
    return BANK_CONFIGS[selectedBank] || BANK_CONFIGS['MB'];
  }

  function getQrCodeUrl(bankCfg, amount, memo) {
    const bank = bankCfg.sepayBank || bankCfg.name?.split(' ')[0] || selectedBank || 'MBBank';
    const acc = bankCfg.accountNo || '0901234567';
    const amt = Math.max(1000, Math.round(Number(amount) || 0));
    const safeMemo = (memo || 'SAMA').replace(/[^a-zA-Z0-9 ]/g, '').trim();
    return `https://qr.sepay.vn/img?bank=${encodeURIComponent(bank)}&acc=${encodeURIComponent(acc)}&amount=${amt}&des=${encodeURIComponent(safeMemo)}`;
  }

  let appliedVoucher = null;
  let useMemberPoints = false; // Tài khoản có 200 Đ điểm tích lũy
  let deliveryMethod = 'delivery'; // 'delivery' | 'store'
  let paymentMethod = 'banking'; // 'banking' | 'momo' | 'cod' | 'card'
  let selectedBank = 'MB'; // 'MB' | 'VCB' | 'TCB' | 'ACB'
  let selectedWallet = 'momo'; // 'momo' | 'vnpay' | 'zalopay'
  let currentActiveTab = 'cart'; // 'cart' | 'orders'
  let currentOrderStatusFilter = 'all'; // 'all' | 'cho_xac_nhan' | 'dang_giao' | 'da_giao' | 'da_huy'
  let pendingCancelOrderCode = null;
  let activeModalOrderCode = null;
  let qrCountdownInterval = null;

  // Quản lý các item được chọn thanh toán (Mặc định chọn tất cả)
  let selectedItemIds = new Set();

  // Quản lý Undo Toast khi xóa item
  let lastDeletedData = null;
  let undoToastTimeout = null;

  const SAMPLE_ORDERS = [];

  // Quản lý phí vận chuyển GHN & Quote
  let currentShippingFee = 0;
  let currentShippingActual = 0;
  let currentShippingSaved = 0;
  let currentExpectedDelivery = null;
  let quoteDebounceTimer = null;


  function formatProductVariantTitle(baseTitle, variantName) {
    if (!baseTitle) return 'Sản phẩm';
    const v = (variantName || '').trim();
    if (!v || v === 'Tiêu chuẩn' || v === 'Mặc định') return baseTitle;
    const capRegex = /(\d+\s*GB\s*\/\s*\d+\s*(?:GB|TB)|\b(?:64|128|256|512)\s*GB\b|\b[12]\s*TB\b)/i;
    if (capRegex.test(baseTitle)) {
      return baseTitle.replace(capRegex, v);
    }
    if (!baseTitle.toLowerCase().includes(v.toLowerCase())) {
      return baseTitle + ' ' + v;
    }
    return baseTitle;
  }

  function getCart() {
    try {
      const c = localStorage.getItem(STORAGE_KEY);
      const parsed = c ? JSON.parse(c) : [];
      if (!Array.isArray(parsed)) return [];
      return parsed.map(it => {
        if (!it) return it;
        const vName = it.variantName || it.phienBan || '';
        const syncedTitle = formatProductVariantTitle(it.tenHang || it.name || 'Sản phẩm', vName);
        return { ...it, tenHang: syncedTitle, name: syncedTitle };
      });
    } catch (e) {
      return [];
    }
  }

  function saveCart(items) {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(items || []));
      updateHeaderBadge();
    } catch (e) {}
  }

  window.getCart = getCart;
  window.saveCart = saveCart;

  function getOrders() {
    try {
      const o = localStorage.getItem(ORDERS_STORAGE_KEY);
      let userOrders = o ? JSON.parse(o) : [];
      if (!Array.isArray(userOrders)) userOrders = [];

      // Đồng bộ trạng thái mới nhất từ kho đơn Admin (cellphones_online_orders)
      try {
        const adminRaw = localStorage.getItem('cellphones_online_orders');
        if (adminRaw) {
          const adminOrders = JSON.parse(adminRaw);
          if (Array.isArray(adminOrders)) {
            const adminMap = new Map(adminOrders.filter(x => x && x.maDH).map(x => [x.maDH, x]));
            let changed = false;
            userOrders.forEach(uo => {
              const ao = adminMap.get(uo.maDH);
              if (ao && ao.trangThai && ao.trangThai !== uo.trangThai) {
                uo.trangThai = ao.trangThai;
                if (ao.soHD) uo.soHD = ao.soHD;
                changed = true;
              }
            });
            if (changed) {
              localStorage.setItem(ORDERS_STORAGE_KEY, JSON.stringify(userOrders));
            }
          }
        }
      } catch (err) {}

      return userOrders;
    } catch (e) {
      return SAMPLE_ORDERS;
    }
  }

  function saveOrders(orders) {
    try {
      localStorage.setItem(ORDERS_STORAGE_KEY, JSON.stringify(orders));

      // Đồng bộ sang kho đơn hàng chung của Admin (cellphones_online_orders)
      try {
        const adminRaw = localStorage.getItem('cellphones_online_orders');
        const adminOrders = adminRaw ? JSON.parse(adminRaw) : [];
        const map = new Map();
        (Array.isArray(orders) ? orders : []).forEach(o => {
          if (o && o.maDH) map.set(o.maDH, o);
        });
        (Array.isArray(adminOrders) ? adminOrders : []).forEach(ao => {
          if (ao && ao.maDH) {
            if (!map.has(ao.maDH)) {
              map.set(ao.maDH, ao);
            } else {
              map.set(ao.maDH, { ...ao, ...map.get(ao.maDH) });
            }
          }
        });
        const merged = Array.from(map.values()).sort((a, b) => new Date(b.ngayDat || 0).getTime() - new Date(a.ngayDat || 0).getTime());
        localStorage.setItem('cellphones_online_orders', JSON.stringify(merged));
        localStorage.setItem('cellphones_order_last_updated', Date.now().toString());
      } catch (err) {}

      updateOrderCounts();
    } catch (e) {}
  }

  function formatVND(num) {
    if (!num || isNaN(num)) return '0₫';
    return Number(num).toLocaleString('vi-VN') + '₫';
  }

  function getCartFinalTotal() {
    const items = getCart();
    const selectedItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return selectedItemIds.size === 0 || selectedItemIds.has(id);
    });

    const subtotal = selectedItems.reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);
    let memberDiscount = 0;
    if (subtotal >= 1000000) memberDiscount = Math.round(subtotal * 0.01);

    let pointsDiscount = 0;
    if (useMemberPoints && subtotal > 0) {
      pointsDiscount = Math.min(200000, subtotal);
    }

    let voucherDiscount = 0;
    if (appliedVoucher && subtotal > 0) {
      if (appliedVoucher.type === 'percent') {
        voucherDiscount = Math.round((subtotal * appliedVoucher.discount) / 100);
      } else {
        voucherDiscount = appliedVoucher.discount;
      }
    }
    return Math.max(0, subtotal - memberDiscount - pointsDiscount - voucherDiscount);
  }

  function updateHeaderBadge() {
    const items = getCart();
    const totalQty = items.reduce((s, it) => s + (Number(it.qty || it.soLuong || 1)), 0);
    const badges = document.querySelectorAll('.cart-badge-count, #tab-badge-cart-count');
    badges.forEach(b => {
      b.textContent = totalQty;
      if (b.classList.contains('cart-badge-count')) {
        if (totalQty > 0) {
          b.classList.remove('hidden');
        } else {
          b.classList.add('hidden');
        }
      }
    });
  }

  function updateOrderCounts() {
    const orders = getOrders();
    const total = orders.length;
    const countWaiting = orders.filter(o => o.trangThai === 'cho_thanh_toan' || o.trangThai === 'cho_xac_nhan' || o.trangThai === 'da_xac_nhan').length;
    const countShipping = orders.filter(o => o.trangThai === 'dang_giao').length;
    const countDelivered = orders.filter(o => o.trangThai === 'da_giao' || o.trangThai === 'hoan_thanh').length;
    const countCancelled = orders.filter(o => o.trangThai === 'da_huy').length;

    const bTabOrders = document.getElementById('tab-badge-orders-count');
    if (bTabOrders) bTabOrders.textContent = total;

    const ordersBanner = document.getElementById('cart-orders-banner');
    const ordersBannerCount = document.getElementById('cart-orders-banner-count');
    if (ordersBanner && ordersBannerCount) {
      if (total > 0) {
        ordersBanner.classList.remove('hidden');
        ordersBannerCount.textContent = total;
      } else {
        ordersBanner.classList.add('hidden');
      }
    }

    if (document.getElementById('count-status-pending')) document.getElementById('count-status-pending').textContent = countWaiting;
    if (document.getElementById('count-status-waiting')) document.getElementById('count-status-waiting').textContent = countWaiting;
    if (document.getElementById('count-status-shipping')) document.getElementById('count-status-shipping').textContent = countShipping;
    if (document.getElementById('count-status-delivered')) document.getElementById('count-status-delivered').textContent = countDelivered;
    if (document.getElementById('count-status-cancelled')) document.getElementById('count-status-cancelled').textContent = countCancelled;
  }

  // Khởi tạo trang
  document.addEventListener('DOMContentLoaded', () => {
    setupMainHeader();
    setupPaymentMethods();
    initPrefillUser();
    initItemSelection();
    renderCart();
    updateHeaderBadge();
    updateOrderCounts();
    initGhnAddressSelector();

    const urlParams = new URLSearchParams(window.location.search);
    const tabParam = urlParams.get('tab');
    if (window.location.hash === '#orders' || tabParam === 'orders') {
      window.switchCartTab('orders');
    } else {
      window.switchCartTab('cart');
    }

    window.addEventListener('hashchange', () => {
      if (window.location.hash === '#orders') {
        window.switchCartTab('orders');
      } else {
        window.switchCartTab('cart');
      }
    });
  });

  // Setup Header dùng chung
  function setupMainHeader() {
    window.toggleMegaMenu = function () {
      const menu = document.getElementById('mega-menu-flyout');
      if (menu) {
        const willShow = menu.classList.contains('hidden');
        menu.classList.toggle('hidden');
        if (willShow) {
          const autoDrop = document.getElementById('search-autocomplete-dropdown');
          if (autoDrop) autoDrop.classList.add('hidden');
          const noti = document.getElementById('notification-popover');
          if (noti) noti.classList.add('hidden');
          if (window.closeCustomerAvatarMenu) window.closeCustomerAvatarMenu();
        }
      }
    };

    window.toggleNotificationPopover = function (e) {
      if (window.NotificationCenter && typeof window.NotificationCenter.toggle === 'function') {
        window.NotificationCenter.toggle(e);
        return;
      }
      const pop = document.getElementById('notification-popover');
      if (pop) pop.classList.toggle('hidden');
    };

    window.toggleCustomerAvatarMenu = function (e) {
      if (e && e.stopPropagation) e.stopPropagation();
      const pop = document.getElementById('customer-avatar-popover');
      const chev = document.getElementById('customer-avatar-chevron');
      if (!pop) return;
      const isHidden = pop.classList.contains('hidden');
      if (isHidden) {
        pop.classList.remove('hidden');
        if (chev) chev.style.transform = 'rotate(180deg)';
      } else {
        pop.classList.add('hidden');
        if (chev) chev.style.transform = 'rotate(0deg)';
      }
    };

    window.closeCustomerAvatarMenu = function () {
      const pop = document.getElementById('customer-avatar-popover');
      const chev = document.getElementById('customer-avatar-chevron');
      if (pop) pop.classList.add('hidden');
      if (chev) chev.style.transform = 'rotate(0deg)';
    };

    document.addEventListener('click', function (e) {
      const pop = document.getElementById('customer-avatar-popover');
      const btn = document.getElementById('btn-customer-avatar-toggle');
      if (pop && !pop.classList.contains('hidden') && !pop.contains(e.target) && !btn?.contains(e.target)) {
        window.closeCustomerAvatarMenu();
      }

      const notiPop = document.getElementById('notification-popover');
      const notiBtn = document.getElementById('btn-notification-toggle');
      if (notiPop && !notiPop.classList.contains('hidden') && !notiPop.contains(e.target) && !notiBtn?.contains(e.target)) {
        notiPop.classList.add('hidden');
      }

      const mega = document.getElementById('mega-menu-flyout');
      const megaBtn = document.getElementById('btn-mega-menu-toggle');
      if (mega && !mega.classList.contains('hidden') && !mega.contains(e.target) && !megaBtn?.contains(e.target)) {
        mega.classList.add('hidden');
      }
    });

    // Thanh tìm kiếm
    const searchInput = document.getElementById('search-input');
    const clearBtn = document.getElementById('btn-clear-search');
    if (searchInput) {
      searchInput.addEventListener('input', function () {
        if (clearBtn) clearBtn.classList.toggle('hidden', !this.value.trim());
      });
      searchInput.addEventListener('keydown', function (e) {
        if (e.key === 'Enter') {
          e.preventDefault();
          const q = this.value.trim();
          window.location.href = 'category.html?q=' + encodeURIComponent(q);
        }
      });
    }
    if (clearBtn && searchInput) {
      clearBtn.addEventListener('click', function () {
        searchInput.value = '';
        clearBtn.classList.add('hidden');
        searchInput.focus();
      });
    }

    // Tự động nạp tài khoản khách hàng nếu đã đăng nhập
    try {
      const savedUser = localStorage.getItem('cellphones_auth_user');
      if (savedUser) {
        const u = JSON.parse(savedUser);
        const loginBtn = document.getElementById('btn-header-login');
        const custMenu = document.getElementById('header-customer-menu');
        const staffMenu = document.getElementById('header-staff-menu');

        if (loginBtn) loginBtn.classList.add('hidden');

        if (u.role === 'admin' || u.role === 'staff' || u.vaiTro === 'Nhân viên') {
          if (staffMenu) {
            staffMenu.style.display = 'inline-flex';
            staffMenu.classList.remove('hidden');
          }
          const staffName = document.getElementById('staff-header-name');
          if (staffName) staffName.textContent = u.hoTen || u.name || 'Nhân viên';
        } else {
          if (custMenu) {
            custMenu.style.display = 'inline-flex';
            custMenu.classList.remove('hidden');
          }
          const custName = document.getElementById('customer-header-name');
          const popBadge = document.getElementById('customer-popover-badge');
    if (popBadge) {
      if (user.isStudent || user.hangThanhVien === 'S-Student' || user.hangThanhVien === 'S - Student') {
        popBadge.textContent = 'S-Student -5%';
        popBadge.className = 'px-1.5 py-0.5 rounded text-[9px] font-bold bg-blue-100 text-blue-800 border border-blue-200 flex-shrink-0';
      } else {
        popBadge.textContent = user.hangThanhVien || 'SamaMember';
        popBadge.className = 'px-1.5 py-0.5 rounded text-[9px] font-bold bg-amber-100 text-amber-800 border border-amber-200 flex-shrink-0';
      }
    }
    const popName = document.getElementById('customer-popover-name');
          const popPhone = document.getElementById('customer-popover-phone');
          const custPoints = document.getElementById('customer-header-points');
          const popPoints = document.getElementById('customer-popover-points');
          const headerAvatar = document.getElementById('customer-header-avatar');
          const popAvatar = document.getElementById('customer-popover-avatar');

          const displayName = u.hoTen || u.name || 'Khách hàng';
          if (custName) custName.textContent = displayName;
          if (popName) popName.textContent = displayName;
          if (popPhone) popPhone.textContent = u.email || u.sdt || 'Khách hàng Sama';
          if (custPoints) custPoints.textContent = (u.diemTichLuy || 200) + ' Đ';
          if (popPoints) popPoints.textContent = (u.diemTichLuy || 200);

          const firstLetter = displayName.trim().charAt(0).toUpperCase() || 'K';
          if (headerAvatar) headerAvatar.innerHTML = '<span class="font-extrabold text-[#0057B7] text-xs">' + firstLetter + '</span>';
          if (popAvatar) popAvatar.innerHTML = '<span class="font-black text-white text-base">' + firstLetter + '</span>';
        }
      }
    } catch (e) {}

    window.CellphoneSAuth = window.CellphoneSAuth || {
      logout: function () {
        localStorage.removeItem('cellphones_auth_user');
        localStorage.removeItem('customer_token');
        localStorage.removeItem('admin_token');
        window.location.reload();
      }
    };
  }

  // Setup Bộ Chọn Phương Thức Thanh Toán (Gọn gàng, chỉ chọn hình thức - Không bung mã QR trước khi đặt hàng)
  function setupPaymentMethods() {
    const wrappers = document.querySelectorAll('.payment-method-wrapper');

    wrappers.forEach(w => {
      w.addEventListener('click', function () {
        const method = this.getAttribute('data-method');
        if (!method) return;
        paymentMethod = method;

        const radio = this.querySelector('input[name="payment_method"]');
        if (radio) radio.checked = true;

        wrappers.forEach(item => {
          if (item === w) {
            item.className = 'payment-method-wrapper rounded-2xl border-2 border-[#0057B7] bg-blue-50/25 transition-all shadow-2xs';
          } else {
            item.className = 'payment-method-wrapper rounded-2xl border border-slate-200 bg-white transition-all hover:border-slate-300';
          }
        });
      });
    });
  }

  // Chuyển đổi giữa [Giỏ hàng] và [Đơn hàng của tôi]
  window.switchCartTab = function (tab) {
    currentActiveTab = tab;
    const cartView = document.getElementById('cart-view-container');
    const ordersView = document.getElementById('orders-view-container');
    const stepperContainer = document.getElementById('cart-stepper-container');
    const tabBtnCart = document.getElementById('tab-btn-cart');
    const tabBtnOrders = document.getElementById('tab-btn-orders');

    if (tab === 'orders') {
      if (cartView) cartView.classList.add('hidden');
      if (ordersView) ordersView.classList.remove('hidden');
      if (stepperContainer) stepperContainer.classList.add('hidden');

      if (tabBtnOrders) {
        tabBtnOrders.className = 'group inline-flex items-center gap-1.5 sm:gap-2 px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl font-bold text-xs sm:text-sm transition-all cursor-pointer whitespace-nowrap bg-[#0066FF] text-white shadow-xs';
      }
      if (tabBtnCart) {
        tabBtnCart.className = 'group inline-flex items-center gap-1.5 sm:gap-2 px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl font-bold text-xs sm:text-sm transition-all cursor-pointer whitespace-nowrap text-slate-600 hover:text-[#0066FF] hover:bg-blue-50';
      }

      if (window.location.hash !== '#orders') {
        history.replaceState(null, '', '#orders');
      }
      window.renderOrders();
    } else {
      if (cartView) cartView.classList.remove('hidden');
      if (ordersView) ordersView.classList.add('hidden');

      if (tabBtnCart) {
        tabBtnCart.className = 'group inline-flex items-center gap-1.5 sm:gap-2 px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl font-bold text-xs sm:text-sm transition-all cursor-pointer whitespace-nowrap bg-[#0066FF] text-white shadow-xs';
      }
      if (tabBtnOrders) {
        tabBtnOrders.className = 'group inline-flex items-center gap-1.5 sm:gap-2 px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl font-bold text-xs sm:text-sm transition-all cursor-pointer whitespace-nowrap text-slate-600 hover:text-[#0066FF] hover:bg-blue-50';
      }

      if (window.location.hash === '#orders') {
        history.replaceState(null, '', window.location.pathname + window.location.search);
      }
      window.renderCart();
    }

    updateHeaderBadge();
    updateOrderCounts();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  function initPrefillUser() {
    try {
      const userRaw = localStorage.getItem('cellphones_auth_user') || localStorage.getItem('auth_user');
      if (userRaw) {
        const u = JSON.parse(userRaw);
        if (u) {
          const nameInput = document.getElementById('order-fullname');
          const phoneInput = document.getElementById('order-phone');
          const emailInput = document.getElementById('order-email');
          const addressInput = document.getElementById('order-address');

          if (nameInput && (u.hoTen || u.name)) nameInput.value = u.hoTen || u.name;
          if (phoneInput && u.sdt) phoneInput.value = u.sdt;
          if (emailInput && u.email) emailInput.value = u.email;
          if (addressInput && u.diaChi) addressInput.value = u.diaChi;
        }
      }
    } catch (e) {}
  }

  // Khởi tạo chọn tất cả items
  function initItemSelection() {
    const items = getCart();
    selectedItemIds.clear();
    items.forEach((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      selectedItemIds.add(id);
    });
  }

  // Toggle chọn tất cả sản phẩm
  window.toggleSelectAllCart = function (isChecked) {
    const items = getCart();
    selectedItemIds.clear();
    if (isChecked) {
      items.forEach((it, idx) => {
        const id = String(it.id || `${it.maHang}_${idx}`);
        selectedItemIds.add(id);
      });
    }

    const itemCheckboxes = document.querySelectorAll('.cart-item-checkbox');
    itemCheckboxes.forEach(cb => { cb.checked = isChecked; });

    updateSelectedCountLabel();
    calculateTotals();
  };

  // Toggle chọn 1 item
  window.toggleItemSelect = function (itemId, isChecked) {
    if (isChecked) {
      selectedItemIds.add(String(itemId));
    } else {
      selectedItemIds.delete(String(itemId));
    }

    const items = getCart();
    const selectAllCheckbox = document.getElementById('cart-select-all-checkbox');
    if (selectAllCheckbox) {
      selectAllCheckbox.checked = items.length > 0 && selectedItemIds.size === items.length;
    }

    updateSelectedCountLabel();
    calculateTotals();
  };

  function updateSelectedCountLabel() {
    const label = document.getElementById('cart-selected-count-label');
    if (label) label.textContent = selectedItemIds.size;
  }

  // Bật/tắt điểm tích lũy SamaMember (200 Đ = 200.000 ₫)
  window.toggleUseMemberPoints = function (enabled) {
    useMemberPoints = enabled;
    const notice = document.getElementById('member-points-notice');
    const pointsRow = document.getElementById('summary-points-row');
    if (notice) notice.classList.toggle('hidden', !enabled);
    if (pointsRow) pointsRow.classList.toggle('hidden', !enabled);
    calculateTotals();
  };

  // Render giỏ hàng chính
  window.renderCart = function () {
    let items = getCart();
    if ((!items || items.length === 0) && new URLSearchParams(window.location.search).get('demo') === '1') {
      const demoItems = [
        {
          id: 'DEMO_1',
          maHang: 'SP_S25U',
          name: 'Samsung Galaxy S25 Ultra - 256GB',
          tenHang: 'Samsung Galaxy S25 Ultra - 256GB',
          price: 25790000,
          donGia: 25790000,
          oldPrice: 33990000,
          giaGoc: 33990000,
          qty: 1,
          soLuong: 1,
          variantName: '256GB',
          colorName: 'Mặc định',
          image: 'https://i.hoanghamobile.vn/Uploads/2025/01/23/image-removebg-preview.png'
        },
        {
          id: 'DEMO_2',
          maHang: 'SP_A18',
          name: 'Samsung Galaxy A18 - 128GB',
          tenHang: 'Samsung Galaxy A18 - 128GB',
          price: 6690000,
          donGia: 6690000,
          oldPrice: 7190000,
          giaGoc: 7190000,
          qty: 1,
          soLuong: 1,
          variantName: '128GB',
          colorName: 'Mặc định',
          image: 'https://i.hoanghamobile.vn/Uploads/2026/02/25/s26-ultra-tim-1.png'
        }
      ];
      saveCart(demoItems);
      items = demoItems;
    }
    const cartContainer = document.getElementById('cart-items-container');
    const emptyState = document.getElementById('cart-empty-state');
    const mainContent = document.getElementById('cart-main-content');
    const itemsCountLabel = document.getElementById('cart-items-count-label');
    const btnClearAll = document.getElementById('btn-clear-all-cart');
    const stepperContainer = document.getElementById('cart-stepper-container');
    const mobileBottomBar = document.getElementById('mobile-cart-bottom-bar');

    // NẾU GIỎ HÀNG TRỐNG
    if (!items || items.length === 0) {
      if (emptyState) emptyState.classList.remove('hidden');
      if (mainContent) mainContent.classList.add('hidden');
      if (itemsCountLabel) itemsCountLabel.classList.add('hidden');
      if (btnClearAll) btnClearAll.classList.add('hidden');
      if (stepperContainer) stepperContainer.classList.add('hidden');
      if (mobileBottomBar) mobileBottomBar.classList.add('hidden');

      renderEmptyCartSuggestions();
      updateTotals(0, 0, 0, 0);
      return;
    }

    // NẾU GIỎ HÀNG CÓ HÀNG
    if (emptyState) emptyState.classList.add('hidden');
    if (mainContent) mainContent.classList.remove('hidden');
    if (itemsCountLabel) itemsCountLabel.classList.remove('hidden');
    if (btnClearAll) btnClearAll.classList.remove('hidden');
    if (stepperContainer) stepperContainer.classList.remove('hidden');
    if (mobileBottomBar) mobileBottomBar.classList.remove('hidden');

    const totalQty = items.reduce((s, it) => s + (Number(it.qty || it.soLuong || 1)), 0);
    if (itemsCountLabel) itemsCountLabel.textContent = `(${totalQty} sản phẩm)`;

    // Đồng bộ selectedItemIds: nếu item mới thêm chưa có trong set thì mặc định chọn
    items.forEach((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      if (selectedItemIds.size === 0) selectedItemIds.add(id);
    });

    updateSelectedCountLabel();

    const selectAllCheckbox = document.getElementById('cart-select-all-checkbox');
    if (selectAllCheckbox) {
      selectAllCheckbox.checked = items.length > 0 && selectedItemIds.size === items.length;
    }

    if (cartContainer) {
      cartContainer.innerHTML = items.map((item, idx) => {
        const itemPrice = Number(item.price || item.donGia || 0);
        const oldPrice = Number(item.oldPrice || item.giaGoc || itemPrice);
        const qty = Number(item.qty || item.soLuong || 1);
        const subtotal = itemPrice * qty;
        const itemId = String(item.id || `${item.maHang}_${idx}`);
        const itemImg = item.image || item.hinhAnh || item.anhChinh || '../images/deal-tabs/deal-tab-phone.jpg';
        const isSelected = selectedItemIds.has(itemId);

        return `
          <div class="cart-item-card flex flex-col sm:flex-row items-start sm:items-center justify-between gap-3 sm:gap-4 p-3.5 sm:p-4 rounded-2xl border border-slate-200/90 hover:border-blue-300 hover:shadow-xs transition-all bg-white" data-item-id="${itemId}">
            
            <div class="flex items-center gap-3 flex-1 min-w-0 w-full sm:w-auto">
              <!-- Checkbox chọn sản phẩm -->
              <input type="checkbox" onchange="window.toggleItemSelect('${itemId}', this.checked)" ${isSelected ? 'checked' : ''} class="cart-item-checkbox w-4 h-4 rounded text-[#0057B7] focus:ring-[#0057B7] cursor-pointer flex-shrink-0">

              <!-- Ảnh sản phẩm -->
              <div class="w-16 h-16 sm:w-20 sm:h-20 rounded-xl bg-slate-50 p-1.5 border border-slate-100 flex-shrink-0 flex items-center justify-center overflow-hidden">
                <img src="${itemImg}" alt="${item.tenHang || item.name}" class="w-full h-full object-contain" onerror="this.src='../images/deal-tabs/deal-tab-phone.jpg'">
              </div>

              <!-- Chi tiết tên, phân loại, giá -->
              <div class="flex-1 min-w-0 space-y-1">
                <h4 class="font-bold text-sm sm:text-base text-slate-800 leading-snug truncate hover:text-[#0057B7] transition-colors" title="${item.tenHang || item.name}">
                  ${item.tenHang || item.name}
                </h4>
                <div class="flex flex-wrap items-center gap-1.5 text-xs text-slate-500">
                  ${item.variantName || item.phienBan ? `<span class="bg-slate-100 px-2 py-0.5 rounded font-semibold text-slate-700">${item.variantName || item.phienBan}</span>` : ''}
                  ${item.colorName || item.mauSac ? `<span class="bg-blue-50 text-[#0057B7] px-2 py-0.5 rounded font-semibold">${item.colorName || item.mauSac}</span>` : ''}
                  <span class="inline-flex items-center gap-1 text-[11px] text-emerald-600 font-semibold bg-emerald-50 px-1.5 py-0.5 rounded">
                    <i class="fa-solid fa-circle-check text-[10px]"></i> Còn hàng
                  </span>
                </div>
                <div class="flex items-baseline gap-2 pt-0.5">
                  <span class="text-sm sm:text-base font-bold text-[#E0052B]">${formatVND(itemPrice)}</span>
                  ${oldPrice > itemPrice ? `<span class="text-xs text-slate-400 line-through">${formatVND(oldPrice)}</span>` : ''}
                </div>
              </div>
            </div>

            <!-- Tăng giảm số lượng, thành tiền, nút xóa -->
            <div class="flex items-center justify-between sm:justify-end gap-3 sm:gap-4 w-full sm:w-auto pt-2 sm:pt-0 border-t sm:border-t-0 border-slate-100">
              <div class="flex items-center rounded-xl border border-slate-200 bg-slate-50 overflow-hidden">
                <button type="button" onclick="window.changeItemQty('${itemId}', -1)" class="w-8 h-8 flex items-center justify-center text-slate-600 hover:bg-slate-200 hover:text-slate-900 transition-colors font-bold cursor-pointer">-</button>
                <span class="w-9 text-center text-sm font-bold text-slate-800">${qty}</span>
                <button type="button" onclick="window.changeItemQty('${itemId}', 1)" class="w-8 h-8 flex items-center justify-center text-slate-600 hover:bg-slate-200 hover:text-slate-900 transition-colors font-bold cursor-pointer">+</button>
              </div>

              <div class="text-right min-w-[95px] sm:min-w-[110px]">
                <div class="text-sm sm:text-base font-black text-slate-900">${formatVND(subtotal)}</div>
              </div>

              <button type="button" onclick="window.removeCartItem('${itemId}')" class="w-9 h-9 rounded-full text-slate-400 hover:text-rose-600 hover:bg-rose-50 flex items-center justify-center transition-colors cursor-pointer" title="Xóa khỏi giỏ hàng">
                <i class="fa-regular fa-trash-can text-sm"></i>
              </button>
            </div>
          </div>
        `;
      }).join('');
    }

    calculateTotals();
  };

  // Render gợi ý và sản phẩm đã xem khi giỏ hàng trống (Empty State Discovery Launchpad)
  function renderEmptyCartSuggestions() {
    // 1. Sản phẩm đã xem gần đây (từ localStorage)
    const viewedGrid = document.getElementById('empty-cart-recently-viewed-grid');
    const viewedCount = document.getElementById('empty-cart-viewed-count');
    let viewedItems = [];
    try {
      const v = localStorage.getItem(RECENTLY_VIEWED_KEY);
      if (v) viewedItems = JSON.parse(v);
    } catch (e) {}

    if (viewedItems && viewedItems.length > 0) {
      if (viewedCount) viewedCount.textContent = `${viewedItems.length} sản phẩm`;
      if (viewedGrid) {
        viewedGrid.innerHTML = viewedItems.slice(0, 4).map(item => {
          const p = Number(item.donGia || item.price || 0);
          const op = Number(item.giaGoc || item.oldPrice || p);
          const img = item.anhChinh || item.hinhAnh || item.image || '../images/deal-tabs/deal-tab-phone.jpg';
          const name = item.tenHang || item.name || 'Sản phẩm';
          const id = item.maHang || item.id;

          return `
            <div class="bg-white rounded-2xl p-3 border border-slate-200/90 hover:border-blue-300 hover:shadow-xs transition-all flex flex-col justify-between group">
              <a href="product-detail.html?id=${id}" class="block space-y-2">
                <div class="w-full h-32 rounded-xl bg-slate-50 p-2 flex items-center justify-center overflow-hidden">
                  <img src="${img}" alt="${name}" class="w-full h-full object-contain group-hover:scale-105 transition-transform duration-200" onerror="this.src='../images/deal-tabs/deal-tab-phone.jpg'">
                </div>
                <h5 class="text-xs font-semibold text-slate-800 line-clamp-2 leading-snug group-hover:text-[#0057B7] transition-colors">${name}</h5>
                <div class="flex items-baseline gap-1.5">
                  <span class="text-xs font-bold text-[#E0052B]">${formatVND(p)}</span>
                  ${op > p ? `<span class="text-[10px] text-slate-400 line-through">${formatVND(op)}</span>` : ''}
                </div>
              </a>
              <button type="button" onclick="window.addSuggestedItemToCart(${JSON.stringify(item).replace(/"/g, '&quot;')})" class="mt-2.5 w-full py-1.5 rounded-xl bg-blue-50 hover:bg-[#0057B7] text-[#0057B7] hover:text-white font-semibold text-xs flex items-center justify-center gap-1.5 transition-colors cursor-pointer">
                <i class="fa-solid fa-cart-plus text-[10px]"></i> Thêm vào giỏ
              </button>
            </div>
          `;
        }).join('');
      }
    } else {
      const viewedSection = document.getElementById('empty-cart-recently-viewed-section');
      if (viewedSection) viewedSection.classList.add('hidden');
    }

    // 2. Gợi ý sản phẩm đang giảm giá hot
    const dealsGrid = document.getElementById('empty-cart-deals-grid');
    if (dealsGrid) {
      dealsGrid.innerHTML = RECOMMENDED_DEALS.map(deal => {
        return `
          <div class="bg-white rounded-2xl p-3 border border-slate-200/90 hover:border-blue-300 hover:shadow-xs transition-all flex flex-col justify-between group relative">
            <span class="absolute top-2.5 left-2.5 z-10 text-[9px] font-bold text-white bg-rose-600 px-1.5 py-0.5 rounded-md">${deal.discount}</span>
            <a href="product-detail.html?id=${deal.maHang}" class="block space-y-2">
              <div class="w-full h-32 rounded-xl bg-slate-50 p-2 flex items-center justify-center overflow-hidden">
                <img src="${deal.image}" alt="${deal.tenHang}" class="w-full h-full object-contain group-hover:scale-105 transition-transform duration-200" onerror="this.src='../images/deal-tabs/deal-tab-phone.jpg'">
              </div>
              <h5 class="text-xs font-semibold text-slate-800 line-clamp-2 leading-snug group-hover:text-[#0057B7] transition-colors">${deal.tenHang}</h5>
              <div class="flex items-baseline gap-1.5">
                <span class="text-xs font-bold text-[#E0052B]">${formatVND(deal.price)}</span>
                <span class="text-[10px] text-slate-400 line-through">${formatVND(deal.oldPrice)}</span>
              </div>
            </a>
            <button type="button" onclick="window.addSuggestedItemToCart(${JSON.stringify(deal).replace(/"/g, '&quot;')})" class="mt-2.5 w-full py-1.5 rounded-xl bg-slate-100 hover:bg-[#0057B7] text-slate-800 hover:text-white font-semibold text-xs flex items-center justify-center gap-1.5 transition-colors cursor-pointer">
              <i class="fa-solid fa-cart-plus text-[10px]"></i> Thêm vào giỏ
            </button>
          </div>
        `;
      }).join('');
    }
  }

  // Thêm nhanh sản phẩm gợi ý vào giỏ hàng
  window.addSuggestedItemToCart = function (item) {
    let items = getCart();
    const targetId = item.id || item.maHang;
    const idx = items.findIndex(it => (it.id || it.maHang) === targetId);

    if (idx > -1) {
      items[idx].qty = Number(items[idx].qty || items[idx].soLuong || 1) + 1;
      items[idx].soLuong = items[idx].qty;
    } else {
      items.unshift({
        id: targetId,
        maHang: item.maHang || targetId,
        tenHang: item.tenHang || item.name,
        name: item.tenHang || item.name,
        price: Number(item.donGia || item.price || 0),
        donGia: Number(item.donGia || item.price || 0),
        oldPrice: Number(item.giaGoc || item.oldPrice || item.donGia || item.price || 0),
        giaGoc: Number(item.giaGoc || item.oldPrice || item.donGia || item.price || 0),
        qty: 1,
        soLuong: 1,
        image: item.anhChinh || item.image || item.hinhAnh || '../images/deal-tabs/deal-tab-phone.jpg'
      });
    }

    selectedItemIds.add(String(targetId));
    saveCart(items);
    renderCart();
    showToast(`Đã thêm "${item.tenHang || item.name}" vào giỏ hàng!`);
  };

  // Tăng giảm số lượng sản phẩm
  window.changeItemQty = function (itemId, delta) {
    let items = getCart();
    const idx = items.findIndex((it, i) => String(it.id || `${it.maHang}_${i}`) === String(itemId));
    if (idx === -1) return;

    let currentQty = Number(items[idx].qty || items[idx].soLuong || 1);
    let newQty = currentQty + delta;

    if (newQty <= 0) {
      window.removeCartItem(itemId);
      return;
    }

    items[idx].qty = newQty;
    items[idx].soLuong = newQty;
    saveCart(items);
    renderCart();
  };

  // Xóa sản phẩm khỏi giỏ hàng kèm Undo Toast 5 giây (thay vì popup)
  window.removeCartItem = function (itemId) {
    let items = getCart();
    const idx = items.findIndex((it, i) => String(it.id || `${it.maHang}_${i}`) === String(itemId));
    if (idx === -1) return;

    const removedItem = items[idx];
    items.splice(idx, 1);
    selectedItemIds.delete(String(itemId));
    saveCart(items);
    renderCart();

    triggerUndoToast({ item: removedItem, index: idx }, `Đã xóa "${removedItem.tenHang || removedItem.name}" khỏi giỏ hàng`);
  };

  // Xóa các sản phẩm đã chọn
  window.removeSelectedCartItems = function () {
    if (selectedItemIds.size === 0) {
      showToast('Vui lòng tích chọn sản phẩm cần xóa!');
      return;
    }

    let items = getCart();
    const removedItems = [];
    const remaining = [];

    items.forEach((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      if (selectedItemIds.has(id)) {
        removedItems.push(it);
      } else {
        remaining.push(it);
      }
    });

    selectedItemIds.clear();
    saveCart(remaining);
    renderCart();

    triggerUndoToast(removedItems, `Đã xóa ${removedItems.length} sản phẩm khỏi giỏ hàng`);
  };

  // Xóa toàn bộ giỏ hàng
  window.clearAllCart = function () {
    const items = getCart();
    if (items.length === 0) return;

    selectedItemIds.clear();
    saveCart([]);
    renderCart();

    triggerUndoToast(items, 'Đã xóa toàn bộ sản phẩm khỏi giỏ hàng');
  };

  // Toast Hoàn tác khi xóa (5 giây)
  function triggerUndoToast(data, message) {
    lastDeletedData = data;
    const toast = document.getElementById('cart-undo-toast');
    const msgEl = document.getElementById('cart-undo-toast-message');

    if (toast) {
      if (msgEl) msgEl.textContent = message || 'Đã xóa sản phẩm khỏi giỏ hàng';
      toast.classList.remove('hidden');
      clearTimeout(undoToastTimeout);
      undoToastTimeout = setTimeout(() => {
        toast.classList.add('hidden');
        lastDeletedData = null;
      }, 5000);
    }
  }

  // Khôi phục khi người dùng nhấn "Hoàn tác"
  window.undoDeleteCartItem = function () {
    if (!lastDeletedData) return;
    let items = getCart();

    if (Array.isArray(lastDeletedData)) {
      items = items.concat(lastDeletedData);
      lastDeletedData.forEach((it, i) => {
        selectedItemIds.add(String(it.id || `${it.maHang}_${i}`));
      });
    } else if (lastDeletedData.item) {
      const insertIdx = Math.min(items.length, Math.max(0, lastDeletedData.index || 0));
      items.splice(insertIdx, 0, lastDeletedData.item);
      selectedItemIds.add(String(lastDeletedData.item.id || lastDeletedData.item.maHang));
    }

    saveCart(items);
    lastDeletedData = null;
    clearTimeout(undoToastTimeout);

    const toast = document.getElementById('cart-undo-toast');
    if (toast) toast.classList.add('hidden');

    renderCart();
    showToast('Đã khôi phục sản phẩm vào giỏ hàng thành công!');
  };

  // Tính toán tổng tiền
  function calculateTotals() {
    const items = getCart();
    const selectedItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return selectedItemIds.size === 0 || selectedItemIds.has(id);
    });

    const subtotal = selectedItems.reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);

    let memberDiscount = 0;
    if (subtotal >= 1000000) {
      memberDiscount = Math.round(subtotal * 0.01);
    }

    let pointsDiscount = 0;
    if (useMemberPoints && subtotal > 0) {
      pointsDiscount = Math.min(200000, subtotal);
    }

    let voucherDiscount = 0;
    if (appliedVoucher && subtotal > 0) {
      if (appliedVoucher.type === 'percent') {
        voucherDiscount = Math.round((subtotal * appliedVoucher.discount) / 100);
      } else {
        voucherDiscount = appliedVoucher.discount;
      }
    }

    const finalTotal = Math.max(0, subtotal - memberDiscount - pointsDiscount - voucherDiscount) + (currentShippingFee || 0);
    updateTotals(subtotal, memberDiscount, voucherDiscount, finalTotal, pointsDiscount);
    if (typeof debounceShippingQuote === 'function') debounceShippingQuote();
  }

  function updateTotals(subtotal, memberDiscount, voucherDiscount, finalTotal, pointsDiscount) {
    if (pointsDiscount === undefined) pointsDiscount = useMemberPoints ? Math.min(200000, subtotal) : 0;
    if (finalTotal === undefined) finalTotal = Math.max(0, subtotal - memberDiscount - pointsDiscount - voucherDiscount) + (currentShippingFee || 0);

    const elSubtotal = document.getElementById('summary-subtotal');
    const elMember = document.getElementById('summary-member-discount');
    const elPointsRow = document.getElementById('summary-points-row');
    const elPointsDiscount = document.getElementById('summary-points-discount');
    const elVoucherRow = document.getElementById('summary-voucher-row');
    const elVoucherDiscount = document.getElementById('summary-voucher-discount');
    const elFinal = document.getElementById('summary-final-total');
    const elMobileFinal = document.getElementById('mobile-summary-final-total');

    if (elSubtotal) elSubtotal.textContent = formatVND(subtotal);
    if (elMember) elMember.textContent = '-' + formatVND(memberDiscount);

    if (elPointsRow && elPointsDiscount) {
      if (pointsDiscount > 0) {
        elPointsRow.classList.remove('hidden');
        elPointsDiscount.textContent = '-' + formatVND(pointsDiscount);
      } else {
        elPointsRow.classList.add('hidden');
      }
    }

    if (elVoucherRow && elVoucherDiscount) {
      if (voucherDiscount > 0) {
        elVoucherRow.classList.remove('hidden');
        elVoucherDiscount.textContent = '-' + formatVND(voucherDiscount);
      } else {
        elVoucherRow.classList.add('hidden');
      }
    }

    if (elFinal) elFinal.textContent = formatVND(finalTotal);
    if (elMobileFinal) elMobileFinal.textContent = formatVND(finalTotal);

    updateFreeshipProgress(subtotal);
    updateVietQR(finalTotal);
    updateEWalletQR(finalTotal);
  }

  // Cập nhật thanh tiến trình Freeship thông minh theo khu vực (TP.HCM 5TR, Toàn quốc 20TR)
  function updateFreeshipProgress(subtotal) {
    const container = document.getElementById('freeship-progress-bar-container');
    const msgText = document.getElementById('freeship-message-text');
    const fill = document.getElementById('freeship-progress-fill');
    const badge = document.getElementById('freeship-badge');
    if (!container || !msgText || !fill) return;

    const elProvince = document.getElementById('order-province');
    const provinceId = elProvince ? Number(elProvince.value || 0) : 202;
    const isHcm = provinceId === 202;
    const threshold = isHcm ? 5000000 : 20000000;
    const areaName = isHcm ? 'nội thành TP.HCM' : 'toàn quốc';

    if (subtotal >= threshold) {
      msgText.innerHTML = `
        <span class="w-6 h-6 rounded-lg bg-emerald-100 text-emerald-600 flex items-center justify-center text-xs">
          <i class="fa-solid fa-circle-check"></i>
        </span>
        <span>Chúc mừng! Bạn đã đủ điều kiện <strong class="text-emerald-600 font-bold">Miễn phí vận chuyển (${areaName})</strong>!</span>
      `;
      fill.style.width = '100%';
      fill.className = 'h-full bg-gradient-to-r from-emerald-500 to-teal-500 rounded-full transition-all duration-300';
      if (badge) {
        badge.textContent = 'Đạt Freeship';
        badge.className = 'text-[10px] font-bold px-2.5 py-0.5 rounded-full bg-emerald-100 text-emerald-700';
      }
    } else {
      const needed = threshold - subtotal;
      const pct = Math.max(8, Math.min(100, Math.round((subtotal / threshold) * 100)));
      msgText.innerHTML = `
        <span class="w-6 h-6 rounded-lg bg-blue-100 text-[#0057B7] flex items-center justify-center text-xs">
          <i class="fa-solid fa-truck-fast"></i>
        </span>
        <span>Mua thêm <strong id="freeship-amount-needed" class="text-rose-600 font-bold">${formatVND(needed)}</strong> để được <strong>Miễn phí vận chuyển (${areaName})</strong>!</span>
      `;
      fill.style.width = pct + '%';
      fill.className = 'h-full bg-gradient-to-r from-blue-500 to-[#0057B7] rounded-full transition-all duration-300';
      if (badge) {
        badge.textContent = isHcm ? 'Freeship 5TR' : 'Freeship 20TR';
        badge.className = 'text-[10px] font-bold px-2.5 py-0.5 rounded-full bg-blue-100 text-[#0057B7]';
      }
    }
  }

  // Voucher
  window.quickApplyVoucher = function (code) {
    const input = document.getElementById('voucher-input');
    if (input) input.value = code;
    window.applyVoucherCode();
  };

  window.applyVoucherCode = function () {
    const input = document.getElementById('voucher-input');
    if (!input) return;

    const code = input.value.trim().toUpperCase();
    if (!code) {
      showVoucherMsg('Vui lòng nhập mã voucher', 'error');
      return;
    }

    const voucher = VOUCHERS[code];
    if (!voucher) {
      showVoucherMsg(`Mã "${code}" không hợp lệ hoặc đã hết hạn`, 'error');
      appliedVoucher = null;
      calculateTotals();
      return;
    }

    const subtotal = getCart().reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);
    if (subtotal < voucher.minOrder) {
      showVoucherMsg(`Mã ${code} chỉ áp dụng cho đơn từ ${formatVND(voucher.minOrder)}`, 'error');
      appliedVoucher = null;
      calculateTotals();
      return;
    }

    appliedVoucher = voucher;
    showVoucherMsg(`Áp dụng thành công: ${voucher.desc}`, 'success');
    calculateTotals();
  };

  function showVoucherMsg(msg, type) {
    const el = document.getElementById('voucher-message');
    if (!el) return;
    el.className = `p-2.5 rounded-xl text-xs font-bold ${type === 'success' ? 'bg-emerald-50 text-emerald-700 border border-emerald-200' : 'bg-rose-50 text-rose-700 border border-rose-200'}`;
    el.textContent = msg;
    el.classList.remove('hidden');
  }

  window.setDeliveryMethod = function (method) {
    deliveryMethod = method;
    const btnDelivery = document.getElementById('btn-method-delivery');
    const btnStore = document.getElementById('btn-method-store');
    const addressBox = document.getElementById('delivery-address-box');
    const storeBox = document.getElementById('store-pickup-box');

    if (method === 'delivery') {
      if (btnDelivery) btnDelivery.className = 'py-3 px-4 rounded-xl font-semibold text-xs sm:text-sm bg-white text-[#0057B7] border-2 border-[#0057B7] shadow-sm flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (btnStore) btnStore.className = 'py-3 px-4 rounded-xl font-medium text-xs sm:text-sm bg-slate-50 text-slate-600 border border-slate-200 hover:bg-slate-100 flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (addressBox) addressBox.classList.remove('hidden');
      if (storeBox) storeBox.classList.add('hidden');
    } else {
      if (btnDelivery) btnDelivery.className = 'py-3 px-4 rounded-xl font-medium text-xs sm:text-sm bg-slate-50 text-slate-600 border border-slate-200 hover:bg-slate-100 flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (btnStore) btnStore.className = 'py-3 px-4 rounded-xl font-semibold text-xs sm:text-sm bg-white text-[#0057B7] border-2 border-[#0057B7] shadow-sm flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (addressBox) addressBox.classList.add('hidden');
      if (storeBox) storeBox.classList.remove('hidden');
    }
    if (typeof updateShippingQuote === 'function') updateShippingQuote();
  };

  // Cập nhật VietQR Code động
  function updateVietQR(amount) {
    const cfg = getActiveBankConfig();
    const qrImg = document.getElementById('vietqr-code-img');
    const qrAmount = document.getElementById('vietqr-amount-text');
    const bankName = document.getElementById('vietqr-bank-name');
    const accountNo = document.getElementById('vietqr-account-no');
    const accountNameEl = document.getElementById('vietqr-account-name');
    const memoEl = document.getElementById('vietqr-memo-text');

    if (qrAmount) qrAmount.textContent = formatVND(amount);
    if (bankName) bankName.textContent = cfg.name;
    if (accountNo) accountNo.textContent = cfg.accountNo;
    if (accountNameEl) accountNameEl.textContent = cfg.accountName || 'CONG TY TNHH SAMA MOBILE';

    const memo = 'SAMA 261106';
    if (memoEl) memoEl.textContent = memo;

    if (qrImg) {
      const amt = Math.max(1000, Math.round(amount));
      qrImg.src = getQrCodeUrl(cfg, amt, memo);
    }
  }

  window.selectVietQrBank = function (bankCode) {
    selectedBank = bankCode;
    const buttons = document.querySelectorAll('.vietqr-bank-chip');
    buttons.forEach(btn => {
      btn.className = 'vietqr-bank-chip p-2 rounded-xl border border-slate-200 bg-white hover:border-slate-300 font-medium text-slate-700 flex items-center justify-center gap-2 cursor-pointer transition-all';
    });
    const targetBtn = document.getElementById(`bank-btn-${bankCode}`);
    if (targetBtn) {
      targetBtn.className = 'vietqr-bank-chip p-2 rounded-xl border-2 border-blue-600 bg-blue-50/60 font-bold text-blue-900 flex items-center justify-center gap-2 cursor-pointer transition-all';
    }
    const finalTotal = getCartFinalTotal();
    updateVietQR(finalTotal);
  };

  // Cập nhật QR Ví Điện Tử
  function updateEWalletQR(amount) {
    const qrImg = document.getElementById('ewallet-qr-img');
    const amtEl = document.getElementById('ewallet-amount-text');
    const labelEl = document.getElementById('ewallet-name-label');
    const appNameEl = document.getElementById('ewallet-app-name');

    if (amtEl) amtEl.textContent = formatVND(amount);

    const amt = Math.max(10000, Math.round(amount));
    const memo = 'SAMA WALLET 261106';

    if (qrImg) {
      const cfg = getActiveBankConfig();
      qrImg.src = getQrCodeUrl(cfg, amt, memo);
    }

    if (labelEl) {
      labelEl.textContent = selectedWallet === 'vnpay' ? 'Cổng VNPAY-QR Doanh Nghiệp' : (selectedWallet === 'zalopay' ? 'Ví ZaloPay Doanh Nghiệp' : 'Ví MoMo Doanh Nghiệp');
    }
    if (appNameEl) {
      appNameEl.textContent = selectedWallet === 'vnpay' ? 'VNPAY / App Ngân Hàng' : (selectedWallet === 'zalopay' ? 'ZaloPay' : 'MoMo');
    }
  }

  window.selectEWallet = function (walletCode) {
    selectedWallet = walletCode;
    const buttons = document.querySelectorAll('.ewallet-tab-btn');
    buttons.forEach(btn => {
      btn.className = 'ewallet-tab-btn p-2 rounded-xl border border-slate-200 bg-white hover:border-slate-300 font-medium text-slate-700 flex items-center justify-center gap-2 cursor-pointer transition-all';
    });
    const target = document.getElementById(`ewallet-btn-${walletCode}`);
    if (target) {
      const color = walletCode === 'momo' ? 'pink' : (walletCode === 'vnpay' ? 'blue' : 'cyan');
      target.className = `ewallet-tab-btn p-2 rounded-xl border-2 border-${color}-600 bg-${color}-50/60 font-bold text-${color}-900 flex items-center justify-center gap-2 cursor-pointer transition-all`;
    }
    const finalTotal = getCartFinalTotal();
    updateEWalletQR(finalTotal);
  };

  // Sao chép văn bản & thông báo toast
  window.copyText = function (text, label) {
    if (!text) return;
    const clean = String(text).trim();
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(clean).then(() => {
        showToast(`Đã sao chép ${label || clean}!`);
      }).catch(() => {
        fallbackCopy(clean, label);
      });
    } else {
      fallbackCopy(clean, label);
    }
  };

  function fallbackCopy(clean, label) {
    const el = document.createElement('textarea');
    el.value = clean;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
    showToast(`Đã sao chép ${label || clean}!`);
  }

  function showToast(msg) {
    let toast = document.getElementById('sama-toast-notification');
    if (!toast) {
      toast = document.createElement('div');
      toast.id = 'sama-toast-notification';
      toast.className = 'fixed bottom-6 right-6 z-50 bg-slate-900 text-white px-4 py-3 rounded-2xl shadow-2xl flex items-center gap-2.5 text-xs font-semibold animate-in fade-in slide-in-from-bottom-5 duration-200';
      document.body.appendChild(toast);
    }
    toast.innerHTML = `<i class="fa-solid fa-circle-check text-emerald-400 text-sm"></i><span>${msg}</span>`;
    toast.classList.remove('hidden');
    clearTimeout(toast._timer);
    toast._timer = setTimeout(() => {
      toast.classList.add('hidden');
    }, 2400);
  }

  // Submit Đặt Hàng
  window.submitCheckoutOrder = async function (e) {
    if (e) e.preventDefault();

    const items = getCart();
    if (!items || items.length === 0) {
      alert('Giỏ hàng đang trống, vui lòng chọn sản phẩm!');
      return;
    }

    const selectedItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return selectedItemIds.size === 0 || selectedItemIds.has(id);
    });

    if (selectedItems.length === 0) {
      alert('Vui lòng tích chọn ít nhất 1 sản phẩm để thanh toán!');
      return;
    }

    const name = (document.getElementById('order-fullname')?.value || '').trim();
    const phone = (document.getElementById('order-phone')?.value || '').trim();
    const email = (document.getElementById('order-email')?.value || '').trim();
    const note = (document.getElementById('order-note')?.value || '').trim();
    const gender = document.querySelector('input[name="order-gender"]:checked')?.value || 'Anh';

    if (!name) {
      alert('Vui lòng nhập họ và tên người nhận hàng');
      document.getElementById('order-fullname')?.focus();
      return;
    }

    const phoneRegex = /^(0)(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$/;
    if (!phoneRegex.test(phone.replace(/\s/g, ''))) {
      alert('Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 chữ số, ví dụ 0901234567)');
      document.getElementById('order-phone')?.focus();
      return;
    }

    let fullAddress = 'Nhận tại cửa hàng SamaMobile (241 Lê Văn Việt, TP. Thủ Đức)';
    if (deliveryMethod === 'delivery') {
      const province = document.getElementById('order-province')?.value || 'TP. Hồ Chí Minh';
      const district = document.getElementById('order-district')?.value || '';
      const ward = document.getElementById('order-ward')?.value || '';
      const street = (document.getElementById('order-address')?.value || '').trim();

      if (!street) {
        alert('Vui lòng nhập địa chỉ nhận hàng cụ thể (Số nhà, tên đường)');
        document.getElementById('order-address')?.focus();
        return;
      }
      const elP = document.getElementById('order-province');
      const elD = document.getElementById('order-district');
      const elW = document.getElementById('order-ward');
      const provinceName = elP ? (elP.options[elP.selectedIndex]?.getAttribute('data-name') || elP.options[elP.selectedIndex]?.text || '') : '';
      const districtName = elD ? (elD.options[elD.selectedIndex]?.getAttribute('data-name') || elD.options[elD.selectedIndex]?.text || '') : '';
      const wardName = elW ? (elW.options[elW.selectedIndex]?.getAttribute('data-name') || elW.options[elW.selectedIndex]?.text || '') : '';
      const toDistrictId = elD ? Number(elD.value || 0) : 0;
      const toWardCode = elW ? (elW.value || '') : '';
      const ghnTag = (toDistrictId > 0 && toWardCode) ? `[GHN:${toDistrictId}:${toWardCode}] ` : '';
      fullAddress = `${ghnTag}${street}, ${wardName ? wardName + ', ' : ''}${districtName ? districtName + ', ' : ''}${provinceName}`;
    }

    const subtotal = selectedItems.reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);
    let discount = 0;
    if (subtotal >= 1000000) discount += Math.round(subtotal * 0.01);
    if (useMemberPoints && subtotal > 0) discount += Math.min(200000, subtotal);
    if (appliedVoucher) {
      discount += appliedVoucher.type === 'percent' ? Math.round((subtotal * appliedVoucher.discount) / 100) : appliedVoucher.discount;
    }
    const finalTotal = Math.max(0, subtotal - discount) + (currentShippingFee || 0);

    const randomCode = Math.floor(1000 + Math.random() * 9000);
    // Giai đoạn 1: Sinh mã đơn hàng ngẫu nhiên dạng DH-XXXXXX (loại bỏ 0,O,1,I,L)
    const ORDER_CODE_CHARSET = '23456789ABCDEFGHJKMNPQRSTUVWXYZ';
    const arr = new Uint8Array(6);
    crypto.getRandomValues(arr);
    let maDH = 'DH-';
    for (let i = 0; i < 6; i++) {
      maDH += ORDER_CODE_CHARSET[arr[i] % ORDER_CODE_CHARSET.length];
    }

    const orderData = {
      maDH: maDH,
      ngayDat: new Date().toISOString(),
      gender: gender,
      tenNguoiNhan: `${gender} ${name}`,
      sdtNguoiNhan: phone,
      email: email,
      diaChiGiao: fullAddress,
      phuongThucThanhToan: paymentMethod,
      ghiChu: note,
      subtotal: subtotal,
      discount: discount,
      tongTien: finalTotal,
      trangThai: (paymentMethod === 'banking' || paymentMethod === 'momo' || paymentMethod === 'card') ? 'cho_thanh_toan' : 'cho_xac_nhan',
      daThanhToan: false,
      items: selectedItems.map(it => ({
        maHang: it.maHang || it.id,
        tenHang: it.tenHang || it.name,
        soLuong: Number(it.qty || it.soLuong || 1),
        donGia: Number(it.price || it.donGia),
        thanhTien: (Number(it.price || it.donGia)) * (Number(it.qty || it.soLuong || 1)),
        image: it.image || it.hinhAnh || it.anhChinh
      }))
    };

    const btnSubmit = document.getElementById('btn-submit-order');
    if (btnSubmit) {
      btnSubmit.disabled = true;
      btnSubmit.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-2"></i> Đang tạo đơn hàng...';
    }

    // Lưu đơn hàng
    try {
      const orders = getOrders();
      orders.unshift(orderData);
      saveOrders(orders);
    } catch (e) {}

    // Gửi realtime qua SignalR & BroadcastChannel tới Admin ngay lập tức
    try {
      if (window.OrderSignalR && typeof window.OrderSignalR.notifyNewOrder === 'function') {
        window.OrderSignalR.notifyNewOrder(orderData);
        window.OrderSignalR.joinOrderGroup(orderData.maDH);
        if (orderData.sdtNguoiNhan) window.OrderSignalR.joinCustomerGroup(orderData.sdtNguoiNhan);
      }
    } catch (e) {}


    // Gửi lên REST API backend (đồng bộ MaDH để Admin và DB khớp 100%)
    try {
      if (window.api && typeof window.api.post === 'function') {
        const postPromise = window.api.post('/orders', {
          maDH: orderData.maDH,
          tenNguoiNhan: orderData.tenNguoiNhan,
          sdtNguoiNhan: orderData.sdtNguoiNhan,
          email: orderData.email,
          diaChiGiao: orderData.diaChiGiao,
          phuongThucThanhToan: orderData.phuongThucThanhToan,
          ghiChu: orderData.ghiChu,
          items: orderData.items.map(it => ({
            maHang: String(it.maHang || 'MH001').substring(0, 10),
            tenHang: it.tenHang,
            soLuong: it.soLuong,
            donGia: it.donGia
          }))
        });
        await Promise.race([
          postPromise,
          new Promise(resolve => setTimeout(resolve, 2500))
        ]).catch(() => {});
      }
    } catch (e) {}

    // Giữ lại các mặt hàng chưa chọn (nếu có), xóa các mặt hàng đã đặt
    const remainingItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return !selectedItemIds.has(id);
    });
    selectedItemIds.clear();
    saveCart(remainingItems);

    // Hiển thị màn hình thành công
    showSuccessScreen(orderData);
  };

  let qrPollingInterval = null;

  function stopQrFlow() {
    if (qrCountdownInterval) {
      clearInterval(qrCountdownInterval);
      qrCountdownInterval = null;
    }
    if (qrPollingInterval) {
      clearInterval(qrPollingInterval);
      qrPollingInterval = null;
    }
  }

  function handlePaymentSuccessUI(maDH, totalAmount) {
    stopQrFlow();

    // 1. Cập nhật LocalStorage: đã thanh toán nhưng trạng thái là "cho_xac_nhan" chờ Admin duyệt!
    try {
      const orders = getOrders();
      const idx = orders.findIndex(o => o.maDH === maDH);
      if (idx > -1) {
        orders[idx].daThanhToan = true;
        orders[idx].trangThai = 'cho_xac_nhan';
        saveOrders(orders);
      }
    } catch (e) {}

    // 2. Cập nhật Icon, Tiêu đề & Badge
    const iconEl = document.getElementById('success-screen-icon');
    const titleEl = document.getElementById('success-screen-title');
    const subEl = document.getElementById('success-screen-subtitle');
    const badgeContainer = document.getElementById('success-screen-badge-container');

    if (iconEl) {
      iconEl.className = 'w-16 h-16 sm:w-20 sm:h-20 mx-auto rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center text-2xl sm:text-3xl shadow-inner animate-bounce';
      iconEl.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
    }
    if (titleEl) titleEl.textContent = 'Đã Nhận Thanh Toán Chuyển Khoản!';
    if (subEl) subEl.textContent = 'SamaMobile đã nhận được tiền thanh toán. Đơn hàng đang ở trạng thái Chờ xác nhận từ Admin để đóng gói xuất kho.';
    if (badgeContainer) {
      badgeContainer.innerHTML = `
        <div class="flex flex-wrap items-center justify-center gap-2">
          <span class="px-3 py-1 rounded-full bg-amber-50 text-amber-800 border border-amber-300 text-xs font-bold inline-flex items-center gap-1.5 shadow-2xs">
            <i class="fa-regular fa-clock text-amber-600"></i> Chờ Admin xác nhận
          </span>
          <span class="px-3 py-1 rounded-full bg-emerald-50 text-emerald-700 border border-emerald-300 text-xs font-bold inline-flex items-center gap-1.5 shadow-2xs">
            <i class="fa-solid fa-check text-emerald-600"></i> Đã thanh toán VietQR
          </span>
        </div>
      `;
    }

    // 3. Ẩn khối QR & Hiện khối ăn mừng
    const vietQrBox = document.getElementById('success-vietqr-box');
    const celebBox = document.getElementById('success-paid-celebration-box');
    if (vietQrBox) vietQrBox.classList.add('hidden');
    if (celebBox) {
      celebBox.classList.remove('hidden');
      const celebTitle = celebBox.querySelector('h3');
      const celebDesc = celebBox.querySelector('p');
      if (celebTitle) celebTitle.textContent = 'Giao Dịch Thanh Toán Thành Công!';
      if (celebDesc) celebDesc.textContent = 'Hệ thống đã tự động ghi nhận chuyển khoản. Đơn hàng của bạn đang được chuyển đến Admin để duyệt đơn và xuất kho.';
    }

    // 4. Phát âm thanh Ting ting & Toast
    if (window.OrderSignalR && typeof window.OrderSignalR.playChime === 'function') {
      window.OrderSignalR.playChime('payment_success');
    }
    if (typeof showToast === 'function') {
      showToast('Thanh toán đơn hàng #' + maDH + ' thành công!', 'success');
    }
  }

  function startPaymentPolling(maDH) {
    if (qrPollingInterval) clearInterval(qrPollingInterval);

    qrPollingInterval = setInterval(async () => {
      try {
        const res = await fetch(`http://localhost:5132/api/payment/status/${encodeURIComponent(maDH)}`);
        if (res.ok) {
          const data = await res.json();
          if (data && (data.isPaid || data.daThanhToan || data.trangThai === 'da_xac_nhan' || data.trangThai === 'dang_giao' || data.trangThai === 'hoan_thanh')) {
            handlePaymentSuccessUI(maDH, data.tongTien);
          }
        }
      } catch (e) {}
    }, 2500);
  }

  function showSuccessScreen(order) {
    const mainScreen = document.getElementById('cart-main-wrapper');
    const successScreen = document.getElementById('order-success-screen');
    const stepperContainer = document.getElementById('cart-stepper-container');
    const mobileBottomBar = document.getElementById('mobile-cart-bottom-bar');

    if (stepperContainer) stepperContainer.classList.add('hidden');
    if (mobileBottomBar) mobileBottomBar.classList.add('hidden');
    if (mainScreen) mainScreen.classList.add('hidden');
    if (successScreen) {
      successScreen.classList.remove('hidden');
      window.scrollTo({ top: 0, behavior: 'smooth' });

      const elCode = document.getElementById('success-order-code');
      const elName = document.getElementById('success-customer-name');
      const elPhone = document.getElementById('success-customer-phone');
      const elAddress = document.getElementById('success-customer-address');
      const elPayment = document.getElementById('success-payment-method');
      const elTotal = document.getElementById('success-final-total');
      const elItemsList = document.getElementById('success-items-list');

      const iconEl = document.getElementById('success-screen-icon');
      const titleEl = document.getElementById('success-screen-title');
      const subEl = document.getElementById('success-screen-subtitle');
      const badgeContainer = document.getElementById('success-screen-badge-container');
      const celebBox = document.getElementById('success-paid-celebration-box');
      if (celebBox) celebBox.classList.add('hidden');

      if (elCode) elCode.textContent = order.maDH;
      try { localStorage.setItem('cellphones_last_order_code', order.maDH); } catch (e) {}
      if (elName) elName.textContent = order.tenNguoiNhan;
      if (elPhone) elPhone.textContent = order.sdtNguoiNhan;
      if (elAddress) elAddress.textContent = order.diaChiGiao;

      const paymentMap = {
        'cod': 'Thanh toán tiền mặt khi nhận hàng (COD)',
        'banking': 'Chuyển khoản Ngân hàng (VietQR 24/7)',
        'momo': 'Ví điện tử MoMo / VNPAY-QR',
        'card': 'Thẻ Quốc tế / Trả góp 0%'
      };
      if (elPayment) elPayment.textContent = paymentMap[order.phuongThucThanhToan] || 'Chuyển khoản';
      if (elTotal) elTotal.textContent = formatVND(order.tongTien);

      if (elItemsList && order.items) {
        elItemsList.innerHTML = order.items.map(it => `
          <div class="flex items-center justify-between text-xs py-1">
            <span class="text-slate-700">${it.tenHang} <strong class="text-slate-900 font-bold">x${it.soLuong}</strong></span>
            <span class="font-bold text-slate-900">${formatVND(it.thanhTien || (it.donGia * it.soLuong))}</span>
          </div>
        `).join('');
      }

      const vietQrBox = document.getElementById('success-vietqr-box');
      const isOnlinePayment = order.phuongThucThanhToan === 'banking' || order.phuongThucThanhToan === 'momo' || order.phuongThucThanhToan === 'card';

      if (isOnlinePayment) {
        // Giao diện Chờ Quét Mã Thanh Toán (Phương án 1)
        if (iconEl) {
          iconEl.className = 'w-20 h-20 mx-auto rounded-full bg-amber-100 text-amber-600 flex items-center justify-center text-3xl shadow-inner animate-pulse';
          iconEl.innerHTML = '<i class="fa-solid fa-qrcode"></i>';
        }
        if (titleEl) titleEl.textContent = 'Đơn Hàng Đã Tạo - Vui Lòng Quét Mã';
        if (subEl) subEl.textContent = 'Quét mã QR bên dưới để hoàn tất thanh toán. Đơn hàng sẽ tự động chuyển sang Chờ xác nhận từ Admin ngay khi tiền về!';
        if (badgeContainer) {
          badgeContainer.innerHTML = '<span class="px-3.5 py-1.5 rounded-full bg-amber-50 text-amber-800 border border-amber-300 text-xs font-bold inline-flex items-center gap-1.5 shadow-2xs"><i class="fa-solid fa-spinner fa-spin text-amber-600"></i> Chờ quét mã thanh toán online</span>';
        }

        if (vietQrBox) {
          vietQrBox.classList.remove('hidden');
          const qrImg = document.getElementById('success-vietqr-img');
          const memoText = document.getElementById('success-transfer-content');
          const qrAmount = document.getElementById('success-qr-amount-text');
          const qrBank = document.getElementById('success-qr-bank-name');
          const qrAccName = document.getElementById('success-qr-account-name');
          const qrAccNo = document.getElementById('success-qr-account-no');
          const memo = order.maDH;

          const cfg = getActiveBankConfig();
          if (memoText) memoText.textContent = memo;
          if (qrAmount) qrAmount.textContent = formatVND(order.tongTien);
          if (qrBank) qrBank.textContent = cfg.name;
          if (qrAccName) qrAccName.textContent = cfg.accountName || 'CONG TY TNHH SAMA MOBILE';
          if (qrAccNo) qrAccNo.textContent = cfg.accountNo;
          if (qrImg) {
            qrImg.src = getQrCodeUrl(cfg, order.tongTien, memo);
          }

          startQrCountdown(order.maDH);
          startPaymentPolling(order.maDH);
        }

        // Lắng nghe Realtime qua SignalR
        const handleStatusEvent = (e) => {
          if (e.detail && e.detail.maDH === order.maDH) {
            if (e.detail.isPaid || e.detail.trangThai === 'da_xac_nhan' || e.detail.trangThai === 'dang_giao' || e.detail.trangThai === 'hoan_thanh') {
              handlePaymentSuccessUI(order.maDH, order.tongTien);
            }
          }
        };
        window.addEventListener('samamobile:payment_success', handleStatusEvent);
        window.addEventListener('samamobile:order_status_updated', handleStatusEvent);

      } else {
        // Giao diện COD (Phương án 1)
        if (iconEl) {
          iconEl.className = 'w-20 h-20 mx-auto rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center text-3xl shadow-inner';
          iconEl.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
        }
        if (titleEl) titleEl.textContent = 'Đặt Hàng Thành Công!';
        if (subEl) subEl.textContent = 'Cảm ơn bạn đã tin tưởng mua sắm tại SamaMobile Store';
        if (badgeContainer) {
          badgeContainer.innerHTML = '<span class="px-3.5 py-1.5 rounded-full bg-blue-50 text-blue-700 border border-blue-200 text-xs font-bold inline-flex items-center gap-1.5 shadow-2xs"><i class="fa-solid fa-clock"></i> Chờ nhân viên xác nhận đơn (COD)</span>';
        }

        if (vietQrBox) {
          vietQrBox.classList.add('hidden');
        }
        stopQrFlow();
      }
    }
  }

  function startQrCountdown(maDH) {
    if (qrCountdownInterval) clearInterval(qrCountdownInterval);
    let secondsLeft = 15 * 60;
    const timerEl = document.getElementById('success-qr-timer');
    const statusTextEl = document.getElementById('qr-realtime-status-text');

    qrCountdownInterval = setInterval(() => {
      secondsLeft--;
      if (secondsLeft <= 0) {
        stopQrFlow();
        if (timerEl) timerEl.textContent = '00:00 (Hết hạn)';
        if (statusTextEl) {
          statusTextEl.textContent = 'Đơn hàng đã hết hạn giữ chỗ thanh toán.';
          statusTextEl.parentElement?.classList.replace('bg-amber-50/90', 'bg-rose-50');
          statusTextEl.parentElement?.classList.replace('text-amber-800', 'text-rose-700');
          statusTextEl.parentElement?.classList.replace('border-amber-200', 'border-rose-200');
        }
        const badgeContainer = document.getElementById('success-screen-badge-container');
        if (badgeContainer) {
          badgeContainer.innerHTML = '<span class="px-3.5 py-1.5 rounded-full bg-rose-50 text-rose-700 border border-rose-300 text-xs font-bold inline-flex items-center gap-1.5 shadow-2xs"><i class="fa-solid fa-ban text-rose-600"></i> Đã hết hạn thanh toán</span>';
        }
        // Gọi API hủy đơn quá hạn
        if (maDH && window.api && typeof window.api.post === 'function') {
          window.api.post(`/donhangonline/${encodeURIComponent(maDH)}/trangthai`, { trangThai: 'da_huy' }).catch(() => {});
        }
        return;
      }
      const mins = Math.floor(secondsLeft / 60);
      const secs = secondsLeft % 60;
      if (timerEl) {
        timerEl.textContent = `${String(mins).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
      }
    }, 1000);
  }

  window.confirmQrTransferSuccess = async function () {
    const elCode = document.getElementById('success-order-code');
    const orderCode = elCode ? elCode.textContent.trim() : '';
    if (!orderCode) return;

    const btn = document.getElementById('btn-manual-confirm-qr');
    if (btn) {
      btn.disabled = true;
      btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1"></i> Đang kiểm tra...';
    }

    try {
      const res = await fetch(`http://localhost:5132/api/payment/confirm/${encodeURIComponent(orderCode)}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          transactionId: `CLIENT_MANUAL_${Date.now()}`,
          note: 'Khách hàng bấm kiểm tra thanh toán ngay trên web'
        })
      });

      if (res.ok) {
        handlePaymentSuccessUI(orderCode);
      } else {
        handlePaymentSuccessUI(orderCode);
      }
    } catch (e) {
      handlePaymentSuccessUI(orderCode);
    }
  };

  window.cancelPendingOrder = async function () {
    const elCode = document.getElementById('success-order-code');
    const orderCode = elCode ? elCode.textContent.trim() : '';
    if (!orderCode) return;

    if (!confirm(`Bạn chắc chắn muốn hủy đơn hàng #${orderCode}?`)) return;

    stopQrFlow();

    try {
      if (window.api && typeof window.api.post === 'function') {
        await window.api.post(`/donhangonline/${encodeURIComponent(orderCode)}/trangthai`, {
          trangThai: 'da_huy'
        }).catch(() => {});
      }
    } catch (e) {}

    try {
      const orders = getOrders();
      const idx = orders.findIndex(o => o.maDH === orderCode);
      if (idx > -1) {
        orders[idx].trangThai = 'da_huy';
        saveOrders(orders);
      }
    } catch (e) {}

    if (typeof showToast === 'function') {
      showToast(`Đã hủy đơn hàng #${orderCode}`, 'info');
    }

    setTimeout(() => {
      window.switchCartTab('orders');
    }, 600);
  };

  window.downloadQrImage = function () {
    const qrImg = document.getElementById('success-vietqr-img');
    if (qrImg && qrImg.src) {
      window.open(qrImg.src, '_blank');
    }
  };

  // =========================================================================
  // LOGIC PHẦN 2: QUẢN LÝ ĐƠN HÀNG, THEO DÕI HÀNH TRÌNH & ĐƠN ĐÃ HỦY
  // =========================================================================

  window.renderOrders = function () {
    const orders = getOrders();
    updateOrderCounts();

    const container = document.getElementById('orders-list-container');
    const emptyState = document.getElementById('orders-empty-state');
    const searchVal = (document.getElementById('order-search-input')?.value || '').trim().toLowerCase();

    let filtered = orders;

    if (currentOrderStatusFilter !== 'all') {
      if (currentOrderStatusFilter === 'cho_xac_nhan') {
        filtered = filtered.filter(o => o.trangThai === 'cho_thanh_toan' || o.trangThai === 'cho_xac_nhan' || o.trangThai === 'da_xac_nhan');
      } else if (currentOrderStatusFilter === 'da_giao') {
        filtered = filtered.filter(o => o.trangThai === 'da_giao' || o.trangThai === 'hoan_thanh');
      } else {
        filtered = filtered.filter(o => o.trangThai === currentOrderStatusFilter);
      }
    }

    if (searchVal) {
      filtered = filtered.filter(o => 
        (o.maDH && o.maDH.toLowerCase().includes(searchVal)) ||
        (o.sdtNguoiNhan && o.sdtNguoiNhan.includes(searchVal)) ||
        (o.items && o.items.some(it => it.tenHang && it.tenHang.toLowerCase().includes(searchVal)))
      );
    }

    if (!container) return;

    if (filtered.length === 0) {
      container.innerHTML = '';
      if (emptyState) emptyState.classList.remove('hidden');
      return;
    }

    if (emptyState) emptyState.classList.add('hidden');

    const statusBadgeMap = {
      'cho_thanh_toan': { label: 'Chờ thanh toán', color: 'bg-amber-100 text-amber-900 border-amber-300' },
      'cho_xac_nhan': { label: 'Chờ xác nhận', color: 'bg-blue-100 text-blue-800 border-blue-200' },
      'da_xac_nhan': { label: 'Đã xác nhận', color: 'bg-sky-100 text-sky-800 border-sky-200' },
      'dang_giao': { label: 'Đang giao hàng', color: 'bg-purple-100 text-purple-800 border-purple-200' },
      'da_giao': { label: 'Giao thành công', color: 'bg-emerald-100 text-emerald-800 border-emerald-200' },
      'hoan_thanh': { label: 'Hoàn thành', color: 'bg-emerald-100 text-emerald-800 border-emerald-200' },
      'da_huy': { label: 'Đã hủy', color: 'bg-slate-100 text-slate-600 border-slate-200' }
    };

    container.innerHTML = filtered.map(order => {
      const badge = statusBadgeMap[order.trangThai] || statusBadgeMap['cho_xac_nhan'];
      const dateStr = new Date(order.ngayDat).toLocaleString('vi-VN', { hour: '2-digit', minute: '2-digit', day: '2-digit', month: '2-digit', year: 'numeric' });
      const canCancel = order.trangThai === 'cho_thanh_toan' || order.trangThai === 'cho_xac_nhan';
      const isQrPayment = (order.phuongThucThanhToan === 'banking' || order.phuongThucThanhToan === 'momo') && !order.daThanhToan && (order.trangThai === 'cho_thanh_toan' || order.trangThai === 'cho_xac_nhan');

      return `
        <div class="bg-white rounded-2xl p-4 sm:p-5 border border-slate-200/90 shadow-2xs space-y-3.5 hover:border-blue-300 transition-all text-xs sm:text-sm">
          <!-- Hàng tiêu đề đơn hàng -->
          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-2.5 pb-3 border-b border-slate-100">
            <div class="flex items-center gap-2.5">
              <span class="w-8 h-8 rounded-xl bg-blue-50 text-[#0057B7] flex items-center justify-center text-xs font-bold flex-shrink-0">
                <i class="fa-solid fa-receipt"></i>
              </span>
              <div>
                <div class="flex flex-wrap items-center gap-2">
                  <h4 class="font-bold text-sm sm:text-base text-slate-900 font-mono tracking-tight">${order.maDH}</h4>
                  <span class="px-2.5 py-0.5 rounded-full text-xs font-semibold border ${badge.color}">${badge.label}</span>
                  ${(order.trackingCode || order.shipping?.trackingCode) ? `
                    <a href="https://donhang.ghn.vn/?order_code=${encodeURIComponent(order.trackingCode || order.shipping?.trackingCode)}" target="_blank" rel="noopener noreferrer" class="px-2.5 py-0.5 rounded-full text-xs font-mono font-bold bg-blue-50 text-blue-700 border border-blue-200 hover:bg-blue-100 flex items-center gap-1" title="Theo dõi bưu phẩm trên GHN">
                      <i class="fa-solid fa-truck text-[10px]"></i> GHN: #${order.trackingCode || order.shipping?.trackingCode}
                    </a>
                  ` : ''}
                  ${order.daThanhToan ? '<span class="px-2 py-0.5 rounded-full text-xs font-semibold bg-emerald-50 text-emerald-700 border border-emerald-200 inline-flex items-center gap-1"><i class="fa-solid fa-check text-[10px]"></i> Đã thanh toán</span>' : ''}
                </div>
                </div>
                <div class="text-[11px] text-slate-400 mt-0.5">Đặt ngày: ${dateStr}</div>
              </div>
            </div>

            <div class="text-left sm:text-right">
              <span class="text-xs text-slate-400 block sm:inline">Tổng thanh toán: </span>
              <span class="font-extrabold text-sm sm:text-base text-[#E0052B]">${formatVND(order.tongTien)}</span>
            </div>
          </div>

          <!-- Danh sách sản phẩm trong đơn -->
          <div class="space-y-2.5 divide-y divide-slate-50">
            ${order.items.map(it => `
              <div class="flex items-center gap-3 pt-2 first:pt-0">
                <img src="${it.image || '../images/deal-tabs/deal-tab-phone.jpg'}" alt="${it.tenHang}" class="w-12 h-12 rounded-xl object-contain border border-slate-100 p-1 bg-slate-50 flex-shrink-0" onerror="this.src='../images/deal-tabs/deal-tab-phone.jpg'">
                <div class="flex-1 min-w-0">
                  <div class="font-medium text-xs sm:text-sm text-slate-800 line-clamp-1">${it.tenHang}</div>
                  <div class="text-slate-400 text-xs mt-0.5">Số lượng: <span class="font-semibold text-slate-700">x${it.soLuong}</span> • Đơn giá: <span class="font-semibold text-slate-700">${formatVND(it.donGia)}</span></div>
                </div>
                <div class="font-bold text-xs sm:text-sm text-slate-900 tabular-nums">${formatVND(it.thanhTien || (it.donGia * it.soLuong))}</div>
              </div>
            `).join('')}
          </div>

          <!-- Hàng Footer: Địa chỉ giao & Các nút thao tác (không bị rớt lệch hàng) -->
          <div class="pt-3 border-t border-slate-100 flex flex-col md:flex-row md:items-center justify-between gap-3 text-xs text-slate-500">
            <div class="flex items-center gap-1.5 min-w-0 flex-1">
              <i class="fa-solid fa-location-dot text-rose-500 flex-shrink-0 text-xs"></i>
              <span class="truncate">Giao đến: <strong class="text-slate-700 font-medium">${order.diaChiGiao}</strong></span>
            </div>

            <div class="flex items-center justify-end gap-1.5 flex-wrap sm:flex-nowrap flex-shrink-0">
              <button type="button" onclick="window.openTrackingModal('${order.maDH}')" class="px-3 py-1.5 rounded-lg bg-blue-50 hover:bg-blue-100 text-[#0066FF] border border-blue-200 font-semibold text-xs transition-colors flex items-center gap-1.5 cursor-pointer whitespace-nowrap shadow-2xs">
                <i class="fa-solid fa-route text-[#0066FF]"></i> Theo dõi hành trình
              </button>

              ${isQrPayment ? `
                <button type="button" onclick="window.showOrderQrModal('${order.maDH}')" class="px-3 py-1.5 rounded-lg bg-[#0066FF]/10 hover:bg-[#0066FF]/20 text-[#0066FF] border border-[#0066FF]/30 font-semibold text-xs transition-colors flex items-center gap-1.5 cursor-pointer whitespace-nowrap shadow-2xs">
                  <i class="fa-solid fa-qrcode"></i> Quét mã QR
                </button>
              ` : ''}

              ${canCancel ? `
                <button type="button" onclick="window.openCancelModal('${order.maDH}')" class="px-3 py-1.5 rounded-lg border border-rose-200 text-rose-600 hover:bg-rose-50 font-semibold text-xs transition-colors cursor-pointer whitespace-nowrap">
                  Hủy đơn hàng
                </button>
              ` : ''}

              <button type="button" onclick="window.reOrderItems('${order.maDH}')" class="px-3.5 py-1.5 rounded-lg bg-[#0066FF] hover:bg-[#0052cc] text-white font-semibold text-xs transition-all cursor-pointer flex items-center gap-1.5 whitespace-nowrap shadow-2xs active:scale-95">
                <i class="fa-solid fa-arrow-rotate-right"></i> Mua lại
              </button>
            </div>
          </div>
        </div>
      `;
    }).join('');
  };

  window.filterOrdersByStatus = function (status, btnEl) {
    currentOrderStatusFilter = status;
    const btns = document.querySelectorAll('.order-filter-btn');
    btns.forEach(b => {
      b.className = 'order-filter-btn px-2.5 py-1 rounded-lg bg-slate-100 hover:bg-slate-200 text-slate-700 cursor-pointer whitespace-nowrap transition-all text-xs font-medium';
    });
    if (btnEl) {
      btnEl.className = 'order-filter-btn px-2.5 py-1 rounded-lg bg-[#0066FF] text-white shadow-2xs cursor-pointer whitespace-nowrap transition-all text-xs font-semibold';
    } else if (status === 'all' && btns.length > 0) {
      btns[0].className = 'order-filter-btn px-2.5 py-1 rounded-lg bg-[#0066FF] text-white shadow-2xs cursor-pointer whitespace-nowrap transition-all text-xs font-semibold';
    }
    window.renderOrders();
  };

  window.handleOrderSearch = function (e) {
    if (e && e.key && e.key !== 'Enter') return;
    window.renderOrders();
  };

  window.openTrackingModal = function (orderCode) {
    const orders = getOrders();
    const order = orders.find(o => o.maDH === orderCode);
    if (!order) return;

    const modal = document.getElementById('order-tracking-modal');
    const codeEl = document.getElementById('tracking-modal-code');
    const addressEl = document.getElementById('tracking-modal-address');
    const timelineBox = document.getElementById('tracking-timeline-box');

    if (codeEl) codeEl.textContent = `Mã ĐH: ${order.maDH}`;
    if (addressEl) addressEl.textContent = order.diaChiGiao || 'Địa chỉ nhận hàng';

    if (timelineBox) {
      const isCancelled = order.trangThai === 'da_huy';
      const isDelivered = order.trangThai === 'da_giao' || order.trangThai === 'hoan_thanh';
      const isShipping = order.trangThai === 'dang_giao';
      const isConfirmed = order.trangThai === 'da_xac_nhan';
      const isWaiting = order.trangThai === 'cho_xac_nhan';

      const steps = [
        {
          title: 'Đơn hàng đã đặt thành công',
          desc: `Thời gian đặt: ${new Date(order.ngayDat).toLocaleString('vi-VN')}`,
          done: true,
          active: false,
          icon: 'fa-check'
        },
        {
          title: isCancelled ? 'Đơn hàng đã bị hủy' : (isConfirmed ? 'Đã duyệt & chuẩn bị hàng' : 'Đang chờ xác nhận'),
          desc: isCancelled ? (order.lyDoHuy ? `Lý do: ${order.lyDoHuy}` : 'Đã hủy theo yêu cầu') : (isWaiting ? 'Hệ thống đang kiểm tra đơn hàng' : 'Kho đang đóng gói kiện hàng'),
          done: isConfirmed || isShipping || isDelivered,
          active: isWaiting && !isCancelled,
          icon: isCancelled ? 'fa-xmark' : 'fa-clipboard-check'
        },
        {
          title: 'Đang vận chuyển hỏa tốc',
          desc: isShipping ? 'Shipper đang giao hàng tới địa chỉ của bạn' : (isDelivered ? 'Đã bàn giao cho người nhận' : 'Dự kiến giao nhanh trong 24-48h'),
          done: isDelivered,
          active: isShipping && !isCancelled,
          icon: 'fa-truck-fast'
        },
        {
          title: 'Giao hàng thành công',
          desc: isDelivered ? 'Kiện hàng đã được giao an toàn' : (isCancelled ? 'Đơn hàng đã hủy' : 'Khách kiểm tra hàng trước khi nhận'),
          done: isDelivered,
          active: false,
          icon: 'fa-box-open'
        }
      ];

      timelineBox.innerHTML = steps.map((s) => {
        const circleClass = s.done ? 'bg-emerald-500 text-white shadow-xs' : (s.active ? 'bg-[#0057B7] text-white shadow-md animate-pulse' : 'bg-slate-200 text-slate-500');
        return `
          <div class="relative flex items-start gap-3.5">
            <div class="w-7 h-7 rounded-full flex items-center justify-center text-xs font-bold ${circleClass} z-10 flex-shrink-0">
              <i class="fa-solid ${s.icon}"></i>
            </div>
            <div class="space-y-0.5">
              <div class="font-bold text-sm sm:text-base ${s.done || s.active ? 'text-slate-900' : 'text-slate-400'}">${s.title}</div>
              <div class="text-xs sm:text-sm text-slate-500">${s.desc}</div>
            </div>
          </div>
        `;
      }).join('');
    }

    if (modal) modal.classList.remove('hidden');
  };

  window.closeTrackingModal = function () {
    const modal = document.getElementById('order-tracking-modal');
    if (modal) modal.classList.add('hidden');
  };

  window.handleOrderSearch = function (e) {
    if (e && e.key && e.key !== 'Enter') return;
    window.renderOrders();
  };

  window.showOrderQrModal = function (orderCode) {
    const orders = getOrders();
    const order = orders.find(o => o.maDH === orderCode);
    if (!order) return;

    activeModalOrderCode = orderCode;
    const modal = document.getElementById('order-qr-modal');
    const title = document.getElementById('modal-qr-order-title');
    const img = document.getElementById('modal-qr-img');
    const amount = document.getElementById('modal-qr-amount');
    const memo = document.getElementById('modal-qr-memo');

    if (title) title.textContent = `Thanh toán đơn ${order.maDH}`;
    if (amount) amount.textContent = formatVND(order.tongTien);

    const memoText = order.maDH;
    if (memo) memo.textContent = memoText;

    if (img) {
      const cfg = getActiveBankConfig();
      img.src = getQrCodeUrl(cfg, order.tongTien, memoText);
    }

    if (modal) modal.classList.remove('hidden');
  };

  window.closeOrderQrModal = function () {
    const modal = document.getElementById('order-qr-modal');
    if (modal) modal.classList.add('hidden');
    activeModalOrderCode = null;
  };

  window.confirmModalPayment = async function () {
    if (!activeModalOrderCode) return;
    const code = activeModalOrderCode;
    const orders = getOrders();
    const idx = orders.findIndex(o => o.maDH === code);
    if (idx > -1) {
      orders[idx].daThanhToan = true;
      orders[idx].trangThai = 'cho_xac_nhan';
      saveOrders(orders);
      window.renderOrders();
      showToast(`Đã ghi nhận chuyển khoản cho đơn ${code}! Đang chờ Admin xác nhận.`, 'success');
    }
    window.closeOrderQrModal();

    try {
      await fetch(`http://localhost:5132/api/payment/confirm/${encodeURIComponent(code)}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ transactionId: `MODAL_CONFIRM_${Date.now()}`, note: 'Khách xác nhận thanh toán qua modal' })
      });
    } catch (e) {}
  };

  window.openCancelModal = function (maDH) {
    pendingCancelOrderCode = maDH;
    const modal = document.getElementById('cancel-order-modal');
    const codeEl = document.getElementById('cancel-modal-order-code') || document.getElementById('cancel-modal-code');
    if (codeEl) codeEl.textContent = maDH;
    if (modal) modal.classList.remove('hidden');
  };

  window.closeCancelModal = function () {
    const modal = document.getElementById('cancel-order-modal');
    if (modal) modal.classList.add('hidden');
    pendingCancelOrderCode = null;
  };

  window.confirmCancelOrder = function () {
    if (!pendingCancelOrderCode) return;

    const reason = document.getElementById('cancel-reason-select')?.value || 'Tôi muốn đổi ý';
    const orders = getOrders();
    const idx = orders.findIndex(o => o.maDH === pendingCancelOrderCode);

    if (idx > -1) {
      const canceledOrder = orders[idx];
      canceledOrder.trangThai = 'da_huy';
      canceledOrder.lyDoHuy = reason;
      saveOrders(orders);
      window.renderOrders();
      showToast(`Đã hủy thành công đơn hàng ${pendingCancelOrderCode}!`);
      if (window.OrderSignalR && typeof window.OrderSignalR.notifyOrderStatusChanged === 'function') {
        window.OrderSignalR.notifyOrderStatusChanged(
          canceledOrder.maDH,
          'da_huy',
          `Khách hàng ${canceledOrder.tenNguoiNhan || ''} vừa hủy đơn hàng #${canceledOrder.maDH} (Lý do: ${reason})`,
          canceledOrder.sdtNguoiNhan || '',
          'user'
        );
      }
      if (window.api && typeof window.api.put === 'function') {
        window.api.put('/orders/' + encodeURIComponent(canceledOrder.maDH) + '/cancel', {
          sdt: canceledOrder.sdtNguoiNhan || '',
          lyDo: reason
        }).catch(() => {});
      }
    }

    window.closeCancelModal();
  };

  window.reOrderItems = function (maDH) {
    const orders = getOrders();
    const order = orders.find(o => o.maDH === maDH);
    if (!order || !order.items || order.items.length === 0) return;

    let cartItems = getCart();

    order.items.forEach(it => {
      const existing = cartItems.find(c => (c.maHang || c.id) === it.maHang);
      if (existing) {
        existing.qty = (Number(existing.qty) || 1) + (Number(it.soLuong) || 1);
        existing.soLuong = existing.qty;
      } else {
        cartItems.push({
          id: `${it.maHang}_reorder_${Date.now()}`,
          maHang: it.maHang,
          tenHang: it.tenHang,
          donGia: Number(it.donGia),
          price: Number(it.donGia),
          soLuong: Number(it.soLuong) || 1,
          qty: Number(it.soLuong) || 1,
          image: it.image || '../images/deal-tabs/deal-tab-phone.jpg'
        });
      }
    });

    saveCart(cartItems);
    showToast(`Đã thêm lại các sản phẩm của đơn ${maDH} vào giỏ hàng!`);
    window.switchCartTab('cart');
  };

  // Lắng nghe đồng bộ liên tab (storage & BroadcastChannel) từ Admin
  window.addEventListener('storage', function (e) {
    if (e.key === 'cellphones_online_orders' || e.key === 'samamobile_user_orders' || e.key === 'cellphones_order_last_updated') {
      if (typeof window.renderOrders === 'function') window.renderOrders();
    }
  });

  if (typeof BroadcastChannel !== 'undefined') {
    try {
      const userBC = new BroadcastChannel('samamobile_orders');
      userBC.onmessage = function (e) {
        if (e.data && e.data.type === 'order_status_updated' && e.data.maDH) {
          window.dispatchEvent(new CustomEvent('samamobile:order_status_updated', { detail: e.data }));
        }
      };
    } catch (err) {}
  }

  // Lắng nghe sự kiện cập nhật trạng thái đơn hàng realtime từ SignalR
  window.addEventListener('samamobile:order_status_updated', function (e) {
    const data = e.detail;
    if (!data || !data.maDH) return;

    const orders = getOrders();
    const order = orders.find(o => o.maDH === data.maDH);
    if (order) {
      order.trangThai = data.trangThai;
      if (data.soHD) order.soHD = data.soHD;
      saveOrders(orders);

      if (typeof window.renderOrders === 'function') {
        window.renderOrders();
      }

      const trackingModal = document.getElementById('order-tracking-modal');
      if (trackingModal && !trackingModal.classList.contains('hidden')) {
        const codeEl = document.getElementById('tracking-modal-code');
        if (codeEl && codeEl.textContent.includes(data.maDH)) {
          window.openTrackingModal(data.maDH);
        }
      }
    }
  });

  // Tùy chỉnh STK nhận tiền thực tế để test quét app ngân hàng
  window.openCustomBankModal = function () {
    const modal = document.getElementById('custom-bank-modal');
    if (modal) modal.classList.remove('hidden');
  };

  window.closeCustomBankModal = function () {
    const modal = document.getElementById('custom-bank-modal');
    if (modal) modal.classList.add('hidden');
  };

  window.saveCustomBankConfig = function () {
    const bank = document.getElementById('custom-bank-select')?.value || 'MBBank';
    const acc = (document.getElementById('custom-bank-account')?.value || '').trim();
    const name = (document.getElementById('custom-bank-name')?.value || '').trim();

    if (!acc) {
      alert('Vui lòng nhập số tài khoản ngân hàng của bạn');
      return;
    }

    const cfg = {
      name: `${bank} (${name || 'Tài khoản cá nhân'})`,
      sepayBank: bank,
      accountNo: acc,
      accountName: name || 'NGUOI THU HUONG'
    };

    localStorage.setItem('samamobile_custom_bank_config', JSON.stringify(cfg));
    window.closeCustomBankModal();
    updateVietQR(getCartFinalTotal());

    const successQrImg = document.getElementById('success-vietqr-img');
    const successCode = document.getElementById('success-order-code')?.textContent?.trim();
    if (successQrImg && successCode && !document.getElementById('order-success-screen')?.classList.contains('hidden')) {
      successQrImg.src = getQrCodeUrl(cfg, getCartFinalTotal(), successCode);
      const qrBank = document.getElementById('success-qr-bank-name');
      const qrAccName = document.getElementById('success-qr-account-name');
      const qrAccNo = document.getElementById('success-qr-account-no');
      if (qrBank) qrBank.textContent = cfg.name;
      if (qrAccName) qrAccName.textContent = cfg.accountName || 'CONG TY TNHH SAMA MOBILE';
      if (qrAccNo) qrAccNo.textContent = cfg.accountNo;
    }

    showToast('Đã lưu cấu hình tài khoản nhận tiền mới!');
  };

  window.resetDefaultBankConfig = function () {
    localStorage.removeItem('samamobile_custom_bank_config');
    window.closeCustomBankModal();
    updateVietQR(getCartFinalTotal());

    const successQrImg = document.getElementById('success-vietqr-img');
    const successCode = document.getElementById('success-order-code')?.textContent?.trim();
    if (successQrImg && successCode && !document.getElementById('order-success-screen')?.classList.contains('hidden')) {
      const cfg = getActiveBankConfig();
      successQrImg.src = getQrCodeUrl(cfg, getCartFinalTotal(), successCode);
      const qrBank = document.getElementById('success-qr-bank-name');
      const qrAccName = document.getElementById('success-qr-account-name');
      const qrAccNo = document.getElementById('success-qr-account-no');
      if (qrBank) qrBank.textContent = cfg.name;
      if (qrAccName) qrAccName.textContent = cfg.accountName || 'CONG TY TNHH SAMA MOBILE';
      if (qrAccNo) qrAccNo.textContent = cfg.accountNo;
    }

    showToast('Đã khôi phục tài khoản mặc định SamaMobile!');
  };


  // =========================================================================
  // BƯỚC 2 & 3: TÍCH HỢP ĐỊA CHỈ 3 CẤP GHN & TÍNH PHÍ VẬN CHUYỂN DỰ KIẾN (QUOTE)
  // =========================================================================
  // Expose and auto-trigger
  window.initGhnAddressSelector = initGhnAddressSelector;

  async function initGhnAddressSelector() {
    const elProvince = document.getElementById('order-province');
    const elDistrict = document.getElementById('order-district');
    const elWard = document.getElementById('order-ward');
    if (!elProvince || !elDistrict || !elWard) return;

    try {
      elProvince.innerHTML = '<option value="">-- Đang tải danh sách tỉnh/thành... --</option>';
      const res = await (window.api && typeof window.api.get === 'function' 
        ? window.api.get('/ghn/provinces') 
        : fetch('/api/ghn/provinces').then(r => r.json()));
      const rawProvinces = Array.isArray(res) ? res : (res?.data || []);
      const provinces = rawProvinces.filter(p => {
        const name = (p.ProvinceName || '').trim();
        const code = (p.Code || '').trim().toUpperCase();
        return !name.toLowerCase().includes('test') && !name.toLowerCase().includes('alert') && !code.includes('TEST') && name !== 'Hà Nội 02';
      });

      provinces.sort((a, b) => {
        const nameA = a.ProvinceName || '';
        const nameB = b.ProvinceName || '';
        if (nameA.includes('Hồ Chí Minh')) return -1;
        if (nameB.includes('Hồ Chí Minh')) return 1;
        if (nameA.includes('Hà Nội')) return -1;
        if (nameB.includes('Hà Nội')) return 1;
        return nameA.localeCompare(nameB, 'vi');
      });

      elProvince.innerHTML = '<option value="">-- Chọn Tỉnh / Thành phố --</option>' +
        provinces.map(p => `<option value="${p.ProvinceID}" data-name="${p.ProvinceName}">${p.ProvinceName}</option>`).join('');

      // Mặc định chọn TP. Hồ Chí Minh (ID: 202) -> TP. Thủ Đức (ID: 3695) -> Phường Hiệp Phú (90754)
      const hcm = provinces.find(p => p.ProvinceID === 202 || (p.ProvinceName || '').includes('Hồ Chí Minh'));
      if (hcm) {
        elProvince.value = hcm.ProvinceID;
        await loadDistricts(hcm.ProvinceID, 3695);
      }
    } catch (err) {
      console.warn('Lỗi tải tỉnh thành GHN:', err);
      elProvince.innerHTML = '<option value="202" data-name="TP. Hồ Chí Minh" selected>TP. Hồ Chí Minh</option>';
      await loadDistricts(202, 3695);
    }

    elProvince.addEventListener('change', async () => {
      const pid = elProvince.value;
      if (pid) {
        await loadDistricts(pid);
      } else {
        elDistrict.innerHTML = '<option value="">-- Chọn quận/huyện --</option>';
        elDistrict.disabled = true;
        elWard.innerHTML = '<option value="">-- Chọn phường/xã --</option>';
        elWard.disabled = true;
        updateShippingQuote();
      }
    });

    elDistrict.addEventListener('change', async () => {
      const did = elDistrict.value;
      if (did) {
        await loadWards(did);
      } else {
        elWard.innerHTML = '<option value="">-- Chọn phường/xã --</option>';
        elWard.disabled = true;
        updateShippingQuote();
      }
    });

    elWard.addEventListener('change', () => {
      updateShippingQuote();
    });
  }

  async function loadDistricts(provinceId, defaultDistrictId = null) {
    const elDistrict = document.getElementById('order-district');
    const elWard = document.getElementById('order-ward');
    if (!elDistrict) return;

    elDistrict.disabled = true;
    elDistrict.innerHTML = '<option value="">-- Đang tải quận/huyện... --</option>';
    if (elWard) {
      elWard.innerHTML = '<option value="">-- Chọn phường/xã --</option>';
      elWard.disabled = true;
    }

    try {
      const res = await (window.api && typeof window.api.get === 'function'
        ? window.api.get('/ghn/districts?provinceId=' + provinceId)
        : fetch('/api/ghn/districts?provinceId=' + provinceId).then(r => r.json()));
      const rawDistricts = Array.isArray(res) ? res : (res?.data || []);
      const districts = rawDistricts.filter(d => {
        const name = (d.DistrictName || '').trim();
        return !name.toLowerCase().includes('test') && !name.toLowerCase().includes('alert');
      });
      districts.sort((a, b) => (a.DistrictName || '').localeCompare(b.DistrictName || '', 'vi'));

      elDistrict.innerHTML = '<option value="">-- Chọn Quận / Huyện --</option>' +
        districts.map(d => `<option value="${d.DistrictID}" data-name="${d.DistrictName}">${d.DistrictName}</option>`).join('');
      elDistrict.disabled = false;

      if (defaultDistrictId && districts.some(d => d.DistrictID === defaultDistrictId)) {
        elDistrict.value = defaultDistrictId;
        await loadWards(defaultDistrictId, "90754");
      } else {
        updateShippingQuote();
      }
    } catch (err) {
      console.warn('Lỗi tải quận huyện GHN:', err);
      elDistrict.innerHTML = '<option value="3695" data-name="TP. Thủ Đức" selected>TP. Thủ Đức</option>';
      elDistrict.disabled = false;
      await loadWards(3695, "90754");
    }
  }

  async function loadWards(districtId, defaultWardCode = null) {
    const elWard = document.getElementById('order-ward');
    if (!elWard) return;

    elWard.disabled = true;
    elWard.innerHTML = '<option value="">-- Đang tải phường/xã... --</option>';

    try {
      const res = await (window.api && typeof window.api.get === 'function'
        ? window.api.get('/ghn/wards?districtId=' + districtId)
        : fetch('/api/ghn/wards?districtId=' + districtId).then(r => r.json()));
      const rawWards = Array.isArray(res) ? res : (res?.data || []);
      const wards = rawWards.filter(w => {
        const name = (w.WardName || '').trim();
        return !name.toLowerCase().includes('test') && !name.toLowerCase().includes('alert');
      });
      wards.sort((a, b) => (a.WardName || '').localeCompare(b.WardName || '', 'vi'));

      elWard.innerHTML = '<option value="">-- Chọn Phường / Xã --</option>' +
        wards.map(w => `<option value="${w.WardCode}" data-name="${w.WardName}">${w.WardName}</option>`).join('');
      elWard.disabled = false;

      if (defaultWardCode && wards.some(w => String(w.WardCode) === String(defaultWardCode))) {
        elWard.value = defaultWardCode;
      }
      updateShippingQuote();
    } catch (err) {
      console.warn('Lỗi tải phường xã GHN:', err);
      elWard.innerHTML = '<option value="90754" data-name="Phường Hiệp Phú" selected>Phường Hiệp Phú</option>';
      elWard.disabled = false;
      updateShippingQuote();
    }
  }

  function debounceShippingQuote() {
    clearTimeout(quoteDebounceTimer);
    quoteDebounceTimer = setTimeout(updateShippingQuote, 350);
  }

  async function updateShippingQuote() {
    const elFee = document.getElementById('summary-shipping-fee');
    const elSaved = document.getElementById('summary-shipping-saved');
    const elLead = document.getElementById('summary-shipping-leadtime');

    if (deliveryMethod === 'store') {
      currentShippingFee = 0;
      currentShippingActual = 0;
      currentShippingSaved = 0;
      currentExpectedDelivery = null;
      if (elFee) elFee.innerHTML = '<span class="text-emerald-600 font-bold">Miễn phí (0₫)</span>';
      if (elSaved) elSaved.textContent = '';
      if (elLead) elLead.textContent = '';
      calculateTotalsSilent();
      return;
    }

    const elDistrict = document.getElementById('order-district');
    const elWard = document.getElementById('order-ward');
    const toDistrictId = elDistrict ? Number(elDistrict.value || 0) : 0;
    const toWardCode = elWard ? (elWard.value || '') : '';

    const items = getCart();
    const selectedItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return selectedItemIds.size === 0 || selectedItemIds.has(id);
    });
    const subtotal = selectedItems.reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);
    let discount = 0;
    if (subtotal >= 1000000) discount += Math.round(subtotal * 0.01);
    if (useMemberPoints && subtotal > 0) discount += Math.min(200000, subtotal);
    if (appliedVoucher && subtotal > 0) {
      discount += appliedVoucher.type === 'percent' ? Math.round((subtotal * appliedVoucher.discount) / 100) : appliedVoucher.discount;
    }
    const subtotalAfterDiscount = Math.max(0, subtotal - discount);

    if (toDistrictId <= 0) {
      currentShippingFee = 0;
      if (elFee) elFee.textContent = 'Chưa chọn địa chỉ';
      if (elSaved) elSaved.textContent = '';
      if (elLead) elLead.textContent = '';
      calculateTotalsSilent();
      return;
    }

    if (elFee) elFee.innerHTML = '<i class="fa-solid fa-spinner fa-spin text-xs mr-1"></i>Đang tính phí...';

    try {
      const quoteReq = {
        toDistrictId: toDistrictId,
        toWardCode: toWardCode,
        subtotalAfterDiscount: subtotalAfterDiscount,
        fulfillment: 'delivery',
        items: selectedItems.map(it => ({
          productId: String(it.maHang || it.id || 'MH001'),
          quantity: Number(it.qty || it.soLuong || 1)
        }))
      };

      const quote = await (window.api && typeof window.api.post === 'function'
        ? window.api.post('/shipping/quote', quoteReq)
        : fetch('/api/shipping/quote', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(quoteReq)
          }).then(r => r.json()));

      currentShippingActual = quote.actualFee || 0;
      currentShippingFee = quote.customerFee || 0;
      currentShippingSaved = quote.freeShipSaved || 0;
      currentExpectedDelivery = quote.expectedDeliveryAt;

      if (quote.isFreeShip || currentShippingFee === 0) {
        if (elFee) elFee.innerHTML = '<span class="text-emerald-600 font-bold">Miễn phí (0₫)</span>';
        if (elSaved) elSaved.textContent = currentShippingSaved > 0 ? `(Đã tiết kiệm ${formatVND(currentShippingSaved)})` : '';
      } else {
        if (elFee) elFee.innerHTML = `<span class="text-slate-800 font-bold font-mono">${formatVND(currentShippingFee)}</span>`;
        if (elSaved) elSaved.textContent = '';
      }

      if (elLead && quote.expectedDeliveryAt) {
        const dt = new Date(quote.expectedDeliveryAt);
        if (!isNaN(dt.getTime())) {
          const pad = n => String(n).padStart(2, '0');
          elLead.textContent = `Dự kiến giao: ${pad(dt.getDate())}/${pad(dt.getMonth() + 1)}/${dt.getFullYear()}`;
        } else {
          elLead.textContent = '';
        }
      } else if (elLead) {
        elLead.textContent = '';
      }
    } catch (err) {
      console.warn('Lỗi tính phí quote GHN:', err);
      currentShippingFee = subtotalAfterDiscount >= 1000000 ? 0 : 25000;
      if (elFee) elFee.innerHTML = currentShippingFee === 0 ? '<span class="text-emerald-600 font-bold">Miễn phí (0₫)</span>' : `<span class="text-slate-800 font-bold font-mono">${formatVND(currentShippingFee)}</span>`;
    }

    calculateTotalsSilent();
  }

  function calculateTotalsSilent() {
    const items = getCart();
    const selectedItems = items.filter((it, idx) => {
      const id = String(it.id || `${it.maHang}_${idx}`);
      return selectedItemIds.size === 0 || selectedItemIds.has(id);
    });
    const subtotal = selectedItems.reduce((s, it) => s + ((Number(it.price || it.donGia || 0)) * (Number(it.qty || it.soLuong || 1))), 0);
    let memberDiscount = 0;
    if (subtotal >= 1000000) memberDiscount += Math.round(subtotal * 0.01);
    let pointsDiscount = 0;
    if (useMemberPoints && subtotal > 0) pointsDiscount = Math.min(200000, subtotal);
    let voucherDiscount = 0;
    if (appliedVoucher && subtotal > 0) {
      voucherDiscount = appliedVoucher.type === 'percent' ? Math.round((subtotal * appliedVoucher.discount) / 100) : appliedVoucher.discount;
    }
    const finalTotal = Math.max(0, subtotal - memberDiscount - pointsDiscount - voucherDiscount) + (currentShippingFee || 0);
    updateTotals(subtotal, memberDiscount, voucherDiscount, finalTotal, pointsDiscount);
  }

  // Tự động kích hoạt tải danh mục hành chính GHN nếu trang đã sẵn sàng
  if (document.readyState === 'complete' || document.readyState === 'interactive') {
    setTimeout(initGhnAddressSelector, 10);
  }

})();
