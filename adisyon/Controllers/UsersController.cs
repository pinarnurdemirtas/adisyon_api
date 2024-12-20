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
                return BadRequest(Constants.InvalidLogin);
            }

            var user = await _userDao.GetUserByUsername(loginUser.Username);
            if (user == null)
            {
                return Unauthorized(Constants.UserNotFound);
            }
            if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                return Unauthorized(Constants.UserNotFound);
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
                return BadRequest(Constants.InvalidRegister);
            }
            var existingUser = await _userDao.GetUserByUsername(registerUser.Username);
            if (existingUser != null)
            {
                return BadRequest(Constants.UsernameUsed);
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

            await _userDao.AddUser(newUser);
            await _userDao.SaveChanges();

            return Ok(Constants.UserCreated);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userDao.GetUserById(id);
            if (user == null)
            {
                return NotFound(Constants.UserNotFound);
            }

            await _userDao.DeleteUser(user);
            await _userDao.SaveChanges();

            return Ok(Constants.UserDeleted);
        }
    }
}
