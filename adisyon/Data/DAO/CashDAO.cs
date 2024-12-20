using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public class CashDAO
    {
        private readonly AdisyonDbContext _context;

        public CashDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Dolu masalardaki siparişleri getirme
        public async Task<List<OrderCash>> GetOrdersFromFullTables()
        {
            return await _context.Ordercash
                .Join(
                    _context.Tables,
                    oc => oc.Table_number,
                    t => t.Table_number,
                    (oc, t) => new { OrderCash = oc, Table = t }
                )
                .Where(joined => joined.Table.Table_status == "Dolu" && joined.OrderCash.Status == "Hazırlandı")
                .Select(joined => joined.OrderCash)
                .ToListAsync();
        }

        // Belirli bir masa numarasına ait siparişleri getirme
        public async Task<List<OrderCash>> GetOrdersByTableNumber(int tableNumber)
        {
            return await _context.Ordercash
                .Where(oc => oc.Table_number == tableNumber)
                .ToListAsync();
        }

        // Siparişlerin durumlarını güncelleme
        public async Task UpdateOrders(IEnumerable<OrderCash> orders)
        {
            _context.Ordercash.UpdateRange(orders);
        }

        // Belirli bir masayı güncelleme
        public async Task UpdateTableStatus(int tableNumber, string status)
        {
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Table_number == tableNumber);
            if (table != null)
            {
                table.Table_status = status;
                _context.Tables.Update(table);
            }
        }

        // Değişiklikleri kaydetme
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}