using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using adisyon.Data;


namespace adisyon.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "kasa")]
    public class CashController : ControllerBase
    {
        private readonly CashRepository _cashRepository;

        public CashController(CashRepository cashRepository)
        {
            _cashRepository = cashRepository;
        }

        // Belirtilen masa numarasına ait tüm siparişleri bulma
        [HttpGet("list-by-table/{tableNumber}")]
        public async Task<IActionResult> GetOrdersByTable(int tableNumber)
        {
            var orderCashList = await _cashRepository.GetOrdersByTableAsync(tableNumber);
            
            if (orderCashList == null || !orderCashList.Any())
            {
                return NotFound("No orders found for this table.");
            }
            
            return Ok(orderCashList);
        }

        // Siparişlerin durumunu "ödendi" olarak güncelleme
        [HttpPut("mark-paid/{tableNumber}")]
        public async Task<IActionResult> MarkOrdersAsPaid(int tableNumber)
        {
            var success = await _cashRepository.MarkOrdersAsPaidAsync(tableNumber);
            
            if (!success)
            {
                return NotFound("No orders found for this table or all orders are already marked as paid.");
            }
            
            return Ok(new { message = "Orders marked as paid." });
        }
    }
}