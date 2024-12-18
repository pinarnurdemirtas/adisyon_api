using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaiterController : ControllerBase
    {
        private readonly WaiterDAO _waiterDAO;

        public WaiterController(WaiterDAO waiterDAO)
        {
            _waiterDAO = waiterDAO;
        }

        // Tüm siparişleri listele
        [HttpGet("allOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _waiterDAO.GetAllOrders();
            return Ok(orders);
        }
        
        
        // Menüye bak
        [HttpGet("menu")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _waiterDAO.GetAllProducts();
            return Ok(products);
        }
        
        
        // Sipariş oluşturma
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            var newOrder = await _waiterDAO.CreateOrder(order);

            if (newOrder == null)
            {
                if (order.Product_id == 0)  // Ürün bulunamadı
                {
                    return BadRequest(Constants.ProductsNotFound);
                }
                if (order.Table_number == 0)  // Masa bulunamadı
                {
                    return BadRequest(Constants.TableNotFound);
                }
            }
            return Ok(Constants.OrderSentToKitchen);  // Sipariş mutfağa iletildi
        }
        
    }
}