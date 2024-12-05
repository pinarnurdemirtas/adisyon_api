using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;

namespace adisyon.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "garson")]
    public class WaiterController : ControllerBase
    {
        private readonly WaiterRepository _waiterRepository;

        public WaiterController(WaiterRepository waiterRepository)
        {
            _waiterRepository = waiterRepository;
        }

        // Yeni sipariş oluşturma
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] Orders order)
        {
            if (order == null)
            {
                return BadRequest("Order data is missing.");
            }

            var createdOrder = await _waiterRepository.CreateOrderAsync(order);

            if (createdOrder == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(new { message = "Order created successfully.", createdOrder });
        }
    }
}