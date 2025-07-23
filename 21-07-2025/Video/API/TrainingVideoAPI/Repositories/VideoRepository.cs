using Microsoft.EntityFrameworkCore;
using TrainingVideoAPI.Models;
using TrainingVideoAPI.Interfaces;
using TrainingVideoAPI.Data;

namespace TrainingVideoAPI.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly AppDbContext _context;

    public VideoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TrainingVideo video)
    {
        _context.TrainingVideos.Add(video);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TrainingVideo>> GetAllAsync()
    {
        return await _context.TrainingVideos.ToListAsync();
    }

    public async Task<TrainingVideo?> GetByIdAsync(int id)
    {
        return await _context.TrainingVideos.FindAsync(id);
    }
}
