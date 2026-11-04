using System.Collections.Concurrent;
using System.Data;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;

namespace PhoneStoreAPI.Common.Services;

public class CodeGenerator : ICodeGenerator
{
    private readonly AppDbContext _context;
    private static readonly ConcurrentDictionary<string, long> _fallbackCounters = new();

    private static readonly Dictionary<string, string> _defaultSequences = new(StringComparer.OrdinalIgnoreCase)
    {
        { "PN", "seq_PhieuNhap" },
        { "HD", "seq_HoaDon" },
        { "BH", "seq_BaoHanh" },
        { "TR", "seq_TraHang" },
        { "TN", "seq_TiepNhan" },
        { "DH", "seq_DonHang" },
        { "NV", "seq_NhanVien" },
        { "KH", "seq_KhachHang" }
    };

    public CodeGenerator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> NextAsync(string prefix, string? sequenceName = null)
    {
        prefix = (prefix ?? string.Empty).Trim().ToUpper();
        string seq = sequenceName 
            ?? (_defaultSequences.TryGetValue(prefix, out var s) ? s : $"seq_{prefix}");

        long nextNumber = 0;

        // Nếu là relational database (PostgreSQL), gọi sequence database
        if (_context.Database.IsRelational())
        {
            try
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT nextval('{seq}')";
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    nextNumber = Convert.ToInt64(result);
                }
            }
            catch
            {
                // Fallback nếu sequence chưa được tạo
                nextNumber = _fallbackCounters.AddOrUpdate(seq, 1, (_, current) => current + 1);
            }
        }
        else
        {
            // In-Memory test fallback
            nextNumber = _fallbackCounters.AddOrUpdate(seq, 1, (_, current) => current + 1);
        }

        // Format: Tiền tố + YYMM (4 số) + Thứ tự modulo (5 số) => Độ dài: 2 + 4 + 5 = 11 ký tự (<= 15 ký tự)
        // Ví dụ: HD261100001, PN261100002
        string datePart = DateTime.UtcNow.ToString("yyMM");
        long seqPart = nextNumber % 100000;
        if (seqPart == 0) seqPart = 1;

        string code = $"{prefix}{datePart}{seqPart:D5}";
        if (code.Length > 15)
        {
            code = code.Substring(0, 15);
        }

        return code;
    }
}
