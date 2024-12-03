using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using adisyon.Data;
namespace adisyon.Controller;


[ApiController]
[Route("api/[controller]")]
public class CashController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public CashController(AdisyonDbContext context)
    {
        _context = context;
    }
    
    
    // Belirtilen masa numarasına ait tüm siparişleri bulma
    [HttpGet("list-by-table/{tableNumber}")]
    public async Task<IActionResult> GetOrdersByTable(int tableNumber)
    {
        var orderCashList = await _context.Order_cash
            .Where(oc => oc.table_number == tableNumber)
            .ToListAsync();
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
        var ordersToUpdate = await _context.Order_cash
            .Where(oc => oc.table_number == tableNumber && oc.Status != "ödendi")
            .ToListAsync();
        if (ordersToUpdate == null || !ordersToUpdate.Any())
        {
            return NotFound("No orders found for this table or all orders are already marked as paid.");
        }
        foreach (var order in ordersToUpdate)
        {
            order.Status = "ödendi";
        }
        await _context.SaveChangesAsync();
        return Ok(new { message = "Orders marked as paid.", ordersToUpdate });
    }
}