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
        private readonly ICashDAO _cashDAO;

        public CashController(ICashDAO cashDAO)
        {
            _cashDAO = cashDAO;
        }

        [HttpGet("fullTables")]
        public async Task<IActionResult> GetOrdersFromOccupiedTables()
        {
            var orders = await _cashDAO.GetOrdersFromFullTables();
            if (orders == null || !orders.Any())
            {
                return NotFound(Message.FullTableNotFound);
            }

            return Ok(orders);
        }

        [HttpPatch("markOrdersAsPaidByTable/{tableNumber}")]
        public async Task<IActionResult> MarkOrdersAsPaidByTable(int tableNumber)
        {
            var orders = await _cashDAO.GetOrdersByTableNumber(tableNumber);
            if (orders == null || !orders.Any())
            {
                return NotFound(Message.NoReadyOrders);
            }

            foreach (var order in orders)
            {
                order.Status = "Ödendi";
            }

            await _cashDAO.UpdateOrders(orders);
            await _cashDAO.UpdateTableStatus(tableNumber, "Boş");
            await _cashDAO.SaveChanges();

            return Ok(Message.OrdersMarkedAsPaid);
        }

        [HttpGet("ordersByDate")]
        public async Task<IActionResult> GetPaidOrdersByDate([FromQuery] DateTime date)
        {
            var orders = await _cashDAO.GetPaidOrdersByDate(date);
            if (orders == null || !orders.Any())
            {
                return NotFound(Message.OrderNotFound);
            }

            return Ok(orders);
        }
    }
}