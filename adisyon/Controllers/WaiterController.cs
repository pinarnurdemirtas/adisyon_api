using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using adisyon.Models;
namespace adisyon.Controller;


[ApiController]
[Route("api/[controller]")]
public class WaiterController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public WaiterController(AdisyonDbContext context)
    {
        _context = context;
    }

        
    // Yeni sipariş oluşturma
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] Orders order)
    {
        if (order == null) return BadRequest("Order data is missing.");
        order.Status = "hazırlanıyor";
        order.Order_date = DateTime.Now;
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Order created successfully.", order });
    }
}