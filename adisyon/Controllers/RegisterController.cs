using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
namespace adisyon.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    public RegisterController(AdisyonDbContext context)
    {
        _context = context;
    }

    // Kullanıcı kaydetme (Register)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Kisi kisi)
    {
        // Kullanıcı adı kontrolü
        if (_context.Set<Kisi>().Any(k => k.username == kisi.username))
        {
            return BadRequest("Kullanıcı adı zaten kullanılıyor.");
        }

        // Şifreyi hash'le
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(kisi.password);
        kisi.password = passwordHash;

        // Varsayılan role (örneğin, "User") atanabilir
        kisi.role = kisi.role ?? "garson";

        // Kullanıcıyı veritabanına ekle
        _context.Set<Kisi>().Add(kisi);
        await _context.SaveChangesAsync();

        return Ok("Kullanıcı başarıyla kaydedildi.");
    }

    // Kullanıcı silme (Delete)
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Kullanıcıyı ID ile bul
        var kisi = await _context.Set<Kisi>().FindAsync(id);

        if (kisi == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        // Kullanıcıyı sil
        _context.Set<Kisi>().Remove(kisi);
        await _context.SaveChangesAsync();

        return Ok("Kullanıcı başarıyla silindi.");
    }
}
