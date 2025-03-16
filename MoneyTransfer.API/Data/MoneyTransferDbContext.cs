using Microsoft.EntityFrameworkCore;
using MoneyTransfer.API.Models;

namespace MoneyTransfer.API.Data;

public class MoneyTransferDbContext : DbContext
{
    public MoneyTransferDbContext(DbContextOptions<MoneyTransferDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.TransactionNumber)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
} 