using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using Microsoft.AspNetCore.Authorization;

namespace adisyon.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "mutfak")]
    public class KitchenController : ControllerBase
    {
        private readonly KitchenDAO _kitchenDao;

        public KitchenController(KitchenDAO kitchenDao)
        {
            _kitchenDao = kitchenDao;
        }

        // Hazırlanan siparişleri listeleme
        [HttpGet("list")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _kitchenDao.GetOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Sipariş durumunu 'Hazırlandı' olarak güncelleme
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id)
        {
            try
            {
                var updatedOrder = await _kitchenDao.UpdateOrderStatusAsync(id);

                if (updatedOrder == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                return Ok(new
                {
                    Message = $"Order with ID {id} has been updated to 'Hazırlandı'.",
                    Order = updatedOrder
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}