using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using NewsApi.Models;

namespace NewsApi.Tokens;

public class TokenGenerator : ITokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public TokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim("sub", user.Username),
            new Claim("id", user.Id.ToString()),
            new Claim("role", user.Role!.Name!)
        };
        
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now + _jwtOptions.AccessValidFor,
            signingCredentials: _jwtOptions.SigningCredentials
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}