using NewsApi.Models;

namespace NewsApi.Tokens;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}