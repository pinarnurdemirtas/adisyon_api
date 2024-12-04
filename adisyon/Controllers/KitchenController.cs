using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
namespace adisyon.Controller;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "mutfak")]

public class KitchenController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public KitchenController(AdisyonDbContext context)
    {
        _context = context;
    }
    
    
    // Hazırlanan siparişleri listeleme
    [HttpGet("list")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders.ToListAsync();
        return Ok(orders);
    }

    
    // Helper: Ürün fiyatını bulma
    private async Task<decimal> GetProductPrice(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        return product?.Price ?? 0;
    } 
    
    
    // Sipariş durumunu 'Hazırlandı' olarak güncelleme
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound("Order not found.");
        order.Status = "hazırlandı";
        var productPrice = await GetProductPrice(order.Product_id);
        var orderCash = new OrderCash()
        {
            Order_id = order.Order_id,
            Product_id = order.Product_id,
            Quantity = order.Quantity,
            Product_price = productPrice,
            Total_price = productPrice * order.Quantity,
            Status = "hazırlandı",
            Order_date = order.Order_date,
            table_number = order.Table_number 
        }; 
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _context.Order_cash.AddAsync(orderCash);
                
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        } 
        return Ok(new { message = "Order status updated and moved to OrderCash.", orderCash });
        }
}