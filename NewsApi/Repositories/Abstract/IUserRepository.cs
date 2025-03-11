using NewsApi.Models;

namespace NewsApi.Repositories.Abstract;

public interface IUserRepository
{
    Task<User> RegisterUserAsync(User user);
    Task<User> LoginUserAsync(string username, string password);
}