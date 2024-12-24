using Microsoft.EntityFrameworkCore;
using adisyon.Models;

namespace adisyon.Data
{ 
    public interface IKitchenDAO
    {
        Task<List<Orders>> GetOrdersByStatus(string status);
        Task<Orders> GetOrderById(int orderId);
        Task<Products> GetProductById(int productId);
        Task UpdateOrder(Orders order);
        Task AddOrderCash(OrderCash orderCash);
        Task SaveChanges();
    }
        
    public class KitchenDAO : IKitchenDAO
    {
        private readonly AdisyonDbContext _context;

        public KitchenDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Siparişlerin durumunu status parametresine göre filtrele
        public async Task<List<Orders>> GetOrdersByStatus(string status)
        {
            return await _context.Orders
                .Where(order => order.Status == status)
                .ToListAsync();
        }

        // Verilen orderId'ye ait siparişi getir
        public async Task<Orders> GetOrderById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        // Verilen productId'ye sahip ürünü veritabanından getir
        public async Task<Products> GetProductById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        // Verilen siparişi veritabanında güncelle
        public async Task UpdateOrder(Orders order)
        {
            _context.Orders.Update(order);
        }

        // Yeni bir OrderCash nesnesini veritabanına ekle
        public async Task AddOrderCash(OrderCash orderCash)
        {
            await _context.Ordercash.AddAsync(orderCash);
        }

        // Değişiklikleri veritabanına kaydet
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
