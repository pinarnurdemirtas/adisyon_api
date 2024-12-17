using Microsoft.EntityFrameworkCore;
using adisyon.Models;
namespace adisyon.Data;

public class AdisyonDbContext : DbContext
{
    public AdisyonDbContext(DbContextOptions<AdisyonDbContext> options) : base(options) { }

    public DbSet<Products> Products { get; set; } 
    public DbSet<Orders> Orders { get; set; } 
    public DbSet<OrderCash> Order_cash { get; set; } 
    public DbSet<Users> Users { get; set; } 
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orders>()
            .HasKey(o => o.Order_id);

        modelBuilder.Entity<OrderCash>()
            .HasKey(c => c.Cash_id);
    }
}
