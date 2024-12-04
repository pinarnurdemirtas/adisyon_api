using System.Security.Claims;
using adisyon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace adisyon.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AdisyonDbContext _context;
    private readonly Security _security; // No need for JwtSettings anymore

    public UsersController(AdisyonDbContext context, Security security)
    {
        _context = context;
        _security = security; // Initialize Security class
    }

    // Kullanıcı giriş işlemi
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login loginUser)
    {
        // Kullanıcıyı veritabanında buluyoruz
        var user = await _context.Users.SingleOrDefaultAsync(u =>
            u.Username == loginUser.username);

        if (user == null)
            return Unauthorized(new { message = "Invalid username or password." });

        // Şifreyi doğrulama
        if (!BCrypt.Net.BCrypt.Verify(loginUser.password, user.Password))
            return Unauthorized(new { message = "Invalid username or password." });

        // JWT oluşturma, Security sınıfındaki CreateToken metodunu kullanarak
        var token = _security.CreateToken(user); // No need to pass _jwtSettings

        return Ok(new
        {
            Token = token,
            User = new
            {
                user.Username,
                user.Role,
                user.Id
            }
        });
    }
    
    
    
    // Kullanıcı kaydetme (Register)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Models.Users users)
    {
        // Kullanıcı adı kontrolü
        if (_context.Set<Models.Users>().Any(k => k.Username == users.Username))
        {
            return BadRequest("Kullanıcı adı zaten kullanılıyor.");
        }

        // Şifreyi hash'le
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(users.Password);
        users.Password = passwordHash;

        // Varsayılan role (örneğin, "User") atanabilir
        users.Role = users.Role ?? "garson";

        // Kullanıcıyı veritabanına ekle
        _context.Set<Models.Users>().Add(users);
        await _context.SaveChangesAsync();

        return Ok("Kullanıcı başarıyla kaydedildi.");
    }

    // Kullanıcı silme (Delete)
    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini al
        var idStr = id.ToString(); // id'yi string'e çevir

        // Konsola yazdırarak her iki değeri kontrol edelim
        Console.WriteLine($"userId: {userId}");
        Console.WriteLine($"idStr: {idStr}");
    

        // Ekstra yetki kontrolü
        if (userId != id.ToString()) 
        {
            return Unauthorized("Bu işlemi yapmaya yetkiniz yok.");
        }

        var kisi = await _context.Set<Models.Users>().FindAsync(id);

        if (kisi == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        _context.Set<Models.Users>().Remove(kisi);
        await _context.SaveChangesAsync();

        return Ok("Kullanıcı başarıyla silindi.");
    }
}