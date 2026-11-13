(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.CellphoneSLookup = root.Lookup = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  function showNotification(message, type = 'info') {
    if (window.CellphoneSCart && typeof window.CellphoneSCart.showToast === 'function') {
      window.CellphoneSCart.showToast(message, type);
      return;
    }
    if (typeof window.showToast === 'function') {
      window.showToast(message, type);
      return;
    }

    let container = document.getElementById('cellphones-toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'cellphones-toast-container';
      container.className = 'fixed top-5 right-5 z-50 flex flex-col gap-2 pointer-events-none';
      document.body.appendChild(container);
    }

    const bgMap = {
      success: 'bg-[#22C55E] text-white',
      warning: 'bg-[#F59E0B] text-slate-900',
      error: 'bg-[#EF4444] text-white',
      info: 'bg-[#2563EB] text-white'
    };
    const iconMap = {
      success: 'fa-circle-check',
      warning: 'fa-triangle-exclamation',
      error: 'fa-circle-xmark',
      info: 'fa-circle-info'
    };

    const toast = document.createElement('div');
    toast.className = `flex items-center gap-3 px-4 py-3 rounded-xl shadow-xl pointer-events-auto transition-all transform duration-300 translate-y-[-20px] opacity-0 ${bgMap[type] || bgMap.info}`;
    toast.innerHTML = `
      <i class="fa-solid ${iconMap[type] || iconMap.info} text-lg"></i>
      <span class="text-xs sm:text-sm font-semibold">${message}</span>
    `;

    container.appendChild(toast);
    requestAnimationFrame(() => toast.classList.remove('translate-y-[-20px]', 'opacity-0'));
    setTimeout(() => {
      toast.classList.add('opacity-0', 'translate-y-[-20px]');
      setTimeout(() => toast.remove(), 300);
    }, 3500);
  }

  function formatDate(isoStr) {
    if (!isoStr) return '-';
    try {
      const d = new Date(isoStr);
      if (isNaN(d.getTime())) return String(isoStr).substring(0, 10);
      return d.toLocaleDateString('vi-VN');
    } catch {
      return String(isoStr).substring(0, 10);
    }
  }

  function formatMoney(amount) {
    return Number(amount || 0).toLocaleString('vi-VN') + ' ₫';
  }

  // === Giai đoạn 2: Validate & Normalize SĐT ===

  function normalizePhone(phone) {
    // Bỏ khoảng trắng, dấu chấm, gạch; đổi +84/84 đầu thành 0
    let p = phone.replace(/[\s.\-]/g, '');
    if (p.startsWith('+84')) p = '0' + p.slice(3);
    else if (p.startsWith('84') && p.length === 11) p = '0' + p.slice(2);
    return p;
  }

  function validatePhone(phone) {
    return /^0[0-9]{9}$/.test(phone);
  }

  // === Giai đoạn 3: Rate Limit phía client (bảo vệ brute-force, bản thật phải làm ở server) ===
  const RL_KEY = 'cellphones_lookup_rl';
  const RL_MAX = 5;                    // sai tối đa 5 lần
  const RL_WINDOW = 10 * 60 * 1000;   // 10 phút

  function checkRateLimit() {
    try {
      const raw = localStorage.getItem(RL_KEY);
      if (!raw) return { blocked: false };
      const rl = JSON.parse(raw);
      if (Date.now() > rl.windowStart + RL_WINDOW) {
        localStorage.removeItem(RL_KEY);
        return { blocked: false };
      }
      if (rl.count >= RL_MAX) {
        const remaining = Math.ceil((rl.windowStart + RL_WINDOW - Date.now()) / 60000);
        return { blocked: true, remainingMinutes: remaining };
      }
      return { blocked: false };
    } catch { return { blocked: false }; }
  }

  function recordFailedAttempt() {
    try {
      const raw = localStorage.getItem(RL_KEY);
      let rl = raw ? JSON.parse(raw) : { count: 0, windowStart: Date.now() };
      if (Date.now() > rl.windowStart + RL_WINDOW) { rl = { count: 0, windowStart: Date.now() }; }
      rl.count++;
      localStorage.setItem(RL_KEY, JSON.stringify(rl));
    } catch {}
  }

  function clearRateLimit() {
    try { localStorage.removeItem(RL_KEY); } catch {}
  }

  // === Giai đoạn 4: OTP Flow helpers ===
  const OTP_TOKEN_KEY = 'cellphones_otp_token';

  async function requestOtp(phone) {
    const apiBase = window.API_BASE || 'http://localhost:5132/api';
    const res = await fetch(`${apiBase}/otp/request`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ sdt: normalizePhone(phone) })
    });
    if (!res.ok) {
      const json = await res.json().catch(() => ({}));
      throw new Error(json.message || 'Không thể gửi OTP. Vui lòng thử lại.');
    }
    return await res.json(); // { message, code (nếu demo mode) }
  }

  async function verifyOtp(phone, otp) {
    const apiBase = window.API_BASE || 'http://localhost:5132/api';
    const res = await fetch(`${apiBase}/otp/verify`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ sdt: normalizePhone(phone), otp })
    });
    const json = await res.json().catch(() => ({}));
    if (!res.ok) throw new Error(json.message || 'Mã OTP không hợp lệ hoặc đã hết hạn.');
    return json; // { token, expiresAt }
  }

  async function fetchOrdersByToken(token) {
    const apiBase = window.API_BASE || 'http://localhost:5132/api';
    const res = await fetch(`${apiBase}/orders/by-phone?token=${encodeURIComponent(token)}`);
    if (!res.ok) return [];
    return await res.json();
  }

  async function fetchWarrantyByImei(imei) {
    if (window.api && typeof window.api.get === 'function') {
      return await window.api.get(`/baohanh/imei/${encodeURIComponent(imei)}`);
    }
    const apiBase = window.API_BASE || '/api';
    const res = await fetch(`${apiBase}/baohanh/imei/${encodeURIComponent(imei)}`);
    if (!res.ok) {
      let errMsg = 'Không tìm thấy thông tin bảo hành cho IMEI này.';
      try {
        const json = await res.json();
        if (json.message) errMsg = json.message;
      } catch {}
      throw new Error(errMsg);
    }
    return await res.json();
  }

  async function fetchOrderDetails(maDH, phone) {
    const path = `/orders/${encodeURIComponent(maDH)}?sdt=${encodeURIComponent(phone)}`;
    if (window.api && typeof window.api.get === 'function') {
      return await window.api.get(path);
    }
    const apiBase = window.API_BASE || '/api';
    const res = await fetch(`${apiBase}${path}`);
    if (!res.ok) {
      let errMsg = res.status === 429
        ? 'Bạn thao tác quá nhiều, vui lòng thử lại sau ít phút.'
        : 'Không tìm thấy đơn hàng phù hợp.';
      try {
        const json = await res.json();
        if (json.message) errMsg = json.message;
      } catch {}
      const err = new Error(errMsg);
      err.status = res.status;
      throw err;
    }
    return await res.json();
  }

  async function fetchOrdersByPhone(phone) {
    const path = `/orders/phone/${encodeURIComponent(phone)}`;
    if (window.api && typeof window.api.get === 'function') {
      return await window.api.get(path);
    }
    const apiBase = window.API_BASE || '/api';
    const res = await fetch(`${apiBase}${path}`);
    if (!res.ok) return [];
    return await res.json();
  }

  function renderWarrantyCard(dto, imei) {
    const isHetHan = dto.trangThai === 'het_han' || (dto.soNgayConLai !== undefined && dto.soNgayConLai < 0);
    const isDangSua = dto.trangThai === 'dang_sua';

    let statusBadge = `<span class="px-2.5 py-1 rounded-full font-bold text-xs bg-[#22C55E]/15 text-[#22C55E] border border-[#22C55E]/30"><i class="fa-solid fa-shield-check mr-1"></i> Còn hạn</span>`;
    if (isHetHan) {
      statusBadge = `<span class="px-2.5 py-1 rounded-full font-bold text-xs bg-[#EF4444]/15 text-[#EF4444] border border-[#EF4444]/30"><i class="fa-solid fa-circle-xmark mr-1"></i> Hết hạn</span>`;
    } else if (isDangSua) {
      statusBadge = `<span class="px-2.5 py-1 rounded-full font-bold text-xs bg-[#F59E0B]/15 text-[#F59E0B] border border-[#F59E0B]/30"><i class="fa-solid fa-wrench mr-1"></i> Đang sửa chữa</span>`;
    }

    const remainingText = isHetHan
      ? '<span class="text-[#EF4444] font-bold">Đã hết hạn bảo hành</span>'
      : (dto.soNgayConLai !== undefined
          ? `<span class="text-[#22C55E] font-bold">Còn ${dto.soNgayConLai} ngày</span>`
          : '<span class="text-[#22C55E] font-bold">Đang hiệu lực</span>');

    // Bảo mật: DTO không chứa tên khách hàng hoặc SĐT
    return `
      <div class="p-4 rounded-2xl bg-white border border-[#E5E7EB] shadow-md text-xs space-y-3">
        <div class="flex items-center justify-between border-b border-slate-100 pb-2.5">
          <div>
            <span class="font-extrabold text-[#111827] text-sm">${dto.tenHang || 'Thiết bị di động'}</span>
            <div class="text-[10px] text-slate-400 font-mono mt-0.5">IMEI: ${dto.soIMEI || imei}</div>
          </div>
          ${statusBadge}
        </div>

        <div class="grid grid-cols-2 gap-2.5 text-slate-600">
          <div>Ngày kích hoạt: <strong class="text-[#111827] block font-mono">${formatDate(dto.ngayBD)}</strong></div>
          <div>Ngày hết hạn: <strong class="text-[#111827] block font-mono">${formatDate(dto.ngayKT)}</strong></div>
          <div>Tình trạng: <span class="block">${remainingText}</span></div>
          <div>Chính sách: <span class="text-[#111827] font-semibold block">1 đổi 1 trong 30 ngày</span></div>
        </div>

        <div class="p-2.5 rounded-xl bg-blue-50/60 text-[#1E3A8A] text-[11px] flex items-center gap-2 border border-blue-100">
          <i class="fa-solid fa-shield-halved text-[#2563EB]"></i>
          <span>Bảo hành chính hãng toàn quốc tại hệ thống CellphoneS - 241 Lê Văn Việt.</span>
        </div>
      </div>
    `;
  }

  // Giai đoạn 2: renderOrderCard nâng cấp — hiển thị masked info, timeline, danh sách SP
  function renderOrderCard(order) {
    const statusMap = {
      cho_thanh_toan: { label: 'Chờ thanh toán', color: 'bg-orange-100 text-orange-800', icon: 'fa-qrcode' },
      cho_xac_nhan: { label: 'Chờ xác nhận', color: 'bg-amber-100 text-amber-800', icon: 'fa-clock' },
      cho_tu_duyet: { label: 'Chờ xác nhận', color: 'bg-amber-100 text-amber-800', icon: 'fa-clock' },
      can_kiem_tra: { label: 'Chờ xác nhận', color: 'bg-amber-100 text-amber-800', icon: 'fa-clock' },
      da_xac_nhan:  { label: 'Đã xác nhận',  color: 'bg-slate-100 text-slate-800', icon: 'fa-circle-check' },
      cho_lay_hang: { label: 'Chờ lấy hàng', color: 'bg-slate-100 text-slate-800', icon: 'fa-boxes-packing' },
      dang_giao:    { label: 'Đang giao hàng', color: 'bg-indigo-100 text-indigo-800', icon: 'fa-truck' },
      da_giao:      { label: 'Đã giao hàng',  color: 'bg-emerald-100 text-emerald-800', icon: 'fa-box-open' },
      hoan_thanh:   { label: 'Hoàn thành',    color: 'bg-emerald-100 text-emerald-800', icon: 'fa-box-open' },
      da_huy:       { label: 'Đã hủy',        color: 'bg-rose-100 text-rose-800',   icon: 'fa-ban' },
      loi_van_don:  { label: 'Lỗi vận chuyển', color: 'bg-rose-100 text-rose-800', icon: 'fa-triangle-exclamation' }
    };
    const st = statusMap[order.trangThai] || statusMap.cho_xac_nhan;

    // Timeline: dùng statusHistory nếu có, ngược lại chỉ hiển thị trạng thái hiện tại
    let timelineHtml = '';
    if (order.statusHistory && order.statusHistory.length > 0) {
      const steps = order.statusHistory;
      timelineHtml = `
        <div class="border-t border-slate-100 pt-2">
          <div class="font-semibold text-slate-700 mb-2">Lịch sử trạng thái:</div>
          <ol class="relative border-l border-slate-200 ml-2 space-y-2">
            ${steps.map(s => {
              const sInfo = statusMap[s.trangThai] || { label: s.trangThai, color: 'bg-slate-100 text-slate-700', icon: 'fa-circle' };
              return `
                <li class="ml-3">
                  <span class="absolute -left-1.5 flex items-center justify-center w-3 h-3 rounded-full ${sInfo.color} ring-2 ring-white">
                    <i class="fa-solid ${sInfo.icon} text-[6px]"></i>
                  </span>
                  <div class="flex items-center gap-2 text-[11px]">
                    <span class="font-semibold text-slate-700">${sInfo.label}</span>
                    <span class="text-slate-400">${s.thoiGian ? new Date(s.thoiGian).toLocaleString('vi-VN') : ''}</span>
                  </div>
                  ${s.ghiChu ? `<div class="text-[10px] text-slate-500">${s.ghiChu}</div>` : ''}
                </li>`;
            }).join('')}
          </ol>
        </div>`;
    }

    // Thông tin giao hàng (backend đã mask sẵn)
    const tenMask   = order.tenNguoiNhanMask || order.tenNguoiNhan || 'Khách hàng';
    const sdtMask   = order.sdtMask   || order.sdtNguoiNhan || '-';
    const emailMask = order.emailMask || order.email || null;
    const diaChiMask = order.diaChiMask || order.diaChiGiao || 'Nhận tại cửa hàng';

    // Danh sách sản phẩm: tên, số lượng, giá, thành tiền
    let itemsHtml = '';
    if (order.items && order.items.length > 0) {
      itemsHtml = `
        <div class="border-t border-slate-100 pt-2 space-y-1">
          <div class="font-semibold text-slate-700 mb-1">Sản phẩm:</div>
          ${order.items.map(it => `
            <div class="flex justify-between items-center text-[11px] text-slate-600">
              <div class="min-w-0 mr-2">
                <span class="font-medium text-navy block truncate">${it.tenHang || it.maHang}</span>
                <span class="text-[10px] text-slate-400">SL: ${it.soLuong} × ${formatMoney(it.donGia)}</span>
              </div>
              <span class="font-semibold whitespace-nowrap">${formatMoney(it.thanhTien || (it.donGia * it.soLuong))}</span>
            </div>
          `).join('')}
        </div>`;
    }

    // Thông tin vận chuyển (Giai đoạn 6)
    let shippingHtml = '';
    const sp = order.shipping;
    if (!sp || sp.carrier === 'NONE') {
      shippingHtml = `
        <div class="border-t border-slate-100 pt-2">
          <div class="font-semibold text-slate-700 mb-1">Vận chuyển:</div>
          <div class="p-2.5 rounded-xl bg-slate-50 border border-slate-200 text-[11px] text-slate-500">
            <i class="fa-solid fa-box mr-1 text-slate-400"></i>Đơn hàng đang được chuẩn bị, chưa bàn giao cho đơn vị vận chuyển.
          </div>
        </div>`;
    } else {
      const carrierLabel = sp.carrier === 'SPX' ? 'SPX Express' : (sp.carrier === 'GHN' ? 'Giao Hàng Nhanh (GHN)' : sp.carrier);
      const trackBtnLabel = sp.carrier === 'SPX' ? 'Tra cứu trên SPX' : 'Xem trên trang hãng';
      const eventsList = (sp.events && sp.events.length > 0)
        ? `<ol class="relative border-l border-blue-200 ml-2 mt-2 space-y-2">
            ${sp.events.map(ev => `
              <li class="ml-3">
                <span class="absolute -left-1.5 w-3 h-3 rounded-full bg-blue-100 text-[#2563EB] flex items-center justify-center ring-2 ring-white">
                  <i class="fa-solid fa-circle text-[5px]"></i>
                </span>
                <div class="text-[11px] font-semibold text-slate-700">${ev.description || ev.status}</div>
                <div class="text-[10px] text-slate-400">${ev.time ? new Date(ev.time).toLocaleString('vi-VN') : ''}${ev.location ? ' · ' + ev.location : ''}</div>
              </li>
            `).join('')}
          </ol>`
        : '';

      shippingHtml = `
        <div class="border-t border-slate-100 pt-2 space-y-2">
          <div class="flex items-center justify-between">
            <span class="font-semibold text-slate-700">Vận chuyển:</span>
            <span class="px-2 py-0.5 rounded-full text-[10px] font-bold bg-blue-50 text-[#1E3A8A] border border-blue-200">${carrierLabel}</span>
          </div>
          <div class="p-2.5 rounded-xl bg-slate-50 border border-slate-200 space-y-1.5 text-[11px]">
            <div class="flex items-center justify-between gap-2">
              <span class="text-slate-500">Mã vận đơn:</span>
              <div class="flex items-center gap-1.5">
                <strong class="font-mono text-[#1E3A8A]">${sp.trackingCode || '-'}</strong>
                ${sp.trackingCode ? `<button type="button" onclick="window.CellphoneSLookup.copyTracking('${sp.trackingCode}')" class="px-1.5 py-0.5 rounded bg-blue-50 hover:bg-blue-100 text-[#2563EB] text-[10px] font-bold border border-blue-200 cursor-pointer"><i class="fa-regular fa-copy"></i></button>` : ''}
              </div>
            </div>
            <div class="flex items-center justify-between">
              <span class="text-slate-500">Trạng thái giao:</span>
              <strong class="text-[#111827]">${sp.shippingStatus || 'Đang xử lý'}</strong>
            </div>
            ${sp.expectedDelivery ? `
              <div class="flex items-center justify-between">
                <span class="text-slate-500">Dự kiến giao:</span>
                <span class="font-medium text-slate-700">${formatDate(sp.expectedDelivery)}</span>
              </div>` : ''}
            ${sp.trackingUrl ? `
              <div class="pt-1">
                <a href="${sp.trackingUrl}" target="_blank" rel="noopener noreferrer" class="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-lg bg-[#2563EB] hover:bg-[#1D4ED8] text-white font-bold text-[10px]">
                  <i class="fa-solid fa-arrow-up-right-from-square"></i>${trackBtnLabel}
                </a>
              </div>` : ''}
          </div>
          ${eventsList}
        </div>`;
    }

    return `
      <div class="p-4 rounded-2xl bg-white border border-[#E5E7EB] shadow-md text-xs space-y-2.5">
        <div class="flex items-center justify-between border-b border-slate-100 pb-2">
          <div>
            <span class="font-extrabold text-[#2563EB] font-mono text-sm">#${order.maDH}</span>
            <div class="text-[10px] text-slate-400">Ngày đặt: ${order.ngayDat ? order.ngayDat.substring(0, 16).replace('T', ' ') : 'Gần đây'}</div>
          </div>
          <span class="px-2.5 py-1 rounded-full font-bold text-xs ${st.color}"><i class="fa-solid ${st.icon} mr-1"></i>${st.label}</span>
        </div>

        <div class="grid grid-cols-2 gap-2 text-slate-600">
          <div>Người nhận: <strong class="text-[#111827] block">${tenMask}</strong></div>
          <div>Số điện thoại: <strong class="text-[#111827] block">${sdtMask}</strong></div>
          <div class="col-span-2">Địa chỉ: <span class="text-[#111827] font-medium block">${diaChiMask}</span></div>
          ${emailMask ? `<div class="col-span-2">Email: <span class="text-[#111827] font-medium block">${emailMask}</span></div>` : ''}
          <div>Phương thức: <span class="text-[#111827] font-medium block uppercase">${order.phuongThucThanhToan || 'COD'}</span></div>
        </div>

        ${itemsHtml}

        ${shippingHtml}

        ${timelineHtml}

        <div class="flex items-center justify-between pt-2 border-t border-slate-100">
          <span class="font-semibold text-slate-600">Tổng thanh toán:</span>
          <span class="font-bold text-base text-[#EF4444]">${formatMoney(order.tongTien)}</span>
        </div>
      </div>
    `;
  }

  class LookupManager {
    openModal(defaultTab = 'order') {
      const modal = document.getElementById('lookup-modal');
      if (modal) {
        modal.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
        this.switchTab(defaultTab);
      }

      // Giai đoạn 5: Tự điền mã đơn gần nhất từ localStorage
      try {
        const lastCode = localStorage.getItem('cellphones_last_order_code');
        const idInput = document.getElementById('lookup-order-id');
        if (lastCode && idInput && !idInput.value) {
          idInput.value = lastCode;
        }
      } catch(e) {}
    }

    closeModal() {
      const modal = document.getElementById('lookup-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    switchTab(tabId) {
      const btnOrder = document.getElementById('btn-lookup-tab-order');
      const btnWarranty = document.getElementById('btn-lookup-tab-warranty');
      const tabOrder = document.getElementById('lookup-tab-order');
      const tabWarranty = document.getElementById('lookup-tab-warranty');

      const activeCls = 'flex-1 py-3 text-center text-xs sm:text-sm font-bold border-b-2 border-[#2563EB] text-[#1E3A8A] cursor-pointer';
      const inactiveCls = 'flex-1 py-3 text-center text-xs sm:text-sm font-semibold text-slate-500 border-b-2 border-transparent hover:text-navy cursor-pointer';

      if (tabId === 'order') {
        if (btnOrder) btnOrder.className = activeCls;
        if (btnWarranty) btnWarranty.className = inactiveCls;
        if (tabOrder) tabOrder.classList.remove('hidden');
        if (tabWarranty) tabWarranty.classList.add('hidden');
      } else {
        if (btnWarranty) btnWarranty.className = activeCls;
        if (btnOrder) btnOrder.className = inactiveCls;
        if (tabWarranty) tabWarranty.classList.remove('hidden');
        if (tabOrder) tabOrder.classList.add('hidden');
      }
    }

    async lookupOrder() {
      const idInput    = document.getElementById('lookup-order-id');
      const phoneInput = document.getElementById('lookup-order-phone');
      const idErr      = document.getElementById('lookup-order-id-err');
      const phoneErr   = document.getElementById('lookup-order-phone-err');
      const resultBox  = document.getElementById('lookup-order-result');
      const btn        = document.getElementById('btn-lookup-order');

      // Helper: hiển thị/ẩn lỗi inline
      const setErr = (el, msg) => {
        if (!el) return;
        if (msg) { el.textContent = msg; el.classList.remove('hidden'); }
        else { el.textContent = ''; el.classList.add('hidden'); }
      };
      setErr(idErr, ''); setErr(phoneErr, '');

      // Giai đoạn 3: Kiểm tra rate limit phía client trước khi gọi API
      const rl = checkRateLimit();
      if (rl.blocked) {
        setErr(idErr, 'Bạn thao tác quá nhiều, vui lòng thử lại sau ít phút.');
        return;
      }

      const maDH  = (idInput  ? idInput.value  : '').trim().toUpperCase();
      const rawPhone = (phoneInput ? phoneInput.value : '').trim();
      const phone = normalizePhone(rawPhone);

      // Validate: mã đơn không được rỗng
      let hasErr = false;
      if (!maDH) {
        setErr(idErr, 'Vui lòng nhập mã đơn hàng.');
        hasErr = true;
      }
      // Validate SĐT phải hợp lệ 10 số bắt đầu 0
      if (!rawPhone) {
        setErr(phoneErr, 'Vui lòng nhập số điện thoại đặt hàng.');
        hasErr = true;
      } else if (!validatePhone(phone)) {
        setErr(phoneErr, 'Số điện thoại phải có 10 chữ số bắt đầu bằng 0.');
        hasErr = true;
      }
      if (hasErr) return;

      if (!resultBox) return;

      const originalBtnHtml = btn ? btn.innerHTML : '';
      if (btn) {
        btn.disabled = true;
        btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1.5"></i> Đang tra cứu...';
      }

      resultBox.classList.remove('hidden');
      resultBox.innerHTML = '<div class="text-center py-6 text-slate-400 text-xs"><i class="fa-solid fa-spinner fa-spin mr-2 text-[#2563EB]"></i> Đang tra cứu dữ liệu đơn hàng...</div>';

      try {
        const order = await fetchOrderDetails(maDH, phone);

        if (!order) {
          // Không tiết lộ mã/SĐT nào sai — thông báo chung
          resultBox.innerHTML = `
            <div class="p-4 rounded-xl bg-rose-50 border border-rose-200 text-rose-700 text-xs text-center">
              <i class="fa-solid fa-circle-exclamation text-lg mb-1 block"></i>
              Không tìm thấy đơn hàng phù hợp.
            </div>
          `;
          recordFailedAttempt(); // Giai đoạn 3: ghi nhận thất bại
          return;
        }

        clearRateLimit(); // Giai đoạn 3: thành công → reset rate limit
        resultBox.innerHTML = renderOrderCard(order);
      } catch (err) {
        const msg = err && err.status === 429
          ? (err.message || 'Bạn thao tác quá nhiều, vui lòng thử lại sau ít phút.')
          : 'Không tìm thấy đơn hàng phù hợp.';
        resultBox.innerHTML = `
          <div class="p-4 rounded-xl bg-rose-50 border border-rose-200 text-rose-700 text-xs text-center">
            <i class="fa-solid fa-circle-exclamation text-lg mb-1 block"></i>
            ${msg}
          </div>
        `;
        if (!err || err.status !== 429) {
          recordFailedAttempt(); // Giai đoạn 3: ghi nhận thất bại
        }
      } finally {
        if (btn) {
          btn.disabled = false;
          btn.innerHTML = originalBtnHtml;
        }
      }
    }

    async lookupWarranty() {
      const imeiInput = document.getElementById('lookup-imei-code');
      const resultBox = document.getElementById('lookup-warranty-result');
      const btn = document.querySelector('#lookup-tab-warranty button[type="button"]');

      const imei = (imeiInput ? imeiInput.value : '').trim().replace(/\s/g, '');

      if (!imei || !/^\d{15}$/.test(imei)) {
        showNotification('Số IMEI phải gồm đúng 15 chữ số!', 'error');
        if (imeiInput) imeiInput.focus();
        return;
      }

      if (!resultBox) return;

      const originalBtnHtml = btn ? btn.innerHTML : '';
      if (btn) {
        btn.disabled = true;
        btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1.5"></i> Đang tra cứu...';
      }

      resultBox.classList.remove('hidden');
      resultBox.innerHTML = '<div class="text-center py-6 text-slate-400 text-xs"><i class="fa-solid fa-spinner fa-spin mr-2 text-[#2563EB]"></i> Đang tra cứu CSDL bảo hành...</div>';

      try {
        const dto = await fetchWarrantyByImei(imei);
        resultBox.innerHTML = renderWarrantyCard(dto, imei);
      } catch (err) {
        resultBox.innerHTML = `
          <div class="p-4 rounded-xl bg-amber-50 border border-amber-200 text-amber-800 text-xs text-center">
            <i class="fa-solid fa-triangle-exclamation text-lg mb-1 block text-amber-600"></i>
            ${err.message || 'Không tìm thấy thông tin bảo hành cho IMEI này. Vui lòng kiểm tra lại.'}
          </div>
        `;
      } finally {
        if (btn) {
          btn.disabled = false;
          btn.innerHTML = originalBtnHtml;
        }
      }
    }

    // === Giai đoạn 4: OTP Flow ===

    showOtpFlow() {
      const orderForm = document.getElementById('lookup-order-form');
      const otpFlow   = document.getElementById('lookup-otp-flow');
      if (orderForm) orderForm.classList.add('hidden');
      if (otpFlow)   otpFlow.classList.remove('hidden');
      this.showOtpStep('a');
      // Reset các ô input OTP
      const phoneInput = document.getElementById('otp-phone-input');
      const codeInput  = document.getElementById('otp-code-input');
      if (phoneInput) phoneInput.value = '';
      if (codeInput)  codeInput.value  = '';
    }

    hideOtpFlow() {
      const orderForm = document.getElementById('lookup-order-form');
      const otpFlow   = document.getElementById('lookup-otp-flow');
      if (otpFlow)   otpFlow.classList.add('hidden');
      if (orderForm) orderForm.classList.remove('hidden');
    }

    showOtpStep(step) {
      // step: 'a' | 'b' | 'c'
      ['a', 'b', 'c'].forEach(s => {
        const el = document.getElementById(`otp-step-${s}`);
        if (el) el.classList.toggle('hidden', s !== step);
      });
    }

    startOtpCountdown() {
      let seconds = 60;
      const countdownEl = document.getElementById('otp-countdown');
      const resendBtn   = document.getElementById('btn-resend-otp');
      if (resendBtn) resendBtn.disabled = true;
      if (countdownEl) countdownEl.textContent = seconds;

      if (this._otpCountdownTimer) clearInterval(this._otpCountdownTimer);
      this._otpCountdownTimer = setInterval(() => {
        seconds--;
        if (countdownEl) countdownEl.textContent = seconds;
        if (seconds <= 0) {
          clearInterval(this._otpCountdownTimer);
          if (resendBtn) resendBtn.disabled = false;
        }
      }, 1000);
    }

    async submitOtpPhone() {
      const phoneInput = document.getElementById('otp-phone-input');
      const phoneErr   = document.getElementById('otp-phone-err');
      const btn        = document.getElementById('btn-send-otp');
      const rawPhone   = (phoneInput ? phoneInput.value : '').trim();
      const phone      = normalizePhone(rawPhone);

      const setErr = (el, msg) => {
        if (!el) return;
        if (msg) { el.textContent = msg; el.classList.remove('hidden'); }
        else { el.textContent = ''; el.classList.add('hidden'); }
      };
      setErr(phoneErr, '');

      if (!validatePhone(phone)) {
        setErr(phoneErr, 'Số điện thoại phải có 10 chữ số bắt đầu bằng 0.');
        return;
      }

      const originalHtml = btn ? btn.innerHTML : '';
      if (btn) { btn.disabled = true; btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1.5"></i> Đang gửi...'; }

      try {
        const result = await requestOtp(phone);
        this._otpPhone = phone;

        // Hiển thị số điện thoại đã nhập ở step B
        const displayEl = document.getElementById('otp-phone-display');
        if (displayEl) displayEl.textContent = rawPhone;

        // Banner demo nếu backend trả về code (chế độ dev)
        const demoBanner = document.getElementById('otp-demo-banner');
        if (demoBanner) {
          if (result && result.code) {
            demoBanner.innerHTML = `<i class="fa-solid fa-flask mr-1"></i><strong>Chế độ demo:</strong> Mã OTP của bạn là <strong class="text-lg tracking-widest">${result.code}</strong>. Trong thực tế mã sẽ được gửi qua SMS/Zalo ZNS.`;
            demoBanner.classList.remove('hidden');
          } else {
            demoBanner.classList.add('hidden');
          }
        }

        this.showOtpStep('b');
        this.startOtpCountdown();
      } catch (err) {
        setErr(phoneErr, err.message || 'Không thể gửi OTP. Vui lòng thử lại.');
      } finally {
        if (btn) { btn.disabled = false; btn.innerHTML = originalHtml; }
      }
    }

    async resendOtp() {
      await this.submitOtpPhone();
    }

    async submitOtpCode() {
      const codeInput = document.getElementById('otp-code-input');
      const codeErr   = document.getElementById('otp-code-err');
      const btn       = document.getElementById('btn-verify-otp');
      const otp       = (codeInput ? codeInput.value : '').trim();

      const setErr = (el, msg) => {
        if (!el) return;
        if (msg) { el.textContent = msg; el.classList.remove('hidden'); }
        else { el.textContent = ''; el.classList.add('hidden'); }
      };
      setErr(codeErr, '');

      if (!otp || !/^\d{6}$/.test(otp)) {
        setErr(codeErr, 'Mã OTP phải gồm đúng 6 chữ số.');
        return;
      }

      const originalHtml = btn ? btn.innerHTML : '';
      if (btn) { btn.disabled = true; btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1.5"></i> Đang xác nhận...'; }

      try {
        const result = await verifyOtp(this._otpPhone || '', otp);
        const token = result.token;
        // Lưu token OTP an toàn vào sessionStorage (không dùng localStorage tránh persist)
        try { sessionStorage.setItem(OTP_TOKEN_KEY, token); } catch(e) {}

        // Lấy danh sách đơn hàng theo token
        const orders = await fetchOrdersByToken(token);
        this.renderOtpOrderList(orders);
        this.showOtpStep('c');
      } catch (err) {
        setErr(codeErr, err.message || 'Mã OTP không hợp lệ hoặc đã hết hạn.');
      } finally {
        if (btn) { btn.disabled = false; btn.innerHTML = originalHtml; }
      }
    }

    renderOtpOrderList(orders) {
      const listEl = document.getElementById('otp-order-list');
      const detailEl = document.getElementById('otp-order-detail');
      if (detailEl) detailEl.classList.add('hidden');
      if (!listEl) return;

      if (!orders || orders.length === 0) {
        listEl.innerHTML = `
          <div class="p-3 rounded-xl bg-slate-50 border border-slate-200 text-slate-500 text-xs text-center">
            <i class="fa-solid fa-box-open text-lg mb-1 block text-slate-300"></i>
            Không tìm thấy đơn hàng nào cho số điện thoại này.
          </div>`;
        return;
      }

      const statusMap = {
        cho_thanh_toan: { label: 'Chờ thanh toán', color: 'bg-orange-100 text-orange-800' },
        cho_xac_nhan: { label: 'Chờ xác nhận', color: 'bg-amber-100 text-amber-800' },
        da_xac_nhan:  { label: 'Đã xác nhận',  color: 'bg-blue-100 text-blue-800' },
        dang_giao:    { label: 'Đang giao',     color: 'bg-indigo-100 text-indigo-800' },
        hoan_thanh:   { label: 'Hoàn thành',    color: 'bg-emerald-100 text-emerald-800' },
        da_huy:       { label: 'Đã hủy',        color: 'bg-rose-100 text-rose-800' }
      };

      listEl.innerHTML = orders.map(o => {
        const st = statusMap[o.trangThai] || statusMap.cho_xac_nhan;
        return `
          <button type="button" onclick="window.CellphoneSLookup.selectOtpOrder('${o.maDH}')"
            class="w-full flex items-center justify-between p-3 rounded-xl border border-slate-200 hover:border-[#2563EB] hover:bg-blue-50/40 transition-colors text-left mb-1.5">
            <div>
              <div class="font-bold text-[#2563EB] font-mono text-xs">#${o.maDH}</div>
              <div class="text-[10px] text-slate-500">${o.ngayDat ? o.ngayDat.substring(0, 10) : ''} · ${formatMoney(o.tongTien)}</div>
            </div>
            <span class="px-2 py-0.5 rounded-full text-[10px] font-bold ${st.color}">${st.label}</span>
          </button>`;
      }).join('');
    }

    async selectOtpOrder(maDH) {
      const detailEl = document.getElementById('otp-order-detail');
      if (!detailEl) return;

      detailEl.classList.remove('hidden');
      detailEl.innerHTML = '<div class="text-center py-4 text-xs text-slate-400"><i class="fa-solid fa-spinner fa-spin mr-2 text-[#2563EB]"></i> Đang tải chi tiết...</div>';

      try {
        let token = '';
        try { token = sessionStorage.getItem(OTP_TOKEN_KEY) || ''; } catch(e) {}
        const apiBase = window.API_BASE || 'http://localhost:5132/api';
        const res = await fetch(`${apiBase}/orders/${encodeURIComponent(maDH)}?token=${encodeURIComponent(token)}`);
        if (res.status === 401) throw new Error('Phiên tra cứu OTP đã hết hạn. Vui lòng xác thực lại.');
        if (!res.ok) throw new Error('Không thể tải chi tiết đơn hàng.');
        const order = await res.json();
        detailEl.innerHTML = renderOrderCard(order);
      } catch (err) {
        detailEl.innerHTML = `
          <div class="p-3 rounded-xl bg-rose-50 border border-rose-200 text-rose-700 text-xs text-center">
            ${err.message || 'Không thể tải chi tiết đơn hàng.'}
          </div>`;
      }
    }

    async copyTracking(code) {
      if (!code) return;
      try {
        await navigator.clipboard.writeText(code);
      } catch {
        const ta = document.createElement('textarea');
        ta.value = code;
        ta.style.position = 'fixed'; ta.style.opacity = '0';
        document.body.appendChild(ta); ta.select();
        document.execCommand('copy');
        document.body.removeChild(ta);
      }
      showNotification('Đã sao chép mã vận đơn', 'success');
    }

    openTradeInModal() {
      const modal = document.getElementById('tradein-modal');
      if (modal) {
        modal.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
      }
    }

    closeTradeInModal() {
      const modal = document.getElementById('tradein-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    submitTradeIn() {
      showNotification('Cảm ơn quý khách! CellphoneS sẽ liên hệ tư vấn.', 'success');
      this.closeTradeInModal();
    }
  }

  return new LookupManager();
}));
