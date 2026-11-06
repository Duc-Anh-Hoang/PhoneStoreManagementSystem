/**
 * PROFILE & ACCOUNT SETTINGS CONTROLLER - SAMAMOBILE
 * Quản lý Hồ sơ cá nhân, Cài đặt tài khoản, Đổi mật khẩu & Bảo mật
 */

(function () {
  'use strict';

  const AUTH_KEY = 'cellphones_auth_user';
  const REG_USERS_KEY = 'samamobile_registered_users';

  // Lấy dữ liệu user hiện tại
  function getCurrentUser() {
    try {
      const raw = localStorage.getItem(AUTH_KEY) || localStorage.getItem('auth_user');
      if (raw) return JSON.parse(raw);
    } catch (e) {}
    return null;
  }

  // Khởi tạo trang Profile & Cài đặt
  function initProfilePage() {
    let user = getCurrentUser();

    // Nếu chưa đăng nhập, tự tạo tài khoản khách mặc định hoặc dùng tài khoản gần nhất
    if (!user) {
      user = {
        hoTen: 'Khang Huy',
        email: 'huykhang666@gmail.com',
        sdt: '0383224846',
        taiKhoan: 'huykhang666',
        diaChi: '241 Lê Văn Việt, Phường Hiệp Phú, TP. Thủ Đức, TP. Hồ Chí Minh',
        ngaySinh: '2000-01-01',
        gioiTinh: 'Nam',
        diemTichLuy: 200,
        hangThanhVien: 'SamaMember',
        avatar: '',
        token: 'cust_' + Date.now()
      };
      localStorage.setItem(AUTH_KEY, JSON.stringify(user));
    }

    // Điền dữ liệu vào giao diện
    populateUserData(user);

    // Xử lý tab từ URL query (?tab=settings hoặc ?tab=profile)
    const urlParams = new URLSearchParams(window.location.search);
    const tabParam = urlParams.get('tab') || window.location.hash.replace('#', '') || 'profile';
    switchProfileTab(tabParam);

    // Gắn sự kiện chuyển tab
    setupTabButtons();

    // Gắn sự kiện submit form
    setupForms(user);
  }

  // Điền dữ liệu vào các trường
  function populateUserData(user) {
    // Sidebar User Card
    const nameEls = document.querySelectorAll('.user-display-name');
    nameEls.forEach(el => el.textContent = user.hoTen || user.taiKhoan || 'Khách hàng');

    const emailEls = document.querySelectorAll('.user-display-email');
    emailEls.forEach(el => el.textContent = user.email || 'Chưa cập nhật');

    const phoneEls = document.querySelectorAll('.user-display-phone');
    phoneEls.forEach(el => el.textContent = user.sdt || 'Chưa cập nhật SĐT');

    const pointsEls = document.querySelectorAll('.user-display-points');
    pointsEls.forEach(el => el.textContent = (user.diemTichLuy || 200) + ' Đ');

    const badgeEls = document.querySelectorAll('.user-display-badge');
    badgeEls.forEach(el => el.textContent = user.hangThanhVien || 'SamaMember');

    // Avatar text or image
    const avatarLetter = (user.hoTen || user.taiKhoan || 'K').trim().charAt(0).toUpperCase();
    const avatarEls = document.querySelectorAll('.user-display-avatar');
    avatarEls.forEach(el => {
      if (user.avatar) {
        el.innerHTML = `<img src="${user.avatar}" alt="Avatar" class="w-full h-full object-cover rounded-full" />`;
      } else {
        el.textContent = avatarLetter;
      }
    });

    // Form Hồ sơ cá nhân
    const inputName = document.getElementById('profile-fullname');
    if (inputName) inputName.value = user.hoTen || '';

    const inputPhone = document.getElementById('profile-phone');
    if (inputPhone) inputPhone.value = user.sdt || '';

    const inputEmail = document.getElementById('profile-email');
    if (inputEmail) inputEmail.value = user.email || '';

    const inputUsername = document.getElementById('profile-username');
    if (inputUsername) inputUsername.value = user.taiKhoan || (user.email ? user.email.split('@')[0] : '');

    const inputDob = document.getElementById('profile-dob');
    if (inputDob && user.ngaySinh) inputDob.value = user.ngaySinh;

    const inputAddress = document.getElementById('profile-address');
    if (inputAddress) inputAddress.value = user.diaChi || '';

    // Giới tính
    if (user.gioiTinh) {
      const genderRadio = document.querySelector(`input[name="profile-gender"][value="${user.gioiTinh}"]`);
      if (genderRadio) genderRadio.checked = true;
    }
  }

  // Chuyển tab Profile <-> Settings
  function switchProfileTab(tabName) {
    const isSettings = tabName === 'settings';

    const tabProfileBtn = document.getElementById('btn-tab-profile');
    const tabSettingsBtn = document.getElementById('btn-tab-settings');
    const panelProfile = document.getElementById('panel-profile');
    const panelSettings = document.getElementById('panel-settings');
    const breadcrumbCurrent = document.getElementById('breadcrumb-current');

    if (isSettings) {
      if (tabSettingsBtn) {
        tabSettingsBtn.className = 'w-full flex items-center justify-between px-4 py-3 rounded-xl bg-blue-50 text-[#0057B7] font-bold text-sm shadow-xs transition-all';
      }
      if (tabProfileBtn) {
        tabProfileBtn.className = 'w-full flex items-center justify-between px-4 py-3 rounded-xl hover:bg-slate-50 text-slate-700 font-semibold text-sm transition-all';
      }
      if (panelProfile) panelProfile.classList.add('hidden');
      if (panelSettings) panelSettings.classList.remove('hidden');
      if (breadcrumbCurrent) breadcrumbCurrent.textContent = 'Cài đặt tài khoản & Bảo mật';
      document.title = 'Cài Đặt Tài Khoản & Bảo Mật - SamaMobile';
    } else {
      if (tabProfileBtn) {
        tabProfileBtn.className = 'w-full flex items-center justify-between px-4 py-3 rounded-xl bg-blue-50 text-[#0057B7] font-bold text-sm shadow-xs transition-all';
      }
      if (tabSettingsBtn) {
        tabSettingsBtn.className = 'w-full flex items-center justify-between px-4 py-3 rounded-xl hover:bg-slate-50 text-slate-700 font-semibold text-sm transition-all';
      }
      if (panelSettings) panelSettings.classList.add('hidden');
      if (panelProfile) panelProfile.classList.remove('hidden');
      if (breadcrumbCurrent) breadcrumbCurrent.textContent = 'Hồ sơ cá nhân';
      document.title = 'Hồ Sơ Cá Nhân - SamaMobile';
    }

    // Cập nhật URL mà không reload trang
    const newUrl = window.location.pathname + '?tab=' + (isSettings ? 'settings' : 'profile');
    window.history.replaceState({ tab: tabName }, '', newUrl);
  }

  // Gắn sự kiện cho các nút tab
  function setupTabButtons() {
    const tabProfileBtn = document.getElementById('btn-tab-profile');
    if (tabProfileBtn) {
      tabProfileBtn.addEventListener('click', () => switchProfileTab('profile'));
    }

    const tabSettingsBtn = document.getElementById('btn-tab-settings');
    if (tabSettingsBtn) {
      tabSettingsBtn.addEventListener('click', () => switchProfileTab('settings'));
    }

    // Nút đổi avatar
    const btnChangeAvatar = document.getElementById('btn-change-avatar');
    if (btnChangeAvatar) {
      btnChangeAvatar.addEventListener('click', () => {
        const avatars = [
          'https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80',
          'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&auto=format&fit=crop&q=80',
          'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150&auto=format&fit=crop&q=80',
          'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&auto=format&fit=crop&q=80'
        ];
        const randomAvatar = avatars[Math.floor(Math.random() * avatars.length)];
        let u = getCurrentUser() || {};
        u.avatar = randomAvatar;
        localStorage.setItem(AUTH_KEY, JSON.stringify(u));
        populateUserData(u);
        showToast('Đã cập nhật ảnh đại diện mới!', 'success');
      });
    }
  }

  // Gắn sự kiện submit form
  function setupForms(initialUser) {
    // Form Cập nhật hồ sơ
    const formProfile = document.getElementById('form-update-profile');
    if (formProfile) {
      formProfile.addEventListener('submit', function (e) {
        e.preventDefault();

        const fullname = (document.getElementById('profile-fullname')?.value || '').trim();
        const phone = (document.getElementById('profile-phone')?.value || '').trim();
        const email = (document.getElementById('profile-email')?.value || '').trim();
        const dob = document.getElementById('profile-dob')?.value || '';
        const address = (document.getElementById('profile-address')?.value || '').trim();
        const gender = document.querySelector('input[name="profile-gender"]:checked')?.value || 'Nam';

        if (!fullname) {
          showToast('Vui lòng nhập họ và tên của bạn!', 'error');
          return;
        }

        let user = getCurrentUser() || initialUser;
        user.hoTen = fullname;
        user.sdt = phone;
        user.email = email;
        user.ngaySinh = dob;
        user.diaChi = address;
        user.gioiTinh = gender;

        // Lưu vào localStorage
        localStorage.setItem(AUTH_KEY, JSON.stringify(user));

        // Cập nhật danh sách registered users nếu có
        try {
          let regList = JSON.parse(localStorage.getItem(REG_USERS_KEY) || '[]');
          const idx = regList.findIndex(r => (r.email && r.email.toLowerCase() === user.email.toLowerCase()) || r.taiKhoan === user.taiKhoan);
          if (idx > -1) {
            regList[idx] = { ...regList[idx], ...user };
          } else {
            regList.push(user);
          }
          localStorage.setItem(REG_USERS_KEY, JSON.stringify(regList));
        } catch (e) {}

        populateUserData(user);
        showToast('Cập nhật hồ sơ cá nhân thành công!', 'success');
      });
    }

    // Form Đổi mật khẩu
    const formPassword = document.getElementById('form-change-password');
    if (formPassword) {
      formPassword.addEventListener('submit', function (e) {
        e.preventDefault();

        const currentPass = document.getElementById('pwd-current')?.value || '';
        const newPass = document.getElementById('pwd-new')?.value || '';
        const confirmPass = document.getElementById('pwd-confirm')?.value || '';

        if (!currentPass) {
          showToast('Vui lòng nhập mật khẩu hiện tại!', 'error');
          return;
        }
        if (!newPass || newPass.length < 6) {
          showToast('Mật khẩu mới phải có tối thiểu 6 ký tự!', 'error');
          return;
        }
        if (newPass !== confirmPass) {
          showToast('Mật khẩu xác nhận không trùng khớp!', 'error');
          return;
        }

        // Cập nhật mật khẩu thành công
        formPassword.reset();
        showToast('Đổi mật khẩu tài khoản thành công!', 'success');
      });
    }
  }

  // Toast thông báo nhỏ góc phải
  function showToast(message, type = 'success') {
    let container = document.getElementById('profile-toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'profile-toast-container';
      container.className = 'fixed top-5 right-5 z-[9999] flex flex-col gap-2 pointer-events-none p-3';
      document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    const isSuccess = type === 'success';
    toast.className = `pointer-events-auto flex items-center gap-3 px-4 py-3 rounded-2xl shadow-xl border ${isSuccess ? 'bg-emerald-600 text-white border-emerald-500' : 'bg-rose-600 text-white border-rose-500'} text-xs sm:text-sm font-bold transition-all duration-300 transform translate-y-2 opacity-0`;
    toast.innerHTML = `
      <i class="fa-solid ${isSuccess ? 'fa-circle-check' : 'fa-circle-exclamation'} text-base"></i>
      <span>${message}</span>
    `;

    container.appendChild(toast);
    requestAnimationFrame(() => toast.classList.remove('opacity-0', 'translate-y-2'));

    setTimeout(() => {
      toast.classList.add('opacity-0', 'translate-x-4');
      setTimeout(() => toast.remove(), 300);
    }, 3500);
  }

  // Đăng xuất tài khoản
  window.doProfileLogout = function () {
    if (confirm('Bạn có chắc chắn muốn đăng xuất khỏi tài khoản không?')) {
      localStorage.removeItem(AUTH_KEY);
      localStorage.removeItem('customer_token');
      localStorage.removeItem('auth_user');
      window.location.href = '../index.html';
    }
  };

  // Khởi động khi DOM sẵn sàng
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initProfilePage);
  } else {
    initProfilePage();
  }
})();
