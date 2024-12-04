using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.DataAccess;

public class UserDAO
{
    AdisyonDbContext _context;

    public UserDAO(AdisyonDbContext context)
    {
        _context = context;
    }

    public List<Users> GetUsers()
    {
        return _context.Users.ToList();
    }

    public Users? GetUser(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public ActionResult<Users> Add(Users user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public Users? Update(Users user, int id)
    {
        _context.Users.Update(user);
        return user;
    }

    public Users? DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            return user;
        }
        return null;
    }

    public Users? CheckUser(Users user)
    {
        var result = _context.Users.FirstOrDefault(x => x.Id == user.Id && x.Password == user.Password);
        return result;
    }

    public bool IsAuthorized(string? mail)
    {
        var user = _context.Users.FirstOrDefault(u => u.Mail == mail);
        return user is { Role: "0" };
    }
}