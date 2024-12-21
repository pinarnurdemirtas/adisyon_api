using Microsoft.EntityFrameworkCore;
using adisyon.Models;
namespace adisyon.Data;

public partial  class AdisyonDbContext : DbContext
{

    public virtual DbSet<Products> Products { get; set; } 
    public virtual DbSet<Orders> Orders { get; set; } 
    public virtual DbSet<OrderCash> Ordercash { get; set; } 
    public virtual DbSet<Users> Users { get; set; } 
    public virtual DbSet<Tables> Tables { get; set; } 

    

    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orders>()
            .HasKey(o => o.Order_id);
        modelBuilder.Entity<OrderCash>()
            .HasKey(c => c.Cash_id);
        modelBuilder.Entity<Tables>()
            .HasKey(o => o.Table_number);
    }
}
