using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public interface IWaiterDAO
    {
        Task<List<Products>> GetAllProducts();
        Task<Products> GetProductById(int productId);
        Task<Tables> GetTableByNumber(int tableNumber);
        Task AddOrder(Orders order);
        Task<List<Orders>> GetOrdersByUserId(int userId);
        Task UpdateTable(Tables table);
        Task SaveChanges();
    }
    public class WaiterDAO : IWaiterDAO
    {
        private readonly AdisyonDbContext _context;

        public WaiterDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Veritabanındaki tüm ürünleri getir
        public async Task<List<Products>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // Verilen productId ile bir ürünü veritabanından getir
        public async Task<Products> GetProductById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        // Verilen masa numarası ile bir masayı veritabanından getir
        public async Task<Tables> GetTableByNumber(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }

        // Yeni bir siparişi veritabanına ekle
        public async Task AddOrder(Orders order)
        {
            await _context.Orders.AddAsync(order);
        }

        // Belirli bir kullanıcının siparişlerini userId'ye göre getir
        public async Task<List<Orders>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Where(order => order.User_id == userId)
                .ToListAsync();
        }

        // Veritabanındaki masa bilgisini güncelle
        public async Task UpdateTable(Tables table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        // Veritabanındaki yapılan değişiklikleri kaydet
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
