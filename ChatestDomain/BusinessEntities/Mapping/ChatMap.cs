using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class ChatMap
{
    // Метод для преобразования CreateChatDTO в сущность Chat
    public static Chat Map(this CreateChatDTO dto, User user)
    {
        return new()
        {
            Id = Guid.NewGuid(), // Генерируем уникальный идентификатор для чата
            Name = dto.Name, // Устанавливаем имя чата из DTO
            Messages = [], // Инициализируем список сообщений (пока пустой)
            Users = [user], // Добавляем пользователя в список участников чата
        };
    }

    // Метод для преобразования сущности Chat в ReadChatDTO
    public static ReadChatDTO Map(this Chat chat, List<Message> messages, List<User> users)
    {
        return new ReadChatDTO(
            chat.Name, // Название чата
            [..messages.Select(x => x.Map(x.User))], // Преобразуем каждое сообщение в ReadMessageDTO
            [..users.Select(x => x.Map())] // Преобразуем каждого пользователя в ReadForChatUserDTO
        );
    }

    // Метод для преобразования сущности Chat в ReadForListChatDTO
    public static ReadForListChatDTO Map(this Chat chat)
    {
        return new ReadForListChatDTO(chat.Id, chat.Name); // Возвращаем DTO с идентификатором и именем чата
    }
}
