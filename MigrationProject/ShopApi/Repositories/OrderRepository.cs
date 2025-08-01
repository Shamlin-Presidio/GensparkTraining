using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Interfaces;
using ShopApi.Models;

// public class OrderRepository : IOrderRepository
// {
//     private readonly AppDbContext _context;

//     public OrderRepository(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<IEnumerable<Order>> GetAllAsync()
//     {
//         return await _context.Orders.ToListAsync();
//     }

//     public async Task<Order?> GetByIdAsync(int id)
//     {
//         return await _context.Orders.FindAsync(id);
//     }

//     public async Task<Order> AddAsync(Order order)
//     {
//         _context.Orders.Add(order);
//         await _context.SaveChangesAsync();
//         return order;
//     }

//     public async Task<bool> UpdateAsync(Order order)
//     {
//         _context.Orders.Update(order);
//         await _context.SaveChangesAsync();
//         return true;
//     }

//     public async Task<bool> DeleteAsync(int id)
//     {
//         var order = await _context.Orders.FindAsync(id);
//         if (order == null) return false;

//         _context.Orders.Remove(order);
//         await _context.SaveChangesAsync();
//         return true;
//     }

//     public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
//     {
//         return await _context.Orders
//             .Where(o => o.UserId == userId)
//             .ToListAsync();
//     }

//     public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync()
//     {
//         return await _context.Orders
//             .Include(o => o.OrderDetails)
//                 .ThenInclude(od => od.Product)
//             .Include(o => o.User)
//             .ToListAsync();
//     }

//     public async Task<Order?> GetOrderWithDetailsByIdAsync(int id)
//     {
//         return await _context.Orders
//             .Include(o => o.OrderDetails)
//                 .ThenInclude(od => od.Product)
//             .Include(o => o.User)
//             .FirstOrDefaultAsync(o => o.OrderId == id);
//     }

//     public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
// }
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Order>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Orders
                .OrderByDescending(o => o.OrderId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
