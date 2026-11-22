const assert = require('assert');
const path = require('path');

const TradeInCore = require('../frontend/js/trade-in-core.js');

console.log('--- STARTING TRADE-IN CORE TESTS ---');

// Test 1: Capacity Multiplier
{
  const capacities = ['128GB', '256GB', '512GB', '1TB'];
  assert.strictEqual(TradeInCore.getCapacityMultiplier(capacities, '128GB'), 1.0, 'Base capacity should be 1.0');
  assert.strictEqual(TradeInCore.getCapacityMultiplier(capacities, '256GB'), 1.08, 'Tier 1 should be 1.08');
  assert.strictEqual(TradeInCore.getCapacityMultiplier(capacities, '512GB'), 1.18, 'Tier 2 should be 1.18');
  assert.strictEqual(TradeInCore.getCapacityMultiplier(capacities, '1TB'), 1.28, 'Tier 3 should be 1.28');
  assert.strictEqual(TradeInCore.getCapacityMultiplier(['256GB', '512GB'], '256GB'), 1.0, 'Base 256GB should be 1.0');
  console.log('✓ Test 1: Capacity Multiplier PASS');
}

// Test 2: Subsidy Tiers
{
  assert.strictEqual(TradeInCore.getTradeInSubsidy(20000000), 2500000, '>= 15M should get 2.5M subsidy');
  assert.strictEqual(TradeInCore.getTradeInSubsidy(15000000), 2500000, '== 15M should get 2.5M subsidy');
  assert.strictEqual(TradeInCore.getTradeInSubsidy(12000000), 1800000, '>= 10M should get 1.8M subsidy');
  assert.strictEqual(TradeInCore.getTradeInSubsidy(7000000), 1200000, '>= 6M should get 1.2M subsidy');
  assert.strictEqual(TradeInCore.getTradeInSubsidy(3000000), 1000000, '< 6M should get 1.0M subsidy');
  assert.strictEqual(TradeInCore.getTradeInSubsidy(0), 0, '0 should get 0 subsidy');
  console.log('✓ Test 2: Subsidy Tiers PASS');
}

// Test 3: Calculate Quote - Grade 1
{
  const model = {
    model: 'iPhone 15 Pro Max',
    basePrice: 20500000,
    capacities: ['256GB', '512GB', '1TB']
  };
  const quote = TradeInCore.calculateQuote({
    targetPrice: 41490000,
    usedModel: model,
    selectedCapacity: '256GB',
    conditionId: 1
  });

  assert.strictEqual(quote.isValid, true);
  assert.strictEqual(quote.usedValuation, 20500000);
  assert.strictEqual(quote.subsidyBonus, 2500000);
  assert.strictEqual(quote.totalBenefit, 23000000);
  assert.strictEqual(quote.compensationDiff, 41490000 - 23000000);
  assert.strictEqual(quote.monthlyInstallment, Math.round((41490000 - 23000000) / 6));
  assert.strictEqual(quote.needsAssessment, false);
  console.log('✓ Test 3: Calculate Quote Grade 1 PASS');
}

// Test 4: Calculate Quote - Grade 2 & 3
{
  const model = {
    model: 'iPhone 15 Pro',
    basePrice: 17200000,
    capacities: ['128GB', '256GB', '512GB']
  };
  const quote2 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel: model,
    selectedCapacity: '128GB',
    conditionId: 2
  });
  // 17200000 * 0.88 = 15136000 -> rounded to 15140000
  assert.strictEqual(quote2.usedValuation, 15140000);
  assert.strictEqual(quote2.subsidyBonus, 2500000);

  const quote3 = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel: model,
    selectedCapacity: '128GB',
    conditionId: 3
  });
  // 17200000 * 0.72 = 12384000 -> rounded to 12380000
  assert.strictEqual(quote3.usedValuation, 12380000);
  assert.strictEqual(quote3.subsidyBonus, 1800000);
  console.log('✓ Test 4: Calculate Quote Grade 2 & 3 PASS');
}

// Test 5: Calculate Quote - Grade 4 with Defects (Requires Assessment, no fabricated values)
{
  const model = {
    model: 'iPhone 14',
    basePrice: 11000000,
    capacities: ['128GB', '256GB']
  };
  const quoteDefect = TradeInCore.calculateQuote({
    targetPrice: 30000000,
    usedModel: model,
    selectedCapacity: '128GB',
    conditionId: 4,
    selectedDefects: ['screen_damaged', 'battery_fault']
  });

  assert.strictEqual(quoteDefect.needsAssessment, true);
  assert.strictEqual(quoteDefect.usedValuation, null, 'Valuation must be null for defect grade requiring assessment');
  assert.strictEqual(quoteDefect.compensationDiff, null, 'Diff must be null pending inspection');
  assert.ok(quoteDefect.assessmentNote.length > 0);
  console.log('✓ Test 5: Calculate Quote Grade 4 with Defects PASS');
}

// Test 6: Zero or negative floor protection
{
  const model = {
    model: 'iPhone 15 Pro Max',
    basePrice: 20500000,
    capacities: ['256GB']
  };
  const quoteExcess = TradeInCore.calculateQuote({
    targetPrice: 15000000, // Cheaper than used phone benefit
    usedModel: model,
    selectedCapacity: '256GB',
    conditionId: 1
  });
  assert.strictEqual(quoteExcess.compensationDiff, 0, 'Diff must not be negative');
  console.log('✓ Test 6: Zero/Negative compensation protection PASS');
}

// Test 7: Form Validation
{
  // Invalid empty
  const v1 = TradeInCore.validateBookingForm({});
  assert.strictEqual(v1.isValid, false);
  assert.ok(v1.errors.name);
  assert.ok(v1.errors.phone);
  assert.ok(v1.errors.branch);

  // Invalid phone format
  const v2 = TradeInCore.validateBookingForm({
    name: 'Nguyễn Văn A',
    phone: '123456',
    branch: 'Chi nhánh 1'
  });
  assert.strictEqual(v2.isValid, false);
  assert.ok(v2.errors.phone);

  // Valid VN phone
  const v3 = TradeInCore.validateBookingForm({
    name: 'Nguyễn Văn A',
    phone: '0987654321',
    branch: 'Chi nhánh 1: 241 Lê Văn Việt'
  });
  assert.strictEqual(v3.isValid, true);
  assert.strictEqual(v3.cleanedData.phone, '0987654321');
  console.log('✓ Test 7: Form Validation PASS');
}

// Test 8: Money formatting
{
  assert.strictEqual(TradeInCore.formatMoney(0), '0₫');
  assert.strictEqual(TradeInCore.formatMoney(41490000).replace(/\s/g, ''), '41.490.000₫'.replace(/\s/g, ''));
  console.log('✓ Test 8: Money Formatting PASS');
}

console.log('--- ALL TRADE-IN CORE TESTS PASSED SUCCESSFULLY! ---');
