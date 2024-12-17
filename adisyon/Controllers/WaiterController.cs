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

        //Sipariş oluşturma
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            if (order == null)
            {
                return BadRequest("Order data is required.");
            }

            var result = await _waiterDAO.CreateOrderAsync(order);

            if (result == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(result); 
        }

        // Masaya ait siparişleri görüntüleme
        [HttpGet("orders/{tableNumber}")]
        public async Task<IActionResult> GetOrdersByTableNumber(int tableNumber)
        {
            var orders = await _waiterDAO.GetOrdersByTableNumberAsync(tableNumber);

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the given table number.");
            }

            return Ok(orders); 
        }
    }
}