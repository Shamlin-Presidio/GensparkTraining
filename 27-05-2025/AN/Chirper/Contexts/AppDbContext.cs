using Microsoft.EntityFrameworkCore;
using Chirper.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Hashtag> Hashtags { get; set; }
    public DbSet<TweetHashtag> TweetHashtags { get; set; }

}
