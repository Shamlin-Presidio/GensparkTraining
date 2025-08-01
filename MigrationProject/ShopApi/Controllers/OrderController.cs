using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        var orders = await _orderService.GetPagedOrdersAsync(page, pageSize);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        var created = await _orderService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetOrder), new { id = created.OrderId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderUpdateDto dto)
    {
        var updated = await _orderService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _orderService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportToPdf()
    {
        var fileBytes = await _orderService.ExportToPdfAsync();
        return File(fileBytes, "application/pdf", $"OrderListing_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf");
    }
}
