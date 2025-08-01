using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public ShoppingCartController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto dto)
    {
        try
        {
            var orderId = await _checkoutService.ProcessOrderAsync(dto);
            return Ok(new { message = "Order placed successfully", orderId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
