using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<WatchHistory> WatchHistories => Set<WatchHistory>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------- USER ----------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.UserId);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.PreferredLanguage)
                  .HasMaxLength(50);
        });

        // ---------------- WATCH HISTORY ----------------
        modelBuilder.Entity<WatchHistory>(entity =>
        {
            entity.ToTable("watchHistory");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.WatchTime)
                  .IsRequired();

            entity.Property(x => x.Timestamp);
                 

            entity.HasIndex(x => new { x.UserId, x.MovieId }); // 🚀 performance

            entity.HasOne(x => x.User)
                  .WithMany(u => u.WatchHistories)
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- USER PREFERENCES ----------------
        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.ToTable("userPreferences");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FavoriteGenre)
                  .HasMaxLength(50);

            entity.HasOne(x => x.User)
                  .WithMany()
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}