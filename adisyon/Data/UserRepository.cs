using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public class UserRepository
    {
        private readonly AdisyonDbContext _context;

        public UserRepository(AdisyonDbContext context)
        {
            _context = context;
        }

        // Statik olmayan metod
        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteUserAsync(Users user)
        {
            _context.Users.Remove(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}