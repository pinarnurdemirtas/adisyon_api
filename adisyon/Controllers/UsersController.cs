using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDAO _userDao;
        private readonly Security _security;

        public UsersController(UserDAO userDao, Security security)
        {
            _userDao = userDao;
            _security = security;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest(new { message = "Invalid request. User information is missing." });
            }

            var user = await _userDao.GetUserByUsernameAsync(loginUser.Username);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = _security.CreateToken(user);

            return Ok(new
            {
                Token = token,
                User = new { user.Username, user.Role, user.Id }
            });
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users registerUser)
        {
            if (registerUser == null)
            {
                return BadRequest(new { message = "Invalid request. User data is missing." });
            }

            var existingUser = await _userDao.GetUserByUsernameAsync(registerUser.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username is already taken." });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

            var newUser = new Users
            {
                Id = registerUser.Id,
                Username = registerUser.Username,
                Password = hashedPassword,
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Gender = registerUser.Gender,
                Mail = registerUser.Mail,
                Role = registerUser.Role,
            };

            await _userDao.AddUserAsync(newUser);
            await _userDao.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userDao.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            await _userDao.DeleteUserAsync(user);
            await _userDao.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }
    }
}
