using adisyon.Data;
using adisyon.Helpers;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;  // BCrypt kütüphanesini dahil etmelisiniz

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AdisyonDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public LoginController(AdisyonDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        // Kullanıcı giriş işlemi
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginUser)
        {
            // Kullanıcıyı veritabanında buluyoruz
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.username == loginUser.username);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            // Şifreyi doğrulama
            if (!BCrypt.Net.BCrypt.Verify(loginUser.password, user.password))
                return Unauthorized(new { message = "Invalid username or password." });

            // JWT oluşturma
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                User = new
                {
                    user.username,
                    user.role,
                    user.Id
                }
            });
        }
    }
}
