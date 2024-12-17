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

        // Hazırlanan siparişleri listeleme (KitchenOrderDTO ile)
        public async Task<List<KitchenOrders>> GetOrdersAsync()
        {
            return await _context.Orders
                .Select(order => new KitchenOrders
                {
                    Product_name = order.Product_name ?? "Unknown",
                    Quantity = order.Quantity,
                    Status = order.Status
                })
                .ToListAsync();
        }
        
        private async Task<decimal> GetProductPrice(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.Price ?? 0;
        } 
        
        // Sipariş durumunu 'Hazırlandı' olarak güncelleme
        public async Task<Orders> UpdateOrderStatusAsync(int id)
        {
            // Siparişi bul
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Order_id == id);

            if (order == null)
            {
                return null; // Sipariş bulunamadı
            }

            // Ürün fiyatını al
            var productPrice = await GetProductPrice(order.Product_id);

            // Siparişi 'Hazırlandı' olarak güncelle
            order.Status = "Hazırlandı";
            await _context.SaveChangesAsync();

            // OrderCash tablosuna yazma
            var orderCash = new OrderCash
            {
                Order_id = order.Order_id,
                Product_id = order.Product_id,
                Quantity = order.Quantity,
                Product_price = productPrice,
                Total_price = order.Quantity * productPrice, // Siparişin toplam fiyatını hesapla
                Order_date = order.Order_date,
                Status = order.Status
            };

            // OrderCash tablosuna kaydetme
            await _context.Order_cash.AddAsync(orderCash);
            await _context.SaveChangesAsync();
    
            return order; // Güncellenmiş siparişi geri döner
        }

    }
}