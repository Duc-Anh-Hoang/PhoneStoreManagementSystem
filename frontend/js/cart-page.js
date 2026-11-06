// =========================================================================
// SAMAMOBILE - CART & ORDER MANAGEMENT PAGE CONTROLLER (THUẦN FRONTEND)
// Quản lý giỏ hàng, đặt hàng, quản lý đơn hàng đã đặt, theo dõi đơn & đơn đã hủy
// =========================================================================

(function () {
  'use strict';

  const STORAGE_KEY = 'cellphones_cart';
  const ORDERS_STORAGE_KEY = 'samamobile_user_orders';
  const VOUCHERS = {
    'SAMAMEMBER': { code: 'SAMAMEMBER', discount: 100000, type: 'fixed', minOrder: 500000, desc: 'Giảm 100.000đ cho hội viên Sama' },
    'HELLOSAMA': { code: 'HELLOSAMA', discount: 5, type: 'percent', minOrder: 0, desc: 'Giảm 5% cho đơn hàng đầu tiên' },
    'GIAM100K': { code: 'GIAM100K', discount: 100000, type: 'fixed', minOrder: 1000000, desc: 'Giảm 100.000đ đơn từ 1 triệu' },
    'FREE50K': { code: 'FREE50K', discount: 50000, type: 'fixed', minOrder: 300000, desc: 'Tặng voucher 50.000đ' }
  };

  let appliedVoucher = null;
  let deliveryMethod = 'delivery'; // 'delivery' | 'store'
  let paymentMethod = 'cod'; // 'cod' | 'banking' | 'momo' | 'vnpay' | 'card'
  let currentActiveTab = 'cart'; // 'cart' | 'orders'
  let currentOrderStatusFilter = 'all'; // 'all' | 'cho_xac_nhan' | 'dang_giao' | 'da_giao' | 'da_huy'
  let pendingCancelOrderCode = null;

  // Dữ liệu đơn hàng mẫu ban đầu (Initial Seed) giúp trải nghiệm trực quan ngay lập tức
  const SAMPLE_ORDERS = [
    {
      maDH: 'DH-261106-8921',
      ngayDat: '2026-11-06T09:30:00.000Z',
      tenNguoiNhan: 'Anh Khang Nguyễn',
      sdtNguoiNhan: '0901234567',
      email: 'huykhang110206@gmail.com',
      diaChiGiao: '241 Lê Văn Việt, Phường Hiệp Phú, TP. Thủ Đức, TP. Hồ Chí Minh',
      phuongThucThanhToan: 'banking',
      ghiChu: 'Giao giờ hành chính, gọi trước 15 phút',
      subtotal: 28990000,
      discount: 389900,
      tongTien: 28600100,
      trangThai: 'cho_xac_nhan', // Chờ xác nhận
      items: [
        {
          maHang: 'IPHONE16PM',
          tenHang: 'iPhone 16 Pro Max 256GB Chính Hãng VN/A',
          soLuong: 1,
          donGia: 28990000,
          thanhTien: 28990000,
          image: 'images/deal-tabs/deal-tab-phone.jpg'
        }
      ]
    },
    {
      maDH: 'DH-261105-4120',
      ngayDat: '2026-11-05T14:15:00.000Z',
      tenNguoiNhan: 'Anh Khang Nguyễn',
      sdtNguoiNhan: '0901234567',
      email: 'huykhang110206@gmail.com',
      diaChiGiao: 'Ký túc xá Khu B ĐHQG, Phường Đông Hòa, Dĩ An, Bình Dương',
      phuongThucThanhToan: 'cod',
      ghiChu: 'Giao buổi chiều',
      subtotal: 23990000,
      discount: 239900,
      tongTien: 23750100,
      trangThai: 'dang_giao', // Đang giao hàng
      items: [
        {
          maHang: 'SS24ULTRA',
          tenHang: 'Samsung Galaxy S24 Ultra 512GB Titan Gray',
          soLuong: 1,
          donGia: 23990000,
          thanhTien: 23990000,
          image: 'images/deal-tabs/deal-tab-dienthoai.jpg'
        }
      ]
    },
    {
      maDH: 'DH-261101-7734',
      ngayDat: '2026-11-01T10:00:00.000Z',
      tenNguoiNhan: 'Anh Khang Nguyễn',
      sdtNguoiNhan: '0901234567',
      email: 'huykhang110206@gmail.com',
      diaChiGiao: '241 Lê Văn Việt, TP. Thủ Đức, TP. Hồ Chí Minh',
      phuongThucThanhToan: 'momo',
      ghiChu: '',
      subtotal: 5390000,
      discount: 100000,
      tongTien: 5290000,
      trangThai: 'da_giao', // Đã giao thành công
      items: [
        {
          maHang: 'AIRPODS2USB',
          tenHang: 'Tai nghe Bluetooth Apple AirPods Pro 2 Type-C (2024)',
          soLuong: 1,
          donGia: 5390000,
          thanhTien: 5390000,
          image: 'images/deal-tabs/deal-tab-audio.jpg'
        }
      ]
    },
    {
      maDH: 'DH-261028-1549',
      ngayDat: '2026-10-28T16:20:00.000Z',
      tenNguoiNhan: 'Anh Khang Nguyễn',
      sdtNguoiNhan: '0901234567',
      email: 'huykhang110206@gmail.com',
      diaChiGiao: 'Số 12 Đường Số 9, P. Linh Trung, TP. Thủ Đức',
      phuongThucThanhToan: 'cod',
      ghiChu: 'Đổi ý mua màu khác',
      subtotal: 450000,
      discount: 0,
      tongTien: 450000,
      trangThai: 'da_huy', // Đã hủy
      lyDoHuy: 'Tôi muốn đổi sang sản phẩm / màu sắc khác',
      items: [
        {
          maHang: 'ANKER30W',
          tenHang: 'Củ sạc nhanh Anker 30W PowerPort III Nano Chính Hãng',
          soLuong: 1,
          donGia: 450000,
          thanhTien: 450000,
          image: 'images/deal-tabs/deal-tab-phukien.jpg'
        }
      ]
    }
  ];

  function getCart() {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      return raw ? JSON.parse(raw) : [];
    } catch (e) {
      return [];
    }
  }

  function saveCart(items) {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
      updateHeaderBadge();
    } catch (e) {}
  }

  function getOrders() {
    try {
      const raw = localStorage.getItem(ORDERS_STORAGE_KEY);
      if (raw) {
        const parsed = JSON.parse(raw);
        if (Array.isArray(parsed) && parsed.length > 0) return parsed;
      }
      // Khởi tạo đơn hàng mẫu nếu chưa có
      localStorage.setItem(ORDERS_STORAGE_KEY, JSON.stringify(SAMPLE_ORDERS));
      return SAMPLE_ORDERS;
    } catch (e) {
      return SAMPLE_ORDERS;
    }
  }

  function saveOrders(orders) {
    try {
      localStorage.setItem(ORDERS_STORAGE_KEY, JSON.stringify(orders));
      updateOrderCounts();
    } catch (e) {}
  }

  function formatVND(num) {
    return new Intl.NumberFormat('vi-VN').format(Math.max(0, Math.round(num || 0))) + ' ₫';
  }

  function formatDate(isoStr) {
    try {
      const d = new Date(isoStr);
      const pad = n => String(n).padStart(2, '0');
      return `${pad(d.getHours())}:${pad(d.getMinutes())} - ${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${d.getFullYear()}`;
    } catch (e) {
      return isoStr || '';
    }
  }

  function updateHeaderBadge() {
    const items = getCart();
    const count = items.reduce((s, it) => s + (it.qty || it.soLuong || 1), 0);
    const cartTabBadge = document.getElementById('tab-badge-cart-count');
    if (cartTabBadge) cartTabBadge.textContent = count;
  }

  function updateOrderCounts() {
    const orders = getOrders();
    const ordersTabBadge = document.getElementById('tab-badge-orders-count');
    if (ordersTabBadge) ordersTabBadge.textContent = orders.length;

    const countAll = orders.length;
    const countPending = orders.filter(o => o.trangThai === 'cho_xac_nhan').length;
    const countShipping = orders.filter(o => o.trangThai === 'dang_giao').length;
    const countDelivered = orders.filter(o => o.trangThai === 'da_giao').length;
    const countCancelled = orders.filter(o => o.trangThai === 'da_huy').length;

    if (document.getElementById('count-status-all')) document.getElementById('count-status-all').textContent = countAll;
    if (document.getElementById('count-status-pending')) document.getElementById('count-status-pending').textContent = countPending;
    if (document.getElementById('count-status-shipping')) document.getElementById('count-status-shipping').textContent = countShipping;
    if (document.getElementById('count-status-delivered')) document.getElementById('count-status-delivered').textContent = countDelivered;
    if (document.getElementById('count-status-cancelled')) document.getElementById('count-status-cancelled').textContent = countCancelled;
  }

  // Khởi tạo trang
  document.addEventListener('DOMContentLoaded', () => {
    initPrefillUser();
    renderCart();
    updateHeaderBadge();
    updateOrderCounts();

    // Lắng nghe URL Hash hoặc Query param để mở thẳng Tab tương ứng (#orders hoặc ?tab=orders)
    const urlParams = new URLSearchParams(window.location.search);
    const tabParam = urlParams.get('tab');
    if (window.location.hash === '#orders' || tabParam === 'orders') {
      window.switchCartTab('orders');
    } else {
      window.switchCartTab('cart');
    }
  });

  // Chuyển đổi giữa [Giỏ hàng] và [Đơn hàng của tôi]
  window.switchCartTab = function (tab) {
    currentActiveTab = tab;
    const cartView = document.getElementById('cart-view-container');
    const ordersView = document.getElementById('orders-view-container');
    const btnTabCart = document.getElementById('tab-nav-cart');
    const btnTabOrders = document.getElementById('tab-nav-orders');

    if (tab === 'orders') {
      if (cartView) cartView.classList.add('hidden');
      if (ordersView) ordersView.classList.remove('hidden');

      if (btnTabCart) {
        btnTabCart.className = 'py-3.5 px-3 sm:px-5 font-bold text-xs sm:text-sm border-b-2 border-transparent text-slate-500 hover:text-slate-900 flex items-center gap-2 cursor-pointer transition-all whitespace-nowrap';
      }
      if (btnTabOrders) {
        btnTabOrders.className = 'py-3.5 px-3 sm:px-5 font-black text-xs sm:text-sm border-b-2 border-[#0057B7] text-[#0057B7] flex items-center gap-2 cursor-pointer transition-all whitespace-nowrap';
      }

      window.renderOrders();
    } else {
      if (cartView) cartView.classList.remove('hidden');
      if (ordersView) ordersView.classList.add('hidden');

      if (btnTabCart) {
        btnTabCart.className = 'py-3.5 px-3 sm:px-5 font-black text-xs sm:text-sm border-b-2 border-[#0057B7] text-[#0057B7] flex items-center gap-2 cursor-pointer transition-all whitespace-nowrap';
      }
      if (btnTabOrders) {
        btnTabOrders.className = 'py-3.5 px-3 sm:px-5 font-bold text-xs sm:text-sm border-b-2 border-transparent text-slate-500 hover:text-slate-900 flex items-center gap-2 cursor-pointer transition-all whitespace-nowrap';
      }

      window.renderCart();
    }

    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // =========================================================================
  // LOGIC PHẦN 1: GIỎ HÀNG & ĐẶT HÀNG
  // =========================================================================

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

          if (nameInput && u.hoTen) nameInput.value = u.hoTen;
          if (phoneInput && u.sdt) phoneInput.value = u.sdt;
          if (emailInput && u.email) emailInput.value = u.email;
          if (addressInput && u.diaChi) addressInput.value = u.diaChi;
        }
      }
    } catch (e) {}
  }

  window.renderCart = function () {
    const items = getCart();
    const cartContainer = document.getElementById('cart-items-container');
    const emptyState = document.getElementById('cart-empty-state');
    const mainContent = document.getElementById('cart-main-content');
    const itemsCountLabel = document.getElementById('cart-items-count-label');

    if (!items || items.length === 0) {
      if (emptyState) emptyState.classList.remove('hidden');
      if (mainContent) mainContent.classList.add('hidden');
      if (itemsCountLabel) itemsCountLabel.textContent = '(0 sản phẩm)';
      updateTotals(0, 0, 0);
      return;
    }

    if (emptyState) emptyState.classList.add('hidden');
    if (mainContent) mainContent.classList.remove('hidden');

    const totalQty = items.reduce((s, it) => s + (it.qty || it.soLuong || 1), 0);
    if (itemsCountLabel) itemsCountLabel.textContent = `(${totalQty} sản phẩm)`;

    if (cartContainer) {
      cartContainer.innerHTML = items.map((item, idx) => {
        const itemPrice = item.price || item.donGia || 0;
        const oldPrice = item.oldPrice || item.giaGoc || itemPrice;
        const qty = item.qty || item.soLuong || 1;
        const subtotal = itemPrice * qty;
        const itemId = item.id || `${item.maHang}_${idx}`;

        return `
          <div class="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 p-4 rounded-2xl border border-slate-200/80 hover:border-blue-200 hover:shadow-xs transition-all bg-white">
            <div class="flex items-center gap-3.5 flex-1 min-w-0">
              <div class="w-16 h-16 sm:w-20 sm:h-20 rounded-xl bg-slate-50 p-1.5 border border-slate-100 flex-shrink-0 flex items-center justify-center">
                <img src="${item.image || item.hinhAnh || 'images/default-phone.png'}" alt="${item.tenHang || item.name}" class="w-full h-full object-contain" onerror="this.src='images/deal-tabs/deal-tab-phone.jpg'">
              </div>
              <div class="flex-1 min-w-0 space-y-1">
                <h4 class="font-extrabold text-xs sm:text-sm text-slate-900 leading-snug truncate" title="${item.tenHang || item.name}">
                  ${item.tenHang || item.name}
                </h4>
                <div class="flex flex-wrap items-center gap-1.5 text-[11px] text-slate-500">
                  ${item.variantName || item.phienBan ? `<span class="bg-slate-100 px-2 py-0.5 rounded font-semibold text-slate-700">${item.variantName || item.phienBan}</span>` : ''}
                  ${item.colorName || item.mauSac ? `<span class="bg-blue-50 text-[#0057B7] px-2 py-0.5 rounded font-semibold">${item.colorName || item.mauSac}</span>` : ''}
                </div>
                <div class="flex items-baseline gap-2 pt-0.5">
                  <span class="text-xs sm:text-sm font-black text-[#E0052B]">${formatVND(itemPrice)}</span>
                  ${oldPrice > itemPrice ? `<span class="text-[11px] text-slate-400 line-through">${formatVND(oldPrice)}</span>` : ''}
                </div>
              </div>
            </div>

            <div class="flex items-center justify-between sm:justify-end gap-4 w-full sm:w-auto pt-2 sm:pt-0 border-t sm:border-t-0 border-slate-100">
              <div class="flex items-center rounded-xl border border-slate-300 bg-slate-50 overflow-hidden">
                <button type="button" onclick="window.changeItemQty('${itemId}', -1)" class="w-8 h-8 flex items-center justify-center text-slate-600 hover:bg-slate-200 transition-colors font-bold cursor-pointer">-</button>
                <span class="w-9 text-center text-xs font-bold text-slate-800">${qty}</span>
                <button type="button" onclick="window.changeItemQty('${itemId}', 1)" class="w-8 h-8 flex items-center justify-center text-slate-600 hover:bg-slate-200 transition-colors font-bold cursor-pointer">+</button>
              </div>

              <div class="text-right min-w-[90px]">
                <div class="text-xs sm:text-sm font-black text-slate-900">${formatVND(subtotal)}</div>
              </div>

              <button type="button" onclick="window.removeCartItem('${itemId}')" class="w-8 h-8 rounded-full text-slate-400 hover:text-rose-600 hover:bg-rose-50 flex items-center justify-center transition-colors cursor-pointer" title="Xóa">
                <i class="fa-regular fa-trash-can text-sm"></i>
              </button>
            </div>
          </div>
        `;
      }).join('');
    }

    calculateTotals();
  };

  window.changeItemQty = function (itemId, delta) {
    let items = getCart();
    const idx = items.findIndex((it, i) => (it.id || `${it.maHang}_${i}`) === itemId);
    if (idx === -1) return;

    let currentQty = items[idx].qty || items[idx].soLuong || 1;
    let newQty = currentQty + delta;

    if (newQty <= 0) {
      if (confirm(`Bạn có chắc muốn xóa "${items[idx].tenHang || items[idx].name}" khỏi giỏ hàng?`)) {
        items.splice(idx, 1);
      }
    } else {
      items[idx].qty = newQty;
      items[idx].soLuong = newQty;
    }

    saveCart(items);
    renderCart();
  };

  window.removeCartItem = function (itemId) {
    let items = getCart();
    items = items.filter((it, i) => (it.id || `${it.maHang}_${i}`) !== itemId);
    saveCart(items);
    renderCart();
  };

  window.clearAllCart = function () {
    if (confirm('Bạn có chắc muốn xóa toàn bộ sản phẩm trong giỏ hàng?')) {
      saveCart([]);
      renderCart();
    }
  };

  function calculateTotals() {
    const items = getCart();
    const subtotal = items.reduce((s, it) => s + ((it.price || it.donGia || 0) * (it.qty || it.soLuong || 1)), 0);

    let memberDiscount = 0;
    if (subtotal >= 1000000) {
      memberDiscount = Math.round(subtotal * 0.01);
    }

    let voucherDiscount = 0;
    if (appliedVoucher) {
      if (appliedVoucher.type === 'percent') {
        voucherDiscount = Math.round((subtotal * appliedVoucher.discount) / 100);
      } else {
        voucherDiscount = appliedVoucher.discount;
      }
    }

    const finalTotal = Math.max(0, subtotal - memberDiscount - voucherDiscount);
    updateTotals(subtotal, memberDiscount, voucherDiscount, finalTotal);
  }

  function updateTotals(subtotal, memberDiscount, voucherDiscount, finalTotal) {
    if (finalTotal === undefined) finalTotal = Math.max(0, subtotal - memberDiscount - voucherDiscount);

    const elSubtotal = document.getElementById('summary-subtotal');
    const elMember = document.getElementById('summary-member-discount');
    const elVoucherRow = document.getElementById('summary-voucher-row');
    const elVoucherDiscount = document.getElementById('summary-voucher-discount');
    const elFinal = document.getElementById('summary-final-total');

    if (elSubtotal) elSubtotal.textContent = formatVND(subtotal);
    if (elMember) elMember.textContent = '-' + formatVND(memberDiscount);

    if (elVoucherRow && elVoucherDiscount) {
      if (voucherDiscount > 0) {
        elVoucherRow.classList.remove('hidden');
        elVoucherDiscount.textContent = '-' + formatVND(voucherDiscount);
      } else {
        elVoucherRow.classList.add('hidden');
      }
    }

    if (elFinal) elFinal.textContent = formatVND(finalTotal);
    updateVietQR(finalTotal);
  }

  window.quickApplyVoucher = function (code) {
    const input = document.getElementById('voucher-input');
    if (input) input.value = code;
    window.applyVoucherCode();
  };

  window.applyVoucherCode = function () {
    const input = document.getElementById('voucher-input');
    const msgEl = document.getElementById('voucher-message');
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

    const subtotal = getCart().reduce((s, it) => s + ((it.price || it.donGia || 0) * (it.qty || it.soLuong || 1)), 0);
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
      if (btnDelivery) btnDelivery.className = 'py-3 px-4 rounded-xl font-extrabold text-xs sm:text-sm bg-white text-[#0057B7] border-2 border-[#0057B7] shadow-sm flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (btnStore) btnStore.className = 'py-3 px-4 rounded-xl font-bold text-xs sm:text-sm bg-slate-50 text-slate-600 border border-slate-200 hover:bg-slate-100 flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (addressBox) addressBox.classList.remove('hidden');
      if (storeBox) storeBox.classList.add('hidden');
    } else {
      if (btnDelivery) btnDelivery.className = 'py-3 px-4 rounded-xl font-bold text-xs sm:text-sm bg-slate-50 text-slate-600 border border-slate-200 hover:bg-slate-100 flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (btnStore) btnStore.className = 'py-3 px-4 rounded-xl font-extrabold text-xs sm:text-sm bg-white text-[#0057B7] border-2 border-[#0057B7] shadow-sm flex items-center justify-center gap-2 cursor-pointer transition-all';
      if (addressBox) addressBox.classList.add('hidden');
      if (storeBox) storeBox.classList.remove('hidden');
    }
  };

  function updateVietQR(amount) {
    const qrImg = document.getElementById('vietqr-code-img');
    const qrAmount = document.getElementById('vietqr-amount-text');
    if (qrAmount) qrAmount.textContent = formatVND(amount);

    if (qrImg) {
      const bankId = 'MB';
      const accountNo = '0901234567';
      const template = 'compact2';
      const desc = encodeURIComponent('SAMA ' + Math.floor(100000 + Math.random() * 900000));
      qrImg.src = `https://img.vietqr.io/image/${bankId}-${accountNo}-${template}.png?amount=${Math.round(amount)}&addInfo=${desc}&accountName=SAMA%20MOBILE%20STORE`;
    }
  }

  window.submitCheckoutOrder = async function (e) {
    if (e) e.preventDefault();

    const items = getCart();
    if (!items || items.length === 0) {
      alert('Giỏ hàng đang trống, vui lòng chọn sản phẩm!');
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
      fullAddress = `${street}, ${ward ? ward + ', ' : ''}${district ? district + ', ' : ''}${province}`;
    }

    const subtotal = items.reduce((s, it) => s + ((it.price || it.donGia || 0) * (it.qty || it.soLuong || 1)), 0);
    let discount = 0;
    if (subtotal >= 1000000) discount += Math.round(subtotal * 0.01);
    if (appliedVoucher) {
      discount += appliedVoucher.type === 'percent' ? Math.round((subtotal * appliedVoucher.discount) / 100) : appliedVoucher.discount;
    }
    const finalTotal = Math.max(0, subtotal - discount);

    const randomCode = Math.floor(1000 + Math.random() * 9000);
    const maDH = `DH-261106-${randomCode}`;

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
      trangThai: 'cho_xac_nhan',
      items: items.map(it => ({
        maHang: it.maHang || it.id,
        tenHang: it.tenHang || it.name,
        soLuong: it.qty || it.soLuong || 1,
        donGia: it.price || it.donGia,
        thanhTien: (it.price || it.donGia) * (it.qty || it.soLuong || 1),
        image: it.image || it.hinhAnh
      }))
    };

    const btnSubmit = document.getElementById('btn-submit-order');
    if (btnSubmit) {
      btnSubmit.disabled = true;
      btnSubmit.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-2"></i> Đang tạo đơn hàng...';
    }

    // Lưu vào LocalStorage
    try {
      const orders = getOrders();
      orders.unshift(orderData);
      saveOrders(orders);
    } catch (e) {}

    // Xóa giỏ hàng
    saveCart([]);

    // Hiển thị màn hình thành công
    showSuccessScreen(orderData);
  };

  function showSuccessScreen(order) {
    const mainScreen = document.getElementById('cart-main-wrapper');
    const successScreen = document.getElementById('order-success-screen');

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

      if (elCode) elCode.textContent = order.maDH;
      if (elName) elName.textContent = order.tenNguoiNhan;
      if (elPhone) elPhone.textContent = order.sdtNguoiNhan;
      if (elAddress) elAddress.textContent = order.diaChiGiao;
      if (elPayment) {
        const paymentMap = {
          'cod': 'Thanh toán tiền mặt khi nhận hàng (COD)',
          'banking': 'Chuyển khoản Ngân hàng (VietQR)',
          'momo': 'Ví điện tử MoMo',
          'vnpay': 'Cổng VNPAY-QR',
          'card': 'Thẻ Quốc tế / Trả góp 0%'
        };
        elPayment.textContent = paymentMap[order.phuongThucThanhToan] || 'COD';
      }
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
      if (vietQrBox) {
        if (order.phuongThucThanhToan === 'banking') {
          vietQrBox.classList.remove('hidden');
          const qrImg = document.getElementById('success-vietqr-img');
          const memoText = document.getElementById('success-transfer-content');
          const memo = `SAMA ${order.maDH.replace('DH-', '').replace(/-/g, '')}`;
          if (memoText) memoText.textContent = memo;
          if (qrImg) {
            qrImg.src = `https://img.vietqr.io/image/MB-0901234567-compact2.png?amount=${Math.round(order.tongTien)}&addInfo=${encodeURIComponent(memo)}&accountName=SAMA%20MOBILE%20STORE`;
          }
        } else {
          vietQrBox.classList.add('hidden');
        }
      }
    }
  }

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

    // 1. Lọc theo trạng thái
    if (currentOrderStatusFilter !== 'all') {
      filtered = filtered.filter(o => o.trangThai === currentOrderStatusFilter);
    }

    // 2. Lọc theo tìm kiếm (Mã đơn hoặc Số điện thoại)
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

    container.innerHTML = filtered.map(order => {
      // Badge trạng thái
      let statusBadge = '';
      switch (order.trangThai) {
        case 'cho_xac_nhan':
          statusBadge = '<span class="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-amber-50 text-amber-700 border border-amber-200"><i class="fa-regular fa-clock"></i> Chờ xác nhận</span>';
          break;
        case 'dang_giao':
          statusBadge = '<span class="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-blue-50 text-[#0057B7] border border-blue-200"><i class="fa-solid fa-truck-fast"></i> Đang giao hàng</span>';
          break;
        case 'da_giao':
          statusBadge = '<span class="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-emerald-50 text-emerald-700 border border-emerald-200"><i class="fa-solid fa-circle-check"></i> Đã giao thành công</span>';
          break;
        case 'da_huy':
          statusBadge = '<span class="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-rose-50 text-rose-700 border border-rose-200"><i class="fa-solid fa-ban"></i> Đã hủy</span>';
          break;
        default:
          statusBadge = '<span class="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold bg-slate-100 text-slate-700">Đang xử lý</span>';
      }

      // Danh sách sản phẩm của đơn
      const itemsHtml = (order.items || []).map(it => `
        <div class="flex items-center justify-between py-2.5 border-b border-slate-100 last:border-b-0 gap-3">
          <div class="flex items-center gap-3 min-w-0">
            <div class="w-12 h-12 rounded-xl bg-slate-50 p-1 border border-slate-200/80 flex-shrink-0 flex items-center justify-center">
              <img src="${it.image || 'images/deal-tabs/deal-tab-phone.jpg'}" alt="${it.tenHang}" class="w-full h-full object-contain" onerror="this.src='images/deal-tabs/deal-tab-phone.jpg'">
            </div>
            <div class="min-w-0">
              <p class="text-xs sm:text-sm font-bold text-slate-900 truncate">${it.tenHang}</p>
              <p class="text-[11px] text-slate-500">Số lượng: <strong>x${it.soLuong}</strong> | Đơn giá: ${formatVND(it.donGia)}</p>
            </div>
          </div>
          <div class="text-right flex-shrink-0 font-black text-xs sm:text-sm text-slate-900">
            ${formatVND(it.thanhTien || (it.donGia * it.soLuong))}
          </div>
        </div>
      `).join('');

      return `
        <div class="bg-white rounded-3xl p-5 sm:p-6 border border-slate-200/90 shadow-2xs hover:shadow-xs transition-all space-y-4">
          <!-- Header Đơn -->
          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-3 pb-3 border-b border-slate-100">
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 rounded-2xl bg-blue-50 text-[#0057B7] flex items-center justify-center font-bold text-base shadow-xs">
                <i class="fa-solid fa-receipt"></i>
              </div>
              <div>
                <div class="flex items-center gap-2">
                  <span class="font-mono font-black text-sm sm:text-base text-slate-900">${order.maDH}</span>
                  <button type="button" onclick="window.copyText('${order.maDH}')" class="text-slate-400 hover:text-[#0057B7] text-xs cursor-pointer" title="Sao chép mã đơn">
                    <i class="fa-regular fa-copy"></i>
                  </button>
                </div>
                <div class="text-[11px] text-slate-400 mt-0.5">Đặt lúc: ${formatDate(order.ngayDat)}</div>
              </div>
            </div>
            <div>
              ${statusBadge}
            </div>
          </div>

          <!-- Danh sách mặt hàng -->
          <div class="bg-slate-50/60 rounded-2xl p-3 sm:p-4 border border-slate-100 space-y-1">
            ${itemsHtml}
          </div>

          <!-- Thông tin giao hàng & Lý do hủy (nếu có) -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-3 text-xs text-slate-600 bg-white pt-1">
            <div>
              <div>Người nhận: <strong class="text-slate-800">${order.tenNguoiNhan}</strong> (${order.sdtNguoiNhan})</div>
              <div class="truncate mt-0.5" title="${order.diaChiGiao}">Địa chỉ: <span class="text-slate-800">${order.diaChiGiao}</span></div>
            </div>
            <div class="md:text-right">
              <div>Thanh toán: <strong class="text-slate-800 uppercase">${order.phuongThucThanhToan}</strong></div>
              ${order.lyDoHuy ? `<div class="text-rose-600 font-semibold mt-0.5">Lý do hủy: ${order.lyDoHuy}</div>` : ''}
            </div>
          </div>

          <!-- Footer Đơn: Tổng tiền & Các nút hành động -->
          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pt-3 border-t border-slate-100">
            <div class="flex items-baseline gap-2">
              <span class="text-xs text-slate-500 font-bold">Tổng thanh toán:</span>
              <span class="text-base sm:text-lg font-black text-[#E0052B]">${formatVND(order.tongTien)}</span>
            </div>

            <!-- Cụm nút hành động -->
            <div class="flex flex-wrap items-center gap-2">
              
              <!-- Nút Theo dõi hành trình (cho đơn chưa hủy) -->
              ${order.trangThai !== 'da_huy' ? `
                <button type="button" onclick="window.openTrackingModal('${order.maDH}')" class="px-3.5 py-2 rounded-xl bg-blue-50 hover:bg-blue-100 text-[#0057B7] font-bold text-xs border border-blue-200 transition-colors flex items-center gap-1.5 cursor-pointer">
                  <i class="fa-solid fa-route"></i> Theo dõi hành trình
                </button>
              ` : ''}

              <!-- Nút Hủy đơn (chỉ cho đơn chờ xác nhận) -->
              ${order.trangThai === 'cho_xac_nhan' ? `
                <button type="button" onclick="window.openCancelModal('${order.maDH}')" class="px-3.5 py-2 rounded-xl bg-rose-50 hover:bg-rose-100 text-rose-700 font-bold text-xs border border-rose-200 transition-colors flex items-center gap-1.5 cursor-pointer">
                  <i class="fa-solid fa-ban"></i> Hủy đơn hàng
                </button>
              ` : ''}

              <!-- Nút Mua lại (cho đơn đã giao hoặc đã hủy) -->
              <button type="button" onclick="window.reOrderItems('${order.maDH}')" class="px-4 py-2 rounded-xl bg-slate-900 hover:bg-[#0057B7] text-white font-bold text-xs transition-colors flex items-center gap-1.5 cursor-pointer">
                <i class="fa-solid fa-arrows-rotate"></i> Mua lại
              </button>

            </div>
          </div>
        </div>
      `;
    }).join('');
  };

  // Lọc theo trạng thái
  window.filterOrdersByStatus = function (status, btnEl) {
    currentOrderStatusFilter = status;

    const btns = document.querySelectorAll('.order-filter-btn');
    btns.forEach(b => {
      b.className = 'order-filter-btn px-4 py-2 rounded-xl bg-slate-100 hover:bg-slate-200 text-slate-700 cursor-pointer whitespace-nowrap transition-all';
    });

    if (btnEl) {
      btnEl.className = 'order-filter-btn px-4 py-2 rounded-xl bg-[#0057B7] text-white shadow-xs cursor-pointer whitespace-nowrap transition-all';
    }

    window.renderOrders();
  };

  // Tra cứu theo tìm kiếm
  window.handleOrderSearch = function (e) {
    if (e && e.key && e.key !== 'Enter') return;
    window.renderOrders();
  };

  // Modal Theo dõi hành trình đơn hàng
  window.openTrackingModal = function (maDH) {
    const orders = getOrders();
    const order = orders.find(o => o.maDH === maDH);
    if (!order) return;

    const modal = document.getElementById('order-tracking-modal');
    const codeEl = document.getElementById('tracking-modal-code');
    const addressEl = document.getElementById('tracking-modal-address');
    const timelineBox = document.getElementById('tracking-timeline-box');

    if (codeEl) codeEl.textContent = `Mã đơn hàng: ${order.maDH}`;
    if (addressEl) addressEl.textContent = `${order.tenNguoiNhan} - ${order.sdtNguoiNhan} | ${order.diaChiGiao}`;

    // Render timeline 4 bước
    const isStep1Done = true;
    const isStep2Done = order.trangThai === 'dang_giao' || order.trangThai === 'da_giao';
    const isStep3Done = order.trangThai === 'dang_giao' || order.trangThai === 'da_giao';
    const isStep4Done = order.trangThai === 'da_giao';

    if (timelineBox) {
      timelineBox.innerHTML = `
        <!-- Bước 1 -->
        <div class="relative flex items-start gap-3">
          <div class="w-6 h-6 rounded-full ${isStep1Done ? 'bg-emerald-500 text-white' : 'bg-slate-300 text-slate-600'} flex items-center justify-center text-xs font-bold flex-shrink-0 z-10 shadow-sm">
            <i class="fa-solid fa-check"></i>
          </div>
          <div>
            <div class="text-xs sm:text-sm font-bold text-slate-900">Đặt hàng thành công</div>
            <div class="text-[11px] text-slate-400">Đơn hàng đã được ghi nhận trên hệ thống SamaMobile</div>
          </div>
        </div>

        <!-- Bước 2 -->
        <div class="relative flex items-start gap-3">
          <div class="w-6 h-6 rounded-full ${isStep2Done ? 'bg-emerald-500 text-white' : 'bg-slate-300 text-slate-600'} flex items-center justify-center text-xs font-bold flex-shrink-0 z-10 shadow-sm">
            <i class="fa-solid ${isStep2Done ? 'fa-check' : 'fa-box'}"></i>
          </div>
          <div>
            <div class="text-xs sm:text-sm font-bold ${isStep2Done ? 'text-slate-900' : 'text-slate-400'}">Đã tiếp nhận & đóng gói</div>
            <div class="text-[11px] text-slate-400">Kho SamaMobile 241 Lê Văn Việt đang kiểm tra và đóng gói sản phẩm</div>
          </div>
        </div>

        <!-- Bước 3 -->
        <div class="relative flex items-start gap-3">
          <div class="w-6 h-6 rounded-full ${isStep3Done ? 'bg-blue-600 text-white animate-pulse' : 'bg-slate-300 text-slate-600'} flex items-center justify-center text-xs font-bold flex-shrink-0 z-10 shadow-sm">
            <i class="fa-solid fa-truck-fast"></i>
          </div>
          <div>
            <div class="text-xs sm:text-sm font-bold ${isStep3Done ? 'text-[#0057B7]' : 'text-slate-400'}">Đang trên đường giao hàng</div>
            <div class="text-[11px] text-slate-400">Tài xế công nghệ đang chuyển hàng tới địa chỉ của bạn (Dự kiến trong 2 giờ)</div>
          </div>
        </div>

        <!-- Bước 4 -->
        <div class="relative flex items-start gap-3">
          <div class="w-6 h-6 rounded-full ${isStep4Done ? 'bg-emerald-500 text-white' : 'bg-slate-300 text-slate-600'} flex items-center justify-center text-xs font-bold flex-shrink-0 z-10 shadow-sm">
            <i class="fa-solid fa-house-chimney-check"></i>
          </div>
          <div>
            <div class="text-xs sm:text-sm font-bold ${isStep4Done ? 'text-emerald-700' : 'text-slate-400'}">Giao hàng thành công</div>
            <div class="text-[11px] text-slate-400">Khách hàng kiểm tra sản phẩm và hoàn tất ký nhận</div>
          </div>
        </div>
      `;
    }

    if (modal) {
      modal.classList.remove('hidden');
      document.body.style.overflow = 'hidden';
    }
  };

  window.closeTrackingModal = function () {
    const modal = document.getElementById('order-tracking-modal');
    if (modal) {
      modal.classList.add('hidden');
      document.body.style.overflow = '';
    }
  };

  // Modal Hủy Đơn Hàng
  window.openCancelModal = function (maDH) {
    pendingCancelOrderCode = maDH;
    const modal = document.getElementById('cancel-order-modal');
    const codeEl = document.getElementById('cancel-modal-code');
    if (codeEl) codeEl.textContent = `Mã đơn: ${maDH}`;

    if (modal) {
      modal.classList.remove('hidden');
      document.body.style.overflow = 'hidden';
    }
  };

  window.closeCancelModal = function () {
    pendingCancelOrderCode = null;
    const modal = document.getElementById('cancel-order-modal');
    if (modal) {
      modal.classList.add('hidden');
      document.body.style.overflow = '';
    }
  };

  window.confirmCancelOrder = function () {
    if (!pendingCancelOrderCode) return;

    const reason = document.getElementById('cancel-reason-select')?.value || 'Tôi muốn đổi ý';
    const orders = getOrders();
    const idx = orders.findIndex(o => o.maDH === pendingCancelOrderCode);

    if (idx > -1) {
      orders[idx].trangThai = 'da_huy';
      orders[idx].lyDoHuy = reason;
      saveOrders(orders);
      window.renderOrders();
      alert(`Đã hủy thành công đơn hàng ${pendingCancelOrderCode}! Đơn hàng đã được chuyển sang mục "Đơn đã hủy".`);
    }

    window.closeCancelModal();
  };

  // Mua lại đơn hàng (Đưa các sản phẩm trong đơn vào giỏ hàng và chuyển sang Tab Giỏ Hàng)
  window.reOrderItems = function (maDH) {
    const orders = getOrders();
    const order = orders.find(o => o.maDH === maDH);
    if (!order || !order.items || order.items.length === 0) return;

    let cartItems = getCart();

    order.items.forEach(it => {
      const existing = cartItems.find(c => (c.maHang || c.id) === it.maHang);
      if (existing) {
        existing.qty = (existing.qty || 1) + (it.soLuong || 1);
      } else {
        cartItems.push({
          id: `${it.maHang}_reorder_${Date.now()}`,
          maHang: it.maHang,
          tenHang: it.tenHang,
          donGia: it.donGia,
          price: it.donGia,
          soLuong: it.soLuong || 1,
          qty: it.soLuong || 1,
          image: it.image || 'images/deal-tabs/deal-tab-phone.jpg'
        });
      }
    });

    saveCart(cartItems);
    alert(`Đã thêm lại các sản phẩm của đơn ${maDH} vào giỏ hàng!`);
    window.switchCartTab('cart');
  };

  window.copyText = function (text) {
    if (!text) return;
    navigator.clipboard.writeText(text).then(() => {
      alert(`Đã sao chép: "${text}" vào bộ nhớ tạm!`);
    }).catch(() => {
      prompt('Sao chép mã này:', text);
    });
  };

})();
