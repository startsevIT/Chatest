using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatestInfrastructure.Auth;

// Сервис для работы с аутентификацией и JWT-токенами
public class AuthService
{
    // Секретный ключ для подписи JWT (лучше вывести его в .env или другой секретный файл)
    const string SECRET = "hello_hello_hello_hello_secret_key";

    // Получение симметричного ключа для подписи токена
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new(Encoding.UTF8.GetBytes(SECRET));
    }

    // Создание параметров валидации JWT-токена
    public static TokenValidationParameters BuildJwtTokenParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = false, // Не проверяем издателя
            ValidateAudience = false, // Не проверяем получателя
            IssuerSigningKey = GetSymmetricSecurityKey(), // Ключ для проверки подписи
            ValidateIssuerSigningKey = true, // Включаем проверку подписи
            ValidateLifetime = true, // Проверяем срок действия токена
        };
    }

    // Создание JWT-токена
    public static string CreateToken(List<Claim> claims)
    {
        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                claims: claims, // Передаем список клеймов (данных о пользователе)
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)), // Токен действителен 100 минут
                signingCredentials: new(GetSymmetricSecurityKey(), "HS256")) // Подписываем токен алгоритмом HS256
        );
    }

    // Хеширование пароля
    public static string HashPassword(string password)
    {
        byte[] hash = new Rfc2898DeriveBytes(password, 0).GetBytes(20); // Хеширование без соли (небезопасно)
        return Convert.ToBase64String(hash);
    }

    // Проверка пароля
    public static bool VerifyPassword(string password, string hashPassword)
    {
        return HashPassword(password) == hashPassword; // Сравнение хешей
    }
}
