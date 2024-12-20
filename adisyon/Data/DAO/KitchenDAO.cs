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

        public async Task<List<Orders>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(order => order.Status == status)
                .ToListAsync();
        }

        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<Products> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateOrderAsync(Orders order)
        {
            _context.Orders.Update(order);
        }

        public async Task AddOrderCashAsync(OrderCash orderCash)
        {
            await _context.Ordercash.AddAsync(orderCash);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}