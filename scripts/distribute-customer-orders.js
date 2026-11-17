/**
 * Script: distribute-customer-orders.js
 * Nhiệm vụ: Phân bổ 40 hóa đơn hiện có cho 15 khách hàng khác nhau
 * để tạo bảng xếp hạng Người dùng mua nhiều nhất (Top Customers) chân thực và sống động.
 */

const { execSync } = require('child_process');

function run() {
  console.log('🔄 Đang lấy danh sách hóa đơn từ PostgreSQL...');

  const getInvoicesCmd = `docker exec phonestore_postgres psql -U postgres -d PhoneStoreDB -t -A -F"," -c "SELECT sohd, thanhtien FROM hoadon WHERE sohd LIKE 'HD_SEED_%' ORDER BY thanhtien DESC;"`;
  const rawInvoices = execSync(getInvoicesCmd, { encoding: 'utf8' }).trim().split('\n').filter(Boolean);

  const customerList = [
    'KH26114517', // Anh Hoàng Đức Anh
    'KH002',      // Lê Hoàng Nam
    'KH001',      // Nguyễn Thị Hoa
    'KH26111695', // Khách VIP Flagship
    'KH003',      // Phạm Minh Tuấn
    'KH010',      // Trần Bảo Long
    'KH011',      // Lê Ngọc Mai
    'KH012',      // Phạm Quốc Bảo
    'KH013',      // Hoàng Thu Thảo
    'KH014',      // Vũ Đức Thịnh
    'KH015',      // Đỗ Thanh Hà
    'KH016',      // Bùi Anh Tuấn
    'KH017',      // Ngô Hải Yến
    'KH018',      // Đinh Trọng Hưng
    'KH019'       // Lý Gia Hân
  ];

  console.log(`📦 Tìm thấy ${rawInvoices.length} hóa đơn cần phân bổ.`);

  const sqlStatements = [];
  sqlStatements.push('BEGIN;\n');

  rawInvoices.forEach((line, idx) => {
    const [sohd] = line.split(',');
    // Gán hóa đơn xoay vòng theo danh sách khách hàng, ưu tiên top đầu nhiều đơn hơn
    let makh;
    if (idx < 6) makh = customerList[0];        // KH26114517: 6 đơn khủng
    else if (idx < 11) makh = customerList[1];   // KH002: 5 đơn
    else if (idx < 15) makh = customerList[2];   // KH001: 4 đơn
    else if (idx < 19) makh = customerList[3];   // KH26111695: 4 đơn
    else if (idx < 22) makh = customerList[4];   // KH003: 3 đơn
    else if (idx < 25) makh = customerList[5];   // KH010: 3 đơn
    else if (idx < 28) makh = customerList[6];   // KH011: 3 đơn
    else if (idx < 31) makh = customerList[7];   // KH012: 3 đơn
    else if (idx < 34) makh = customerList[8];   // KH013: 3 đơn
    else if (idx < 36) makh = customerList[9];   // KH014: 2 đơn
    else if (idx < 38) makh = customerList[10];  // KH015: 2 đơn
    else makh = customerList[11 + (idx % 4)];    // Các khách còn lại: 1 đơn

    sqlStatements.push(`UPDATE HoaDon SET MAKH = '${makh}' WHERE SOHD = '${sohd}';`);
  });

  sqlStatements.push('\nCOMMIT;\n');

  const fullSql = sqlStatements.join('\n');
  console.log('🔄 Đang cập nhật phân bổ khách hàng vào PostgreSQL...');
  execSync(`docker exec -i phonestore_postgres psql -U postgres -d PhoneStoreDB`, {
    input: fullSql,
    stdio: ['pipe', 'inherit', 'inherit']
  });

  console.log('🎉 Hoàn tất! Đã phân bổ hóa đơn cho các khách hàng.');
}

run();
