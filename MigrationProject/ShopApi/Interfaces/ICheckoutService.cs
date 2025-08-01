using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces;

public interface ICheckoutService
{
    Task<int> ProcessOrderAsync(CheckoutRequestDto dto);
}
