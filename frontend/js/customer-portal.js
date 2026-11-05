  const REAL_GOOGLE_CLIENT_ID = '14817006751-2ru02lo7drrtlf5mb45uoonv1guu59iv.apps.googleusercontent.com';

  /**
   * Bấm nút Google -> Mở Popup Đăng nhập Google thật (Google Identity Services)
   */
  window.triggerGoogleOAuthLogin = function() {
    const btn = document.getElementById('btn-google-login');
    const originalText = btn ? btn.innerHTML : '';

    if (window.google && window.google.accounts && window.google.accounts.oauth2) {
      if (btn) {
        btn.disabled = true;
        btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin text-sm"></i> Đang mở Google...';
      }

      try {
        const tokenClient = google.accounts.oauth2.initTokenClient({
          client_id: REAL_GOOGLE_CLIENT_ID,
          scope: 'email profile openid',
          callback: async (tokenResponse) => {
            if (btn) {
              btn.disabled = false;
              btn.innerHTML = originalText;
            }

            if (tokenResponse.error) {
              console.error('Google OAuth error:', tokenResponse);
              if (tokenResponse.error !== 'popup_closed_by_user') {
                alert('Đăng nhập Google thất bại: ' + (tokenResponse.error_description || tokenResponse.error));
              }
              return;
            }

            // Gửi access_token thật từ Google lên Backend
            try {
              if (btn) {
                btn.disabled = true;
                btn.innerHTML = '<i class="fa-solid fa-spinner fa-spin text-sm"></i> Đang xác thực...';
              }

              const resp = await fetch(API_BASE + '/auth/google-login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ accessToken: tokenResponse.access_token })
              });

              const data = await resp.json().catch(() => ({}));
              if (resp.ok && data.token) {
                applyCustomerLoginSuccess(data);
              } else {
                alert(data.message || 'Xác thực tài khoản Google với hệ thống thất bại.');
              }
            } catch (netErr) {
              alert('Không thể kết nối đến máy chủ backend để hoàn tất đăng nhập.');
            } finally {
              if (btn) {
                btn.disabled = false;
                btn.innerHTML = originalText;
              }
            }
          }
        });

        // Mở popup tài khoản Google thật
        tokenClient.requestAccessToken({ prompt: 'select_account' });
      } catch (err) {
        if (btn) {
          btn.disabled = false;
          btn.innerHTML = originalText;
        }
        console.error('Lỗi mở Google Sign-In:', err);
        alert('Lỗi khởi tạo Google Sign-In: ' + err.message);
      }
    } else {
      alert('Đang tải thư viện Google Sign-In... Vui lòng đợi trong giây lát rồi thử lại!');
    }
  };

function applyCustomerLoginSuccess(resJson, fallbackEmail) {
    const u = resJson.user || {};
    const customerData = {
      type: 'customer',
      sdt: u.sdt || '',
      hoTen: u.hoTen || 'Khách Hàng Google',
      maKH: u.maKH || '',
      email: u.email || fallbackEmail || '',
      diaChi: u.diaChi || 'TP. Hồ Chí Minh',
      diemTichLuy: 0,
      tongChiTieu: 0,
      token: resJson.token
    };

    localStorage.setItem('customer_token', resJson.token);

    if (window.CellphoneSAuth || window.Auth) {
      (window.CellphoneSAuth || window.Auth).setUser(customerData);
    }

    window.CellphoneSCustomerPortal.closeLoginModal();
    if (window.CellphoneSCart || window.Cart) {
      (window.CellphoneSCart || window.Cart).showToast('Chào mừng ' + customerData.hoTen + ' đã đăng nhập!', 'success');
    }
  }

  window.quickSocialLogin = function(provider) {
    alert('Tính năng đăng nhập qua ' + provider + ' đang được hoàn thiện. Vui lòng đăng nhập qua Google hoặc Số điện thoại!');
  };

  window.switchLoginToPassword = function() {
    window.location.href = 'admin.html';
  };

  return new CustomerPortalManager();
}));
