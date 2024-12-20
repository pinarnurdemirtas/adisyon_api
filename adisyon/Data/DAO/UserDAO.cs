using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public class UserDAO
    {
        private readonly AdisyonDbContext _context;

        public UserDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUser(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteUser(Users user)
        {
            _context.Users.Remove(user);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}