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

        // "Hazırlanıyor" durumdaki siparişleri listeleyen method
        public async Task<object> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Where(order => order.Status == "Hazırlanıyor") 
                .Select(order => new Orders
                {
                    Order_id = order.Order_id,
                    Product_id = order.Product_id,
                    Product_name = order.Product_name ?? "Unknown",
                    Quantity = order.Quantity,
                    Table_number = order.Table_number,
                    Status = order.Status,
                    Order_date = order.Order_date,
                    User_id = order.User_id,
                })
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return Constants.NoActiveOrders; 
            }

            return orders; 
        }
        
        // Ürün fiyatını alma
        private async Task<decimal> GetProductPrice(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.Price ?? 0;
        } 
        
        // Sipariş durumunu "Hazırlandı" olarak güncelleyen method
        public async Task<string> UpdateOrderStatusAsync(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Order_id == id);

            if (order == null)
            {
                return Constants.NoActiveOrders; 
            }

            var productPrice = await GetProductPrice(order.Product_id);

            order.Status = "Hazırlandı";
            await _context.SaveChangesAsync();

            var orderCash = new OrderCash
            {
                Order_id = order.Order_id,
                Product_id = order.Product_id,
                Quantity = order.Quantity,
                Product_price = productPrice,
                Total_price = order.Quantity * productPrice, 
                Order_date = order.Order_date,
                Table_number = order.Table_number,
                Status = order.Status,
            };

            await _context.Ordercash.AddAsync(orderCash);
            await _context.SaveChangesAsync();
            
            return Constants.OrderStatusUpdated;
        }


    }
}