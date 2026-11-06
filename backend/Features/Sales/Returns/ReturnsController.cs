using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns;

[ApiController]
[Route("api/trahang")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnsService _returnsService;

    public ReturnsController(IReturnsService returnsService)
    {
        _returnsService = returnsService;
    }

    /// <summary>
    /// 1. Tiếp nhận hồ sơ đổi trả hàng
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> CreateTraHang([FromBody] CreateTraHangRequest request)
    {
        var result = await _returnsService.CreateTraHangAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// 2. Danh sách hồ sơ đổi trả kèm bộ lọc và phân trang
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> GetTraHangs(
        [FromQuery] string? trangThai,
        [FromQuery] string? loaiYeuCau,
        [FromQuery] string? hinhThuc,
        [FromQuery] DateOnly? tuNgay,
        [FromQuery] DateOnly? denNgay,
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _returnsService.GetTraHangsAsync(trangThai, loaiYeuCau, hinhThuc, tuNgay, denNgay, keyword, page, pageSize);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    /// <summary>
    /// 3. Tra cứu nhanh điều kiện đổi trả theo số IMEI
    /// </summary>
    [HttpGet("tracuu")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> TraCuuImei([FromQuery] string soIMEI)
    {
        var result = await _returnsService.TraCuuImeiAsync(soIMEI);
        return Ok(result);
    }

    /// <summary>
    /// 4. Chi tiết hồ sơ đổi trả kèm lịch sử nhật ký
    /// </summary>
    [HttpGet("{maTR}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> GetById([FromRoute] string maTR)
    {
        var result = await _returnsService.GetByIdAsync(maTR);
        return Ok(result);
    }

    /// <summary>
    /// 5. Kỹ thuật viên kiểm tra máy
    /// </summary>
    [HttpPut("{maTR}/kiemtra")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> KiemTra([FromRoute] string maTR, [FromBody] KiemTraTraHangRequest request)
    {
        var result = await _returnsService.KiemTraAsync(maTR, request);
        return Ok(result);
    }

    /// <summary>
    /// 6. Quản lý duyệt hoặc từ chối hồ sơ
    /// </summary>
    [HttpPut("{maTR}/duyet")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> Duyet([FromRoute] string maTR, [FromBody] DuyetTraHangRequest request)
    {
        var result = await _returnsService.DuyetAsync(maTR, request);
        return Ok(result);
    }

    /// <summary>
    /// 7. Hoàn tất hồ sơ: Hoàn tiền hoặc Cấp máy đổi mới
    /// </summary>
    [HttpPut("{maTR}/hoantat")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Cashier},{AppConstants.Roles.Sales}")]
    public async Task<IActionResult> HoanTat([FromRoute] string maTR, [FromBody] HoanTatTraHangRequest request)
    {
        var result = await _returnsService.HoanTatAsync(maTR, request);
        return Ok(result);
    }

    /// <summary>
    /// 8. Hủy hồ sơ tiếp nhận
    /// </summary>
    [HttpPut("{maTR}/huy")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> Huy([FromRoute] string maTR, [FromBody] HuyTraHangRequest request)
    {
        var result = await _returnsService.HuyAsync(maTR, request);
        return Ok(result);
    }

    /// <summary>
    /// 9. Quản lý đổi hình thức xử lý khi hết máy
    /// </summary>
    [HttpPut("{maTR}/hinhthuc")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> DoiHinhThuc([FromRoute] string maTR, [FromBody] DoiHinhThucRequest request)
    {
        var result = await _returnsService.DoiHinhThucAsync(maTR, request);
        return Ok(result);
    }
}
