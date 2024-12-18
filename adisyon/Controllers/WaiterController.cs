using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using adisyon;  // Constants sınıfını kullanmak için ekleniyor

namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "garson")]
    public class WaiterController : ControllerBase
    {
        private readonly WaiterDAO _waiterDAO;

        public WaiterController(WaiterDAO waiterDAO)
        {
            _waiterDAO = waiterDAO;
        }

        // Menüyü görüntüleme
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _waiterDAO.GetAllProducts();

            if (products == null || !products.Any())
            {
                return NotFound(Constants.ProductsNotFound);
            }

            return Ok(products); 
        }
        
        // Sipariş oluşturma
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            if (order == null)
            {
                return BadRequest(Constants.MissingOrder);  
            }
            
            var result = await _waiterDAO.CreateOrder(order);
            if (result == Constants.ProductsNotFound)
            {
                return NotFound(Constants.ProductsNotFound);  
            }
            if (result == Constants.TableNotFound)
            {
                return NotFound(Constants.TableNotFound);
            }
            
            return Ok(Constants.OrderSentToKitchen);
        }

        // Siparişleri görüntüleme
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _waiterDAO.GetAllOrders();

            if (orders == null || !orders.Any())
            {
                return NotFound(Constants.NoActiveOrders);  // Constants'dan mesaj alınır
            }

            return Ok(orders); 
        }
    }
}