using adisyon.Models;


namespace adisyon.Data
{
    public class WaiterDAO
    {
        private readonly AdisyonDbContext _context;

        // AdisyonDbContext'i alan yapıcı
        public WaiterDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni sipariş oluşturma
        public async Task<Orders> CreateOrderAsync(CreateOrder order)
        {
            // Ürün bilgisini al
            var product = await _context.Products.FindAsync(order.Product_id);

            if (product == null)
            {
                return null; // Ürün bulunamadı
            }

            // Sipariş nesnesini oluştur
            var newOrder = new Orders
            {
                Table_number = order.Table_number,
                Product_id = order.Product_id,
                Product_name = product.Name,
                Quantity = order.Quantity,
                Status = "Hazırlanıyor", 
                Order_date = DateTime.Now
            };

            // Siparişi veritabanına ekle
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder; // Yeni oluşturulan siparişi geri döner
        }

    }
}