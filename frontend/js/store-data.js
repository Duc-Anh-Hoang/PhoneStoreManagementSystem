/**
 * CellphoneS - 241 Lê Văn Việt - DATA LAYER & REPOSITORY
 * Danh mục nghiệp vụ từ REST API .NET và PostgreSQL.
 */

(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.CellphoneSData = root.StoreData = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  async function read(path) {
    const localPreview = ['5500', '5501', '3000'].includes(window.location.port) || window.location.protocol === 'file:';
    const apiBase = window.API_BASE || (localPreview ? 'http://localhost:5132/api' : '/api');
    const response = await fetch(apiBase + path);
    if (!response.ok) throw new Error('Không tải được ' + path);
    return response.json();
  }

  async function readCatalog() {
    const products = [];
    const pageSize = 100;
    for (let page = 1; ; page++) {
      const rows = await read('/mathang?pageSize=' + pageSize + '&page=' + page);
      products.push(...rows);
      if (rows.length < pageSize) return products;
    }
  }

  class StoreDataManager {
    constructor() {
      this.products = [];
      this.nhomHang = [];
      this.loaiHang = [];
      this.promotions = [];
      this.isInitialized = false;
      this.loadError = null;
    }

    async init() {
      if (this.isInitialized) return;

      try {
        const [products, groups, types, promotions, media] = await Promise.all([
          readCatalog(),
          read('/nhomhang'),
          read('/loaihang'),
          read('/promotions/active'),
          fetch('/data/products-meta.json').then(r => r.ok ? r.json() : {}).catch(() => ({}))
        ]);
        this.nhomHang = groups;
        this.loaiHang = types;
        this.promotions = promotions;
        this.products = products.map(item => this.decorateProduct(item, media[item.maHang]));
        this.media = media;
        this.isInitialized = true;
        this.loadError = null;
      } catch (err) {
        this.isInitialized = false;
        this.loadError = 'Không tải được sản phẩm, vui lòng thử lại';
        console.error('StoreDataManager init error:', err);
        const shelf = document.getElementById('product-shelves');
        if (shelf) {
          shelf.innerHTML = `<div class="p-8 text-center text-rose-600 font-bold bg-rose-50 rounded-2xl border border-rose-200 my-6">
            <i class="fa-solid fa-circle-exclamation text-2xl mb-2 block"></i>
            Không tải được sản phẩm, vui lòng thử lại.
          </div>`;
        }
      }
    }

    decorateProduct(item, media = {}) {
      const image = media.anhChinh || item.hinhAnh || '';
      return {
        ...item,
        anhChinh: image,
        gallery: media.gallery,
        album: media.album,
        slug: media.slug,
        nhanHieu: item.tenLoai || '',
        giaGoc: item.donGia,
        phienBan: [{ id: 'std', dungLuong: 'Tiêu chuẩn', gia: item.donGia, giaGoc: item.donGia, tonKho: item.tonKho }],
        thongSo: Object.fromEntries((item.thongSo || []).map(spec => [spec.tenThuocTinh, spec.giaTri]))
      };
    }

    async getProductDetail(code) {
      const item = await read('/mathang/' + encodeURIComponent(code));
      let media = this.media;
      if (!media) {
        media = await fetch('/data/products-meta.json').then(r => r.ok ? r.json() : {}).catch(() => ({}));
      }
      this.media = media;
      return this.decorateProduct(item, media[item.maHang]);
    }

    async getProductsByGroup(group, pageSize = 6) {
      if (!group) return [];
      const rows = await read('/mathang?pageSize=' + pageSize + '&maNhom=' + encodeURIComponent(group));
      return rows.map(item => this.decorateProduct(item, this.media?.[item.maHang]));
    }

    getAllProducts() {
      return this.products;
    }

    getProductById(maHang) {
      return this.products.find(p => p.maHang === maHang) || null;
    }

    getFeaturedPhones(brand = 'all') {
      return this.products.filter(p => {
        const isPhone = p.maNhom === 'NH01' || p.maNhom === 'DIENTHOAI';
        if (!isPhone) return false;
        if (!brand || brand === 'all') return true;
        return (p.nhanHieu || '').toLowerCase().includes(brand.toLowerCase());
      });
    }

    getNewArrivals() {
      return this.products.slice(0, 8);
    }

    getBestSellers() {
      return this.products.slice(8, 16);
    }

    getFlashSaleProducts() {
      return [];
    }

    getRecentlyViewed() {
      return [];
    }

    getAccessories() {
      return this.products.filter(p => p.maNhom === 'NH02' || p.maNhom === 'NH03' || p.maNhom === 'PHUKIEN' || p.maNhom === 'AMTHANH');
    }

    getProductsByBrand(brand = 'all') {
      if (!brand || brand === 'all') return this.products;
      return this.products.filter(p => (p.nhanHieu || '').toLowerCase().includes(brand.toLowerCase()));
    }

    search(query = '', filters = {}) {
      const q = (query || '').trim().toLowerCase();
      let list = [...this.products];

      if (q) {
        list = list.filter(p => {
          const matchName = (p.tenHang || '').toLowerCase().includes(q);
          const matchCode = (p.maHang || '').toLowerCase().includes(q);
          const matchBrand = (p.nhanHieu || '').toLowerCase().includes(q);
          const matchGroup = (p.tenNhom || '').toLowerCase().includes(q);
          const matchType = (p.tenLoai || '').toLowerCase().includes(q);
          return matchName || matchCode || matchBrand || matchGroup || matchType;
        });
      }

      if (filters.brand && filters.brand !== 'all') {
        list = list.filter(p => (p.nhanHieu || '').toLowerCase().includes(filters.brand.toLowerCase()));
      }

      if (filters.category && filters.category !== 'all') {
        list = list.filter(p => p.maNhom === filters.category);
      }

      if (filters.priceRange && filters.priceRange !== 'all') {
        switch (filters.priceRange) {
          case 'under_5m':
            list = list.filter(p => p.donGia < 5000000);
            break;
          case '5m_15m':
            list = list.filter(p => p.donGia >= 5000000 && p.donGia <= 15000000);
            break;
          case '15m_25m':
            list = list.filter(p => p.donGia > 15000000 && p.donGia <= 25000000);
            break;
          case 'over_25m':
            list = list.filter(p => p.donGia > 25000000);
            break;
        }
      }

      if (filters.inStockOnly) {
        list = list.filter(p => p.tonKho > 0);
      }

      if (filters.sortBy) {
        switch (filters.sortBy) {
          case 'price_asc':
            list.sort((a, b) => a.donGia - b.donGia);
            break;
          case 'price_desc':
            list.sort((a, b) => b.donGia - a.donGia);
            break;
        }
      }

      return list;
    }

    verifyPromotion(maKM) {
      if (!maKM) return null;
      const code = maKM.trim().toUpperCase();
      const now = new Date();

      const promo = this.promotions.find(p => (p.maKM || '').toUpperCase() === code);
      if (!promo) return null;

      if (promo.tamDung) return null;
      if (promo.ngayBD && new Date(promo.ngayBD) > now) return null;
      if (promo.ngayKT && new Date(promo.ngayKT) < now) return null;

      return {
        maKM: promo.maKM,
        tenKM: promo.tenKM,
        phanTramGiam: Number(promo.phanTramGiam) || 0,
        moTa: promo.moTa || `Giảm ${promo.phanTramGiam}% trên tổng giá trị hóa đơn`
      };
    }

    formatVND(amount) {
      return new Intl.NumberFormat('vi-VN').format(Math.round(amount || 0)) + ' đ';
    }
  }

  return new StoreDataManager();
}));
