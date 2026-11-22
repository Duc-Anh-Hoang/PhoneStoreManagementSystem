const fs = require('fs');
const path = require('path');
const assert = require('assert');

const TradeInCore = require('../frontend/js/trade-in-core.js');

console.log('=== STARTING TRADE-IN ACCEPTANCE VERIFICATION ===\n');

// 1. Verify HTML File & Required IDs
const htmlPath = path.resolve(__dirname, '../frontend/pages/trade-in.html');
assert.ok(fs.existsSync(htmlPath), 'trade-in.html must exist');
const htmlContent = fs.readFileSync(htmlPath, 'utf8');

const REQUIRED_ELEMENT_IDS = [
  'target-name',
  'target-price',
  'target-img',
  'brand-buttons-container',
  'used-search-input',
  'used-models-grid',
  'capacity-options-container',
  'condition-tiers-container',
  'condition-defects-wrapper',
  'defect-checkboxes-container',
  'calc-target-price',
  'calc-used-value',
  'calc-subsidy',
  'calc-total-benefit',
  'calc-final-diff',
  'calc-installment',
  'booking-name',
  'booking-phone',
  'booking-branch',
  'btn-submit-trade-in',
  'condition-guide-modal',
  'booking-success-modal',
  'mobile-tradein-bar'
];

for (const id of REQUIRED_ELEMENT_IDS) {
  assert.ok(htmlContent.includes(`id="${id}"`), `HTML must contain element with id="${id}"`);
}
console.log('✓ AC Test 1: All required UI elements and modal IDs are present in trade-in.html');

// 2. Verify Label Updates (P1.2)
assert.ok(htmlContent.includes('3. Dung lượng lưu trữ:'), 'Label must be "3. Dung lượng lưu trữ:"');
assert.ok(htmlContent.includes('Hướng dẫn phân loại tình trạng máy'), 'Must contain "Hướng dẫn phân loại tình trạng máy"');
assert.ok(!htmlContent.includes('3. Dung lượng máy cũ:'), 'Old label "3. Dung lượng máy cũ:" must not exist');
console.log('✓ AC Test 2: UI labels correctly updated according to specifications (P1.2)');

// 3. Verify Target Product switching alters Right Column Price correctly
{
  const iPhone18Price = 41490000;
  const s26UltraPrice = 30890000;
  const usedModel = { model: 'iPhone 15 Pro Max', basePrice: 20500000, capacities: ['256GB', '512GB', '1TB'] };

  const quote1 = TradeInCore.calculateQuote({
    targetPrice: iPhone18Price,
    usedModel,
    selectedCapacity: '256GB',
    conditionId: 1
  });
  const quote2 = TradeInCore.calculateQuote({
    targetPrice: s26UltraPrice,
    usedModel,
    selectedCapacity: '256GB',
    conditionId: 1
  });

  assert.strictEqual(quote1.targetPrice, iPhone18Price);
  assert.strictEqual(quote2.targetPrice, s26UltraPrice);
  assert.strictEqual(quote1.compensationDiff, 41490000 - 23000000);
  assert.strictEqual(quote2.compensationDiff, 30890000 - 23000000);
  console.log('✓ AC Test 3: Switching target product correctly updates targetPrice and final compensation');
}

// 4. Verify Changing Storage capacity updates quote accurately
{
  const usedModel = { model: 'iPhone 15 Pro', basePrice: 17200000, capacities: ['128GB', '256GB', '512GB'] };

  const q128 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '128GB',
    conditionId: 1
  });
  const q256 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '256GB',
    conditionId: 1
  });
  const q512 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '512GB',
    conditionId: 1
  });

  assert.strictEqual(q128.usedValuation, 17200000);
  assert.strictEqual(q256.usedValuation, Math.round((17200000 * 1.08) / 10000) * 10000);
  assert.strictEqual(q512.usedValuation, Math.round((17200000 * 1.18) / 10000) * 10000);
  assert.ok(q512.usedValuation > q256.usedValuation);
  assert.ok(q256.usedValuation > q128.usedValuation);
  console.log('✓ AC Test 4: Capacity storage variants calculate progressive real valuations');
}

