using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage.Repos;

// Репозиторий для работы с сообщениями
public class MessageRepo : IMessageRepo
{
    // Создание нового сообщения
    public async Task CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId)
    public async Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId)
    {
        using SQLiteDbContext db = new();

        // Поиск пользователя, если не найден — выбрасываем исключение
        User user = await db.Users.FindAsync(userId) ?? throw new Exception("Not found User");

        // Поиск чата, если не найден — выбрасываем исключение
        Chat chat = await db.Chats.FindAsync(chatId) ?? throw new Exception("Not found Chat");

        Message message = dto.Map(chat, user);

        await db.Messages.AddAsync(message);
        // Маппинг DTO в сущность сообщения и добавление в базу данных
        await db.Messages.AddAsync(dto.Map(chat, user));
        await db.SaveChangesAsync();
        return message.Id;
    }

    // Чтение сообщения по ID
    public async Task<ReadMessageDTO> ReadAsync(Guid id)
    {
        using SQLiteDbContext db = new();

        // Поиск сообщения с подгрузкой информации о пользователе
        Message message = await db.Messages
            .Include(x => x.User) // Загружаем данные о авторе сообщения
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Not found message"); // Если сообщение не найдено, выбрасываем исключение

        // Возвращаем DTO сообщения с данными о пользователе
        return message.Map(message.User);
    }
}
