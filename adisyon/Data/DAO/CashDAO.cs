using Microsoft.EntityFrameworkCore;
using adisyon.Models;

namespace adisyon.Data
{
    public class CashDAO
    {
        private readonly AdisyonDbContext _context;

        public CashDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // "Hazırlandı" durumundaki siparişleri getirme
        public async Task<List<OrderCash>> GetReadyOrdersByTableAsync(int tableNumber)
        {
            return await _context.Ordercash
                .Where(oc => oc.Table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();
        }

        // Belirli bir masanın durumunu getirme
        public async Task<Tables> GetTableByNumberAsync(int tableNumber)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.Table_number == tableNumber);
        }

        // Sipariş durumlarını güncelleme
        public async Task UpdateOrdersAsync(IEnumerable<OrderCash> orders)
        {
            _context.Ordercash.UpdateRange(orders);
        }

        // Masayı güncelleme
        public async Task UpdateTableAsync(Tables table)
        {
            _context.Tables.Update(table);
        }

        // Siparişler tablosunda ilgili kayıtları getirme
        public async Task<List<Orders>> GetOrdersByIdsAsync(IEnumerable<int> orderIds)
        {
            return await _context.Orders
                .Where(o => orderIds.Contains(o.Order_id))
                .ToListAsync();
        }

        // Orders tablosundaki siparişleri güncelleme
        public async Task UpdateOrdersInOrdersTableAsync(IEnumerable<Orders> orders)
        {
            _context.Orders.UpdateRange(orders);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}