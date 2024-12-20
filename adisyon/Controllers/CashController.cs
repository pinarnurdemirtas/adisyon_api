using adisyon.Data;
using adisyon.Models;
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

        [HttpPost("markAsPaid")]
        public async Task<IActionResult> MarkOrdersAsPaid([FromBody] int tableNumber)
        {
            // "Hazırlandı" durumundaki siparişleri getir
            var readyOrders = await _cashDAO.GetReadyOrdersByTableAsync(tableNumber);
            if (readyOrders == null || !readyOrders.Any())
            {
                return NotFound(Constants.NoReadyOrders);
            }

            // Siparişleri güncelle
            foreach (var order in readyOrders)
            {
                order.Status = "Ödendi";
            }
            await _cashDAO.UpdateOrdersAsync(readyOrders);

            // Orders tablosundaki siparişleri güncelle
            var orderIds = readyOrders.Select(o => o.Order_id).ToList();
            var ordersInOrderTable = await _cashDAO.GetOrdersByIdsAsync(orderIds);

            foreach (var order in ordersInOrderTable)
            {
                order.Status = "Ödendi";
            }
            await _cashDAO.UpdateOrdersInOrdersTableAsync(ordersInOrderTable);

            // Masayı güncelle
            var table = await _cashDAO.GetTableByNumberAsync(tableNumber);
            if (table != null)
            {
                table.Table_status = "Boş";
                await _cashDAO.UpdateTableAsync(table);
            }

            // Değişiklikleri kaydet
            await _cashDAO.SaveChangesAsync();

            return Ok(Constants.OrdersMarkedAsPaid);
        }

        [HttpGet("readyOrders/{tableNumber}")]
        public async Task<IActionResult> GetReadyOrders(int tableNumber)
        {
            var orders = await _cashDAO.GetReadyOrdersByTableAsync(tableNumber);
            if (orders == null || !orders.Any())
            {
                return NotFound(Constants.OrderNotFound);
            }
            
            return Ok(orders);
        }
    }
}
