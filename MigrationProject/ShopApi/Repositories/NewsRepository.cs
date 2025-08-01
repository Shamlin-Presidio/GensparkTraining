using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Interfaces;
using ShopApi.Models;

namespace ShopApi.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly AppDbContext _context;

    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<News>> GetAllAsync()
    {
        return await _context.News.ToListAsync();
    }

    public async Task<News?> GetByIdAsync(int id)
    {
        return await _context.News
            .FirstOrDefaultAsync(n => n.NewsId == id);
    }

    public async Task<News> AddAsync(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
        return news;
    }

    public async Task<bool> UpdateAsync(News news)
    {
        _context.News.Update(news);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var news = await GetByIdAsync(id);
        if (news == null) return false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<News>> GetTopNewsAsync(int count)
    {
        return await _context.News
            .Take(count)
            .ToListAsync();
    }

    // public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
