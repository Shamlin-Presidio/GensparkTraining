using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Models;

namespace EventManagementAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
      public DbSet<Wallet> Wallets => Set<Wallet>();
      public DbSet<WalletTransactions> WalletTransactions => Set<WalletTransactions>();

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            //   U S E R   
            modelBuilder.Entity<User>(entity =>
            {
                  entity.HasKey(u => u.Id);
                  entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                  entity.Property(u => u.PasswordHash).IsRequired();
                  entity.Property(u => u.Role).IsRequired();
                  // entity.Property(u => u.ProfilePicturePath).HasMaxLength(255);
                  entity.Property(u => u.CreatedAt).HasDefaultValueSql("NOW()");

                  entity.HasOne(u => u.Wallet)
                        .WithOne(w => w.User)
                        .HasForeignKey<User>(u => u.WalletId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            //   E V E N T
            modelBuilder.Entity<Event>(entity =>
            {
                  entity.HasKey(e => e.Id);

                  entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                  entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                  entity.Property(e => e.StartTime)
                    .IsRequired();

                  entity.Property(e => e.EndTime)
                    .IsRequired();

                  entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(200);

                  entity.Property(e => e.GoogleMapLink)
                    .HasMaxLength(300);

                  entity.Property(e => e.OnlineMeetUrl)
                    .HasMaxLength(300);

                  // entity.Property(e => e.ImagePath)
                  // .HasMaxLength(255);

                  entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                  entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                  entity.HasQueryFilter(e => !e.IsDeleted);
                  entity.HasOne(e => e.Organizer)
                    .WithMany(u => u.OrganizedEvents)
                    .HasForeignKey(e => e.OrganizerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //   R E G I S T R A T I O N
            modelBuilder.Entity<Registration>(entity =>
            {
                  entity.HasKey(r => r.Id);

                  entity.Property(r => r.RegisteredAt)
                    .HasDefaultValueSql("NOW()");

                  entity.HasOne(r => r.Event)
                    .WithMany(e => e.Registrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                  entity.HasOne(r => r.Attendee)
                    .WithMany(u => u.Registrations)
                    .HasForeignKey(r => r.AttendeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                  entity.HasIndex(r => new { r.EventId, r.AttendeeId }).IsUnique();
            });

            // Wallet
            modelBuilder.Entity<Wallet>(entity =>
            {
                  entity.HasKey(w => w.Id);

                  entity.HasMany(w => w.Transactions)
                        .WithOne(wt => wt.Wallet);
            });

            // WalletTransactions
            modelBuilder.Entity<WalletTransactions>(entity =>
            {
                  entity.HasKey(wt => wt.Id);

                  entity.HasOne(wt => wt.Wallet)
                        .WithMany(w => w.Transactions)
                        .HasForeignKey(wt => wt.WalletId)
                        .OnDelete(DeleteBehavior.Cascade);
            });
      }
}
