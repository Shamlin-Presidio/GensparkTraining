namespace ShopApi.Dtos;

public class CheckoutRequestDto
{
    public int UserId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string PaymentType { get; set; } = "Cash";

    public List<CartItemDto> CartItems { get; set; } = new();
}
