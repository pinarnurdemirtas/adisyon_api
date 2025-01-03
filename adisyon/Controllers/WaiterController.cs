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
            var products = await _waiterDAO.GetAllProducts();
            return Ok(products);
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized(Message.UserIdNotFound);
            }

            var product = await _waiterDAO.GetProductById(order.Product_id);
            if (product == null)
            {
                return BadRequest(Message.ProductsNotFound);
            }

            var table = await _waiterDAO.GetTableByNumber(order.Table_number);
            if (table == null)
            {
                return BadRequest(Message.TableNotFound);
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

            await _waiterDAO.AddOrder(newOrder);
            table.Table_status = "Dolu";
            await _waiterDAO.UpdateTable(table);
    
            return Ok(newOrder);
        }

        [HttpGet("waiterTables")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized(Message.UserIdNotFound);
            }

            var userOrders = await _waiterDAO.GetOrdersByUserId(userId);
            return Ok(userOrders);
        }
    }
}
