/**
 * SAMAMOBILE / CELLPHONES - BỘ SO SÁNH SẢN PHẨM ĐỐI ĐẦU (PRODUCT COMPARISON)
 * Chuẩn CellphoneS 100% Khớp Screenshot Người Dùng:
 * 1. Floating Dock màu trắng thanh lịch với 3 slots (Slot đã chọn có ảnh, tên & nút xóa x; Slot trống có viền đứt nét + "Chọn sản phẩm so sánh").
 * 2. Cụm điều khiển: "Đã chọn X sản phẩm" + Nút "Thu gọn" + Nút "So sánh" (Đỏ #E11B22).
 * 3. Chế độ thu gọn thành mini-pill nổi góc phải (So sánh (X)).
 * 4. Modal "Chọn sản phẩm so sánh" tìm kiếm trực quan theo tên, giá đỏ, nút "Chọn".
 * 5. Bảng so sánh chi tiết đối đầu 2-3 sản phẩm (Màn hình, Chip, RAM, ROM, Camera, Pin, Sạc, OS).
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
  const COLLAPSED_KEY = 'samamobile_compare_collapsed';
  const MAX_COMPARE = 3;

  function fixProductImage(path) {
    if (!path) return '../images/banner/iphone-18-cat-082925094.png';
    if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:') || path.startsWith('//')) {
      return path;
    }
    const clean = path.replace(/^\.\.\//, '').replace(/^\//, '');
    const isInsidePages = window.location.pathname.includes('/pages/') || window.location.href.includes('/pages/');
    return isInsidePages ? ('../' + clean) : clean;
  }

  class CompareManager {
    constructor() {
      this.items = this.loadItems();
      this.isCollapsed = this.loadCollapsedState();
      this.catalog = [];
      this.catalogLoaded = false;
      this.activeSlot = null;
      this.searchKeyword = '';
      this.selectedCategory = 'ALL';
      this.init();
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

    loadCollapsedState() {
      try {
        return sessionStorage.getItem(COLLAPSED_KEY) === 'true';
      } catch (e) {
        return false;
      }
    }

    saveCollapsedState(state) {
      this.isCollapsed = !!state;
      try {
        sessionStorage.setItem(COLLAPSED_KEY, this.isCollapsed ? 'true' : 'false');
      } catch (e) {}
      this.renderDock();
    }

    init() {
      const run = () => {
        this.ensureDomElements();
        this.loadCatalog();
        this.renderDock();
        this.syncCardButtons();
        this.bindGlobalKeys();
      };

      if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', run);
      } else {
        run();
      }
    }

    async loadCatalog() {
      if (this.catalogLoaded && this.catalog.length > 0) return;

      // 1. Check window.allProductsMeta
      if (window.allProductsMeta && typeof window.allProductsMeta === 'object') {
        this.ingestCatalogMap(window.allProductsMeta);
        return;
      }

      // 2. Fetch from JSON
      const candidates = [
        '../data/products-meta.json',
        'data/products-meta.json',
        '/frontend/data/products-meta.json',
        '../../data/products-meta.json'
      ];

      for (const url of candidates) {
        try {
          const res = await fetch(url);
          if (res.ok) {
            const data = await res.json();
            if (data && typeof data === 'object') {
              this.ingestCatalogMap(data);
              return;
            }
          }
        } catch (e) {}
      }
    }

    ingestCatalogMap(map) {
      if (!map) return;
      const list = Array.isArray(map) ? map : Object.values(map);
      this.catalog = list.map(p => ({
        maHang: String(p.maHang || p.id || ''),
        tenHang: p.tenHang || p.name || 'Sản phẩm',
        nhanHieu: p.nhanHieu || p.brand || '',
        maNhom: p.maNhom || '',
        tenNhom: p.tenNhom || '',
        donGia: Number(p.donGia || p.price) || 0,
        giaGoc: Number(p.giaGoc || p.originalPrice) || (Number(p.donGia || p.price || 0) * 1.15),
        anhChinh: fixProductImage(p.anhChinh || p.image || ''),
        thongSo: p.thongSo || {},
        thongSoNoiBat: p.thongSoNoiBat || {},
        thongSoChiTiet: p.thongSoChiTiet || {}
      })).filter(x => x.maHang && x.donGia > 0);
      this.catalogLoaded = true;
    }

    ensureDomElements() {
      // 1. Dock Container
      let dock = document.getElementById('compare-floating-dock');
      if (!dock) {
        dock = document.createElement('div');
        dock.id = 'compare-floating-dock';
        dock.className = 'fixed bottom-4 sm:bottom-6 left-1/2 -translate-x-1/2 z-[9990] transition-all duration-300 w-[95%] max-w-4xl bg-white rounded-2xl sm:rounded-3xl shadow-2xl border border-slate-200/90 p-3 sm:p-4 translate-y-36 opacity-0 pointer-events-none font-sans';
        document.body.appendChild(dock);
      } else {
        // Upgrade existing dock classes
        dock.className = 'fixed bottom-4 sm:bottom-6 left-1/2 -translate-x-1/2 z-[9990] transition-all duration-300 w-[95%] max-w-4xl bg-white rounded-2xl sm:rounded-3xl shadow-2xl border border-slate-200/90 p-3 sm:p-4 font-sans';
      }

      // 2. Mini Pill for collapsed state
      let pill = document.getElementById('compare-mini-pill');
      if (!pill) {
        pill = document.createElement('div');
        pill.id = 'compare-mini-pill';
        pill.className = 'fixed bottom-5 right-5 z-[9990] transition-all duration-300 translate-y-24 opacity-0 pointer-events-none font-sans';
        document.body.appendChild(pill);
      }

      // 3. Search Modal for picking comparison products
      let searchModal = document.getElementById('compare-search-modal');
      if (!searchModal) {
        searchModal = document.createElement('div');
        searchModal.id = 'compare-search-modal';
        searchModal.className = 'hidden fixed inset-0 z-[10000] flex items-center justify-center p-3 sm:p-4 bg-slate-950/60 backdrop-blur-xs font-sans';
        searchModal.innerHTML = `
          <div class="bg-white w-full max-w-xl rounded-3xl shadow-2xl border border-slate-200 overflow-hidden flex flex-col max-h-[85vh] animate-in fade-in zoom-in-95 duration-200 my-auto">
            <!-- Modal Header -->
            <div class="p-4 sm:px-6 sm:py-4 border-b border-slate-100 flex items-center justify-between bg-slate-50/70">
              <h3 class="font-extrabold text-slate-900 text-sm sm:text-base flex items-center gap-2">
                <i class="fa-solid fa-code-compare text-[#E11B22]"></i>
                <span>Chọn sản phẩm so sánh</span>
              </h3>
              <button type="button" onclick="window.ProductCompare.closeSearchModal()" class="w-8 h-8 rounded-full hover:bg-slate-200/60 text-slate-400 hover:text-slate-700 flex items-center justify-center transition-colors cursor-pointer" aria-label="Đóng">
                <i class="fa-solid fa-xmark text-sm"></i>
              </button>
            </div>
            <!-- Search & Filters -->
            <div class="p-4 pb-2 border-b border-slate-100 bg-white space-y-2.5">
              <div class="relative">
                <i class="fa-solid fa-magnifying-glass absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-xs sm:text-sm"></i>
                <input type="text" id="compare-search-input" placeholder="Tìm sản phẩm muốn so sánh (tên, hãng, cấu hình)..." class="w-full pl-9 pr-9 py-2.5 rounded-xl bg-slate-100/90 border border-slate-200 focus:border-[#E11B22] focus:bg-white text-xs sm:text-sm text-slate-800 outline-none transition-all placeholder:text-slate-400">
                <button type="button" id="compare-search-clear" onclick="window.ProductCompare.clearSearchInput()" class="hidden absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 text-xs cursor-pointer">
                  <i class="fa-solid fa-circle-xmark"></i>
                </button>
              </div>
              <!-- Category Tabs -->
              <div id="compare-category-tabs" class="flex items-center gap-1.5 overflow-x-auto no-scrollbar py-1 text-xs">
                <button type="button" onclick="window.ProductCompare.setSearchCategory('ALL')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-blue-600 text-white" data-cat="ALL">Tất cả</button>
                <button type="button" onclick="window.ProductCompare.setSearchCategory('DIENTHOAI')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200" data-cat="DIENTHOAI">Điện thoại</button>
                <button type="button" onclick="window.ProductCompare.setSearchCategory('TABLET')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200" data-cat="TABLET">Tablet</button>
                <button type="button" onclick="window.ProductCompare.setSearchCategory('LAPTOP')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200" data-cat="LAPTOP">Laptop</button>
                <button type="button" onclick="window.ProductCompare.setSearchCategory('APPLE')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200" data-cat="APPLE">Apple</button>
                <button type="button" onclick="window.ProductCompare.setSearchCategory('SAMSUNG')" class="compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200" data-cat="SAMSUNG">Samsung</button>
              </div>
            </div>
            <!-- Search Results List -->
            <div id="compare-search-results" class="flex-1 overflow-y-auto p-3 space-y-2 max-h-[50vh] custom-scrollbar divide-y divide-slate-100">
              <div class="p-8 text-center text-slate-400 text-xs">Đang tải danh sách sản phẩm...</div>
            </div>
          </div>
        `;
        document.body.appendChild(searchModal);

        // Bind search input events
        const input = document.getElementById('compare-search-input');
        const clearBtn = document.getElementById('compare-search-clear');
        if (input) {
          input.addEventListener('input', (e) => {
            this.searchKeyword = e.target.value.trim();
            if (clearBtn) clearBtn.classList.toggle('hidden', !this.searchKeyword);
            this.renderSearchResults();
          });
        }
      }

      // 4. Comparison Specs Modal
      let compareModal = document.getElementById('compare-modal');
      if (!compareModal) {
        compareModal = document.createElement('div');
        compareModal.id = 'compare-modal';
        compareModal.className = 'hidden fixed inset-0 z-[10000] flex items-center justify-center p-2 sm:p-4 bg-slate-950/80 backdrop-blur-md overflow-y-auto font-sans';
        compareModal.innerHTML = `
          <div class="bg-white w-full max-w-5xl rounded-3xl shadow-2xl border border-slate-200 overflow-hidden my-4 max-h-[94vh] flex flex-col animate-in fade-in zoom-in-95 duration-200">
            <div class="p-4 sm:px-6 sm:py-4 bg-gradient-to-r from-slate-900 via-blue-950 to-slate-900 text-white flex items-center justify-between border-b border-slate-800">
              <div class="flex items-center gap-2.5">
                <div class="w-8 h-8 rounded-xl bg-[#E11B22] flex items-center justify-center text-sm shadow-md text-white">
                  <i class="fa-solid fa-code-compare"></i>
                </div>
                <div>
                  <h3 class="font-extrabold text-sm sm:text-base">So Sánh Sản Phẩm Đối Đầu</h3>
                  <p class="text-[11px] text-slate-400">Đối chiếu chi tiết cấu hình, camera, pin và giá bán</p>
                </div>
              </div>
              <button type="button" onclick="window.ProductCompare.closeModal()" class="w-8 h-8 rounded-xl bg-slate-800 hover:bg-rose-600 text-slate-300 hover:text-white flex items-center justify-center text-sm transition-colors cursor-pointer" aria-label="Đóng">
                <i class="fa-solid fa-xmark"></i>
              </button>
            </div>
            <div id="compare-modal-content" class="flex-1 overflow-y-auto"></div>
          </div>
        `;
        document.body.appendChild(compareModal);
      }
    }

    bindGlobalKeys() {
      document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
          if (this.isSearchModalOpen()) this.closeSearchModal();
          else if (this.isModalOpen()) this.closeModal();
        }
      });
    }

    isInCompare(maHang) {
      if (!maHang) return false;
      return this.items.some(x => String(x.maHang).toLowerCase() === String(maHang).toLowerCase());
    }

    findProduct(maHang) {
      if (!maHang) return null;
      const clean = String(maHang).trim().toLowerCase();

      // 1. In items
      const fromItems = this.items.find(x => String(x.maHang).toLowerCase() === clean);
      if (fromItems) return fromItems;

      // 2. In catalog
      const fromCatalog = this.catalog.find(x => String(x.maHang).toLowerCase() === clean);
      if (fromCatalog) return fromCatalog;

      // 3. In window.allProductsMeta
      if (window.allProductsMeta) {
        if (window.allProductsMeta[maHang]) return window.allProductsMeta[maHang];
        const match = Object.values(window.allProductsMeta).find(p => String(p.maHang || p.id).toLowerCase() === clean);
        if (match) return match;
      }

      // 4. In window.currentProduct (on product detail page)
      if (window.currentProduct) {
        const cur = window.currentProduct;
        if (!maHang || String(cur.maHang).toLowerCase() === clean || String(cur.id).toLowerCase() === clean) {
          return cur;
        }
      }

      // 5. Check StoreData / CellphoneSData
      const store = window.CellphoneSData || window.StoreData;
      if (store) {
        if (typeof store.getProductById === 'function') {
          const p = store.getProductById(maHang);
          if (p) return p;
        }
        if (Array.isArray(store.products)) {
          const p = store.products.find(x => String(x.maHang || x.id).toLowerCase() === clean);
          if (p) return p;
        }
      }

      // 6. DOM fallback if on product detail page
      const titleEl = document.getElementById('product-title');
      if (titleEl && titleEl.textContent.trim()) {
        const priceEl = document.getElementById('product-final-price') || document.querySelector('.product-price');
        const imgEl = document.getElementById('main-product-img') || document.querySelector('#product-main-image img');
        return {
          maHang: maHang || 'CURRENT',
          tenHang: titleEl.textContent.trim(),
          donGia: priceEl ? parseInt(priceEl.textContent.replace(/[^\d]/g, ''), 10) || 0 : 0,
          giaGoc: 0,
          anhChinh: imgEl ? imgEl.src : ''
        };
      }

      return null;
    }

    startCompareWithCurrent(maHang) {
      // Helper called when user clicks "So sánh" on Product Detail page
      let targetId = maHang;
      if (!targetId) {
        const urlParams = new URLSearchParams(window.location.search);
        targetId = urlParams.get('id') || urlParams.get('maHang');
      }
      if (!targetId && window.currentProduct) {
        targetId = window.currentProduct.maHang || window.currentProduct.id;
      }

      if (targetId && !this.isInCompare(targetId)) {
        this.add(targetId, true);
      }

      // Expand dock
      this.saveCollapsedState(false);
      this.renderDock();

      // If only 1 item selected, automatically open search modal to pick item #2
      if (this.items.length < 2) {
        this.openSearchModal(1);
      } else {
        this.showToast(`Bảng so sánh đã có ${this.items.length} sản phẩm. Bấm "So sánh" để xem chi tiết!`, 'success');
      }
    }

    add(maHang, silent = false) {
      if (this.items.length >= MAX_COMPARE) {
        this.showToast(`Bạn chỉ có thể so sánh tối đa ${MAX_COMPARE} sản phẩm cùng lúc.`, 'warning');
        return;
      }

      if (this.isInCompare(maHang)) {
        if (!silent) this.showToast('Sản phẩm này đã có trong bảng so sánh.', 'info');
        return;
      }

      let p = this.findProduct(maHang);
      if (!p) {
        this.showToast('Không tìm thấy thông tin sản phẩm.', 'error');
        return;
      }

      const item = {
        maHang: String(p.maHang || p.id),
        tenHang: p.tenHang || p.name || 'Sản phẩm',
        donGia: Number(p.donGia || p.price) || 0,
        giaGoc: Number(p.giaGoc || p.originalPrice) || (Number(p.donGia || p.price || 0) * 1.15),
        anhChinh: fixProductImage(p.anhChinh || p.image || ''),
        thongSo: p.thongSo || {},
        thongSoNoiBat: p.thongSoNoiBat || {},
        thongSoChiTiet: p.thongSoChiTiet || {}
      };

      this.items.push(item);
      this.isCollapsed = false;
      this.saveItems();

      if (!silent) {
        this.showToast(`Đã thêm "${item.tenHang}" vào bảng so sánh!`, 'success');
      }
    }

    remove(maHang) {
      this.items = this.items.filter(x => String(x.maHang).toLowerCase() !== String(maHang).toLowerCase());
      this.saveItems();
      if (this.items.length === 0) {
        this.isCollapsed = false;
        if (this.isModalOpen()) this.closeModal();
      } else if (this.isModalOpen()) {
        this.renderModalContent();
      }
    }

    clearAll() {
      this.items = [];
      this.saveItems();
      if (this.isModalOpen()) this.closeModal();
      this.showToast('Đã xóa toàn bộ sản phẩm khỏi bảng so sánh.', 'info');
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

    collapseDock() {
      this.saveCollapsedState(true);
    }

    expandDock() {
      this.saveCollapsedState(false);
    }

    renderDock() {
      const dock = document.getElementById('compare-floating-dock');
      const pill = document.getElementById('compare-mini-pill');
      if (!dock || !pill) return;

      // 0 sản phẩm: ẩn cả hai
      if (this.items.length === 0) {
        dock.classList.add('translate-y-36', 'opacity-0', 'pointer-events-none');
        dock.classList.remove('translate-y-0', 'opacity-100');
        pill.classList.add('translate-y-24', 'opacity-0', 'pointer-events-none');
        pill.classList.remove('translate-y-0', 'opacity-100');
        return;
      }

      // Chế độ Thu gọn (collapsed) -> Hiện Mini Pill, ẩn dock
      if (this.isCollapsed) {
        dock.classList.add('translate-y-36', 'opacity-0', 'pointer-events-none');
        dock.classList.remove('translate-y-0', 'opacity-100');
        pill.classList.remove('translate-y-24', 'opacity-0', 'pointer-events-none');
        pill.classList.add('translate-y-0', 'opacity-100');
        pill.innerHTML = `
          <button type="button" onclick="window.ProductCompare.expandDock()" class="flex items-center gap-2.5 px-4 py-2.5 rounded-full bg-[#E11B22] hover:bg-[#c4151b] text-white font-bold text-xs sm:text-sm shadow-2xl transition-all cursor-pointer hover:shadow-red-500/40 hover:scale-105 active:scale-95">
            <i class="fa-solid fa-code-compare"></i>
            <span>So sánh (${this.items.length})</span>
            <i class="fa-solid fa-chevron-up text-[10px]"></i>
          </button>
        `;
        return;
      }

      // Chế độ Mở (expanded) -> Hiện Floating Dock, ẩn Mini Pill
      pill.classList.add('translate-y-24', 'opacity-0', 'pointer-events-none');
      pill.classList.remove('translate-y-0', 'opacity-100');
      dock.classList.remove('translate-y-36', 'opacity-0', 'pointer-events-none');
      dock.classList.add('translate-y-0', 'opacity-100');

      // Khớp chuẩn Screenshot 100%
      let slotsHtml = '';
      for (let i = 0; i < MAX_COMPARE; i++) {
        const item = this.items[i];
        if (item) {
          // Ô đã có sản phẩm (Slot filled)
          slotsHtml += `
            <div class="relative w-[130px] sm:w-[155px] h-[120px] sm:h-[130px] bg-white rounded-xl border border-slate-200 p-2 flex flex-col items-center justify-between text-center shadow-2xs group shrink-0">
              <button type="button" onclick="window.ProductCompare.remove('${item.maHang}')" class="absolute top-1.5 right-1.5 w-5 h-5 rounded-full bg-slate-100 hover:bg-rose-500 hover:text-white text-slate-400 flex items-center justify-center text-[10px] transition-colors cursor-pointer" title="Bỏ chọn">
                <i class="fa-solid fa-xmark"></i>
              </button>
              <div class="w-12 h-12 sm:w-14 sm:h-14 flex items-center justify-center p-0.5 mt-1">
                <img src="${item.anhChinh}" alt="${item.tenHang}" class="max-w-full max-h-full object-contain" onerror="this.src='../images/banner/iphone-18-cat-082925094.png'">
              </div>
              <div class="w-full text-center">
                <div class="text-[11px] font-semibold text-slate-800 line-clamp-2 leading-tight px-1" title="${item.tenHang}">
                  ${item.tenHang}
                </div>
              </div>
            </div>
          `;
        } else {
          // Ô trống có viền đứt nét (Slot empty)
          slotsHtml += `
            <button type="button" onclick="window.ProductCompare.openSearchModal(${i})" class="w-[130px] sm:w-[155px] h-[120px] sm:h-[130px] rounded-xl border-2 border-dashed border-slate-200 hover:border-blue-500 bg-white hover:bg-blue-50/20 p-2 flex flex-col items-center justify-center text-center transition-all cursor-pointer group shrink-0">
              <div class="w-7 h-7 sm:w-8 sm:h-8 rounded-full border border-slate-300 text-slate-400 group-hover:border-blue-500 group-hover:text-blue-500 flex items-center justify-center text-xs mb-1.5 transition-colors">
                <i class="fa-solid fa-plus"></i>
              </div>
              <span class="text-[11px] sm:text-xs text-slate-500 group-hover:text-blue-600 font-medium leading-tight px-1">
                Chọn sản phẩm so sánh
              </span>
            </button>
          `;
        }
      }

      dock.innerHTML = `
        <div class="flex flex-col sm:flex-row items-center justify-between gap-3 sm:gap-4">
          <!-- 3 SLOTS SO SÁNH -->
          <div class="flex items-center gap-2 sm:gap-3 flex-1 overflow-x-auto no-scrollbar w-full sm:w-auto py-0.5 justify-start sm:justify-start">
            ${slotsHtml}
          </div>

          <!-- BẢNG ĐIỀU KHIỂN BÊN PHẢI -->
          <div class="flex flex-row sm:flex-col items-center sm:items-end justify-between sm:justify-center w-full sm:w-auto shrink-0 pt-2 sm:pt-0 border-t sm:border-t-0 border-slate-100 gap-2 sm:gap-2.5">
            <div class="text-xs text-slate-600 font-medium whitespace-nowrap">
              Đã chọn <strong class="text-slate-900 font-bold">${this.items.length}</strong> sản phẩm
            </div>
            <div class="flex items-center gap-2">
              <button type="button" onclick="window.ProductCompare.collapseDock()" class="px-3.5 py-2 rounded-xl border border-slate-200 bg-white hover:bg-slate-100 text-slate-700 font-semibold text-xs transition-colors cursor-pointer shadow-2xs">
                Thu gọn
              </button>
              <button type="button" onclick="window.ProductCompare.openCompare()" class="px-4 py-2 rounded-xl bg-[#E11B22] hover:bg-[#c4151b] text-white font-bold text-xs shadow-md shadow-red-500/20 active:scale-95 transition-all cursor-pointer whitespace-nowrap">
                So sánh
              </button>
            </div>
          </div>
        </div>
      `;
    }

    openSearchModal(slotIndex = null) {
      this.activeSlot = slotIndex;
      const modal = document.getElementById('compare-search-modal');
      if (!modal) return;

      modal.classList.remove('hidden');
      document.body.style.overflow = 'hidden';

      // Load catalog if empty
      if (this.catalog.length === 0) {
        this.loadCatalog().then(() => this.renderSearchResults());
      } else {
        this.renderSearchResults();
      }

      // Auto focus search box
      setTimeout(() => {
        const input = document.getElementById('compare-search-input');
        if (input) input.focus();
      }, 100);
    }

    closeSearchModal() {
      const modal = document.getElementById('compare-search-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    isSearchModalOpen() {
      const modal = document.getElementById('compare-search-modal');
      return modal && !modal.classList.contains('hidden');
    }

    clearSearchInput() {
      const input = document.getElementById('compare-search-input');
      const clearBtn = document.getElementById('compare-search-clear');
      if (input) {
        input.value = '';
        input.focus();
      }
      if (clearBtn) clearBtn.classList.add('hidden');
      this.searchKeyword = '';
      this.renderSearchResults();
    }

    setSearchCategory(cat) {
      this.selectedCategory = cat;
      const buttons = document.querySelectorAll('.compare-cat-btn');
      buttons.forEach(btn => {
        if (btn.dataset.cat === cat) {
          btn.className = 'compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-blue-600 text-white';
        } else {
          btn.className = 'compare-cat-btn px-3 py-1 rounded-full font-bold text-[11px] whitespace-nowrap transition-colors bg-slate-100 text-slate-600 hover:bg-slate-200';
        }
      });
      this.renderSearchResults();
    }

    renderSearchResults() {
      const container = document.getElementById('compare-search-results');
      if (!container) return;

      let list = this.catalog.filter(p => !this.isInCompare(p.maHang));

      // Category filter
      if (this.selectedCategory !== 'ALL') {
        const cat = this.selectedCategory.toLowerCase();
        list = list.filter(p => {
          const hay = ((p.maNhom || '') + ' ' + (p.tenNhom || '') + ' ' + (p.nhanHieu || '') + ' ' + (p.tenHang || '')).toLowerCase();
          return hay.includes(cat);
        });
      }

      // Keyword filter
      if (this.searchKeyword) {
        const kw = this.searchKeyword.toLowerCase();
        list = list.filter(p => {
          const hay = (p.tenHang + ' ' + (p.nhanHieu || '') + ' ' + (p.tenNhom || '')).toLowerCase();
          return hay.includes(kw);
        });
      }

      if (list.length === 0) {
        container.innerHTML = `
          <div class="py-12 text-center text-slate-400 space-y-2">
            <i class="fa-solid fa-box-open text-3xl text-slate-300"></i>
            <p class="text-xs">Không tìm thấy sản phẩm phù hợp. Thử tìm từ khóa khác!</p>
          </div>
        `;
        return;
      }

      // Render top 40 matches
      const displayItems = list.slice(0, 40);
      container.innerHTML = displayItems.map(p => `
        <div class="flex items-center justify-between gap-3 p-2.5 rounded-2xl hover:bg-slate-50 transition-colors">
          <div class="flex items-center gap-3 min-w-0 flex-1">
            <img src="${p.anhChinh}" alt="${p.tenHang}" class="w-12 h-12 sm:w-14 sm:h-14 object-contain rounded-xl bg-white border border-slate-100 p-1 shrink-0" onerror="this.src='../images/banner/iphone-18-cat-082925094.png'">
            <div class="min-w-0 flex-1">
              <div class="font-bold text-xs sm:text-sm text-slate-900 truncate leading-snug" title="${p.tenHang}">
                ${p.tenHang}
              </div>
              <div class="flex items-center gap-2 mt-0.5 flex-wrap">
                <span class="text-[#E11B22] font-black text-xs sm:text-sm font-mono">${this.formatMoney(p.donGia)}</span>
                ${p.giaGoc && p.giaGoc > p.donGia ? `<span class="text-slate-400 line-through text-[11px] font-mono">${this.formatMoney(p.giaGoc)}</span>` : ''}
              </div>
            </div>
          </div>
          <button type="button" onclick="window.ProductCompare.selectProductFromSearch('${p.maHang}')" class="px-3.5 py-1.5 rounded-xl border border-[#E11B22] text-[#E11B22] hover:bg-[#E11B22] hover:text-white font-bold text-xs transition-colors shrink-0 cursor-pointer shadow-2xs">
            Chọn
          </button>
        </div>
      `).join('');
    }

    selectProductFromSearch(maHang) {
      this.add(maHang);
      this.closeSearchModal();
      this.renderDock();
    }

    openCompare() {
      if (this.items.length < 2) {
        this.showToast('Vui lòng chọn thêm ít nhất 1 sản phẩm để so sánh!', 'warning');
        this.openSearchModal(1);
        return;
      }
      this.openModal();
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
                  <button type="button" onclick="window.ProductCompare.remove('${p.maHang}')" class="absolute top-2 right-2 w-6 h-6 rounded-full bg-slate-100 hover:bg-rose-500 hover:text-white text-slate-400 flex items-center justify-center text-xs transition-colors cursor-pointer" title="Bỏ so sánh">
                    <i class="fa-solid fa-xmark"></i>
                  </button>
                  <img src="${p.anhChinh}" alt="${p.tenHang}" class="w-24 h-24 object-contain mb-2 group-hover:scale-105 transition-transform" onerror="this.src='../images/banner/iphone-18-cat-082925094.png'">
                  <h4 class="font-extrabold text-xs sm:text-sm text-slate-900 line-clamp-2 min-h-[34px] mb-1.5" title="${p.tenHang}">${p.tenHang}</h4>
                  <div class="text-[#E11B22] font-black font-mono text-sm sm:text-base mb-2">${this.formatMoney(p.donGia)}</div>
                  <div class="flex items-center gap-1.5 w-full mt-auto">
                    <button type="button" onclick="window.quickAddToCart && window.quickAddToCart('${p.maHang}', event)" class="flex-1 py-2 px-2.5 rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-bold text-xs shadow-md transition-all flex items-center justify-center gap-1.5 cursor-pointer">
                      <i class="fa-solid fa-cart-plus text-[11px]"></i>
                      <span>Mua ngay</span>
                    </button>
                    <button type="button" onclick="window.openTradeInModal ? window.openTradeInModal('${p.maHang}', event) : (window.location.href='trade-in.html?id=${p.maHang}')" class="py-2 px-2.5 rounded-xl bg-amber-50 hover:bg-amber-100 text-amber-700 border border-amber-200 font-bold text-xs transition-colors cursor-pointer" title="Thu cũ lên đời máy này">
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

    syncCardButtons() {
      const btns = document.querySelectorAll('.btn-compare-product');
      btns.forEach(btn => {
        const ma = btn.dataset.maHang;
        if (!ma) return;
        const active = this.isInCompare(ma);
        if (active) {
          btn.classList.add('bg-blue-600', 'text-white', 'border-blue-600');
          btn.classList.remove('bg-white/95', 'text-slate-700', 'border-slate-200');
          btn.innerHTML = '<i class="fa-solid fa-check text-[10px]"></i><span>Đã so sánh</span>';
        } else {
          btn.classList.remove('bg-blue-600', 'text-white', 'border-blue-600');
          btn.classList.add('bg-white/95', 'text-slate-700', 'border-slate-200');
          btn.innerHTML = '<i class="fa-solid fa-code-compare text-[10px]"></i><span>So sánh</span>';
        }
      });
    }

    formatMoney(val) {
      if (!val || isNaN(val)) return '0₫';
      return Number(val).toLocaleString('vi-VN') + '₫';
    }

    showToast(msg, type = 'info') {
      const toast = document.createElement('div');
      const bg = type === 'success' ? 'bg-emerald-600' : (type === 'warning' ? 'bg-amber-600' : (type === 'error' ? 'bg-rose-600' : 'bg-slate-800'));
      toast.className = `fixed top-20 right-4 z-[10001] px-4 py-2.5 rounded-2xl text-white font-bold text-xs shadow-2xl flex items-center gap-2 ${bg} animate-in fade-in slide-in-from-top-4 duration-200 font-sans`;
      toast.innerHTML = `<i class="fa-solid ${type === 'success' ? 'fa-circle-check' : (type === 'warning' ? 'fa-triangle-exclamation' : 'fa-bell')}"></i><span>${msg}</span>`;
      document.body.appendChild(toast);
      setTimeout(() => {
        toast.classList.add('opacity-0', 'transition-opacity');
        setTimeout(() => {
          if (typeof toast.remove === 'function') {
            toast.remove();
          } else if (toast.parentNode) {
            toast.parentNode.removeChild(toast);
          }
        }, 300);
      }, 2500);
    }
  }

  const manager = new CompareManager();
  window.ProductCompare = manager;
  window.toggleCompareProduct = (id, e) => manager.toggleCompare(id, e);
  return manager;
}));
