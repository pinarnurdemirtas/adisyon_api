using System.Security.Claims;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "garson")]
    public class WaiterController : ControllerBase
    {
        private readonly IWaiterDAO _waiterDAO;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WaiterController(IWaiterDAO waiterDAO, IHttpContextAccessor httpContextAccessor)
        {
            _waiterDAO = waiterDAO;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdString, out int userId) ? userId : 0;
        }

        [HttpGet("menu")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _waiterDAO.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized("User ID not found.");
            }

            var product = await _waiterDAO.GetProductByIdAsync(order.Product_id);
            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            var table = await _waiterDAO.GetTableByNumberAsync(order.Table_number);
            if (table == null)
            {
                return BadRequest("Table not found.");
            }

            var newOrder = new Orders
            {
                Table_number = order.Table_number,
                Product_id = order.Product_id,
                Product_name = product.Name,
                Quantity = order.Quantity,
                Status = "Hazırlanıyor",
                Order_date = DateTime.Now,
                User_id = userId
            };

            await _waiterDAO.AddOrderAsync(newOrder);
            table.Table_status = "Dolu";
            await _waiterDAO.UpdateTableAsync(table);

            return Ok(newOrder);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized("User ID not found.");
            }

            var userOrders = await _waiterDAO.GetOrdersByUserIdAsync(userId);
            return Ok(userOrders);
        }
    }
}
