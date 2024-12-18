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

        public async Task<object> GetAllReadyOrdersAsync()
        {
            var orders = await _context.Ordercash
                .Where(oc => oc.Status == "Hazırlandı")
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return Constants.NoReadyOrders; // Eğer hazırda sipariş yoksa dönecek mesaj
            }

            return orders; // "Hazırlandı" durumundaki tüm siparişleri döndür
        }
        

        // Siparişlerin durumunu "Ödendi" olarak güncelleyen metod
        public async Task<string> MarkOrdersAsPaidAsync(int tableNumber)
        {
            // "Hazırlandı" durumundaki siparişleri bul
            var ordersToUpdate = await _context.Ordercash
                .Where(oc => oc.Table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();

            if (ordersToUpdate == null || !ordersToUpdate.Any())
            {
                return Constants.TableEmpty; 
            }

            // Siparişlerin durumunu "Ödendi" olarak güncelle
            foreach (var order in ordersToUpdate)
            {
                order.Status = "Ödendi";
            }

            // İlgili siparişleri Orders tablosunda da güncelle
            var ordersInOrderTable = await _context.Orders
                .Where(o => ordersToUpdate.Select(oc => oc.Order_id).Contains(o.Order_id))
                .ToListAsync();

            foreach (var order in ordersInOrderTable)
            {
                order.Status = "Ödendi";
            }

            // Masanın Table_status'unu boş yap
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Table_number == tableNumber);
    
            if (table != null)
            {
                table.Table_status = "Boş"; // Masa durumu boş olarak güncelleniyor
                _context.Tables.Update(table);
            }

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return Constants.OrdersMarkedAsPaid; 
        }


    }
}
