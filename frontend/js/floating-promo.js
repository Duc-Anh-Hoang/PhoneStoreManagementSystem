/**
 * SAMAMOBILE.COM - FLOATING PROMO STICKER (THẾ GIỚI DI ĐỘNG STYLE)
 * Hiển thị sticker ưu đãi "Khách hàng mới giảm thêm 1 triệu" cố định ở góc dưới bên trái:
 * - Tỉ lệ kích thước được cân chỉnh chuẩn xác, hài hòa cân đối với mọi kích thước màn hình
 * - Hiệu ứng nhấp nháy to nhỏ (breathing pulse zoom animation) êm dịu, chuẩn phong cách Thegioididong
 * - Nút đóng (X) tròn xám mờ đồng bộ scale theo sticker, bấm đóng siêu nhạy
 * - Nút mini tròn icon hộp quà 3D tinh tế khi đóng, đường kính 40px không che lấp giao diện
 * - Click vào sticker mở Popup Modal VIP chi tiết ưu đãi + mã voucher 1-click copy
 * - Tự động tương thích cả trang chủ (index.html) và các trang con (pages/*.html)
 */

(function () {
  'use strict';

  // Khóa lưu trạng thái tắt trong SessionStorage
  const HIDE_STORAGE_KEY = 'samamobile_hide_floating_promo';
  const VOUCHER_CODE = 'SAMA1TRIEU';

  // Helper xác định đường dẫn tài nguyên
  function getAssetPath(relPath) {
    const isInsidePages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
    const clean = relPath.replace(/^\.\.\//, '').replace(/^\//, '');
    return isInsidePages ? ('../' + clean) : clean;
  }

  // Inject CSS trực tiếp
  function injectStyles() {
    let style = document.getElementById('floating-promo-style');
    if (!style) {
      style = document.createElement('style');
      style.id = 'floating-promo-style';
      document.head.appendChild(style);
    }
    style.textContent = `
      /* Keyframe nhấp nháy to nhỏ êm dịu chuẩn Thegioididong */
      @keyframes tgddStickerPulse {
        0% {
          transform: scale(0.92);
          filter: drop-shadow(0 4px 10px rgba(0, 60, 160, 0.22));
        }
        50% {
          transform: scale(1.06);
          filter: drop-shadow(0 8px 18px rgba(0, 80, 200, 0.38));
        }
        100% {
          transform: scale(0.92);
          filter: drop-shadow(0 4px 10px rgba(0, 60, 160, 0.22));
        }
      }

      /* Container chính góc dưới bên trái - Tỉ lệ cân đối màn hình */
      .floating-promo-container {
        position: fixed;
        left: 20px;
        bottom: 78px;
        z-index: 45;
        display: flex;
        align-items: center;
        justify-content: center;
        user-select: none;
        pointer-events: none; /* Chỉ wrapper nhận sự kiện chuột */
        transition: opacity 0.3s ease, transform 0.3s ease;
      }

      @media (max-width: 640px) {
        .floating-promo-container {
          left: 12px;
          bottom: 96px; /* Tránh cấn thanh bottom bar mua hàng */
        }
      }

      /* Wrapper sticker có animation to nhỏ - Kích thước chuẩn cân đối */
      .floating-promo-wrapper {
        position: relative;
        display: inline-block;
        width: 78px; /* Tỉ lệ cân đối chuẩn màn hình laptop / desktop */
        height: auto;
        cursor: pointer;
        pointer-events: auto;
        animation: tgddStickerPulse 1.8s ease-in-out infinite;
        transform-origin: center bottom;
        transition: transform 0.25s cubic-bezier(0.34, 1.56, 0.64, 1);
      }

      @media (min-width: 1440px) {
        .floating-promo-wrapper {
          width: 86px; /* Màn hình lớn 2K/FHD rộng rãi */
        }
      }

      @media (max-width: 1024px) {
        .floating-promo-wrapper {
          width: 68px; /* Màn hình tablet hoặc laptop 1366px */
        }
      }

      @media (max-width: 640px) {
        .floating-promo-wrapper {
          width: 58px; /* Điện thoại di động */
        }
      }

      /* Khi hover vào sticker thì tạm dừng nhấp nháy và phóng nhẹ để người dùng dễ bấm */
      .floating-promo-wrapper:hover {
        animation-play-state: paused;
        transform: scale(1.1);
      }

      /* Ảnh sticker 3D */
      .floating-promo-img {
        display: block;
        width: 100%;
        height: auto;
        pointer-events: auto;
      }

      /* Nút đóng tròn màu xám mờ chuẩn Thegioididong */
      .floating-promo-close-btn {
        position: absolute;
        top: 2%;
        right: 2%;
        width: 17px;
        height: 17px;
        border-radius: 9999px;
        background: rgba(100, 116, 139, 0.75);
        color: #ffffff;
        border: none;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        padding: 0;
        margin: 0;
        font-size: 8.5px;
        line-height: 1;
        transition: all 0.2s ease;
        backdrop-filter: blur(4px);
        -webkit-backdrop-filter: blur(4px);
        box-shadow: 0 1px 4px rgba(0, 0, 0, 0.25);
        z-index: 2;
      }

      @media (max-width: 640px) {
        .floating-promo-close-btn {
          width: 15px;
          height: 15px;
          font-size: 7.5px;
        }
      }

      .floating-promo-close-btn:hover {
        background: rgba(225, 29, 72, 0.95);
        transform: scale(1.15);
      }

      /* Nút tròn mini quà tặng tinh tế khi đã đóng sticker - Không che lấp màn hình */
      .floating-promo-mini-btn {
        position: fixed;
        left: 20px;
        bottom: 78px;
        z-index: 44;
        width: 42px;
        height: 42px;
        border-radius: 50%;
        background: linear-gradient(135deg, #0057B7 0%, #008774 100%);
        color: #ffffff;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 19px;
        box-shadow: 0 4px 14px rgba(0, 87, 183, 0.38);
        cursor: pointer;
        border: 2px solid #ffffff;
        transition: all 0.25s cubic-bezier(0.34, 1.56, 0.64, 1);
        animation: tgddMiniBounce 2.5s infinite;
        user-select: none;
      }

      @media (max-width: 640px) {
        .floating-promo-mini-btn {
          left: 12px;
          bottom: 96px;
          width: 38px;
          height: 38px;
          font-size: 17px;
        }
      }

      .floating-promo-mini-btn:hover {
        transform: scale(1.12) translateY(-2px);
        box-shadow: 0 8px 20px rgba(0, 87, 183, 0.5);
      }

      .floating-promo-mini-badge {
        position: absolute;
        top: -5px;
        right: -6px;
        background: #E11B1E;
        color: #ffffff;
        font-size: 8px;
        font-weight: 900;
        padding: 1.5px 4.5px;
        border-radius: 9999px;
        border: 1.5px solid #ffffff;
        line-height: 1;
        box-shadow: 0 2px 5px rgba(225, 27, 30, 0.4);
      }

      @keyframes tgddMiniBounce {
        0%, 100% { transform: translateY(0); }
        50% { transform: translateY(-3px); }
      }

      /* Modal chi tiết ưu đãi */
      .promo-modal-overlay {
        position: fixed;
        inset: 0;
        background: rgba(15, 23, 42, 0.65);
        backdrop-filter: blur(6px);
        -webkit-backdrop-filter: blur(6px);
        z-index: 9999;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 16px;
        animation: promoFadeIn 0.2s ease-out;
      }

      @keyframes promoFadeIn {
        from { opacity: 0; }
        to { opacity: 1; }
      }

      .promo-modal-box {
        background: #ffffff;
        width: 100%;
        max-width: 440px;
        border-radius: 28px;
        overflow: hidden;
        box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.35);
        border: 1px solid rgba(226, 232, 240, 0.8);
        position: relative;
        animation: promoZoomIn 0.25s cubic-bezier(0.16, 1, 0.3, 1);
      }

      @keyframes promoZoomIn {
        from { transform: scale(0.92); opacity: 0; }
        to { transform: scale(1); opacity: 1; }
      }
    `;
  }

  // Khởi tạo HTML DOM
  function createDOMElements() {
    if (document.getElementById('floating-promo-container')) return;

    const imgUrl = getAssetPath('images/promo/khach-hang-moi-giam-1-trieu.png');

    // 1. Container Sticker chính
    const container = document.createElement('div');
    container.id = 'floating-promo-container';
    container.className = 'floating-promo-container';

    container.innerHTML = `
      <div class="floating-promo-wrapper" id="floating-promo-wrapper" title="Bấm để nhận ngay ưu đãi Khách Hàng Mới giảm 1 Triệu">
        <!-- Nút đóng (X) tròn xám mờ chuẩn Thegioididong -->
        <button type="button" class="floating-promo-close-btn" id="floating-promo-close-btn" aria-label="Đóng ưu đãi" title="Đóng">
          <i class="fa-solid fa-xmark"></i>
        </button>

        <!-- Ảnh sticker 3D nhấp nháy to nhỏ -->
        <img src="${imgUrl}" alt="Khách hàng mới giảm thêm 1 triệu" class="floating-promo-img" id="floating-promo-img" />
      </div>
    `;

    // 2. Nút tròn mini quà tặng tinh tế khi đóng (không che lấp giao diện)
    const miniBtn = document.createElement('div');
    miniBtn.id = 'floating-promo-mini-btn';
    miniBtn.className = 'floating-promo-mini-btn';
    miniBtn.style.display = 'none';
    miniBtn.title = 'Mở lại ưu đãi Khách Hàng Mới giảm 1 Triệu';
    miniBtn.innerHTML = `
      <span>🎁</span>
      <span class="floating-promo-mini-badge">-1TR</span>
    `;

    // 3. Modal Popup chi tiết
    const modal = document.createElement('div');
    modal.id = 'floating-promo-modal';
    modal.className = 'promo-modal-overlay';
    modal.style.display = 'none';
    modal.innerHTML = `
      <div class="promo-modal-box" onclick="event.stopPropagation()">
        <!-- Header gradient xanh thương hiệu -->
        <div style="background: linear-gradient(135deg, #0057B7 0%, #008774 100%); padding: 22px 20px 18px; text-align: center; color: #ffffff; position: relative;">
          <button type="button" id="promo-modal-close-btn" style="position: absolute; top: 12px; right: 12px; width: 30px; height: 30px; border-radius: 50%; background: rgba(255,255,255,0.2); color: #fff; border: none; display: flex; align-items: center; justify-content: center; cursor: pointer; transition: background 0.2s;" title="Đóng">
            <i class="fa-solid fa-xmark"></i>
          </button>
          
          <div style="display: inline-block; width: 72px; height: 72px; margin-bottom: 4px;">
            <img src="${imgUrl}" alt="1 Triệu" style="width: 100%; height: 100%; object-fit: contain; filter: drop-shadow(0 4px 10px rgba(0,0,0,0.25));" />
          </div>

          <div style="display: inline-block; padding: 2px 10px; background: rgba(255,255,255,0.22); border-radius: 9999px; font-size: 10.5px; font-weight: 800; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 5px;">
            ✦ ĐẶC QUYỀN KHÁCH HÀNG MỚI ✦
          </div>

          <h3 style="font-size: 19px; font-weight: 900; line-height: 1.25; margin: 0; text-transform: uppercase;">
            GIẢM THÊM 1.000.000 ₫
          </h3>
          <p style="font-size: 11.5px; opacity: 0.9; margin-top: 4px; font-weight: 500;">
            Áp dụng cho hóa đơn mua sắm thiết bị công nghệ đầu tiên tại SamaMobile
          </p>
        </div>

        <!-- Body Modal -->
        <div style="padding: 20px 18px 18px;">
          <!-- Khung Mã Voucher -->
          <div style="background: #F8FAFC; border: 2px dashed #93C5FD; border-radius: 16px; padding: 12px 15px; display: flex; align-items: center; justify-content: space-between; gap: 12px; margin-bottom: 16px;">
            <div>
              <div style="font-size: 10.5px; color: #64748B; font-weight: 600;">MÃ GIẢM GIÁ ĐỘC QUYỀN:</div>
              <div style="font-size: 17px; font-weight: 900; color: #0057B7; font-family: monospace; letter-spacing: 1px; margin-top: 2px;">
                ${VOUCHER_CODE}
              </div>
            </div>
            <button type="button" id="promo-copy-voucher-btn" style="background: #0057B7; color: #ffffff; border: none; padding: 8px 14px; border-radius: 11px; font-size: 11.5px; font-weight: 800; cursor: pointer; transition: all 0.2s; display: flex; align-items: center; gap: 5px; box-shadow: 0 4px 10px rgba(0,87,183,0.25);">
              <i class="fa-regular fa-copy"></i>
              <span id="promo-copy-text">Sao chép</span>
            </button>
          </div>

          <!-- Điều kiện áp dụng chi tiết -->
          <div style="font-size: 11.5px; color: #334155; line-height: 1.55; margin-bottom: 18px;">
            <div style="display: flex; align-items: flex-start; gap: 7px; margin-bottom: 6px;">
              <i class="fa-solid fa-circle-check" style="color: #10B981; font-size: 12px; margin-top: 2px;"></i>
              <span>Giảm ngay <strong>1.000.000₫</strong> cho đơn hàng Điện thoại, Laptop, Tablet từ <strong>10.000.000₫</strong>.</span>
            </div>
            <div style="display: flex; align-items: flex-start; gap: 7px; margin-bottom: 6px;">
              <i class="fa-solid fa-circle-check" style="color: #10B981; font-size: 12px; margin-top: 2px;"></i>
              <span>Áp dụng đồng thời cùng chương trình <strong>Trả góp 0%</strong> và <strong>Thu cũ đổi mới</strong>.</span>
            </div>
            <div style="display: flex; align-items: flex-start; gap: 7px;">
              <i class="fa-solid fa-circle-check" style="color: #10B981; font-size: 12px; margin-top: 2px;"></i>
              <span>Bảo hành chính hãng 12 - 24 tháng, <strong>1 đổi 1 trong 30 ngày</strong> đầu tiên.</span>
            </div>
          </div>

          <!-- Nút hành động -->
          <div style="display: flex; gap: 9px;">
            <button type="button" id="promo-shop-now-btn" style="flex: 1; background: linear-gradient(135deg, #E11B1E 0%, #BE123C 100%); color: #fff; border: none; padding: 11px 15px; border-radius: 13px; font-size: 12.5px; font-weight: 800; cursor: pointer; text-transform: uppercase; letter-spacing: 0.5px; box-shadow: 0 5px 14px rgba(225,27,30,0.3); transition: transform 0.15s;">
              Mua Sắm Ngay
            </button>
            <button type="button" id="promo-modal-cancel-btn" style="background: #F1F5F9; color: #475569; border: none; padding: 11px 16px; border-radius: 13px; font-size: 12.5px; font-weight: 700; cursor: pointer; transition: background 0.15s;">
              Đóng
            </button>
          </div>
        </div>
      </div>
    `;

    document.body.appendChild(container);
    document.body.appendChild(miniBtn);
    document.body.appendChild(modal);

    // Gắn sự kiện
    setupEvents(container, miniBtn, modal);
  }

  // Thiết lập sự kiện
  function setupEvents(container, miniBtn, modal) {
    const wrapper = document.getElementById('floating-promo-wrapper');
    const closeBtn = document.getElementById('floating-promo-close-btn');
    const modalCloseBtn = document.getElementById('promo-modal-close-btn');
    const modalCancelBtn = document.getElementById('promo-modal-cancel-btn');
    const copyBtn = document.getElementById('promo-copy-voucher-btn');
    const shopNowBtn = document.getElementById('promo-shop-now-btn');

    // 1. Click vào thân sticker -> Mở Modal chi tiết
    if (wrapper) {
      wrapper.addEventListener('click', function (e) {
        if (e.target.closest('#floating-promo-close-btn')) return;
        openModal();
      });
    }

    // 2. Click nút đóng (X) -> Ẩn sticker và hiện nút tròn mini
    if (closeBtn) {
      closeBtn.addEventListener('click', function (e) {
        e.stopPropagation();
        closeSticker();
      });
    }

    // 3. Click nút mini -> Hiện lại sticker
    if (miniBtn) {
      miniBtn.addEventListener('click', function () {
        restoreSticker();
      });
    }

    // 4. Đóng modal
    if (modalCloseBtn) modalCloseBtn.addEventListener('click', closeModal);
    if (modalCancelBtn) modalCancelBtn.addEventListener('click', closeModal);
    if (modal) {
      modal.addEventListener('click', function (e) {
        if (e.target === modal) closeModal();
      });
    }

    // 5. Copy Voucher Code
    if (copyBtn) {
      copyBtn.addEventListener('click', function () {
        navigator.clipboard.writeText(VOUCHER_CODE).then(function () {
          const txt = document.getElementById('promo-copy-text');
          if (txt) txt.textContent = 'Đã chép!';
          copyBtn.style.background = '#10B981';
          setTimeout(function () {
            if (txt) txt.textContent = 'Sao chép';
            copyBtn.style.background = '#0057B7';
          }, 2500);
        }).catch(function () {
          alert('Mã giảm giá của bạn là: ' + VOUCHER_CODE);
        });
      });
    }

    // 6. Nút Mua Sắm Ngay
    if (shopNowBtn) {
      shopNowBtn.addEventListener('click', function () {
        closeModal();
        const isInsidePages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
        const target = (isInsidePages ? '' : 'pages/') + 'category.html?type=dienthoai';
        window.location.href = target;
      });
    }

    // Kiểm tra trạng thái đã đóng trước đó trong phiên duyệt
    try {
      const isHidden = sessionStorage.getItem(HIDE_STORAGE_KEY);
      if (isHidden === '1') {
        container.style.display = 'none';
        miniBtn.style.display = 'flex';
      }
    } catch (e) {}
  }

  function openModal() {
    const modal = document.getElementById('floating-promo-modal');
    if (modal) {
      modal.style.display = 'flex';
      document.body.style.overflow = 'hidden';
    }
  }

  function closeModal() {
    const modal = document.getElementById('floating-promo-modal');
    if (modal) {
      modal.style.display = 'none';
      document.body.style.overflow = '';
    }
  }

  function closeSticker() {
    const container = document.getElementById('floating-promo-container');
    const miniBtn = document.getElementById('floating-promo-mini-btn');

    if (container) {
      container.style.opacity = '0';
      container.style.transform = 'scale(0.5)';
      setTimeout(function () {
        container.style.display = 'none';
        if (miniBtn) {
          miniBtn.style.display = 'flex';
          miniBtn.style.opacity = '0';
          miniBtn.style.transform = 'scale(0.5)';
          setTimeout(function () {
            miniBtn.style.opacity = '1';
            miniBtn.style.transform = 'scale(1)';
          }, 50);
        }
      }, 200);
    }

    try {
      sessionStorage.setItem(HIDE_STORAGE_KEY, '1');
    } catch (e) {}
  }

  function restoreSticker() {
    const container = document.getElementById('floating-promo-container');
    const miniBtn = document.getElementById('floating-promo-mini-btn');

    if (miniBtn) {
      miniBtn.style.opacity = '0';
      miniBtn.style.transform = 'scale(0.5)';
      setTimeout(function () {
        miniBtn.style.display = 'none';
      }, 150);
    }

    if (container) {
      container.style.display = 'flex';
      container.style.opacity = '0';
      container.style.transform = 'scale(0.8)';
      setTimeout(function () {
        container.style.opacity = '1';
        container.style.transform = 'scale(1)';
      }, 50);
    }

    try {
      sessionStorage.removeItem(HIDE_STORAGE_KEY);
    } catch (e) {}
  }

  // Khởi tạo khi DOM sẵn sàng
  function init() {
    injectStyles();
    createDOMElements();
  }

  // API toàn cục
  window.FloatingPromo = {
    openModal: openModal,
    closeModal: closeModal,
    close: closeSticker,
    restore: restoreSticker
  };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
