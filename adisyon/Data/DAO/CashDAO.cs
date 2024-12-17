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

        // Belirtilen masa numarasına ait "Hazırlandı" durumundaki siparişleri alan metod
        public async Task<List<OrderCash>> GetOrdersByTableAsync(int tableNumber)
        {
            return await _context.Order_cash
                .Where(oc => oc.table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();
        }

        // "Hazırlandı" durumundaki masaların numaralarını getiren metod
        public async Task<List<int>> GetTablesWithReadyOrdersAsync()
        {
            return await _context.Order_cash
                .Where(oc => oc.Status == "Hazırlandı")
                .Select(oc => oc.table_number)
                .Distinct()
                .ToListAsync();
        }

        // Siparişlerin durumunu "Ödendi" olarak güncelleyen metod
        public async Task<bool> MarkOrdersAsPaidAsync(int tableNumber)
        {
            var ordersToUpdate = await _context.Order_cash
                .Where(oc => oc.table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();

            if (ordersToUpdate == null || !ordersToUpdate.Any())
            {
                return false; // Eğer siparişler yoksa veya hepsi "Hazırlandı" değilse
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