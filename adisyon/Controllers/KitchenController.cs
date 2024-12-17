using Microsoft.AspNetCore.Mvc;
using adisyon.Data;


namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly KitchenDAO _kitchenDAO;

        public KitchenController(KitchenDAO kitchenDAO)
        {
            _kitchenDAO = kitchenDAO;
        }

        // "Hazırlanıyor" durumundaki siparişleri getirme
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var orders = await _kitchenDAO.GetOrdersAsync();

            if (orders.ToString() == Constants.NoActiveOrders)
            {
                return NotFound(orders);
            }

            return Ok(orders);
        }

        // Siparişin durumunu "Hazırlandı" olarak güncelleme
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id)
        {
            var result = await _kitchenDAO.UpdateOrderStatusAsync(id);

            if (result == Constants.NoActiveOrders)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}