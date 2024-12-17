using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public class WaiterDAO
    {
        private readonly AdisyonDbContext _context;

        public WaiterDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni sipariş oluşturma methodu
        public async Task<string> CreateOrderAsync(CreateOrder order)
        {
            var product = await _context.Products.FindAsync(order.Product_id);

            if (product == null)
            {
                return null; 
            }

            var newOrder = new Orders
            {
                Table_number = order.Table_number,
                Product_id = order.Product_id,
                Product_name = product.Name,
                Quantity = order.Quantity,
                Status = "Hazırlanıyor",
                Order_date = DateTime.Now
            };

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return Constants.OrderSentToKitchen;
        }


        // Masa numarasına göre siparişleri getiren method
        public async Task<List<Orders>> GetOrdersByTableNumberAsync(int tableNumber)
        {
            var orders = await _context.Orders
                .Where(o => o.Table_number == tableNumber)
                .ToListAsync();

            return orders; 
        }
    }
}