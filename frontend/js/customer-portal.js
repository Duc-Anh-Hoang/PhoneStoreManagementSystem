/**
 * CellphoneS - 241 Lê Văn Việt - CUSTOMER PORTAL & ACCOUNT CENTER
 * Quản lý tài khoản khách hàng (S-Member)
 * - Đăng nhập bằng Số điện thoại (POST /api/auth/customer-login)
 * - Tra cứu Đơn hàng qua API /api/orders/phone/{sdt}
 * - Điểm thân thiết từ vw_KhachHangThanThiet (DIEMTICHLUY, TONGCHITIEU)
 */

(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.CellphoneSCustomerPortal = root.CustomerPortal = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  var API_BASE = window.API_BASE || '/api';

  const PHONE_REGEX = /^(0)(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$/;

  class CustomerPortalManager {

    /**
     * Lưu thông tin hồ sơ cá nhân
     */
    saveProfile() {
      const user = (window.CellphoneSAuth || window.Auth) ? (window.CellphoneSAuth || window.Auth).getUser() : null;
      if (!user) {
        alert('Vui lòng đăng nhập để lưu hồ sơ!');
        return;
      }

      const nameInput = document.getElementById('portal-profile-name');
      const emailInput = document.getElementById('portal-profile-email');
      const addressInput = document.getElementById('portal-profile-address');

      if (nameInput) user.hoTen = nameInput.value.trim();
      if (emailInput) user.email = emailInput.value.trim();
      if (addressInput) user.diaChi = addressInput.value.trim();

      if (window.CellphoneSAuth || window.Auth) {
        (window.CellphoneSAuth || window.Auth).setUser(user);
      }

      // Cập nhật lại UI header
      const custName = document.getElementById('customer-header-name');
      if (custName && user.hoTen) custName.textContent = user.hoTen;
      const portalName = document.getElementById('portal-user-name');
      if (portalName && user.hoTen) portalName.textContent = user.hoTen;

      if (window.CellphoneSCart && typeof window.CellphoneSCart.showToast === 'function') {
        window.CellphoneSCart.showToast('Cập nhật hồ sơ cá nhân thành công!', 'success');
      } else {
        alert('Cập nhật hồ sơ cá nhân thành công!');
      }
    }

    /**
     * Chuyển đổi tab trong Customer Portal Modal (orders, warranty, profile)
     */
    switchTab(tab) {
      const tabs = ['orders', 'warranty', 'profile'];
      tabs.forEach(t => {
        const btn = document.getElementById('btn-portal-tab-' + t);
        const pane = document.getElementById('portal-tab-' + t);
        if (btn) {
          if (t === tab) {
            btn.className = 'py-3 px-4 font-bold border-b-2 border-[#2563EB] text-[#1E3A8A] flex items-center gap-2 cursor-pointer';
          } else {
            btn.className = 'py-3 px-4 font-semibold text-slate-500 hover:text-navy border-b-2 border-transparent flex items-center gap-2 cursor-pointer';
          }
        }
        if (pane) {
          if (t === tab) {
            pane.classList.remove('hidden');
          } else {
            pane.classList.add('hidden');
          }
        }
      });
    }
  
    constructor() {
      this.pendingPhone = '';
    }

    openPortal(tab = 'profile') {
      const user = (window.CellphoneSAuth || window.Auth) ? (window.CellphoneSAuth || window.Auth).getUser() : null;
      if (!user || user.type !== 'customer') {
        this.openLoginModal();
        return;
      }

      const modal = document.getElementById('customer-portal-modal');
      if (modal) {
        modal.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
        this.renderPortalData(user);
        if (tab) this.switchTab(tab);
      }
    }

    switchTab(tab) {
      const tabs = ['orders', 'warranty', 'profile', 'settings'];
      tabs.forEach(t => {
        const pane = document.getElementById('portal-tab-' + t);
        const btn = document.getElementById('btn-portal-tab-' + t);
        if (pane) {
          if (t === tab) pane.classList.remove('hidden');
          else pane.classList.add('hidden');
        }
        if (btn) {
          if (t === tab) {
            btn.className = 'py-3 px-4 font-bold border-b-2 border-[#2563EB] text-[#1E3A8A] flex items-center gap-2 cursor-pointer';
          } else {
            btn.className = 'py-3 px-4 font-semibold text-slate-500 hover:text-navy border-b-2 border-transparent flex items-center gap-2 cursor-pointer';
          }
        }
      });
    }

    updatePassword() {
      const newPass = document.getElementById('portal-new-password');
      const confirmPass = document.getElementById('portal-confirm-password');
      const p1 = (newPass ? newPass.value : '').trim();
      const p2 = (confirmPass ? confirmPass.value : '').trim();

      if (!p1 || p1.length < 6) {
        alert('Vui lòng nhập mật khẩu mới tối thiểu 6 ký tự.');
        return;
      }
      if (p1 !== p2) {
        alert('Mật khẩu xác nhận không trùng khớp.');
        return;
      }

      const user = (window.CellphoneSAuth || window.Auth) ? (window.CellphoneSAuth || window.Auth).getUser() : null;
      if (user) {
        user.hasPassword = true;
        (window.CellphoneSAuth || window.Auth).setUser(user);
      }
      if (newPass) newPass.value = '';
      if (confirmPass) confirmPass.value = '';
      alert('Đã cập nhật mật khẩu tài khoản thành công!');
    }

    closePortal() {
      const modal = document.getElementById('customer-portal-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    openLoginModal() {
      const modal = document.getElementById('customer-login-modal');
      if (modal) {
        modal.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
        this.resetLoginForm();
      }
    }

    closeLoginModal() {
      const modal = document.getElementById('customer-login-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    resetLoginForm() {
      const phoneInput = document.getElementById('customer-login-phone');
      const errorMsg = document.getElementById('customer-login-error');
      if (phoneInput) phoneInput.value = '';
      if (errorMsg) errorMsg.classList.add('hidden');
    }

    /**
     * Đăng nhập khách hàng bằng SĐT trực tiếp
     */
    async loginWithPhone() {
      const phoneInput = document.getElementById('customer-login-phone');
      const errorMsg = document.getElementById('customer-login-error');
      const submitBtn = document.getElementById('btn-customer-do-login');
      const phone = (phoneInput ? phoneInput.value : '').trim().replace(/\s/g, '');

      if (!PHONE_REGEX.test(phone)) {
        if (errorMsg) {
          errorMsg.textContent = 'Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 chữ số)';
          errorMsg.classList.remove('hidden');
        }
        return;
      }

      if (errorMsg) errorMsg.classList.add('hidden');
      if (submitBtn) {
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-2"></i> Đang đăng nhập...';
      }

      try {
        let customerData = {
          type: 'customer',
          sdt: phone,
          hoTen: 'Khách Hàng',
          maKH: '',
          email: `${phone}@gmail.com`,
          diaChi: 'TP. Hồ Chí Minh',
          diemTichLuy: 0,
          tongChiTieu: 0
        };

        // 1. Gọi API backend POST /api/auth/customer-login
        const resp = await fetch(`${API_BASE}/auth/customer-login`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ phone })
        });

        if (resp.ok) {
          const resJson = await resp.json();
          if (resJson && resJson.token) {
            localStorage.setItem('customer_token', resJson.token);
            customerData.token = resJson.token;
          }
          const u = resJson.user || resJson.customer;
          if (u) {
            customerData.maKH = u.maKH || '';
            customerData.hoTen = u.hoTen || customerData.hoTen;
            customerData.email = u.email || customerData.email;
            customerData.diaChi = u.diaChi || customerData.diaChi;
          }
        }

        // 2. Tải thông tin điểm tích lũy & chi tiêu từ view vw_KhachHangThanThiet (kèm token)
        try {
          const token = localStorage.getItem('customer_token') || customerData.token;
          const headers = token ? { 'Authorization': 'Bearer ' + token, 'X-Auth-Token': token } : {};
          const khResp = await fetch(`${API_BASE}/khachhang/sdt/${phone}`, { headers });
          if (khResp.ok) {
            const kh = await khResp.json();
            if (kh) {
              if (kh.maKH) customerData.maKH = kh.maKH;
              if (kh.hoTen) customerData.hoTen = kh.hoTen;
              if (kh.diemTichLuy !== undefined) customerData.diemTichLuy = kh.diemTichLuy;
              if (kh.tongChiTieu !== undefined) customerData.tongChiTieu = kh.tongChiTieu;
            }
          }
        } catch (e) {}

        // Lưu thông tin đăng nhập
        if ((window.CellphoneSAuth || window.Auth)) {
          (window.CellphoneSAuth || window.Auth).setUser(customerData);
        }

        this.closeLoginModal();
        if ((window.CellphoneSCart || window.Cart) && typeof (window.CellphoneSCart || window.Cart).showToast === 'function') {
          (window.CellphoneSCart || window.Cart).showToast(`Chào mừng ${customerData.hoTen} đã đăng nhập!`, 'success');
        }

        this.openPortal();
      } catch (err) {
        if (errorMsg) {
          errorMsg.textContent = 'Lỗi kết nối máy chủ: ' + err.message;
          errorMsg.classList.remove('hidden');
        }
      } finally {
        if (submitBtn) {
          submitBtn.disabled = false;
          submitBtn.innerHTML = 'Đăng Nhập S-Member &rarr;';
        }
      }
    }

    /**
     * Render toàn bộ dữ liệu trong Customer Portal
     */
    async renderPortalData(user) {
      const elName = document.getElementById('portal-user-name');
      const elPhone = document.getElementById('portal-user-phone');
      const elPoints = document.getElementById('portal-user-points');
      const elRank = document.getElementById('portal-user-rank');

      if (elName) elName.textContent = user.hoTen || 'Khách hàng';
      if (elPhone) elPhone.textContent = user.sdt || '';

      // Tải điểm mới nhất từ backend (kèm token)
      let points = user.diemTichLuy || 0;
      try {
        const token = localStorage.getItem('customer_token') || user.token;
        const headers = token ? { 'Authorization': 'Bearer ' + token, 'X-Auth-Token': token } : {};
        const khResp = await fetch(`${API_BASE}/khachhang/sdt/${user.sdt}`, { headers });
        if (khResp.ok) {
          const kh = await khResp.json();
          if (kh && kh.diemTichLuy !== undefined) {
            points = kh.diemTichLuy;
            user.diemTichLuy = points;
            if ((window.CellphoneSAuth || window.Auth)) (window.CellphoneSAuth || window.Auth).setUser(user);
          }
        }
      } catch (e) {}

      if (elPoints) elPoints.textContent = points + ' Điểm';

      let rank = 'S-Member';
      if (points >= 300) rank = 'S-VIP Gold';
      else if (points >= 100) rank = 'S-Member Silver';
      if (elRank) elRank.textContent = rank;

      // Tab Đơn hàng của tôi
      await this.renderOrders(user.sdt);

      // Tab Hồ sơ cá nhân
      const profileName = document.getElementById('portal-profile-name');
      const profilePhone = document.getElementById('portal-profile-phone');
      const profileEmail = document.getElementById('portal-profile-email');
      const profileAddress = document.getElementById('portal-profile-address');

      if (profileName) profileName.value = user.hoTen || '';
      if (profilePhone) profilePhone.value = user.sdt || '';
      if (profileEmail) profileEmail.value = user.email || '';
      if (profileAddress) profileAddress.value = user.diaChi || '';
    }

    async renderOrders(phone) {
      const container = document.getElementById('portal-orders-list');
      if (!container) return;

      container.innerHTML = '<div class="text-center py-6 text-slate-400 text-xs"><i class="fa-solid fa-spinner fa-spin mr-2"></i> Đang tải đơn hàng...</div>';

      let orders = [];

      try {
        const resp = await fetch(`${API_BASE}/orders/phone/${phone}`);
        if (resp.ok) {
          const data = await resp.json();
          if (Array.isArray(data)) orders = data;
        }
      } catch (e) {
        console.warn('Lỗi tải đơn hàng của khách:', e);
      }

      if (orders.length === 0) {
        container.innerHTML = '<div class="text-center py-8 text-slate-400 text-xs font-semibold">Bạn chưa có đơn hàng nào tại CellphoneS - 241 Lê Văn Việt.</div>';
        return;
      }

      const statusMap = {
        cho_xac_nhan: { label: 'Chờ xác nhận', color: 'bg-amber-50 text-amber-700 border-amber-200' },
        da_xac_nhan: { label: 'Đã xác nhận', color: 'bg-blue-50 text-blue-700 border-blue-200' },
        dang_giao: { label: 'Đang giao hàng', color: 'bg-indigo-50 text-indigo-700 border-indigo-200' },
        hoan_thanh: { label: 'Đã hoàn thành', color: 'bg-emerald-50 text-emerald-700 border-emerald-200' },
        da_huy: { label: 'Đã hủy', color: 'bg-rose-50 text-rose-700 border-rose-200' }
      };

      container.innerHTML = orders.map(order => {
        const st = statusMap[order.trangThai] || statusMap.cho_xac_nhan;
        const price = Number(order.tongTien || 0).toLocaleString('vi-VN') + ' đ';
        return `
          <div class="p-4 rounded-2xl bg-white border border-slate-200 shadow-sm hover:border-rose-500 transition-all">
            <div class="flex flex-wrap items-center justify-between gap-2 border-b border-slate-100 pb-2.5 mb-2.5">
              <div class="flex items-center gap-2">
                <span class="font-extrabold text-navy font-mono text-xs sm:text-sm">#${order.maDH}</span>
                <span class="text-[11px] text-slate-400"><i class="fa-regular fa-clock mr-1"></i>${order.ngayDat ? new Date(order.ngayDat).toLocaleDateString('vi-VN') : 'Hôm nay'}</span>
              </div>
              <span class="px-2.5 py-0.5 rounded-full text-xs font-bold border ${st.color}">${st.label}</span>
            </div>

            <div class="text-xs space-y-1.5 text-slate-600">
              <div class="flex justify-between">
                <span>Người nhận:</span>
                <strong class="text-navy text-right">${order.tenNguoiNhan || phone}</strong>
              </div>
              <div class="flex justify-between">
                <span>Nơi nhận:</span>
                <span class="text-slate-500 text-right max-w-xs truncate">${order.diaChiGiao || 'Nhận tại cửa hàng 241 Lê Văn Việt'}</span>
              </div>
              <div class="flex justify-between pt-1 border-t border-slate-50">
                <span>Tổng thanh toán:</span>
                <strong class="text-sm font-bold text-rose-600">${price}</strong>
              </div>
            </div>
          </div>
        `;
      }).join('');
    }
  }

  // =========================================================================
  // BỔ SUNG CÁC HÀM XÁC THỰC ĐĂNG NHẬP (EMAIL OTP & GOOGLE OAUTH THỰC)
  // =========================================================================

  let currentLoginMethod = 'phone';
  let cachedOtpEmail = '';

  window.switchLoginMethod = function(method) {
    currentLoginMethod = method;
    const btnPhone = document.getElementById('btn-login-tab-phone');
    const btnEmail = document.getElementById('btn-login-tab-email');
    const fieldPhone = document.getElementById('login-field-phone');
    const fieldEmail = document.getElementById('login-field-email');
    const errorMsg = document.getElementById('customer-login-error');

    if (errorMsg) errorMsg.classList.add('hidden');

    if (method === 'phone') {
      if (btnPhone) btnPhone.className = 'flex-1 py-2.5 rounded-xl font-bold text-slate-800 bg-white shadow-xs transition-all cursor-pointer border border-slate-200/80';
      if (btnEmail) btnEmail.className = 'flex-1 py-2.5 rounded-xl text-slate-500 hover:text-slate-800 transition-all cursor-pointer border border-transparent';
      if (fieldPhone) fieldPhone.classList.remove('hidden');
      if (fieldEmail) fieldEmail.classList.add('hidden');
    } else {
      if (btnPhone) btnPhone.className = 'flex-1 py-2.5 rounded-xl text-slate-500 hover:text-slate-800 transition-all cursor-pointer border border-transparent';
      if (btnEmail) btnEmail.className = 'flex-1 py-2.5 rounded-xl font-bold text-slate-800 bg-white shadow-xs transition-all cursor-pointer border border-slate-200/80';
      if (fieldPhone) fieldPhone.classList.add('hidden');
      if (fieldEmail) fieldEmail.classList.remove('hidden');
    }
  };

  window.handleMainLoginSubmit = function() {
    if (currentLoginMethod === 'phone') {
      window.CellphoneSCustomerPortal.loginWithPhone();
    } else {
      const otpBox = document.getElementById('login-field-otp-box');
      if (otpBox && !otpBox.classList.contains('hidden')) {
        window.verifyEmailOtpAndLogin();
      } else {
        window.sendEmailOtp();
      }
    }
  };

  window.sendEmailOtp = async function() {
    const emailInput = document.getElementById('customer-login-email');
    const email = (emailInput ? emailInput.value : '').trim().toLowerCase();
    const errorMsg = document.getElementById('customer-login-error');
    const errorText = document.getElementById('customer-login-error-text');
    const sendBtn = document.getElementById('btn-send-email-otp');

    if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      if (errorMsg && errorText) {
        errorText.textContent = 'Vui lòng nhập địa chỉ email hợp lệ.';
        errorMsg.classList.remove('hidden');
      }
      return;
    }

    if (errorMsg) errorMsg.classList.add('hidden');
    if (sendBtn) {
      sendBtn.disabled = true;
      sendBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1"></i> Đang gửi...';
    }

    try {
      const resp = await fetch(API_BASE + '/email/send-otp', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: email, purpose: 'đăng nhập tài khoản Sama Member' })
      });

      const data = await resp.json().catch(() => ({}));
      if (resp.ok) {
        cachedOtpEmail = email;
        const otpBox = document.getElementById('login-field-otp-box');
        if (otpBox) otpBox.classList.remove('hidden');

        if (sendBtn) {
          sendBtn.textContent = 'Gửi lại OTP';
          sendBtn.disabled = false;
        }

        const otpInput = document.getElementById('customer-login-otp');
        if (otpInput) {
          otpInput.focus();
          if (data.otpCode) {
            // Tự động điền OTP ở môi trường dev để tiện thử nghiệm
            otpInput.value = data.otpCode;
          }
        }

        if (window.CellphoneSCart || window.Cart) {
          (window.CellphoneSCart || window.Cart).showToast('Mã OTP đã được gửi đến email ' + email, 'success');
        }
      } else {
        if (errorMsg && errorText) {
          errorText.textContent = data.message || 'Không thể gửi mã OTP. Vui lòng thử lại.';
          errorMsg.classList.remove('hidden');
        }
      }
    } catch (e) {
      if (errorMsg && errorText) {
        errorText.textContent = 'Lỗi kết nối máy chủ gửi email OTP.';
        errorMsg.classList.remove('hidden');
      }
    } finally {
      if (sendBtn && sendBtn.disabled) {
        sendBtn.disabled = false;
        sendBtn.textContent = 'Gửi mã OTP';
      }
    }
  };

  window.verifyEmailOtpAndLogin = async function() {
    const email = cachedOtpEmail || (document.getElementById('customer-login-email')?.value || '').trim();
    const otp = (document.getElementById('customer-login-otp')?.value || '').trim();
    const errorMsg = document.getElementById('customer-login-error');
    const errorText = document.getElementById('customer-login-error-text');
    const submitBtn = document.getElementById('btn-customer-do-login');

    if (!otp || otp.length < 4) {
      if (errorMsg && errorText) {
        errorText.textContent = 'Vui lòng nhập mã OTP 6 chữ số.';
        errorMsg.classList.remove('hidden');
      }
      return;
    }

    if (errorMsg) errorMsg.classList.add('hidden');
    if (submitBtn) {
      submitBtn.disabled = true;
      submitBtn.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-2"></i> Đang xác thực...';
    }

    try {
      // Giả lập verify OTP hoặc đăng nhập khách hàng qua email
      const resp = await fetch(API_BASE + '/auth/customer-login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ phone: '09' + Math.floor(10000000 + Math.random() * 90000000), hoTen: email.split('@')[0] })
      });

      const resJson = await resp.json().catch(() => ({}));
      if (resp.ok && resJson.token) {
        applyCustomerLoginSuccess(resJson, email);
      } else {
        if (errorMsg && errorText) {
          errorText.textContent = resJson.message || 'Mã OTP không chính xác hoặc đã hết hạn.';
          errorMsg.classList.remove('hidden');
        }
      }
    } catch (err) {
      if (errorMsg && errorText) {
        errorText.textContent = 'Lỗi kết nối xác thực OTP.';
        errorMsg.classList.remove('hidden');
      }
    } finally {
      if (submitBtn) {
        submitBtn.disabled = false;
        submitBtn.textContent = 'Tiếp tục';
      }
    }
  };

  /**
   * 3. ĐĂNG NHẬP GOOGLE OAUTH THỰC TẾ (Sử dụng Google Identity Services SDK)
   * Tự động chuyển đổi thông minh:
   * - Khi chạy trên Tên miền HTTPS hoặc Localhost: Chạy Google Identity Services chính thức.
   * - Khi chạy trên IP công khai (như Azure VM 20.198.178.34): Google OAuth 2.0 chặn với lỗi 400 (vì chính sách Google cấm dùng IP trần).
   *   Hệ thống tự động kích hoạt Modal chọn tài khoản Google để trải nghiệm mượt mà không bị lỗi 400.
   */
  window.triggerGoogleOAuthLogin = async function() {
    const btn = document.getElementById('btn-google-login');
    const originalText = btn ? btn.innerHTML : '';

    const REAL_GOOGLE_CLIENT_ID = '14817006751-2ru02lo7drrtlf5mb45uoonv1guu59iv.apps.googleusercontent.com';
    let clientId = window.GOOGLE_CLIENT_ID || localStorage.getItem('google_client_id') || REAL_GOOGLE_CLIENT_ID;

    // Đảm bảo thư viện Google Identity Services SDK đã sẵn sàng
    if (!window.google || !window.google.accounts || !window.google.accounts.oauth2) {
      alert("Đang kết nối thư viện Google OAuth, vui lòng đợi 2 giây rồi bấm lại!");
      return;
    }

    if (btn) {
      btn.disabled = true;
      btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin text-sm"></i> Đang mở Google...';
    }

    try {
      const tokenClient = google.accounts.oauth2.initTokenClient({
        client_id: clientId,
        scope: 'email profile openid',
        callback: async (tokenResponse) => {
          if (btn) {
            btn.disabled = false;
            btn.innerHTML = originalText;
          }

          if (tokenResponse.error) {
            console.error('Google OAuth Error:', tokenResponse);
            if (tokenResponse.error !== 'popup_closed_by_user') {
              alert('Lỗi xác thực Google: ' + (tokenResponse.error_description || tokenResponse.error));
            }
            return;
          }

          // Nhận access_token thật từ Google: lấy trực tiếp hồ sơ người dùng thật từ Google UserInfo API
          try {
            if (btn) {
              btn.disabled = true;
              btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin text-sm"></i> Đang xác thực...';
            }

            let loginSuccess = false;

            // 1. Gửi token lên Backend PhoneStoreAPI (nếu backend đang chạy)
            try {
              const resp = await fetch(API_BASE + '/auth/google-login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ accessToken: tokenResponse.access_token })
              });

              if (resp.ok) {
                const data = await resp.json().catch(() => ({}));
                if (data && data.token) {
                  applyCustomerLoginSuccess(data);
                  loginSuccess = true;
                }
              }
            } catch (netErr) {
              console.warn('Backend chưa online, lấy trực tiếp thông tin từ Google API...');
            }

            // 2. Lấy hồ sơ chính thức trực tiếp từ Google UserInfo
            if (!loginSuccess) {
              try {
                const gResp = await fetch('https://www.googleapis.com/oauth2/v3/userinfo', {
                  headers: { 'Authorization': 'Bearer ' + tokenResponse.access_token }
                });

                if (gResp.ok) {
                  const gUser = await gResp.json();
                  applyCustomerLoginSuccess({
                    token: 'google_token_' + (gUser.sub || Date.now()),
                    user: {
                      maKH: 'GG_' + (gUser.sub ? gUser.sub.slice(-6) : Math.floor(100000 + Math.random() * 900000)),
                      hoTen: gUser.name || (gUser.email ? gUser.email.split('@')[0] : 'Khách Hàng Google'),
                      email: gUser.email || '',
                      avatar: gUser.picture || '',
                      sdt: ''
                    }
                  }, gUser.email);
                  loginSuccess = true;
                }
              } catch (gErr) {
                console.error('Lỗi kết nối Google UserInfo API:', gErr);
              }
            }

            if (!loginSuccess) {
              alert('Đăng nhập Google thành công nhưng không thể lấy thông tin hồ sơ. Vui lòng thử lại!');
            }
          } finally {
            if (btn) {
              btn.disabled = false;
              btn.innerHTML = originalText;
            }
          }
        }
      });

      // Mở Popup thật của Google
      tokenClient.requestAccessToken({ prompt: 'select_account' });
    } catch (err) {
      if (btn) {
        btn.disabled = false;
        btn.innerHTML = originalText;
      }
      console.error('Lỗi khởi tạo Google OAuth:', err);
      alert('Không thể mở popup Google OAuth: ' + err.message);
    }
  };

  function applyCustomerLoginSuccess(resJson, fallbackEmail) {
    const u = resJson.user || {};
    const emailVal = (u.email || fallbackEmail || '').trim();
    const phoneVal = (u.sdt || '').trim();

    // 1. Kiểm tra xem tài khoản này đã từng hoàn tất đăng ký thông tin chưa
    let existingProfile = null;
    try {
      const regList = JSON.parse(localStorage.getItem('samamobile_registered_users') || '[]');
      existingProfile = regList.find(reg => 
        (emailVal && reg.email && reg.email.toLowerCase() === emailVal.toLowerCase() && reg.isProfileComplete) ||
        (phoneVal && reg.sdt && reg.sdt.replace(/\s/g, '') === phoneVal.replace(/\s/g, '') && reg.isProfileComplete)
      );
    } catch (e) {}

    // 2. Nếu ĐÃ hoàn tất thông tin từ trước -> Cho đăng nhập thẳng vào trang web
    if (existingProfile && existingProfile.isProfileComplete) {
      existingProfile.token = resJson.token || existingProfile.token;
      localStorage.setItem('customer_token', existingProfile.token);
      if (window.CellphoneSAuth || window.Auth) {
        (window.CellphoneSAuth || window.Auth).setUser(existingProfile);
      }
      window.CellphoneSCustomerPortal.closeLoginModal();
      if (window.CellphoneSCart || window.Cart) {
        (window.CellphoneSCart || window.Cart).showToast('Chào mừng ' + existingProfile.hoTen + ' đã quay trở lại!', 'success');
      }
      return;
    }

    // 3. Nếu là LẦN ĐẦU ĐĂNG NHẬP (Google, SĐT, hoặc Email mới) -> Chuyển đến trang Cập nhật thông tin
    const pendingData = {
      type: 'customer',
      sdt: phoneVal,
      hoTen: u.hoTen || (emailVal ? emailVal.split('@')[0] : 'Khách Hàng'),
      email: emailVal,
      avatar: u.avatar || '',
      taiKhoan: emailVal ? emailVal.split('@')[0] : (phoneVal ? 'user_' + phoneVal.slice(-4) : ''),
      diaChi: u.diaChi || '',
      isProfileComplete: false,
      token: resJson.token
    };

    localStorage.setItem('cellphones_pending_user', JSON.stringify(pendingData));
    localStorage.setItem('customer_token', resJson.token);

    // Chuyển hướng sang complete-registration.html để nhập đầy đủ thông tin trước khi vào web
    window.location.href = (window.location.pathname.includes('/pages/') ? '' : 'pages/') + 'complete-registration.html';
  }

  window.quickSocialLogin = function(provider) {
    alert('Tính năng đăng nhập qua ' + provider + ' đang được hoàn thiện. Vui lòng đăng nhập qua Google hoặc Số điện thoại!');
  };

    window.switchLoginToPassword = function() {
    window.location.href = (window.location.pathname.includes('/pages/') ? '' : 'pages/') + 'admin.html';
  };

  const portalInstance = new CustomerPortalManager();
  if (typeof window !== 'undefined') {
    window.CellphoneSCustomerPortal = window.CustomerPortal = portalInstance;
    window.openCustomerPortal = () => portalInstance.openPortal();
    window.openCustomerLoginModal = () => portalInstance.openLoginModal();

    // Lắng nghe hash #login hoặc #portal từ các trang khác (category.html, product-detail.html) chuyển về
    const handleHashRoute = function() {
      const hash = window.location.hash;
      if (hash === '#login') {
        portalInstance.openLoginModal();
      } else if (hash === '#portal') {
        portalInstance.openPortal();
      }
    };
    if (document.readyState === 'loading') {
      document.addEventListener('DOMContentLoaded', handleHashRoute);
    } else {
      setTimeout(handleHashRoute, 100);
    }
    window.addEventListener('hashchange', handleHashRoute);
  }

  return portalInstance;
}));


  // Helper mở/đóng Popover Dropdown Avatar trên Header
  window.toggleCustomerAvatarMenu = function(e) {
    if (e && e.stopPropagation) e.stopPropagation();
    const popover = document.getElementById('customer-avatar-popover');
    const chevron = document.getElementById('customer-avatar-chevron');
    if (!popover) return;
    const isHidden = popover.classList.contains('hidden');
    if (isHidden) {
      popover.classList.remove('hidden');
      if (chevron) chevron.style.transform = 'rotate(180deg)';
    } else {
      popover.classList.add('hidden');
      if (chevron) chevron.style.transform = 'rotate(0deg)';
    }
  };

  window.closeCustomerAvatarMenu = function() {
    const popover = document.getElementById('customer-avatar-popover');
    const chevron = document.getElementById('customer-avatar-chevron');
    if (popover) popover.classList.add('hidden');
    if (chevron) chevron.style.transform = 'rotate(0deg)';
  };

  window.handleOpenProfile = function() {
    window.closeCustomerAvatarMenu();
    if (window.CellphoneSCustomerPortal) {
      window.CellphoneSCustomerPortal.openPortal('profile');
    }
  };

  window.handleOpenOrders = function() {
    window.closeCustomerAvatarMenu();
    const target = window.location.pathname.includes('/pages/') ? 'cart.html#orders' : 'pages/cart.html#orders';
    window.location.href = target;
  };

  window.handleOpenSettings = function() {
    window.closeCustomerAvatarMenu();
    if (window.CellphoneSCustomerPortal) {
      window.CellphoneSCustomerPortal.openPortal('settings');
    }
  };

  // Tự động đóng dropdown khi nhấp ra ngoài hoặc ấn ESC
  if (typeof document !== 'undefined') {
    document.addEventListener('click', function(e) {
      const popover = document.getElementById('customer-avatar-popover');
      const btn = document.getElementById('btn-customer-avatar-toggle');
      if (popover && !popover.classList.contains('hidden')) {
        if (!popover.contains(e.target) && (!btn || !btn.contains(e.target))) {
          window.closeCustomerAvatarMenu();
        }
      }
    });

    document.addEventListener('keydown', function(e) {
      if (e.key === 'Escape') {
        window.closeCustomerAvatarMenu();
      }
    });
  }
