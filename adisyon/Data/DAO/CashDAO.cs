using Microsoft.EntityFrameworkCore;
using adisyon.Models;

namespace adisyon.Data
{
    public class CashDAO
    {
        private readonly AdisyonDbContext _context;

        // AdisyonDbContext'i alan yapıcı
        public CashDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Belirtilen masa numarasına ait tüm siparişleri alan metod
        public async Task<List<OrderCash>> GetOrdersByTableAsync(int tableNumber)
        {
            return await _context.Order_cash
                .Where(oc => oc.table_number == tableNumber)
                .ToListAsync();
        }

        // Siparişlerin durumunu "ödendi" olarak güncelleyen metod
        public async Task<bool> MarkOrdersAsPaidAsync(int tableNumber)
        {
            var ordersToUpdate = await _context.Order_cash
                .Where(oc => oc.table_number == tableNumber && oc.Status != "Ödendi")
                .ToListAsync();

            if (ordersToUpdate == null || !ordersToUpdate.Any())
            {
                return false; // Eğer siparişler yoksa veya hepsi zaten "ödendi" durumundaysa
            }

            foreach (var order in ordersToUpdate)
            {
                order.Status = "Ödendi";
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}