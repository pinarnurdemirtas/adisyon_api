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
        private readonly WaiterDAO _waiterDao;

        public WaiterController(WaiterDAO waiterDao)
        {
            _waiterDao = waiterDao;
        }

        // Yeni sipariş oluşturma
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdOrder = await _waiterDao.CreateOrderAsync(order);

            if (createdOrder == null)
            {
                return NotFound("Ürün bulunamadı");
            }

            return Ok(createdOrder);
        }

    }
}