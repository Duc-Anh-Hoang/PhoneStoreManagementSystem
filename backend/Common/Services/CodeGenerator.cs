using System.Collections.Concurrent;
using System.Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PhoneStoreAPI.Data;

namespace PhoneStoreAPI.Common.Services;

public class CodeGenerator : ICodeGenerator
{
    private readonly AppDbContext _context;
    private static readonly ConcurrentDictionary<string, long> _fallbackCounters = new();

    private static readonly Dictionary<string, string> _dbSequences = new(StringComparer.OrdinalIgnoreCase)
    {
        { "PN", "seq_PhieuNhap" },
        { "HD", "seq_HoaDon" },
        { "BH", "seq_BaoHanh" },
        { "TR", "seq_TraHang" },
        { "TN", "seq_TiepNhan" },
        { "DH", "seq_DonHang" }
    };

    // Các bảng có khóa chính VARCHAR(10): KH, NV, NCC, KM, MH, NH, LH, TT
    private static readonly HashSet<string> _tenCharPrefixes = new(StringComparer.OrdinalIgnoreCase)
    {
        "KH", "NV", "NCC", "KM", "MH", "NH", "LH", "TT"
    };

    // Charset an toàn: loại bỏ 0, O, 1, I, L để tránh nhầm lẫn khi đọc
    private const string OrderCodeCharset = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";

    public CodeGenerator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> NextAsync(string prefix, string? sequenceName = null)
    {
        prefix = (prefix ?? string.Empty).Trim().ToUpper();
        long nextNumber = 0;

        // Chỉ gọi nextval() cho các sequence thực sự tồn tại trong 01-schema.sql để không làm abort transaction PostgreSQL
        if (_context.Database.IsRelational() && (sequenceName != null || _dbSequences.ContainsKey(prefix)))
        {
            string seq = sequenceName ?? _dbSequences[prefix];
            try
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                await using var cmd = conn.CreateCommand();
                if (_context.Database.CurrentTransaction != null)
                {
                    cmd.Transaction = _context.Database.CurrentTransaction.GetDbTransaction();
                }
                cmd.CommandText = $"SELECT nextval('{seq}')";
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    nextNumber = Convert.ToInt64(result);
                }
            }
            catch
            {
                nextNumber = _fallbackCounters.AddOrUpdate(seq, Random.Shared.Next(100, 9000), (_, current) => current + 1);
            }
        }
        else
        {
            string key = sequenceName ?? $"seq_{prefix}";
            nextNumber = _fallbackCounters.AddOrUpdate(key, Random.Shared.Next(100, 9000), (_, current) => current + 1);
        }

        string datePart = DateTime.UtcNow.ToString("yyMM");

        // Nếu là bảng có PK VARCHAR(10) (ví dụ KhachHang.MAKH, NhanVien.MANV): độ dài tối đa 10 ký tự (VD: KH26110123)
        if (_tenCharPrefixes.Contains(prefix))
        {
            long shortSeq = nextNumber % 10000;
            if (shortSeq == 0) shortSeq = 1;
            string shortCode = $"{prefix}{datePart}{shortSeq:D4}";
            return shortCode.Length > 10 ? shortCode.Substring(0, 10) : shortCode;
        }

        // Các bảng chứng từ có PK VARCHAR(15) (HD, PN, DH, BH, TR, TN): độ dài 11 ký tự (VD: DH261100001)
        long seqPart = nextNumber % 100000;
        if (seqPart == 0) seqPart = 1;

        string code = $"{prefix}{datePart}{seqPart:D5}";
        if (code.Length > 15)
        {
            code = code.Substring(0, 15);
        }

        return code;
    }

    /// <summary>
    /// Sinh mã đơn hàng ngẫu nhiên dạng "DH-XXXXXX" (6 ký tự từ charset an toàn, không có 0,O,1,I,L).
    /// Dùng RandomNumberGenerator để đảm bảo tính ngẫu nhiên mật mã học.
    /// Kiểm tra trùng lặp qua existsCheck, tối đa 10 lần thử.
    /// </summary>
    public async Task<string> GenerateOrderCode(Func<string, Task<bool>> existsCheck)
    {
        const int codeLength = 6;
        const int maxAttempts = 10;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Dùng RandomNumberGenerator.GetBytes() — nguồn ngẫu nhiên mật mã học (không dùng Random.Shared)
            var bytes = RandomNumberGenerator.GetBytes(codeLength);
            var chars = new char[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                // Modulo bias giảm thiểu bằng cách charset có 31 phần tử (< 256), sai số chấp nhận được
                chars[i] = OrderCodeCharset[bytes[i] % OrderCodeCharset.Length];
            }

            string candidate = "DH-" + new string(chars);

            // Kiểm tra trùng lặp với DB
            if (!await existsCheck(candidate))
            {
                return candidate;
            }
        }

        // Fallback cực hiếm: sau 10 lần vẫn trùng → dùng sequence tuần tự
        return await NextAsync("DH");
    }
}
