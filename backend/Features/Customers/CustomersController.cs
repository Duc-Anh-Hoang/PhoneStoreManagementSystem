using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Customers.DTOs;

namespace PhoneStoreAPI.Features.Customers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var (items, totalCount) = await _customerService.GetAllAsync(keyword, page, pageSize);
        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(items);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> GetById(string id)
    {
        var item = await _customerService.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpGet("sdt/{sdt}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByPhone(string sdt)
    {
        var item = await _customerService.GetByPhoneAsync(sdt);
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var created = await _customerService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.MaKH }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCustomerRequest request)
    {
        var updated = await _customerService.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _customerService.DeleteAsync(id);
        return Ok(new { message = $"Đã xóa khách hàng {id} thành công." });
    }
}
