using adisyon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "kasa")]

    public class CashController : ControllerBase
    {
        private readonly CashDAO _cashDAO;
        public CashController(CashDAO cashDAO)
        {
            _cashDAO = cashDAO;
        }

        [HttpGet("ordersFromOccupiedTables")]
        public async Task<IActionResult> GetOrdersFromOccupiedTables()
        {
            // Sadece "Dolu" masalardaki tüm siparişleri getir
            var orders = await _cashDAO.GetOrdersFromFullTables();
            if (orders == null || !orders.Any())
            {
                return NotFound(Message.FullTableNotFound);
            }

            return Ok(orders);
        }
        
        [HttpPut("markOrdersAsPaidByTable/{tableNumber}")]
        public async Task<IActionResult> MarkOrdersAsPaidByTable(int tableNumber)
        {
            // Masa numarasına göre siparişleri getir
            var orders = await _cashDAO.GetOrdersByTableNumber(tableNumber);
            if (orders == null || !orders.Any())
            {
                return NotFound(Message.NoReadyOrders);
            }

            // Siparişlerin durumunu "Ödendi" olarak güncelle
            foreach (var order in orders)
            {
                order.Status = "Ödendi";
            }

            await _cashDAO.UpdateOrders(orders);

            // Masanın durumunu "Boş" olarak güncelle
            await _cashDAO.UpdateTableStatus(tableNumber, "Boş");

            await _cashDAO.SaveChanges();
            
            return Ok(Message.OrdersMarkedAsPaid);
        }


        
    }
}
