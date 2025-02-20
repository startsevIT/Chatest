using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage.Repos;

// Репозиторий для работы с чатами
public class ChatRepo : IChatRepo
{
    // Создание нового чата
    public async Task CreateAsync(CreateChatDTO dto, Guid userId)
    {
        using SQLiteDbContext db = new();

        // Поиск пользователя по ID, если не найден — выбрасываем исключение
        User user = await db.Users.FindAsync(userId) ?? throw new Exception("Not found User");

        // Маппинг DTO в сущность чата и добавление в базу данных
        await db.Chats.AddAsync(dto.Map(user));
        await db.SaveChangesAsync();
    }

    // Чтение чата по ID и добавление пользователя, если его там нет
    public async Task<ReadChatDTO> ReadAndLinkAsync(Guid id, Guid userId)
    {
        using SQLiteDbContext db = new();

        // Поиск чата с загрузкой связанных сообщений и пользователей
        Chat chat = await db.Chats
            .Include(x => x.Messages) // Загружаем связанные сообщения
            .Include(x => x.Users) // Загружаем пользователей чата
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Not found chat"); // Если чат не найден, выбрасываем исключение

        // Проверяем, есть ли пользователь в чате
        if (chat.Users.Find(x => x.Id == userId) == null)
        {
            // Если пользователя нет, находим его в базе
            User user = await db.Users.FindAsync(userId) ?? throw new Exception("User not found");

            // Добавляем пользователя в чат и сохраняем изменения
            chat.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Возвращаем DTO чата с его сообщениями и пользователями
        return chat.Map(chat.Messages, chat.Users);
    }

    public async Task<bool> CheckUserAsync(Guid id, Guid userId)
    {
        using SQLiteDbContext db = new();

        Chat chat = await db.Chats
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Not found chat");

        User user = await db.Users.FindAsync(userId) 
            ?? throw new Exception("Not found user");

        if (!chat.Users.Contains(user))
            return false;

        return true;
    }
}
