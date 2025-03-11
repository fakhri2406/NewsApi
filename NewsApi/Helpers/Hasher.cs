using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace NewsApi.Helpers;

public static class Hasher
{
    public static string HashPassword(string password)
    {
        SHA256 sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}