using Microsoft.IdentityModel.Tokens;

namespace NewsApi.Tokens;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan AccessValidFor { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
}