// 5. Switching Grade 1 -> 2 -> 3 -> 4 -> 1 does not retain errors or stale defect deductions
{
  const usedModel = { model: 'iPhone 15', basePrice: 13500000, capacities: ['128GB', '256GB'] };

  // Grade 1
  const qG1 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '128GB',
    conditionId: 1
  });
  assert.strictEqual(qG1.needsAssessment, false);
  assert.strictEqual(qG1.usedValuation, 13500000);

  // Grade 4 with defects
  const qG4 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '128GB',
    conditionId: 4,
    selectedDefects: ['screen_damaged', 'battery_fault']
  });
  assert.strictEqual(qG4.needsAssessment, true);
  assert.strictEqual(qG4.usedValuation, null);
  assert.strictEqual(qG4.compensationDiff, null);

  // Switching back to Grade 1 (defects cleared)
  const qBackG1 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '128GB',
    conditionId: 1,
    selectedDefects: []
  });
  assert.strictEqual(qBackG1.needsAssessment, false);
  assert.strictEqual(qBackG1.usedValuation, 13500000);
  assert.strictEqual(qBackG1.compensationDiff, qG1.compensationDiff);
  console.log('✓ AC Test 5: Condition switching resets defect state and recalculates cleanly');
}

// 6. Grade 4 requires assessment when defects exist - no fabricated price deduction
{
  const usedModel = { model: 'Galaxy S23 Ultra', basePrice: 14500000, capacities: ['256GB', '512GB'] };
  const quote = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel,
    selectedCapacity: '256GB',
    conditionId: 4,
    selectedDefects: ['screen_damaged']
  });

  assert.strictEqual(quote.needsAssessment, true);
  assert.strictEqual(quote.usedValuation, null, 'Must be null to avoid fabricating price');
  assert.strictEqual(quote.subsidyBonus, null);
  assert.strictEqual(quote.compensationDiff, null);
  assert.ok(quote.assessmentNote.includes('thẩm định'));
  console.log('✓ AC Test 6: Grade 4 with defects flags assessment without fabricating deductions');
}

// 7. Non-negative compensation check
{
  const usedModel = { model: 'iPhone 17 Pro Max', basePrice: 28500000, capacities: ['256GB'] };
  // Target product is cheaper than used phone trade-in value
  const quote = TradeInCore.calculateQuote({
    targetPrice: 20000000,
    usedModel,
    selectedCapacity: '256GB',
    conditionId: 1
  });
  assert.strictEqual(quote.compensationDiff, 0, 'Compensation diff must never be negative');
  console.log('✓ AC Test 7: Compensation diff capped at zero (no negative payment requirement)');
}

// 8. Form Validation Suite
{
  // Missing all
  let res = TradeInCore.validateBookingForm({});
  assert.strictEqual(res.isValid, false);
  assert.ok(res.errors.name);
  assert.ok(res.errors.phone);
  assert.ok(res.errors.branch);

  // Short name
  res = TradeInCore.validateBookingForm({ name: 'A', phone: '0901234567', branch: 'Chi nhánh 1' });
  assert.strictEqual(res.isValid, false);
  assert.ok(res.errors.name);

  // Invalid phone numbers
  const invalidPhones = ['12345', 'abcdefghij', '012345678', '0201234567', '091234567890'];
  for (const p of invalidPhones) {
    res = TradeInCore.validateBookingForm({ name: 'Lê Văn B', phone: p, branch: 'Chi nhánh 1' });
    assert.strictEqual(res.isValid, false, `Phone "${p}" should be invalid`);
    assert.ok(res.errors.phone);
  }

  // Valid Vietnamese phones
  const validPhones = ['0909123456', '0388123456', '0777123456', '0868123456', '+84909123456'];
  for (const p of validPhones) {
    res = TradeInCore.validateBookingForm({ name: 'Lê Văn B', phone: p, branch: 'Chi nhánh 1' });
    assert.strictEqual(res.isValid, true, `Phone "${p}" should be valid`);
  }
  console.log('✓ AC Test 8: Form validation strictly enforces full name, VN phone, and branch selection');
}

// 9. Verify Accessibility & Keyboard attributes in HTML
{
  assert.ok(htmlContent.includes('role="radiogroup"'), 'Must have radiogroup roles for accessibility');
  assert.ok(htmlContent.includes('aria-label="Chọn dung lượng máy cũ"'), 'Capacity radiogroup aria-label present');
  assert.ok(htmlContent.includes('role="dialog"'), 'Modals must have role="dialog"');
  assert.ok(htmlContent.includes('aria-modal="true"'), 'Modals must have aria-modal="true"');
  assert.ok(htmlContent.includes('focus-visible:ring-[#0066FF]'), 'Keyboard focus rings present');
  console.log('✓ AC Test 9: Accessibility attributes (role, aria-modal, focus-visible) fully implemented');
}

console.log('\n=== ALL 9 ACCEPTANCE TEST SUITES PASSED FLAWLESSLY! ===');
