using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class MessageMap
{
    // Метод для преобразования CreateMessageDTO в сущность Message
    public static Message Map(this CreateMessageDTO dto, Chat chat, User user)
    {
        return new()
        {
            Id = Guid.NewGuid(), // Генерируем уникальный идентификатор для сообщения
            Text = dto.Text, // Текст сообщения из DTO
            Chat = chat, // Привязываем чат к сообщению
            User = user, // Привязываем пользователя к сообщению
            DateTime = DateTime.Now // Устанавливаем текущую дату и время для сообщения
        };
    }

    // Метод для преобразования сущности Message в ReadMessageDTO
    public static ReadMessageDTO Map(this Message message, User user)
    {
        return new ReadMessageDTO(message.Text, user.NickName, message.DateTime); // Возвращаем DTO с текстом, никнеймом пользователя и временем сообщения
    }
}
