using adisyon.Models;


namespace adisyon.Data
{
    public class WaiterRepository
    {
        private readonly AdisyonDbContext _context;

        public WaiterRepository(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni sipariş oluşturma
        public async Task<Orders> CreateOrderAsync(Orders order)
        {
            // Ürün bilgisini al
            var product = await _context.Products.FindAsync(order.Product_id);

            if (product == null)
            {
                return null; // Ürün bulunamadı
            }

            order.Product_name = product.Name;
            order.Status = "Hazırlanıyor";
            order.Order_date = DateTime.Now;

            // Siparişi veritabanına ekle
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order; // Yeni oluşturulan siparişi geri döner
        }
    }
}