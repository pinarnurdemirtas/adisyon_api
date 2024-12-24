using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public interface IUserDAO
    {
        Task<Users> GetUserByUsername(string username);
        Task<Users> GetUserById(int id);
        Task AddUser(Users user);
        Task DeleteUser(Users user);
        Task SaveChanges();
    }
    public class UserDAO : IUserDAO
    {
        private readonly AdisyonDbContext _context;

        public UserDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Kullanıcı adı ile bir kullanıcıyı veritabanından getir
        public async Task<Users> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        // Verilen id ile bir kullanıcıyı getir
        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Yeni bir kullanıcıyı veritabanına ekle
        public async Task AddUser(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        // Verilen kullanıcıyı veritabanından sil
        public async Task DeleteUser(Users user)
        {
            _context.Users.Remove(user);
        }

        // Veritabanındaki yapılan değişiklikleri kaydet
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}