using Microsoft.EntityFrameworkCore;
using adisyon.Models;
namespace adisyon.Data;

public class AdisyonDbContext : DbContext
{
    public AdisyonDbContext(DbContextOptions<AdisyonDbContext> options) : base(options) { }

    public DbSet<Urun> Products { get; set; } 
    public DbSet<Siparis> Orders { get; set; } 
    public DbSet<Detay> Order_details { get; set; } 
    public DbSet<Kisi> Users { get; set; } 
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detay>().HasNoKey();  
    }

}
