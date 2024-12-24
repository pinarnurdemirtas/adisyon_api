using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "mutfak")]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenDAO _kitchenDAO;

        public KitchenController(IKitchenDAO kitchenDAO)
        {
            _kitchenDAO = kitchenDAO;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetPreparingOrders()
        {
            var orders = await _kitchenDAO.GetOrdersByStatusAsync("Hazırlanıyor");
            if (orders == null || orders.Count == 0)
            {
                return NotFound(Message.OrderNotFound);
            }
            return Ok(orders);
        }

        [HttpPatch("updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] int orderId)
        {
            var order = await _kitchenDAO.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound(Message.OrderNotFound);
            }

            var product = await _kitchenDAO.GetProductByIdAsync(order.Product_id);
            if (product == null)
            {
                return NotFound(Message.ProductsNotFound);
            }

            order.Status = "Hazırlandı";
            await _kitchenDAO.UpdateOrderAsync(order);

            var orderCash = new OrderCash
            {
                Order_id = order.Order_id,
                Product_id = order.Product_id,
                Quantity = order.Quantity,
                Product_price = product.Price,
                Total_price = order.Quantity * product.Price,
                Order_date = order.Order_date,
                Table_number = order.Table_number,
                Status = order.Status,
            };

            await _kitchenDAO.AddOrderCashAsync(orderCash);
            await _kitchenDAO.SaveChangesAsync();

            return Ok(orderCash);
        }
    }
}