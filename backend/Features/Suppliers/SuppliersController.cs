using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Suppliers.DTOs;

namespace PhoneStoreAPI.Features.Suppliers;

[ApiController]
[Route("api/suppliers")]
[AllowAnonymous]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var (items, totalCount) = await _supplierService.GetAllAsync(page, pageSize);
        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var item = await _supplierService.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request)
    {
        var created = await _supplierService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.MaNCC }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateSupplierRequest request)
    {
        var updated = await _supplierService.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _supplierService.DeleteAsync(id);
        return Ok(new { message = $"Đã xóa nhà cung cấp {id} thành công." });
    }
}
