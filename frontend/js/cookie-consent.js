/**
 * SAMAMOBILE - QUẢN LÝ COOKIE & POPUP CHÀO MỪNG SIÊU SALE 20/11
 * 1. Tự động hiển thị Popup Siêu Sale 20/11 khi vừa truy cập website (khớp ảnh mẫu xanh người dùng gửi).
 * 2. Thanh thông báo Cookie Consent hỏi người dùng cho phép lưu trữ cookies hay không.
 * 3. Ghi nhớ trạng thái qua document.cookie chuẩn 30 ngày (không làm phiền lại nếu đã đồng ý).
 */
(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.CookieConsent = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  const COOKIE_CONSENT_KEY = 'samamobile_cookie_consent';
  const CAMPAIGN_POPUP_KEY = 'samamobile_campaign_2011_seen';
  const CAMPAIGN_DISMISS_UNTIL_KEY = 'samamobile_campaign_dismiss_until';
  const DONT_SHOW_DURATION_MS = 24 * 60 * 60 * 1000; // Ẩn trong 24 giờ rồi tự động hiển thị lại

  class CookieManager {
    constructor() {
      this.consent = this.getCookie(COOKIE_CONSENT_KEY);
      this.init();
    }

    setCookie(name, value, days) {
      let expires = '';
      if (days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = '; expires=' + date.toUTCString();
      }
      document.cookie = name + '=' + encodeURIComponent(value || '') + expires + '; path=/; SameSite=Lax';
    }

    getCookie(name) {
      const nameEQ = name + '=';
      const ca = document.cookie.split(';');
      for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
      }
      return null;
    }

    eraseCookie(name) {
      document.cookie = name + '=; Max-Age=-99999999; path=/;';
    }

    init() {
      const run = () => {
        this.checkAndShowCookieConsent();
        this.checkAndShowCampaignPopup();
      };

      if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', run);
      } else {
        run();
      }
    }

    checkAndShowCookieConsent() {
      const consent = this.getCookie(COOKIE_CONSENT_KEY);
      if (!consent) {
        setTimeout(() => {
          const bar = document.getElementById('cookie-consent-bar');
          if (bar) bar.classList.remove('hidden');
        }, 900);
      }
    }

    checkAndShowCampaignPopup() {
      // Kiểm tra nếu người dùng đã tích chọn "Không hiển thị lại nữa"
      try {
        const dismissUntil = localStorage.getItem(CAMPAIGN_DISMISS_UNTIL_KEY);
        if (dismissUntil) {
          const expiryTime = parseInt(dismissUntil, 10);
          if (!isNaN(expiryTime) && Date.now() < expiryTime) {
            // Vẫn còn trong thời gian tạm ẩn (chưa hết 24h) -> Không hiển thị
            return;
          } else {
            // Đã hết thời hạn tạm ẩn -> Tự động giải phóng để hiển thị lại
            localStorage.removeItem(CAMPAIGN_DISMISS_UNTIL_KEY);
          }
        }
      } catch (err) {
        console.warn('Lỗi đọc trạng thái popup chiến dịch:', err);
      }

      // Tự động hiển thị banner sau 400ms
      setTimeout(() => {
        const modal = document.getElementById('welcome-campaign-modal');
        if (modal) {
          modal.classList.remove('hidden');
          // Đảm bảo checkbox không bị tick sẵn khi mở lại
          const chk = document.getElementById('chk-dont-show-campaign-again');
          if (chk) chk.checked = false;
        }
      }, 400);
    }

    acceptAllCookies() {
      this.setCookie(COOKIE_CONSENT_KEY, 'all', 30);
      this.hideCookieBar();
      this.showToast('Đã lưu tùy chọn: Cho phép lưu trữ Cookies đầy đủ.', 'success');
    }

    acceptEssentialCookies() {
      this.setCookie(COOKIE_CONSENT_KEY, 'essential', 7);
      this.hideCookieBar();
      this.showToast('Đã lưu tùy chọn: Chỉ cho phép Cookie thiết yếu.', 'info');
    }

    hideCookieBar() {
      const bar = document.getElementById('cookie-consent-bar');
      if (bar) {
        bar.classList.add('opacity-0', 'translate-y-8', 'transition-all', 'duration-300');
        setTimeout(() => bar.classList.add('hidden'), 350);
      }
    }

    closeCampaignModal(forceDismiss = true) {
      const chk = document.getElementById('chk-dont-show-campaign-again');
      if (chk && chk.checked) {
        try {
          const expiryTime = Date.now() + DONT_SHOW_DURATION_MS;
          localStorage.setItem(CAMPAIGN_DISMISS_UNTIL_KEY, expiryTime.toString());
        } catch (err) {
          console.warn('Lỗi lưu trạng thái ẩn banner:', err);
        }
      }

      const modal = document.getElementById('welcome-campaign-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    /**
     * Hỗ trợ reset trạng thái popup khi cần kiểm tra lại ngay lập tức
     */
    resetCampaignDismiss() {
      try {
        localStorage.removeItem(CAMPAIGN_DISMISS_UNTIL_KEY);
        this.showToast('Đã xóa bộ nhớ ẩn banner. Banner sẽ hiển thị lại khi tải trang.', 'info');
      } catch (e) {}
    }

    goToSale2011() {
      this.closeCampaignModal(true);

      const targetSection = document.getElementById('section-sieu-sale-20-11');
      if (targetSection) {
        targetSection.scrollIntoView({ behavior: 'smooth', block: 'start' });

        // Highlight nhẹ với ánh sáng vàng kim
        targetSection.classList.add('ring-4', 'ring-amber-400', 'transition-all', 'duration-500');
        setTimeout(() => {
          targetSection.classList.remove('ring-4', 'ring-amber-400');
        }, 1500);
      }
    }

    reopenCampaignModal() {
      const modal = document.getElementById('welcome-campaign-modal');
      if (modal) {
        modal.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
      }
    }

    reopenCookieSettings() {
      const bar = document.getElementById('cookie-consent-bar');
      if (bar) {
        bar.classList.remove('hidden', 'opacity-0', 'translate-y-8');
      }
    }

    showToast(msg, type = 'info') {
      const toast = document.createElement('div');
      const bg = type === 'success' ? 'bg-emerald-600' : 'bg-slate-800';
      toast.className = `fixed top-20 right-4 z-[99999] px-4 py-2.5 rounded-[4px] text-white font-bold text-xs shadow-2xl flex items-center gap-2 ${bg} animate-in fade-in slide-in-from-top-4 duration-200 font-sans`;
      toast.innerHTML = `<i class="fa-solid ${type === 'success' ? 'fa-circle-check' : 'fa-cookie-bite'} text-amber-300"></i><span>${msg}</span>`;
      document.body.appendChild(toast);
      setTimeout(() => {
        toast.classList.add('opacity-0', 'transition-opacity');
        setTimeout(() => toast.remove(), 300);
      }, 3000);
    }
  }

  const manager = new CookieManager();
  window.CookieConsent = manager;
  return manager;
}));
