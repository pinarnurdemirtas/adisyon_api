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

        // Belirtilen masa numarasına ait "Hazırlandı" durumundaki siparişleri alan metod
        public async Task<object> GetOrdersByTableAsync(int tableNumber)
        {
            var orders = await _context.Order_cash
                .Where(oc => oc.Table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return Constants.TableEmpty;
            }

            return orders; 
        }
        

        // Siparişlerin durumunu "Ödendi" olarak güncelleyen metod
        public async Task<string> MarkOrdersAsPaidAsync(int tableNumber)
        {
            var ordersToUpdate = await _context.Order_cash
                .Where(oc => oc.Table_number == tableNumber && oc.Status == "Hazırlandı")
                .ToListAsync();

            if (ordersToUpdate == null || !ordersToUpdate.Any())
            {
                return Constants.TableEmpty; 
            }

            foreach (var order in ordersToUpdate)
            {
                order.Status = "Ödendi";
            }

            var ordersInOrderTable = await _context.Orders
                .Where(o => ordersToUpdate.Select(oc => oc.Order_id).Contains(o.Order_id))
                .ToListAsync();

            foreach (var order in ordersInOrderTable)
            {
                order.Status = "Ödendi";
            }

            await _context.SaveChangesAsync();

            return Constants.OrdersMarkedAsPaid; 
        }

    }
}
