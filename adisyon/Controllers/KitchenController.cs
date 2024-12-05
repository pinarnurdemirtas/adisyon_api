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
        private readonly KitchenRepository _kitchenRepository;

        public KitchenController(KitchenRepository kitchenRepository)
        {
            _kitchenRepository = kitchenRepository;
        }

        // Hazırlanan siparişleri listeleme
        [HttpGet("list")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _kitchenRepository.GetOrdersAsync();
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
                var updatedOrder = await _kitchenRepository.UpdateOrderStatusAsync(id);

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