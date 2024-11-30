using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using adisyon.Data;
using adisyon.Models;

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AdisyonDbContext _context;

        public OrdersController(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni sipariş oluşturma
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] Orders order)
        {
            if (order == null) return BadRequest("Order data is missing.");

            order.Status = "hazırlanıyor";
            order.Order_date = DateTime.Now;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order created successfully.", order });
        }

        // Tüm siparişleri listeleme
        [HttpGet("list")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        // Sipariş durumunu 'Hazırlandı' olarak güncelleme
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound("Order not found.");

            order.Status = "hazırlandı";

            // OrderCash tablosuna ekleme
            var orderCash = new OrderCash()
            {
                Order_id = order.Order_id,
                Product_id = order.Product_id,
                Quantity = order.Quantity,
                Product_price = await GetProductPrice(order.Product_id),
                Total_price = await GetProductPrice(order.Product_id) * order.Quantity,
                Status = "hazırlandı",
                Order_date = order.Order_date
            };

            _context.Orders.Remove(order); // Orders tablosundan silme
            await _context.Order_cash.AddAsync(orderCash);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order status updated and moved to OrderCash.", orderCash });
        }

        // Helper: Ürün fiyatını bulma
        private async Task<decimal> GetProductPrice(int productId)
        {
            // Örnek bir ürün tablosu olduğunu varsayarak buraya sorgu eklenir
            var product = await _context.Products.FindAsync(productId);
            return product?.Price ?? 0;
        }

        // Tüm OrderCash kayıtlarını listeleme
        [HttpGet("cash-list")]
        public async Task<IActionResult> GetOrderCashList()
        {
            var orderCashList = await _context.Order_cash.ToListAsync();
            return Ok(orderCashList);
        }
    }
}
