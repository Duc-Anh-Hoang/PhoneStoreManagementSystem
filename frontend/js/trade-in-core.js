/**
 * SamaMobile - TRADE-IN CORE ENGINE
 * Pure calculation logic, condition tier definitions, defect matrices, and validation rules.
 * Compatible with Node.js (tests) and Browser (UMD).
 */
(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    module.exports = factory();
  } else {
    root.TradeInCore = factory();
  }
}(typeof self !== 'undefined' ? self : this, function () {
  'use strict';

  // Định nghĩa 4 phân loại tình trạng chuẩn
  const CONDITION_TIERS = [
    {
      id: 1,
      name: 'Loại 1 — Như mới',
      gradeCode: 'GRADE_1',
      subtitle: 'Ngoại hình đẹp 99%',
      shortDesc: 'Ngoại hình đẹp, không trầy xước, không lỗi chức năng, pin tốt (> 85%).',
      rate: 1.0,
      requiresAssessment: false,
      badgeColor: 'emerald'
    },
    {
      id: 2,
      name: 'Loại 2 — Xước nhẹ',
      gradeCode: 'GRADE_2',
      subtitle: 'Dùng lướt 95%',
      shortDesc: 'Vết xước dăm li ti khó thấy, màn hình đẹp, mọi chức năng hoạt động hoàn hảo, pin > 80%.',
      rate: 0.88,
      requiresAssessment: false,
      badgeColor: 'blue'
    },
    {
      id: 3,
      name: 'Loại 3 — Cấn / Trầy nhiều',
      gradeCode: 'GRADE_3',
      subtitle: 'Ngoại hình 85%',
      shortDesc: 'Thân vỏ cấn móp hoặc trầy xước rõ, màn hình có vết xước nhẹ, phần cứng hoạt động bình thường.',
      rate: 0.72,
      requiresAssessment: false,
      badgeColor: 'amber'
    },
    {
      id: 4,
      name: 'Loại 4 — Máy lỗi / Thẩm định',
      gradeCode: 'GRADE_4',
      subtitle: 'Hỏng hóc / Lỗi phần cứng',
      shortDesc: 'Vỡ kính, nứt màn hình, chai pin nặng, camera lỗi hoặc hỏng chức năng. Thẩm định tại cửa hàng.',
      rate: 0.52,
      requiresAssessment: true,
      badgeColor: 'rose'
    }
  ];

  // Danh mục câu hỏi lỗi chi tiết dành cho Loại 4
  const DEFECT_ITEMS = [
    {
      id: 'screen_damaged',
      label: 'Màn hình vỡ kính / sọc / chảy mực / đốm sáng / lỗi cảm ứng',
      severity: 'high'
    },
    {
      id: 'battery_fault',
      label: 'Pin chai nặng (< 70%) / phồng rộp / báo bảo trì',
      severity: 'medium'
    },
    {
      id: 'camera_fault',
      label: 'Camera đục mờ / rung lag / hỏng zoom / nứt kính camera',
      severity: 'medium'
    },
    {
      id: 'power_board_fault',
      label: 'Máy sập nguồn đột ngột / nóng máy bất thường / chập chờn cổng sạc',
      severity: 'high'
    },
    {
      id: 'biometric_sound_fault',
      label: 'Lỗi Face ID / Vân tay / Loa ngoài rè / Hỏng micro thoại',
      severity: 'medium'
    },
    {
      id: 'body_damaged',
      label: 'Vỏ cong vênh nặng / nứt bể mặt lưng kính',
      severity: 'medium'
    },
    {
      id: 'account_locked',
      label: 'Dính tài khoản ẩn (iCloud / Google / Mi Cloud / Knox)',
      severity: 'critical'
    }
  ];

  /**
   * Tính hệ số dung lượng tương đối dựa vào danh sách dung lượng của model.
   * Dung lượng cơ bản (index 0) = 1.0, các mức kế tiếp tăng tương ứng.
   */
  function getCapacityMultiplier(capacities, selectedCapacity) {
    if (!Array.isArray(capacities) || capacities.length <= 1 || !selectedCapacity) {
      return 1.0;
    }
    const idx = capacities.findIndex(c => String(c).toUpperCase() === String(selectedCapacity).toUpperCase());
    if (idx <= 0) return 1.0;

    // Bảng nấc tăng trưởng dung lượng chuẩn
    // Cứ mỗi nấc bộ nhớ cao hơn tăng xấp xỉ 8% - 10%
    const stepIncrements = [1.0, 1.08, 1.18, 1.28, 1.38];
    return stepIncrements[Math.min(idx, stepIncrements.length - 1)];
  }

  /**
   * Tính tiền trợ giá SamaMobile dựa trên giá trị máy thu cũ
   */
  function getTradeInSubsidy(usedValuation) {
    if (typeof usedValuation !== 'number' || usedValuation <= 0 || isNaN(usedValuation)) {
      return 0;
    }
    if (usedValuation >= 15000000) return 2500000;
    if (usedValuation >= 10000000) return 1800000;
    if (usedValuation >= 6000000) return 1200000;
    return 1000000;
  }

  /**
   * Pure calculation: Định giá máy cũ và số tiền bù lên đời
   * @param {Object} params
   * @param {number} params.targetPrice - Giá bán của máy mới
   * @param {Object} params.usedModel - Model máy cũ { model, basePrice, capacities }
   * @param {string} params.selectedCapacity - Dung lượng máy cũ
   * @param {number} params.conditionId - 1, 2, 3 hoặc 4
   * @param {Array<string>} [params.selectedDefects] - Danh sách mã lỗi checked
   * @returns {Object} Quote calculation result
   */
  function calculateQuote(params) {
    const {
      targetPrice = 0,
      usedModel = null,
      selectedCapacity = '',
      conditionId = 1,
      selectedDefects = []
    } = params || {};

    if (!usedModel || typeof usedModel.basePrice !== 'number' || usedModel.basePrice <= 0) {
      return {
        isValid: false,
        error: 'Chưa chọn mẫu máy cũ hợp lệ',
        targetPrice: Number(targetPrice) || 0,
        usedValuation: 0,
        maxReferencePrice: 0,
        subsidyBonus: 0,
        totalBenefit: 0,
        compensationDiff: Number(targetPrice) || 0,
        monthlyInstallment: 0,
        needsAssessment: false,
        condition: CONDITION_TIERS[0],
        selectedDefects: []
      };
    }

    const condition = CONDITION_TIERS.find(t => t.id === Number(conditionId)) || CONDITION_TIERS[0];
    const capacityFactor = getCapacityMultiplier(usedModel.capacities, selectedCapacity);

    // Giá tham khảo tối đa (Loại 1 theo dung lượng)
    const maxReferencePrice = Math.round((usedModel.basePrice * capacityFactor) / 10000) * 10000;

    // Kiểm tra trường hợp Loại 4 có lỗi cụ thể
    const hasDefects = Array.isArray(selectedDefects) && selectedDefects.length > 0;
    const isCriticalLock = hasDefects && selectedDefects.includes('account_locked');

    if (condition.id === 4 && (hasDefects || isCriticalLock)) {
      // Máy loại 4 có lỗi cụ thể: Không tự ý sinh mức trừ tùy tiện, chuyển sang trạng thái cần thẩm định
      return {
        isValid: true,
        targetPrice: Number(targetPrice) || 0,
        maxReferencePrice: maxReferencePrice,
        usedValuation: null, // Cần thẩm định
        subsidyBonus: null, // Sẽ tính sau khi thẩm định máy
        totalBenefit: null,
        compensationDiff: null, // Sẽ xác định sau khi thẩm định
        monthlyInstallment: null,
        needsAssessment: true,
        condition: condition,
        selectedDefects: selectedDefects,
        isCriticalLock: isCriticalLock,
        assessmentNote: isCriticalLock
          ? 'Máy dính tài khoản ẩn/khóa máy. SamaMobile chỉ hỗ trợ thu mua linh kiện sau khi kiểm tra và thẩm định pháp lý tại cửa hàng.'
          : 'Máy có lỗi kỹ thuật/ngoại hình. Kỹ thuật viên SamaMobile sẽ kiểm tra và thẩm định trực tiếp linh kiện còn hoạt động để đưa ra mức giá tốt nhất.'
      };
    }

    // Tính giá thu ước tính
    let rawValuation = usedModel.basePrice * capacityFactor * condition.rate;
    // Làm tròn đến 10.000₫
    const usedValuation = Math.round(rawValuation / 10000) * 10000;

    // Trợ giá lên đời SamaMobile
    const subsidyBonus = getTradeInSubsidy(usedValuation);

    // Tổng quyền lợi thu cũ đổi mới
    const totalBenefit = usedValuation + subsidyBonus;

    // Số tiền cần bù = max(0, giá máy mới - tổng quyền lợi)
    const safeTargetPrice = Math.max(0, Number(targetPrice) || 0);
    const compensationDiff = Math.max(0, safeTargetPrice - totalBenefit);

    // Trả góp 0% dự kiến trong 6 tháng
    const monthlyInstallment = Math.round(compensationDiff / 6);

    return {
      isValid: true,
      targetPrice: safeTargetPrice,
      maxReferencePrice: maxReferencePrice,
      usedValuation: usedValuation,
      subsidyBonus: subsidyBonus,
      totalBenefit: totalBenefit,
      compensationDiff: compensationDiff,
      monthlyInstallment: monthlyInstallment,
      needsAssessment: condition.id === 4, // Nếu loại 4 chưa chọn lỗi thì cũng ghi nhận là loại thẩm định
      condition: condition,
      selectedDefects: selectedDefects,
      isCriticalLock: false
    };
  }

  /**
   * Validate thông tin đăng ký giữ suất thu cũ lên đời
   */
  function validateBookingForm(data) {
    const { name, phone, branch } = data || {};
    const errors = {};

    const cleanName = typeof name === 'string' ? name.trim() : '';
    if (!cleanName) {
      errors.name = 'Vui lòng nhập họ và tên của bạn.';
    } else if (cleanName.length < 2) {
      errors.name = 'Họ và tên cần có ít nhất 2 ký tự.';
    }

    const cleanPhone = typeof phone === 'string' ? phone.trim().replace(/\s+/g, '') : '';
    const phoneRegex = /^(0|\+84)(3|5|7|8|9)[0-9]{8}$/;
    if (!cleanPhone) {
      errors.phone = 'Vui lòng nhập số điện thoại để nhận mã giữ giá.';
    } else if (!phoneRegex.test(cleanPhone)) {
      errors.phone = 'Số điện thoại không đúng định dạng (Ví dụ: 0912345678).';
    }

    const cleanBranch = typeof branch === 'string' ? branch.trim() : '';
    if (!cleanBranch) {
      errors.branch = 'Vui lòng chọn chi nhánh tiếp nhận gần bạn nhất.';
    }

    return {
      isValid: Object.keys(errors).length === 0,
      errors: errors,
      cleanedData: {
        name: cleanName,
        phone: cleanPhone,
        branch: cleanBranch
      }
    };
  }

  /**
   * Format tiền VND chuẩn `vi-VN`
   */
  function formatMoney(amount) {
    if (amount === null || amount === undefined || isNaN(amount)) {
      return '0₫';
    }
    return Number(amount).toLocaleString('vi-VN') + '₫';
  }

  return {
    CONDITION_TIERS,
    DEFECT_ITEMS,
    getCapacityMultiplier,
    getTradeInSubsidy,
    calculateQuote,
    validateBookingForm,
    formatMoney
  };
}));
