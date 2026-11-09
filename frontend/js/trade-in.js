/**
 * SamaMobile / CellphoneS - BỘ CÔNG CỤ THU CŨ ĐỔI MỚI (TRADE-IN VALUATION CALCULATOR)
 * Định giá máy cũ, tính tiền trợ giá độc quyền và số tiền bù chính xác để lên đời máy mới.
 */
(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.TradeInCalculator = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  // Cơ sở dữ liệu định giá máy cũ chuẩn thị trường (Giá gốc loại 1)
  const USED_DEVICES_DB = [
    // APPLE
    { brand: 'Apple', model: 'iPhone 15 Pro Max', basePrice: 20500000, capacities: ['256GB', '512GB', '1TB'] },
    { brand: 'Apple', model: 'iPhone 15 Pro', basePrice: 17200000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 15 Plus', basePrice: 15100000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 15', basePrice: 13500000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 14 Pro Max', basePrice: 16800000, capacities: ['128GB', '256GB', '512GB', '1TB'] },
    { brand: 'Apple', model: 'iPhone 14 Pro', basePrice: 14200000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 14 Plus', basePrice: 12000000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 14', basePrice: 11000000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 13 Pro Max', basePrice: 13200000, capacities: ['128GB', '256GB', '512GB', '1TB'] },
    { brand: 'Apple', model: 'iPhone 13 Pro', basePrice: 11500000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 13', basePrice: 9500000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 12 Pro Max', basePrice: 10200000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 12 Pro', basePrice: 8500000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 12', basePrice: 6800000, capacities: ['64GB', '128GB', '256GB'] },
    { brand: 'Apple', model: 'iPhone 11 Pro Max', basePrice: 7500000, capacities: ['64GB', '256GB', '512GB'] },
    { brand: 'Apple', model: 'iPhone 11', basePrice: 5200000, capacities: ['64GB', '128GB', '256GB'] },

    // SAMSUNG
    { brand: 'Samsung', model: 'Galaxy S24 Ultra', basePrice: 18500000, capacities: ['256GB', '512GB', '1TB'] },
    { brand: 'Samsung', model: 'Galaxy S24 Plus', basePrice: 13800000, capacities: ['256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy S24', basePrice: 11200000, capacities: ['256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy S23 Ultra', basePrice: 14500000, capacities: ['256GB', '512GB', '1TB'] },
    { brand: 'Samsung', model: 'Galaxy S23 Plus', basePrice: 10800000, capacities: ['256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy S23', basePrice: 8900000, capacities: ['128GB', '256GB'] },
    { brand: 'Samsung', model: 'Galaxy Z Fold 5', basePrice: 16500000, capacities: ['256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy Z Flip 5', basePrice: 9800000, capacities: ['256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy S22 Ultra', basePrice: 10500000, capacities: ['128GB', '256GB', '512GB'] },
    { brand: 'Samsung', model: 'Galaxy S21 Ultra', basePrice: 7500000, capacities: ['128GB', '256GB'] },

    // XIAOMI
    { brand: 'Xiaomi', model: 'Xiaomi 14 Ultra', basePrice: 16000000, capacities: ['256GB', '512GB'] },
    { brand: 'Xiaomi', model: 'Xiaomi 14', basePrice: 11500000, capacities: ['256GB', '512GB'] },
    { brand: 'Xiaomi', model: 'Xiaomi 13 Pro', basePrice: 9200000, capacities: ['256GB', '512GB'] },
    { brand: 'Xiaomi', model: 'Xiaomi 13', basePrice: 7800000, capacities: ['128GB', '256GB'] }
  ];

  // Hệ số tình trạng máy
  const CONDITION_TIERS = [
    { id: 1, name: 'Loại 1 (Máy đẹp 99%)', desc: 'Màn hình & thân máy đẹp, không trầy xước, pin tốt, mọi chức năng hoạt động hoàn hảo', rate: 1.0 },
    { id: 2, name: 'Loại 2 (Xước nhẹ 95%)', desc: 'Màn hình đẹp, thân máy có vết xước lông mèo nhỏ, pin & chức năng ổn định', rate: 0.88 },
    { id: 3, name: 'Loại 3 (Cấn viền 90%)', desc: 'Màn hình có xước nhẹ, thân máy cấn móp nhẹ ở các góc, không lỗi chức năng', rate: 0.72 },
    { id: 4, name: 'Loại 4 (Màn ám / Lỗi nhẹ)', desc: 'Thân máy trầy xước nhiều, màn hình có vết ố/ám nhẹ, các tính năng cơ bản còn tốt', rate: 0.52 }
  ];

  class TradeInManager {
    constructor() {
      this.selectedTargetProduct = null;
      this.currentBrand = 'Apple';
      this.selectedModel = USED_DEVICES_DB[0];
      this.selectedCapacity = this.selectedModel.capacities[0];
      this.selectedCondition = CONDITION_TIERS[0];
    }

    openModal(targetProductId = null) {
      const isPagesDir = window.location.pathname.includes('/pages/');
      const targetUrl = (isPagesDir ? 'trade-in.html' : 'pages/trade-in.html') + (targetProductId ? `?id=${encodeURIComponent(targetProductId)}` : '');
      window.location.href = targetUrl;
    }

    closeModal() {
      const modal = document.getElementById('trade-in-modal');
      if (modal) {
        modal.classList.add('hidden');
        document.body.style.overflow = '';
      }
    }

    setBrand(brand) {
      this.currentBrand = brand;
      const filtered = USED_DEVICES_DB.filter(x => x.brand === brand);
      this.selectedModel = filtered[0] || USED_DEVICES_DB[0];
      this.selectedCapacity = this.selectedModel.capacities[0];
      this.render();
    }

    setModel(modelName) {
      const found = USED_DEVICES_DB.find(x => x.model === modelName);
      if (found) {
        this.selectedModel = found;
        this.selectedCapacity = found.capacities[0];
        this.render();
      }
    }

    setCapacity(cap) {
      this.selectedCapacity = cap;
      this.renderCalculation();
    }

    setCondition(tierId) {
      const found = CONDITION_TIERS.find(t => t.id === Number(tierId));
      if (found) {
        this.selectedCondition = found;
        this.renderCalculation();
      }
    }

    setTargetProduct(maHang) {
      const data = (window.CellphoneSData || window.StoreData);
      const allProducts = data && Array.isArray(data.products) ? data.products : [];
      const found = allProducts.find(x => String(x.maHang) === String(maHang));
      if (found) {
        this.selectedTargetProduct = found;
        this.renderCalculation();
      }
    }

    calculate() {
      if (!this.selectedModel) return { valuation: 0, subsidy: 0, targetPrice: 0, diff: 0, installment: 0 };

      // Tính giá máy cũ
      let val = this.selectedModel.basePrice;
      if (this.selectedCapacity === '256GB') val *= 1.08;
      else if (this.selectedCapacity === '512GB') val *= 1.18;
      else if (this.selectedCapacity === '1TB') val *= 1.28;

      val = Math.round((val * this.selectedCondition.rate) / 10000) * 10000;

      // Trợ giá lên đời độc quyền SamaMobile
      let subsidy = 1000000; // Mặc định trợ giá 1 triệu
      if (val >= 15000000) subsidy = 2500000; // Trợ giá 2.5 triệu cho máy cao cấp
      else if (val >= 10000000) subsidy = 1800000; // Trợ giá 1.8 triệu
      else if (val >= 6000000) subsidy = 1200000;

      const targetPrice = Number(this.selectedTargetProduct?.donGia) || 30990000;
      const totalTradeInBenefit = val + subsidy;
      const diff = Math.max(0, targetPrice - totalTradeInBenefit);
      const installment = Math.round(diff / 6);

      return {
        usedValuation: val,
        subsidyBonus: subsidy,
        totalBenefit: totalTradeInBenefit,
        targetPrice: targetPrice,
        compensationDiff: diff,
        monthlyInstallment: installment
      };
    }

    render() {
      const container = document.getElementById('trade-in-modal-body');
      if (!container) return;

      const data = (window.CellphoneSData || window.StoreData);
      const allProducts = data && Array.isArray(data.products) ? data.products.slice(0, 30) : [];
      const brandModels = USED_DEVICES_DB.filter(x => x.brand === this.currentBrand);

      container.innerHTML = `
        <div class="grid grid-cols-1 lg:grid-cols-12 gap-5 p-4 sm:p-6 overflow-y-auto max-h-[82vh] custom-scrollbar">
          
          <!-- CỘT TRÁI (7 CỘT): CHỌN THÔNG TIN MÁY CŨ & MÁY MỚI -->
          <div class="lg:col-span-7 space-y-5">
            
            <!-- BƯỚC 1: CHỌN MÁY MUỐN LÊN ĐỜI (MÁY MỚI) -->
            <div class="p-4 rounded-2xl bg-blue-50/60 border border-blue-200">
              <div class="flex items-center justify-between mb-2">
                <span class="text-xs font-black uppercase text-blue-900 tracking-wider flex items-center gap-1.5">
                  <span class="w-5 h-5 rounded-full bg-blue-600 text-white flex items-center justify-center text-[10px]">1</span>
                  Chọn máy mới muốn lên đời:
                </span>
                <span class="text-[11px] font-bold text-blue-600">Máy chính hãng 100%</span>
              </div>
              <div class="relative">
                <select onchange="window.TradeInCalculator.setTargetProduct(this.value)" class="w-full px-3.5 py-2.5 rounded-xl border border-blue-300 bg-white font-bold text-xs sm:text-sm text-slate-900 focus:outline-none focus:ring-2 focus:ring-blue-500 shadow-xs cursor-pointer">
                  ${allProducts.map(p => `
                    <option value="${p.maHang}" ${this.selectedTargetProduct && String(this.selectedTargetProduct.maHang) === String(p.maHang) ? 'selected' : ''}>
                      ${p.tenHang} — ${this.formatMoney(p.donGia)}
                    </option>
                  `).join('')}
                </select>
              </div>
            </div>

            <!-- BƯỚC 2: ĐỊNH GIÁ MÁY CŨ BẠN ĐANG DÙNG -->
            <div class="p-4 rounded-2xl bg-white border border-slate-200 shadow-sm space-y-4">
              <div class="flex items-center justify-between border-b border-slate-100 pb-2">
                <span class="text-xs font-black uppercase text-slate-800 tracking-wider flex items-center gap-1.5">
                  <span class="w-5 h-5 rounded-full bg-emerald-600 text-white flex items-center justify-center text-[10px]">2</span>
                  Máy cũ bạn đang sử dụng:
                </span>
                <span class="text-[11px] font-semibold text-emerald-600">Định giá chuẩn thị trường</span>
              </div>

              <!-- Chọn Hãng -->
              <div>
                <label class="block text-xs font-bold text-slate-700 mb-1.5">Hãng sản xuất:</label>
                <div class="flex items-center gap-2">
                  ${['Apple', 'Samsung', 'Xiaomi'].map(b => `
                    <button type="button" onclick="window.TradeInCalculator.setBrand('${b}')" class="flex-1 py-2 px-3 rounded-xl border text-xs font-bold transition-all cursor-pointer ${this.currentBrand === b ? 'bg-blue-600 text-white border-blue-600 shadow-sm' : 'bg-slate-50 text-slate-700 border-slate-200 hover:bg-slate-100'}">
                      ${b}
                    </button>
                  `).join('')}
                </div>
              </div>

              <!-- Chọn Dòng Máy Cũ -->
              <div>
                <label class="block text-xs font-bold text-slate-700 mb-1.5">Dòng máy cũ:</label>
                <select onchange="window.TradeInCalculator.setModel(this.value)" class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 bg-white font-bold text-xs sm:text-sm text-slate-900 focus:outline-none focus:ring-2 focus:ring-blue-500 shadow-xs cursor-pointer">
                  ${brandModels.map(m => `
                    <option value="${m.model}" ${this.selectedModel && this.selectedModel.model === m.model ? 'selected' : ''}>
                      ${m.model}
                    </option>
                  `).join('')}
                </select>
              </div>

              <!-- Chọn Dung Lượng -->
              <div>
                <label class="block text-xs font-bold text-slate-700 mb-1.5">Phiên bản bộ nhớ (Dung lượng):</label>
                <div class="flex flex-wrap gap-2">
                  ${(this.selectedModel?.capacities || []).map(cap => `
                    <button type="button" onclick="window.TradeInCalculator.setCapacity('${cap}')" class="py-1.5 px-3.5 rounded-xl border text-xs font-bold transition-all cursor-pointer ${this.selectedCapacity === cap ? 'bg-indigo-600 text-white border-indigo-600 shadow-xs' : 'bg-slate-50 text-slate-700 border-slate-200 hover:bg-slate-100'}">
                      ${cap}
                    </button>
                  `).join('')}
                </div>
              </div>

              <!-- Chọn Tình Trạng Máy -->
              <div>
                <label class="block text-xs font-bold text-slate-700 mb-1.5">Tình trạng ngoại hình máy cũ:</label>
                <div class="grid grid-cols-1 sm:grid-cols-2 gap-2">
                  ${CONDITION_TIERS.map(tier => `
                    <label onclick="window.TradeInCalculator.setCondition(${tier.id})" class="p-2.5 rounded-xl border cursor-pointer transition-all flex flex-col justify-between ${this.selectedCondition.id === tier.id ? 'bg-emerald-50/70 border-emerald-500 ring-2 ring-emerald-300' : 'bg-slate-50/60 border-slate-200 hover:border-slate-300'}">
                      <div class="flex items-center justify-between mb-1">
                        <span class="text-xs font-extrabold text-slate-900">${tier.name}</span>
                        <input type="radio" name="trade-in-condition" value="${tier.id}" ${this.selectedCondition.id === tier.id ? 'checked' : ''} class="text-emerald-600">
                      </div>
                      <p class="text-[10px] text-slate-500 leading-snug line-clamp-2">${tier.desc}</p>
                    </label>
                  `).join('')}
                </div>
              </div>

            </div>

          </div>

          <!-- CỘT PHẢI (5 CỘT): BẢNG TÍNH TÀI CHÍNH THÔNG MINH & FORM ĐẶT LỊCH -->
          <div id="trade-in-result-panel" class="lg:col-span-5 flex flex-col justify-between p-4 sm:p-5 rounded-2xl bg-gradient-to-br from-slate-900 via-slate-800 to-indigo-950 text-white shadow-xl">
            <!-- Render động bằng renderCalculation -->
          </div>

        </div>
      `;

      this.renderCalculation();
    }

    renderCalculation() {
      const panel = document.getElementById('trade-in-result-panel');
      if (!panel) return;

      const calc = this.calculate();
      const targetName = this.selectedTargetProduct?.tenHang || 'Sản phẩm mới';

      panel.innerHTML = `
        <div>
          <!-- Tiêu đề thẻ -->
          <div class="flex items-center justify-between border-b border-slate-700/80 pb-3 mb-4">
            <div class="flex items-center gap-2">
              <i class="fa-solid fa-calculator text-amber-400"></i>
              <span class="font-extrabold text-sm sm:text-base text-white">Bảng Tính Trợ Giá Lên Đời</span>
            </div>
            <span class="px-2 py-0.5 rounded-full bg-emerald-500/20 text-emerald-400 border border-emerald-500/40 text-[10px] font-black">Ưu đãi độc quyền</span>
          </div>

          <!-- Chi tiết bảng trừ tiền -->
          <div class="space-y-2.5 text-xs">
            <div class="flex items-center justify-between">
              <span class="text-slate-300">Giá máy mới niêm yết:</span>
              <span class="font-bold font-mono text-sm">${this.formatMoney(calc.targetPrice)}</span>
            </div>

            <div class="flex items-center justify-between text-emerald-400">
              <span class="flex items-center gap-1.5">
                <i class="fa-solid fa-circle-check text-xs"></i>
                <span>Thu lại máy cũ (${this.selectedModel?.model}):</span>
              </span>
              <span class="font-bold font-mono text-sm">- ${this.formatMoney(calc.usedValuation)}</span>
            </div>

            <div class="flex items-center justify-between text-amber-300">
              <span class="flex items-center gap-1.5">
                <i class="fa-solid fa-gift text-xs"></i>
                <span>Trợ giá lên đời SamaCare:</span>
              </span>
              <span class="font-bold font-mono text-sm">- ${this.formatMoney(calc.subsidyBonus)}</span>
            </div>

            <div class="pt-2 border-t border-slate-700/80">
              <div class="text-[11px] text-slate-400 mb-0.5">Tổng số tiền được giảm trừ:</div>
              <div class="text-base font-extrabold text-emerald-400 font-mono">
                Tiết kiệm: ${this.formatMoney(calc.totalBenefit)}
              </div>
            </div>

            <!-- Khung nổi bật: Số tiền bù thực tế -->
            <div class="p-3.5 rounded-xl bg-gradient-to-r from-rose-900/60 to-red-950/60 border border-rose-500/50 mt-3 shadow-inner">
              <div class="text-[11px] font-bold uppercase tracking-wider text-rose-300 mb-1">
                Số tiền bạn chỉ cần thanh toán / bù thêm:
              </div>
              <div class="text-2xl sm:text-3xl font-black text-rose-400 font-mono leading-none mb-1.5">
                ${this.formatMoney(calc.compensationDiff)}
              </div>
              <div class="text-[11px] text-slate-300 flex items-center gap-1 font-semibold">
                <i class="fa-solid fa-credit-card text-blue-400"></i>
                <span>Hoặc trả góp 0% chỉ từ <strong>${this.formatMoney(calc.monthlyInstallment)} / tháng</strong></span>
              </div>
            </div>
          </div>

          <!-- Form đăng ký nhận mã giữ giá -->
          <div class="mt-4 pt-3 border-t border-slate-700/80 space-y-2.5">
            <div class="text-xs font-bold text-slate-200">Đăng ký giữ suất trợ giá trong 7 ngày:</div>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-2">
              <input type="text" id="trade-in-name" placeholder="Họ và tên của bạn" class="px-3 py-2 rounded-xl bg-slate-900/90 border border-slate-700 text-xs text-white placeholder:text-slate-500 focus:outline-none focus:border-blue-400">
              <input type="tel" id="trade-in-phone" placeholder="Số điện thoại nhận SMS" class="px-3 py-2 rounded-xl bg-slate-900/90 border border-slate-700 text-xs text-white placeholder:text-slate-500 focus:outline-none focus:border-blue-400">
            </div>
          </div>
        </div>

        <!-- Nút hành động -->
        <div class="mt-4 pt-3 border-t border-slate-700 flex items-center gap-2">
          <button type="button" onclick="window.TradeInCalculator.submitBooking()" class="flex-1 py-3 rounded-xl bg-gradient-to-r from-amber-500 to-orange-600 hover:from-amber-600 hover:to-orange-700 text-white font-extrabold text-xs sm:text-sm shadow-lg shadow-orange-900/40 active:scale-98 transition-all flex items-center justify-center gap-2 cursor-pointer">
            <i class="fa-solid fa-bolt"></i>
            <span>ĐẶT LỊCH THU CŨ LÊN ĐỜI NGAY</span>
          </button>
        </div>
      `;
    }

    submitBooking() {
      const name = document.getElementById('trade-in-name')?.value?.trim();
      const phone = document.getElementById('trade-in-phone')?.value?.trim();

      if (!phone || phone.length < 9) {
        alert('Vui lòng nhập số điện thoại hợp lệ để hệ thống gửi mã giữ giá và lịch thẩm định máy!');
        return;
      }

      const calc = this.calculate();
      const bookingCode = 'TC-' + Math.floor(100000 + Math.random() * 900000);

      alert(`🎉 ĐẶT LỊCH THU CŨ ĐỔI MỚI THÀNH CÔNG!\n\n` +
            `• Mã phiếu giữ giá: ${bookingCode} (Có hiệu lực 7 ngày)\n` +
            `• Khách hàng: ${name || 'Quý khách'} (${phone})\n` +
            `• Máy thu cũ: ${this.selectedModel?.model} (${this.selectedCapacity})\n` +
            `• Định giá dự kiến: ${this.formatMoney(calc.usedValuation)}\n` +
            `• Trợ giá SamaCare: + ${this.formatMoney(calc.subsidyBonus)}\n` +
            `• Số tiền bù lên đời: ${this.formatMoney(calc.compensationDiff)}\n\n` +
            `Chuyên viên SamaMobile sẽ gọi xác nhận và giữ máy mới cho bạn trong 15 phút!`);

      this.closeModal();
    }

    formatMoney(val) {
      if (!val || isNaN(val)) return '0₫';
      return Number(val).toLocaleString('vi-VN') + '₫';
    }
  }

  const manager = new TradeInManager();
  window.TradeInCalculator = manager;
  window.openTradeInModal = (id, e) => {
    if (e) { e.stopPropagation(); e.preventDefault(); }
    manager.openModal(id);
  };
  return manager;
}));
