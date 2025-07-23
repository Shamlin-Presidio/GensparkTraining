using Microsoft.EntityFrameworkCore;
using TrainingVideoAPI.Models;

namespace TrainingVideoAPI.Data;
public class AppDbContext : DbContext
{
    public DbSet<TrainingVideo> TrainingVideos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
