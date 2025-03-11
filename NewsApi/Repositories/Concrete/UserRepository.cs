using Microsoft.EntityFrameworkCore;
using NewsApi.Data;
using NewsApi.Helpers;
using NewsApi.Models;
using NewsApi.Repositories.Abstract;

namespace NewsApi.Repositories.Concrete;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<User> RegisterUserAsync(User user)
    {
        user.Salt = Guid.NewGuid().ToString();
        user.Password = Hasher.HashPassword($"{user.Password}{user.Salt}");
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User> LoginUserAsync(string username, string password)
    {
        var loggedInUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        if (loggedInUser is null)
        {
            throw new KeyNotFoundException("Invalid username or password.");
        }
        
        var hashedPassword = Hasher.HashPassword($"{password}{loggedInUser.Salt}");
        if (hashedPassword != loggedInUser.Password)
        {
            throw new KeyNotFoundException("Invalid password.");
        }
        
        return loggedInUser;
    }
}