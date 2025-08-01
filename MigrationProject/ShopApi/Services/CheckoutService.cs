using ShopApi.Dtos;
using ShopApi.Interfaces;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services;

public class CheckoutService : ICheckoutService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public CheckoutService(IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<int> ProcessOrderAsync(CheckoutRequestDto dto)
    {
        var order = new Order
        {
            UserId = dto.UserId,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            CustomerEmail = dto.CustomerEmail,
            CustomerAddress = dto.CustomerAddress,
            PaymentType = dto.PaymentType
        };

        foreach (var item in dto.CartItems)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {item.ProductId} not found");

            var detail = new OrderDetail
            {
                ProductId = product.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };

            order.OrderDetails.Add(detail);
        }

        await _orderRepo.AddAsync(order);
        return order.OrderId;
    }
}
