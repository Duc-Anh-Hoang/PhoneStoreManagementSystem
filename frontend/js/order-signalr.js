// =========================================================================
// SAMAMOBILE - SIGNALR & MULTI-CHANNEL REALTIME ORDER NOTIFICATION SYSTEM
// Kết nối realtime 2 chiều (SignalR + BroadcastChannel + LocalStorage Sync):
// 1. Khách đặt hàng / hủy đơn -> Gửi thông báo nổi + chuông + chuông thông báo Header tới Admin ngay lập tức
// 2. Admin duyệt / giao / hoàn tất / hủy đơn -> Gửi thông báo nổi + chuông + chuông thông báo Header tới User ngay lập tức
// =========================================================================

(function () {
  'use strict';

  const USER_NOTIF_KEY = 'samamobile_user_notifications';
  const ADMIN_NOTIF_KEY = 'samamobile_admin_notifications';
  const isAdminPage = window.location.pathname.includes('admin.html');

  // Âm chuông thông báo Web Audio API
  function playChime(type) {
    try {
      const AudioCtx = window.AudioContext || window.webkitAudioContext;
      if (!AudioCtx) return;
      const ctx = new AudioCtx();
      if (ctx.state === 'suspended') {
        ctx.resume().catch(() => {});
      }

      const now = ctx.currentTime;
      const freqs = type === 'new_order'
        ? [523.25, 659.25, 783.99, 1046.50] // C5 -> E5 -> G5 -> C6 (Chuông đơn mới Admin)
        : [587.33, 783.99, 1174.66]; // D5 -> G5 -> D6 (Chuông cập nhật đơn User)

      freqs.forEach((freq, idx) => {
        const osc = ctx.createOscillator();
        const gain = ctx.createGain();
        osc.type = 'sine';
        osc.frequency.setValueAtTime(freq, now + idx * 0.12);

        gain.gain.setValueAtTime(0.001, now + idx * 0.12);
        gain.gain.exponentialRampToValueAtTime(0.28, now + idx * 0.12 + 0.03);
        gain.gain.exponentialRampToValueAtTime(0.0001, now + idx * 0.12 + 0.35);

        osc.connect(gain);
        gain.connect(ctx.destination);

        osc.start(now + idx * 0.12);
        osc.stop(now + idx * 0.12 + 0.4);
      });
    } catch (e) {}
  }

  function formatVNTime(isoOrDate) {
    try {
      const d = isoOrDate ? new Date(isoOrDate) : new Date();
      if (isNaN(d.getTime())) return 'Vừa xong';
      return d.toLocaleString('vi-VN', {
        timeZone: 'Asia/Ho_Chi_Minh',
        hour: '2-digit',
        minute: '2-digit',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour12: false
      });
    } catch (e) {
      return 'Vừa xong';
    }
  }

  function getOrdersPageHref() {
    return window.location.pathname.includes('/pages/') ? 'cart.html#orders' : 'pages/cart.html#orders';
  }

  // =========================================================================
  // TRUNG TÂM THÔNG BÁO (CHUÔNG HEADER CHO CẢ USER & ADMIN)
  // =========================================================================
  function getStoredNotifications(forAdmin) {
    try {
      const key = forAdmin ? ADMIN_NOTIF_KEY : USER_NOTIF_KEY;
      const raw = localStorage.getItem(key);
      if (raw) {
        const parsed = JSON.parse(raw);
        if (Array.isArray(parsed)) return parsed;
      }
    } catch (e) {}
    return [];
  }

  function saveStoredNotifications(list, forAdmin) {
    try {
      const key = forAdmin ? ADMIN_NOTIF_KEY : USER_NOTIF_KEY;
      localStorage.setItem(key, JSON.stringify((list || []).slice(0, 30)));
    } catch (e) {}
  }

  function addNotificationItem(item, forAdmin) {
    const list = getStoredNotifications(forAdmin);
    const id = item.id || `${item.maDH || 'notif'}_${item.trangThai || item.type || Date.now()}`;
    const existingIdx = list.findIndex(x => x.id === id);
    const newItem = {
      id,
      maDH: item.maDH || '',
      title: item.title || 'Thông báo đơn hàng',
      message: item.message || '',
      time: item.time || formatVNTime(new Date()),
      icon: item.icon || 'fa-solid fa-bell',
      bgClass: item.bgClass || 'bg-blue-100',
      colorClass: item.colorClass || 'text-[#2563EB]',
      unread: true,
      createdAt: Date.now()
    };
    if (existingIdx > -1) {
      list.splice(existingIdx, 1);
    }
    list.unshift(newItem);
    saveStoredNotifications(list, forAdmin);
    if (forAdmin === isAdminPage) {
      renderHeaderNotifications();
    }
  }

  function renderHeaderNotifications() {
    const listEl = document.getElementById('notification-list');
    const badge = document.getElementById('notification-badge');
    const countBadge = document.getElementById('notification-count-badge');
    if (!listEl) return;

    const notifications = getStoredNotifications(isAdminPage);
    const unreadCount = notifications.filter(n => n.unread !== false).length;

    if (unreadCount > 0) {
      if (badge) {
        badge.classList.remove('hidden');
        if (badge.tagName === 'SPAN' && badge.classList.contains('min-w-[18px]')) {
          badge.textContent = unreadCount > 9 ? '9+' : String(unreadCount);
        }
      }
      if (countBadge) {
        countBadge.textContent = `${unreadCount} tin mới`;
        countBadge.classList.remove('hidden');
      }
    } else {
      if (badge) badge.classList.add('hidden');
      if (countBadge) countBadge.classList.add('hidden');
    }

    if (notifications.length === 0) {
      listEl.innerHTML = `
        <div id="notification-empty" class="py-8 px-4 text-center text-slate-400 space-y-2">
          <i class="fa-regular fa-bell-slash text-2xl text-slate-300"></i>
          <p class="text-xs">Hiện chưa có thông báo mới</p>
        </div>
      `;
      return;
    }

    listEl.innerHTML = `
      <div class="px-3.5 py-2 bg-slate-50/80 flex items-center justify-between text-[11px] border-b border-slate-100">
        <button type="button" onclick="window.markAllNotificationsRead && window.markAllNotificationsRead()" class="text-[#0057B7] hover:underline font-semibold cursor-pointer">Đánh dấu đã đọc</button>
        <button type="button" onclick="window.clearAllNotifications && window.clearAllNotifications()" class="text-slate-400 hover:text-rose-600 font-medium cursor-pointer">Xóa tất cả</button>
      </div>
    ` + notifications.map(item => `
      <div class="p-3.5 hover:bg-blue-50/50 transition-colors cursor-pointer flex gap-3 items-start ${item.unread !== false ? 'bg-blue-50/25' : ''}" onclick="window.onHeaderNotificationClick && window.onHeaderNotificationClick('${item.id}', '${item.maDH || ''}')">
        <div class="w-8 h-8 rounded-lg ${item.bgClass || 'bg-blue-100'} ${item.colorClass || 'text-[#2563EB]'} flex items-center justify-center flex-shrink-0 text-xs shadow-2xs">
          <i class="${item.icon || 'fa-solid fa-bell'}"></i>
        </div>
        <div class="space-y-0.5 min-w-0 flex-1">
          <div class="flex items-center justify-between gap-2">
            <p class="font-bold text-slate-900 text-xs truncate">${item.title || ''}</p>
            ${item.unread !== false ? '<span class="w-2 h-2 rounded-full bg-rose-500 flex-shrink-0"></span>' : ''}
          </div>
          <p class="text-[11px] text-slate-600 leading-snug line-clamp-2">${item.message || ''}</p>
          <span class="text-[10px] text-slate-400 block">${item.time || 'Vừa xong'}</span>
        </div>
      </div>
    `).join('');
  }

  window.markAllNotificationsRead = function () {
    const list = getStoredNotifications(isAdminPage).map(n => ({ ...n, unread: false }));
    saveStoredNotifications(list, isAdminPage);
    renderHeaderNotifications();
  };

  window.clearAllNotifications = function () {
    saveStoredNotifications([], isAdminPage);
    renderHeaderNotifications();
  };

  window.onHeaderNotificationClick = function (id, maDH) {
    const list = getStoredNotifications(isAdminPage).map(n => n.id === id ? { ...n, unread: false } : n);
    saveStoredNotifications(list, isAdminPage);
    renderHeaderNotifications();

    const pop = document.getElementById('notification-popover');
    if (pop) pop.classList.add('hidden');

    if (isAdminPage) {
      if (typeof window.switchSection === 'function') {
        window.switchSection('view-donhang');
      }
      if (typeof window.renderDonHangTable === 'function') {
        window.renderDonHangTable();
      }
    } else {
      if (window.location.pathname.includes('cart.html')) {
        if (typeof window.switchCartTab === 'function') window.switchCartTab('orders');
        if (maDH && typeof window.openTrackingModal === 'function') {
          window.openTrackingModal(maDH);
        }
      } else {
        window.location.href = getOrdersPageHref();
      }
    }
  };

  window.toggleNotificationPopover = function (e) {
    if (e && e.stopPropagation) e.stopPropagation();
    const pop = document.getElementById('notification-popover');
    if (!pop) return;
    pop.classList.toggle('hidden');
    if (!pop.classList.contains('hidden')) {
      renderHeaderNotifications();
    }
  };

  if (window.NotificationCenter) {
    window.NotificationCenter.toggle = window.toggleNotificationPopover;
    window.NotificationCenter.render = renderHeaderNotifications;
  } else {
    window.NotificationCenter = {
      toggle: window.toggleNotificationPopover,
      setBadge: function () { renderHeaderNotifications(); },
      render: renderHeaderNotifications
    };
  }

  // Tự động tìm Hub URL SignalR
  function getHubUrl() {
    let apiBase = window.API_BASE || '';
    if (!apiBase) {
      const isLocal = window.location.protocol === 'file:' || window.location.port === '5500' || window.location.port === '5501' || window.location.port === '3000';
      apiBase = isLocal ? 'http://localhost:5132/api' : '/api';
    }
    const root = apiBase.replace(/\/api\/?$/, '');
    return (root || '') + '/hubs/order';
  }

  // Quản lý deduplicate thông báo realtime
  const recentStatusUpdates = new Map();
  const recentNewOrders = new Set();

  // Banner thông báo realtime nổi bật trên màn hình User
  function showUserRealtimeOrderBanner(data) {
    if (isAdminPage) return;
    let container = document.getElementById('signalr-user-toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'signalr-user-toast-container';
      container.className = 'fixed top-5 right-5 z-[99999] flex flex-col gap-3 max-w-[420px] w-full pointer-events-none p-3';
      document.body.appendChild(container);
    }

    if (data && data.maDH) {
      const existingToast = container.querySelector(`[data-order-toast="${data.maDH}"]`);
      if (existingToast) existingToast.remove();
    }

    const toast = document.createElement('div');
    toast.dataset.orderToast = data.maDH || '';
    toast.className = 'pointer-events-auto bg-white rounded-2xl shadow-2xl border-2 border-emerald-500 p-4 transition-all duration-300 transform translate-y-2 opacity-0 flex items-start gap-3.5';

    let iconBg = 'bg-emerald-500';
    let icon = 'fa-circle-check';
    let title = 'ĐƠN HÀNG ĐÃ ĐƯỢC XÁC NHẬN!';
    let badgeColor = 'bg-emerald-100 text-emerald-800';

    if (data.trangThai === 'dang_giao') {
      iconBg = 'bg-blue-600';
      icon = 'fa-truck-fast';
      title = 'ĐƠN HÀNG ĐANG ĐƯỢC GIAO!';
      badgeColor = 'bg-blue-100 text-blue-800';
    } else if (data.trangThai === 'hoan_thanh') {
      iconBg = 'bg-emerald-600';
      icon = 'fa-box-open';
      title = 'ĐƠN HÀNG ĐÃ HOÀN TẤT!';
      badgeColor = 'bg-emerald-100 text-emerald-800';
    } else if (data.trangThai === 'da_huy') {
      iconBg = 'bg-rose-500';
      icon = 'fa-ban';
      title = 'ĐƠN HÀNG ĐÃ HỦY!';
      badgeColor = 'bg-rose-100 text-rose-800';
    }

    const ordersHref = getOrdersPageHref();
    toast.innerHTML = `
      <div class="w-10 h-10 rounded-xl ${iconBg} text-white flex items-center justify-center flex-shrink-0 shadow-md">
        <i class="fa-solid ${icon} text-lg"></i>
      </div>
      <div class="flex-1 min-w-0">
        <div class="flex items-center justify-between gap-2 mb-1">
          <span class="text-xs font-black tracking-wide text-slate-800 uppercase">${title}</span>
          <span class="text-[10px] font-mono font-bold px-2 py-0.5 rounded-full ${badgeColor}">${data.maDH || ''}</span>
        </div>
        <p class="text-xs sm:text-[13px] text-slate-700 leading-snug font-medium mb-1.5">${data.message || data.statusText || 'Đơn hàng của bạn đã được Admin cập nhật trạng thái!'}</p>
        <div class="flex items-center justify-between text-[11px] text-slate-400">
          <span>Thời gian: ${formatVNTime(new Date())}</span>
          <a href="${ordersHref}" onclick="if(window.switchCartTab){window.switchCartTab('orders');}" class="text-[#0057B7] hover:underline font-bold">Xem đơn hàng &rarr;</a>
        </div>
      </div>
      <button type="button" class="text-slate-400 hover:text-slate-600 p-1 -mr-1 -mt-1 cursor-pointer">
        <i class="fa-solid fa-xmark text-sm"></i>
      </button>
    `;

    const closeBtn = toast.querySelector('button');
    if (closeBtn) {
      closeBtn.onclick = () => {
        toast.classList.add('opacity-0', 'translate-x-4');
        setTimeout(() => toast.remove(), 300);
      };
    }

    container.appendChild(toast);
    requestAnimationFrame(() => {
      toast.classList.remove('opacity-0', 'translate-y-2');
    });

    setTimeout(() => {
      if (toast.parentElement) {
        toast.classList.add('opacity-0', 'translate-x-4');
        setTimeout(() => toast.remove(), 300);
      }
    }, 9000);
  }

  // Banner thông báo đơn hàng mới nổi bật trên màn hình Admin
  function showAdminRealtimeOrderBanner(order, isCancel = false) {
    if (!isAdminPage || !order || !order.maDH) return;
    let container = document.getElementById('signalr-admin-toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'signalr-admin-toast-container';
      container.className = 'fixed top-16 right-5 z-[99999] flex flex-col gap-3 max-w-[430px] w-full pointer-events-none p-3';
      document.body.appendChild(container);
    }

    const existing = container.querySelector(`[data-admin-order-toast="${order.maDH}_${isCancel ? 'cancel' : 'new'}"]`);
    if (existing) existing.remove();

    const ten = order.tenNguoiNhan || 'Khách hàng';
    const sdt = order.sdtNguoiNhan || order.sdt || '';
    const tien = Number(order.tongTien || 0).toLocaleString('vi-VN') + ' ₫';

    const toast = document.createElement('div');
    toast.dataset.adminOrderToast = `${order.maDH}_${isCancel ? 'cancel' : 'new'}`;
    toast.className = `pointer-events-auto bg-white rounded-2xl shadow-2xl border-2 ${isCancel ? 'border-rose-500' : 'border-blue-600'} p-4 transition-all duration-300 transform translate-y-2 opacity-0 flex items-start gap-3.5`;

    toast.innerHTML = `
      <div class="w-10 h-10 rounded-xl ${isCancel ? 'bg-rose-500' : 'bg-blue-600'} text-white flex items-center justify-center flex-shrink-0 shadow-md">
        <i class="fa-solid ${isCancel ? 'fa-ban' : 'fa-cart-Arrow-down fa-bell'} text-lg"></i>
      </div>
      <div class="flex-1 min-w-0">
        <div class="flex items-center justify-between gap-2 mb-1">
          <span class="text-xs font-black tracking-wide ${isCancel ? 'text-rose-700' : 'text-blue-900'} uppercase">${isCancel ? 'KHÁCH HỦY ĐƠN HÀNG!' : 'ĐƠN ĐẶT HÀNG MỚI!'}</span>
          <span class="text-[10px] font-mono font-bold px-2 py-0.5 rounded-full ${isCancel ? 'bg-rose-100 text-rose-800' : 'bg-blue-100 text-blue-800'}">${order.maDH}</span>
        </div>
        <p class="text-xs sm:text-[13px] text-slate-700 leading-snug font-medium mb-2">
          ${isCancel
            ? (order.message || `Khách hàng <b>${ten}</b> vừa hủy đơn <b>#${order.maDH}</b>.`)
            : `Khách hàng <b>${ten}</b> ${sdt ? `(${sdt})` : ''} vừa đặt đơn hàng mới trị giá <b class="text-rose-600">${tien}</b>.`}
        </p>
        <div class="flex items-center justify-between text-[11px] text-slate-400">
          <span>${formatVNTime(order.ngayDat || new Date())}</span>
          <button type="button" class="btn-goto-order px-2.5 py-1 rounded-lg bg-blue-600 hover:bg-blue-700 text-white font-bold text-[11px] cursor-pointer shadow-xs">
            Xem & Duyệt ngay &rarr;
          </button>
        </div>
      </div>
      <button type="button" class="btn-close-toast text-slate-400 hover:text-slate-600 p-1 -mr-1 -mt-1 cursor-pointer">
        <i class="fa-solid fa-xmark text-sm"></i>
      </button>
    `;

    const gotoBtn = toast.querySelector('.btn-goto-order');
    if (gotoBtn) {
      gotoBtn.onclick = () => {
        if (typeof window.switchSection === 'function') window.switchSection('view-donhang');
        if (typeof window.renderDonHangTable === 'function') window.renderDonHangTable();
        toast.remove();
      };
    }

    const closeBtn = toast.querySelector('.btn-close-toast');
    if (closeBtn) {
      closeBtn.onclick = () => {
        toast.classList.add('opacity-0', 'translate-x-4');
        setTimeout(() => toast.remove(), 300);
      };
    }

    container.appendChild(toast);
    requestAnimationFrame(() => {
      toast.classList.remove('opacity-0', 'translate-y-2');
    });

    setTimeout(() => {
      if (toast.parentElement) {
        toast.classList.add('opacity-0', 'translate-x-4');
        setTimeout(() => toast.remove(), 300);
      }
    }, 10000);
  }

  // Xử lý thống nhất khi có đơn hàng mới gửi tới Admin
  function handleIncomingNewOrderEvent(order) {
    if (!order || !order.maDH) return;
    if (recentNewOrders.has(order.maDH)) return;
    recentNewOrders.add(order.maDH);
    setTimeout(() => recentNewOrders.delete(order.maDH), 10000);

    const ten = order.tenNguoiNhan || 'Khách hàng';
    const sdt = order.sdtNguoiNhan || '';
    const tien = Number(order.tongTien || 0).toLocaleString('vi-VN') + 'đ';

    // Ghi vào chuông thông báo của Admin
    addNotificationItem({
      id: `admin_new_${order.maDH}`,
      maDH: order.maDH,
      title: `Đơn hàng mới #${order.maDH}`,
      message: `${ten}${sdt ? ' (' + sdt + ')' : ''} vừa đặt đơn trị giá ${tien}.`,
      time: formatVNTime(order.ngayDat || new Date()),
      icon: 'fa-solid fa-cart-shopping',
      bgClass: 'bg-blue-100',
      colorClass: 'text-blue-700'
    }, true);

    if (isAdminPage) {
      playChime('new_order');
      showAdminRealtimeOrderBanner(order, false);
      window.dispatchEvent(new CustomEvent('samamobile:new_order', { detail: order }));
    }
  }

  // Xử lý thống nhất khi trạng thái đơn hàng thay đổi (Admin duyệt/giao/hoàn tất/hủy hoặc User hủy)
  function handleIncomingOrderStatusEvent(data) {
    if (!data || !data.maDH || !data.trangThai) return;

    const dedupKey = `${data.maDH}_${data.trangThai}`;
    const now = Date.now();
    if (recentStatusUpdates.has(dedupKey) && (now - recentStatusUpdates.get(dedupKey) < 3500)) {
      return;
    }
    recentStatusUpdates.set(dedupKey, now);

    // Đồng bộ trạng thái vào localStorage (cả samamobile_user_orders và cellphones_online_orders)
    let belongsToCurrentUser = false;
    try {
      const rawUser = localStorage.getItem('samamobile_user_orders');
      if (rawUser) {
        const userList = JSON.parse(rawUser);
        if (Array.isArray(userList)) {
          const found = userList.find(o => o && o.maDH === data.maDH);
          if (found) {
            belongsToCurrentUser = true;
            found.trangThai = data.trangThai;
            if (data.soHD) found.soHD = data.soHD;
            localStorage.setItem('samamobile_user_orders', JSON.stringify(userList));
          }
        }
      }
      const rawAdmin = localStorage.getItem('cellphones_online_orders');
      if (rawAdmin) {
        const adminList = JSON.parse(rawAdmin);
        if (Array.isArray(adminList)) {
          const foundAdmin = adminList.find(o => o && o.maDH === data.maDH);
          if (foundAdmin) {
            foundAdmin.trangThai = data.trangThai;
            if (data.soHD) foundAdmin.soHD = data.soHD;
            localStorage.setItem('cellphones_online_orders', JSON.stringify(adminList));
          }
        }
      }
    } catch (e) {}

    // Kiểm tra thêm theo SĐT user đang đăng nhập nếu đơn đặt từ thiết bị khác
    if (!belongsToCurrentUser) {
      try {
        const authRaw = localStorage.getItem('cellphones_auth_user') || localStorage.getItem('auth_user');
        if (authRaw && data.sdt) {
          const u = JSON.parse(authRaw);
          if (u && u.sdt && String(u.sdt).replace(/\D/g, '') === String(data.sdt).replace(/\D/g, '')) {
            belongsToCurrentUser = true;
          }
        }
      } catch (e) {}
    }

    const statusMeta = {
      'da_xac_nhan': {
        title: `Đơn #${data.maDH} đã được duyệt!`,
        defaultMsg: `Đơn đặt hàng #${data.maDH} đã được Admin xác nhận và đang chuẩn bị đóng gói.`,
        icon: 'fa-solid fa-circle-check',
        bgClass: 'bg-emerald-100',
        colorClass: 'text-emerald-700'
      },
      'dang_giao': {
        title: `Đơn #${data.maDH} đang giao hàng!`,
        defaultMsg: `Đơn đặt hàng #${data.maDH} đã xuất kho và đang trên đường giao đến bạn.`,
        icon: 'fa-solid fa-truck-fast',
        bgClass: 'bg-blue-100',
        colorClass: 'text-blue-700'
      },
      'hoan_thanh': {
        title: `Đơn #${data.maDH} đã hoàn tất!`,
        defaultMsg: `Đơn đặt hàng #${data.maDH} đã được giao thành công. Cảm ơn bạn đã mua sắm tại SamaMobile!`,
        icon: 'fa-solid fa-box-open',
        bgClass: 'bg-emerald-100',
        colorClass: 'text-emerald-700'
      },
      'da_huy': {
        title: `Đơn #${data.maDH} đã hủy`,
        defaultMsg: `Đơn đặt hàng #${data.maDH} đã chuyển sang trạng thái Đã hủy.`,
        icon: 'fa-solid fa-ban',
        bgClass: 'bg-rose-100',
        colorClass: 'text-rose-600'
      }
    }[data.trangThai] || {
      title: `Cập nhật đơn #${data.maDH}`,
      defaultMsg: `Đơn hàng #${data.maDH} vừa được cập nhật trạng thái mới.`,
      icon: 'fa-solid fa-bell',
      bgClass: 'bg-blue-100',
      colorClass: 'text-blue-700'
    };

    const msgText = data.message || data.statusText || statusMeta.defaultMsg;

    // Lưu vào chuông thông báo của User (nếu không phải do chính User tự hủy)
    if (data.actor !== 'user') {
      addNotificationItem({
        id: `user_status_${data.maDH}_${data.trangThai}`,
        maDH: data.maDH,
        trangThai: data.trangThai,
        title: statusMeta.title,
        message: msgText,
        time: formatVNTime(data.updatedAt || new Date()),
        icon: statusMeta.icon,
        bgClass: statusMeta.bgClass,
        colorClass: statusMeta.colorClass
      }, false);
    }

    // Nếu khách hàng hủy đơn, lưu vào chuông thông báo của Admin và hiện banner cho Admin
    if (data.actor === 'user' && data.trangThai === 'da_huy') {
      addNotificationItem({
        id: `admin_cancel_${data.maDH}`,
        maDH: data.maDH,
        trangThai: 'da_huy',
        title: `Khách hủy đơn #${data.maDH}`,
        message: msgText,
        time: formatVNTime(new Date()),
        icon: 'fa-solid fa-ban',
        bgClass: 'bg-rose-100',
        colorClass: 'text-rose-600'
      }, true);

      if (isAdminPage) {
        playChime('order_status');
        showAdminRealtimeOrderBanner({ maDH: data.maDH, message: msgText, sdt: data.sdt }, true);
      }
    }

    // Phát sự kiện để trang hiện tại cập nhật bảng đơn hàng
    window.dispatchEvent(new CustomEvent('samamobile:order_status_updated', { detail: data }));

    // Nếu đang ở trang User và Admin vừa cập nhật trạng thái -> Phát chuông + Hiện Banner nổi cho User
    if (!isAdminPage && data.actor !== 'user') {
      playChime('order_status');
      showUserRealtimeOrderBanner({
        ...data,
        message: msgText
      });
    }
  }

  // Lắng nghe BroadcastChannel liên tab (độ trễ 0ms giữa tab User và tab Admin)
  if (typeof BroadcastChannel !== 'undefined') {
    try {
      const bc = new BroadcastChannel('samamobile_orders');
      bc.onmessage = function (e) {
        if (!e.data) return;
        if (e.data.type === 'new_order' && e.data.order) {
          handleIncomingNewOrderEvent(e.data.order);
        } else if (e.data.type === 'order_status_updated' && e.data.maDH) {
          handleIncomingOrderStatusEvent(e.data);
        }
      };
    } catch (e) {}
  }

  // Lắng nghe sự kiện localStorage (đồng bộ dự phòng 100% giữa các tab cùng trình duyệt)
  window.addEventListener('storage', function (e) {
    try {
      if (e.key === 'samamobile_realtime_new_order' && e.newValue) {
        const parsed = JSON.parse(e.newValue);
        if (parsed && parsed.order) {
          handleIncomingNewOrderEvent(parsed.order);
        }
      } else if (e.key === 'samamobile_realtime_status_update' && e.newValue) {
        const parsed = JSON.parse(e.newValue);
        if (parsed && parsed.maDH) {
          handleIncomingOrderStatusEvent(parsed);
        }
      } else if (e.key === USER_NOTIF_KEY || e.key === ADMIN_NOTIF_KEY) {
        renderHeaderNotifications();
      }
    } catch (err) {}
  });

  // Quản lý kết nối SignalR
  let connection = null;
  let isConnecting = false;

  async function initSignalR() {
    renderHeaderNotifications();

    if (typeof signalR === 'undefined') {
      console.warn('SignalR library not loaded yet.');
      return null;
    }
    if (connection) return connection;

    const hubUrl = getHubUrl();
    console.log('Connecting to Order SignalR Hub at:', hubUrl);

    connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection.on('ReceiveNewOrder', handleIncomingNewOrderEvent);
    connection.on('OrderCreatedNotification', handleIncomingNewOrderEvent);

    connection.onreconnected(async function () {
      if (isAdminPage) {
        try { await connection.invoke('JoinAdminGroup'); } catch (e) {}
      }
    });

    connection.on('OrderStatusUpdated', function (data) {
      handleIncomingOrderStatusEvent(data);
    });

    connection.on('ReceiveOrderCancelled', function (data) {
      if (data && data.maDH) {
        handleIncomingOrderStatusEvent({ ...data, trangThai: 'da_huy', actor: 'user' });
      }
      window.dispatchEvent(new CustomEvent('samamobile:order_cancelled', { detail: data }));
    });

    try {
      isConnecting = true;
      await connection.start();
      isConnecting = false;
      console.log('Connected to Order SignalR Hub successfully!');

      if (isAdminPage) {
        await connection.invoke('JoinAdminGroup');
        console.log('Joined SignalR AdminGroup');
      } else {
        try {
          const raw = localStorage.getItem('samamobile_user_orders');
          if (raw) {
            const list = JSON.parse(raw);
            if (Array.isArray(list)) {
              list.forEach(o => {
                if (o.maDH) connection.invoke('JoinOrderGroup', o.maDH).catch(() => {});
                if (o.sdtNguoiNhan) connection.invoke('JoinCustomerGroup', o.sdtNguoiNhan).catch(() => {});
              });
            }
          }
        } catch (e) {}
      }
    } catch (err) {
      isConnecting = false;
      console.warn('Cannot connect to SignalR Hub (using BroadcastChannel/localStorage fallback):', err);
    }

    return connection;
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
      initSignalR();
      setTimeout(renderHeaderNotifications, 150);
    });
  } else {
    initSignalR();
    setTimeout(renderHeaderNotifications, 150);
  }

  window.addEventListener('load', () => {
    renderHeaderNotifications();
    const btn = document.getElementById('btn-notification-toggle');
    if (btn && !btn._hasNotifSync) {
      btn._hasNotifSync = true;
      btn.addEventListener('click', () => {
        setTimeout(renderHeaderNotifications, 10);
      });
    }
  });

  // Public API
  window.OrderSignalR = {
    getConnection: () => connection,
    playChime: playChime,
    showUserRealtimeOrderBanner: showUserRealtimeOrderBanner,
    showAdminRealtimeOrderBanner: showAdminRealtimeOrderBanner,
    renderHeaderNotifications: renderHeaderNotifications,
    addNotificationItem: addNotificationItem,
    notifyNewOrder: async function (orderData) {
      if (!orderData || !orderData.maDH) return;

      // Thêm thông báo xác nhận đặt hàng vào chuông của User
      const tien = Number(orderData.tongTien || 0).toLocaleString('vi-VN') + 'đ';
      addNotificationItem({
        id: `user_created_${orderData.maDH}`,
        maDH: orderData.maDH,
        trangThai: 'cho_xac_nhan',
        title: `Đặt hàng thành công #${orderData.maDH}`,
        message: `Đơn hàng (${tien}) đã được gửi tới Admin và đang chờ duyệt xác nhận.`,
        time: formatVNTime(orderData.ngayDat || new Date()),
        icon: 'fa-solid fa-bag-shopping',
        bgClass: 'bg-blue-100',
        colorClass: 'text-[#0057B7]'
      }, false);

      // Lưu vào chuông của Admin và phát qua localStorage + BroadcastChannel
      addNotificationItem({
        id: `admin_new_${orderData.maDH}`,
        maDH: orderData.maDH,
        title: `Đơn hàng mới #${orderData.maDH}`,
        message: `${orderData.tenNguoiNhan || 'Khách hàng'} (${orderData.sdtNguoiNhan || ''}) vừa đặt đơn trị giá ${tien}.`,
        time: formatVNTime(orderData.ngayDat || new Date()),
        icon: 'fa-solid fa-cart-shopping',
        bgClass: 'bg-blue-100',
        colorClass: 'text-blue-700'
      }, true);

      try {
        localStorage.setItem('samamobile_realtime_new_order', JSON.stringify({
          order: orderData,
          ts: Date.now()
        }));
      } catch (e) {}

      try {
        if (typeof BroadcastChannel !== 'undefined') {
          const bc = new BroadcastChannel('samamobile_orders');
          bc.postMessage({ type: 'new_order', order: orderData });
          bc.close();
        }
      } catch (e) {}

      if (connection && connection.state === signalR.HubConnectionState.Connected) {
        try {
          await connection.invoke('NotifyNewOrder', orderData);
        } catch (e) {
          console.warn('Failed to send NotifyNewOrder via SignalR:', e);
        }
      }
    },
    notifyOrderStatusChanged: async function (maDH, trangThai, message, phone, actor = 'admin', soHD = null) {
      const payload = {
        type: 'order_status_updated',
        maDH,
        trangThai,
        message,
        statusText: message,
        sdt: phone || '',
        soHD: soHD || null,
        actor: actor || 'admin',
        updatedAt: new Date().toISOString(),
        ts: Date.now()
      };

      // Lưu vào chuông thông báo của phía đối diện ngay lập tức
      handleIncomingOrderStatusEvent(payload);

      // Ghi nhận vào chuông của Admin khi Admin thao tác duyệt/giao/hoàn tất
      if (actor === 'admin') {
        addNotificationItem({
          id: `admin_action_${maDH}_${trangThai}`,
          maDH,
          trangThai,
          title: `Đã cập nhật đơn #${maDH}`,
          message: message || `Đã chuyển trạng thái đơn #${maDH} sang ${trangThai}.`,
          time: formatVNTime(new Date()),
          icon: 'fa-solid fa-clipboard-check',
          bgClass: 'bg-emerald-100',
          colorClass: 'text-emerald-700'
        }, true);
      }

      try {
        localStorage.setItem('samamobile_realtime_status_update', JSON.stringify(payload));
      } catch (e) {}

      try {
        if (typeof BroadcastChannel !== 'undefined') {
          const bc = new BroadcastChannel('samamobile_orders');
          bc.postMessage(payload);
          bc.close();
        }
      } catch (e) {}

      if (connection && connection.state === signalR.HubConnectionState.Connected) {
        try {
          await connection.invoke('NotifyOrderStatusChanged', maDH, trangThai, message, phone || '');
        } catch (e) {
          console.warn('Failed to send NotifyOrderStatusChanged via SignalR:', e);
        }
      }
    },
    joinOrderGroup: function (maDH) {
      if (connection && connection.state === signalR.HubConnectionState.Connected && maDH) {
        connection.invoke('JoinOrderGroup', maDH).catch(() => {});
      }
    },
    joinCustomerGroup: function (phone) {
      if (connection && connection.state === signalR.HubConnectionState.Connected && phone) {
        connection.invoke('JoinCustomerGroup', phone).catch(() => {});
      }
    }
  };

})();
