using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace adisyon.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SiparisController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public SiparisController(AdisyonDbContext context)
    {
        _context = context;
    }

    
    // GET: api/Siparis
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Siparis>>> GetSiparis()
    {
        return await _context.Orders.ToListAsync();
    }
    
    
    // GET: api/siparis/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Siparis>> GetSiparis(int id)
    {
        var siparis = await _context.Orders.FindAsync(id);
        if (siparis == null) return NotFound();
        return Ok(siparis);
    }

    
    // POST: api/siparis
    [HttpPost]
    public async Task<ActionResult<Siparis>> PostSiparis(Siparis siparis)
    {
        _context.Orders.Add(siparis);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSiparis), new { id = siparis.Id }, siparis);
    }

}
