using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using Microsoft.AspNetCore.Authorization;


namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "kasa")]
    public class CashController : ControllerBase
    {
        private readonly CashDAO _cashDAO;

        public CashController(CashDAO cashDAO)
        {
            _cashDAO = cashDAO;
        }
        
        // Belirtilen masa numarasına ait "Hazırlandı" durumundaki siparişleri getirme
        [HttpGet("orders/{tableNumber}")]
        public async Task<IActionResult> GetOrdersByTableAsync(int tableNumber)
        {
            var orders = await _cashDAO.GetOrdersByTableAsync(tableNumber);

            if (orders.ToString() == Constants.TableEmpty)
            {
                return NotFound(orders); 
            }

            return Ok(orders); 
        }
        
        // Belirtilen masa numarasındaki siparişlerin durumunu "Ödendi" olarak güncelleme
        [HttpPut("mark-paid/{tableNumber}")]
        public async Task<IActionResult> MarkOrdersAsPaidAsync(int tableNumber)
        {
            var result = await _cashDAO.MarkOrdersAsPaidAsync(tableNumber);

            if (result == Constants.TableEmpty)
            {
                return NotFound(result); // Masada güncellenebilir sipariş yoksa 404 döneriz
            }

            return Ok(result); // Siparişler "Ödendi" olarak işaretlendiyse, 200 ile döneriz
        }

    }
}
