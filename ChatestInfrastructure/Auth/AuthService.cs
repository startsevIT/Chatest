using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatestInfrastructure.Auth;

public class AuthService
{
    const string SECRET = "hello_hello_hello_hello_secret_key";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new(Encoding.UTF8.GetBytes(SECRET));
    }

    public static string CreateToken(List<Claim> claims)
    {
        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
                signingCredentials: new(GetSymmetricSecurityKey(), "HS256"))
            );
    }

    public static string HashPassword(string password)
    {
        byte[] hash = new Rfc2898DeriveBytes(password, 0).GetBytes(20);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string hashPassword)
    {
        return HashPassword(password) == hashPassword;
    }
}
