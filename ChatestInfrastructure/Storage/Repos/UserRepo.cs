using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using ChatestInfrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatestInfrastructure.Storage.Repos;

// Репозиторий для работы с пользователями
public class UserRepo : IUserRepo
{
    // Авторизация пользователя (логин)
    public async Task<string> LoginAsync(LoginUserDTO dto)
    {
        using SQLiteDbContext db = new();

        // Поиск пользователя по логину
        User user = await db.Users
            .FirstOrDefaultAsync(x => x.Login == dto.Login)
            ?? throw new Exception("Not found User");

        // Проверка пароля
        if (!AuthService.VerifyPassword(dto.Password, user.Password))
            throw new Exception("Not correct password");

        // Генерация JWT-токена
        return AuthService.CreateToken([
                new Claim("id", user.Id.ToString()) // Добавляем ID пользователя в токен
            ]);
    }

    // Получение информации о пользователе по ID
    public async Task<ReadForAccountUserDTO> ReadAsync(Guid id)
    {
        using SQLiteDbContext db = new();

        // Поиск пользователя с загрузкой связанных чатов
        User user = await db.Users
            .Include(x => x.Chats) // Загружаем связанные чаты
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Not found user");

        // Возвращаем DTO пользователя с его чатами
        return user.Map(user.Chats);
    }

    // Регистрация нового пользователя
    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        using SQLiteDbContext db = new();

        // Проверка существования пользователя с таким же логином
        User? tryUser = await db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login);
        if (tryUser != null)
            throw new Exception("User already exist");

        // Хеширование пароля перед сохранением
        var hashed = new RegisterUserDTO(dto.Login, AuthService.HashPassword(dto.Password), dto.NickName);

        // Маппинг DTO в сущность пользователя и добавление в базу данных
        await db.Users.AddAsync(hashed.Map());
        await db.SaveChangesAsync();
    }
}
