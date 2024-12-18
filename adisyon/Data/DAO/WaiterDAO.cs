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

        // Menüyü listeleyen metod
        public async Task<List<Products>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products; 
        }
        
        // Yeni sipariş oluşturma methodu
        public async Task<Orders> CreateOrder(CreateOrder order)
        {
            // İlgili ürünü bul
            var product = await _context.Products.FindAsync(order.Product_id);
            if (product == null)
            {
                return null;  
            }

            // İlgili masayı bul
            var table = await _context.Tables.FindAsync(order.Table_number);
            if (table == null)
            {
                return null;  
            }

            // Yeni siparişi oluştur
            var newOrder = new Orders
            {
                Table_number = order.Table_number,
                Product_id = order.Product_id,
                Product_name = product.Name,
                Quantity = order.Quantity,
                Status = "Hazırlanıyor",
                Order_date = DateTime.Now,
                User_id = order.User_id,
            };

            // Siparişi ekle ve kaydet
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            // Masanın durumunu "Dolu" olarak güncelle
            table.Table_status = "Dolu";
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();

            return newOrder; // Sipariş başarıyla oluşturuldu
        }

        // Siparişleri getiren method
        public async Task<List<Orders>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders; 
        }
    }
}
