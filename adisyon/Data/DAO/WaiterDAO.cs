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

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<Tables> GetTableByNumberAsync(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }

        public async Task AddOrderAsync(Orders order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(order => order.User_id == userId)
                .ToListAsync();
        }


        public async Task UpdateTableAsync(Tables table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}