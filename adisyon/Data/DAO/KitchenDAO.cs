using Microsoft.EntityFrameworkCore;
using adisyon.Models;

namespace adisyon.Data
{
    public class KitchenDAO
    {
        private readonly AdisyonDbContext _context;

        public KitchenDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Siparişlerin durumunu status parametresine göre filtrele
        public async Task<List<Orders>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(order => order.Status == status)
                .ToListAsync();
        }

        // Verilen orderId'ye ait siparişi getir
        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        // Verilen productId'ye sahip ürünü veritabanından getir
        public async Task<Products> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        // Verilen siparişi veritabanında güncelle
        public async Task UpdateOrderAsync(Orders order)
        {
            _context.Orders.Update(order);
        }

        // Yeni bir OrderCash nesnesini veritabanına ekle
        public async Task AddOrderCashAsync(OrderCash orderCash)
        {
            await _context.Ordercash.AddAsync(orderCash);
        }

        // Değişiklikleri veritabanına kaydet
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
