using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Interfaces;
using ShopApi.Models;

namespace ShopApi.Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly AppDbContext _context;

    public OrderDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDetail>> GetAllAsync()
    {
        return await _context.OrderDetails.ToListAsync();
    }

    public async Task<OrderDetail?> GetByIdAsync(int id)
    {
        return await _context.OrderDetails.FindAsync(id);
    }

    public async Task<OrderDetail> AddAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<bool> UpdateAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Update(orderDetail);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null) return false;

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
    {
        return await _context.OrderDetails
            .Where(od => od.OrderId == orderId)
            .Include(od => od.Product)
            .ToListAsync();
    }

    // public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}
