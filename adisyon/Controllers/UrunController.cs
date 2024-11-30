using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace adisyon.Controllers;

    
[Route("api/[controller]")]
[ApiController]
public class UrunController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public UrunController(AdisyonDbContext context)
    {
        _context = context;
    }

    
    // GET: api/urun
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Urun>>> GetUrun()
    {
        return await _context.Products.ToListAsync();
    }

    
    // GET: api/urun/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Urun>> GetUrun(int id)
    {
        var urun = await _context.Products.FindAsync(id);
        if (urun == null) return NotFound();
        return Ok (urun);
    }

    
    // POST: api/urun
    [HttpPost]
    public async Task<ActionResult<Urun>> PostUrun(Urun urun)
    {
        _context.Products.Add(urun);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUrun), new { id = urun.Id }, urun);
    }

    
    // PUT: api/urun/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUrun(int id, Urun urun)
    {
        if (id != urun.Id) return BadRequest();
        _context.Entry(urun).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    
    // DELETE: api/urun/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrun(int id)
    {
        var urun = await _context.Products.FindAsync(id);
        if (urun == null) return NotFound();
        _context.Products.Remove(urun);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
