using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public interface IWaiterDAO
        {
            Task<List<Products>> GetAllProductsAsync();
            Task<Products> GetProductByIdAsync(int productId);
            Task<Tables> GetTableByNumberAsync(int tableNumber);
            Task AddOrderAsync(Orders order);
            Task<List<Orders>> GetOrdersByUserIdAsync(int userId);
            Task UpdateTableAsync(Tables table);
            Task SaveChangesAsync();
        }
    public class WaiterDAO : IWaiterDAO
    {
        private readonly AdisyonDbContext _context;

        public WaiterDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Veritabanındaki tüm ürünleri getir
        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Verilen productId ile bir ürünü veritabanından getir
        public async Task<Products> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        // Verilen masa numarası ile bir masayı veritabanından getir
        public async Task<Tables> GetTableByNumberAsync(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }

        // Yeni bir siparişi veritabanına ekle
        public async Task AddOrderAsync(Orders order)
        {
            await _context.Orders.AddAsync(order);
        }

        // Belirli bir kullanıcının siparişlerini userId'ye göre getir
        public async Task<List<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(order => order.User_id == userId)
                .ToListAsync();
        }

        // Veritabanındaki masa bilgisini güncelle
        public async Task UpdateTableAsync(Tables table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        // Veritabanındaki yapılan değişiklikleri kaydet
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
