/**
 * SamaMobile / CellphoneS - BỘ SO SÁNH SẢN PHẨM ĐỐI ĐẦU (PRODUCT COMPARISON)
 * Cho phép so sánh trực diện 2 đến 3 sản phẩm: Màn hình, Chip, RAM, ROM, Camera, Pin, Giá bán.
 */
(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.ProductCompare = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  const STORAGE_KEY = 'samamobile_compare_items';
  const MAX_COMPARE = 3;

  class CompareManager {
    constructor() {
      this.items = this.loadItems();
      this.initEvents();
    }

    loadItems() {
      try {
        const raw = sessionStorage.getItem(STORAGE_KEY);
        return raw ? JSON.parse(raw) : [];
      } catch (e) {
        return [];
      }
    }

    saveItems() {
      try {
        sessionStorage.setItem(STORAGE_KEY, JSON.stringify(this.items));
      } catch (e) {}
      this.renderDock();
      this.syncCardButtons();
    }

    initEvents() {
      document.addEventListener('DOMContentLoaded', () => {
        this.renderDock();
        this.syncCardButtons();
      });
    }

    isInCompare(maHang) {
      return this.items.some(x => String(x.maHang) === String(maHang));
    }

    toggleCompare(maHang, event) {
      if (event) {
        event.stopPropagation();
        event.preventDefault();
      }

      if (this.isInCompare(maHang)) {
        this.remove(maHang);
      } else {
        this.add(maHang);
      }
    }

    findProduct(maHang) {
      const data = (window.CellphoneSData || window.StoreData);
      if (data && typeof data.getProductById === 'function') {
        const p = data.getProductById(maHang);
        if (p) return p;
      }
      if (data && Array.isArray(data.products)) {
        const p = data.products.find(x => String(x.maHang) === String(maHang));
        if (p) return p;
      }
      return null;
    }

    add(maHang) {
      if (this.items.length >= MAX_COMPARE) {
        this.showToast(`Bạn chỉ có thể so sánh tối đa ${MAX_COMPARE} sản phẩm cùng lúc.`, 'warning');
        return;
      }

      const p = this.findProduct(maHang);
      if (!p) {
        this.showToast('Không tìm thấy thông tin sản phẩm này.', 'error');
        return;
      }

      this.items.push({
        maHang: p.maHang,
        tenHang: p.tenHang,
        donGia: Number(p.donGia) || 0,
        giaGoc: Number(p.giaGoc) || (Number(p.donGia) * 1.15),
        anhChinh: p.anhChinh || '',
        thongSo: p.thongSo || {},
        thongSoNoiBat: p.thongSoNoiBat || {},
        thongSoChiTiet: p.thongSoChiTiet || {}
      });

      this.saveItems();
      this.showToast(`Đã thêm "${p.tenHang}" vào bảng so sánh!`, 'success');
    }

    remove(maHang) {
      this.items = this.items.filter(x => String(x.maHang) !== String(maHang));
      this.saveItems();
      if (this.isModalOpen()) {
        if (this.items.length === 0) {
          this.closeModal();
        } else {
          this.renderModalContent();
        }
      }
    }

    clearAll() {
      this.items = [];
      this.saveItems();
      if (this.isModalOpen()) {
        this.closeModal();
      }
    }

    syncCardButtons() {
      const btns = document.querySelectorAll('.btn-compare-product');
      btns.forEach(btn => {
        const ma = btn.dataset.maHang;
        if (!ma) return;
        const active = this.isInCompare(ma);
        if (active) {
          btn.classList.add('bg-blue-600', 'text-white', 'border-blue-600');
          btn.classList.remove('bg-white/90', 'text-slate-700', 'border-slate-200');
          btn.innerHTML = '<i class="fa-solid fa-check text-[10px]"></i><span>Đã so sánh</span>';
        } else {
          btn.classList.remove('bg-blue-600', 'text-white', 'border-blue-600');
          btn.classList.add('bg-white/90', 'text-slate-700', 'border-slate-200');
          btn.innerHTML = '<i class="fa-solid fa-code-compare text-[10px]"></i><span>So sánh</span>';
        }
      });
    }

    renderDock() {
      let dock = document.getElementById('compare-floating-dock');
      if (!dock) return;

      if (this.items.length === 0) {
        dock.classList.add('translate-y-32', 'opacity-0', 'pointer-events-none');
        dock.classList.remove('translate-y-0', 'opacity-100');
        return;
      }

      dock.classList.remove('translate-y-32', 'opacity-0', 'pointer-events-none');
      dock.classList.add('translate-y-0', 'opacity-100');

      const countEl = document.getElementById('compare-dock-count');
      if (countEl) countEl.textContent = this.items.length;

      const listEl = document.getElementById('compare-dock-items');
      if (listEl) {
        listEl.innerHTML = this.items.map(item => `
          <div class="relative flex items-center gap-2 bg-slate-800/90 border border-slate-700 rounded-xl p-1.5 pr-2.5 max-w-[200px] shadow-sm animate-in fade-in zoom-in-95">
            <img src="${item.anhChinh || 'images/banner/iphone-18-cat-082925094.png'}" alt="${item.tenHang}" class="w-9 h-9 object-contain bg-white rounded-lg p-0.5" onerror="this.src='images/banner/iphone-18-cat-082925094.png'">
            <div class="truncate text-[11px] font-bold text-white flex-1 min-w-0 leading-tight">
              <span class="truncate block">${item.tenHang}</span>
              <span class="text-rose-400 font-mono text-[10px]">${this.formatMoney(item.donGia)}</span>
            </div>
            <button type="button" onclick="window.ProductCompare.remove('${item.maHang}')" class="w-5 h-5 rounded-full bg-slate-700 hover:bg-rose-600 text-slate-300 hover:text-white flex items-center justify-center text-[10px] cursor-pointer transition-colors" title="Bỏ chọn">
              <i class="fa-solid fa-xmark"></i>
            </button>
          </div>
        `).join('');

        // Thêm slot trống nếu chưa đủ 3 máy
        for (let i = this.items.length; i < MAX_COMPARE; i++) {
          listEl.innerHTML += `
            <div class="hidden sm:flex items-center justify-center border border-dashed border-slate-700 rounded-xl px-3 py-2 text-[11px] text-slate-400 gap-1.5 h-12">
              <i class="fa-solid fa-plus text-[10px]"></i>
              <span>Thêm sản phẩm</span>
            </div>
          `;
        }
      }
    }

    openModal() {
      if (this.items.length === 0) {
        this.showToast('Vui lòng chọn ít nhất 1 sản phẩm để so sánh!', 'warning');
        return;
      }

      const modal = document.getElementById('compare-modal');
      if (!modal) return;

      this.renderModalContent();
      modal.classList.remove('hidden');
      document.body.style.overflow = 'hidden';
    }

    closeModal() {
      const modal = document.getElementById('compare-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    isModalOpen() {
      const modal = document.getElementById('compare-modal');
      return modal && !modal.classList.contains('hidden');
    }

    getSpecValue(p, key) {
      const specs = p.thongSoChiTiet || p.thongSo || {};
      const highlight = p.thongSoNoiBat || {};
      const name = (p.tenHang || '').toLowerCase();

      switch (key) {
        case 'man_hinh':
          return specs['Màn hình'] || highlight['Kích thước màn hình'] || specs['Kích thước màn hình'] || (name.includes('iphone') ? '6.9" Super Retina XDR OLED 120Hz' : '6.8" Dynamic AMOLED 2X 120Hz');
        case 'chip':
          return specs['Vi xử lý'] || highlight['Vi xử lý'] || specs['Chip'] || (name.includes('iphone') ? 'Apple A18 / A19 Pro (3nm)' : 'Qualcomm Snapdragon 8 Elite');
        case 'ram':
          return specs['RAM'] || highlight['RAM'] || (name.includes('iphone 16 pro') ? '8GB LPDDR5X' : '12GB LPDDR5X');
        case 'rom':
          return specs['Bộ nhớ trong'] || highlight['Bộ nhớ trong'] || specs['ROM'] || '256GB NVMe / UFS 4.0';
        case 'camera_sau':
          return specs['Camera sau'] || (name.includes('iphone') ? '48MP Fusion + 48MP Ultra Wide + 12MP Tele 5x' : '200MP Chính + 50MP Periscope + 12MP Ultra Wide');
        case 'camera_truoc':
          return specs['Camera trước'] || (name.includes('iphone') ? '12MP TrueDepth Autofocus, 4K Dolby Vision' : '12MP Dual Pixel AF');
        case 'pin':
          return specs['Pin & Sạc'] || highlight['Dung lượng pin hiển thị'] || specs['Dung lượng pin'] || (name.includes('iphone') ? '4.850 mAh, Sạc nhanh 45W, MagSafe 25W' : '5.000 mAh, Sạc siêu nhanh 45W');
        case 'os':
          return specs['Hệ điều hành'] || (name.includes('iphone') ? 'iOS 19 (Apple Intelligence)' : 'Android 15 (Galaxy AI)');
        case 'tinh_nang':
          return specs['Chuẩn kháng nước/bụi'] || specs['Khung viền & Chất liệu'] || 'Kháng nước/bụi IP68, Khung viền Titanium cao cấp';
        default:
          return 'Đang cập nhật';
      }
    }

    renderModalContent() {
      const container = document.getElementById('compare-modal-content');
      if (!container) return;

      const colClass = this.items.length === 1 ? 'grid-cols-1' : (this.items.length === 2 ? 'grid-cols-2' : 'grid-cols-3');

      container.innerHTML = `
        <div class="overflow-x-auto custom-scrollbar">
          <div class="min-w-[650px] divide-y divide-slate-200">
            
            <!-- HÀNG 1: HEADER THÔNG TIN MÁY & NÚT MUA -->
            <div class="grid ${colClass} gap-4 p-4 bg-slate-50 sticky top-0 z-20 shadow-xs">
              ${this.items.map(p => `
                <div class="flex flex-col items-center text-center p-3 bg-white rounded-2xl border border-slate-200/90 shadow-sm relative group">
                  <button type="button" onclick="window.ProductCompare.remove('${p.maHang}')" class="absolute top-2 right-2 w-6 h-6 rounded-full bg-slate-100 hover:bg-rose-500 hover:text-white text-slate-400 flex items-center justify-center text-xs transition-colors" title="Bỏ so sánh">
                    <i class="fa-solid fa-xmark"></i>
                  </button>
                  <img src="${p.anhChinh || 'images/banner/iphone-18-cat-082925094.png'}" alt="${p.tenHang}" class="w-24 h-24 object-contain mb-2 group-hover:scale-105 transition-transform" onerror="this.src='images/banner/iphone-18-cat-082925094.png'">
                  <h4 class="font-extrabold text-xs sm:text-sm text-slate-900 line-clamp-2 min-h-[34px] mb-1.5" title="${p.tenHang}">${p.tenHang}</h4>
                  <div class="text-rose-600 font-black font-mono text-sm sm:text-base mb-2">${this.formatMoney(p.donGia)}</div>
                  <div class="flex items-center gap-1.5 w-full mt-auto">
                    <button type="button" onclick="window.quickAddToCart('${p.maHang}', event)" class="flex-1 py-2 px-2.5 rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-bold text-xs shadow-md transition-all flex items-center justify-center gap-1.5">
                      <i class="fa-solid fa-cart-plus text-[11px]"></i>
                      <span>Mua ngay</span>
                    </button>
                    <button type="button" onclick="window.openTradeInModal('${p.maHang}', event)" class="py-2 px-2.5 rounded-xl bg-amber-50 hover:bg-amber-100 text-amber-700 border border-amber-200 font-bold text-xs transition-colors" title="Thu cũ lên đời máy này">
                      <i class="fa-solid fa-arrows-rotate"></i>
                    </button>
                  </div>
                </div>
              `).join('')}
            </div>

            <!-- CÁC HÀNG THÔNG SỐ SO SÁNH -->
            <div class="p-4 space-y-4">
              
              <!-- Nhóm 1: Màn hình -->
              <div class="rounded-xl border border-slate-200 overflow-hidden">
                <div class="bg-blue-50 px-3 py-1.5 text-xs font-bold text-blue-900 flex items-center gap-2 border-b border-blue-100">
                  <i class="fa-solid fa-mobile-screen text-blue-600"></i>
                  <span>Màn hình hiển thị</span>
                </div>
                <div class="grid ${colClass} divide-x divide-slate-100 bg-white">
                  ${this.items.map(p => `
                    <div class="p-3 text-xs text-slate-800 leading-relaxed font-medium">
                      ${this.getSpecValue(p, 'man_hinh')}
                    </div>
                  `).join('')}
                </div>
              </div>

              <!-- Nhóm 2: Vi xử lý & Hiệu năng -->
              <div class="rounded-xl border border-slate-200 overflow-hidden">
                <div class="bg-purple-50 px-3 py-1.5 text-xs font-bold text-purple-900 flex items-center gap-2 border-b border-purple-100">
                  <i class="fa-solid fa-microchip text-purple-600"></i>
                  <span>Vi xử lý (CPU) & RAM</span>
                </div>
                <div class="grid ${colClass} divide-x divide-slate-100 bg-white">
                  ${this.items.map(p => `
                    <div class="p-3 text-xs space-y-1.5">
                      <div><strong class="text-slate-500 text-[11px]">Chipset:</strong> <span class="font-bold text-slate-900">${this.getSpecValue(p, 'chip')}</span></div>
                      <div><strong class="text-slate-500 text-[11px]">RAM:</strong> <span class="font-bold text-purple-700">${this.getSpecValue(p, 'ram')}</span></div>
                      <div><strong class="text-slate-500 text-[11px]">Bộ nhớ:</strong> <span class="font-semibold text-slate-800">${this.getSpecValue(p, 'rom')}</span></div>
                    </div>
                  `).join('')}
                </div>
              </div>

              <!-- Nhóm 3: Camera -->
              <div class="rounded-xl border border-slate-200 overflow-hidden">
                <div class="bg-emerald-50 px-3 py-1.5 text-xs font-bold text-emerald-900 flex items-center gap-2 border-b border-emerald-100">
                  <i class="fa-solid fa-camera text-emerald-600"></i>
                  <span>Hệ thống Máy ảnh (Camera)</span>
                </div>
                <div class="grid ${colClass} divide-x divide-slate-100 bg-white">
                  ${this.items.map(p => `
                    <div class="p-3 text-xs space-y-1.5">
                      <div><strong class="text-slate-500 text-[11px]">Camera sau:</strong> <span class="font-semibold text-slate-800">${this.getSpecValue(p, 'camera_sau')}</span></div>
                      <div><strong class="text-slate-500 text-[11px]">Camera trước:</strong> <span class="font-semibold text-slate-800">${this.getSpecValue(p, 'camera_truoc')}</span></div>
                    </div>
                  `).join('')}
                </div>
              </div>

              <!-- Nhóm 4: Pin & Sạc -->
              <div class="rounded-xl border border-slate-200 overflow-hidden">
                <div class="bg-amber-50 px-3 py-1.5 text-xs font-bold text-amber-900 flex items-center gap-2 border-b border-amber-100">
                  <i class="fa-solid fa-battery-three-quarters text-amber-600"></i>
                  <span>Dung lượng Pin & Công nghệ Sạc</span>
                </div>
                <div class="grid ${colClass} divide-x divide-slate-100 bg-white">
                  ${this.items.map(p => `
                    <div class="p-3 text-xs text-slate-800 font-semibold leading-relaxed">
                      ${this.getSpecValue(p, 'pin')}
                    </div>
                  `).join('')}
                </div>
              </div>

              <!-- Nhóm 5: Tính năng đặc biệt & Hệ điều hành -->
              <div class="rounded-xl border border-slate-200 overflow-hidden">
                <div class="bg-slate-100 px-3 py-1.5 text-xs font-bold text-slate-800 flex items-center gap-2 border-b border-slate-200">
                  <i class="fa-solid fa-layer-group text-slate-600"></i>
                  <span>Hệ điều hành & Chuẩn kháng nước</span>
                </div>
                <div class="grid ${colClass} divide-x divide-slate-100 bg-white">
                  ${this.items.map(p => `
                    <div class="p-3 text-xs space-y-1">
                      <div><strong class="text-slate-500 text-[11px]">Hệ điều hành:</strong> <span class="font-bold text-blue-700">${this.getSpecValue(p, 'os')}</span></div>
                      <div><strong class="text-slate-500 text-[11px]">Đặc điểm:</strong> <span class="text-slate-700">${this.getSpecValue(p, 'tinh_nang')}</span></div>
                    </div>
                  `).join('')}
                </div>
              </div>

            </div>

          </div>
        </div>
      `;
    }

    formatMoney(val) {
      if (!val || isNaN(val)) return '0₫';
      return Number(val).toLocaleString('vi-VN') + '₫';
    }

    showToast(msg, type = 'info') {
      const toast = document.createElement('div');
      const bg = type === 'success' ? 'bg-emerald-600' : (type === 'warning' ? 'bg-amber-600' : 'bg-slate-800');
      toast.className = `fixed top-20 right-4 z-50 px-4 py-2.5 rounded-2xl text-white font-bold text-xs shadow-2xl flex items-center gap-2 ${bg} animate-in fade-in slide-in-from-top-4 duration-200`;
      toast.innerHTML = `<i class="fa-solid ${type === 'success' ? 'fa-circle-check' : 'fa-bell'}"></i><span>${msg}</span>`;
      document.body.appendChild(toast);
      setTimeout(() => {
        toast.classList.add('opacity-0', 'transition-opacity');
        setTimeout(() => toast.remove(), 300);
      }, 2500);
    }
  }

  const manager = new CompareManager();
  window.ProductCompare = manager;
  window.toggleCompareProduct = (id, e) => manager.toggleCompare(id, e);
  return manager;
}));
