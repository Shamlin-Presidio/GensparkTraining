using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Interfaces;
using ShopApi.Models;

namespace ShopApi.Repositories;

public class ModelRepository : IModelRepository
{
    private readonly AppDbContext _context;

    public ModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Model>> GetAllAsync()
    {
        return await _context.Models.ToListAsync();
    }

    public async Task<IEnumerable<Model>> GetAllWithProductsAsync()
    {
        return await _context.Models
            .Include(m => m.Products)
            .ToListAsync();
    }

    public async Task<Model?> GetByIdAsync(int id)
    {
        return await _context.Models
            .FirstOrDefaultAsync(m => m.ModelId == id);
    }

    public async Task<Model> AddAsync(Model model)
    {
        _context.Models.Add(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<bool> UpdateAsync(Model model)
    {
        _context.Models.Update(model);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var model = await GetByIdAsync(id);
        if (model == null) return false;
        await _context.SaveChangesAsync();
        return true;
    }

    // public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }


}
