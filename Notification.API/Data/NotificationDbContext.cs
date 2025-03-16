using Microsoft.EntityFrameworkCore;
using Notification.API.Models;

namespace Notification.API.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Notification>()
            .HasIndex(n => new { n.UserId, n.CreatedAt });
    }
} 