using Microsoft.EntityFrameworkCore;
using CustomerRegistration.API.Models;

namespace CustomerRegistration.API.Data;

public class CustomerRegistrationDbContext : DbContext
{
    public CustomerRegistrationDbContext(DbContextOptions<CustomerRegistrationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.CustomerNumber)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.IdentityNumber)
            .IsUnique();
    }
} 