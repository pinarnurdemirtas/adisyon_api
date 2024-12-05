using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
namespace adisyon.Controller;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "garson")]

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

        // Ürün ID'sine göre Product tablosundan ürün alınması
        var product = await _context.Products.FindAsync(order.Product_id);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Product_name null kontrolü yapılır ve null değilse atanır
        order.Product_name = product.Name ?? "Unknown";  // Eğer product.Name null ise "Unknown" kullanılır.

        order.Status = "hazırlanıyor";
        order.Order_date = DateTime.Now;

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Order created successfully.", order });
    }

}